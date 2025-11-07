using System.Collections.Generic;
using System.Linq;

namespace OctopathTraveler
{
	static class Util
	{
		public static void WriteNumber(uint? address, uint size, uint value, uint min, uint max)
		{
			if (value < min) value = min;
			if (value > max) value = max;
			SaveData.Instance().WriteNumber(address, size, value);
		}

		public static uint? FindFirstAddress(string name, uint? startAddress, bool required = true)
		{
            var addresses = SaveData.Instance().FindAddress(name, startAddress ?? 0, true);
			if (addresses.Count > 0)
				return addresses[0];

			return required ? throw new KeyNotFoundException($"Address for '{name}' not found.") : null;
        }

		public static uint ReadNumber(this GVASData gvasData)
		{
			return SaveData.Instance().ReadNumber(gvasData.Address, gvasData.Size);
		}

		public static uint ReadNumber(this GVAS gvas, string key)
		{
			return ReadNumber(gvas.Key(key));
		}

		public static uint ReadNumberOrDefault(this GVAS gvas, string key, uint defaultValue = default)
		{
			if (!gvas.HasKey(key))
				return defaultValue;

            return ReadNumber(gvas.Key(key));
        }
	}
}
