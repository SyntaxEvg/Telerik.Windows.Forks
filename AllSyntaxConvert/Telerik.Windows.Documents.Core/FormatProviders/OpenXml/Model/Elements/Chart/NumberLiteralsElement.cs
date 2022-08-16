using System;
using System.Collections.Generic;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Export;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Import;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Core;
using Telerik.Windows.Documents.Model.Drawing.Charts;

namespace Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Elements.Chart
{
	class NumberLiteralsElement : LiteralsDataElementBase
	{
		public NumberLiteralsElement(OpenXmlPartsManager partsManager)
			: base(partsManager)
		{
			this.dataPoint = base.RegisterChildElement<DataPointElement>("pt");
		}

		public override string ElementName
		{
			get
			{
				return "numLit";
			}
		}

		public DataPointElement DataPointElement
		{
			get
			{
				return this.dataPoint.Element;
			}
			set
			{
				this.dataPoint.Element = value;
			}
		}

		protected override bool ShouldExport(IOpenXmlExportContext context)
		{
			return this.data != null;
		}

		public void CopyPropertiesFrom(NumericChartData numericChartData)
		{
			this.data = numericChartData;
		}

		protected override IEnumerable<string> EnumerateDataAsString()
		{
			foreach (double num in this.data.NumericLiterals)
			{
				double dataPoint = num;
				double num2 = dataPoint;
				yield return num2.ToString();
			}
			yield break;
		}

		protected override void OnAfterReadChildElement(IOpenXmlImportContext context, OpenXmlElementBase childElement)
		{
			base.OnAfterReadChildElement(context, childElement);
			DataPointElement dataPointElement = childElement as DataPointElement;
			if (dataPointElement != null)
			{
				if (this.dataPoints == null)
				{
					this.dataPoints = new List<double>();
				}
				this.dataPoints.Add(double.Parse(dataPointElement.ValueElement.InnerText));
				base.ReleaseElement(this.dataPoint);
			}
		}

		public IChartData CreateLiteralChartData()
		{
			NumericChartData result = new NumericChartData(this.dataPoints);
			this.dataPoints = null;
			return result;
		}

		readonly OpenXmlChildElement<DataPointElement> dataPoint;

		NumericChartData data;

		List<double> dataPoints;
	}
}
