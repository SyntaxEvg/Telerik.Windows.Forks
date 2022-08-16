using System;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Import.Parser;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.ColorSpaces;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.DocumentStructure;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.Fonts;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.GraphicsState;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.Objects;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.Patterns;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Types;
using Telerik.Windows.Documents.Fixed.Model;
using Telerik.Windows.Documents.Fixed.Model.Common;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Import
{
	class PdfContentImportContext : IPdfContentImportContext
	{
		public PdfContentImportContext(IRadFixedDocumentImportContext owner, IResourceHolder resourceHolder, IContentRootElement contentRoot)
		{
			Guard.ThrowExceptionIfNull<IRadFixedDocumentImportContext>(owner, "owner");
			Guard.ThrowExceptionIfNull<IResourceHolder>(resourceHolder, "resourceHolder");
			Guard.ThrowExceptionIfNull<IContentRootElement>(contentRoot, "contentRoot");
			this.owner = owner;
			this.resourceHolder = resourceHolder;
			this.contentRoot = contentRoot;
		}

		public IRadFixedDocumentImportContext Owner
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

		public Marker CurrentMarker { get; set; }

		public FontObject GetFont(PostScriptReader reader, PdfName key)
		{
			Guard.ThrowExceptionIfNull<PostScriptReader>(reader, "reader");
			Guard.ThrowExceptionIfNull<PdfName>(key, "key");
			FontObject result;
			if (this.ResourceHolder.Resources != null && this.ResourceHolder.Resources.Fonts != null && this.ResourceHolder.Resources.Fonts.TryGetElement<FontObject>(reader, this.Owner, key.Value, out result))
			{
				return result;
			}
			return null;
		}

		public XObjectBase GetXObject(PostScriptReader reader, PdfName key)
		{
			Guard.ThrowExceptionIfNull<PostScriptReader>(reader, "reader");
			Guard.ThrowExceptionIfNull<PdfName>(key, "key");
			XObjectBase result;
			this.ResourceHolder.Resources.XObjects.TryGetElement<XObjectBase>(reader, this.Owner, key.Value, out result);
			return result;
		}

		public ExtGStateObject GetExtGState(PostScriptReader reader, PdfName key)
		{
			Guard.ThrowExceptionIfNull<PostScriptReader>(reader, "reader");
			Guard.ThrowExceptionIfNull<PdfName>(key, "key");
			ExtGStateObject result;
			this.ResourceHolder.Resources.ExtGState.TryGetElement<ExtGStateObject>(reader, this.Owner, key.Value, out result);
			return result;
		}

		public PatternColorObject GetPatternColor(PostScriptReader reader, PdfName key)
		{
			Guard.ThrowExceptionIfNull<PostScriptReader>(reader, "reader");
			Guard.ThrowExceptionIfNull<PdfName>(key, "key");
			PatternColorObject result;
			this.ResourceHolder.Resources.Patterns.TryGetElement<PatternColorObject>(reader, this.Owner, key.Value, out result);
			return result;
		}

		public ColorSpaceObject GetColorSpace(PostScriptReader reader, PdfName key)
		{
			Guard.ThrowExceptionIfNull<PostScriptReader>(reader, "reader");
			Guard.ThrowExceptionIfNull<PdfName>(key, "key");
			ColorSpaceObject result;
			this.ResourceHolder.Resources.ColorSpaces.TryGetElement<ColorSpaceObject>(reader, this.Owner, key.Value, out result);
			return result;
		}

		readonly IRadFixedDocumentImportContext owner;

		readonly IResourceHolder resourceHolder;

		readonly IContentRootElement contentRoot;
	}
}
