using System;
using System.Windows;
using System.Windows.Media;
using Telerik.Windows.Documents.Flow.Model.Cloning;
using Telerik.Windows.Documents.Flow.Model.Styles;
using Telerik.Windows.Documents.Flow.Model.Styles.Core;
using Telerik.Windows.Documents.Spreadsheet.Model;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Flow.Model
{
	public sealed class Run : InlineBase, IElementWithStyle, IElementWithProperties
	{
		public Run(RadFlowDocument document)
			: base(document)
		{
			this.text = string.Empty;
			this.properties = new CharacterProperties(this);
			this.Shading = new Shading(this.properties);
			this.Underline = new Underline(this.properties);
		}

		DocumentElementPropertiesBase IElementWithProperties.Properties
		{
			get
			{
				return this.properties;
			}
		}

		public CharacterProperties Properties
		{
			get
			{
				return this.properties;
			}
		}

		public string Text
		{
			get
			{
				return this.text;
			}
			set
			{
				Guard.ThrowExceptionIfNull<string>(value, "value");
				this.text = TextHelper.RemoveNewLines(value);
			}
		}

		public string StyleId
		{
			get
			{
				return this.Properties.StyleId;
			}
			set
			{
				this.Properties.StyleId = value;
			}
		}

		public ThemableFontFamily FontFamily
		{
			get
			{
				return this.properties.FontFamily.GetActualValue();
			}
			set
			{
				this.properties.FontFamily.LocalValue = value;
			}
		}

		public double FontSize
		{
			get
			{
				return this.Properties.FontSize.GetActualValue().Value;
			}
			set
			{
				this.Properties.FontSize.LocalValue = new double?(value);
			}
		}

		public Shading Shading { get; set; }

		public FontStyle FontStyle
		{
			get
			{
				return this.Properties.FontStyle.GetActualValue().Value;
			}
			set
			{
				this.Properties.FontStyle.LocalValue = new FontStyle?(value);
			}
		}

		public FontWeight FontWeight
		{
			get
			{
				return this.Properties.FontWeight.GetActualValue().Value;
			}
			set
			{
				this.Properties.FontWeight.LocalValue = new FontWeight?(value);
			}
		}

		public ThemableColor ForegroundColor
		{
			get
			{
				return this.Properties.ForegroundColor.GetActualValue();
			}
			set
			{
				this.Properties.ForegroundColor.LocalValue = value;
			}
		}

		public Color HighlightColor
		{
			get
			{
				return this.Properties.HighlightColor.GetActualValue().Value;
			}
			set
			{
				this.Properties.HighlightColor.LocalValue = new Color?(value);
			}
		}

		public Telerik.Windows.Documents.Flow.Model.Styles.BaselineAlignment BaselineAlignment
		{
			get
			{
				return this.Properties.BaselineAlignment.GetActualValue().Value;
			}
			set
			{
				this.Properties.BaselineAlignment.LocalValue = new Telerik.Windows.Documents.Flow.Model.Styles.BaselineAlignment?(value);
			}
		}

		public bool Strikethrough
		{
			get
			{
				return this.Properties.Strikethrough.GetActualValue().Value;
			}
			set
			{
				this.Properties.Strikethrough.LocalValue = new bool?(value);
			}
		}

		public Underline Underline { get; set; }

		public Telerik.Windows.Documents.Flow.Model.Styles.FlowDirection FlowDirection
		{
			get
			{
				return this.Properties.FlowDirection.GetActualValue().Value;
			}
			set
			{
				this.Properties.FlowDirection.LocalValue = new Telerik.Windows.Documents.Flow.Model.Styles.FlowDirection?(value);
			}
		}

		internal override DocumentElementType Type
		{
			get
			{
				return DocumentElementType.Run;
			}
		}

		public Run Clone()
		{
			return this.CloneInternal(null);
		}

		public Run Clone(RadFlowDocument document)
		{
			Guard.ThrowExceptionIfNull<RadFlowDocument>(document, "document");
			return this.CloneInternal(document);
		}

		public override string ToString()
		{
			return this.Text;
		}

		internal override DocumentElementBase CloneCore(CloneContext cloneContext)
		{
			Guard.ThrowExceptionIfNull<CloneContext>(cloneContext, "cloneContext");
			Run run = new Run(cloneContext.Document);
			run.text = this.text;
			run.Properties.CopyPropertiesFrom(this.Properties);
			if (cloneContext.RenamedStyles.ContainsKey(run.StyleId))
			{
				run.StyleId = cloneContext.RenamedStyles[run.StyleId];
			}
			return run;
		}

		internal override string GetChildrenDebuggerDisplayText()
		{
			return DocumentElementBase.DebugVisualizer.EscapeXmlContent(this.Text);
		}

		Run CloneInternal(RadFlowDocument document)
		{
			return (Run)this.CloneCore(new CloneContext(document ?? this.Document));
		}

		static Run()
		{
			Run.FontSizePropertyDefinition.Validation.AddRule(new ValidationRule<double>((double value) => value > 0.0));
			Run.FontWeightPropertyDefinition.Validation.AddRule(new ValidationRule<FontWeight>((FontWeight value) => value == FontWeights.Bold || value == FontWeights.Normal));
			Run.FontStylePropertyDefinition.Validation.AddRule(new ValidationRule<FontStyle>((FontStyle value) => value == FontStyles.Italic || value == FontStyles.Normal));
		}

		readonly CharacterProperties properties;

		string text;

		public static readonly StylePropertyDefinition<double?> FontSizePropertyDefinition = new StylePropertyDefinition<double?>("FontSize", new double?(DocumentDefaultStyleSettings.FontSize), StylePropertyType.Character);

		public static readonly StylePropertyDefinition<ThemableFontFamily> FontFamilyPropertyDefinition = new StylePropertyDefinition<ThemableFontFamily>("FontFamily", DocumentDefaultStyleSettings.FontFamily, StylePropertyType.Character);

		public static readonly StylePropertyDefinition<FontStyle?> FontStylePropertyDefinition = new StylePropertyDefinition<FontStyle?>("FontStyle", new FontStyle?(DocumentDefaultStyleSettings.FontStyle), StylePropertyType.Character);

		public static readonly StylePropertyDefinition<FontWeight?> FontWeightPropertyDefinition = new StylePropertyDefinition<FontWeight?>("FontWeight", new FontWeight?(DocumentDefaultStyleSettings.FontWeight), StylePropertyType.Character);

		public static readonly StylePropertyDefinition<ThemableColor> ForegroundColorPropertyDefinition = new StylePropertyDefinition<ThemableColor>("ForegroundColor", DocumentDefaultStyleSettings.ForegroundColor, StylePropertyType.Character);

		public static readonly StylePropertyDefinition<Color?> HighlightColorPropertyDefinition = new StylePropertyDefinition<Color?>("HighlightColor", new Color?(DocumentDefaultStyleSettings.HighlightColor), StylePropertyType.Character);

		public static readonly StylePropertyDefinition<Telerik.Windows.Documents.Flow.Model.Styles.BaselineAlignment?> BaselineAlignmentPropertyDefinition = new StylePropertyDefinition<Telerik.Windows.Documents.Flow.Model.Styles.BaselineAlignment?>("BaselineAlignment", new Telerik.Windows.Documents.Flow.Model.Styles.BaselineAlignment?(DocumentDefaultStyleSettings.BaselineAlignment), StylePropertyType.Character);

		public static readonly StylePropertyDefinition<bool?> StrikethroughPropertyDefinition = new StylePropertyDefinition<bool?>("Strikethrough", new bool?(DocumentDefaultStyleSettings.Strikethrough), StylePropertyType.Character);

		public static readonly StylePropertyDefinition<ThemableColor> BackgroundColorPropertyDefinition = new StylePropertyDefinition<ThemableColor>("BackgroundColor", DocumentDefaultStyleSettings.BackgroundColor, StylePropertyType.Character);

		public static readonly StylePropertyDefinition<ThemableColor> ShadingPatternColorPropertyDefinition = new StylePropertyDefinition<ThemableColor>("ShadingPatternColor", DocumentDefaultStyleSettings.ShadingPatternColor, StylePropertyType.Character);

		public static readonly StylePropertyDefinition<ShadingPattern?> ShadingPatternPropertyDefinition = new StylePropertyDefinition<ShadingPattern?>("ShadingPattern", new ShadingPattern?(DocumentDefaultStyleSettings.ShadingPattern), StylePropertyType.Character);

		public static readonly StylePropertyDefinition<ThemableColor> UnderlineColorPropertyDefinition = new StylePropertyDefinition<ThemableColor>("UnderlineColor", DocumentDefaultStyleSettings.UnderlineColor, StylePropertyType.Character);

		public static readonly StylePropertyDefinition<UnderlinePattern?> UnderlinePatternPropertyDefinition = new StylePropertyDefinition<UnderlinePattern?>("UnderlinePattern", new UnderlinePattern?(DocumentDefaultStyleSettings.UnderlinePattern), StylePropertyType.Character);

		public static readonly StylePropertyDefinition<Telerik.Windows.Documents.Flow.Model.Styles.FlowDirection?> FlowDirectionPropertyDefinition = new StylePropertyDefinition<Telerik.Windows.Documents.Flow.Model.Styles.FlowDirection?>("FlowDirection", new Telerik.Windows.Documents.Flow.Model.Styles.FlowDirection?(DocumentDefaultStyleSettings.FlowDirection), StylePropertyType.Character);
	}
}
