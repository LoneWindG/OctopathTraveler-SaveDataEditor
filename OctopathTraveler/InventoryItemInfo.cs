using System;
using System.Collections.Generic;

namespace OctopathTraveler
{
    class InventoryItemInfo : NameValueInfo
    {
        public int Type { get; private set; }

        public string TypeName { get; private set; } = string.Empty;

        public override bool CheckHeaderRow(IDictionary<string, object> row)
        {
            return base.CheckHeaderRow(row) && row.Count >= 4;
        }

        public override bool Parse(dynamic row)
        {
            if (!base.Parse((IDictionary<string, object>)row))
                return false;

            Type = Convert.ToInt32(row.C);
            TypeName = Convert.ToString(row.D);
            return true;
        }
    }
}
