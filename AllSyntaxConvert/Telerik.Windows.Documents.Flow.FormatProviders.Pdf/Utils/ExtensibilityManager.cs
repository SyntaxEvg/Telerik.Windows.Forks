using System;
using Telerik.Windows.Documents.Flow.Model.Lists;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Pdf.Utils
{
	public class ExtensibilityManager
	{
		internal ExtensibilityManager()
		{
			this.listHelper = new ListHelper();
		}

		internal ListHelper ListHelper
		{
			get
			{
				return this.listHelper;
			}
		}

		public void RegisterNumberingStyleConverter(NumberingStyle numberingStyle, INumberingStyleConverter converter)
		{
			this.ListHelper.RegisterNumberingStyle(numberingStyle, converter);
		}

		readonly ListHelper listHelper;
	}
}
