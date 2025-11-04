# OCTOPATH TRAVELER(オクトパス トラベラー)のセーブデータ編集Tool

[![GitHub release](https://img.shields.io/github/v/release/LoneWindG/OctopathTraveler-SaveDataEditor?style=for-the-badge)](https://github.com/LoneWindG/OctopathTraveler-SaveDataEditor/releases/latest)
[![GitHub all releases](https://img.shields.io/github/downloads/LoneWindG/OctopathTraveler-SaveDataEditor/total?style=for-the-badge&color=00B000)](https://github.com/LoneWindG/OctopathTraveler-SaveDataEditor/releases)
[![GPLv3 license](https://img.shields.io/github/license/LoneWindG/OctopathTraveler-SaveDataEditor?style=for-the-badge&color=blue)](https://github.com/LoneWindG/OctopathTraveler-SaveDataEditor/blob/master/LICENSE)
[![Windows](https://img.shields.io/badge/PLATFORM-Windows-blueviolet?style=for-the-badge)](https://dotnet.microsoft.com/zh-cn/apps/desktop)
[![.NET 6.0](https://img.shields.io/badge/.NET-6.0-%234122AA?style=for-the-badge)](https://dotnet.microsoft.com/zh-cn/download/dotnet/6.0)

## Language

* READEME: [English](README_EN.md) [中文](README.md)
* 対応言語：英語、簡体字中国語、日本語（一部対応）
  * デフォルトの表示はシステム言語に基づきます。サポートされていない言語は英語で表示されます。
  * 表示言語の強制：ショートカットを作成し、対象フィールドのファイルパスに「-language=言語エンコード」（ハイフンの前にスペースが必要です）を追加します。
    * 対応言語エンコード（大文字と小文字は区別されません）：en（enで始まるすべてのエンコード、英語で表示）、zh-CN/zh_CN（簡体字中国語で表示）、ja-JP/ja_jp（日本語で表示）。

## 概要

Steam, Xbox PC, Nintendo Switch OCTOPATH TRAVELER(オクトパス トラベラー)のセーブデータ編集Tool

## ソフト

http://www.jp.square-enix.com/octopathtraveler

## 実行に必要

* Windows マシン
* [.NET デスクトップ ランタイム 6.0](https://dotnet.microsoft.com/download), [クリックダウンロード](https://aka.ms/dotnet/6.0/windowsdesktop-runtime-win-x64.exe)
* セーブデータの吸い出し
* セーブデータの書き戻し

## Build環境

* Windows 10 (64bit)
* Visual Studio 2022

## Nintendo Switch用編集の手順

* SaveDataを吸い出す
  * 結果、以下が取得可能
    * KSSaveData1(KSSaveData2、KSSaveData3、、、)
    * KSSystemData
* KSSaveData1(KSSaveData2、KSSaveData3、、、)を読み込む
* 任意の編集を行う
* KSSaveData1(KSSaveData2、KSSaveData3、、、)を書き出す
* SaveDataを書き戻す

## Special Thanks

* https://gbatemp.net/threads/octopath-traveler-save-editing.511125/
  * [SleepyPrince](https://gbatemp.net/members/sleepyprince.94652/)
  * [Takumah](https://gbatemp.net/members/takumah.456165/)
  * [Translate English by gen212](https://github.com/gen212/OctopathTraveler)
  * [八方旅人全成就指南 - Steam Community](https://steamcommunity.com/sharedfiles/filedetails/?id=2795091350)
  * [Octopath Traveler Resource - Google Sheets](https://docs.google.com/spreadsheets/d/14Kz5mTAYdxqdgjbkbotAMGC2aoiJBbrBUiLeh8Pwu0Q)
  * [Octopath Traveler : TreasureStates - Google Sheets](https://docs.google.com/spreadsheets/d/1WGN0166crI5IbnJ4QADnLiNHrL2FUr0MVFqmWH7dBRg)
  * [八方旅人宝箱状态对照说明 - Baidu Tieba](https://tieba.baidu.com/p/7822253075)
  * [Octopath Traveler : MONSTERS ID LIST - Google Sheets](https://docs.google.com/spreadsheets/d/1O1OYHmLNsUcak5dByXbmEFDaxIbp-mDSHGC6j92P5ho)
