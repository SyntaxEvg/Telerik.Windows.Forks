using System;
using System.Text;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Common.Model.Protection
{
	abstract class ProtectionHelperBase
	{
		internal virtual string GenerateSaltBase64()
		{
			byte[] array = new byte[ProtectionHelperBase.saltLength];
			ProtectionHelperBase.random.NextBytes(array);
			return Convert.ToBase64String(array);
		}

		internal virtual string GenerateHashBase64(string password, string salt, string algorithmName, int spinCount)
		{
			Guard.ThrowExceptionIfNullOrEmpty(password, "password");
			Guard.ThrowExceptionIfNullOrEmpty(salt, "salt");
			Guard.ThrowExceptionIfNullOrEmpty(algorithmName, "algorithmName");
			Guard.ThrowExceptionIfLessThan<int>(1, spinCount, "spinCount");
			password = this.PreprocessPassword(password);
			byte[] bytes = Encoding.Unicode.GetBytes(string.Format("{0:X8}", password));
			IProtectionAlgorithm protectionAlgorithm = HashingAlgorithmsProvider.GetProtectionAlgorithm(algorithmName);
			byte[] array = ProtectionHelperBase.Concatenate(Convert.FromBase64String(salt), bytes);
			for (int i = 0; i < spinCount; i++)
			{
				array = protectionAlgorithm.ComputeHash(array);
				array = ProtectionHelperBase.Concatenate(array, i);
			}
			array = protectionAlgorithm.ComputeHash(array);
			return Convert.ToBase64String(array);
		}

		protected virtual string PreprocessPassword(string password)
		{
			return password;
		}

		internal virtual bool IsPasswordCorrect(string password, string hash, string salt, string algorithmName, int spinCount)
		{
			bool result = false;
			if (string.IsNullOrEmpty(hash) && string.IsNullOrEmpty(password))
			{
				result = true;
			}
			else if (!string.IsNullOrEmpty(hash) && !string.IsNullOrEmpty(password))
			{
				string b = this.GenerateHashBase64(password, salt, algorithmName, spinCount);
				result = string.Equals(hash, b, StringComparison.Ordinal);
			}
			return result;
		}

		static byte[] Concatenate(byte[] a, int b)
		{
			byte[] bytes = BitConverter.GetBytes(b);
			byte[] array = new byte[a.Length + bytes.Length];
			Array.Copy(a, array, a.Length);
			array[a.Length] = bytes[0];
			array[a.Length + 1] = bytes[1];
			array[a.Length + 2] = bytes[2];
			array[a.Length + 3] = bytes[3];
			return array;
		}

		static byte[] Concatenate(byte[] a, byte[] b)
		{
			byte[] array = new byte[a.Length + b.Length];
			Array.Copy(a, array, a.Length);
			Array.Copy(b, 0, array, a.Length, b.Length);
			return array;
		}

		static readonly int saltLength = 16;

		static readonly Random random = new Random();
	}
}
