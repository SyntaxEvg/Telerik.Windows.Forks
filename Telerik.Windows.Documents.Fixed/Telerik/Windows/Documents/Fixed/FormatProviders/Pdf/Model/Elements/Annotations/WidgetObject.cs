using System;
using System.Collections.Generic;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Export;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Import;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Import.Parser;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Data;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.Forms;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.Objects;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Types;
using Telerik.Windows.Documents.Fixed.Model;
using Telerik.Windows.Documents.Fixed.Model.Annotations;
using Telerik.Windows.Documents.Fixed.Model.InteractiveForms;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.Annotations
{
	class WidgetObject : AnnotationObject, IVariableTextPropertiesObject
	{
		public WidgetObject()
		{
			this.parentField = base.RegisterReferenceProperty<FormFieldNode>(new PdfPropertyDescriptor("Parent", true, PdfPropertyRestrictions.MustBeIndirectReference));
			this.highlightingMode = base.RegisterDirectProperty<PdfName>(new PdfPropertyDescriptor("H"), new PdfName("I"));
			this.dynamicAppearanceCharacteristics = base.RegisterReferenceProperty<AppearanceCharacteristics>(new PdfPropertyDescriptor("MK"));
			this.actions = base.RegisterReferenceProperty<PdfDictionary>(new PdfPropertyDescriptor("A"));
			this.additionalActions = base.RegisterReferenceProperty<PdfDictionary>(new PdfPropertyDescriptor("AA"));
			this.defaultAppearance = base.RegisterReferenceProperty<PdfString>(new PdfPropertyDescriptor("DA"));
			this.quadding = base.RegisterDirectProperty<PdfInt>(new PdfPropertyDescriptor("Q"));
		}

		public FormFieldNode FormField
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

		public PdfName HighlightingMode
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

		public AppearanceCharacteristics DynamicAppearanceCharacteristics
		{
			get
			{
				return this.dynamicAppearanceCharacteristics.GetValue();
			}
			set
			{
				this.dynamicAppearanceCharacteristics.SetValue(value);
			}
		}

		public PdfDictionary Actions
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

		public PdfDictionary AdditionalActions
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

		public PdfString GetDefaultAppearance(AcroFormObject form)
		{
			PdfString value = this.defaultAppearance.GetValue();
			if (value == null)
			{
				value = this.FormField.GetDefaultAppearance(form);
			}
			return value;
		}

		public void SetDefaultAppearance(PdfString value)
		{
			this.defaultAppearance.SetValue(value);
		}

		public PdfInt GetQuadding(AcroFormObject form)
		{
			PdfInt value = this.quadding.GetValue();
			if (value == null)
			{
				value = this.FormField.GetQuadding(form);
			}
			return value;
		}

		public void SetQuadding(PdfInt value)
		{
			this.quadding.SetValue(value);
		}

		public void CopyPropertiesFrom(IPdfExportContext context, Widget annotation, RadFixedPage fixedPage)
		{
			Guard.ThrowExceptionIfNull<IPdfExportContext>(context, "context");
			Guard.ThrowExceptionIfNull<Widget>(annotation, "annotation");
			Guard.ThrowExceptionIfNull<RadFixedPage>(fixedPage, "fixedPage");
			base.CopyPropertiesFrom(context, annotation, fixedPage);
			this.FormField = context.CreateFormFieldObject(annotation.Field, false);
			WidgetPropertiesExporter widgetPropertiesExporter = new WidgetPropertiesExporter(this, context);
			widgetPropertiesExporter.ExportWidgetProperties(annotation);
		}

		public override Annotation ToAnnotationOverride(PostScriptReader reader, IRadFixedDocumentImportContext context)
		{
			Guard.ThrowExceptionIfNull<PostScriptReader>(reader, "reader");
			Guard.ThrowExceptionIfNull<IRadFixedDocumentImportContext>(context, "context");
			return this.ToWidget(reader, context);
		}

		public Widget ToWidget(PostScriptReader reader, IRadFixedDocumentImportContext context)
		{
			Widget widget;
			if (!context.TryGetWidget(this, out widget) && this.FormField != null)
			{
				FormField formField = this.FormField.ToFormField(reader, context, true);
				widget = formField.AddWidget();
				context.MapWidgets(this, widget);
				WidgetPropertiesImporter widgetPropertiesImporter = new WidgetPropertiesImporter(this, reader, context);
				widgetPropertiesImporter.ImportWidgetProperties(widget);
			}
			return widget;
		}

		internal bool TryGetOnStateName(out string stateName)
		{
			if (base.Appearances != null)
			{
				Appearance normalAppearance = base.Appearances.NormalAppearance;
				foreach (KeyValuePair<string, FormXObject> keyValuePair in normalAppearance.StateAppearances)
				{
					if (keyValuePair.Key != "Off")
					{
						stateName = keyValuePair.Key;
						return true;
					}
				}
			}
			stateName = null;
			return false;
		}

		readonly DirectProperty<PdfName> highlightingMode;

		readonly ReferenceProperty<AppearanceCharacteristics> dynamicAppearanceCharacteristics;

		readonly ReferenceProperty<PdfString> defaultAppearance;

		readonly DirectProperty<PdfInt> quadding;

		readonly ReferenceProperty<PdfDictionary> actions;

		readonly ReferenceProperty<PdfDictionary> additionalActions;

		readonly ReferenceProperty<FormFieldNode> parentField;
	}
}
