using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using Microsoft.Win32;
using static OctopathTraveler.Properties.Resources;

namespace OctopathTraveler
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            MenuItemLanguage.Items.Clear();
            foreach (var lang in App.SupportedLanguage)
            {
                bool isCurrent = Culture == lang;
                var item = new MenuItem
                {
                    Header = App.GetScriptCulture(lang).NativeName,
                    IsCheckable = true,
                    IsChecked = isCurrent,
                    DataContext = lang
                };

                item.Click += MenuItemLanguageOption_Click;
                MenuItemLanguage.Items.Add(item);
            }
        }

        private void Window_PreviewDragOver(object sender, DragEventArgs e)
        {
            e.Handled = e.Data.GetDataPresent(DataFormats.FileDrop);
        }

        private void Window_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetData(DataFormats.FileDrop) is not string[] files) return;
            if (files.Length == 0 || !File.Exists(files[0])) return;

            Open(files[0]);
        }

        private void MenuItemFileOpen_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new OpenFileDialog();
            if (!dlg.ShowDialog().GetValueOrDefault()) return;

            Open(dlg.FileName);
        }

        private void MenuItemFileSave_Click(object sender, RoutedEventArgs e)
        {
            if (SaveData.IsReadonlyMode)
            {
                MessageBox.Show(MeaageSaveFail, "ReadonlyMode");
                return;
            }
            Save();
        }

        private void MenuItemFileSaveAs_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new SaveFileDialog();
            if (!dlg.ShowDialog().GetValueOrDefault()) return;

            if (string.Equals(dlg.FileName, SaveData.Instance().FileName))
            {
                MessageBox.Show(MeaageSaveAsSameFileFail, MenuFileSaveAs);
                return;
            }

            if (SaveData.Instance().SaveAs(dlg.FileName) == true) MessageBox.Show(MessageSaveSuccess);
            else MessageBox.Show(MeaageSaveFail, SaveData.IsReadonlyMode ? "ReadonlyMode" : "");
        }

        private void MenuItemFileSaveAsJson_Click(object sender, RoutedEventArgs e)
        {
            ConvertSaveDataToJson(false);
        }

        private void MenuItemExportInfoExcel_Click(object sender, RoutedEventArgs e)
        {
            if (!Info.TryGetEmbeddedInfoExcel(out var excels))
            {
                MessageBox.Show(MeaageSaveFail);
                return;
            }

            bool isSaveAny = false;
            foreach ((var fileName, var bytes) in excels)
            {
                string ext = Path.GetExtension(fileName);
                string v = $"Excel files (*{ext})|*{ext}|All files (*.*)|*.*";
                var dialog = new SaveFileDialog
                {
                    InitialDirectory = Directory.GetCurrentDirectory(),
                    FileName = fileName,
                    Filter = v
                };
                if (dialog.ShowDialog() == false)
                    continue;

                File.WriteAllBytes(dialog.FileName, bytes);
                isSaveAny = true;
            }

            MessageBox.Show(isSaveAny ? MessageSaveSuccess : MeaageSaveFail);
        }

        private void MenuExportExampleSaveFile_Click(object sender, RoutedEventArgs e)
        {
            var exampleSaveData = ExampleSaveFile;
            if (exampleSaveData == null || exampleSaveData.Length == 0)
            {
                MessageBox.Show("File not found", MeaageSaveFail);
                return;
            }
            MessageBox.Show(MenuExportExampleSaveFileTip, MenuExportExampleSaveFile);

            var dlg = new SaveFileDialog
            {
                InitialDirectory = Directory.GetCurrentDirectory(),
                FileName = "ExampleSaveData",
            };
            if (!dlg.ShowDialog().GetValueOrDefault())
                return;

            File.WriteAllBytes(dlg.FileName, exampleSaveData);
            MessageBox.Show(MessageSaveSuccess, MenuExportExampleSaveFile);
        }

        private void MenuItemExit_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void MenuItemAbout_Click(object sender, RoutedEventArgs e)
        {
            new AboutWindow().ShowDialog();
        }

        private void MenuItemLanguageOption_Click(object sender, RoutedEventArgs e)
        {
            if (sender is not MenuItem clickedItem) return;
            if (clickedItem.DataContext is not CultureInfo cultureInfo) return;

            if (cultureInfo == Culture)
            {
                clickedItem.IsChecked = true;
                return;
            }
            App.SetLanguage(cultureInfo);
            var oldDataContext = DataContext;
            var newWindow = new MainWindow();
            if (oldDataContext is not null)
            {
                Info.Release();
                newWindow.Init();
            }
            newWindow.Show();
            Close();
        }

        private void ToolBarFileOpen_Click(object sender, RoutedEventArgs e)
        {
            MenuItemFileOpen_Click(sender, e);
        }

        private void ToolBarFileSave_Click(object sender, RoutedEventArgs e)
        {
            if (SaveData.IsReadonlyMode)
            {
                MessageBox.Show("ReadonlyMode");
                return;
            }
            Save();
        }

        private void ToolBarConvertToJson_Click(object sender, RoutedEventArgs e)
        {
            ConvertSaveDataToJson(true);
        }

        private void Init()
        {
            SetWeakFilter(0);
            SetTreasureStateFilter(0);
            _itemInventoryFilter = 0;
            DataContext = new DataContext();
        }

        private void Open(string fileName)
        {
            SaveData.Instance().Open(fileName);
            Init();
            string backupFile = SaveData.Instance().BackupFileName;
            if (!string.IsNullOrEmpty(backupFile) && File.Exists(backupFile))
            {
                MessageBox.Show($"Backup:\n" + backupFile, MessageLoadSuccess);
            }
            else
            {
                MessageBox.Show("No backup", MessageLoadSuccess);
            }
        }

        private void Save()
        {
            if (SaveData.Instance().Save() == true) MessageBox.Show(MessageSaveSuccess);
            else MessageBox.Show(MeaageSaveFail, SaveData.IsReadonlyMode ? "ReadonlyMode" : "");
        }

        private static void ConvertSaveDataToJson(bool convertOtherFile)
        {
            string fileName;
            if (convertOtherFile)
            {
                var openDialog = new OpenFileDialog();
                if (openDialog.ShowDialog() == false) return;
                fileName = openDialog.FileName;
            }
            else
            {
                fileName = SaveData.Instance().FileName;
                if (string.IsNullOrWhiteSpace(fileName))
                {
                    MessageBox.Show(MeaageSaveFail);
                    return;
                }
            }

            string folder = Path.GetDirectoryName(fileName) ?? string.Empty;
            var saveDialog = new SaveFileDialog
            {
                InitialDirectory = string.IsNullOrEmpty(folder) ? Directory.GetCurrentDirectory() : folder,
                FileName = Path.GetFileName(fileName) + ".json",
                Filter = "Json files (*.json)|*.json|All files (*.*)|*.*"
            };
            if (saveDialog.ShowDialog() == false)
                return;

            bool saved;
            Exception? ex;

            if (convertOtherFile)
                (saved, ex) = GvasFormat.GvasConverter.Convert2JsonFile(saveDialog.FileName, File.OpenRead(fileName));
            else
                (saved, ex) = SaveData.Instance().SaveAsJson(saveDialog.FileName);

            if (ex != null)
            {
                if (saved)
                    MessageBox.Show("Only part of the data is saved as json because of the exception:\n" + ex.ToString(), MessageSaveSuccess);
                else
                    MessageBox.Show(ex.ToString(), MeaageSaveFail);

            }
            else
            {
                MessageBox.Show(saved ? MessageSaveSuccess : MeaageSaveFail);
            }
        }

        private void ButtonGotoItemInventory_Click(object sender, RoutedEventArgs e)
        {
            TabItemItemInventory.IsSelected = true;
        }

        private void ButtonSword_Click(object sender, RoutedEventArgs e)
        {
            if (CharactorList.SelectedItem is Charactor chara)
            {
                chara.Sword = ChoiceEquipment(chara.Sword);
            }
        }

        private void ButtonLance_Click(object sender, RoutedEventArgs e)
        {
            if (CharactorList.SelectedItem is Charactor chara)
            {
                chara.Lance = ChoiceEquipment(chara.Lance);
            }
        }

        private void ButtonDagger_Click(object sender, RoutedEventArgs e)
        {
            if (CharactorList.SelectedItem is Charactor chara)
            {
                chara.Dagger = ChoiceEquipment(chara.Dagger);
            }
        }

        private void ButtonAxe_Click(object sender, RoutedEventArgs e)
        {
            if (CharactorList.SelectedItem is Charactor chara)
            {
                chara.Axe = ChoiceEquipment(chara.Axe);
            }
        }

        private void ButtonBow_Click(object sender, RoutedEventArgs e)
        {
            if (CharactorList.SelectedItem is Charactor chara)
            {
                chara.Bow = ChoiceEquipment(chara.Bow);
            }
        }

        private void ButtonRod_Click(object sender, RoutedEventArgs e)
        {
            if (CharactorList.SelectedItem is Charactor chara)
            {
                chara.Rod = ChoiceEquipment(chara.Rod);
            }
        }

        private void ButtonShield_Click(object sender, RoutedEventArgs e)
        {
            if (CharactorList.SelectedItem is Charactor chara)
            {
                chara.Shield = ChoiceEquipment(chara.Shield);
            }
        }

        private void ButtonHead_Click(object sender, RoutedEventArgs e)
        {
            if (CharactorList.SelectedItem is Charactor chara)
            {
                chara.Head = ChoiceEquipment(chara.Head);
            }
        }

        private void ButtonBody_Click(object sender, RoutedEventArgs e)
        {
            if (CharactorList.SelectedItem is Charactor chara)
            {
                chara.Body = ChoiceEquipment(chara.Body);
            }
        }

        private void ButtonAccessory1_Click(object sender, RoutedEventArgs e)
        {
            if (CharactorList.SelectedItem is Charactor chara)
            {
                chara.Accessory1 = ChoiceEquipment(chara.Accessory1);
            }
        }

        private void ButtonAccessory2_Click(object sender, RoutedEventArgs e)
        {
            if (CharactorList.SelectedItem is Charactor chara)
            {
                chara.Accessory2 = ChoiceEquipment(chara.Accessory2);
            }
        }

        private void ButtonItem_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button { DataContext: Item currentItem })
            {
                var items = ((DataContext)DataContext).Items;
                HashSet<uint> ownedItemIds = new();
                foreach (var item in items)
                {
                    if (item.ID != 0)
                        ownedItemIds.Add(item.ID);
                }

                var choiceItems = new List<NameValueInfo>();
                foreach (var item in Info.Instance().Items)
                {
                    if (!ownedItemIds.Contains(item.Value))
                    {
                        choiceItems.Add(item);
                    }
                }

                var window = new ItemChoiceWindow(currentItem.ID, choiceItems);
                window.ShowDialog();
                currentItem.ID = window.ID;
            }
        }

        private static uint ChoiceEquipment(uint id)
        {
            var window = new ItemChoiceWindow(id, Info.Instance().Equipments);
            window.ShowDialog();
            return window.ID;
        }

        private void MenuItemWeakProgress_Click(object sender, RoutedEventArgs e)
        {
            string text = "Completed : 0/0";
            if (DataContext != null)
            {
                var enermies = ((DataContext)DataContext)?.EnemyWeaknesses;
                if (enermies != null)
                {
                    int count = 0;

                    var builder = new StringBuilder(256);
                    foreach (var item in enermies)
                    {
                        if (item.IsBreakAll)
                            count++;
                    }

                    static void AppendNum(StringBuilder builder, string des, int completed, int total)
                    {
                        builder.Append(des);
                        builder.Append(" : ");
                        builder.Append(completed);
                        builder.Append('/');
                        builder.Append(total);
                        builder.AppendLine();
                    }

                    AppendNum(builder, "Completed", count, enermies.Count);
                    int uncompletedCount = enermies.Count - count;
                    if (uncompletedCount > 0)
                    {
                        AppendNum(builder, "Uncompleted", uncompletedCount, enermies.Count);
                    }
                    text = builder.ToString();
                }
            }

            MessageBox.Show(text, "Progress");
        }

        private void MenuItemWeakAll_Click(object sender, RoutedEventArgs e)
        {
            SetWeakFilter(0);
        }

        private void MenuItemWeakCompleted_Click(object sender, RoutedEventArgs e)
        {
            SetWeakFilter(1);
        }

        private void MenuItemWeakUncompleted_Click(object sender, RoutedEventArgs e)
        {
            SetWeakFilter(-1);
        }

        private void SetWeakFilter(int type)
        {
            MenuItemWeakAll.IsChecked = type == 0;
            MenuItemWeakCompleted.IsChecked = type > 0;
            MenuItemWeakUncompleted.IsChecked = type < 0;
            if (DataContext is not DataContext dataContext)
                return;

            var collectionView = CollectionViewSource.GetDefaultView(dataContext.EnemyWeaknesses);
            if (type > 0)
                collectionView.Filter = obj => ((EnemyWeakness)obj).IsBreakAll;
            else if (type < 0)
                collectionView.Filter = obj => !((EnemyWeakness)obj).IsBreakAll;
            else
                collectionView.Filter = null;
        }

        private void MenuItemTreasureStateProgress_Click(object sender, EventArgs e)
        {
            string text = "Completed : 0/0";
            if (DataContext != null)
            {
                var treasureStates = ((DataContext)DataContext)?.TreasureStates;
                if (treasureStates != null)
                {
                    uint completedChest = 0;
                    uint completedHiddenItem = 0;
                    uint totalChest = 0;
                    uint totalHiddenItem = 0;
                    uint completedCount = 0;

                    var builder = new StringBuilder(256);
                    foreach (var item in treasureStates)
                    {
                        var info = item.Info;

                        totalChest += info.Chest;
                        totalHiddenItem += info.HiddenItem;
                        completedChest += item.Chest;
                        completedHiddenItem += item.HiddenItem;
                        if (item.IsCollectAll)
                            completedCount++;
                    }

                    static void AppendNum(StringBuilder builder, string des, uint completed, uint total)
                    {
                        builder.Append(des);
                        builder.Append(" : ");
                        builder.Append(completed);
                        builder.Append('/');
                        builder.Append(total);
                        builder.AppendLine();
                    }

                    AppendNum(builder, "Completed", completedCount, (uint)treasureStates.Count);
                    uint uncompletedCount = (uint)treasureStates.Count - completedCount;
                    if (uncompletedCount > 0)
                    {
                        AppendNum(builder, "Uncompleted", uncompletedCount, (uint)treasureStates.Count);
                    }
                    AppendNum(builder, TreasureStatesSummation, completedChest + completedHiddenItem, totalChest + totalHiddenItem);
                    AppendNum(builder, TreasureStatesChest, completedChest, totalChest);
                    AppendNum(builder, TreasureStatesHiddenItem, completedHiddenItem, totalHiddenItem);
                    text = builder.ToString();
                }
            }

            MessageBox.Show(text, "Progress");
        }

        private int _itemInventoryFilter;

        private void ButtonItemInventoryFilter_Click(object sender, EventArgs e)
        {
            if (DataContext is not DataContext dataContext)
                return;

            var collectionView = CollectionViewSource.GetDefaultView(dataContext.BasicData.ItemInventoryStates);
            if (_itemInventoryFilter > 0)
            {
                _itemInventoryFilter = 0;
                collectionView.Filter = null;
            }
            else if (_itemInventoryFilter < 0)
            {
                _itemInventoryFilter = 1;
                collectionView.Filter = obj => ((ItemInventoryState)obj).IsGot;
            }
            else
            {
                _itemInventoryFilter = -1;
                collectionView.Filter = obj => !((ItemInventoryState)obj).IsGot;
            }
        }

        private void MenuItemTreasureStateAll_Click(object sender, RoutedEventArgs e)
        {
            SetTreasureStateFilter(0);
        }

        private void MenuItemTreasureStateCompleted_Click(object sender, RoutedEventArgs e)
        {
            SetTreasureStateFilter(1);
        }

        private void MenuItemTreasureStateUncompleted_Click(object sender, RoutedEventArgs e)
        {
            SetTreasureStateFilter(-1);
        }

        private void SetTreasureStateFilter(int type)
        {
            MenuItemTreasureStateAll.IsChecked = type == 0;
            MenuItemTreasureStateCompleted.IsChecked = type > 0;
            MenuItemTreasureStateUncompleted.IsChecked = type < 0;
            if (DataContext is not DataContext dataContext)
                return;

            var collectionView = CollectionViewSource.GetDefaultView(dataContext.TreasureStates);
            if (type > 0)
                collectionView.Filter = obj => ((TreasureState)obj).IsCollectAll;
            else if (type < 0)
                collectionView.Filter = obj => !((TreasureState)obj).IsCollectAll;
            else
                collectionView.Filter = null;
        }
    }
}
