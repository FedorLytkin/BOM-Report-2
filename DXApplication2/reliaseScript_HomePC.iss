; Script generated by the Inno Setup Script Wizard.
; SEE THE DOCUMENTATION FOR DETAILS ON CREATING INNO SETUP SCRIPT FILES!

#define MyAppName "BOM-Report Kompas 3D"
#define MyAppVersion "18.26.20.64"
#define MyAppPublisher "DXF-AutoHelp - dxfautohelp@gmail.com"
#define MyAppURL "dxfautohelp@gmail.com"
#define MyAppExeName "BOM Report.exe"

[Setup]
; NOTE: The value of AppId uniquely identifies this application.
; Do not use the same AppId value in installers for other applications.
; (To generate a new GUID, click Tools | Generate GUID inside the IDE.)
AppId={{62AF8549-D41F-489F-985F-89A2D2CFD67F}
AppName={#MyAppName}
AppVersion={#MyAppVersion}
;AppVerName={#MyAppName} {#MyAppVersion}
AppPublisher={#MyAppPublisher}
AppPublisherURL={#MyAppURL}
AppSupportURL={#MyAppURL}
AppUpdatesURL={#MyAppURL}
DefaultDirName=C:\Program Files (x86)\NSoft\{#MyAppName}
DisableDirPage=yes
DisableProgramGroupPage=yes
LicenseFile=C:\Users\fedor\source\repos\DXApplication2\DXApplication2\bin\Debug\lic.txt
InfoBeforeFile=C:\Users\fedor\source\repos\DXApplication2\DXApplication2\bin\Debug\ThisDemoVersion.txt
InfoAfterFile=C:\Users\fedor\source\repos\DXApplication2\DXApplication2\bin\Debug\ReadMe.txt
OutputDir=C:\Users\fedor\source\repos\DXApplication2\DXApplication2\bin\Release
OutputBaseFilename={#MyAppName}_{#MyAppVersion}_setup
SetupIconFile=C:\Users\fedor\source\repos\DXApplication2\DXApplication2\icons8-collage-30.ico
Compression=lzma
SolidCompression=yes

[Languages]
Name: "english"; MessagesFile: "compiler:Default.isl"
Name: "russian"; MessagesFile: "compiler:Languages\Russian.isl"

[Tasks]
Name: "desktopicon"; Description: "{cm:CreateDesktopIcon}"; GroupDescription: "{cm:AdditionalIcons}"; Flags: unchecked

[Files]
Source: "C:\Users\fedor\source\repos\DXApplication2\DXApplication2\bin\Debug\BOM Report.exe"; DestDir: "{app}"; Flags: ignoreversion
Source: "C:\Users\fedor\source\repos\DXApplication2\DXApplication2\bin\Debug\*"; DestDir: "{app}"; Flags: ignoreversion recursesubdirs createallsubdirs
; NOTE: Don't use "Flags: ignoreversion" on any shared system files

[Icons]
Name: "{commonprograms}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"
Name: "{commondesktop}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"; Tasks: desktopicon

[Run]
Filename: "{app}\{#MyAppExeName}"; Description: "{cm:LaunchProgram,{#StringChange(MyAppName, '&', '&&')}}"; Flags: nowait postinstall skipifsilent

