using System;
using Telerik.Documents.SpreadsheetStreaming.ImportExport.Core;
using Telerik.Documents.SpreadsheetStreaming.ImportExport.Core.Attributes;
using Telerik.Documents.SpreadsheetStreaming.Model.Formatting;

namespace Telerik.Documents.SpreadsheetStreaming.ImportExport.Model.Elements.Styles
{
	class BorderElement : DirectElementBase<SpreadCellBorders>
	{
		public BorderElement()
		{
			this.diagonalDown = base.RegisterAttribute<BoolOpenXmlAttribute>(new BoolOpenXmlAttribute("diagonalDown"));
			this.diagonalUp = base.RegisterAttribute<BoolOpenXmlAttribute>(new BoolOpenXmlAttribute("diagonalUp"));
		}

		public override string ElementName
		{
			get
			{
				return "border";
			}
		}

		bool DiagonalDown
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

		bool DiagonalUp
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

		protected override void InitializeAttributesOverride(SpreadCellBorders value)
		{
			if (!BorderElement.IsNullOrNone(value.DiagonalDown))
			{
				this.DiagonalDown = true;
			}
			if (!BorderElement.IsNullOrNone(value.DiagonalUp))
			{
				this.DiagonalUp = true;
			}
		}

		protected override void WriteChildElementsOverride(SpreadCellBorders value)
		{
			LeftBorderElement leftBorderElement = base.CreateChildElement<LeftBorderElement>();
			leftBorderElement.Write(value.Left);
			RightBorderElement rightBorderElement = base.CreateChildElement<RightBorderElement>();
			rightBorderElement.Write(value.Right);
			TopBorderElement topBorderElement = base.CreateChildElement<TopBorderElement>();
			topBorderElement.Write(value.Top);
			BottomBorderElement bottomBorderElement = base.CreateChildElement<BottomBorderElement>();
			bottomBorderElement.Write(value.Bottom);
			SpreadBorder spreadBorder = value.DiagonalUp;
			if (spreadBorder == null)
			{
				spreadBorder = value.DiagonalDown;
			}
			DiagonalBorderElement diagonalBorderElement = base.CreateChildElement<DiagonalBorderElement>();
			diagonalBorderElement.Write(spreadBorder);
		}

		protected override void CopyAttributesOverride(ref SpreadCellBorders value)
		{
		}

		protected override void ReadChildElementOverride(ElementBase element, ref SpreadCellBorders value)
		{
			string elementName;
			if ((elementName = element.ElementName) != null)
			{
				if (elementName == "left")
				{
					SpreadBorder left = BorderElement.ReadBorderElement(element);
					value.Left = left;
					return;
				}
				if (elementName == "right")
				{
					SpreadBorder right = BorderElement.ReadBorderElement(element);
					value.Right = right;
					return;
				}
				if (elementName == "top")
				{
					SpreadBorder top = BorderElement.ReadBorderElement(element);
					value.Top = top;
					return;
				}
				if (elementName == "bottom")
				{
					SpreadBorder bottom = BorderElement.ReadBorderElement(element);
					value.Bottom = bottom;
					return;
				}
				if (elementName == "diagonal")
				{
					SpreadBorder spreadBorder = BorderElement.ReadBorderElement(element);
					if (this.DiagonalDown)
					{
						value.DiagonalDown = spreadBorder;
					}
					if (this.DiagonalUp)
					{
						value.DiagonalUp = spreadBorder;
						return;
					}
					return;
				}
			}
			throw new InvalidOperationException();
		}

		static SpreadBorder ReadBorderElement(ElementBase element)
		{
			BorderTypeElementBase borderTypeElementBase = element as BorderTypeElementBase;
			SpreadBorder result = null;
			borderTypeElementBase.Read(ref result);
			return result;
		}

		static bool IsNullOrNone(SpreadBorder border)
		{
			return border == null || border.Style == SpreadBorderStyle.None;
		}

		readonly BoolOpenXmlAttribute diagonalDown;

		readonly BoolOpenXmlAttribute diagonalUp;
	}
}
