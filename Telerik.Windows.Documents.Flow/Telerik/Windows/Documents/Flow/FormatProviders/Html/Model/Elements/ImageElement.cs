using System;
using System.IO;
using System.Windows;
using Telerik.Windows.Documents.Flow.FormatProviders.Html.Export;
using Telerik.Windows.Documents.Flow.FormatProviders.Html.Import;
using Telerik.Windows.Documents.Flow.FormatProviders.Html.Model.Attributes;
using Telerik.Windows.Documents.Flow.FormatProviders.Html.Model.Elements.Text;
using Telerik.Windows.Documents.Flow.Model.Shapes;
using Telerik.Windows.Documents.Media;
using Telerik.Windows.Documents.Model.Drawing.Shapes;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Html.Model.Elements
{
	class ImageElement : BodyElementBase, IPhrasingElement
	{
		public ImageElement(HtmlContentManager contentManager)
			: base(contentManager)
		{
			this.source = base.RegisterAttribute<string>("src", true);
			this.alternativeText = base.RegisterAttribute<string>("alt", false);
			this.title = base.RegisterAttribute<string>("title", false);
			this.width = base.RegisterAttribute<double>("width", false);
			this.height = base.RegisterAttribute<double>("height", false);
		}

		public override string Name
		{
			get
			{
				return "img";
			}
		}

		public string Source
		{
			get
			{
				return this.source.Value;
			}
			set
			{
				this.source.Value = value;
			}
		}

		public void SetAsociatedFlowElement(Image image)
		{
			Guard.ThrowExceptionIfNull<Image>(image, "image");
			this.image = image;
		}

		protected override void OnAfterReadAttributes(IHtmlReader reader, IHtmlImportContext context)
		{
			Guard.ThrowExceptionIfNull<IHtmlReader>(reader, "reader");
			Guard.ThrowExceptionIfNull<IHtmlImportContext>(context, "context");
			ImageInline imageInline = new ImageInline(context.Document);
			if (this.Source != null)
			{
				string value = this.Source.Trim(new char[] { ' ', '\t', '\n', '\f', '\r' });
				if (!string.IsNullOrEmpty(value))
				{
					ImageElement.SetImageSource(context, imageInline, value);
				}
			}
			if (this.width.HasValue)
			{
				imageInline.Image.Width = this.width.Value;
			}
			if (this.height.HasValue)
			{
				imageInline.Image.Height = this.height.Value;
			}
			context.InsertInline(imageInline);
		}

		protected override void OnBeforeWrite(IHtmlWriter writer, IHtmlExportContext context)
		{
			Guard.ThrowExceptionIfNull<IHtmlWriter>(writer, "writer");
			Guard.ThrowExceptionIfNull<IHtmlExportContext>(context, "context");
			ImageExportingEventArgs imageExportingEventArgs = new ImageExportingEventArgs(this.image);
			context.Settings.OnImageExporting(imageExportingEventArgs);
			if (!string.IsNullOrEmpty(imageExportingEventArgs.AlternativeText))
			{
				this.alternativeText.Value = imageExportingEventArgs.AlternativeText;
			}
			if (imageExportingEventArgs.ExportSize && this.image.SizeInternal != Size.Empty)
			{
				this.width.Value = this.image.Width;
				this.height.Value = this.image.Height;
			}
			if (!string.IsNullOrEmpty(imageExportingEventArgs.Title))
			{
				this.title.Value = imageExportingEventArgs.Title;
			}
			this.ExportImageSource(context, imageExportingEventArgs);
		}

		protected override void ClearOverride()
		{
			base.ClearOverride();
			this.image = null;
		}

		static void SetImageSource(IHtmlImportContext context, ImageInline imageInline, string source)
		{
			if (source.StartsWith("data:image/"))
			{
				imageInline.Image.ImageSource = ImageElement.CreateImageSourceFromBase64(source);
				return;
			}
			byte[] dataFromUri = context.Settings.GetDataFromUri(source);
			imageInline.Image.ImageSource = new UriImageSource(new Uri(source, UriKind.RelativeOrAbsolute), dataFromUri);
		}

		static ImageSource CreateImageSourceFromBase64(string source)
		{
			Guard.ThrowExceptionIfNullOrEmpty(source, "source");
			ImageSource result;
			try
			{
				source = source.Replace("data:image/", string.Empty);
				source = source.Replace("base64,", string.Empty);
				string[] array = source.Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
				result = new ImageSource(Convert.FromBase64String(array[1]), array[0]);
			}
			catch
			{
				throw new InvalidOperationException("The source is not in the required format");
			}
			return result;
		}

		static string BuildBase64Source(ImageSource imageSource)
		{
			Guard.ThrowExceptionIfNull<ImageSource>(imageSource, "imageSource");
			return string.Format(ImageElement.base64SourceFormat, imageSource.Extension, Convert.ToBase64String(imageSource.Data));
		}

		void ExportImageSource(IHtmlExportContext context, ImageExportingEventArgs args)
		{
			switch (context.Settings.ImagesExportMode)
			{
			case ImagesExportMode.Embedded:
			{
				ImageSource imageSource = this.image.ImageSource;
				UriImageSource uriImageSource = imageSource as UriImageSource;
				if (uriImageSource != null)
				{
					this.source.Value = uriImageSource.Uri.OriginalString;
					return;
				}
				this.source.Value = ImageElement.BuildBase64Source(imageSource);
				return;
			}
			case ImagesExportMode.External:
			{
				if (args.Handled)
				{
					Guard.ThrowExceptionIfNull<string>(args.Source, "Source");
					this.source.Value = args.Source;
					return;
				}
				UriImageSource uriImageSource2 = this.image.ImageSource as UriImageSource;
				if (uriImageSource2 != null)
				{
					this.source.Value = uriImageSource2.Uri.OriginalString;
					return;
				}
				this.ExportImageExternal(context);
				return;
			}
			default:
				throw new NotSupportedException(string.Format("Unsupported {0}.", typeof(ImagesExportMode).Name));
			}
		}

		void ExportImageExternal(IHtmlExportContext context)
		{
			int id = this.image.ImageSource.Id;
			string str;
			if (!context.ImageRepository.TryGetImageName(id, out str))
			{
				if (context.Settings.ImagesFolderPath == null)
				{
					throw new NotSupportedException("When exporting image to external file, HtmlExportSettings.ImagesFolderPath should be set, or HtmlExportSettings.ExternalStylesExport should be marked as handled.");
				}
				str = context.ImageRepository.RegisterImage(id);
				byte[] data = this.image.ImageSource.Data;
				File.WriteAllBytes(Path.Combine(context.Settings.ImagesFolderPath, str + "." + this.image.ImageSource.Extension), data);
			}
			this.source.Value = Path.Combine(context.Settings.ImagesSourceBasePath, str + "." + this.image.ImageSource.Extension);
		}

		const string DataPrefix = "data:image/";

		const string Base64Prefix = "base64,";

		const string Delimiter = ";";

		static readonly string base64SourceFormat = "data:image/{0};base64,{1}";

		readonly HtmlAttribute<string> source;

		readonly HtmlAttribute<string> alternativeText;

		readonly HtmlAttribute<string> title;

		readonly HtmlAttribute<double> width;

		readonly HtmlAttribute<double> height;

		Image image;
	}
}
