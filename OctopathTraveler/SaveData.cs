using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;

namespace OctopathTraveler
{
    class SaveData
    {
        public static bool IsReadonlyMode { get; } = ParseBoolConfig(Environment.GetEnvironmentVariable("READONLY_MODE"), false);

        private static SaveData? mThis;
        private byte[] mBuffer = Array.Empty<byte>();
        private readonly System.Text.Encoding mEncode = System.Text.Encoding.ASCII;
        public uint Adventure { private get; set; } = 0;
        public string FileName { get; private set; } = string.Empty;
        public string BackupFileName { get; private set; } = string.Empty;

        private SaveData()
        { }

        public static SaveData Instance()
        {
            return mThis ??= new SaveData();
        }

        private readonly Dictionary<char, List<uint>> _charIndex = new();

        public void Open(string filename)
        {
            mBuffer = File.ReadAllBytes(filename);
            FileName = filename;
            Backup();
            BuildCharIndex();
        }

        private void BuildCharIndex()
        {
            _charIndex.Clear();
            if (mBuffer == null || mBuffer.Length == 0) return;

            int len = mBuffer.Length;
            for (uint i = 0; i < len; i++)
            {
                byte b = mBuffer[i];
                if ((b >= 'a' && b <= 'z') || (b >= 'A' && b <= 'Z'))
                {
                    char c = (char)b;
                    if (!_charIndex.TryGetValue(c, out var list))
                    {
                        list = new List<uint>();
                        _charIndex.Add(c, list);
                    }
                    list.Add(i);
                }
            }
        }

        public bool Save()
        {
            if (IsReadonlyMode || string.IsNullOrEmpty(FileName) || mBuffer == null || mBuffer.Length <= 0) return false;
            File.WriteAllBytes(FileName, mBuffer);
            return true;
        }

        public bool SaveAs(string filenname)
        {
            if (mBuffer == null || mBuffer.Length <= 0) return false;
            if (string.Equals(FileName, filenname, StringComparison.OrdinalIgnoreCase)) return false;
            File.WriteAllBytes(filenname, mBuffer);
            return true;
        }

        public (bool, Exception?) SaveAsJson(string filePath)
        {
            if (mBuffer == null) return (false, null);
            return GvasFormat.GvasConverter.Convert2JsonFile(filePath, new MemoryStream(mBuffer, false));
        }

        public uint ReadNumber(uint? address, uint size)
        {
            if (address == null) return 0;

            if (mBuffer == null) return 0;
            address = CalcAddress(address.Value);
            if (address + size > mBuffer.Length) return 0;
            uint result = 0;
            for (int i = 0; i < size; i++)
            {
                result += (uint)(mBuffer[address.Value + i]) << (i * 8);
            }
            return result;
        }

        // 0 to 7.
        public bool ReadBit(uint address, uint bit)
        {
            if (bit < 0) return false;
            if (bit > 7) return false;
            if (mBuffer == null) return false;
            address = CalcAddress(address);
            if (address > mBuffer.Length) return false;
            byte mask = (byte)(1 << (int)bit);
            byte result = (byte)(mBuffer[address] & mask);
            return result != 0;
        }

        public string ReadText(uint address, uint size)
        {
            if (mBuffer == null) return "";
            address = CalcAddress(address);
            if (address + size > mBuffer.Length) return "";

            byte[] tmp = new byte[size];
            for (uint i = 0; i < size; i++)
            {
                //if (mBuffer[address + i] == 0) break;
                tmp[i] = mBuffer[address + i];
            }
            return mEncode.GetString(tmp).Trim('\0');
        }

        public void WriteNumber(uint? address, uint size, uint value)
        {
            if (address == null) return;

            if (mBuffer == null) return;
            address = CalcAddress(address.Value);
            if (address + size > mBuffer.Length) return;
            for (uint i = 0; i < size; i++)
            {
                mBuffer[address.Value + i] = (byte)(value & 0xFF);
                value >>= 8;
            }
        }

        // 0 to 7.
        public void WriteBit(uint address, uint bit, bool value)
        {
            if (bit < 0) return;
            if (bit > 7) return;
            if (mBuffer == null) return;
            address = CalcAddress(address);
            if (address > mBuffer.Length) return;
            byte mask = (byte)(1 << (int)bit);
            if (value) mBuffer[address] = (byte)(mBuffer[address] | mask);
            else mBuffer[address] = (byte)(mBuffer[address] & ~mask);
        }

        public void WriteText(uint address, uint size, string value)
        {
            if (mBuffer == null) return;
            address = CalcAddress(address);
            if (address + size > mBuffer.Length) return;
            byte[] tmp = mEncode.GetBytes(value);
            Array.Resize(ref tmp, (int)size);
            for (uint i = 0; i < size; i++)
            {
                mBuffer[address + i] = tmp[i];
            }
        }

        public void Fill(uint address, uint size, byte number)
        {
            if (mBuffer == null) return;
            address = CalcAddress(address);
            if (address + size > mBuffer.Length) return;
            for (uint i = 0; i < size; i++)
            {
                mBuffer[address + i] = number;
            }
        }

        public void Copy(uint from, uint to, uint size)
        {
            if (mBuffer == null) return;
            from = CalcAddress(from);
            to = CalcAddress(to);
            if (from + size > mBuffer.Length) return;
            if (to + size > mBuffer.Length) return;
            for (uint i = 0; i < size; i++)
            {
                mBuffer[to + i] = mBuffer[from + i];
            }
        }

        public void Swap(uint from, uint to, uint size)
        {
            if (mBuffer == null) return;
            from = CalcAddress(from);
            to = CalcAddress(to);
            if (from + size > mBuffer.Length) return;
            if (to + size > mBuffer.Length) return;
            for (uint i = 0; i < size; i++)
            {
                byte tmp = mBuffer[to + i];
                mBuffer[to + i] = mBuffer[from + i];
                mBuffer[from + i] = tmp;
            }
        }

        public IList<uint> FindAddress(string name, uint startAddress, bool onlyFirst = false)
        {
            if (mBuffer == null || string.IsNullOrWhiteSpace(name)) return Array.Empty<uint>();

            char firstChar = name[0];
            if (!_charIndex.TryGetValue(firstChar, out var candidates))
            {
                return FindAddressSlow(name, startAddress, onlyFirst);
            }

            IList<uint>? result = null;
            ReadOnlySpan<byte> nameSpan = System.Text.Encoding.ASCII.GetBytes(name);
            int nameLen = nameSpan.Length;
            int bufferLen = mBuffer.Length;

            foreach (var addr in candidates)
            {
                if (addr < startAddress) continue;
                if (addr + nameLen > bufferLen) continue;
                if (new ReadOnlySpan<byte>(mBuffer, (int)addr, nameLen).SequenceEqual(nameSpan))
                {
                    if (onlyFirst)
                    {
                        result = new uint[1] { addr };
                        break;
                    }
                    result ??= new List<uint>();
                    result.Add(addr);
                }
            }
            return result ?? Array.Empty<uint>();
        }

        private IList<uint> FindAddressSlow(string name, uint startAddress, bool onlyFirst = false)
        {
            if (string.IsNullOrWhiteSpace(name) || mBuffer == null) return Array.Empty<uint>();

            IList<uint>? result = null;
            ReadOnlySpan<byte> nameSpan = System.Text.Encoding.ASCII.GetBytes(name);
            int nameLen = nameSpan.Length;
            int bufferLen = mBuffer.Length;

            uint addr = startAddress;
            while (addr <= bufferLen - nameLen)
            {
                if (mBuffer[addr] != nameSpan[0])
                {
                    addr++;
                    continue;
                }

                int len = 1;
                for (; len < nameLen; len++)
                {
                    if (mBuffer[addr + len] != nameSpan[len]) break;
                }
                if (len >= nameLen)
                {
                    if (onlyFirst)
                    {
                        result = new uint[1] { addr };
                        break;
                    }
                    result ??= new List<uint>();
                    result.Add(addr);
                }
                addr += (uint)len;
            }
            return result ?? Array.Empty<uint>();
        }

        private uint CalcAddress(uint address)
        {
            return address;
        }

        private void Backup()
        {
            if (IsReadonlyMode)
                return;

            if (mBuffer == null || mBuffer.Length <= 0) return;

            string path = Path.Combine(Directory.GetCurrentDirectory(), "OctopathTraveler Backup");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            var hash = MD5.HashData(mBuffer);
            path = Path.Combine(path, $"{Path.GetFileName(FileName)} - {Convert.ToHexString(hash)}");
            File.WriteAllBytes(path, mBuffer);
            BackupFileName = path;
        }

        private static bool ParseBoolConfig(string? value, bool defaultValue = false)
        {
            if (string.IsNullOrWhiteSpace(value))
                return defaultValue;

            var v = value.Trim().ToLowerInvariant();
            switch (v)
            {
                case "1":
                case "true":
                case "yes":
                case "y":
                case "on":
                case "enable":
                case "enabled":
                    return true;
                case "0":
                case "false":
                case "no":
                case "n":
                case "off":
                case "disable":
                case "disabled":
                    return false;
            }

            return defaultValue;
        }
    }
}
