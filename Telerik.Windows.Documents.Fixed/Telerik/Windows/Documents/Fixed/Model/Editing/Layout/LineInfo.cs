using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Fixed.Model.Editing.Layout
{
	class LineInfo
	{
		public LineInfo()
		{
			this.elements = new List<LayoutElementBase>();
		}

		public IEnumerable<LayoutElementBase> Elements
		{
			get
			{
				return this.elements;
			}
		}

		public double Width { get; set; }

		public double TrimmedWidth { get; set; }

		public Rect BoundingRect { get; set; }

		public double BaselineOffset { get; set; }

		public double Descent { get; set; }

		public double LineSpacingBaselineOffset { get; set; }

		public double LineSpacingDescent { get; set; }

		public double UnderlineThickness { get; set; }

		public double UnderlinePosition { get; set; }

		public bool IsEndingWithLineBreak
		{
			get
			{
				return this.elements.Count > 0 && this.elements[this.elements.Count - 1].IsLineBreak;
			}
		}

		public void Add(LayoutElementBase element)
		{
			Guard.ThrowExceptionIfNull<LayoutElementBase>(element, "element");
			if (element.TrimmedWidth != 0.0)
			{
				this.TrimmedWidth = this.Width + element.TrimmedWidth;
			}
			this.Width += element.Width;
			this.UnderlinePosition = Math.Max(this.UnderlinePosition, element.UnderlinePosition);
			this.UnderlineThickness = Math.Max(this.UnderlineThickness, element.UnderlineThickness);
			this.BaselineOffset = Math.Max(this.BaselineOffset, element.BaselineOffset);
			this.Descent = Math.Max(this.Descent, element.Descent);
			this.LineSpacingBaselineOffset = Math.Max(this.LineSpacingBaselineOffset, element.LineSpacingBaselineOffset);
			this.LineSpacingDescent = Math.Max(this.LineSpacingDescent, element.LineSpacingDescent);
			this.elements.Add(element);
		}

		public void AddRange(IEnumerable<LayoutElementBase> elements)
		{
			Guard.ThrowExceptionIfNull<IEnumerable<LayoutElementBase>>(elements, "elements");
			foreach (LayoutElementBase element in elements)
			{
				this.Add(element);
			}
		}

		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (LayoutElementBase layoutElementBase in this.elements)
			{
				stringBuilder.Append(layoutElementBase.ToString());
			}
			return stringBuilder.ToString();
		}

		readonly List<LayoutElementBase> elements;
	}
}
