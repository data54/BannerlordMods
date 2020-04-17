del /S "Modules" /F /Q
del /S "PartyManager.zip"

Xcopy /E /I /Y "L:\SteamLibrary\steamapps\common\Mount & Blade II Bannerlord\Modules\PartyManager\bin\Win64_Shipping_Client\*" "Modules\PartyManager\bin\Win64_Shipping_Client\"
Xcopy /E /I /Y "L:\SteamLibrary\steamapps\common\Mount & Blade II Bannerlord\Modules\PartyManager\GUI\*" "Modules\PartyManager\GUI\"
copy "C:\Users\datah\Desktop\PartyManager Bin Deployment\Modules\PartyManager\bin\Win64_Shipping_Client\SubModule.xml" "Modules\PartyManager\SubModule.xml"
del /S "Modules\PartyManager\bin\Win64_Shipping_Client\SubModule.xml"  /F /Q
del /S "Modules\PartyManager\bin\Win64_Shipping_Client\*.pdb" /F /Q
del /S "Modules\*vortex*" /F /Q
Powershell Compress-Archive "Modules" "PartyManager.zip"

PAUSE