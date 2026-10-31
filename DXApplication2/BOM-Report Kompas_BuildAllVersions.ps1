# ============================================================
#  BOM-Report Kompas_BuildAllVersions.ps1
#  Сборка плагина и установщиков под диапазон версий CAD-системы.
#
#  Скрипт кладётся в папку проекта (рядом с .csproj и .iss).
#  Проект: BOM-Report Kompas
#  Всё определяется автоматически:
#    .iss           - *_CompilFile_V<N>.iss рядом со скриптом (берётся наибольший номер N)
#    .csproj        - единственный *.csproj рядом со скриптом
#    AssemblyInfo   - Properties\AssemblyInfo.cs
#    .sln           - ближайшее вверх по папкам решение, в котором есть этот .csproj
#    MSBuild        - самая новая Visual Studio (через vswhere)
#    ISCC           - установленный Inno Setup 6
#    Имя проекта, CAD-система - из AssemblyInfo.cs / .csproj
#
#  Примеры запуска:
#    .\"BOM-Report Kompas_BuildAllVersions.ps1"
#    .\"BOM-Report Kompas_BuildAllVersions.ps1" -OutputRoot 'D:\Releases' -VersionStart 24 -VersionEnd 26
#    .\"BOM-Report Kompas_BuildAllVersions.ps1" -DryRun        # только показать найденные настройки
# ============================================================

param(
    # Корневая папка для установщиков. '' = значение по умолчанию из .iss
    [string]$OutputRoot   = '',

    # Диапазон версий CAD-системы (первое число AssemblyVersion)
    [int]$VersionStart    = 19,
    [int]$VersionEnd      = 25,

    [string]$BuildConfig  = 'Debug',

    # Платформа решения. '' = определить по PlatformTarget в настройках проекта (.csproj)
    [string]$BuildPlatform = '',

    # Скрипт Inno Setup. '' = *_CompilFile_V<N>.iss рядом с этим файлом с наибольшим N
    [string]$IssFile      = '',

    # Не возвращать исходный AssemblyInfo.cs после сборки (оставить последнюю версию)
    [switch]$KeepLastVersion,

    # Остановиться при первой ошибке
    [switch]$StopOnError,

    # Только вывести найденные настройки, ничего не собирать
    [switch]$DryRun
)

$ErrorActionPreference = 'Stop'
$ProjectDir = $PSScriptRoot

function Fail([string]$Message) {
    Write-Host "ОШИБКА: $Message" -ForegroundColor Red
    exit 1
}

# ---------- ПОИСК ФАЙЛОВ ПРОЕКТА ----------------------------

$csprojs = @(Get-ChildItem $ProjectDir -Filter *.csproj)
if ($csprojs.Count -ne 1) { Fail "в папке скрипта должен быть ровно один .csproj (найдено: $($csprojs.Count))" }
$ProjectPath = $csprojs[0].FullName

$AssemblyInfoPath = Join-Path $ProjectDir 'Properties\AssemblyInfo.cs'
if (-not (Test-Path $AssemblyInfoPath)) { Fail "не найден $AssemblyInfoPath" }

if ([string]::IsNullOrWhiteSpace($IssFile)) {
    $IssFilePath = Get-ChildItem $ProjectDir -Filter '*_CompilFile_V*.iss' |
        Where-Object { $_.BaseName -match '_CompilFile_V(\d+)$' } |
        Sort-Object { [int]([regex]::Match($_.BaseName, '_CompilFile_V(\d+)$').Groups[1].Value) } -Descending |
        Select-Object -First 1 -ExpandProperty FullName
    if (-not $IssFilePath) { Fail "рядом со скриптом не найден файл *_CompilFile_V<N>.iss" }
} else {
    $IssFilePath = if ([IO.Path]::IsPathRooted($IssFile)) { $IssFile } else { Join-Path $ProjectDir $IssFile }
}
if (-not (Test-Path $IssFilePath)) { Fail "не найден скрипт Inno Setup: $IssFilePath" }

# Решение: ищем вверх по папкам .sln, который ссылается на наш .csproj
$SolutionPath = $null
$projFileName = [regex]::Escape($csprojs[0].Name)
$dir = $ProjectDir
while ($dir -and -not $SolutionPath) {
    $SolutionPath = Get-ChildItem $dir -Filter *.sln -ErrorAction SilentlyContinue |
        Where-Object { (Get-Content $_.FullName -Raw) -match $projFileName } |
        Select-Object -First 1 -ExpandProperty FullName
    $dir = Split-Path $dir -Parent
}
if (-not $SolutionPath) { Fail "не найдено решение (.sln), содержащее $($csprojs[0].Name)" }

# ---------- ПОИСК MSBuild И ISCC ----------------------------

$vswhere = Join-Path ${env:ProgramFiles(x86)} 'Microsoft Visual Studio\Installer\vswhere.exe'
$MSBuild = $null
if (Test-Path $vswhere) {
    $MSBuild = & $vswhere -latest -prerelease -products * -requires Microsoft.Component.MSBuild `
        -find 'MSBuild\**\Bin\MSBuild.exe' | Select-Object -First 1
}
if (-not $MSBuild -or -not (Test-Path $MSBuild)) { Fail 'не найден MSBuild (нужна установленная Visual Studio)' }

$ISCC = $null
foreach ($key in 'HKLM:\SOFTWARE\WOW6432Node\Microsoft\Windows\CurrentVersion\Uninstall\Inno Setup 6_is1',
                 'HKLM:\SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\Inno Setup 6_is1',
                 'HKCU:\SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\Inno Setup 6_is1') {
    $loc = (Get-ItemProperty $key -ErrorAction SilentlyContinue).InstallLocation
    if ($loc -and (Test-Path (Join-Path $loc 'ISCC.exe'))) { $ISCC = Join-Path $loc 'ISCC.exe'; break }
}
if (-not $ISCC) {
    $ISCC = @("${env:ProgramFiles(x86)}\Inno Setup 6\ISCC.exe", "$env:ProgramFiles\Inno Setup 6\ISCC.exe") |
        Where-Object { Test-Path $_ } | Select-Object -First 1
}
if (-not $ISCC) { Fail 'не найден Inno Setup 6 (ISCC.exe)' }

# ---------- ИМЯ ПРОЕКТА И CAD-СИСТЕМА -----------------------

$projContent  = Get-Content $ProjectPath -Raw -Encoding UTF8
$assemblyText = Get-Content $AssemblyInfoPath -Raw -Encoding UTF8

if ($assemblyText -match '\[assembly: AssemblyTitle\("([^"]+)"\)\]') {
    $ProjectName = $Matches[1]
} elseif ($projContent -match '<AssemblyName>\s*([^<]+?)\s*</AssemblyName>') {
    $ProjectName = $Matches[1]
} else {
    $ProjectName = [IO.Path]::GetFileNameWithoutExtension($ProjectPath)
}

$cadSignatures = [ordered]@{
    'Inventor'   = 'Autodesk\.Inventor\.Interop|Inventor\.Interop|COMReference Include="Inventor'
    'SolidWorks' = 'SolidWorks\.Interop|SldWorks'
    'КОМПАС-3D'  = 'Kompas6API|KompasAPI7|Kompas6Constants|KAPITypes|Kompas'
    'AutoCAD'    = 'AcMgd|AcDbMgd|AcCoreMgd|Autodesk\.AutoCAD'
}
$found = @($cadSignatures.Keys | Where-Object { $projContent -match $cadSignatures[$_] })
$CadName = if ($found.Count -eq 0) { 'CAD' } else { $found -join ' / ' }

# ---------- ПЛАТФОРМА ---------------------------------------

# Логика (по настройкам .csproj для выбранной конфигурации):
#   1. Если у платформы проекта по умолчанию (<Platform Condition="'$(Platform)' == ''">)
#      явно задана разрядность <PlatformTarget> x64 / x86 - собираем «родную» для неё платформу
#      (Debug|x64 для x64), а если такой нет - саму платформу по умолчанию.
#   2. Иначе (по умолчанию AnyCPU) - если в проекте есть отдельная конфигурация x64
#      (платформа x64 с PlatformTarget x64), собираем её: CAD-системы 64-битные.
#   3. Иначе - платформу по умолчанию.
#   Платформа проекта переводится в платформу решения через сопоставления в .sln (ActiveCfg).
function Get-ProjectPlatform {
    param([string]$SolutionFile, [string]$ProjectFile, [string]$Config)

    $norm = { param($s) ("$s" -replace '\s', '').ToLowerInvariant() }   # 'Any CPU' == 'AnyCPU'

    [xml]$proj = Get-Content $ProjectFile -Raw -Encoding UTF8
    $groups = @($proj.SelectNodes("//*[local-name()='PropertyGroup']"))
    $child  = { param($node, $name) $n = $node.SelectSingleNode("*[local-name()='$name']"); if ($n) { $n.InnerText.Trim() } else { '' } }

    # Платформа по умолчанию
    $defaultPlatform = 'AnyCPU'
    foreach ($pg in $groups) {
        $v = & $child $pg 'Platform'
        if (-not $pg.GetAttribute('Condition') -and $v) { $defaultPlatform = $v; break }
    }

    # Конфигурации проекта для $Config: платформа -> PlatformTarget
    $configs = [ordered]@{}
    foreach ($pg in $groups) {
        $cond = $pg.GetAttribute('Condition')
        if ($cond -match "'\`$\(Configuration\)\|\`$\(Platform\)'\s*==\s*'([^|']+)\|([^']+)'" -and $Matches[1].Trim() -eq $Config) {
            $target = & $child $pg 'PlatformTarget'
            $configs[$Matches[2].Trim()] = if ($target) { $target } else { 'AnyCPU' }
        }
    }
    if ($configs.Count -eq 0) { throw "в .csproj нет конфигураций '$Config'" }

    $defaultKey = $configs.Keys | Where-Object { (& $norm $_) -eq (& $norm $defaultPlatform) } | Select-Object -First 1
    if (-not $defaultKey) { $defaultKey = @($configs.Keys)[0] }
    $defaultTarget = $configs[$defaultKey]

    # «Родная» платформа для разрядности: имя платформы совпадает с её PlatformTarget
    $native = { param($bits) $configs.Keys | Where-Object { (& $norm $_) -eq $bits -and (& $norm $configs[$_]) -eq $bits } | Select-Object -First 1 }

    if ((& $norm $defaultTarget) -ne 'anycpu') {
        $projPlatform = & $native (& $norm $defaultTarget)
        if (-not $projPlatform) { $projPlatform = $defaultKey }
        $reason = "PlatformTarget $defaultTarget у платформы по умолчанию"
    } elseif ($projPlatform = & $native 'x64') {
        $reason = 'в проекте есть конфигурация x64'
    } else {
        $projPlatform = $defaultKey
        $reason = "платформа проекта по умолчанию ($defaultTarget)"
    }

    # Перевод в платформу решения
    $sln = Get-Content $SolutionFile -Raw -Encoding UTF8
    $projFile = [regex]::Escape([IO.Path]::GetFileName($ProjectFile))
    $slnPlatform = $null
    if ($sln -match "Project\(`"\{[^}]+\}`"\)\s*=\s*`"[^`"]*`",\s*`"[^`"]*$projFile`",\s*`"(\{[^}]+\})`"") {
        $guid = [regex]::Escape($Matches[1])
        $maps = @([regex]::Matches($sln, "(?mi)^\s*$guid\.$([regex]::Escape($Config))\|([^.\r\n]+)\.ActiveCfg\s*=\s*[^|\r\n]+\|(.+?)\s*$"))
        $toProj = @($maps | Where-Object { (& $norm $_.Groups[2].Value) -eq (& $norm $projPlatform) } | ForEach-Object { $_.Groups[1].Value.Trim() })
        # Если на платформу проекта ссылаются несколько платформ решения - берём одноимённую
        $slnPlatform = $toProj | Where-Object { (& $norm $_) -eq (& $norm $projPlatform) } | Select-Object -First 1
        if (-not $slnPlatform) { $slnPlatform = $toProj | Select-Object -First 1 }
        # На платформу проекта ничего не сопоставлено - берём одноимённую платформу решения
        if (-not $slnPlatform) {
            $slnPlatform = $maps | ForEach-Object { $_.Groups[1].Value.Trim() } |
                Where-Object { (& $norm $_) -eq (& $norm $projPlatform) } | Select-Object -First 1
        }
    }
    if (-not $slnPlatform) { throw "в решении нет платформы для '$Config|$projPlatform' проекта" }

    return [PSCustomObject]@{ Solution = $slnPlatform; Project = $projPlatform; Reason = $reason }
}

$platformSource = 'задана вручную'
if ([string]::IsNullOrWhiteSpace($BuildPlatform)) {
    try { $p = Get-ProjectPlatform -SolutionFile $SolutionPath -ProjectFile $ProjectPath -Config $BuildConfig }
    catch { Fail "определение платформы: $_" }
    $BuildPlatform  = $p.Solution
    $platformSource = "из проекта: $($p.Reason) -> платформа проекта $($p.Project)"
}

# ---------- ВЕРСИЯ ------------------------------------------

if ($assemblyText -match '(?m)^\s*\[assembly: AssemblyVersion\("(\d+)\.(\d+\.\d+\.\d+)"\)\]') {
    $OriginalMajor = $Matches[1]
    $MinorVersion  = $Matches[2]
} else {
    Fail 'не удалось прочитать AssemblyVersion из AssemblyInfo.cs'
}

Write-Host "Проект:       $ProjectName ($CadName)"               -ForegroundColor Cyan
Write-Host "  .csproj:    $ProjectPath"                          -ForegroundColor DarkCyan
Write-Host "  .sln:       $SolutionPath"                         -ForegroundColor DarkCyan
Write-Host "  .iss:       $IssFilePath"                          -ForegroundColor DarkCyan
Write-Host "  MSBuild:    $MSBuild"                              -ForegroundColor DarkCyan
Write-Host "  ISCC:       $ISCC"                                 -ForegroundColor DarkCyan
Write-Host "Сборка:       $BuildConfig | $BuildPlatform ($platformSource)" -ForegroundColor Cyan
Write-Host "Версии:       $CadName $VersionStart – $VersionEnd, x.$MinorVersion (сейчас $OriginalMajor.$MinorVersion)" -ForegroundColor Cyan
Write-Host "Результат:    $(if ($OutputRoot) { $OutputRoot } else { 'по умолчанию из .iss' })" -ForegroundColor Cyan

if ($DryRun) { Write-Host "`n-DryRun: сборка не выполнялась." -ForegroundColor Yellow; exit 0 }
if ($VersionStart -gt $VersionEnd) { Fail 'VersionStart больше VersionEnd' }

# ---------- ВСПОМОГАТЕЛЬНЫЕ ФУНКЦИИ ------------------------

# Исходный файл держим в памяти байт-в-байт: без .bak в репозитории и с сохранением кодировки
$originalBytes = [IO.File]::ReadAllBytes($AssemblyInfoPath)
$hasBom = $originalBytes.Length -ge 3 -and $originalBytes[0] -eq 0xEF -and $originalBytes[1] -eq 0xBB -and $originalBytes[2] -eq 0xBF
$encoding = New-Object System.Text.UTF8Encoding($hasBom)

function Set-AssemblyVersion([string]$NewVersion) {
    $content = [IO.File]::ReadAllText($AssemblyInfoPath)
    $content = $content -replace '(?m)^\[assembly: AssemblyVersion\("[^"]+"\)\]',     "[assembly: AssemblyVersion(`"$NewVersion`")]"
    $content = $content -replace '(?m)^\[assembly: AssemblyFileVersion\("[^"]+"\)\]', "[assembly: AssemblyFileVersion(`"$NewVersion`")]"
    [IO.File]::WriteAllText($AssemblyInfoPath, $content, $encoding)
}

function Invoke-Build {
    & $MSBuild $SolutionPath /p:Configuration=$BuildConfig "/p:Platform=$BuildPlatform" /t:Rebuild /v:minimal /nologo
    if ($LASTEXITCODE -ne 0) { throw "MSBuild завершился с кодом $LASTEXITCODE" }
}

function Invoke-InnoSetup {
    $isccArgs = @()
    if ($OutputRoot) { $isccArgs += "/DOutputRoot=$OutputRoot" }
    $isccArgs += $IssFilePath
    $log = & $ISCC @isccArgs 2>&1
    if ($LASTEXITCODE -ne 0) {
        $log | Select-Object -Last 5 | ForEach-Object { Write-Host "    $_" -ForegroundColor DarkRed }
        throw "ISCC завершился с кодом $LASTEXITCODE"
    }
    # Последняя строка вывода ISCC - путь к готовому установщику
    return ($log | Where-Object { "$_" -match '\.exe\s*$' } | Select-Object -Last 1)
}

# ---------- ОСНОВНОЙ ЦИКЛ -----------------------------------

$results = @()
try {
    for ($cadVer = $VersionStart; $cadVer -le $VersionEnd; $cadVer++) {
        $version = "$cadVer.$MinorVersion"
        Write-Host "`n========== $ProjectName для $CadName $cadVer  →  $version ==========" -ForegroundColor Magenta
        try {
            Write-Host '  [1/3] AssemblyInfo.cs ...'
            Set-AssemblyVersion $version

            Write-Host "  [2/3] MSBuild ($BuildConfig | $BuildPlatform) ..."
            Invoke-Build

            Write-Host '  [3/3] Inno Setup ...'
            $setup = Invoke-InnoSetup

            $results += [PSCustomObject][ordered]@{ $CadName = $cadVer; Версия = $version; Статус = 'OK'; Установщик = "$setup".Trim() }
            Write-Host "  Готово: $setup" -ForegroundColor Green
        } catch {
            $results += [PSCustomObject][ordered]@{ $CadName = $cadVer; Версия = $version; Статус = "ОШИБКА: $_"; Установщик = '' }
            Write-Host "  ОШИБКА: $_" -ForegroundColor Red
            if ($StopOnError) { break }
        }
    }
} finally {
    # Возвращаем AssemblyInfo.cs как было - даже при ошибке или Ctrl+C
    if (-not $KeepLastVersion) {
        [IO.File]::WriteAllBytes($AssemblyInfoPath, $originalBytes)
        Write-Host "`nAssemblyInfo.cs восстановлен ($OriginalMajor.$MinorVersion)." -ForegroundColor Cyan
    }
}

# ---------- ИТОГИ -------------------------------------------

Write-Host "`n============ ИТОГИ: $ProjectName ($CadName $VersionStart – $VersionEnd, $BuildConfig | $BuildPlatform) ============" -ForegroundColor Cyan
$results | Format-Table -AutoSize -Wrap

$errors = @($results | Where-Object { $_.Статус -ne 'OK' })
if ($errors.Count) {
    Write-Host "Завершено с ошибками: $($errors.Count) из $($results.Count)." -ForegroundColor Red
    if (-not $KeepLastVersion) {
        Write-Host 'Внимание: в папке сборки остался exe последней собранной версии, а не исходной.' -ForegroundColor Yellow
    }
    exit 1
}
Write-Host "Все версии $CadName собраны успешно." -ForegroundColor Green
if (-not $KeepLastVersion) {
    Write-Host 'Внимание: в папке сборки остался exe последней собранной версии - пересоберите решение перед отладкой.' -ForegroundColor Yellow
}
exit 0
