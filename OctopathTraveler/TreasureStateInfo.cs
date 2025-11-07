using System;
using System.Collections.Generic;

namespace OctopathTraveler
{
    class TreasureStateInfo : NameValueInfo
    {
        public uint Summation { get; private set; }
        public uint Chest { get; private set; }
        public uint HiddenItem { get; private set; }

        public override bool CheckHeaderRow(IDictionary<string, object> row)
        {
            return base.CheckHeaderRow(row) && row.Count >= 3;
        }

        public override bool Parse(dynamic row)
        {
            if (!base.Parse((IDictionary<string, object>)row))
                return false;

            string num = row.C?.ToString() ?? string.Empty;
            if (num == "0" || string.IsNullOrWhiteSpace(num))
            {
                Summation = 0;
                Chest = 0;
                HiddenItem = 0;
                return true;
            }

            Summation = Convert.ToUInt32(num);

            num = row.D?.ToString() ?? string.Empty;
            Chest = string.IsNullOrWhiteSpace(num) ? 0 : Convert.ToUInt32(num);

            num = row.E?.ToString() ?? string.Empty;
            HiddenItem = string.IsNullOrWhiteSpace(num) ? 0 : Convert.ToUInt32(num);
            if (Summation != Chest + HiddenItem)
            {
                string msg = $"Bad treasure state line, ID: {Value}, Name: {Name}, " +
                    $"Sumation: {Summation}, Chest: {Chest}, HidenItem: {HiddenItem}, plase check excel file";
                throw new ArgumentException(msg);
            }
            return true;
        }
    }
}
