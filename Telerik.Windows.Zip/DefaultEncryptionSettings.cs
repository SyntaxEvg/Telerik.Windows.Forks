using System;

namespace Telerik.Windows.Zip
{
	public class DefaultEncryptionSettings : EncryptionSettings
	{
		public DefaultEncryptionSettings()
		{
			base.Algorithm = "DEFAULT";
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
				base.OnPropertyChanged("Password");
			}
		}

		internal uint FileTime { get; set; }

		string password;
	}
}
