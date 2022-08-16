using System;
using Telerik.Windows.Documents.Fixed.Model.InteractiveForms;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Fixed.Model.Annotations
{
	public abstract class Widget : Annotation
	{
		internal Widget(FormField field)
		{
			Guard.ThrowExceptionIfNull<FormField>(field, "field");
			this.field = field;
			this.HighlightingMode = HighlightingMode.InvertContentOfAnnotationRectangle;
			this.TextProperties = new VariableTextProperties(field.TextProperties);
			base.IsPrintable = true;
		}

		public HighlightingMode HighlightingMode { get; set; }

		public VariableTextProperties TextProperties
		{
			get
			{
				return this.textProperties;
			}
			set
			{
				Guard.ThrowExceptionIfNull<VariableTextProperties>(value, "value");
				this.textProperties = value;
			}
		}

		public FormField Field
		{
			get
			{
				return this.field;
			}
		}

		public abstract WidgetContentType WidgetContentType { get; }

		public override AnnotationType Type
		{
			get
			{
				return AnnotationType.Widget;
			}
		}

		internal virtual string ContentMarkerName
		{
			get
			{
				return null;
			}
		}

		public abstract void RecalculateContent();

		internal void InvalidateAppearance()
		{
			this.isAppearanceInvalidated = true;
		}

		internal sealed override void DoOnAppearancesImport()
		{
			AnnotationAppearances appearances = base.Appearances;
			base.Appearances = null;
			this.ConsumeImportedAppearances(appearances);
		}

		internal void EnsureContentIsUpToDate()
		{
			if (this.isAppearanceInvalidated)
			{
				this.isAppearanceInvalidated = false;
				this.RecalculateContent();
			}
		}

		internal sealed override Annotation CreateClonedInstance(RadFixedDocumentCloneContext cloneContext)
		{
			Widget widget = this.CreateClonedWidgetInstance(cloneContext);
			widget.TextProperties = new VariableTextProperties(this.TextProperties);
			widget.HighlightingMode = this.HighlightingMode;
			return widget;
		}

		internal abstract Widget CreateClonedWidgetInstance(RadFixedDocumentCloneContext cloneContext);

		internal abstract void ConsumeImportedAppearances(AnnotationAppearances appearances);

		readonly FormField field;

		VariableTextProperties textProperties;

		bool isAppearanceInvalidated;
	}
}
