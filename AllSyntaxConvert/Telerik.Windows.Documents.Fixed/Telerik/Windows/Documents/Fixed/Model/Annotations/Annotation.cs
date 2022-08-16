using System;
using System.Windows;
using Telerik.Windows.Documents.Core.Data;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.Utils;
using Telerik.Windows.Documents.Fixed.Model.Common;
using Telerik.Windows.Documents.Fixed.Model.Internal.Annotations;

namespace Telerik.Windows.Documents.Fixed.Model.Annotations
{
	public abstract class Annotation : FixedDocumentElementBase, IInstanceIdOwner, IContextClonable<Annotation>
	{
		internal Annotation()
		{
			this.id = InstanceIdGenerator.GetNextId();
			this.Border = AnnotationBorder.DefaultAnnotationBorder;
			this.InitializeDefaultAnnotationFlags();
		}

		public Rect Rect { get; set; }

		public abstract AnnotationType Type { get; }

		public AnnotationBorder Border { get; set; }

		public bool IsPrintable { get; set; }

		internal bool IsDisplayedWhenNotSupported { get; set; }

		internal bool IsHidden { get; set; }

		internal bool IsZoomingWithPage { get; set; }

		internal bool IsRotatedWithPage { get; set; }

		internal bool IsVisibleInViewerUI { get; set; }

		internal bool IsReadOnly { get; set; }

		internal bool IsLockedByPositionAndSize { get; set; }

		internal bool IsContentLocked { get; set; }

		internal bool IsTogglingVisibilityInViewerUI { get; set; }

		internal AnnotationAppearances Appearances { get; set; }

		internal Rect? BoundingRect { get; set; }

		internal Rect AnnotationOldRect { get; set; }

		internal AnnotationAppearancesOld AppearanceOld { get; set; }

		internal override FixedDocumentElementType ElementType
		{
			get
			{
				return FixedDocumentElementType.Annotation;
			}
		}

		int IInstanceIdOwner.InstanceId
		{
			get
			{
				return this.id;
			}
		}

		internal Rect Arrange(Matrix matrix)
		{
			this.BoundingRect = new Rect?(Helper.GetBoundingRect(this.AnnotationOldRect, matrix));
			return this.BoundingRect.Value;
		}

		internal virtual Annotation CreateClonedInstance(RadFixedDocumentCloneContext cloneContext)
		{
			throw new NotImplementedException("ClonedInstance should be created in Annotation class inheritors.");
		}

		internal virtual void DoOnAppearancesImport()
		{
		}

		void InitializeDefaultAnnotationFlags()
		{
			this.IsDisplayedWhenNotSupported = true;
			this.IsHidden = false;
			this.IsPrintable = false;
			this.IsZoomingWithPage = true;
			this.IsRotatedWithPage = true;
			this.IsVisibleInViewerUI = true;
			this.IsReadOnly = false;
			this.IsLockedByPositionAndSize = false;
			this.IsTogglingVisibilityInViewerUI = false;
			this.IsContentLocked = false;
		}

		Annotation IContextClonable<Annotation>.Clone(RadFixedDocumentCloneContext cloneContext)
		{
			Annotation annotation = this.CreateClonedInstance(cloneContext);
			if (this.Appearances != null)
			{
				annotation.Appearances = this.Appearances.Clone(cloneContext);
			}
			annotation.Border = this.Border;
			annotation.BoundingRect = this.BoundingRect;
			annotation.Rect = this.Rect;
			return annotation;
		}

		readonly int id;
	}
}
