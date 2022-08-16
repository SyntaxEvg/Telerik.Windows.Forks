using System;
using Telerik.Windows.Documents.Model.Drawing.Charts;

namespace Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Elements.Chart
{
	abstract class ChartGroupElementBase<T> : ChartGroupElementBase where T : SeriesGroup
	{
		public ChartGroupElementBase(OpenXmlPartsManager partsManager)
			: base(partsManager)
		{
		}

		public new T SeriesGroup
		{
			get
			{
				return base.SeriesGroup as T;
			}
			set
			{
				base.SeriesGroup = value;
			}
		}
	}
}
