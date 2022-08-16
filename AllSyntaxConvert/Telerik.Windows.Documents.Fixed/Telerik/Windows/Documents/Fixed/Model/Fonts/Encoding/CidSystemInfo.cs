using System;

namespace Telerik.Windows.Documents.Fixed.Model.Fonts.Encoding
{
	class CidSystemInfo
	{
		public CidSystemInfo(string registry, string ordering, int supplement)
		{
			this.registry = registry;
			this.ordering = ordering;
			this.supplement = supplement;
		}

		public static CidSystemInfo Default
		{
			get
			{
				return CidSystemInfo.defaultValue;
			}
		}

		public string Registry
		{
			get
			{
				return this.registry;
			}
		}

		public string Ordering
		{
			get
			{
				return this.ordering;
			}
		}

		public int Supplement
		{
			get
			{
				return this.supplement;
			}
		}

		static readonly CidSystemInfo defaultValue = new CidSystemInfo("Adobe", "Identity", 0);

		readonly string registry;

		readonly string ordering;

		readonly int supplement;
	}
}
