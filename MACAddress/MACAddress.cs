using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Net.NetworkInformation;

namespace System.Net
{
    public class MACAddress
    {
        private const long MAXLONGADDRESS = 281474976710655L;
        private const long MINLONGADDRESS = 0L;
        private byte[] _byte_address;

        public static MACAddress None
        {
            get 
            {
                return new MACAddress(0L);
            }
        }

        [Obsolete("This property has been deprecated. It is address family dependent. Please use MACAddress.Equals method to perform comparisons. http://go.microsoft.com/fwlink/?linkid=14202")]
        public long Address
        {
            get
            {
                long _long_address = 0L;
                for (int i = 0; i < 6; i++)
                {
                    _long_address += Convert.ToInt64(_byte_address[i] * Math.Pow(256, i));
                }
                return _long_address;
            }

            set
            {
                if ((value < MINLONGADDRESS) || (value > MAXLONGADDRESS)) throw new System.ArgumentOutOfRangeException("Заданный аргумент находится вне диапазона допустимых значений.");
                this._byte_address = BitConverter.GetBytes(value);
            }
        }

        public MACAddress(byte[] address)
        {
            if (address == null) throw new System.ArgumentNullException("Значение не может быть неопределенным.");
            if (address.Length != 6) throw new System.ArgumentException("Указан недопустимый MAC адрес.");
            this._byte_address = address;
        }

        public MACAddress(long newAddress)
        {
            if ((newAddress < MINLONGADDRESS) || (newAddress > MAXLONGADDRESS)) throw new System.ArgumentOutOfRangeException("Заданный аргумент находится вне диапазона допустимых значений.");
            this._byte_address = BitConverter.GetBytes(newAddress);
        }

        public byte[] GetAddressBytes()
        {
            return this._byte_address;
        }

        public static MACAddress Parse(string macString)
        {
            if (macString == null) throw new System.ArgumentNullException("Значение не может быть неопределенным.");
            if (macString == string.Empty) throw new System.FormatException("Указан недопустимый MAC адрес.");
            Regex rx = new Regex(@"[0-9A-F]{2}[: .-]{1}[0-9A-F]{2}[: .-]{1}[0-9A-F]{2}[: .-]{1}[0-9A-F]{2}[: .-]{1}[0-9A-F]{2}[: .-]{1}[0-9A-F]{2}");
            Match mymatch = rx.Match((macString = macString.ToUpper()));
            if (!mymatch.Success) throw new System.FormatException("Указан недопустимый MAC адрес.");
            if (macString != mymatch.Groups[0].Value.ToString()) throw new System.FormatException("Указан недопустимый MAC адрес.");
            List<byte> bAddress = new List<byte>();
            string[] SplitAddress = macString.Split(new Char[] { ':', '-' });
            foreach (string sSplitAddress in SplitAddress)
            {
                bAddress.Add(Convert.ToByte(sSplitAddress, 16));
            }
            return new MACAddress(bAddress.ToArray());
        }

        public static bool TryParse(string macString, out MACAddress address)
        {
            address = null;
            if (macString == null) return false;
            if (macString == string.Empty) return false;
            Regex rx = new Regex(@"[0-9A-F]{2}[: .-]{1}[0-9A-F]{2}[: .-]{1}[0-9A-F]{2}[: .-]{1}[0-9A-F]{2}[: .-]{1}[0-9A-F]{2}[: .-]{1}[0-9A-F]{2}");
            Match mymatch = rx.Match((macString = macString.ToUpper()));
            if (mymatch.Success)
            {
                if (macString == mymatch.Groups[0].Value.ToString())
                {
                    address = MACAddress.Parse(macString);
                    return true;
                }
            }
            return false;
        }

        public override bool Equals(object comparand)
        {
            MACAddress _first = comparand as MACAddress;
            return (_first == null) ? false : this._byte_address.Equals(_first.GetAddressBytes());
        }

        public override int GetHashCode()
        {
            long _long_address = 0L;
            for (int i = 0; i < 6; i++)
            {
                _long_address += Convert.ToInt64(_byte_address[i] * Math.Pow(256, i));
            }

            return ((int)_long_address ^ (int)(_long_address >> 32));
        }

        public override string ToString()
        {
            string str_temp = "";
            for (int i = 0; i < 6; i++)
            {
                if (this._byte_address[i] < 0x10) str_temp += "0";
                str_temp += this._byte_address[i].ToString("X");
                if (i != 5) str_temp += ":";
            }
            return str_temp;
        }
    }
}
