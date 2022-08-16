using System;

namespace Telerik.Windows.Documents.Flow.Model.Protection
{
	public class ProtectionSettings
	{
		internal ProtectionSettings()
		{
			this.ProtectionMode = ProtectionSettings.DefaultProtectionMode;
			this.Enforced = ProtectionSettings.DefaultEnforced;
			this.AlgorithmName = ProtectionSettings.DefaultAlgorithmName;
			this.Hash = ProtectionSettings.DefaultHash;
			this.Salt = ProtectionSettings.DefaultSalt;
			this.SpinCount = ProtectionSettings.DefaultSpinCount;
		}

		public ProtectionMode ProtectionMode { get; set; }

		public bool Enforced { get; set; }

		public string AlgorithmName { get; set; }

		public string Salt { get; set; }

		public string Hash { get; set; }

		public int SpinCount { get; set; }

		internal ProtectionSettings Clone()
		{
			return new ProtectionSettings
			{
				ProtectionMode = this.ProtectionMode,
				Enforced = this.Enforced,
				AlgorithmName = this.AlgorithmName,
				Salt = this.Salt,
				Hash = this.Hash,
				SpinCount = this.SpinCount
			};
		}

		static readonly ProtectionMode DefaultProtectionMode = ProtectionMode.NoProtection;

		static readonly bool DefaultEnforced = false;

		static readonly string DefaultAlgorithmName = "SHA-512";

		static readonly string DefaultHash = string.Empty;

		static readonly string DefaultSalt = string.Empty;

		static readonly int DefaultSpinCount = 100000;
	}
}
