rem @echo off

set scr=dp_scr.txt
echo select disk 0>> %scr%
echo clean>> %scr%
echo create partition primary>> %scr%
diskpart /s scr
del %scr%