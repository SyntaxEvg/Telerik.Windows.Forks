using System;

namespace Telerik.Windows.Documents.Spreadsheet.FormatProviders.Contexts
{
	class ProtectionInfoBase
	{
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

		public string HashValue
		{
			get
			{
				return this.hashValue;
			}
			set
			{
				this.hashValue = value;
			}
		}

		public string SaltValue
		{
			get
			{
				return this.saltValue;
			}
			set
			{
				this.saltValue = value;
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

		bool enforced;

		string algorithmName;

		string hashValue;

		string saltValue;

		int spinCount;

		string password;
	}
}
