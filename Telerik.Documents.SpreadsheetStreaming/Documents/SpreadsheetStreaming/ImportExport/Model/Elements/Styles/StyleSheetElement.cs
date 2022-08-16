using System;
using System.Collections.Generic;
using System.Linq;
using Telerik.Documents.SpreadsheetStreaming.ImportExport.Core;
using Telerik.Documents.SpreadsheetStreaming.Model.Formatting;

namespace Telerik.Documents.SpreadsheetStreaming.ImportExport.Model.Elements.Styles
{
	class StyleSheetElement : DirectElementBase<StylesRepository>
	{
		public StyleSheetElement()
		{
			this.elementNameToReadAction = new Dictionary<string, Action<ElementBase, StylesRepository>>();
			this.elementNameToReadAction.Add("numFmts", new Action<ElementBase, StylesRepository>(this.ReadNumberFormats));
			this.elementNameToReadAction.Add("fonts", new Action<ElementBase, StylesRepository>(this.ReadFonts));
			this.elementNameToReadAction.Add("fills", new Action<ElementBase, StylesRepository>(this.ReadFills));
			this.elementNameToReadAction.Add("borders", new Action<ElementBase, StylesRepository>(this.ReadBorders));
			this.elementNameToReadAction.Add("cellStyleXfs", new Action<ElementBase, StylesRepository>(this.ReadCellStyleFormats));
			this.elementNameToReadAction.Add("cellXfs", new Action<ElementBase, StylesRepository>(this.ReadCellFormats));
			this.elementNameToReadAction.Add("cellStyles", new Action<ElementBase, StylesRepository>(this.ReadCellStyles));
			this.elementNameToReadAction.Add("dxfs", new Action<ElementBase, StylesRepository>(this.ReadDxfs));
		}

		public override OpenXmlNamespace Namespace
		{
			get
			{
				return OpenXmlNamespaces.SpreadsheetMLNamespace;
			}
		}

		public override string ElementName
		{
			get
			{
				return "styleSheet";
			}
		}

		protected override void InitializeAttributesOverride(StylesRepository value)
		{
		}

		protected override void WriteChildElementsOverride(StylesRepository value)
		{
			this.WriteNumberFormat(value.NumberFormats.ToList<NumberFormat>());
			this.WriteFonts(value.Fonts.ToList<FontProperties>());
			this.WriteFills(value.Fills.ToList<ISpreadFill>());
			this.WriteBorders(value.Borders.ToList<SpreadCellBorders>());
			this.WriteCellStyleFormats(value.CellStyleFormats.ToList<DiferentialFormat>());
			this.WriteCellFormats(value.CellFormats.ToList<DiferentialFormat>());
			this.WriteCellStyles(value.CellStyles.ToList<CellStyleInfo>());
			this.WriteDxfs();
		}

		protected override void CopyAttributesOverride(ref StylesRepository value)
		{
		}

		protected override void ReadChildElementOverride(ElementBase element, ref StylesRepository value)
		{
			this.elementNameToReadAction[element.ElementName](element, value);
		}

		static int AddItemToStylesRepository<T>(T item, Action<T> addItemAction, int startIndex)
		{
			int num = startIndex;
			addItemAction(item);
			if (num == startIndex)
			{
				startIndex++;
			}
			return startIndex;
		}

		void WriteNumberFormat(List<NumberFormat> numberFormats)
		{
			NumberFormatsElement numberFormatsElement = base.CreateChildElement<NumberFormatsElement>();
			numberFormatsElement.Write(numberFormats);
		}

		void ReadNumberFormats(ElementBase element, StylesRepository styles)
		{
			NumberFormatsElement numberFormatsElement = element as NumberFormatsElement;
			List<NumberFormat> list = new List<NumberFormat>();
			numberFormatsElement.Read(ref list);
			foreach (NumberFormat item in list)
			{
				styles.NumberFormatsStartIndex = StyleSheetElement.AddItemToStylesRepository<NumberFormat>(item, delegate(NumberFormat p)
				{
					styles.AddNumberFormat(p);
				}, styles.NumberFormatsStartIndex);
			}
		}

		void WriteFonts(List<FontProperties> fonts)
		{
			FontsElement fontsElement = base.CreateChildElement<FontsElement>();
			fontsElement.Write(fonts);
		}

		void ReadFonts(ElementBase element, StylesRepository styles)
		{
			FontsElement fontsElement = element as FontsElement;
			List<FontProperties> list = new List<FontProperties>();
			fontsElement.Read(ref list);
			foreach (FontProperties item in list)
			{
				styles.FontsStartIndex = StyleSheetElement.AddItemToStylesRepository<FontProperties>(item, delegate(FontProperties p)
				{
					styles.AddFont(p);
				}, styles.FontsStartIndex);
			}
		}

		void WriteFills(List<ISpreadFill> fills)
		{
			FillsElement fillsElement = base.CreateChildElement<FillsElement>();
			fillsElement.Write(fills);
		}

		void ReadFills(ElementBase element, StylesRepository styles)
		{
			FillsElement fillsElement = element as FillsElement;
			List<ISpreadFill> list = new List<ISpreadFill>();
			fillsElement.Read(ref list);
			foreach (ISpreadFill item in list)
			{
				styles.FillsStartIndex = StyleSheetElement.AddItemToStylesRepository<ISpreadFill>(item, delegate(ISpreadFill p)
				{
					styles.AddFill(p);
				}, styles.FillsStartIndex);
			}
		}

		void WriteBorders(List<SpreadCellBorders> borders)
		{
			BordersElement bordersElement = base.CreateChildElement<BordersElement>();
			bordersElement.Write(borders);
		}

		void ReadBorders(ElementBase element, StylesRepository styles)
		{
			BordersElement bordersElement = element as BordersElement;
			List<SpreadCellBorders> list = new List<SpreadCellBorders>();
			bordersElement.Read(ref list);
			foreach (SpreadCellBorders item in list)
			{
				styles.BordersStartIndex = StyleSheetElement.AddItemToStylesRepository<SpreadCellBorders>(item, delegate(SpreadCellBorders p)
				{
					styles.AddBorders(p);
				}, styles.BordersStartIndex);
			}
		}

		void WriteCellStyleFormats(List<DiferentialFormat> cellStyleFormats)
		{
			CellStyleFormatsElement cellStyleFormatsElement = base.CreateChildElement<CellStyleFormatsElement>();
			cellStyleFormatsElement.Write(cellStyleFormats);
		}

		void ReadCellStyleFormats(ElementBase element, StylesRepository styles)
		{
			CellStyleFormatsElement cellStyleFormatsElement = element as CellStyleFormatsElement;
			List<DiferentialFormat> list = new List<DiferentialFormat>();
			cellStyleFormatsElement.Read(ref list);
			foreach (DiferentialFormat item in list)
			{
				styles.CellStyleFormatsStartIndex = StyleSheetElement.AddItemToStylesRepository<DiferentialFormat>(item, delegate(DiferentialFormat p)
				{
					styles.AddCellStyleFormat(p);
				}, styles.CellStyleFormatsStartIndex);
			}
		}

		void WriteCellFormats(List<DiferentialFormat> cellFormats)
		{
			CellFormatsElement cellFormatsElement = base.CreateChildElement<CellFormatsElement>();
			cellFormatsElement.Write(cellFormats);
		}

		void ReadCellFormats(ElementBase element, StylesRepository styles)
		{
			CellFormatsElement cellFormatsElement = element as CellFormatsElement;
			List<DiferentialFormat> list = new List<DiferentialFormat>();
			cellFormatsElement.Read(ref list);
			foreach (DiferentialFormat item in list)
			{
				styles.CellFormatsStartIndex = StyleSheetElement.AddItemToStylesRepository<DiferentialFormat>(item, delegate(DiferentialFormat p)
				{
					styles.AddCellFormat(p);
				}, styles.CellFormatsStartIndex);
			}
		}

		void WriteCellStyles(List<CellStyleInfo> cellStyles)
		{
			CellStylesElement cellStylesElement = base.CreateChildElement<CellStylesElement>();
			cellStylesElement.Write(cellStyles);
		}

		void ReadCellStyles(ElementBase element, StylesRepository styles)
		{
			CellStylesElement cellStylesElement = element as CellStylesElement;
			List<CellStyleInfo> list = new List<CellStyleInfo>();
			cellStylesElement.Read(ref list);
			foreach (CellStyleInfo item in list)
			{
				styles.CellStylesStartIndex = StyleSheetElement.AddItemToStylesRepository<CellStyleInfo>(item, delegate(CellStyleInfo p)
				{
					styles.AddCellStyle(p);
				}, styles.CellStylesStartIndex);
			}
		}

		void WriteDxfs()
		{
			DifferentialFormatsElement differentialFormatsElement = base.CreateChildElement<DifferentialFormatsElement>();
			differentialFormatsElement.Write(0);
		}

		void ReadDxfs(ElementBase element, StylesRepository styles)
		{
			DifferentialFormatsElement differentialFormatsElement = element as DifferentialFormatsElement;
			int num = 0;
			differentialFormatsElement.Read(ref num);
		}

		readonly Dictionary<string, Action<ElementBase, StylesRepository>> elementNameToReadAction;
	}
}
