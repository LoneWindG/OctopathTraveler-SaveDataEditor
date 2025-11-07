using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;

namespace OctopathTraveler
{
    class BasicData : INotifyPropertyChanged
    {
        private readonly uint _moneyAddress;
        private readonly uint _heroAddress;
        private readonly uint? _battleCountAddress;
        private readonly uint? _escapeCountAddress;
        private readonly uint? _treasureCountAddress;
        private readonly uint? _hiddenPointCountAddress;

        public event PropertyChangedEventHandler? PropertyChanged;

        public bool IsExistBattleCount => _battleCountAddress != null;

        public bool IsExistEscapeCount => _escapeCountAddress != null;

        public bool IsExistTreasureCount => _treasureCountAddress != null;

        public bool IsExistHiddenPointCount => _hiddenPointCountAddress != null;

        public string SaveDate { get; }

        public string PlayTime { get; }

        public uint Money
        {
            get { return SaveData.Instance().ReadNumber(_moneyAddress, 4); }
            set
            {
                Util.WriteNumber(_moneyAddress, 4, value, 0, 9999999);
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Money)));
            }
        }

        public uint Hero
        {
            get => SaveData.Instance().ReadNumber(_heroAddress, 4);
            set
            {
                if (value < 1 || value > 8)
                {
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Hero)));
                    return;
                }
                Util.WriteNumber(_heroAddress, 4, value, 1, 8);
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Hero)));
            }
        }

        public uint BattleCount
        {
            get => SaveData.Instance().ReadNumber(_battleCountAddress, 4);
            set
            {
                Util.WriteNumber(_battleCountAddress, 4, value, 0, 9999999);
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(BattleCount)));
            }
        }

        public uint EscapeCount
        {
            get => SaveData.Instance().ReadNumber(_escapeCountAddress, 4);
            set
            {
                Util.WriteNumber(_escapeCountAddress, 4, value, 0, 9999999);
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(EscapeCount)));
            }
        }

        public uint TreasureCount
        {
            get => SaveData.Instance().ReadNumber(_treasureCountAddress, 4);
            set
            {
                Util.WriteNumber(_treasureCountAddress, 4, value, 0, 734);
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(TreasureCount)));
            }
        }

        public uint HiddenPointCount
        {
            get => SaveData.Instance().ReadNumber(_hiddenPointCountAddress, 4);
            set
            {
                Util.WriteNumber(_hiddenPointCountAddress, 4, value, 0, 152);
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(HiddenPointCount)));
            }
        }

        public List<ItemInventoryState> ItemInventoryStates { get; } = new List<ItemInventoryState>();
        public List<ItemInventoryTypeState> ItemInventoryTypeStates { get; private set; } = new List<ItemInventoryTypeState>();

        public int ItemInventoryCount { get; private set; }

        public BasicData(ReadOnlyCollection<Item> ownedItems)
        {
            var gvas = new GVAS(null);

            uint? soltDataAddress = Util.FindFirstAddress("slotData", 0, false);
            if (soltDataAddress == null)
            {
                SaveDate = "N/A";
                PlayTime = "N/A";
            }
            else
            {
                foreach (var address in SaveData.Instance().FindAddress("SaveDate_", soltDataAddress.Value))
                {
                    gvas.AppendValue(address, false);
                }
                SaveDate = new DateTime((int)gvas.ReadNumberOrDefault("SaveDate_Year"), (int)gvas.ReadNumberOrDefault("SaveDate_Month"),
                    (int)gvas.ReadNumberOrDefault("SaveDate_Day"), (int)gvas.ReadNumberOrDefault("SaveDate_Hour"),
                    (int)gvas.ReadNumberOrDefault("SaveDate_Minute"), (int)gvas.ReadNumberOrDefault("SaveDate_Second")).ToString();

                gvas.AppendValue(Util.FindFirstAddress("PlayTime", soltDataAddress));
                var playTime = new TimeSpan(gvas.ReadNumberOrDefault("PlayTime") * TimeSpan.TicksPerSecond);
                PlayTime = $"{(int)playTime.TotalHours}:{playTime.Minutes:D2}:{playTime.Seconds:D2}";
            }

            gvas.AppendValue(Util.FindFirstAddress("Money_", 0));
            _moneyAddress = gvas.Key("Money").Address;

            gvas.AppendValue(Util.FindFirstAddress("FirstSelectCharacterID", 0));
            _heroAddress = gvas.Key("FirstSelectCharacterID").Address;

            // NS save data does not have achievement data(maybe)
            uint? achievementAddress = Util.FindFirstAddress("Achievement", 0, false);
            gvas.AppendValue(Util.FindFirstAddress("BattleCount", achievementAddress, false));
            _battleCountAddress = gvas.KeyOrNull("BattleCount")?.Address;

            gvas.AppendValue(Util.FindFirstAddress("EscapeCount", achievementAddress, false));
            _escapeCountAddress = gvas.KeyOrNull("EscapeCount")?.Address;

            gvas.AppendValue(Util.FindFirstAddress("TreasureCount", achievementAddress, false));
            _treasureCountAddress = gvas.KeyOrNull("TreasureCount")?.Address;

            gvas.AppendValue(Util.FindFirstAddress("HiddenPointCount", achievementAddress, false));
            _hiddenPointCountAddress = gvas.KeyOrNull("HiddenPointCount")?.Address;

            gvas = new GVAS(null);
            gvas.AppendValue(Util.FindFirstAddress("ItemFlag", achievementAddress, false));

            int flagIndex = 0;
            string flagKey = "ItemFlag_" + flagIndex;
            var gotItemIds = new HashSet<uint>();
            while (gvas.HasKey(flagKey))
            {
                uint flags = gvas.ReadNumber(flagKey);
                for (int i = 0; i < 32; i++)
                {
                    uint itemId = (uint)(flagIndex * 32 + i);
                    bool isGot = ((flags >> i) & 1) == 1;
                    if (isGot)
                    {
                        gotItemIds.Add(itemId);
                        ItemInventoryCount++;
                    }
                }

                flagIndex++;
                flagKey = "ItemFlag_" + flagIndex;
            }
            gotItemIds.Remove(0);//Remove the item ID 0, which is invalid

            var itemInventory = Info.Instance().ItemInventory;
            itemInventory.Sort((x, y) =>
            {
                int typeCompare = x.Type.CompareTo(y.Type);
                if (typeCompare != 0)
                    return typeCompare;
                return x.Value.CompareTo(y.Value);
            });

            var inventoryTypeStates = new Dictionary<int, ItemInventoryTypeState>();
            foreach (var item in itemInventory)
            {
                uint id = item.Value;
                bool isGot = gotItemIds.Remove(item.Value);
                var inventoryState = new ItemInventoryState(id, isGot, item, ownedItems);
                ItemInventoryStates.Add(inventoryState);
                if (!inventoryTypeStates.TryGetValue(item.Type, out var typeInfo))
                {
                    typeInfo = new ItemInventoryTypeState
                    {
                        Type = item.Type < 10 ? $" {item.Type}  {item.TypeName}" : $"{item.Type}  {item.TypeName}",
                    };
                    inventoryTypeStates.Add(item.Type, typeInfo);
                    ItemInventoryTypeStates.Add(typeInfo);
                }
                typeInfo.TotalCount++;
                inventoryState.Order = typeInfo.TotalCount;
                if (isGot)
                {
                    typeInfo.GotCount++;
                }
            }
            foreach (var itemId in gotItemIds)
            {
                ItemInventoryStates.Add(new ItemInventoryState(itemId, true, null, ownedItems));
            }
        }
    }

    class ItemInventoryState
    {
        public uint ID { get; }

        public bool IsGot { get; }

        public bool IsOwned
        {
            get
            {
                if (ID == 0)
                    return false;

                if (_item == null || _item.ID != ID)
                {
                    _item = _ownedItems.FirstOrDefault(i => i.ID == ID);
                }
                return _item != null;
            }
        }

        public uint ItemCount => IsOwned ? _item!.Count : 0;

        public string Name { get; }

        public string TypeName { get; }

        public int Order { get; set; }

        private readonly ReadOnlyCollection<Item> _ownedItems;

        private Item? _item;

        public ItemInventoryState(uint id, bool isGot, InventoryItemInfo? inventoryItem, ReadOnlyCollection<Item> ownedItems)
        {
            ID = id;
            IsGot = isGot;
            _ownedItems = ownedItems;
            if (inventoryItem != null)
            {
                Name = inventoryItem.Name;
                TypeName = inventoryItem.TypeName;
            }
            else
            {
                Name = "UNKNOWN";
                TypeName = "UNKNOWN";
            }
        }
    }

    class ItemInventoryTypeState
    {
        public string Type { get; init; } = string.Empty;

        public int TotalCount { get; set; }

        public int GotCount { get; set; }

        public string Count => $"{GotCount}/{TotalCount}";

        public bool IsCompleted => TotalCount > 0 && GotCount >= TotalCount;

        public string Progress => TotalCount > 0 ? (GotCount / (float)TotalCount).ToString("P0") : string.Empty;
    }
}
