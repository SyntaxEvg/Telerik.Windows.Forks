using System;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Docx.Model.Types
{
	static class DocxConverters
	{
		public static TableLookConverter TableLookConverter
		{
			get
			{
				return DocxConverters.tableLookConverter;
			}
		}

		public static AutoColorConverter AutoColorConverter
		{
			get
			{
				return DocxConverters.autoColorConverter;
			}
		}

		static readonly TableLookConverter tableLookConverter = new TableLookConverter();

		static readonly AutoColorConverter autoColorConverter = new AutoColorConverter();
	}
}
