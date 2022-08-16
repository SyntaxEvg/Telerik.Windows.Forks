using System;

namespace Telerik.Windows.Documents.Flow.Model.Styles
{
	class BuiltInStyleMetaData
	{
		public StyleType StyleType { get; set; }

		public bool IsPrimary { get; set; }

		public int UIPriority { get; set; }

		public CreateBuiltInStyleCallback CreateStyleMethod { get; set; }

		public Style GetStyle()
		{
			Style style = this.CreateStyleMethod(this.StyleType);
			style.IsPrimary = this.IsPrimary;
			style.UIPriority = this.UIPriority;
			return style;
		}
	}
}
