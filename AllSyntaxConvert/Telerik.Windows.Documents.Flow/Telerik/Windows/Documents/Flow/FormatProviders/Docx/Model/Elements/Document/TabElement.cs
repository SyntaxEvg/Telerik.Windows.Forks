using System;
using Telerik.Windows.Documents.Flow.FormatProviders.Docx.Export;
using Telerik.Windows.Documents.Flow.FormatProviders.Docx.Model.Types;
using Telerik.Windows.Documents.Flow.Model.Styles;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Core;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Docx.Model.Elements.Document
{
	class TabElement : DocumentElementBase
	{
		public TabElement(DocxPartsManager partsManager)
			: base(partsManager)
		{
			this.positionAttribute = base.RegisterAttribute<double>("pos", OpenXmlNamespaces.WordprocessingMLNamespace, false);
			this.valAttribute = base.RegisterAttribute<MappedOpenXmlAttribute<TabStopType>>(new MappedOpenXmlAttribute<TabStopType>("val", OpenXmlNamespaces.WordprocessingMLNamespace, TypeMappers.TabStopTypeMapper, false));
			this.leaderAttribute = base.RegisterAttribute<MappedOpenXmlAttribute<TabStopLeader>>(new MappedOpenXmlAttribute<TabStopLeader>("leader", OpenXmlNamespaces.WordprocessingMLNamespace, TypeMappers.TabStopLeaderMapper, false));
		}

		public override string ElementName
		{
			get
			{
				return "tab";
			}
		}

		public double Position
		{
			get
			{
				return this.positionAttribute.Value;
			}
			set
			{
				this.positionAttribute.Value = value;
			}
		}

		public TabStopType Type
		{
			get
			{
				return this.valAttribute.Value;
			}
			set
			{
				this.valAttribute.Value = value;
			}
		}

		public TabStopLeader Leader
		{
			get
			{
				return this.leaderAttribute.Value;
			}
			set
			{
				this.leaderAttribute.Value = value;
			}
		}

		protected override bool ShouldExport(IDocxExportContext context)
		{
			return true;
		}

		readonly OpenXmlAttribute<double> positionAttribute;

		readonly MappedOpenXmlAttribute<TabStopType> valAttribute;

		readonly MappedOpenXmlAttribute<TabStopLeader> leaderAttribute;
	}
}
