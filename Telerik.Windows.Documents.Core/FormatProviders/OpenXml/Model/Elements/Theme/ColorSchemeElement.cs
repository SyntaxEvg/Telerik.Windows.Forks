using System;
using System.Windows.Media;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Export;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Import;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Core;
using Telerik.Windows.Documents.Spreadsheet.Theming;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Elements.Theme
{
	class ColorSchemeElement : ThemeElementBase
	{
		public ColorSchemeElement(OpenXmlPartsManager partsManager)
			: base(partsManager)
		{
			this.name = base.RegisterAttribute<string>("name", true);
			this.dark1 = base.RegisterChildElement<Dark1Element>("dk1");
			this.light1 = base.RegisterChildElement<Light1Element>("lt1");
			this.dark2 = base.RegisterChildElement<Dark2Element>("dk2");
			this.light2 = base.RegisterChildElement<Light2Element>("lt2");
			this.accent1 = base.RegisterChildElement<Accent1Element>("accent1");
			this.accent2 = base.RegisterChildElement<Accent2Element>("accent2");
			this.accent3 = base.RegisterChildElement<Accent3Element>("accent3");
			this.accent4 = base.RegisterChildElement<Accent4Element>("accent4");
			this.accent5 = base.RegisterChildElement<Accent5Element>("accent5");
			this.accent6 = base.RegisterChildElement<Accent6Element>("accent6");
			this.hyperlink = base.RegisterChildElement<HlinkElement>("hlink");
			this.followedHyperlink = base.RegisterChildElement<FollowedHyperlinkElement>("folHlink");
		}

		public override OpenXmlNamespace Namespace
		{
			get
			{
				return OpenXmlNamespaces.DrawingMLNamespace;
			}
		}

		public override string ElementName
		{
			get
			{
				return "clrScheme";
			}
		}

		public string Name
		{
			get
			{
				return this.name.Value;
			}
			set
			{
				this.name.Value = value;
			}
		}

		public Dark1Element Dark1Element
		{
			get
			{
				return this.dark1.Element;
			}
			set
			{
				this.dark1.Element = value;
			}
		}

		public Light1Element Light1Element
		{
			get
			{
				return this.light1.Element;
			}
			set
			{
				this.light1.Element = value;
			}
		}

		public Dark2Element Dark2Element
		{
			get
			{
				return this.dark2.Element;
			}
			set
			{
				this.dark2.Element = value;
			}
		}

		public Light2Element Light2Element
		{
			get
			{
				return this.light2.Element;
			}
			set
			{
				this.light2.Element = value;
			}
		}

		public Accent1Element Accent1Element
		{
			get
			{
				return this.accent1.Element;
			}
			set
			{
				this.accent1.Element = value;
			}
		}

		public Accent2Element Accent2Element
		{
			get
			{
				return this.accent2.Element;
			}
			set
			{
				this.accent2.Element = value;
			}
		}

		public Accent3Element Accent3Element
		{
			get
			{
				return this.accent3.Element;
			}
			set
			{
				this.accent3.Element = value;
			}
		}

		public Accent4Element Accent4Element
		{
			get
			{
				return this.accent4.Element;
			}
			set
			{
				this.accent4.Element = value;
			}
		}

		public Accent5Element Accent5Element
		{
			get
			{
				return this.accent5.Element;
			}
			set
			{
				this.accent5.Element = value;
			}
		}

		public Accent6Element Accent6Element
		{
			get
			{
				return this.accent6.Element;
			}
			set
			{
				this.accent6.Element = value;
			}
		}

		public HlinkElement HyperlinkElement
		{
			get
			{
				return this.hyperlink.Element;
			}
			set
			{
				this.hyperlink.Element = value;
			}
		}

		public FollowedHyperlinkElement FollowedHyperlinkElement
		{
			get
			{
				return this.followedHyperlink.Element;
			}
			set
			{
				this.followedHyperlink.Element = value;
			}
		}

		public ThemeColorScheme CreateColorScheme(IOpenXmlImportContext context)
		{
			Guard.ThrowExceptionIfNull<IOpenXmlImportContext>(context, "context");
			Color text = this.Dark1Element.ToColor(context);
			Color background = this.Light1Element.ToColor(context);
			Color text2 = this.Dark2Element.ToColor(context);
			Color background2 = this.Light2Element.ToColor(context);
			Color color = this.Accent1Element.ToColor(context);
			Color color2 = this.Accent2Element.ToColor(context);
			Color color3 = this.Accent3Element.ToColor(context);
			Color color4 = this.Accent4Element.ToColor(context);
			Color color5 = this.Accent5Element.ToColor(context);
			Color color6 = this.Accent6Element.ToColor(context);
			Color color7 = this.HyperlinkElement.ToColor(context);
			Color color8 = this.FollowedHyperlinkElement.ToColor(context);
			base.ReleaseElement(this.dark1);
			base.ReleaseElement(this.light1);
			base.ReleaseElement(this.dark2);
			base.ReleaseElement(this.light2);
			base.ReleaseElement(this.accent1);
			base.ReleaseElement(this.accent2);
			base.ReleaseElement(this.accent3);
			base.ReleaseElement(this.accent4);
			base.ReleaseElement(this.accent5);
			base.ReleaseElement(this.accent6);
			base.ReleaseElement(this.hyperlink);
			base.ReleaseElement(this.followedHyperlink);
			return new ThemeColorScheme(this.Name, background, text, background2, text2, color, color2, color3, color4, color5, color6, color7, color8);
		}

		protected override void OnBeforeWrite(IOpenXmlExportContext context)
		{
			Guard.ThrowExceptionIfNull<IOpenXmlExportContext>(context, "context");
			this.Name = context.Theme.ColorScheme.Name;
			base.CreateElement(this.dark1);
			base.CreateElement(this.light1);
			base.CreateElement(this.dark2);
			base.CreateElement(this.light2);
			base.CreateElement(this.accent1);
			base.CreateElement(this.accent2);
			base.CreateElement(this.accent3);
			base.CreateElement(this.accent4);
			base.CreateElement(this.accent5);
			base.CreateElement(this.accent6);
			base.CreateElement(this.hyperlink);
			base.CreateElement(this.followedHyperlink);
		}

		readonly OpenXmlAttribute<string> name;

		readonly OpenXmlChildElement<Dark1Element> dark1;

		readonly OpenXmlChildElement<Light1Element> light1;

		readonly OpenXmlChildElement<Dark2Element> dark2;

		readonly OpenXmlChildElement<Light2Element> light2;

		readonly OpenXmlChildElement<Accent1Element> accent1;

		readonly OpenXmlChildElement<Accent2Element> accent2;

		readonly OpenXmlChildElement<Accent3Element> accent3;

		readonly OpenXmlChildElement<Accent4Element> accent4;

		readonly OpenXmlChildElement<Accent5Element> accent5;

		readonly OpenXmlChildElement<Accent6Element> accent6;

		readonly OpenXmlChildElement<HlinkElement> hyperlink;

		readonly OpenXmlChildElement<FollowedHyperlinkElement> followedHyperlink;
	}
}
