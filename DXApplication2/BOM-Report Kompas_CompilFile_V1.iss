; Скрипт установщика BOM-Report Kompas.
; Универсальный: лежит в папке проекта рядом с .csproj, пути и имена определяются сами:
;   BasePath        - папка, где лежит этот .iss файл
;   ProjectFile     - единственный *.csproj в этой папке
;   MyAppExeName    - <AssemblyName> из .csproj + ".exe"
;   BuildPath       - из всех <OutputPath> в .csproj выбирается тот, где exe собран последним
;                     (т.е. текущая конфигурация решения, которую вы только что собрали)
;   SetupIconFile   - <ApplicationIcon> из .csproj
;   Тексты          - lic.txt / ThisDemoVersion.txt / ReadMe.txt из папки TxtFiles, Info или DocFiles
;                     (подключаются только те, что есть)
;   MyAppVersion    - версия из собранного exe; сверяется с AssemblyVersion в Properties\AssemblyInfo.cs
;
; Любой параметр можно переопределить из командной строки, например:
;   ISCC "этот файл.iss" /DOutputRoot="D:\Releases" /DBuildDir="bin\x64\Release\"
;   /DSkipVersionCheck - не сверять версию exe с AssemblyInfo.cs

; ======================== НАСТРОЙКИ ПРОЕКТА ========================
; Корневая папка для готовых установщиков
#ifndef OutputRoot
  #define OutputRoot "F:\Мой диск\Программы Realise\BOM-Report\BR для Компас 3D"
#endif
; Подпапка версии = префикс + первое число версии (например "Inventor 20" + "26" -> "Inventor 2026")
#ifndef OutputSubDirPrefix
  #define OutputSubDirPrefix "Kompas 3D "
#endif
; Имя программы в установщике. "" = <AssemblyName> из .csproj
#ifndef AppNameOverride
  #define AppNameOverride "BOM-Report Kompas 3D"
#endif
; Добавка к имени файла установщика: <имя><добавка>_<версия>_setup.exe
#ifndef SetupNameSuffix
  #define SetupNameSuffix ""
#endif
; Постоянный AppId (чтобы новая версия обновляла старую). "" = новый AppId из MD5 exe при каждой сборке
#ifndef FixedAppId
  #define FixedAppId ""
#endif
; 1 = не показывать страницу выбора папки установки
#ifndef DisableDirPage
  #define DisableDirPage 0
#endif
; 1 = предлагать ярлык в панели быстрого запуска
#ifndef QuickLaunchIcon
  #define QuickLaunchIcon 1
#endif
; ===================================================================

; Постоянные данные издателя
#define MyAppPublisher "DXF-AutoHelp - dxfautohelp@gmail.com"
#define MyAppURL "dxfautohelp@gmail.com"

; ---------- Папка проекта = папка этого скрипта ----------
#define BasePath Copy(SourcePath, 1, Len(SourcePath) - 1)

; ---------- Поиск .csproj ----------
#ifndef ProjectFile
  #define FindHandle FindFirst(SourcePath + "*.csproj", 0)
  #if !FindHandle
    #error Рядом со скриптом не найден файл *.csproj
  #endif
  #define ProjectFile SourcePath + FindGetFileName(FindHandle)
  #expr FindClose(FindHandle)
#endif

; ---------- Папка с текстами для установщика ----------
#if DirExists(SourcePath + "TxtFiles")
  #define InfoDir SourcePath + "TxtFiles\"
#elif DirExists(SourcePath + "Info")
  #define InfoDir SourcePath + "Info\"
#elif DirExists(SourcePath + "DocFiles")
  #define InfoDir SourcePath + "DocFiles\"
#else
  #define InfoDir ""
#endif

; Значение тега <Tag>...</Tag> из строки, либо "" если тега в строке нет
#define TagValue(str Line, str Tag) \
  (Pos("<" + Tag + ">", Line) > 0 && Pos("</" + Tag + ">", Line) > Pos("<" + Tag + ">", Line)) \
    ? Copy(Line, Pos("<" + Tag + ">", Line) + Len(Tag) + 2, \
           Pos("</" + Tag + ">", Line) - Pos("<" + Tag + ">", Line) - Len(Tag) - 2) \
    : ""

; ---------- Служебные переменные ----------
; Все переменные, которые меняются внутри #sub, объявляются здесь, до первого #for:
; переменные, объявленные после #for, препроцессор внутри #sub не видит.
#define AssemblyName ""
#define MyAppExeName ""
#define ApplicationIcon ""
#define OutputPaths ""
#define CurLine ""
#define CurValue ""
#define FileHandle 0
#define RestPaths ""
#define CurPath ""
#define CurExe ""
#define CurDate 0
#define CurTime 0
#define BestDate 0
#define BestTime 0
#define DetectedBuildDir ""
#define SourceVersion ""
#define VersionPrefix '[assembly: AssemblyVersion("'

; ---------- Чтение .csproj: AssemblyName, ApplicationIcon, все OutputPath ----------

#sub ReadCsprojLine
  #expr CurLine = FileRead(FileHandle)
  #expr CurValue = TagValue(CurLine, "AssemblyName")
  #if CurValue != "" && AssemblyName == ""
    #expr AssemblyName = CurValue
  #endif
  #expr CurValue = TagValue(CurLine, "ApplicationIcon")
  #if CurValue != "" && ApplicationIcon == ""
    #expr ApplicationIcon = CurValue
  #endif
  #expr CurValue = TagValue(CurLine, "OutputPath")
  #if CurValue != ""
    #expr OutputPaths = OutputPaths + CurValue + "|"
  #endif
#endsub

#for {FileHandle = FileOpen(ProjectFile); FileHandle && !FileEof(FileHandle); ""} ReadCsprojLine
#if FileHandle
  #expr FileClose(FileHandle)
#endif

#if AssemblyName == ""
  #error В .csproj не найден тег AssemblyName
#endif

#expr MyAppExeName = AssemblyName + ".exe"
#if AppNameOverride != ""
  #define MyAppName AppNameOverride
#else
  #define MyAppName AssemblyName
#endif

; ---------- Выбор папки сборки: где exe собран позже всех ----------
#ifndef BuildDir
  #expr RestPaths = OutputPaths

  #sub CheckOutputPath
    #expr CurPath = Copy(RestPaths, 1, Pos("|", RestPaths) - 1)
    #expr RestPaths = Copy(RestPaths, Pos("|", RestPaths) + 1, Len(RestPaths))
    #expr CurExe = SourcePath + CurPath + MyAppExeName
    #if FileExists(CurExe)
      #expr CurDate = Int(GetFileDateTimeString(CurExe, "yyyymmdd", "/", ":"), 0)
      #expr CurTime = Int(GetFileDateTimeString(CurExe, "hhnnss", "/", ":"), 0)
      #if CurDate > BestDate || (CurDate == BestDate && CurTime > BestTime)
        #expr BestDate = CurDate
        #expr BestTime = CurTime
        #expr DetectedBuildDir = CurPath
      #endif
    #endif
  #endsub

  #for {""; Pos("|", RestPaths) > 0; ""} CheckOutputPath

  #if DetectedBuildDir == ""
    #error Не найден собранный exe ни в одной из папок OutputPath проекта. Сначала соберите решение.
  #endif
  #define BuildDir DetectedBuildDir
#endif

; BuildDir может быть относительным (от папки проекта) или абсолютным
#if Pos(":", BuildDir) > 0
  #define BuildPath BuildDir
#else
  #define BuildPath SourcePath + BuildDir
#endif
#if Copy(BuildPath, Len(BuildPath), 1) != "\"
  #define BuildPath BuildPath + "\"
#endif

#define MyAppExePath BuildPath + MyAppExeName
#if !FileExists(MyAppExePath)
  #expr Error("Не найден файл: " + MyAppExePath)
#endif

; ---------- Версия ----------
#define MyAppVersion GetStringFileInfo(MyAppExePath, "FileVersion")
#define MajorVersion Copy(MyAppVersion, 1, Pos(".", MyAppVersion) - 1)

; Сверка с AssemblyVersion в исходниках: защита от забытой пересборки
#define AssemblyInfoFile SourcePath + "Properties\AssemblyInfo.cs"
#sub ReadAssemblyInfoLine
  #expr CurLine = Trim(FileRead(FileHandle))
  #if Pos(VersionPrefix, CurLine) == 1
    #expr SourceVersion = Copy(CurLine, Len(VersionPrefix) + 1, Len(CurLine))
    #expr SourceVersion = Copy(SourceVersion, 1, Pos('"', SourceVersion) - 1)
  #endif
#endsub
#if FileExists(AssemblyInfoFile) && !Defined(SkipVersionCheck)
  #for {FileHandle = FileOpen(AssemblyInfoFile); FileHandle && !FileEof(FileHandle); ""} ReadAssemblyInfoLine
  #if FileHandle
    #expr FileClose(FileHandle)
  #endif
  #if SourceVersion != "" && SourceVersion != MyAppVersion
    #expr Error("Версия собранного exe (" + MyAppVersion + ") не совпадает с AssemblyVersion в AssemblyInfo.cs (" + SourceVersion + "). Пересоберите решение.")
  #endif
#endif

; ---------- Итоговые пути ----------
#define DynamicOutputDir OutputRoot + "\" + OutputSubDirPrefix + MajorVersion
#if FixedAppId != ""
  #define AppGUID FixedAppId
#else
  #define AppGUID "{{" + GetMD5OfFile(MyAppExePath) + "}"
#endif

#pragma message "Проект:      " + ProjectFile
#pragma message "Сборка:      " + BuildPath
#pragma message "Программа:   " + MyAppName + " (" + MyAppExeName + ")  v" + MyAppVersion
#pragma message "Результат:   " + DynamicOutputDir

[Setup]
AppId={#AppGUID}
AppName={#MyAppName}
AppVersion={#MyAppVersion}
AppPublisher={#MyAppPublisher}
AppPublisherURL={#MyAppURL}
AppSupportURL={#MyAppURL}
AppUpdatesURL={#MyAppURL}
DefaultDirName={commonpf32}\NSoft\{#MyAppName}
#if DisableDirPage
DisableDirPage=yes
#endif
DisableProgramGroupPage=yes
#if InfoDir != "" && FileExists(InfoDir + "lic.txt")
LicenseFile={#InfoDir}lic.txt
#endif
#if InfoDir != "" && FileExists(InfoDir + "ThisDemoVersion.txt")
InfoBeforeFile={#InfoDir}ThisDemoVersion.txt
#endif
#if InfoDir != "" && FileExists(InfoDir + "ReadMe.txt")
InfoAfterFile={#InfoDir}ReadMe.txt
#endif
OutputDir={#DynamicOutputDir}
OutputBaseFilename={#MyAppName}{#SetupNameSuffix}_{#MyAppVersion}_setup
#if ApplicationIcon != "" && FileExists(SourcePath + ApplicationIcon)
SetupIconFile={#SourcePath}{#ApplicationIcon}
#endif
UninstallDisplayIcon={app}\{#MyAppExeName}
Compression=lzma
SolidCompression=yes

[Languages]
Name: "english"; MessagesFile: "compiler:Default.isl"
Name: "russian"; MessagesFile: "compiler:Languages\Russian.isl"

[Tasks]
Name: "desktopicon"; Description: "{cm:CreateDesktopIcon}"; GroupDescription: "{cm:AdditionalIcons}"; Flags: unchecked
#if QuickLaunchIcon
Name: "quicklaunchicon"; Description: "{cm:CreateQuickLaunchIcon}"; GroupDescription: "{cm:AdditionalIcons}"; Flags: unchecked; OnlyBelowVersion: 0,6.1
#endif

[Files]
Source: "{#BuildPath}*"; DestDir: "{app}"; Flags: ignoreversion recursesubdirs createallsubdirs

[Icons]
Name: "{commonprograms}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"
Name: "{commondesktop}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"; Tasks: desktopicon
#if QuickLaunchIcon
Name: "{userappdata}\Microsoft\Internet Explorer\Quick Launch\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"; Tasks: quicklaunchicon
#endif

[Run]
Filename: "{app}\{#MyAppExeName}"; Description: "{cm:LaunchProgram,{#StringChange(MyAppName, '&', '&&')}}"; Flags: nowait postinstall skipifsilent
