using System;

namespace Telerik.Windows.Documents.Spreadsheet.Model.Protection
{
	class ProtectionData
	{
		public ProtectionData()
		{
			this.AlgorithmName = ProtectionData.DefaultProtectionAlgorithm;
			this.SpinCount = 100000;
			this.Salt = SpreadsheetProtectionHelper.Instance.GenerateSaltBase64();
		}

		public bool Enforced
		{
			get
			{
				return this.enforced;
			}
			set
			{
				this.enforced = value;
			}
		}

		public string AlgorithmName
		{
			get
			{
				return this.algorithmName;
			}
			set
			{
				this.algorithmName = value;
			}
		}

		public string Hash
		{
			get
			{
				return this.hash;
			}
			set
			{
				this.hash = value;
			}
		}

		public string Salt
		{
			get
			{
				return this.salt;
			}
			set
			{
				this.salt = value;
			}
		}

		public int SpinCount
		{
			get
			{
				return this.spinCount;
			}
			set
			{
				this.spinCount = value;
			}
		}

		public string Password
		{
			get
			{
				return this.password;
			}
			set
			{
				this.password = value;
			}
		}

		public bool IsUsingLegacyProtectionScheme
		{
			get
			{
				return !string.IsNullOrEmpty(this.password);
			}
		}

		public bool RequiresUserPassword
		{
			get
			{
				return !string.IsNullOrEmpty(this.Hash) || !string.IsNullOrEmpty(this.Password);
			}
		}

		public bool TryRemoveProtection(string input)
		{
			bool flag;
			if (this.IsUsingLegacyProtectionScheme)
			{
				if (string.IsNullOrEmpty(input))
				{
					flag = string.IsNullOrEmpty(this.Password);
				}
				else
				{
					string b = SpreadsheetProtectionHelper.Instance.GeneratePasswordLegacyScheme(input);
					flag = string.Equals(this.Password, b, StringComparison.Ordinal);
				}
			}
			else
			{
				flag = SpreadsheetProtectionHelper.Instance.IsPasswordCorrect(input, this.Hash, this.Salt, this.AlgorithmName, this.SpinCount);
			}
			if (flag)
			{
				this.RemoveProtection();
			}
			return flag;
		}

		public void EnforceProtection(string input)
		{
			this.Enforced = true;
			if (!string.IsNullOrEmpty(input))
			{
				if (string.IsNullOrEmpty(this.AlgorithmName))
				{
					this.AlgorithmName = ProtectionData.DefaultProtectionAlgorithm;
				}
				if (this.SpinCount < 1)
				{
					this.SpinCount = 100000;
				}
				this.Salt = SpreadsheetProtectionHelper.Instance.GenerateSaltBase64();
				this.hash = SpreadsheetProtectionHelper.Instance.GenerateHashBase64(input, this.Salt, this.AlgorithmName, this.SpinCount);
			}
		}

		void RemoveProtection()
		{
			this.enforced = false;
			this.hash = null;
			this.salt = null;
			this.password = null;
		}

		internal void CopyFrom(ProtectionData fromProtectionData)
		{
			this.AlgorithmName = fromProtectionData.AlgorithmName;
			this.Enforced = fromProtectionData.Enforced;
			this.Hash = fromProtectionData.Hash;
			this.Salt = fromProtectionData.Salt;
			this.SpinCount = fromProtectionData.SpinCount;
			this.Password = fromProtectionData.Password;
		}

		const int DefaultSpinCount = 100000;

		internal static string DefaultProtectionAlgorithm = "SHA-512";

		bool enforced;

		string algorithmName;

		string hash;

		string salt;

		int spinCount;

		string password;
	}
}
