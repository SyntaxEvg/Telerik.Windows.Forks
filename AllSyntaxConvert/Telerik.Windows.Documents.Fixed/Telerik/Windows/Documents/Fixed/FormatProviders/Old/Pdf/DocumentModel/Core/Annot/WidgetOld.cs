using System;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.DocumentModel.Core.Forms;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.DocumentModel.Data;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.DocumentModel.PdfConverters;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.PdfReader;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.Utils;
using Telerik.Windows.Documents.Fixed.Model.Annotations;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.DocumentModel.Core.Annot
{
	[PdfClass(TypeName = "Annot", SubtypeProperty = "Subtype", SubtypeValue = "Widget")]
	class WidgetOld : AnnotationOld
	{
		public WidgetOld(PdfContentManager contentManager)
			: base(contentManager)
		{
			this.highlightingMode = base.CreateInstantLoadProperty<PdfNameOld>(new PdfPropertyDescriptor("H"));
			this.appearanceCharacteristics = base.CreateLoadOnDemandProperty<AppearanceCharacteristicsOld>(new PdfPropertyDescriptor("MK"), Converters.PdfDictionaryToPdfObjectConverter);
			this.actions = base.CreateLoadOnDemandProperty<PdfDictionaryOld>(new PdfPropertyDescriptor("A"));
			this.additionalActions = base.CreateLoadOnDemandProperty<PdfDictionaryOld>(new PdfPropertyDescriptor("AA"));
			this.borderStyle = base.CreateLoadOnDemandProperty<PdfDictionaryOld>(new PdfPropertyDescriptor("BS"));
			this.parentField = base.CreateLoadOnDemandProperty<FormFieldNodeOld>(new PdfPropertyDescriptor("Parent"));
		}

		public override AnnotationType Type
		{
			get
			{
				return AnnotationType.Widget;
			}
		}

		public FormFieldNodeOld FormField
		{
			get
			{
				return this.parentField.GetValue();
			}
			set
			{
				this.parentField.SetValue(value);
			}
		}

		public PdfNameOld HighlightingMode
		{
			get
			{
				return this.highlightingMode.GetValue();
			}
			set
			{
				this.highlightingMode.SetValue(value);
			}
		}

		public AppearanceCharacteristicsOld AppearanceCharacteristics
		{
			get
			{
				return this.appearanceCharacteristics.GetValue();
			}
			set
			{
				this.appearanceCharacteristics.SetValue(value);
			}
		}

		public PdfDictionaryOld Actions
		{
			get
			{
				return this.actions.GetValue();
			}
			set
			{
				this.actions.SetValue(value);
			}
		}

		public PdfDictionaryOld AdditionalActions
		{
			get
			{
				return this.additionalActions.GetValue();
			}
			set
			{
				this.additionalActions.SetValue(value);
			}
		}

		public PdfDictionaryOld BorderStyle
		{
			get
			{
				return this.borderStyle.GetValue();
			}
			set
			{
				this.borderStyle.SetValue(value);
			}
		}

		readonly InstantLoadProperty<PdfNameOld> highlightingMode;

		readonly LoadOnDemandProperty<AppearanceCharacteristicsOld> appearanceCharacteristics;

		readonly LoadOnDemandProperty<PdfDictionaryOld> actions;

		readonly LoadOnDemandProperty<PdfDictionaryOld> additionalActions;

		readonly LoadOnDemandProperty<PdfDictionaryOld> borderStyle;

		readonly LoadOnDemandProperty<FormFieldNodeOld> parentField;
	}
}
