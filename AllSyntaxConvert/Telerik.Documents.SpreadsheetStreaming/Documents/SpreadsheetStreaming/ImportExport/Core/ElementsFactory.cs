using System;
using System.Collections.Generic;
using Telerik.Documents.SpreadsheetStreaming.ImportExport.Model.Elements;
using Telerik.Documents.SpreadsheetStreaming.ImportExport.Model.Elements.Styles;
using Telerik.Documents.SpreadsheetStreaming.ImportExport.Model.Elements.Worksheet;
using Telerik.Documents.SpreadsheetStreaming.ImportExport.Model.Writers;
using Telerik.Documents.SpreadsheetStreaming.ImportExport.Model.Writers.Worksheet.Styles;

namespace Telerik.Documents.SpreadsheetStreaming.ImportExport.Core
{
	static class ElementsFactory
	{
		static ElementsFactory()
		{
			ElementsFactory.RegisterFactoryMethod<DefaultElement>("Default");
			ElementsFactory.RegisterFactoryMethod<OverrideElement>("Override");
			ElementsFactory.RegisterFactoryMethod<RelationshipElement>("Relationship");
			ElementsFactory.RegisterFactoryMethod<NumberFormatsElement>("numFmts");
			ElementsFactory.RegisterFactoryMethod<NumberFormatElement>("numFmt");
			ElementsFactory.RegisterFactoryMethod<FontsElement>("fonts");
			ElementsFactory.RegisterFactoryMethod<FontElement>("font");
			ElementsFactory.RegisterFactoryMethod<FontSizeElement>("sz");
			ElementsFactory.RegisterFactoryMethod<ColorElement>("color");
			ElementsFactory.RegisterFactoryMethod<FontNameElement>("name");
			ElementsFactory.RegisterFactoryMethod<FontFamilyElement>("family");
			ElementsFactory.RegisterFactoryMethod<SchemeElement>("scheme");
			ElementsFactory.RegisterFactoryMethod<BoldElement>("b");
			ElementsFactory.RegisterFactoryMethod<ItalicElement>("i");
			ElementsFactory.RegisterFactoryMethod<UnderlineElement>("u");
			ElementsFactory.RegisterFactoryMethod<FillsElement>("fills");
			ElementsFactory.RegisterFactoryMethod<FillElement>("fill");
			ElementsFactory.RegisterFactoryMethod<PatternFillElement>("patternFill");
			ElementsFactory.RegisterFactoryMethod<BordersElement>("borders");
			ElementsFactory.RegisterFactoryMethod<BorderElement>("border");
			ElementsFactory.RegisterFactoryMethod<LeftBorderElement>("left");
			ElementsFactory.RegisterFactoryMethod<RightBorderElement>("right");
			ElementsFactory.RegisterFactoryMethod<TopBorderElement>("top");
			ElementsFactory.RegisterFactoryMethod<BottomBorderElement>("bottom");
			ElementsFactory.RegisterFactoryMethod<DiagonalBorderElement>("diagonal");
			ElementsFactory.RegisterFactoryMethod<CellStyleFormatsElement>("cellStyleXfs");
			ElementsFactory.RegisterFactoryMethod<FormatElement>("xf");
			ElementsFactory.RegisterFactoryMethod<CellFormatsElement>("cellXfs");
			ElementsFactory.RegisterFactoryMethod<CellStylesElement>("cellStyles");
			ElementsFactory.RegisterFactoryMethod<CellStyleElement>("cellStyle");
			ElementsFactory.RegisterFactoryMethod<DifferentialFormatsElement>("dxfs");
			ElementsFactory.RegisterFactoryMethod<SheetsElement>("sheets");
			ElementsFactory.RegisterFactoryMethod<SheetElement>("sheet");
			ElementsFactory.RegisterFactoryMethod<SheetDataElement>("sheetData");
			ElementsFactory.RegisterFactoryMethod<ColumnsElement>("cols");
			ElementsFactory.RegisterFactoryMethod<ColumnElement>("col");
			ElementsFactory.RegisterFactoryMethod<RowElement>("row");
			ElementsFactory.RegisterFactoryMethod<CellElement>("c");
			ElementsFactory.RegisterFactoryMethod<CellValueElement>("v");
			ElementsFactory.RegisterFactoryMethod<ForegroundColorElement>("fgColor");
			ElementsFactory.RegisterFactoryMethod<BackgroundColorElement>("bgColor");
			ElementsFactory.RegisterFactoryMethod<AlignmentElement>("alignment");
			ElementsFactory.RegisterFactoryMethod<GradientFillElement>("gradientFill");
			ElementsFactory.RegisterFactoryMethod<StopElement>("stop");
			ElementsFactory.RegisterFactoryMethod<VerticalAlignmentElement>("vertAlign");
			ElementsFactory.RegisterFactoryMethod<ProtectionElement>("protection");
			ElementsFactory.RegisterFactoryMethod<StrikeElement>("strike");
		}

		public static ElementBase CreateElement(string elementName, ElementContext context)
		{
			ElementBase elementBase = null;
			Func<ElementBase> func = null;
			if (ElementsFactory.elementNameToCreateFunc.TryGetValue(elementName, out func))
			{
				if (func == null)
				{
					return null;
				}
				elementBase = func();
				elementBase.SetContext(context);
			}
			return elementBase;
		}

		public static Type GetElementType(string elementName)
		{
			return ElementsFactory.elementNameToType[elementName];
		}

		public static bool CanCreateElement(string elementName)
		{
			return ElementsFactory.elementNameToType.ContainsKey(elementName) && ElementsFactory.elementNameToType[elementName] != null;
		}

		static void RegisterFactoryMethod<T>(string elementName) where T : ElementBase, new()
		{
			Type typeFromHandle = typeof(T);
			ElementsFactory.elementNameToType.Add(elementName, typeFromHandle);
			ElementsFactory.elementNameToCreateFunc.Add(elementName, () => Activator.CreateInstance<T>());
			if (typeFromHandle != null)
			{
				ElementsFactory.typeToElementName.Add(typeFromHandle, elementName);
			}
		}

		static readonly Dictionary<string, Type> elementNameToType = new Dictionary<string, Type>();

		static readonly Dictionary<Type, string> typeToElementName = new Dictionary<Type, string>();

		static readonly Dictionary<string, Func<ElementBase>> elementNameToCreateFunc = new Dictionary<string, Func<ElementBase>>();
	}
}
