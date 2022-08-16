using System;
using System.Collections.Generic;
using Telerik.Documents.SpreadsheetStreaming.ImportExport.Core;
using Telerik.Documents.SpreadsheetStreaming.ImportExport.Core.Attributes;
using Telerik.Documents.SpreadsheetStreaming.ImportExport.Core.Types;

namespace Telerik.Documents.SpreadsheetStreaming.ImportExport.Model.Writers.Worksheet.Styles
{
	class GradientFillElement : DirectElementBase<ISpreadFill>
	{
		public GradientFillElement()
		{
			this.type = base.RegisterAttribute<string>("type", GradientTypes.Linear, false);
			this.degree = base.RegisterAttribute<double>("degree", 0.0, false);
			this.left = base.RegisterAttribute<double>("left", 0.0, false);
			this.right = base.RegisterAttribute<double>("right", 0.0, false);
			this.top = base.RegisterAttribute<double>("top", 0.0, false);
			this.bottom = base.RegisterAttribute<double>("bottom", 0.0, false);
		}

		public override string ElementName
		{
			get
			{
				return "gradientFill";
			}
		}

		string Type
		{
			get
			{
				return this.type.Value;
			}
			set
			{
				this.type.Value = value;
			}
		}

		double Bottom
		{
			get
			{
				return this.bottom.Value;
			}
			set
			{
				this.bottom.Value = value;
			}
		}

		double Degree
		{
			get
			{
				return this.degree.Value;
			}
			set
			{
				this.degree.Value = value;
			}
		}

		double Left
		{
			get
			{
				return this.left.Value;
			}
			set
			{
				this.left.Value = value;
			}
		}

		double Right
		{
			get
			{
				return this.right.Value;
			}
			set
			{
				this.right.Value = value;
			}
		}

		double Top
		{
			get
			{
				return this.top.Value;
			}
			set
			{
				this.top.Value = value;
			}
		}

		protected override void InitializeAttributesOverride(ISpreadFill value)
		{
			SpreadGradientFill gradientFill = value as SpreadGradientFill;
			this.gradientFillInfo = GradientFillInfo.Create(gradientFill);
			if (this.gradientFillInfo.Stops.Count == 0)
			{
				return;
			}
			if (this.gradientFillInfo.InfoType != null)
			{
				this.Type = GradientTypes.GetGradientTypeName(this.gradientFillInfo.InfoType.Value);
			}
			if (this.gradientFillInfo.Degree != null)
			{
				this.Degree = this.gradientFillInfo.Degree.Value;
			}
			if (this.gradientFillInfo.Left != null)
			{
				this.Left = this.gradientFillInfo.Left.Value;
			}
			if (this.gradientFillInfo.Right != null)
			{
				this.Right = this.gradientFillInfo.Right.Value;
			}
			if (this.gradientFillInfo.Top != null)
			{
				this.Top = this.gradientFillInfo.Top.Value;
			}
			if (this.gradientFillInfo.Bottom != null)
			{
				this.Bottom = this.gradientFillInfo.Bottom.Value;
			}
		}

		protected override void WriteChildElementsOverride(ISpreadFill value)
		{
			List<GradientStop> stops = this.gradientFillInfo.Stops;
			foreach (GradientStop value2 in stops)
			{
				StopElement stopElement = base.CreateChildElement<StopElement>();
				stopElement.Write(value2);
			}
		}

		protected override void CopyAttributesOverride(ref ISpreadFill value)
		{
			if (this.type.HasValue)
			{
				this.gradientFillInfo.InfoType = GradientTypes.GetGradientTypeValue(this.Type);
			}
			if (this.degree.HasValue)
			{
				this.gradientFillInfo.Degree = new double?(this.Degree);
			}
			if (this.left.HasValue)
			{
				this.gradientFillInfo.Left = new double?(this.Left);
			}
			if (this.right.HasValue)
			{
				this.gradientFillInfo.Right = new double?(this.Right);
			}
			if (this.top.HasValue)
			{
				this.gradientFillInfo.Top = new double?(this.Top);
			}
			if (this.bottom.HasValue)
			{
				this.gradientFillInfo.Bottom = new double?(this.Bottom);
			}
			value = this.gradientFillInfo.ToFill();
		}

		protected override void ReadChildElementOverride(ElementBase element, ref ISpreadFill value)
		{
			if (this.gradientFillInfo == null)
			{
				this.gradientFillInfo = new GradientFillInfo();
			}
			string elementName;
			if ((elementName = element.ElementName) != null && elementName == "stop")
			{
				GradientStop item = new GradientStop();
				StopElement stopElement = element as StopElement;
				stopElement.Read(ref item);
				this.gradientFillInfo.Stops.Add(item);
				return;
			}
			throw new NotImplementedException();
		}

		readonly OpenXmlAttribute<double> bottom;

		readonly OpenXmlAttribute<double> degree;

		readonly OpenXmlAttribute<double> left;

		readonly OpenXmlAttribute<double> right;

		readonly OpenXmlAttribute<double> top;

		readonly OpenXmlAttribute<string> type;

		GradientFillInfo gradientFillInfo;
	}
}
