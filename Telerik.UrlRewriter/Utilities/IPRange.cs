using System;
using System.Net;
using System.Text.RegularExpressions;

namespace Telerik.UrlRewriter.Utilities
{
	public sealed class IPRange
	{
		public IPRange(IPAddress address)
		{
			this._minimumAddress = address;
			this._maximumAddress = address;
		}

		public IPRange(IPAddress minimumAddress, IPAddress maximumAddress)
		{
			if (IPRange.Compare(minimumAddress, maximumAddress) == -1)
			{
				this._minimumAddress = minimumAddress;
				this._maximumAddress = maximumAddress;
				return;
			}
			this._minimumAddress = maximumAddress;
			this._maximumAddress = minimumAddress;
		}

		public static IPRange Parse(string pattern)
		{
			pattern = Regex.Replace(pattern, "([0-9]{1,3}\\.[0-9]{1,3}\\.[0-9]{1,3})\\.\\*", "$1.0-$1.255");
			pattern = Regex.Replace(pattern, "([0-9]{1,3}\\.[0-9]{1,3})\\.\\*", "$1.0.0-$1.255.255");
			pattern = Regex.Replace(pattern, "([0-9]{1,3})\\.\\*", "$1.0.0.0-$1.255.255.255");
			string[] array = pattern.Split(new char[] { '-' });
			if (array.Length > 1)
			{
				return new IPRange(IPAddress.Parse(array[0].Trim()), IPAddress.Parse(array[1].Trim()));
			}
			return new IPRange(IPAddress.Parse(pattern.Trim()));
		}

		public bool InRange(IPAddress address)
		{
			return IPRange.Compare(this.MinimumAddress, address) <= 0 && IPRange.Compare(address, this.MaximumAddress) <= 0;
		}

		public IPAddress MinimumAddress
		{
			get
			{
				return this._minimumAddress;
			}
		}

		public IPAddress MaximumAddress
		{
			get
			{
				return this._maximumAddress;
			}
		}

		public static int Compare(IPAddress left, IPAddress right)
		{
			if (left == null)
			{
				throw new ArgumentNullException("left");
			}
			if (right == null)
			{
				throw new ArgumentNullException("right");
			}
			byte[] addressBytes = left.GetAddressBytes();
			byte[] addressBytes2 = right.GetAddressBytes();
			if (addressBytes.Length != addressBytes2.Length)
			{
				throw new ArgumentOutOfRangeException(MessageProvider.FormatString(Message.AddressesNotOfSameType, new object[0]));
			}
			for (int i = 0; i < addressBytes.Length; i++)
			{
				if (addressBytes[i] < addressBytes2[i])
				{
					return -1;
				}
				if (addressBytes[i] > addressBytes2[i])
				{
					return 1;
				}
			}
			return 0;
		}

		IPAddress _minimumAddress;

		IPAddress _maximumAddress;
	}
}
