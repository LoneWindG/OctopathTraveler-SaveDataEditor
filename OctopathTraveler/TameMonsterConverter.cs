using System;
using System.Globalization;
using System.Windows.Data;

namespace OctopathTraveler
{
	class TameMonsterConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			uint uintValue = (uint)value;
            for (int i = 0; i < Info.Instance().TameMonsters.Count; i++)
			{
				if (Info.Instance().TameMonsters[i].Value == uintValue) return i;
			}
			return -1;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			int index = (int)value;
			if (index < 0 || index >= Info.Instance().TameMonsters.Count) return -1;
			return Info.Instance().TameMonsters[index].Value;
		}
	}
}
