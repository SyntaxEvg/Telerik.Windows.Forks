using System;
using System.Collections.Generic;
using System.Windows.Media;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Data;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.DocumentStructure;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Types;
using Telerik.Windows.Documents.Fixed.Model;
using Telerik.Windows.Documents.Fixed.Model.ColorSpaces;
using Telerik.Windows.Documents.Fixed.Model.Common;
using Telerik.Windows.Documents.Fixed.Model.Fonts;
using Telerik.Windows.Documents.Fixed.Model.Resources;
using Telerik.Windows.Documents.Fixed.Model.Text;
using Telerik.Windows.Documents.Fixed.Utilities.Rendering;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Export
{
	class PdfContentExportContext : IPdfContentExportContext
	{
		public PdfContentExportContext(IPdfExportContext owner, IResourceHolder resourceHolder, IContentRootElement contentRoot)
		{
			Guard.ThrowExceptionIfNull<IPdfExportContext>(owner, "owner");
			Guard.ThrowExceptionIfNull<IResourceHolder>(resourceHolder, "resourceHolder");
			Guard.ThrowExceptionIfNull<IContentRootElement>(contentRoot, "contentRoot");
			this.owner = owner;
			this.resourceHolder = resourceHolder;
			this.contentRoot = contentRoot;
			this.rootDipToPointTransform = owner.GetDipToPdfPointTransformation(this.contentRoot);
			this.graph = new ContentElementsGraph(this.contentRoot);
		}

		public IPdfExportContext Owner
		{
			get
			{
				return this.owner;
			}
		}

		public IResourceHolder ResourceHolder
		{
			get
			{
				return this.resourceHolder;
			}
		}

		public IContentRootElement ContentRoot
		{
			get
			{
				return this.contentRoot;
			}
		}

		public Matrix RootDipToPointTransformation
		{
			get
			{
				return this.rootDipToPointTransform;
			}
		}

		public MarkedContentExportInfo GetCurrentContentMarkerExportInfo(Marker currentContentMarker)
		{
			MarkedContentExportInfo markedContentExportInfo = new MarkedContentExportInfo();
			bool flag = currentContentMarker != this.previousContentMarker;
			markedContentExportInfo.ShouldExportMarkedContentStart = flag && currentContentMarker != null;
			markedContentExportInfo.ShouldExportMarkedContentEnd = flag && this.previousContentMarker != null;
			this.previousContentMarker = currentContentMarker;
			return markedContentExportInfo;
		}

		public ResourceEntry GetResource(FontBase font)
		{
			Guard.ThrowExceptionIfNull<FontBase>(font, "font");
			if (this.resourceHolder.Resources.Fonts == null)
			{
				this.resourceHolder.Resources.Fonts = new PdfDictionary();
			}
			ResourceEntry resource = this.Owner.GetResource(font);
			this.resourceHolder.Resources.Fonts[resource.ResourceKey] = resource.Resource.Reference;
			return resource;
		}

		public ResourceEntry GetResource(FormSource formSource)
		{
			Guard.ThrowExceptionIfNull<FormSource>(formSource, "formSource");
			ResourceEntry resource = this.Owner.GetResource(formSource);
			this.AddXObjectsResourceEntry(resource);
			return resource;
		}

		public ResourceEntry GetResource(Telerik.Windows.Documents.Fixed.Model.Resources.ImageSource imageSource)
		{
			Guard.ThrowExceptionIfNull<Telerik.Windows.Documents.Fixed.Model.Resources.ImageSource>(imageSource, "imageSource");
			ResourceEntry resource = this.Owner.GetResource(imageSource);
			this.AddXObjectsResourceEntry(resource);
			return resource;
		}

		public ResourceEntry GetResource(PatternColor pattern)
		{
			Guard.ThrowExceptionIfNull<PatternColor>(pattern, "pattern");
			if (this.resourceHolder.Resources.Patterns == null)
			{
				this.resourceHolder.Resources.Patterns = new PdfDictionary();
			}
			ResourceEntry resource = this.Owner.GetResource(pattern, this);
			this.resourceHolder.Resources.Patterns[resource.ResourceKey] = resource.Resource.Reference;
			return resource;
		}

		public ResourceEntry GetResource(ColorSpaceBase first, ColorSpaceBase second)
		{
			Guard.ThrowExceptionIfNull<ColorSpaceBase>(first, "first");
			Guard.ThrowExceptionIfNull<ColorSpaceBase>(second, "second");
			if (this.resourceHolder.Resources.ColorSpaces == null)
			{
				this.resourceHolder.Resources.ColorSpaces = new PdfDictionary();
			}
			ResourceEntry resource = this.Owner.GetResource(first, second);
			this.resourceHolder.Resources.ColorSpaces[resource.ResourceKey] = resource.Resource.Reference;
			return resource;
		}

		public ResourceEntry GetResource(ExtGState state)
		{
			Guard.ThrowExceptionIfNull<ExtGState>(state, "state");
			if (this.resourceHolder.Resources.ExtGState == null)
			{
				this.resourceHolder.Resources.ExtGState = new PdfDictionary();
			}
			ResourceEntry resource = this.Owner.GetResource(state);
			this.resourceHolder.Resources.ExtGState[resource.ResourceKey] = resource.Resource.Reference;
			return resource;
		}

		public void SetUsedCharacters(FontBase font, TextCollection glyphs)
		{
			Guard.ThrowExceptionIfNull<FontBase>(font, "font");
			Guard.ThrowExceptionIfNull<TextCollection>(glyphs, "glyphs");
			this.Owner.SetUsedCharacters(font, glyphs);
		}

		public IEnumerable<CharInfo> GetUsedCharacters(FontBase font)
		{
			Guard.ThrowExceptionIfNull<FontBase>(font, "font");
			return this.Owner.GetUsedCharacters(font);
		}

		public IEnumerable<ContentElementBase> GetClippingChildren(Clipping clipping)
		{
			return this.graph.GetClippingChildren(clipping);
		}

		public IEnumerable<ContentElementBase> GetRootElements()
		{
			return this.graph.GetRootElements();
		}

		void AddXObjectsResourceEntry(ResourceEntry resource)
		{
			if (this.resourceHolder.Resources.XObjects == null)
			{
				this.resourceHolder.Resources.XObjects = new PdfDictionary();
			}
			this.resourceHolder.Resources.XObjects[resource.ResourceKey] = resource.Resource.Reference;
		}

		readonly IPdfExportContext owner;

		readonly IResourceHolder resourceHolder;

		readonly IContentRootElement contentRoot;

		readonly Matrix rootDipToPointTransform;

		readonly ContentElementsGraph graph;

		Marker previousContentMarker;
	}
}
