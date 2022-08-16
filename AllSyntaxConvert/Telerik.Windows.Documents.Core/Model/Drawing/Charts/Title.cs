using System;

namespace Telerik.Windows.Documents.Model.Drawing.Charts
{
	public abstract class Title
	{
		public abstract TitleType TitleType { get; }

		public abstract Title Clone();
	}
}
