# Change screen saver timeout
$signature = @"
[DllImport("user32.dll")]
public static extern bool SystemParametersInfo(int uAction, int uParam, ref int lpvParam, int flags );
"@

$systemParamInfo = Add-Type -memberDefinition  $signature -Name ScreenSaver -passThru

Function Get-ScreenSaverTimeout
{
    [Int32]$value = 0
    $systemParamInfo::SystemParametersInfo(14, 0, [REF]$value, 0)
    $($value/60)
}

Function Set-ScreenSaverTimeout
{
    Param ([Int32]$value)
    $seconds = $value * 60
    [Int32]$nullVar = 0
    $systemParamInfo::SystemParametersInfo(15, $seconds, [REF]$nullVar, 2)
}