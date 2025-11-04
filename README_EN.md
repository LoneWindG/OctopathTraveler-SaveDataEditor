# OCTOPATH TRAVELER SaveData Editor

[![GitHub release](https://img.shields.io/github/v/release/LoneWindG/OctopathTraveler-SaveDataEditor?style=for-the-badge)](https://github.com/LoneWindG/OctopathTraveler-SaveDataEditor/releases/latest)
[![GitHub all releases](https://img.shields.io/github/downloads/LoneWindG/OctopathTraveler-SaveDataEditor/total?style=for-the-badge&color=00B000)](https://github.com/LoneWindG/OctopathTraveler-SaveDataEditor/releases)
[![GPLv3 license](https://img.shields.io/github/license/LoneWindG/OctopathTraveler-SaveDataEditor?style=for-the-badge&color=blue)](https://github.com/LoneWindG/OctopathTraveler-SaveDataEditor/blob/master/LICENSE)
[![Windows](https://img.shields.io/badge/PLATFORM-Windows-blueviolet?style=for-the-badge)](https://dotnet.microsoft.com/zh-cn/apps/desktop)
[![.NET 6.0](https://img.shields.io/badge/.NET-6.0-%234122AA?style=for-the-badge)](https://dotnet.microsoft.com/zh-cn/download/dotnet/6.0)

## Language
* READEME: [日本語](README_JA.md) [中文](README.md)
* Supported languages: English, Simplified Chinese, Japanese (partial support only)
  * Default display is based on system language; unsupported languages ​​will be displayed as English.
  * Force display language: Create a shortcut and add "-language=language encoding" (a space is required before the hyphen) to the file path in the target field.
    * Supported language encodings (case-insensitive): en (any encoding starting with en, displays in English), zh-CN/zh_CN (displays in Simplified Chinese), ja-JP/ja_jp (displays in Japanese).

## Overview
Available for Steam, Xbox PC, Nintendo Switch, etc.

## Official Game Site
https://www.square-enix-games.com/games/octopath-traveler

## Prerequisites
* Windows machine
* .NET Desktop Runtime 6.0
* Octopath Traveler SaveData file

## Build environment
* Windows 10 (64 bit)
* Visual Studio 2022

# Steps to edit for Nintendo Switch
* Acquire save data from Nintendo Switch console.
* You should have a set of files that look similar to this:
      * KSSaveData1(KSSaveData2、KSSaveData3、、、)
      * KSSystemData
* Save editor will read in KSSaveData1(KSSaveData2、KSSaveData3、、、)
* Perform any editing
* Write out KSSaveData1(KSSaveData2、KSSaveData3、、、)
* Import newly edited saveData to Nintendo Switch console

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
