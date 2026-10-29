# ============================================================
#  Build-AllVersions.ps1
#  Автоматическая сборка DXF-Auto Inventor для Inventor 20–26
# ============================================================

# ---------- НАСТРОЙКИ (измените под себя) -------------------

$SolutionPath   = 'C:\Users\ACK\Documents\GitHub\BOM-Report-2\BOM Report 2.sln'
$AssemblyInfoPath = 'C:\Users\ACK\Documents\GitHub\BOM-Report-2\DXApplication2\Properties\AssemblyInfo.cs'
$IssFilePath    = 'C:\Users\ACK\Documents\GitHub\BOM-Report-2\DXApplication2\BOMREport_reliaseScript_WorkPCAstana.iss'

# Путь к MSBuild (Visual Studio 2022 Enterprise)
$MSBuild = 'C:\Program Files\Microsoft Visual Studio\2022\Enterprise\MSBuild\Current\Bin\MSBuild.exe'

# Путь к компилятору Inno Setup
$ISCC = 'C:\Program Files (x86)\Inno Setup 6\ISCC.exe'

# Диапазон версий Inventor
$VersionStart = 19
$VersionEnd   = 25

# Конфигурация сборки
$BuildConfig    = 'Debug'
$BuildPlatform  = 'x64'

# ---------- ПРОВЕРКА ПУТЕЙ ----------------------------------

foreach ($path in @($AssemblyInfoPath, $IssFilePath, $MSBuild, $ISCC)) {
    if (-not (Test-Path $path)) {
        Write-Host "ОШИБКА: Файл не найден:`n  $path" -ForegroundColor Red
        exit 1
    }
}

# ---------- ЧТЕНИЕ MinorVersion ИЗ AssemblyInfo.cs ----------

$assemblyContent = Get-Content $AssemblyInfoPath -Raw -Encoding UTF8
if ($assemblyContent -match '\[assembly: AssemblyVersion\("(\d+)\.(\d+\.\d+\.\d+)"\)\]') {
    $MinorVersion = $Matches[2]
    Write-Host "MinorVersion из AssemblyInfo.cs: $MinorVersion" -ForegroundColor Cyan
} else {
    Write-Host "ОШИБКА: Не удалось прочитать AssemblyVersion из AssemblyInfo.cs" -ForegroundColor Red
    exit 1
}

# ---------- РЕЗЕРВНАЯ КОПИЯ AssemblyInfo.cs -----------------

$backup = $AssemblyInfoPath + '.bak'
Copy-Item $AssemblyInfoPath $backup -Force
Write-Host "Резервная копия: $backup" -ForegroundColor Cyan

# ---------- ВСПОМОГАТЕЛЬНЫЕ ФУНКЦИИ ------------------------

function Set-AssemblyVersion {
    param([string]$FilePath, [string]$NewVersion)

    $content = Get-Content $FilePath -Raw -Encoding UTF8

    # Заменяем строку AssemblyVersion
    $content = $content -replace `
        '\[assembly: AssemblyVersion\("[^"]+"\)\]', `
        "[assembly: AssemblyVersion(`"$NewVersion`")]"

    # Если AssemblyFileVersion раскомментирована — обновляем и её
    $content = $content -replace `
        '\[assembly: AssemblyFileVersion\("[^"]+"\)\]', `
        "[assembly: AssemblyFileVersion(`"$NewVersion`")]"

    Set-Content $FilePath $content -Encoding UTF8 -NoNewline
}

function Invoke-Build {
    param([string]$Solution, [string]$Config, [string]$Platform)

    Write-Host "  Сборка: $Config | $Platform ..." -ForegroundColor Yellow
    & $MSBuild $Solution `
        /p:Configuration=$Config `
        /p:Platform=$Platform `
        /t:Rebuild `
        /v:minimal `
        /nologo

    if ($LASTEXITCODE -ne 0) {
        throw "MSBuild завершился с кодом $LASTEXITCODE"
    }
}

function Invoke-InnoSetup {
    param([string]$IssFile)

    Write-Host "  Компиляция установщика ..." -ForegroundColor Yellow
    & $ISCC $IssFile

    if ($LASTEXITCODE -ne 0) {
        throw "ISCC завершился с кодом $LASTEXITCODE"
    }
}

# ---------- ОСНОВНОЙ ЦИКЛ -----------------------------------

$results = @()

for ($inv = $VersionStart; $inv -le $VersionEnd; $inv++) {

    $version = "$inv.$MinorVersion"
    Write-Host "`n========================================" -ForegroundColor Magenta
    Write-Host "  Inventor $inv  →  версия $version"      -ForegroundColor Magenta
    Write-Host "========================================`n" -ForegroundColor Magenta

    try {
        # 1. Меняем версию
        Write-Host "  [1/3] Обновление AssemblyInfo.cs ..."
        Set-AssemblyVersion -FilePath $AssemblyInfoPath -NewVersion $version

        # 2. Пересобираем
        Write-Host "  [2/3] MSBuild ..."
        Invoke-Build -Solution $SolutionPath -Config $BuildConfig -Platform $BuildPlatform

        # 3. Компилируем установщик
        Write-Host "  [3/3] Inno Setup ..."
        Invoke-InnoSetup -IssFile $IssFilePath

        $results += [PSCustomObject]@{ Inventor = $inv; Версия = $version; Статус = "✅ OK" }
        Write-Host "  Готово для Inventor $inv" -ForegroundColor Green

    } catch {
        $results += [PSCustomObject]@{ Inventor = $inv; Версия = $version; Статус = "❌ ОШИБКА: $_" }
        Write-Host "  ОШИБКА для Inventor $inv`: $_" -ForegroundColor Red

        # Раскомментируйте следующую строку, если хотите остановиться при первой ошибке:
        # break
    }
}

# ---------- ВОССТАНОВЛЕНИЕ (опционально) --------------------
# По умолчанию — оставляем последнюю версию (26).
# Чтобы восстановить оригинал — раскомментируйте:
# Copy-Item $backup $AssemblyInfoPath -Force
# Write-Host "`nAssemblyInfo.cs восстановлен из резервной копии." -ForegroundColor Cyan

# ---------- ИТОГОВАЯ ТАБЛИЦА --------------------------------

Write-Host "`n`n============ ИТОГИ ============" -ForegroundColor Cyan
$results | Format-Table -AutoSize

$errors = $results | Where-Object { $_.Статус -like "❌*" }
if ($errors) {
    Write-Host "Завершено с ошибками." -ForegroundColor Red
    exit 1
} else {
    Write-Host "Все версии собраны успешно." -ForegroundColor Green
    exit 0
}