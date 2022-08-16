using System;
using System.Collections.Generic;
using System.Globalization;
using Telerik.Windows.Documents.Flow.FormatProviders.Html.Model.Attributes;
using Telerik.Windows.Documents.Flow.FormatProviders.Html.Model.Styles;
using Telerik.Windows.Documents.Flow.Model;
using Telerik.Windows.Documents.Flow.Model.BorderEvaluation;
using Telerik.Windows.Documents.Flow.Model.Styles;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Html.Export
{
	class HtmlExportContext : IHtmlExportContext
	{
		public HtmlExportContext(RadFlowDocument document, HtmlExportSettings settings)
		{
			Guard.ThrowExceptionIfNull<RadFlowDocument>(document, "document");
			Guard.ThrowExceptionIfNull<HtmlExportSettings>(settings, "settings");
			this.document = document;
			this.settings = settings;
			this.htmlStyleRepository = new HtmlStyleRepository();
			this.imageRepository = new ImageRepository();
			this.tableBorderGrids = new Dictionary<Table, TableBorderGrid>();
		}

		public CultureInfo Culture
		{
			get
			{
				return CultureInfo.InvariantCulture;
			}
		}

		public RadFlowDocument Document
		{
			get
			{
				return this.document;
			}
		}

		public HtmlExportSettings Settings
		{
			get
			{
				return this.settings;
			}
		}

		public HtmlStyleRepository HtmlStyleRepository
		{
			get
			{
				return this.htmlStyleRepository;
			}
		}

		public ImageRepository ImageRepository
		{
			get
			{
				return this.imageRepository;
			}
		}

		public void BeginExport()
		{
			foreach (Style style in this.Document.StyleRepository.GetSortedTopologicallyStyles())
			{
				this.RegisterStyleForExport(style);
			}
		}

		public void ClearParagraphClasses(ClassNamesCollection classes)
		{
			Guard.ThrowExceptionIfNull<ClassNamesCollection>(classes, "classes");
			if (classes.Count == 2 && classes.PeekFirst() == StyleNamesConverter.PrefixedNormalStyleId && classes.PeekLast() == StyleNamesConverter.PrefixedNormalWebStyleId)
			{
				classes.Clear();
			}
		}

		public void EndExport()
		{
		}

		public void BeginExportTable(Table table)
		{
			TableBorderGrid tableBorderGrid = new TableBorderGrid(table);
			if (!table.HasCellSpacing)
			{
				tableBorderGrid.AssureValid();
			}
			this.tableBorderGrids.Add(table, tableBorderGrid);
		}

		public void EndExportTable(Table table)
		{
			this.tableBorderGrids.Remove(table);
		}

		public TableBorderGrid GetBorderGrid(Table table)
		{
			return this.tableBorderGrids.GetValueOrNull(table);
		}

		void RegisterStyleForExport(Style style)
		{
			string styleName;
			if (StyleToElementSelectorMappings.TryGetElementSelector(style.Id, out styleName))
			{
				this.RegisterStyleForExport(style, SelectorType.Elements, styleName);
			}
			styleName = StyleNamesConverter.ConvertStyleNameOnExport(style.Id);
			if (style.StyleType == StyleType.Table)
			{
				this.RegisterTableStyle(styleName, style);
				return;
			}
			this.RegisterStyleForExport(style, SelectorType.Style, styleName);
		}

		void RegisterStyleForExport(Style style, SelectorType selectorType, string styleName)
		{
			Selector selector = new Selector(styleName, selectorType);
			if (style.Id == "NormalWeb" && style.BasedOnStyleId == "Normal")
			{
				selector.CopyFrom(this, this.Document.StyleRepository.GetStyle("Normal"));
			}
			selector.CopyFrom(this, style);
			this.HtmlStyleRepository.RegisterStyle(styleName, selector);
		}

		Selector RegisterTableStyle(string styleName, Style style)
		{
			Selector selector = new Selector(styleName, SelectorType.StyleForElements, "table");
			selector.CopyFrom(this, style.CharacterProperties);
			selector.CopyFrom(this, style.ParagraphProperties);
			selector.CopyFrom(this, style.TableProperties);
			this.HtmlStyleRepository.RegisterStyle(styleName, selector);
			if (style.TableProperties.Borders.HasLocalValue || style.TableCellProperties.VerticalAlignment.LocalValue != TableCell.VerticalAlignmentPropertyDefinition.DefaultValue)
			{
				selector = new Selector(styleName, SelectorType.StyleForElements, "td");
				if (style.TableProperties.Borders.LocalValue != null)
				{
					TableBorders obj = new TableBorders(style.TableProperties.Borders.LocalValue.Left);
					if (style.TableProperties.Borders.LocalValue.Equals(obj))
					{
						selector.CopyFrom(this, style.TableProperties.GetStyleProperty(Table.TableBordersPropertyDefinition), style.TableProperties, true);
					}
				}
				selector.CopyFrom(this, style.TableCellProperties);
				this.HtmlStyleRepository.RegisterStyle(styleName, selector);
			}
			return selector;
		}

		readonly RadFlowDocument document;

		readonly HtmlExportSettings settings;

		readonly HtmlStyleRepository htmlStyleRepository;

		readonly ImageRepository imageRepository;

		readonly Dictionary<Table, TableBorderGrid> tableBorderGrids;
	}
}
