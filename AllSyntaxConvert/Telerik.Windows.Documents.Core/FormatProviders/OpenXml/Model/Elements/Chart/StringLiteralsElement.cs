using System;
using System.Collections.Generic;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Export;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Core;
using Telerik.Windows.Documents.Model.Drawing.Charts;

namespace Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Elements.Chart
{
	class StringLiteralsElement : LiteralsDataElementBase
	{
		public StringLiteralsElement(OpenXmlPartsManager partsManager)
			: base(partsManager)
		{
		}

		public override string ElementName
		{
			get
			{
				return "strLit";
			}
		}

		protected override bool ShouldExport(IOpenXmlExportContext context)
		{
			return this.data != null;
		}

		public void CopyPropertiesFrom(StringChartData stringChartData)
		{
			this.data = stringChartData;
		}

		protected override IEnumerable<string> EnumerateDataAsString()
		{
			foreach (string dataPoint in this.data.StringLiterals)
			{
				yield return dataPoint;
			}
			yield break;
		}

		public IChartData CreateLiteralChartData()
		{
			List<string> list = new List<string>();
			foreach (IOpenXmlChildElement openXmlChildElement in base.ChildElements)
			{
				OpenXmlElementBase element = openXmlChildElement.GetElement();
				if (element.ElementName == "pt")
				{
					DataPointElement dataPointElement = element as DataPointElement;
					list.Add(dataPointElement.ValueElement.InnerText);
				}
			}
			return new StringChartData(list);
		}

		StringChartData data;
	}
}
