@pushd %~dp0

%windir%\Microsoft.NET\Framework64\v4.0.30319\MSBuild.exe "Evercraft_model_Specs.csproj"

@if ERRORLEVEL 1 goto end

@cd ..\packages\SpecRun.*\tools

@set profile=%1
@if "%profile%" == "" set profile=Default

SpecRun.exe run %profile%.srprofile "/baseFolder:%~dp0\bin\Debug" /log:specrun.log %2 %3 %4 %5

:end

@popd
