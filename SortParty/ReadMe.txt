In order to avoid overwriting user changes during updates the config file won't exist until after the module is loaded for the first time

Config file located at:
(steam install location)\Modules\PartyManager\ModuleData\PartyManager.xml

---------------Sort Types-----------------------------
The following are available sort options for the config SortOrder node, all will have companions at the top:
TierDesc 			- (default option) Ordered by Tier descending
TierAsc 			- Ordered by Tier ascending
TierDescType 		- Ordered by Tier descending followed by mounted/foot and then ranged/melee
TierAscType 		- Ordered by Tier ascending followed by mounted/foot and then ranged/melee
MountRangeTierDesc 	- Ordered by mounted/foot followed by ranged/melee then Tier descending
MountRangeTierAsc 	- Ordered by mounted/foot followed by ranged/melee then Tier ascending
CultureTierDesc 	- Ordered by culture then Tier descending
CultureTierAsc 		- Ordered by culture then Tier ascending
RangeMountTierDesc	- Melee/Ranged then by mounted then tier desc
RangeMountTierAsc	- Melee/Ranged then by mounted then tier Asc
Custom				- Sorts by user defined criteria

---------------Custom Sort Instructions---------------

Custom sort can be configured in game by clicking the settings button in the top right on the party screen

Custom sort will ignore the flags for CavalryAboveFootmen and MeleeAboveArchers as they're user defined

Available Sort Fields are:

None

TierAsc
TierDesc

MountedAsc
MountedDesc

MeleeAsc
MeleeDesc

CultureAsc
CultureDesc

UnitNameAsc
UnitNameDesc

Custom Sort uses the following settings in order to sort the list, you can use from 1-5 of them, default for all fields is None:
CustomSortOrderField1
CustomSortOrderField2
CustomSortOrderField3
CustomSortOrderField4
CustomSortOrderField5

The sort will essentially group your troops by the first field value and then each subgroup will be grouped by the next sort field and so on.

Examples (field1, field2, field3, field4, field5):

TierAsc MountedAsc MeleeAsc UnitNameAsc None - Ordered by tier ascending, each tier will be grouped into if they're mounted with horses on bottom followed by melee/ranged with melee on top and then they'll be ordered alphabetically

TierDesc CultureAsc UnitNameAsc None None - Grouped by tier descending, each tier will then be grouped by culture aphabetically and then ordered by name
CultureAsc TierDesc UnitNameAsc None None - Grouped by culture ascending, then each culture will then be grouped by Tier descending and then ordered by name
MountedAsc MeleeAsc UnitNameAsc None None - Infantry Archers MeleeCavalry RangedCavalry 
MountedAsc MeleeDesc UnitNameAsc None None - Archers Infantry RangedCavalry MeleeCavalry 
MountedAsc MeleeAsc TierDesc UnitNameAsc None - Infantry Archers MeleeCavalry RangedCavalry (each group is ordered by tier desc and each tier is alphabetized)
