using System;
using System.Collections.Generic;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Export;

namespace Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Elements.Chart
{
	abstract class LiteralsDataElementBase : ChartElementBase
	{
		public LiteralsDataElementBase(OpenXmlPartsManager partsManager)
			: base(partsManager)
		{
		}

		protected override IEnumerable<OpenXmlElementBase> EnumerateChildElements(IOpenXmlExportContext context)
		{
			int pointIndex = 0;
			foreach (string literal in this.EnumerateDataAsString())
			{
				DataPointElement dataPointElement = base.CreateElement<DataPointElement>("pt");
				dataPointElement.CopyPropertiesFrom(literal);
				dataPointElement.Index = pointIndex;
				pointIndex++;
				yield return dataPointElement;
			}
			yield break;
		}

		protected abstract IEnumerable<string> EnumerateDataAsString();
	}
}
