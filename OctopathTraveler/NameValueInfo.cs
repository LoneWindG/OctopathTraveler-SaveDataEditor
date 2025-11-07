using System;
using System.Collections.Generic;

namespace OctopathTraveler
{
    class NameValueInfo : IRowParser
    {
        public uint Value { get; set; }
        public string Name { get; set; } = string.Empty;

        public virtual bool CheckHeaderRow(IDictionary<string, object> row)
        {
            if (row.Count < 2 || !row.TryGetValue("A", out var id))
                return false;

            if (id is string content && (content == "#ID" || content == "ID"))
                return true;

            return false;
        }

        public virtual bool Parse(dynamic row)
        {
            if (row.A == null)
                return false;

            var cell0 = row.A.ToString() ?? string.Empty;
            if (cell0.Length >= 3 && cell0[0] == '0' && cell0[1] == 'x')
            {
                Value = Convert.ToUInt32(cell0, 16);
            }
            else
            {
                try
                {
                    Value = Convert.ToUInt32(cell0);
                }
                catch (Exception)
                {

                    throw;
                }
            }
            Name = row.B?.ToString() ?? string.Empty;
            return !string.IsNullOrWhiteSpace(Name);
        }
    }
}
