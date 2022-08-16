using System;
using System.Collections.Generic;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Export;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Import;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Core;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Parts;
using Telerik.Windows.Documents.Model.Drawing.Shapes;

namespace Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Elements.Chart
{
	class ChartReferenceElement : ChartElementBase
	{
		public ChartReferenceElement(OpenXmlPartsManager partsManager)
			: base(partsManager)
		{
			this.relationId = base.RegisterAttribute<string>("id", OpenXmlNamespaces.OfficeDocumentRelationshipsNamespace, false);
		}

		public override string ElementName
		{
			get
			{
				return "chart";
			}
		}

		public override IEnumerable<OpenXmlNamespace> Namespaces
		{
			get
			{
				yield return OpenXmlNamespaces.OfficeDocumentRelationshipsNamespace;
				yield break;
			}
		}

		public string RelationshipId
		{
			get
			{
				return this.relationId.Value;
			}
			set
			{
				this.relationId.Value = value;
			}
		}

		public void CopyPropertiesFrom(IOpenXmlExportContext context, ChartShape chart)
		{
			ChartPart chartPartForChart = context.GetChartPartForChart(chart);
			this.RelationshipId = base.PartsManager.CreateRelationship(base.Part.Name, chartPartForChart.Name, OpenXmlRelationshipTypes.ChartRelationshipType, null);
		}

		public void CopyPropertiesTo(IOpenXmlImportContext context, ChartShape chartShape)
		{
			string relationshipTarget = base.PartsManager.GetRelationshipTarget(base.Part.Name, this.RelationshipId);
			string resourceName = base.Part.GetResourceName(relationshipTarget);
			ChartPart part = base.PartsManager.GetPart<ChartPart>(resourceName);
			context.RegisterChartPartForChart(chartShape, part);
		}

		readonly OpenXmlAttribute<string> relationId;
	}
}
