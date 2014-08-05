QuickRestore
============

Quick Backup and Restore for Dev databases.

POWERSHELL:
==========

Add this to C:\Users\%user name%\Documents\WindowsPowershell\Microsoft.PowerShell_profile.ps1 to get shortcuts in powershell:

function qb($database)
{ 
	& "<path to exe>\QuickRestore.exe" -b $database
}

function qr($database)
{ 
	& "<path to exe>\QuickRestore.exe" -r $database
}


USAGE:
======

To backup default database: 
> qb 

To restore default database:
> qr

To backup named database:
> qb databaseName

To restore named database:
> qr databaseName
