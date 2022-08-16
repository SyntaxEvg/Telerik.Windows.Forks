using System;
using System.ComponentModel;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Utils;

namespace Telerik.Windows.Documents.Fixed.FormatProviders
{
	[TypeConverter(typeof(FormatProviderSettingsTypeConverter))]
	public class FormatProviderSettings
	{
		public static FormatProviderSettings ReadAllAtOnce
		{
			get
			{
				return new FormatProviderSettings(ReadingMode.AllAtOnce);
			}
		}

		public static FormatProviderSettings ReadOnDemand
		{
			get
			{
				return new FormatProviderSettings(ReadingMode.OnDemand);
			}
		}

		public FormatProviderSettings(ReadingMode mode)
		{
			this.ReadingMode = mode;
		}

		public ReadingMode ReadingMode { get; set; }
	}
}
