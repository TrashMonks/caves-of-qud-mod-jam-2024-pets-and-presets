# Changelog
This document lists the changes made to this pack to support Caves of Qud's 1.0 release.

## Updated mods
These mods had updates since the last time the pack was updated:
* Acid Rustler and Stubble
* Belongings
* Dawn-and-Dusk
* Denmagn
* my son crump
* Outcast Priest and Lampothy
* Relic Hunters

## Fliped preset tiles
Starting with Qud's 1.0 release, preset tiles are assumed to be "facing" left instead of right and are automatically flipped in-game.
* Aji's Favor
* Belongings
* Knight of the Wastes and Pantaglia Merchant
* Monster Catcher
* Oralloy Core
* Redox Glider
* Relic Hunters

## Fixed errors and warnings
* Belongings
  * Corrected the `NaturalWeapon` part to `MeleeWeapon` on Belonging's bite (`BelongingsBite`).
* In Search of a Friend
  * Removed the unused `ColorString` and `DetailColor` attributes from the Wandering Parasite preset.
* Ping
  * Correct closing `</anatomies>` tag.
* Relic Hunters
  * Removed `Yurtmat`'s nonexistent `IsBootSensitive` attribute on the base khopesh (`Dancerogue_RelicHunter_BaseKhopesh`).
  * Removed `VampiricWeapon`'s nonexistent `ShowInShortDescription` attribute on Nephrolepsis and Necrolepsis (`Dancerogue_RH_Nephrolepsis`, `Dancerogue_RH_Necrolepsis`).
* The Brothers Twins
  * Corrected the `pettable` parts on the twins to `Pettable`.
