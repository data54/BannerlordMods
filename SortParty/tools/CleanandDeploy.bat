del /S "L:\SteamLibrary\steamapps\common\Mount & Blade II Bannerlord\Modules\PartyManager\*" /F /Q

msbuild ..\..\BannerLordMods.sln

Xcopy /E /I /Y ".\PartyManager\GUI\*" "L:\SteamLibrary\steamapps\common\Mount & Blade II Bannerlord\Modules\PartyManager\GUI"
copy "L:\SteamLibrary\steamapps\common\Mount & Blade II Bannerlord\Modules\PartyManager\bin\Win64_Shipping_Client\SubModule.xml" "L:\SteamLibrary\steamapps\common\Mount & Blade II Bannerlord\Modules\PartyManager\SubModule.xml"

PAUSE