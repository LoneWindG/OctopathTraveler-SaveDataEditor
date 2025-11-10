# 八方旅人存档修改器

[![GitHub release](https://img.shields.io/github/v/release/LoneWindG/OctopathTraveler-SaveDataEditor?style=for-the-badge)](https://github.com/LoneWindG/OctopathTraveler-SaveDataEditor/releases/latest)
[![GitHub all releases](https://img.shields.io/github/downloads/LoneWindG/OctopathTraveler-SaveDataEditor/total?style=for-the-badge&color=00B000)](https://github.com/LoneWindG/OctopathTraveler-SaveDataEditor/releases)
[![GPLv3 license](https://img.shields.io/github/license/LoneWindG/OctopathTraveler-SaveDataEditor?style=for-the-badge&color=blue)](https://github.com/LoneWindG/OctopathTraveler-SaveDataEditor/blob/master/LICENSE)
[![Windows](https://img.shields.io/badge/PLATFORM-Windows-blueviolet?style=for-the-badge)](https://dotnet.microsoft.com/zh-cn/apps/desktop)
[![.NET 6.0](https://img.shields.io/badge/.NET-6.0-%234122AA?style=for-the-badge)](https://dotnet.microsoft.com/zh-cn/download/dotnet/6.0)

## 概要

适用于Steam, Xbox PC, Nintendo Switch等平台的八方旅人游戏存档修改
* 原作者: https://github.com/turtle-insect/OctopathTraveler
* **推荐**
  * 原作者写的UE引擎的Gvas格式存档的查看工具 https://github.com/turtle-insect/GvasViewer

## 语言

* READEME: [日本語](README_JA.md)    [English](README_EN.md)
* 程序支持语言: 英语, 简体中文, 日语（仅部分支持）

## 游戏官网

https://octopathtraveler.nintendo.com/
http://www.jp.square-enix.com/octopathtraveler/

## 运行要求

* Windows操作系统
* [.NET 桌面运行时 6.0](https://dotnet.microsoft.com/download/dotnet), [点此下载](https://aka.ms/dotnet/6.0/windowsdesktop-runtime-win-x64.exe)
* 八方旅人存档文件
  * Steam存档路径`%USERPROFILE%/Documents/My Games/Octopath_Traveler/(数字编号)/SaveGames`, 在SaveGames文件夹里SaveData0对应着游戏的自动存档. SaveData1/2/3对应相应顺序的存档
  * Xbox PC存档路径`%LOCALAPPDATA%/Packages/39EA002F.FrigateMS_n746a19ndrrjg/SystemAppData/wgs/(一串字母数字)/(一串字母数字)`, 在该文件夹内大小为1000多或2000多KB的几个文件为每个存档位的存档文件;要快速找到要修改的存档, 可以通过进入游戏内保存一个存档, 此目录内修改日期最近的那个就是
    * 推荐一个快速导出指定Xbox PC游戏存档的工具 https://github.com/Tom60chat/Xbox-Live-Save-Exporter, 但是导出时候会转换名称, 如果需要替换原存档需要自行对照转换前后的存档名称
  * Nintendo Switch, 从机器中导入导出存档(官方系统无法导出)

## Build环境

* Windows 10 (64位)
* Visual Studio 2022

## 功能说明

* **_功能问题_**
  * 有疑问或者问题请提[issue](https://github.com/LoneWindG/OctopathTraveler-SaveDataEditor/issues), 其余平台的发布帖子可能看不到通知
* 快捷按钮(从左到右):
  * 打开存档按钮: 打开存档后时自动在程序当前目录下的"OctopathTraveler Backup"文件夹备份打开的这个存档, 备份文件全名为"打开的存档名称(包含扩展名) - 存档的MD5"
  * 保存按钮, 覆盖修改后的数据到打开的存档
  * 两个Json转换按钮: 将游戏存档(UE引擎的Gvas格式存档)转换为部分可读的Json文件的按钮
* 预先说明, **请慎重修改存档**, 工具只提供存档查看修改功能, 未完全验证各项修改后是否会"坏档"(存档无法加载, 或加载后游戏无法正常进行)
* 1.基础数据: 显示一些存档基础数据及存档内保存的成就进度数据
  * 不在成就进度内显示的其他涉及到进度的例如需要识破弱点的381个怪物, 是没有单独保存识破数量在存档内的
* 2.角色: 显示角色的数据及穿戴装备
  * 角色数据: 显示等级经验当前血蓝职业副职业JP点等
    * 额外增加属性: 后面带+的数据为额外增加数据, 游戏内通过吃对应坚果获得; 几个特殊属性: ACC+, EVA+, CON+, AGI+分别对应命中, 回避, 会心, 速度
    * Exp为当前总经验, RawHP/MP为当前血蓝(非最大), 不确定当前总经验修改超过下一等级总经验及当前血蓝修改超过最大血蓝是否会坏档
    * FirstJob/SecondJob, 正常游玩主/副职业都不建议修改(主副职业改为相同/一个职业同时设置到2个3个人物身上/设置为未解锁职业, 等操作有可能导致坏档)
  * 角色装备: 显示角色当前穿戴的装备, 装备右侧按钮为修改当前穿戴装备(不建议修改, 穿戴未拥有装备/穿戴数量超过拥有数量, 等操作有可能导致坏档)
  * 队伍: 编辑队伍成员, 原作者程序内就有的功能, 正常游玩建议游戏内改
* 3.道具 & 道具收集: 分别用于修改已拥有的道具数量或添加新道具, 和显示道具道具收集成就的统计状态(暂时仅提供查看功能, 不支持修改)
* 4.地点
  * 显示地点是否已到达过(有些地点游戏内会到达, 但是我不知道这个ID对应的地点名字是什么)
  * 179-185的7个基础职业祠堂, 我不确定顺序是不是对的(因为原作者只给了一个猎人祠堂的ID并且我添加其他祠堂时我自己存档已经全解锁过了, 目前是按游戏内的职业顺序排的), 有可能是按祠堂所属地区顺序排的(地区顺序看该页面的ID顺序)
* 5.宝箱 & 隐藏道具: 显示地点的宝箱隐藏道具获取数量
  * 标题栏的总和, 单击可显示收集进度, 右击可进行显示筛选
  * VALUE的含义可查看这里[八方旅人宝箱状态对照说明 - 百度贴吧](https://tieba.baidu.com/p/7822253075)
  * 此处显示内容无法进行修改, 如果错过宝箱或隐藏道具但想完成成就, 可在成就进度那里对应栏增加你错过已无法获取的数量
* 6.怪物弱点
  * 该页面目前只显示了成就需要的381个怪物, 没有其他例如Boss(及召唤物), NPC的弱点显示
  * ID左侧复选框表示是否已识破该怪物全部弱点, 修改会同步勾选或取消勾选右侧全部弱点
  * 弱点复选框为灰色表示该怪物没有这个弱点(改实际是可以改的, 但是我不知道改了会不会有问题, 而且改了也没意义所以就屏蔽掉了)
  * 标题栏的ID, 单击会显示当前识破及未识破的怪物数量, 右击可进行显示筛选
  * "战略家"成就可通过修改这里获得, 已验证的可行方式为: 
    * 保留一个未全识破的怪物, 其余怪物全部勾选, 进入游戏遇到该怪物然后识破弱点即可完成
    * 勾选全部怪物保存存档然后进游戏能否之间解锁成就未知
* 7.捕获的魔物
  * 猎人的捕获魔物, 显示魔物名称, 技能剩余次数, 有没有启用(这个不知道干嘛的, 存档内有这个)
  * 该页面功能没有在原作者基础上进行修改
* 8.任务
  * 该页面功能没有在原作者基础上进行修改

## 游戏数据表

* 程序内嵌有info.xlsx及info_xxx.xlsx(xxx为语言代码)Excel数据表, 保存程序内显示的道具, 地点等的名称及一些额外数据
* 程序运行时可在文件菜单导出内嵌的数据表用于查看和编辑, 导出时会导出当前语言的info_xxx.xlsx数据表及info.xlsx英文数据表, 内嵌的没有当前语言数据表或当前为英文时只会导出info.xlsx
* 程序运行时会优先读取程序运行目录的info_xxx.xlsx或info.xlsx(没有当前语言的info_xxx.xlsx时读取), 没有时读取内嵌的数据表; 所以导出到当前运行目录的数据表请勿在不了解具体结构及读取规则的前提下随意修改, 否则可能会导致加载数据表时报错或某些数据为读取到不显示
* 快捷按钮栏右侧会显示当前加载的数据表名称及是否为内嵌数据表

## 特别感谢

* https://gbatemp.net/threads/octopath-traveler-save-editing.511125/
  * [SleepyPrince](https://gbatemp.net/members/sleepyprince.94652/)
  * [Takumah](https://gbatemp.net/members/takumah.456165/)
  * [Translate English by gen212](https://github.com/gen212/OctopathTraveler)
  * [SUDALV92](https://github.com/SUDALV92/OctopathTraveler-TreasureChests-)
* [八方旅人全成就指南 - Steam社区](https://steamcommunity.com/sharedfiles/filedetails/?id=2795091350)
* [八方旅人全资源清单 - Google表格](https://docs.google.com/spreadsheets/d/14Kz5mTAYdxqdgjbkbotAMGC2aoiJBbrBUiLeh8Pwu0Q)
* [八方旅人宝箱状态对照表 - Google表格](https://docs.google.com/spreadsheets/d/1WGN0166crI5IbnJ4QADnLiNHrL2FUr0MVFqmWH7dBRg)
* [八方旅人宝箱状态对照说明 - 百度贴吧](https://tieba.baidu.com/p/7822253075)
* [八方旅人怪物ID对照表 - Google表格](https://docs.google.com/spreadsheets/d/1O1OYHmLNsUcak5dByXbmEFDaxIbp-mDSHGC6j92P5ho)
* [【成就心得】學者 X 戰略家、收藏家、滴水不漏](https://forum.gamer.com.tw/C.php?bsn=31593&snA=585)
* [GVAS游戏存档转换工具](https://github.com/januwA/gvas-converter)
