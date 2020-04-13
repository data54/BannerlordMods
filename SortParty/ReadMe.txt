The mod executes whenever the party screen is opened, no user interaction required past installing it unless you want to change the sort order.

In order to avoid overwriting user changes during updates the config file won't exist until after the first execution, quickest way to get it is load the game and open the party screen (P key).

Config file located at:
(steam install location)\Modules\SortParty\ModuleData\SortPartySettings.xml

The following are available sort options for the config SortOrder node, all will have companions at the top:
TierDesc 			- (default option) Ordered by Tier descending
TierAsc 			- Ordered by Tier ascending
TierDescType 		- Ordered by Tier descending followed by mounted/foot and then ranged/melee
TierAscType 		- Ordered by Tier ascending followed by mounted/foot and then ranged/melee
MountRangeTierDesc 	- Ordered by mounted/foot followed by ranged/melee then Tier descending
MountRangeTierAsc 	- Ordered by mounted/foot followed by ranged/melee then Tier ascending
CultureTierDesc 	- Ordered by culture then Tier descending
CultureTierAsc 		- Ordered by culture then Tier ascending


EnableAutoSort setting node turns the autosort when you open/close the party screen on/off value should be true or false
EnableHotkey setting node turns the CTRL-SHIFT-S hotkey that will sort your party on/off so it can be disabled if it conflicts with another mod, value should be true or false
