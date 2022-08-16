using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Media;
using Telerik.Windows.Documents.Flow.FormatProviders.Docx.Export;
using Telerik.Windows.Documents.Flow.FormatProviders.Docx.Import;
using Telerik.Windows.Documents.Flow.FormatProviders.Docx.Utilities;
using Telerik.Windows.Documents.Flow.Model.Watermarks;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Core;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Elements;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Types;
using Telerik.Windows.Documents.Media;
using Telerik.Windows.Documents.Model.Drawing.Shapes;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Docx.Model.Elements.Vector
{
	class ShapeElement : VectorElementBase
	{
		public ShapeElement(DocxPartsManager partsManager)
			: base(partsManager)
		{
			this.id = base.RegisterAttribute<OpenXmlAttribute<string>>(new OpenXmlAttribute<string>("id", false));
			this.type = base.RegisterAttribute<OpenXmlAttribute<string>>(new OpenXmlAttribute<string>("type", false));
			this.style = base.RegisterAttribute<OpenXmlAttribute<string>>(new OpenXmlAttribute<string>("style", false));
			this.fillColor = base.RegisterAttribute<ConvertedOpenXmlAttribute<Color>>(new ConvertedOpenXmlAttribute<Color>("fillcolor", Converters.NameOrHashedHexBinary3Converter, false));
			this.stroked = base.RegisterAttribute<BoolOpenXmlAttribute>(new BoolOpenXmlAttribute("stroked"));
			this.randomGenerator = new Random();
		}

		public override string ElementName
		{
			get
			{
				return "shape";
			}
		}

		public string Id
		{
			get
			{
				return this.id.Value;
			}
			set
			{
				this.id.Value = value;
			}
		}

		public string Type
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

		public string Style
		{
			get
			{
				return this.style.Value;
			}
			set
			{
				this.style.Value = value;
			}
		}

		public Color FillColor
		{
			get
			{
				return this.fillColor.Value;
			}
			set
			{
				this.fillColor.Value = value;
			}
		}

		public bool Stroked
		{
			get
			{
				return this.stroked.Value;
			}
			set
			{
				this.stroked.Value = value;
			}
		}

		public ShapeTypes ShapeType
		{
			get
			{
				if (this.Id.Contains("PowerPlusWaterMarkObject") || this.Id.Contains("WordPictureWatermark"))
				{
					return ShapeTypes.Watermark;
				}
				if (this.imageSource != null)
				{
					return ShapeTypes.Image;
				}
				return ShapeTypes.Unknown;
			}
		}

		public void SetAssociatedWatermarkElement(Watermark watermark)
		{
			Guard.ThrowExceptionIfNull<Watermark>(watermark, "watermark");
			this.watermark = watermark;
		}

		public Image CreateImage()
		{
			ShapeStyleInfo shapeStyleInfo = this.ReadShapeStyle();
			return new Image
			{
				ImageSource = this.imageSource,
				Width = shapeStyleInfo.Width,
				Height = shapeStyleInfo.Height,
				RotationAngle = shapeStyleInfo.RotationAngle
			};
		}

		public Watermark CreateWatermark()
		{
			if (this.textWatermarkSettings == null && this.imageWatermarkSettings == null)
			{
				if (this.Id.Contains("PowerPlusWaterMarkObject"))
				{
					this.textWatermarkSettings = new TextWatermarkSettings();
				}
				else
				{
					if (!this.Id.Contains("WordPictureWatermark"))
					{
						return null;
					}
					this.imageWatermarkSettings = new ImageWatermarkSettings();
				}
			}
			if (this.textWatermarkSettings != null)
			{
				ShapeStyleInfo shapeStyleInfo = this.ReadShapeStyle();
				this.textWatermarkSettings.ForegroundColor = this.FillColor;
				this.textWatermarkSettings.Width = shapeStyleInfo.Width;
				this.textWatermarkSettings.Height = shapeStyleInfo.Height;
				this.textWatermarkSettings.Angle = shapeStyleInfo.RotationAngle;
				return new Watermark(this.textWatermarkSettings);
			}
			if (this.imageWatermarkSettings != null)
			{
				ShapeStyleInfo shapeStyleInfo = this.ReadShapeStyle();
				this.imageWatermarkSettings.Width = shapeStyleInfo.Width;
				this.imageWatermarkSettings.Height = shapeStyleInfo.Height;
				this.imageWatermarkSettings.Angle = shapeStyleInfo.RotationAngle;
				this.imageWatermarkSettings.ImageSource = this.imageSource;
				return new Watermark(this.imageWatermarkSettings);
			}
			return null;
		}

		protected override void OnBeforeWrite(IDocxExportContext context)
		{
			if (this.watermark == null)
			{
				return;
			}
			switch (this.watermark.WatermarkType)
			{
			case WatermarkType.Image:
			{
				double num = Unit.DipToPoint(this.watermark.ImageSettings.Width);
				double num2 = Unit.DipToPoint(this.watermark.ImageSettings.Height);
				this.Id = "WordPictureWatermark" + this.randomGenerator.Next();
				this.Type = "#_x0000_t75";
				this.Style = string.Format("position:absolute;margin-left:0;margin-top:0;width:{0}pt;height:{1}pt;rotation:{2};z-index:-251658752;mso-position-horizontal:center;mso-position-horizontal-relative:margin;mso-position-vertical:center;mso-position-vertical-relative:margin", num, num2, this.watermark.ImageSettings.Angle);
				return;
			}
			case WatermarkType.Text:
			{
				double num = Unit.DipToPoint(this.watermark.TextSettings.Width);
				double num2 = Unit.DipToPoint(this.watermark.TextSettings.Height);
				this.Id = "PowerPlusWaterMarkObject" + this.randomGenerator.Next();
				this.Type = "#_x0000_t136";
				this.Style = string.Format("position:absolute;margin-left:0;margin-top:0;width:{0}pt;height:{1}pt;rotation:{2};z-index:-251658752;mso-position-horizontal:center;mso-position-horizontal-relative:margin;mso-position-vertical:center;mso-position-vertical-relative:margin", num, num2, this.watermark.TextSettings.Angle);
				this.FillColor = this.watermark.TextSettings.ForegroundColor;
				this.Stroked = false;
				return;
			}
			default:
				throw new NotSupportedException(this.watermark.WatermarkType + " is unsupported watermark type.");
			}
		}

		protected override IEnumerable<OpenXmlElementBase> EnumerateChildElements(IDocxExportContext context)
		{
			if (this.watermark != null)
			{
				switch (this.watermark.WatermarkType)
				{
				case WatermarkType.Image:
				{
					ImageDataElement imageDataElement = base.CreateElement<ImageDataElement>("imagedata");
					if (this.watermark.ImageSettings.ImageSource != null)
					{
						imageDataElement.Id = this.CreateRelationshipIdFromImageSource(this.watermark.ImageSettings.ImageSource);
					}
					yield return imageDataElement;
					break;
				}
				case WatermarkType.Text:
				{
					TextpathElement textpathElement = base.CreateElement<TextpathElement>("textpath");
					textpathElement.StringAttribute = this.watermark.TextSettings.Text;
					textpathElement.Style = string.Format("font-family:&quot;{0}&quot;", this.watermark.TextSettings.FontFamily);
					yield return textpathElement;
					FillElement fillElement = base.CreateElement<FillElement>("fill");
					fillElement.Opacity = this.watermark.TextSettings.Opacity;
					yield return fillElement;
					break;
				}
				default:
					throw new NotSupportedException(this.watermark.WatermarkType + " is unsupported watermark type.");
				}
			}
			yield break;
		}

		protected override void OnAfterReadChildElement(IDocxImportContext context, OpenXmlElementBase childElement)
		{
			string elementName = childElement.ElementName;
			string a;
			if ((a = elementName) != null)
			{
				if (!(a == "textpath"))
				{
					if (a == "fill")
					{
						if (this.textWatermarkSettings == null)
						{
							this.textWatermarkSettings = new TextWatermarkSettings();
						}
						FillElement fillElement = childElement as FillElement;
						this.textWatermarkSettings.Opacity = fillElement.Opacity;
						return;
					}
					if (!(a == "imagedata"))
					{
						return;
					}
					ImageDataElement imageDataElement = childElement as ImageDataElement;
					this.imageSource = this.CreateImageSourceFromRelationshipId(imageDataElement.Id, context);
				}
				else
				{
					if (this.textWatermarkSettings == null)
					{
						this.textWatermarkSettings = new TextWatermarkSettings();
					}
					TextpathElement textpathElement = childElement as TextpathElement;
					FontFamily fontFamilyFromTextPathElementStyleString = ShapeElement.GetFontFamilyFromTextPathElementStyleString(textpathElement.Style);
					if (fontFamilyFromTextPathElementStyleString != null)
					{
						this.textWatermarkSettings.FontFamily = fontFamilyFromTextPathElementStyleString;
					}
					if (!string.IsNullOrEmpty(textpathElement.StringAttribute))
					{
						this.textWatermarkSettings.Text = textpathElement.StringAttribute;
						return;
					}
				}
			}
		}

		protected override void ClearOverride()
		{
			this.imageSource = null;
			this.watermark = null;
			this.textWatermarkSettings = null;
			this.imageWatermarkSettings = null;
		}

		static bool TryParseSize(string input, out double size)
		{
			string s = input;
			string text = string.Empty;
			if (input.Length > 2)
			{
				text = input.Substring(input.Length - 2);
			}
			if (!string.IsNullOrEmpty(text))
			{
				s = input.Remove(input.Length - 2);
			}
			bool result = false;
			string a;
			if (double.TryParse(s, NumberStyles.Any, CultureInfo.InvariantCulture, out size) && (a = text) != null)
			{
				if (!(a == "in"))
				{
					if (a == "pt")
					{
						size = Unit.PointToDip(size);
						result = true;
					}
				}
				else
				{
					size = Unit.InchToDip(size);
					result = true;
				}
			}
			return result;
		}

		static FontFamily GetFontFamilyFromTextPathElementStyleString(string styleString)
		{
			int num = styleString.IndexOf("font-family:");
			if (num >= 0)
			{
				string text = styleString.Substring(num + "font-family:".Length);
				string[] array = text.Split(new string[] { "\"", "&quot;" }, StringSplitOptions.RemoveEmptyEntries);
				if (array.Length > 0)
				{
					return new FontFamily(array[0]);
				}
			}
			return null;
		}

		string CreateRelationshipIdFromImageSource(Telerik.Windows.Documents.Media.ImageSource resource)
		{
			string target = DocxHelper.CreateResourceName(resource);
			return base.PartsManager.CreateRelationship(base.Part.Name, target, OpenXmlRelationshipTypes.GetRelationshipTypeByExtension(resource.Extension), null);
		}

		Telerik.Windows.Documents.Media.ImageSource CreateImageSourceFromRelationshipId(string relationshipId, IDocxImportContext context)
		{
			string relationshipTarget = base.PartsManager.GetRelationshipTarget(base.Part.Name, relationshipId);
			string resourceName = base.Part.GetResourceName(relationshipTarget);
			return (Telerik.Windows.Documents.Media.ImageSource)context.GetResourceByResourceKey(resourceName);
		}

		ShapeStyleInfo ReadShapeStyle()
		{
			string[] array = this.Style.Split(new char[] { ';' });
			double width = 0.0;
			double height = 0.0;
			double rotationAngle = 0.0;
			string text = "width";
			string text2 = "height";
			string text3 = "rotation";
			string text4 = ":";
			foreach (string text5 in array)
			{
				if (text5.StartsWith(text))
				{
					string input = text5.Substring(text.Length + text4.Length);
					if (!ShapeElement.TryParseSize(input, out width))
					{
						width = 0.0;
					}
				}
				else if (text5.StartsWith(text2))
				{
					string input2 = text5.Substring(text2.Length + text4.Length);
					if (!ShapeElement.TryParseSize(input2, out height))
					{
						height = 0.0;
					}
				}
				else if (text5.StartsWith(text3))
				{
					string s = text5.Substring(text3.Length + text4.Length);
					if (!double.TryParse(s, out rotationAngle))
					{
						rotationAngle = 0.0;
					}
				}
			}
			return new ShapeStyleInfo(width, height, rotationAngle);
		}

		const string ImageWatermarkId = "WordPictureWatermark";

		const string TextWatermarkId = "PowerPlusWaterMarkObject";

		const string ImageWatermarkType = "#_x0000_t75";

		const string TextWatermarkType = "#_x0000_t136";

		const string StyleString = "position:absolute;margin-left:0;margin-top:0;width:{0}pt;height:{1}pt;rotation:{2};z-index:-251658752;mso-position-horizontal:center;mso-position-horizontal-relative:margin;mso-position-vertical:center;mso-position-vertical-relative:margin";

		const string TextpathStyleString = "font-family:&quot;{0}&quot;";

		const int UnitTypeLength = 2;

		readonly Random randomGenerator;

		readonly OpenXmlAttribute<string> id;

		readonly OpenXmlAttribute<string> type;

		readonly OpenXmlAttribute<string> style;

		readonly ConvertedOpenXmlAttribute<Color> fillColor;

		readonly BoolOpenXmlAttribute stroked;

		Telerik.Windows.Documents.Media.ImageSource imageSource;

		Watermark watermark;

		TextWatermarkSettings textWatermarkSettings;

		ImageWatermarkSettings imageWatermarkSettings;
	}
}
