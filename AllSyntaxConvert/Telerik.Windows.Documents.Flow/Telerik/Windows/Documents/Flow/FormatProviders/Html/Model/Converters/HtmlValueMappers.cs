using System;
using System.Windows;
using Telerik.Windows.Documents.Core.Data;
using Telerik.Windows.Documents.Flow.Model.Lists;
using Telerik.Windows.Documents.Flow.Model.Styles;
using VerticalAlignment = Telerik.Windows.Documents.Flow.Model.Styles.VerticalAlignment;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Html.Model.Converters
{
	static class HtmlValueMappers
	{
		static ValueMapper<string, TableLayoutType> InitializeTableLayoutTypeMapper()
		{
			ValueMapper<string, TableLayoutType> valueMapper = new ValueMapper<string, TableLayoutType>();
			valueMapper.AddPair("auto", TableLayoutType.AutoFit);
			valueMapper.AddPair("fixed", TableLayoutType.FixedWidth);
			return valueMapper;
		}

		static ValueMapper<string, Telerik.Windows.Documents.Flow.Model.Styles.BaselineAlignment> InitializeBaselineAlignmentMapper()
		{
			ValueMapper<string, Telerik.Windows.Documents.Flow.Model.Styles.BaselineAlignment> valueMapper = new ValueMapper<string, Telerik.Windows.Documents.Flow.Model.Styles.BaselineAlignment>();
			valueMapper.AddPair("sub", Telerik.Windows.Documents.Flow.Model.Styles.BaselineAlignment.Subscript);
			valueMapper.AddPair("super", Telerik.Windows.Documents.Flow.Model.Styles.BaselineAlignment.Superscript);
			valueMapper.AddPair("baseline", Telerik.Windows.Documents.Flow.Model.Styles.BaselineAlignment.Baseline);
			return valueMapper;
		}

		static ValueMapper<string, VerticalAlignment> InitializeVerticalAlignmentMapper()
		{
			ValueMapper<string, VerticalAlignment> valueMapper = new ValueMapper<string, VerticalAlignment>();
			valueMapper.AddPair("top", VerticalAlignment.Top);
			valueMapper.AddPair("middle", VerticalAlignment.Center);
			valueMapper.AddPair("bottom", VerticalAlignment.Bottom);
			return valueMapper;
		}

		static ValueMapper<string, NumberingStyle> InitializeNumberingStyleTypeMapper()
		{
			ValueMapper<string, NumberingStyle> valueMapper = new ValueMapper<string, NumberingStyle>("decimal", NumberingStyle.Decimal);
			valueMapper.AddPair("circle", NumberingStyle.Bullet);
			valueMapper.AddPair("square", NumberingStyle.Bullet);
			valueMapper.AddPair("disc", NumberingStyle.Bullet);
			valueMapper.AddPair("lower-greek", NumberingStyle.Decimal);
			valueMapper.AddPair("lower-alpha", NumberingStyle.LowerLetter);
			valueMapper.AddPair("upper-alpha", NumberingStyle.UpperLetter);
			valueMapper.AddPair("lower-roman", NumberingStyle.LowerRoman);
			valueMapper.AddPair("upper-roman", NumberingStyle.UpperRoman);
			valueMapper.AddPair("lower-latin", NumberingStyle.LowerLetter);
			valueMapper.AddPair("upper-latin", NumberingStyle.UpperLetter);
			valueMapper.AddPair("decimal", NumberingStyle.Decimal);
			return valueMapper;
		}

		static ValueMapper<string, NumberingStyle> InitializeNumberingStyleMapper()
		{
			ValueMapper<string, NumberingStyle> valueMapper = new ValueMapper<string, NumberingStyle>("1", NumberingStyle.Decimal);
			valueMapper.AddPair("a", NumberingStyle.LowerLetter);
			valueMapper.AddPair("A", NumberingStyle.UpperLetter);
			valueMapper.AddPair("i", NumberingStyle.LowerRoman);
			valueMapper.AddPair("I", NumberingStyle.UpperRoman);
			valueMapper.AddPair("disc", NumberingStyle.Bullet);
			valueMapper.AddPair("circle", NumberingStyle.Bullet);
			valueMapper.AddPair("square", NumberingStyle.Bullet);
			return valueMapper;
		}

		static ValueMapper<string, Alignment> InitializeAlignmentMapper()
		{
			ValueMapper<string, Alignment> valueMapper = new ValueMapper<string, Alignment>();
			valueMapper.AddPair("left", Alignment.Left);
			valueMapper.AddPair("center", Alignment.Center);
			valueMapper.AddPair("right", Alignment.HighKashida);
			valueMapper.AddPair("right", Alignment.MediumKashida);
			valueMapper.AddPair("right", Alignment.LowKashida);
			valueMapper.AddPair("right", Alignment.Right);
			valueMapper.AddPair("justify", Alignment.ThaiDistribute);
			valueMapper.AddPair("justify", Alignment.Distribute);
			valueMapper.AddPair("justify", Alignment.Justified);
			return valueMapper;
		}

		static ValueMapper<string, BorderStyle> InitializeBorderStyleMapper()
		{
			ValueMapper<string, BorderStyle> valueMapper = new ValueMapper<string, BorderStyle>("solid", BorderStyle.Inherit);
			valueMapper.AddPair("hidden", BorderStyle.None);
			valueMapper.AddPair("none", BorderStyle.None);
			valueMapper.AddPair("groove", BorderStyle.Single);
			valueMapper.AddPair("ridge", BorderStyle.Single);
			valueMapper.AddPair("solid", BorderStyle.Single);
			valueMapper.AddPair("dotted", BorderStyle.Dotted);
			valueMapper.AddPair("dashed", BorderStyle.Dashed);
			valueMapper.AddPair("double", BorderStyle.Double);
			valueMapper.AddPair("inset", BorderStyle.Inset);
			valueMapper.AddPair("outset", BorderStyle.Outset);
			return valueMapper;
		}

		static ValueMapper<string, Telerik.Windows.Documents.Flow.Model.Styles.FlowDirection> InitializeFlowDirectionMapper()
		{
			ValueMapper<string, Telerik.Windows.Documents.Flow.Model.Styles.FlowDirection> valueMapper = new ValueMapper<string, Telerik.Windows.Documents.Flow.Model.Styles.FlowDirection>();
			valueMapper.AddPair("rtl", Telerik.Windows.Documents.Flow.Model.Styles.FlowDirection.RightToLeft);
			valueMapper.AddPair("ltr", Telerik.Windows.Documents.Flow.Model.Styles.FlowDirection.LeftToRight);
			return valueMapper;
		}

		static ValueMapper<string, FontWeight> InitializeFontWeightMapper()
		{
			ValueMapper<string, FontWeight> valueMapper = new ValueMapper<string, FontWeight>();
			valueMapper.AddPair("bold", FontWeights.Bold);
			valueMapper.AddPair("normal", FontWeights.Normal);
			return valueMapper;
		}

		static ValueMapper<string, FontStyle> InitializeFontStyleMapper()
		{
			ValueMapper<string, FontStyle> valueMapper = new ValueMapper<string, FontStyle>();
			valueMapper.AddPair("italic", FontStyles.Italic);
			valueMapper.AddPair("normal", FontStyles.Normal);
			return valueMapper;
		}

		static ValueMapper<string, bool> InitializeStrikethroughMapper()
		{
			ValueMapper<string, bool> valueMapper = new ValueMapper<string, bool>();
			valueMapper.AddPair("line-through", true);
			valueMapper.AddPair(string.Empty, false);
			return valueMapper;
		}

		public static readonly ValueMapper<string, TableLayoutType> TableLayoutTypeMapper = HtmlValueMappers.InitializeTableLayoutTypeMapper();

		public static readonly ValueMapper<string, Telerik.Windows.Documents.Flow.Model.Styles.BaselineAlignment> BaselineAlignmentMapper = HtmlValueMappers.InitializeBaselineAlignmentMapper();

		public static readonly ValueMapper<string, VerticalAlignment> VerticalAlignmentMapper = HtmlValueMappers.InitializeVerticalAlignmentMapper();

		public static readonly ValueMapper<string, NumberingStyle> NumberingStyleTypeMapper = HtmlValueMappers.InitializeNumberingStyleTypeMapper();

		public static readonly ValueMapper<string, NumberingStyle> NumberingStyleMapper = HtmlValueMappers.InitializeNumberingStyleMapper();

		public static readonly ValueMapper<string, Alignment> AlignmentValueMapper = HtmlValueMappers.InitializeAlignmentMapper();

		public static readonly ValueMapper<string, BorderStyle> BorderStyleValueMapper = HtmlValueMappers.InitializeBorderStyleMapper();

		public static readonly ValueMapper<string, Telerik.Windows.Documents.Flow.Model.Styles.FlowDirection> FlowDirectionValueMapper = HtmlValueMappers.InitializeFlowDirectionMapper();

		public static readonly ValueMapper<string, FontWeight> FontWeightValueMapper = HtmlValueMappers.InitializeFontWeightMapper();

		public static readonly ValueMapper<string, FontStyle> FontStyleValueMapper = HtmlValueMappers.InitializeFontStyleMapper();

		public static readonly ValueMapper<string, bool> StrikethroughValueMapper = HtmlValueMappers.InitializeStrikethroughMapper();
	}
}
