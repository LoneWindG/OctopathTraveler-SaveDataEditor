using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace OctopathTraveler
{
    /// <summary>
    /// ItemChoiceWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class ItemChoiceWindow : Window
	{
		public uint ID { get; set; }

		private List<NameValueInfo> ChoiceItems { get; }

        internal ItemChoiceWindow(uint id, List<NameValueInfo> choiceItems)
		{
			ID = id;
            ChoiceItems = choiceItems;
			InitializeComponent();
        }

		private void Window_Loaded(object sender, RoutedEventArgs e)
		{
			CreateItemList("");
			if (ID == 0) return;
            foreach (var item in ListBoxItem.Items)
			{
				NameValueInfo? info = item as NameValueInfo;
				if (info?.Value == ID)
				{
					ListBoxItem.SelectedItem = item;
					ListBoxItem.ScrollIntoView(item);
					break;
				}
			}
		}

		private void TextBoxFilter_TextChanged(object sender, TextChangedEventArgs e)
		{
			CreateItemList(TextBoxFilter.Text);
		}

		private void ListBoxItem_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			ButtonDecision.IsEnabled = ListBoxItem.SelectedIndex >= 0;
		}

		private void ButtonDecision_Click(object sender, RoutedEventArgs e)
		{
            if (ListBoxItem.SelectedItem is not NameValueInfo info) return;
            ID = info.Value;
			Close();
		}

		private void CreateItemList(string filter)
		{
			ListBoxItem.Items.Clear();
			if (ChoiceItems == null || ChoiceItems.Count == 0) return;
            if (string.IsNullOrWhiteSpace(filter))
			{
                foreach (var item in ChoiceItems)
                {
					ListBoxItem.Items.Add(item);
                }
            }
			else
			{
				if (uint.TryParse(filter, out var filterId))
				{
                    foreach (var item in ChoiceItems)
                    {
                        if (item.Value == filterId)
                        {
                            ListBoxItem.Items.Add(item);
                        }
                    }
				}
				else
				{
                    foreach (var item in ChoiceItems)
                    {
                        if (item.Name.Contains(filter, StringComparison.OrdinalIgnoreCase))
                        {
                            ListBoxItem.Items.Add(item);
                        }
                    }
                }
			}

		}
	}
}
