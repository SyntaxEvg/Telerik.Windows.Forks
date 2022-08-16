using System;
using Telerik.Windows.Documents.Fixed.Model.Common;

namespace Telerik.Windows.Documents.Fixed.Model.Fonts.Encoding
{
	class SimpleEncoding : ISimpleEncoding
	{
		public SimpleEncoding()
		{
			this.predefinedEncoding = new PdfProperty<string>();
			this.differences = new PdfProperty<DifferencesRanges>();
		}

		public PdfProperty<string> PredefinedEncoding
		{
			get
			{
				return this.predefinedEncoding;
			}
		}

		public PdfProperty<DifferencesRanges> Differences
		{
			get
			{
				return this.differences;
			}
		}

		public bool IsNamedEncoding
		{
			get
			{
				return this.PredefinedEncoding.HasValue && !this.Differences.HasValue;
			}
		}

		public string BaseEncodingName
		{
			get
			{
				return this.PredefinedEncoding.Value;
			}
		}

		public string GetName(byte b)
		{
			this.EnsureNamesAreInitialized();
			return this.names[(int)b];
		}

		internal static string[] GetNamesWithoutDifferences(string baseEncoding)
		{
			PredefinedSimpleEncoding predefinedSimpleEncoding = PredefinedSimpleEncoding.GetPredefinedEncoding(baseEncoding);
			predefinedSimpleEncoding = predefinedSimpleEncoding ?? PredefinedSimpleEncoding.StandardEncoding;
			return predefinedSimpleEncoding.GetNames();
		}

		void EnsureNamesAreInitialized()
		{
			if (this.names == null)
			{
				this.names = SimpleEncoding.GetNamesWithoutDifferences(this.PredefinedEncoding.Value);
				this.CalculateDifferences();
			}
		}

		void CalculateDifferences()
		{
			if (this.Differences.HasValue)
			{
				foreach (object obj in this.Differences.Value)
				{
					DifferencesRange differencesRange = (DifferencesRange)obj;
					int num = (int)differencesRange.StartCharCode;
					foreach (string text in differencesRange.Differences)
					{
						if (num >= this.names.Length)
						{
							break;
						}
						this.names[num] = text;
						num++;
					}
				}
			}
		}

		readonly PdfProperty<string> predefinedEncoding;

		readonly PdfProperty<DifferencesRanges> differences;

		string[] names;
	}
}
