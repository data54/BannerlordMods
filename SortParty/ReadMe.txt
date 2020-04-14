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


The following settings should have a value of either true or false:
EnableAutoSort - turns the autosort when you open/close the party screen on/off
EnableHotkey - turns the CTRL-SHIFT-S hotkey that will sort your party on/off
EnableRecruitUpgradeSortHotkey - turns the CTRL-SHIFT-R hotkey that will sort with upgradeable/recruitable units at the top on/off
EnableSortTypeCycleHotkey - turns the CTRL+SHIFT+- hotkey that will cycle sort types on/off
