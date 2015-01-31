@ECHO OFF
REM ***********************************************
REM Bootstrapper for the Monsoon Workstation Setup
REM ***********************************************
:TEST
	REM	GOTO EOF

:GLOBAL_VARS
	REM Setting global variables for installation
	SET currentdir=%~dp0
	SET destinationdir=c:\moo-ws-install
	SET installroot=c:\monsoon
	SET testfile=c:\test
	SET chefClientmsi=chef-client-11.6.0-1.windows.msi
	SET installoptions=/qn /norestart
	SET installparameters=INSTALLLOCATION^^^="%installroot%"
	SET VERBOSE=TRUE
	SET BREAK=FALSE
	SEO CHEF_VER=
	
:ENTRY
	REM Check if script is running with elevation
	echo > %testfile%
	IF NOT EXIST %testfile% GOTO ELEVATION
	del %testfile%
	if %VERBOSE%==TRUE echo Verified that the batch is running with elevated priveleges.
	if %BREAK%==TRUE pause
	GOTO COPY_FILES
	echo An unknown error occurred and I wasn't able to initialize the installation.	
	if %BREAK%==TRUE pause
	GOTO EOF

:ELEVATION
	echo This needs to be run as administrator.
	GOTO EOF
	
:COPY_FILES
	REM Check if there was a previous attempt to run the install
	if exist %destinationdir% (
		REM Remove file from stale install attempts
		if %VERBOSE%==TRUE echo %destinationdir% already exists.  Removing...
		RMDIR /S /Q %destinationdir%
		if %VERBOSE%==TRUE (
			echo Removal complete
			pause
		)
	)
	if %VERBOSE%==TRUE echo Creating %destinationdir%
	md %destinationdir%
	if %BREAK%==TRUE pause
	if %VERBOSE%==TRUE echo Copying files from: %currentdir%
	if %VERBOSE%==TRUE echo Copying files to:   %destinationdir%
	if %VERBOSE%==TRUE echo Using xcopy /E %currentdir%* %destinationdir%
	xcopy /E %currentdir%* %destinationdir%
	REM Check to make sure the files were copied successfully
	if not exist %destinationdir%\files\%chefClientmsi% (
		echo There was an error copying the required files to %destinationdir%.
		GOTO EOF
	)
	if %VERBOSE%==TRUE echo File copy completed successfully.
	GOTO INSTALL_CHEF
	
:INSTALL_CHEF
	REM Now we can start the install of the chef client
	if %VERBOSE%==TRUE echo Beginning installation of %chefClientmsi%
	if %VERBOSE%==TRUE echo Executing msiexec /i "%destinationdir%\files\%chefClientmsi%"  %installoptions% %installParameters%
	msiexec /i "%destinationdir%\files\%chefClientmsi%"  %installoptions% %installParameters%
	if %VERBOSE%==TRUE echo Installation Complete.
	pause
	if %VERBOSE%==TRUE echo Initializing chef settings...
	%destinationdir%\settings.bat CHEF
	pause
	if %VERBOSE%==TRUE echo Checking for successful installation.
	@Chef-Client --version
	Chef-Client --version > %CHEF_VER%
	echo %CHEF_VER%
	Chef-Client --version > c:\Chef_install_log.txt
	set >> c:\Chef_install_log.txt	
	GOTO EOF

:EOF
pause
REM exit /b