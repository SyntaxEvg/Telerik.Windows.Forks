using System;
using Telerik.Documents.SpreadsheetStreaming.ImportExport.Core;
using Telerik.Documents.SpreadsheetStreaming.ImportExport.Core.Attributes;
using Telerik.Documents.SpreadsheetStreaming.Model;

namespace Telerik.Documents.SpreadsheetStreaming.ImportExport.Model.Elements.Worksheet
{
	class ColumnElement : DirectElementBase<ColumnRange>
	{
		public ColumnElement()
		{
			this.min = base.RegisterAttribute<IntOpenXmlAttribute>(new IntOpenXmlAttribute("min", true));
			this.max = base.RegisterAttribute<IntOpenXmlAttribute>(new IntOpenXmlAttribute("max", true));
			this.bestFit = base.RegisterAttribute<BoolOpenXmlAttribute>(new BoolOpenXmlAttribute("bestFit"));
			this.customWidth = base.RegisterAttribute<BoolOpenXmlAttribute>(new BoolOpenXmlAttribute("customWidth"));
			this.styleIndex = base.RegisterAttribute<IntOpenXmlAttribute>(new IntOpenXmlAttribute("style", 0, false));
			this.width = base.RegisterAttribute<double>("width", false);
			this.hidden = base.RegisterAttribute<BoolOpenXmlAttribute>(new BoolOpenXmlAttribute("hidden"));
			this.outlineLevel = base.RegisterAttribute<IntOpenXmlAttribute>(new IntOpenXmlAttribute("outlineLevel", false));
		}

		public override string ElementName
		{
			get
			{
				return "col";
			}
		}

		public int Min
		{
			get
			{
				return this.min.Value;
			}
			set
			{
				this.min.Value = value;
			}
		}

		public int Max
		{
			get
			{
				return this.max.Value;
			}
			set
			{
				this.max.Value = value;
			}
		}

		public bool CustomWidth
		{
			get
			{
				return this.customWidth.Value;
			}
			set
			{
				this.customWidth.Value = value;
			}
		}

		public double Width
		{
			get
			{
				return this.width.Value;
			}
			set
			{
				this.width.Value = value;
			}
		}

		public int OutlineLevel
		{
			get
			{
				return this.outlineLevel.Value;
			}
			set
			{
				this.outlineLevel.Value = value;
			}
		}

		public bool Hidden
		{
			get
			{
				return this.hidden.Value;
			}
			set
			{
				this.hidden.Value = value;
			}
		}

		bool BestFit
		{
			set
			{
				this.bestFit.Value = value;
			}
		}

		int StyleIndex
		{
			set
			{
				this.styleIndex.Value = value;
			}
		}

		protected override void InitializeAttributesOverride(ColumnRange value)
		{
			if (value.CustomWidth != null)
			{
				this.CustomWidth = value.CustomWidth.Value;
			}
			if (value.Hidden != null)
			{
				this.Hidden = value.Hidden.Value;
			}
			this.Max = value.LastColumnNumber;
			this.Min = value.FirstColumnNumber;
			if (value.OutlineLevel != null)
			{
				this.OutlineLevel = value.OutlineLevel.Value;
			}
			if (value.Width != null)
			{
				this.Width = value.Width.Value;
			}
		}

		protected override void CopyAttributesOverride(ref ColumnRange value)
		{
			value = new ColumnRange(this.Min);
			value.LastColumnNumber = this.Max;
			if (this.customWidth.HasValue)
			{
				value.CustomWidth = new bool?(this.CustomWidth);
			}
			if (this.hidden.HasValue)
			{
				value.Hidden = new bool?(this.Hidden);
			}
			if (this.outlineLevel.HasValue)
			{
				value.OutlineLevel = new int?(this.OutlineLevel);
			}
			if (this.width.HasValue)
			{
				value.Width = new double?(this.Width);
			}
		}

		protected override void WriteChildElementsOverride(ColumnRange value)
		{
		}

		readonly IntOpenXmlAttribute min;

		readonly IntOpenXmlAttribute max;

		readonly BoolOpenXmlAttribute bestFit;

		readonly BoolOpenXmlAttribute customWidth;

		readonly IntOpenXmlAttribute styleIndex;

		readonly OpenXmlAttribute<double> width;

		readonly BoolOpenXmlAttribute hidden;

		readonly IntOpenXmlAttribute outlineLevel;
	}
}
