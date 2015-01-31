@ECHO OFF
REM cls
IF "%1."=="." GOTO NOPARAM
GOTO PARAMS

:NOPARAM
	echo You gave me nothing to work with...
	set errorlevel=1
	GOTO EOF

:PARAMS
	echo > c:\this
	IF NOT EXIST c:\this GOTO ELEVATION
	del c:\this
	GOTO %1
	echo %1 is not a valid profile.  Please try again.
	GOTO EOF

:ELEVATION
	echo This needs to be run as administrator.
	GOTO EOF
	
:CHEF
	REM Seems that we need the chef-client version from
	REM 11.6.0 version, and the GEM version < 2.4.0

	REM after install of chef-client 11.6.0
	REM >where chef-client
	REM C:\devops\opscode\chef\bin
	SET PATH=%PATH%;C:\monsoon\chef\bin

	REM after install of chef-client 11.6.0
	REM >where ruby
	REM C:\devops\opscode\chef\embedded\bin
	SET PATH=%PATH%;C:\monsoon\chef\embedded\bin

	REM Adding Devkit to front of path
	SET PATH=C:\monsoon\chef\embedded\mingw\bin;%PATH%
	SET RI_DEVKIT=C:\monsoon\chef\embedded\

	REM Enable advance logging of kitchen actions
	SET KITCHEN_LOG=DEBUG
	GOTO EOF

:GIT
	REM Adding Env Variables required based on monsoon docs here:
	REM https://monsoon.mo.sap.corp/docs/technical-setup/git-setup/
	SET GIT_SSH=C:\Program Files (x86)\Git\bin\ssh.exe
	SET HOME=%USERPROFILE%
	REM Adding GIT configuration settings from same page
	REM Get user 
	REM would like to use a sub-function for this: GOTO NAME GIT1
	REM For now hard-coded in the batch
	set /p fname=What is your first name?
	set /p lname=What is your last name?
	set SAPemail=%fname%.%lname%@sap.com
	echo Setting your email address as %SAPemail%
	echo.
	git config --global user.name  "%fname% %lname%"
	git config --global user.email "%SAPemail%"
	git config --global color.ui   true
	git config --global http.sslVerify false
	GOTO EOF

:NAME
	echo You entered the name loop from %1
	set /p fname=What is your first name?
	set /p lname=What is your last name?
	set SAPemail=%fname%.%lname%@sap.com
	echo Please confirm that your email address is %SAPemail%
	choice
	set correctemail=%errorlevel%
	IF %correctemail% 1 GOTO %1
	GOTO NAME
	GOTO EOF


:AWS
	REM AWS = Amazon Web Services
	REM These settings are configured in the same way one would 
	REM	configure for AWS, since Monsoon uses a similar framework
	
	SET AWS_ORGANIZATION=I837633
	SET AWS_SSH_KEY_ID=default
	REM Settings are project dependant.
	REM These keys may need to be maintained in multiple files.

	REM Settings for project: kitchen
	SET AWS_PROJECT=kitchen
	SET AWS_ACCESS_KEY=STgzNzYzMzo6MTc3OTc%%3D%%0A
	SET AWS_SECRET_KEY=hRfAb%%2FmOz6Phg%%2B%%2B73%%2BwuQhMmqz%%2BmSAHg%%2FZ%%2FyR1Ch4b4%%3D%%0A

	REM Settings for project: testKproject
	REM SET AWS_PROJECT=testKproject
	REM SET AWS_ACCESS_KEY=STgzNzYzMzo6MTc0MjI%%3D%%0A
	REM SET AWS_SECRET_KEY=Or8JTM2ShBT%%2Bx3bdgthoOsmrPIZZbvDpClFAgS3JqMA%%3D%%0A

	GOTO EOF

:EC2
	REM These settings are for the Amazon EC2 Command Line Interface Tool
	REM http://docs.aws.amazon.com/AWSEC2/latest/CommandLineReference/set-up-ec2-cli-windows.html
	SET EC2_HOME=c:\monsoon\ec2-api-tools-1.6.9.0
	SET CLASSPATH=%EC2_HOME%\lib
	SET EC2_SSH_KEY=%userprofile%\.ssh\id_rsa
	SET EC2_URL=https://ec2-us-west.api.monsoon.mo.sap.corp:443
	SET PATH=%PATH%;%EC2_HOME%\bin
	REM Alternate URL, I am unclear on the difference as both seem to work:
	REM SET EC2_URL=https://monsoon.mo.sap.corp/api/ec2
	GOTO EOF

:FOG
	REM I'm not really sure what this file is for right now...
	REM Seems to have to do with the monsoon-kitchen
	SET FOGFILE=.fog
	echo default: > %userprofile%\%FOGFILE%
	echo   aws_access_key_id: STgzNzYzMzo6MTc0MjI%3D%0A >> %userprofile%\%FOGFILE%
	echo   aws_secret_access_key: Or8JTM2ShBT%2Bx3bdgthoOsmrPIZZbvDpClFAgS3JqMA%3D%0A >> %userprofile%\%FOGFILE%
	echo   host: monsoon.mo.sap.corp >> %userprofile%\%FOGFILE%
	echo   path: /api/ec2 >> %userprofile%\%FOGFILE%
	GOTO EOF

:RSYNC
	REM  I really have no idea why this is here...
	REM Rsync was installed as a part of the instructions
	REM But it was never configured, beyond this line.
	SET PATH=%PATH%;c:\monsoon\cwRsync\bin
	GOTO EOF

:GEM
	REM GEMs are artefacts of ruby code that add functionality to the ruby stack
	REM The config file defines some sources and settings for the GEMs
	
	REM creates the ~/.gemrc file
	SET GEMRC_FILE=.gemrc
	echo --- > %userprofile%\%GEMRC_FILE%
	echo http_proxy: :no_proxy >> %userprofile%\%GEMRC_FILE%
	echo :bulk_threshold: 1000 >> %userprofile%\%GEMRC_FILE%
	echo :update_sources: true >> %userprofile%\%GEMRC_FILE%
	echo :backtrace: false >> %userprofile%\%GEMRC_FILE%
	echo :verbose: true >> %userprofile%\%GEMRC_FILE%
	echo :sources: >> %userprofile%\%GEMRC_FILE%
	echo - http://moo-repo.wdf.sap.corp:8080/geminabox/ >> %userprofile%\%GEMRC_FILE%
	echo - http://moo-repo.wdf.sap.corp:8080/rubygemsorg/ >> %userprofile%\%GEMRC_FILE%
	echo :benchmark: false >> %userprofile%\%GEMRC_FILE%
	echo gem: --no-http-proxy --no-ri --no-rdoc >> %userprofile%\%GEMRC_FILE%

	REM Setting GEM Vars
	SET GEM_PATH=C:\monsoon\Vagrant\bin\\..\embedded\gems
	GOTO EOF

:KITMON
	REM Based on the add2bashrc from the Kitchen-Monsoon Readme
	REM Leaving out most of the variables because they are defined in EC2 and AWS
	REM https://github.wdf.sap.corp/monsoon/kitchen-monsoon/blob/master/example/add2bashrc
	SET path=%path%;c:\monsoon\Vagrant\embedded\bin
	GOTO EOF

:VAG
	REM Vagrant is a virtualization API that kitchen uses to talk to EC2 Tools
	Rem and in turn talk to Monsoon, to provision and run cookbooks.
	
	REM Set up Vagrant Environment requirements
	SET EMBEDDED_DIR=C:\monsoon\Vagrant\bin\\..\embedded


:VARS-ONLY
	REM This section sets all of the variables above, without changing any files
	REM This is used to initialize a command session on the fly, so that the 
	REM Environmental variables aren't hanging out in the system permanently.

	REM From :CHEF
	SET PATH=%PATH%;C:\monsoon\chef\bin
	SET PATH=%PATH%;C:\monsoon\chef\embedded\bin
	SET PATH=C:\monsoon\chef\embedded\mingw\bin;%PATH%
	SET RI_DEVKIT=C:\monsoon\chef\embedded\s
	SET KITCHEN_LOG=DEBUG

	REM From :GIT
	SET GIT_SSH=C:\Program Files (x86)\Git\bin\ssh.exe
	SET HOME=%USERPROFILE%

	REM From :AWS
	SET AWS_ORGANIZATION=I837633
	SET AWS_PROJECT=kitchen
	SET AWS_ACCESS_KEY=STgzNzYzMzo6MTc3OTc%%3D%%0A
	SET AWS_SECRET_KEY=hRfAb%%2FmOz6Phg%%2B%%2B73%%2BwuQhMmqz%%2BmSAHg%%2FZ%%2FyR1Ch4b4%%3D%%0A
	SET AWS_SSH_KEY_ID=default

	REM From :EC2
	SET EC2_HOME=c:\monsoon\ec2-api-tools-1.6.9.0
	SET CLASSPATH=%EC2_HOME%\lib
	SET EC2_SSH_KEY=%userprofile%\.ssh\id_rsa
	SET EC2_URL=https://ec2-us-west.api.monsoon.mo.sap.corp:443
	SET PATH=%PATH%;%EC2_HOME%\bin

	REM From :RSYNC
	SET PATH=%PATH%;c:\monsoon\cwRsync\bin

	REM From :GEM
	SET GEM_PATH=C:\monsoon\Vagrant\bin\\..\embedded\gems

	REM From :KITMON
	SET PATH=%path%;c:\monsoon\Vagrant\embedded\bin
	SET PATH=%path%;C:\monsoon\Vagrant\bin

	REM From :VAG
	SET EMBEDDED_DIR=C:\monsoon\Vagrant\bin\\..\embedded

:EOF
cd %userprofile%\Documents\GitHub
set errorlevel=0