using System;
using Telerik.Windows.Documents.Fixed.Model.Editing;
using Telerik.Windows.Documents.Fixed.Model.InteractiveForms;

namespace Telerik.Windows.Documents.Fixed.Model.Annotations
{
	public class SignatureWidget : Widget<DynamicAppearanceCharacteristics>, IContentAnnotation
	{
		internal SignatureWidget(FormField field)
			: base(field)
		{
			this.content = new AnnotationContentSource();
		}

		public sealed override WidgetContentType WidgetContentType
		{
			get
			{
				return WidgetContentType.SignatureContent;
			}
		}

		public AnnotationContentSource Content
		{
			get
			{
				base.EnsureContentIsUpToDate();
				return this.content;
			}
		}

		public sealed override void RecalculateContent()
		{
			this.Content.NormalContentSource = base.RecalculateAppearance(new Action<FixedContentEditor>(this.DrawSignatureContent));
			this.Content.MouseDownContentSource = null;
			this.Content.MouseOverContentSource = null;
		}

		internal override void ConsumeImportedAppearances(AnnotationAppearances appearances)
		{
			if (appearances.AppearancesType == AnnotationAppearancesType.SingleStateAppearances)
			{
				SingleStateAppearances singleStateAppearances = (SingleStateAppearances)appearances;
				this.Content.Initialize(singleStateAppearances);
			}
		}

		internal void PrepareAppearancesForExport()
		{
			base.Appearances = new SingleStateAppearances(this.Content);
		}

		internal sealed override Widget CreateClonedWidgetInstance(RadFixedDocumentCloneContext cloneContext)
		{
			FormField clonedField = cloneContext.GetClonedField(base.Field);
			SignatureWidget signatureWidget = new SignatureWidget(clonedField);
			signatureWidget.Content.Initialize(this.Content);
			signatureWidget.AppearanceCharacteristics = new DynamicAppearanceCharacteristics(base.AppearanceCharacteristics);
			return signatureWidget;
		}

		void DrawSignatureContent(FixedContentEditor editor)
		{
		}

		readonly AnnotationContentSource content;
	}
}
