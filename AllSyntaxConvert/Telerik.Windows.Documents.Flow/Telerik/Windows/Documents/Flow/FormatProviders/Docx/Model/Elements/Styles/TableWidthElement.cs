using System;
using Telerik.Windows.Documents.Flow.FormatProviders.Docx.Export;
using Telerik.Windows.Documents.Flow.FormatProviders.Docx.Model.Elements.Document;
using Telerik.Windows.Documents.Flow.FormatProviders.Docx.Model.Types;
using Telerik.Windows.Documents.Flow.Model.Styles;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Core;
using Telerik.Windows.Documents.Media;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Docx.Model.Elements.Styles
{
	class TableWidthElement : DocumentElementBase
	{
		public TableWidthElement(DocxPartsManager partsManager, string elementName)
			: base(partsManager)
		{
			this.elementName = elementName;
			this.widthAttribute = base.RegisterAttribute<IntOpenXmlAttribute>(new IntOpenXmlAttribute("w", OpenXmlNamespaces.WordprocessingMLNamespace, true));
			this.widthTypeAttribute = base.RegisterAttribute<MappedOpenXmlAttribute<TableWidthUnitType>>(new MappedOpenXmlAttribute<TableWidthUnitType>("type", OpenXmlNamespaces.WordprocessingMLNamespace, TypeMappers.TableWidthTypeMapper, true));
		}

		public override string ElementName
		{
			get
			{
				return this.elementName;
			}
		}

		public TableWidthUnit Value
		{
			get
			{
				double num = Unit.TwipToDip((double)this.widthAttribute.Value);
				if (this.widthTypeAttribute.Value == TableWidthUnitType.Nil)
				{
					num = 0.0;
				}
				else if (this.widthTypeAttribute.Value == TableWidthUnitType.Percent)
				{
					num = (double)(this.widthAttribute.Value / 50);
				}
				else if (this.widthTypeAttribute.Value == TableWidthUnitType.Auto && num == 0.0)
				{
					return new TableWidthUnit(TableWidthUnitType.Auto);
				}
				return new TableWidthUnit(this.widthTypeAttribute.Value, num);
			}
			set
			{
				this.widthTypeAttribute.Value = value.Type;
				if (value.Type == TableWidthUnitType.Fixed)
				{
					this.widthAttribute.Value = (int)Unit.DipToTwip(value.Value);
					return;
				}
				if (value.Type == TableWidthUnitType.Percent)
				{
					this.widthAttribute.Value = (int)(value.Value * 50.0);
					return;
				}
				this.widthAttribute.Value = 0;
			}
		}

		public double ToDouble()
		{
			if (this.Value.Type == TableWidthUnitType.Percent || this.Value.Type == TableWidthUnitType.Auto)
			{
				return 0.0;
			}
			return this.Value.Value;
		}

		protected override bool ShouldExport(IDocxExportContext context)
		{
			return true;
		}

		readonly string elementName;

		readonly IntOpenXmlAttribute widthAttribute;

		readonly MappedOpenXmlAttribute<TableWidthUnitType> widthTypeAttribute;
	}
}
