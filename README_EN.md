# OCTOPATH TRAVELER SaveData Editor

[![GitHub release](https://img.shields.io/github/v/release/LoneWindG/OctopathTraveler-SaveDataEditor?style=for-the-badge)](https://github.com/LoneWindG/OctopathTraveler-SaveDataEditor/releases/latest)
[![GitHub all releases](https://img.shields.io/github/downloads/LoneWindG/OctopathTraveler-SaveDataEditor/total?style=for-the-badge&color=00B000)](https://github.com/LoneWindG/OctopathTraveler-SaveDataEditor/releases)
[![GPLv3 license](https://img.shields.io/github/license/LoneWindG/OctopathTraveler-SaveDataEditor?style=for-the-badge&color=blue)](https://github.com/LoneWindG/OctopathTraveler-SaveDataEditor/blob/master/LICENSE)
[![Windows](https://img.shields.io/badge/PLATFORM-Windows-blueviolet?style=for-the-badge)](https://dotnet.microsoft.com/apps/desktop)
[![.NET 6.0](https://img.shields.io/badge/.NET-6.0-%234122AA?style=for-the-badge)](https://dotnet.microsoft.com/download/dotnet/6.0)

## Summary 

Octopath Traveler game save modification for platforms such as Steam, Xbox PC, and Nintendo Switch
  * Original author: https://github.com/turtle-insect/OctopathTraveler
  * **Recommended**
    * The Gvas format save viewing tool written by the original https://github.com/turtle-insect/GvasViewer

## Language
* README: [日本語](README_JA.md) [中文](README.md)
* Supported languages: English, Simplified Chinese, Japanese (partial support only)

## Official Game Site
https://octopathtraveler.nintendo.com/
https://www.square-enix-games.com/games/octopath-traveler

## Prerequisites
* Windows machine
* [.NET Desktop Runtime 6.0](https://dotnet.microsoft.com/download/dotnet), [Download](https://aka.ms/dotnet/6.0/windowsdesktop-runtime-win-x64.exe)
* Octopath Traveler save files
  * Steam save path: `%USERPROFILE%/Documents/My Games/Octopath_Traveler/(number)/(SaveGames)`, within the SaveGames folder, SaveData0 corresponds to the game's auto-save. SaveData1/2/3 correspond to the saves in the respective order.
  * Xbox PC save path: `%LOCALAPPDATA%/Packages/39EA002F.FrigateMS_n746a19ndrrjg/SystemAppData/wgs/(string of alphanumeric characters)/(string of alphanumeric characters)`, within this folder, the files that are over 1000 or 2000 KB in size are the save files for each save slot; to quickly find the save you want to modify, you can save a new save in the game, and the one with the most recent modification date in this directory is the one you just made.
    * Recommended a tool for quickly exporting specified Xbox PC game saves: https://github.com/Tom60chat/Xbox-Live-Save-Exporter, but the names will be converted during export. If you need to replace the original save, you will have to match the names of the saves before and after conversion yourself.
  * Nintendo Switch: Import and export saves from the console (official system cannot export).
    * Steps to edit for Nintendo Switch
      * Acquire save data from Nintendo Switch console.
      * You should have a set of files that look similar to this:
        * KSSaveData1(KSSaveData2、KSSaveData3、、、)
        * KSSystemData
      * Save editor will read in KSSaveData1(KSSaveData2、KSSaveData3、、、)
      * Perform any editing
      * Write out KSSaveData1(KSSaveData2、KSSaveData3、、、)
      * Import newly edited saveData to Nintendo Switch console

## Build environment
* Windows 10 (64 bit)
* Visual Studio 2022

## Function Description

* **_Function Issues_**
  * If you have any questions or problems, please create new [issue](https://github.com/LoneWindG/OctopathTraveler-SaveDataEditor/issues).
* This feature description was translated from Chinese using AI. See the [Original Description](README.md#功能说明) here.
* Quick buttons (from left to right):
  * Open Save Button: When opening a save, it automatically backs up the opened save in the "OctopathTraveler Backup" folder in the current program directory. The backup file name is "Opened Save Name (including extension) - Save MD5".
  * Save Button: Overwrite the modified data to the opened save.
  * Two Json Conversion Buttons: Buttons to convert the game save (UE engine Gvas format save) to a partially readable Json file.
* Precaution: **Please modify saves with caution**, the tool only provides save viewing and modification functions, and has not fully verified whether modifying various items will cause a "bad save" (save cannot be loaded, or the game cannot proceed normally after loading).
* 1. Basic Data: Displays some basic save data and achievement progress data saved in the save.
  * Other progress-related items not included in the achievement progress, such as the 381 monsters that need to have their weaknesses identified, do not have the number of identified weaknesses saved separately in the save.
* 2. Characters: Displays character data and equipped items.
  * Character Data: Displays level, experience, current HP/MP, class, sub-class, Job Point, etc.
    * Exp is the current total experience, RawHP/MP is the current HP/MP (not the maximum). It is uncertain whether modifying the current total experience to exceed the total experience required for the next level or modifying the current HP/MP to exceed the maximum HP/MP will cause a bad save.
    * FirstJob/SecondJob: It is not recommended to modify the main and sub-classes during normal gameplay (modifying the main and sub-classes to be the same, setting one class to two or three characters, setting to an unlocked class, etc., may cause a bad save).
  * Character Equipment: Displays the currently equipped items of the character. The button on the right side of the equipment is for modifying the currently equipped items (not recommended to modify; equipping items not owned or equipping more items than owned may cause a bad save).
  * Party: Edit party members. This is a function already available in the original author's program. It is recommended to modify the party in the game during normal gameplay.
* 3. Items / Item Collector: Used to modify the quantity of owned items or add new items, and to display the collection status of item collection achievements (currently only provides viewing function, does not support modification).
* 4. Places
  * Displays whether a place has been visited (some locations may be reached in the game, but I don't know the names of the locations corresponding to these IDs).
  * The 7 basic class shrines from 179 to 185, I'm not sure if the order is correct (because the original author only provided the ID of the Hunter Shrine and I added the other shrines after my save was already fully unlocked, currently they are arranged in the order of the classes in the game, but they might be arranged in the order of the regions where the shrines are located (refer to the ID order on this page)).
* 5. Treasure & Hidden Item: Displays the number of treasure chests and hidden items obtained at each location.
  * The total in the title bar, single-click to display the collection progress, right-click to filter the display. * The meaning of VALUE can be found here [八方旅人宝箱状态说明 - Baidu Tieba](https://tieba.baidu.com/p/7822253075)
  * The content displayed here cannot be modified. If you miss a treasure chest or hidden item but still want to complete the achievement, you can increase the number of missed items that you can no longer obtain in the corresponding column of the achievement progress.
* 6. Weaknesses
  * This page currently only shows the weaknesses of the 381 monsters required for achievements. Weaknesses of other monsters such as bosses (and their summons), and NPCs are not displayed.
  * The checkbox to the left of the ID indicates whether all weaknesses of the monster have been identified. Checking or unchecking this box will simultaneously check or uncheck all weaknesses on the right.
  * If the checkbox for a weakness is grayed out, it means the monster does not have that weakness. (This can actually be modified, but I don't know if it will cause any issues, and it's not meaningful to do so, so it's been disabled.)
  * Clicking the title bar's ID will display the number of identified and unidentified monsters. Right-clicking allows you to filter the display.
  * The "Strategist" achievement can be obtained by modifying here. A verified feasible method is:
    * Keep one monster with all weaknesses unidentified, and check all others. When you encounter this monster in the game and identify its weaknesses, the achievement will be completed.
    * It is unknown whether saving the file with all monsters checked and then entering the game will unlock the achievement immediately.
* 7. Tame Monster
  * The monsters captured by the Hunter, showing the monster's name, remaining skill uses, and whether it is enabled (I don't know what this is for, but it's in the save file).
  * This page's functionality has not been modified from the original author's version.
* 8. Quests
  * This page's functionality has not been modified from the original author's version.

# Special Thanks
* https://gbatemp.net/threads/octopath-traveler-save-editing.511125/
   * [SleepyPrince](https://gbatemp.net/members/sleepyprince.94652/)
   * [Takumah](https://gbatemp.net/members/takumah.456165/)
   * [Translate English by gen212](https://github.com/gen212/OctopathTraveler)
   * [八方旅人全成就指南 - Steam社区](https://steamcommunity.com/sharedfiles/filedetails/?id=2795091350)
   * [Octopath Traveler Resource - Google Sheets](https://docs.google.com/spreadsheets/d/14Kz5mTAYdxqdgjbkbotAMGC2aoiJBbrBUiLeh8Pwu0Q)
   * [Octopath Traveler : TreasureStates - Google Sheets](https://docs.google.com/spreadsheets/d/1WGN0166crI5IbnJ4QADnLiNHrL2FUr0MVFqmWH7dBRg)
   * [八方旅人宝箱状态对照说明 - Baidu Tieba](https://tieba.baidu.com/p/7822253075)
   * [Octopath Traveler : MONSTERS ID LIST - Google Sheets](https://docs.google.com/spreadsheets/d/1O1OYHmLNsUcak5dByXbmEFDaxIbp-mDSHGC6j92P5ho)
   * [GVAS-Converter](https://github.com/januwA/gvas-converter)
