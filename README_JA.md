# OCTOPATH TRAVELER(オクトパス トラベラー)のセーブデータ編集Tool

[![GitHub release](https://img.shields.io/github/v/release/LoneWindG/OctopathTraveler-SaveDataEditor?style=for-the-badge)](https://github.com/LoneWindG/OctopathTraveler-SaveDataEditor/releases/latest)
[![GitHub all releases](https://img.shields.io/github/downloads/LoneWindG/OctopathTraveler-SaveDataEditor/total?style=for-the-badge&color=00B000)](https://github.com/LoneWindG/OctopathTraveler-SaveDataEditor/releases)
[![GPLv3 license](https://img.shields.io/github/license/LoneWindG/OctopathTraveler-SaveDataEditor?style=for-the-badge&color=blue)](https://github.com/LoneWindG/OctopathTraveler-SaveDataEditor/blob/master/LICENSE)
[![Windows](https://img.shields.io/badge/PLATFORM-Windows-blueviolet?style=for-the-badge)](https://dotnet.microsoft.com/apps/desktop)
[![.NET 6.0](https://img.shields.io/badge/.NET-6.0-%234122AA?style=for-the-badge)](https://dotnet.microsoft.com/download/dotnet/6.0)

## 概要 (がいよう)

Steam、Xbox PC、Nintendo Switchなどのプラットフォームで利用可能なオクトパストラベラーのゲームセーブデータ編集
* 原作者: [https://github.com/turtle-insect/OctopathTraveler](https://github.com/turtle-insect/OctopathTraveler)
* **推奨**
  * 原作者によるUEエンジンGvas形式セーブデータビューア [https://github.com/turtle-insect/GvasViewer](https://github.com/turtle-insect/GvasViewer)

## 言語 (げんご)
* READEME: [English](README_EN.md) [中文](README.md)
* プログラム対応言語: 英語、簡体字中国語、日本語（一部のみ対応）

## ソフト
https://octopathtraveler.nintendo.com/
http://www.jp.square-enix.com/octopathtraveler

## 実行に必要

* Windowsオペレーティングシステム
* [.NET Desktop Runtime 6.0](https://dotnet.microsoft.com/download/dotnet)、[ダウンロード](https://aka.ms/dotnet/6.0/windowsdesktop-runtime-win-x64.exe)
* オクトパストラベラーのセーブファイル
  * Nintendo Switch：本体からセーブデータをインポート/エクスポートする必要があります（公式システムではエクスポート不可）。
    * SaveDataを吸い出す
    * 結果、以下が取得可能
      * KSSaveData1(KSSaveData2、KSSaveData3、、、)
      * KSSystemData
    * KSSaveData1(KSSaveData2、KSSaveData3、、、)を読み込む
    * 任意の編集を行う
    * KSSaveData1(KSSaveData2、KSSaveData3、、、)を書き出す
    * SaveDataを書き戻す
  * Steamのセーブデータパス：`%USERPROFILE%/Documents/My Games/Octopath_Traveler/(数字番号)/SaveGames`。`SaveGames`フォルダー内の`SaveData0`が**自動セーブ**に対応します。`SaveData1`/`2`/`3`はそれぞれ対応する順番のセーブデータです。
  * Xbox PCのセーブデータパス：`%LOCALAPPDATA%/Packages/39EA002F.FrigateMS_n746a19ndrrjg/SystemAppData/wgs/(一連の英数字)/(一連の英数字)`。このフォルダー内にある1000KBから2000KB程度のファイルが、各セーブスロットのセーブデータファイルです。編集したいセーブデータをすぐに見つけるには、ゲーム内で一度セーブし、このディレクトリ内で**更新日時が最新**のファイルを確認してください。
    * 特定のXbox PCゲームのセーブデータを素早くエクスポートするためのツールとして、[https://github.com/Tom60chat/Xbox-Live-Save-Exporter](https://github.com/Tom60chat/Xbox-Live-Save-Exporter)を推奨しますが、エクスポート時にファイル名が変換されるため、元のセーブデータと置き換える場合は、変換前後のファイル名を自身で照合する必要があります。

## Build環境

* Windows 10 (64bit)
* Visual Studio 2022

## 機能説明 (きのうせつめい)
* **_機能に関する問題_**
  * 疑問点や問題点がある場合は[issue](https://github.com/LoneWindG/OctopathTraveler-SaveDataEditor/issues)を立ててください
* [英語での説明](README_EN.md#Function-Description)を見る
* [中国語での説明](README.md#功能说明)を見る

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
