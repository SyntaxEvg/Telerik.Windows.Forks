using System;
using System.Collections.Generic;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Export;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Import;
using Telerik.Windows.Documents.Model.Drawing.Charts;

namespace Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Elements.Chart
{
	abstract class ChartGroupWithAxesElementBase<T> : ChartGroupElementBase<T> where T : SeriesGroup, ISupportAxes
	{
		public ChartGroupWithAxesElementBase(OpenXmlPartsManager partsManager)
			: base(partsManager)
		{
		}

		protected override IEnumerable<OpenXmlElementBase> EnumerateChildElements(IOpenXmlExportContext context)
		{
			foreach (OpenXmlElementBase element in base.EnumerateChildElements(context))
			{
				yield return element;
			}
			AxisIdElement horizontalAxisId = base.CreateElement<AxisIdElement>("axId");
			AxisIdElement axisIdElement = horizontalAxisId;
			T seriesGroup = base.SeriesGroup;
			axisIdElement.Value = ((seriesGroup.AxisGroupName == AxisGroupName.Primary) ? 0 : 2);
			yield return horizontalAxisId;
			AxisIdElement verticalAxisId = base.CreateElement<AxisIdElement>("axId");
			AxisIdElement axisIdElement2 = verticalAxisId;
			T seriesGroup2 = base.SeriesGroup;
			axisIdElement2.Value = ((seriesGroup2.AxisGroupName == AxisGroupName.Primary) ? 1 : 3);
			yield return verticalAxisId;
			yield break;
		}

		protected override void OnAfterReadChildElement(IOpenXmlImportContext context, OpenXmlElementBase childElement)
		{
			base.OnAfterReadChildElement(context, childElement);
			if (!(childElement.ElementName == "axId"))
			{
				return;
			}
			AxisIdElement axisIdElement = childElement as AxisIdElement;
			if (this.importedCatAxisId == null)
			{
				this.importedCatAxisId = new int?(axisIdElement.Value);
				return;
			}
			if (this.importedValAxisId == null)
			{
				this.importedValAxisId = new int?(axisIdElement.Value);
				return;
			}
			throw new InvalidOperationException("The number of axes for this seriesGroup is not valid.");
		}

		public override void CopyPropertiesTo(IOpenXmlImportContext context, SeriesGroup seriesGroup)
		{
			base.CopyPropertiesTo(context, seriesGroup);
			if (this.importedCatAxisId == null || this.importedValAxisId == null)
			{
				throw new InvalidOperationException("The number of axes for this seriesGroup is not valid.");
			}
			ISupportAxes seriesGroup2 = seriesGroup as ISupportAxes;
			context.RegisterSeriesGroupAwaitingAxisGroupName(seriesGroup2, this.importedCatAxisId.Value, this.importedValAxisId.Value);
		}

		protected override void ClearOverride()
		{
			base.ClearOverride();
			this.importedCatAxisId = null;
			this.importedValAxisId = null;
		}

		const int ObligatoryNumberOfAxes = 2;

		int? importedCatAxisId;

		int? importedValAxisId;
	}
}
