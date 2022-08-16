using System;
using System.Collections.Generic;
using Telerik.Windows.Documents.Core.Imaging;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.DocumentModel.Core;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.DocumentModel.Core.Fonts;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.DocumentModel.Core.XObjects;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.DocumentModel.Data;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.PdfReader.Parsers;
using Telerik.Windows.Documents.Fixed.Model.Internal;
using Telerik.Windows.Documents.Fixed.Model.Internal.Classes;
using Telerik.Windows.Documents.Fixed.Model.Resources;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.PdfReader
{
	class PdfResourceManager
	{
		public PdfResourceManager(PdfContentManager contentManager)
		{
			Guard.ThrowExceptionIfNull<PdfContentManager>(contentManager, "contentManager");
			this.localImages = new Dictionary<ImageResourceKey, InlineImageInfo>();
			this.globalImages = new Dictionary<ImageResourceKey, XImage>();
			this.xFormContents = new Dictionary<XForm, IEnumerable<ContentElement>>();
			this.localFontKeys = new Dictionary<FontBaseOld, ResourceKey>();
			this.contentManager = contentManager;
		}

		public ImageDataSource GetImageSource(ImageResourceKey resourceKey)
		{
			Guard.ThrowExceptionIfNull<ImageResourceKey>(resourceKey, "resourceKey");
			switch (resourceKey.Type)
			{
			case ResourceType.Global:
				return this.GetGlobalImageSource(resourceKey);
			case ResourceType.Local:
				return this.GetInlineImageSource(resourceKey);
			default:
				throw new NotSupportedException("Resource key type is not supported.");
			}
		}

		public ImageDataSource GetImageSourceWithStencilColor(ImageResourceKey resourceKey)
		{
			Guard.ThrowExceptionIfNull<ImageResourceKey>(resourceKey, "resourceKey");
			Guard.ThrowExceptionIfNull<Color?>(resourceKey.StencilColor, "resourceKey.StencilColor");
			if (resourceKey.Type == ResourceType.Local)
			{
				return this.GetInlineImageSource(resourceKey);
			}
			return this.GetGlobalImageSourceWithStencilColor(resourceKey);
		}

		public bool TryGetInlineImageInfo(ImageResourceKey resourceKey, out InlineImageInfo imageInfo)
		{
			return this.localImages.TryGetValue(resourceKey, out imageInfo);
		}

		public void RegisterInlineImage(InlineImageInfo imageInfo, ImageResourceKey resourceKey)
		{
			Guard.ThrowExceptionIfNull<InlineImageInfo>(imageInfo, "imageInfo");
			Guard.ThrowExceptionIfNull<ImageResourceKey>(resourceKey, "resourceKey");
			this.localImages[resourceKey] = imageInfo;
		}

		public ImageResourceKey RegisterXImage(XImage image, Color stencilColor)
		{
			Guard.ThrowExceptionIfNull<XImage>(image, "image");
			Guard.ThrowExceptionIfNull<IndirectReferenceOld>(image.Reference, "reference");
			IndirectReferenceOld reference = image.Reference;
			ImageResourceKey imageResourceKey = new ImageResourceKey();
			imageResourceKey.Id = reference.ObjectNumber;
			imageResourceKey.Type = ResourceType.Global;
			if (image.ImageMask != null && image.ImageMask.Value)
			{
				imageResourceKey.StencilColor = new Color?(stencilColor);
			}
			this.globalImages[imageResourceKey] = image;
			return imageResourceKey;
		}

		public ResourceKey GetFontKey(FontBaseOld font)
		{
			if (font.Reference != null)
			{
				return new ResourceKey
				{
					Id = font.Reference.ObjectNumber,
					Type = ResourceType.Global
				};
			}
			ResourceKey result;
			if (this.localFontKeys.TryGetValue(font, out result))
			{
				return result;
			}
			this.localFontKeys[font] = new ResourceKey
			{
				Id = this.localFontKeys.Count,
				Type = ResourceType.Local
			};
			return this.localFontKeys[font];
		}

		public IEnumerable<ContentElement> GetXFormContent(PdfResourceOld baseResources, XForm form, bool skipNonTextRelatedOffset)
		{
			Guard.ThrowExceptionIfNull<XForm>(form, "form");
			form.Load();
			IEnumerable<ContentElement> enumerable;
			if (!this.xFormContents.TryGetValue(form, out enumerable))
			{
				byte[] data = this.contentManager.ReadData(form.Reference);
				PdfResourceOld resources = ((form.Resources == null) ? baseResources : form.Resources);
				ContentStreamParser contentStreamParser = new ContentStreamParser(this.contentManager, data, form.Clip, resources, form.Reference);
				if (skipNonTextRelatedOffset)
				{
					enumerable = contentStreamParser.ParseOnlyTextRelatedContent();
				}
				else
				{
					enumerable = contentStreamParser.ParseContent();
					this.xFormContents[form] = enumerable;
				}
			}
			return enumerable;
		}

		ImageDataSource GetGlobalImageSource(ImageResourceKey resourceKey)
		{
			Guard.ThrowExceptionIfNull<ImageResourceKey>(resourceKey, "resourceKey");
			XImage ximage;
			if (!this.globalImages.TryGetValue(resourceKey, out ximage))
			{
				throw new NotSupportedException();
			}
			return ximage.CreateImageSource(ximage);
		}

		ImageDataSource GetGlobalImageSourceWithStencilColor(ImageResourceKey resourceKey)
		{
			Guard.ThrowExceptionIfNull<ImageResourceKey>(resourceKey, "resourceKey");
			Guard.ThrowExceptionIfNull<Color?>(resourceKey.StencilColor, "resourceKey.StencilColor");
			XImage ximage;
			if (!this.globalImages.TryGetValue(resourceKey, out ximage))
			{
				throw new NotSupportedException();
			}
			return ximage.CreateImageSourceWithStencilColor(ximage, resourceKey.StencilColor.Value);
		}

		ImageDataSource GetInlineImageSource(ImageResourceKey resourceKey)
		{
			InlineImageInfo inlineImageInfo = this.localImages[resourceKey];
			return (resourceKey.StencilColor != null) ? inlineImageInfo.XImage.CreateImageSourceWithStencilColor(inlineImageInfo, resourceKey.StencilColor.Value) : inlineImageInfo.XImage.CreateImageSource(inlineImageInfo);
		}

		readonly Dictionary<ImageResourceKey, InlineImageInfo> localImages;

		readonly Dictionary<ImageResourceKey, XImage> globalImages;

		readonly Dictionary<FontBaseOld, ResourceKey> localFontKeys;

		readonly Dictionary<XForm, IEnumerable<ContentElement>> xFormContents;

		readonly PdfContentManager contentManager;
	}
}
