using System;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Core;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Contexts;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Export;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Import;
using Telerik.Windows.Documents.Spreadsheet.Model;
using Telerik.Windows.Documents.Spreadsheet.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Model.Elements.StyleSheet
{
	class BorderElement : StyleSheetElementBase
	{
		public BorderElement(XlsxPartsManager partsManager)
			: base(partsManager)
		{
			this.diagonalDown = base.RegisterAttribute<BoolOpenXmlAttribute>(new BoolOpenXmlAttribute("diagonalDown"));
			this.diagonalUp = base.RegisterAttribute<BoolOpenXmlAttribute>(new BoolOpenXmlAttribute("diagonalUp"));
			this.left = base.RegisterChildElement<LeftBorderElement>("left");
			this.right = base.RegisterChildElement<RightBorderElement>("right");
			this.top = base.RegisterChildElement<TopBorderElement>("top");
			this.bottom = base.RegisterChildElement<BottomBorderElement>("bottom");
			this.diagonal = base.RegisterChildElement<DiagonalBorderElement>("diagonal");
		}

		public override string ElementName
		{
			get
			{
				return "border";
			}
		}

		public bool DiagonalDown
		{
			get
			{
				return this.diagonalDown.Value;
			}
			set
			{
				this.diagonalDown.Value = value;
			}
		}

		public bool DiagonalUp
		{
			get
			{
				return this.diagonalUp.Value;
			}
			set
			{
				this.diagonalUp.Value = value;
			}
		}

		public LeftBorderElement LeftBorder
		{
			get
			{
				return this.left.Element;
			}
		}

		public RightBorderElement RightBorder
		{
			get
			{
				return this.right.Element;
			}
		}

		public TopBorderElement TopBorder
		{
			get
			{
				return this.top.Element;
			}
		}

		public BottomBorderElement BottomBorder
		{
			get
			{
				return this.bottom.Element;
			}
		}

		public DiagonalBorderElement DiagonalBorder
		{
			get
			{
				return this.diagonal.Element;
			}
		}

		public void CopyPropertiesFrom(IXlsxWorkbookExportContext context, BordersInfo border)
		{
			Guard.ThrowExceptionIfNull<IXlsxWorkbookExportContext>(context, "context");
			Guard.ThrowExceptionIfNull<BordersInfo>(border, "border");
			if (border.Left != null)
			{
				base.CreateElement(this.left);
				this.LeftBorder.CopyPropertiesFrom(context, border.Left);
			}
			if (border.Top != null)
			{
				base.CreateElement(this.top);
				this.TopBorder.CopyPropertiesFrom(context, border.Top);
			}
			if (border.Right != null)
			{
				base.CreateElement(this.right);
				this.RightBorder.CopyPropertiesFrom(context, border.Right);
			}
			if (border.Bottom != null)
			{
				base.CreateElement(this.bottom);
				this.BottomBorder.CopyPropertiesFrom(context, border.Bottom);
			}
			if (!BorderElement.IsNullOrNone(border.DiagonalDown))
			{
				base.CreateElement(this.diagonal);
				this.DiagonalBorder.CopyPropertiesFrom(context, border.DiagonalDown);
				this.DiagonalDown = true;
			}
			if (!BorderElement.IsNullOrNone(border.DiagonalUp))
			{
				base.CreateElement(this.diagonal);
				this.DiagonalBorder.CopyPropertiesFrom(context, border.DiagonalUp);
				this.DiagonalUp = true;
			}
		}

		protected override void OnAfterRead(IXlsxWorkbookImportContext context)
		{
			Guard.ThrowExceptionIfNull<IXlsxWorkbookImportContext>(context, "context");
			BordersInfo resource = default(BordersInfo);
			if (this.LeftBorder != null)
			{
				resource.Left = this.LeftBorder.CreateCellBorder(context);
				base.ReleaseElement(this.left);
			}
			if (this.TopBorder != null)
			{
				resource.Top = this.TopBorder.CreateCellBorder(context);
				base.ReleaseElement(this.top);
			}
			if (this.RightBorder != null)
			{
				resource.Right = this.RightBorder.CreateCellBorder(context);
				base.ReleaseElement(this.right);
			}
			if (this.BottomBorder != null)
			{
				resource.Bottom = this.BottomBorder.CreateCellBorder(context);
				base.ReleaseElement(this.bottom);
			}
			if (this.DiagonalBorder != null)
			{
				CellBorder cellBorder = this.DiagonalBorder.CreateCellBorder(context);
				if (this.DiagonalUp)
				{
					resource.DiagonalUp = cellBorder;
					base.ReleaseElement(this.diagonal);
				}
				if (this.DiagonalDown)
				{
					resource.DiagonalDown = cellBorder;
					base.ReleaseElement(this.diagonal);
				}
			}
			context.StyleSheet.BordersInfoTable.Add(resource);
		}

		static bool IsNullOrNone(CellBorder border)
		{
			return border == null || border.Style == CellBorderStyle.None;
		}

		readonly BoolOpenXmlAttribute diagonalDown;

		readonly BoolOpenXmlAttribute diagonalUp;

		readonly OpenXmlChildElement<BottomBorderElement> bottom;

		readonly OpenXmlChildElement<DiagonalBorderElement> diagonal;

		readonly OpenXmlChildElement<LeftBorderElement> left;

		readonly OpenXmlChildElement<RightBorderElement> right;

		readonly OpenXmlChildElement<TopBorderElement> top;
	}
}
