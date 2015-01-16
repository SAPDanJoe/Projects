Const ADS_UF_SCRIPT = &H0001 
Const ADS_UF_ACCOUNTDISABLE = &H0002 
Const ADS_UF_HOMEDIR_REQUIRED = &H0008 
Const ADS_UF_LOCKOUT = &H0010 
Const ADS_UF_PASSWD_NOTREQD = &H0020 
Const ADS_UF_PASSWD_CANT_CHANGE = &H0040 
Const ADS_UF_ENCRYPTED_TEXT_PASSWORD_ALLOWED = &H0080 
Const ADS_UF_DONT_EXPIRE_PASSWD = &H10000 
Const ADS_UF_SMARTCARD_REQUIRED = &H40000 
Const ADS_UF_PASSWORD_EXPIRED = &H800000 
 
Set usr = GetObject("WinNT://./EventUser")
flag = usr.Get("UserFlags")
 
If flag AND ADS_UF_SCRIPT Then
    Wscript.Echo "Flag Set ADS_UF_SCRIPT: Logon script will be executed."
Else
    Wscript.Echo "No Flag ADS_UF_SCRIPT: Logon script will not be executed."
End If
 
If flag AND ADS_UF_ACCOUNTDISABLE Then
    Wscript.Echo "Flag Set ADS_UF_ACCOUNTDISABLE: Account is disabled."
Else
    Wscript.Echo "No Flag ADS_UF_ACCOUNTDISABLE: Account is not disabled."
End If
 
If flag AND ADS_UF_HOMEDIR_REQUIRED Then
    Wscript.Echo "Flag Set ADS_UF_HOMEDIR_REQUIRE: Home directory required."
Else
    Wscript.Echo "No Flag ADS_UF_HOMEDIR_REQUIRE: Home directory not required."
End If
 
If flag AND ADS_UF_PASSWD_NOTREQD Then
    Wscript.Echo "Flag Set ADS_UF_PASSWD_NOTREQD: Password not required."
Else
    Wscript.Echo "No Flag ADS_UF_PASSWD_NOTREQD: Password required."
End If
 
If flag AND ADS_PASSWORD_CANT_CHANGE Then
    Wscript.Echo "Flag Set ADS_PASSWORD_CANT_CHANGE: User cannot change password."
Else
    Wscript.Echo "No Flag ADS_PASSWORD_CANT_CHANGE: User can change password."
End If
 
If flag AND ADS_UF_ENCRYPTED_TEXT_PASSWORD_ALLOWED Then
    Wscript.Echo "Flag Set ADS_UF_ENCRYPTED_TEXT_PASSWORD_ALLOWED: Encrypted password allowed."
Else
    Wscript.Echo "No Flag ADS_UF_ENCRYPTED_TEXT_PASSWORD_ALLOWED: Encrypted password not allowed."
End If
 
If flag AND ADS_UF_DONT_EXPIRE_PASSWD Then
    Wscript.Echo "Flag Set ADS_UF_DONT_EXPIRE_PASSWD: Password does not expire."
Else
    Wscript.Echo "No Flag ADS_UF_DONT_EXPIRE_PASSWD: Password expires."
End If
 
If flag AND ADS_UF_SMARTCARD_REQUIRED Then
    Wscript.Echo "Flag Set ADS_UF_SMARTCARD_REQUIRED: Smartcard required for logon."
Else
    Wscript.Echo "No Flag ADS_UF_SMARTCARD_REQUIRED: Smart card not required for logon."
End If
 
If flag AND ADS_UF_PASSWORD_EXPIRED Then
    Wscript.Echo "Flag Set ADS_UF_PASSWORD_EXPIRED: Password has expired."
Else
    Wscript.Echo "No Flag ADS_UF_PASSWORD_EXPIRED: Password has not expired."
End If
