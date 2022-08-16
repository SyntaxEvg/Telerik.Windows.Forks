using System;
using Telerik.Windows.Documents.Core.Data;
using Telerik.Windows.Documents.Spreadsheet.Layout;
using Telerik.Windows.Documents.Spreadsheet.Model;

namespace Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Model.Types
{
	static class TypeMappers
	{
		static TypeMappers()
		{
			TypeMappers.InitializeSheetVisibilityMapper();
			TypeMappers.InitializeViewportTypeMapper();
			TypeMappers.InitializeViewportStateMapper();
		}

		public static ValueMapper<string, SheetVisibility> SheetVisibilityMapper { get; set; }

		public static ValueMapper<string, ViewportPaneType> ViewportTypeMapper { get; set; }

		public static ValueMapper<string, PaneState> ViewportStateMapper { get; set; }

		static void InitializeSheetVisibilityMapper()
		{
			TypeMappers.SheetVisibilityMapper = new ValueMapper<string, SheetVisibility>("visible", SheetVisibility.Visible);
			TypeMappers.SheetVisibilityMapper.AddPair("hidden", SheetVisibility.Hidden);
			TypeMappers.SheetVisibilityMapper.AddPair("veryHidden", SheetVisibility.VeryHidden);
		}

		static void InitializeViewportTypeMapper()
		{
			TypeMappers.ViewportTypeMapper = new ValueMapper<string, ViewportPaneType>("topLeft", ViewportPaneType.Fixed);
			TypeMappers.ViewportTypeMapper.AddPair("bottomRight", ViewportPaneType.Scrollable);
			TypeMappers.ViewportTypeMapper.AddPair("topRight", ViewportPaneType.HorizontalScrollable);
			TypeMappers.ViewportTypeMapper.AddPair("bottomLeft", ViewportPaneType.VerticalScrollable);
		}

		static void InitializeViewportStateMapper()
		{
			TypeMappers.ViewportStateMapper = new ValueMapper<string, PaneState>("frozen", PaneState.Frozen);
		}
	}
}
