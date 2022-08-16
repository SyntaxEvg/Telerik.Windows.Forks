using System;
using System.Collections.Generic;
using System.Windows;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Export;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Import;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Import.Parser;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Data;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.Common;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.DocumentStructure;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.Objects;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Types;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Utilities;
using Telerik.Windows.Documents.Fixed.Model;
using Telerik.Windows.Documents.Fixed.Model.Annotations;
using Telerik.Windows.Documents.Fixed.Model.Resources;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.Annotations
{
	abstract class AnnotationObject : PdfObject
	{
		public AnnotationObject()
		{
			this.rect = base.RegisterDirectProperty<PdfArray>(new PdfPropertyDescriptor("Rect", true));
			this.parentPage = base.RegisterReferenceProperty<Page>(new PdfPropertyDescriptor("P", false, PdfPropertyRestrictions.MustBeIndirectReference));
			this.flags = base.RegisterDirectProperty<PdfInt>(new PdfPropertyDescriptor("F"), new PdfInt(0));
			this.appearanceState = base.RegisterDirectProperty<PdfName>(new PdfPropertyDescriptor("AS"));
			this.borderStyle = base.RegisterReferenceProperty<BorderStyle>(new PdfPropertyDescriptor("BS"));
			this.appearances = base.RegisterReferenceProperty<Appearances>(new PdfPropertyDescriptor("AP"));
		}

		public virtual bool IsSupported
		{
			get
			{
				return true;
			}
		}

		public PdfArray Rect
		{
			get
			{
				return this.rect.GetValue();
			}
			set
			{
				this.rect.SetValue(value);
			}
		}

		public Page ParentPage
		{
			get
			{
				return this.parentPage.GetValue();
			}
			set
			{
				this.parentPage.SetValue(value);
			}
		}

		public PdfInt Flags
		{
			get
			{
				return this.flags.GetValue();
			}
			set
			{
				this.flags.SetValue(value);
			}
		}

		public BorderStyle BorderStyle
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

		public Appearances Appearances
		{
			get
			{
				return this.appearances.GetValue();
			}
			set
			{
				this.appearances.SetValue(value);
			}
		}

		public PdfName AppearanceState
		{
			get
			{
				return this.appearanceState.GetValue();
			}
			set
			{
				this.appearanceState.SetValue(value);
			}
		}

		public Annotation ToAnnotation(PostScriptReader reader, IRadFixedDocumentImportContext context, double pageHeightInDip)
		{
			Guard.ThrowExceptionIfNull<PostScriptReader>(reader, "reader");
			Guard.ThrowExceptionIfNull<IRadFixedDocumentImportContext>(context, "context");
			Annotation annotation = this.ToAnnotationOverride(reader, context);
			if (annotation == null)
			{
				return null;
			}
			annotation.Rect = this.ReadRect(reader, context, pageHeightInDip);
			this.ImportFlagsToAnnotation(annotation);
			if (this.BorderStyle != null)
			{
				annotation.Border = new AnnotationBorder();
				this.BorderStyle.CopyPropertiesTo(annotation.Border);
			}
			if (this.Appearances != null)
			{
				if (this.AppearanceState == null)
				{
					annotation.Appearances = this.CreateSingleStateAppearances(reader, context);
				}
				else
				{
					MultiStateAppearances multiStateAppearances = this.CreateMultiStateAppearances(reader, context);
					multiStateAppearances.CurrentState = this.AppearanceState.Value;
					annotation.Appearances = multiStateAppearances;
				}
				annotation.DoOnAppearancesImport();
			}
			return annotation;
		}

		public abstract Annotation ToAnnotationOverride(PostScriptReader reader, IRadFixedDocumentImportContext context);

		protected void CopyPropertiesFrom(IPdfExportContext context, Annotation annotation, RadFixedPage fixedPage)
		{
			Guard.ThrowExceptionIfNull<IPdfExportContext>(context, "context");
			Guard.ThrowExceptionIfNull<Annotation>(annotation, "annotation");
			Guard.ThrowExceptionIfNull<RadFixedPage>(fixedPage, "fixedPage");
			this.Rect = AnnotationObject.ConvertRect(annotation.Rect, fixedPage.Size.Height).ToPointPdfArray();
			this.BorderStyle = new BorderStyle();
			this.BorderStyle.CopyPropertiesFrom(context, annotation.Border);
			this.ExportAnnotationFlagsFromAnnotation(annotation);
			if (annotation.Appearances != null)
			{
				switch (annotation.Appearances.AppearancesType)
				{
				case AnnotationAppearancesType.SingleStateAppearances:
					this.Appearances = this.CreateSingleStateAppearances(context, (SingleStateAppearances)annotation.Appearances);
					break;
				case AnnotationAppearancesType.MultiStateAppearances:
					this.Appearances = this.CreateMultiStateAppearances(context, (MultiStateAppearances)annotation.Appearances);
					break;
				default:
					throw new NotSupportedException(string.Format("Not supported appearances type: {0}", annotation.Appearances.AppearancesType));
				}
			}
			Page page;
			if (context.TryGetPage(fixedPage, out page))
			{
				this.ParentPage = page;
			}
		}

		internal static Rect ConvertRect(Rect rect, double height)
		{
			return new Rect(rect.X, height - rect.Y - rect.Height, rect.Width, rect.Height);
		}

		internal static void ImportFlagsToAnnotation(Annotation annotation, int flags)
		{
			FlagReader<AnnotationFlag> flagReader = new FlagReader<AnnotationFlag>(flags);
			annotation.IsDisplayedWhenNotSupported = !flagReader.IsSet(AnnotationFlag.Invisble);
			annotation.IsHidden = flagReader.IsSet(AnnotationFlag.Hidden);
			annotation.IsPrintable = flagReader.IsSet(AnnotationFlag.Print);
			annotation.IsZoomingWithPage = !flagReader.IsSet(AnnotationFlag.NoZoom);
			annotation.IsRotatedWithPage = !flagReader.IsSet(AnnotationFlag.NoRotate);
			annotation.IsVisibleInViewerUI = !flagReader.IsSet(AnnotationFlag.NoView);
			annotation.IsReadOnly = flagReader.IsSet(AnnotationFlag.ReadOnly);
			annotation.IsLockedByPositionAndSize = flagReader.IsSet(AnnotationFlag.Locked);
			annotation.IsTogglingVisibilityInViewerUI = flagReader.IsSet(AnnotationFlag.ToggleNoView);
			annotation.IsContentLocked = flagReader.IsSet(AnnotationFlag.LockedContents);
		}

		Rect ReadRect(PostScriptReader reader, IRadFixedDocumentImportContext context, double pageHeightInDip)
		{
			Rect rect = this.Rect.ToDipRect(reader, context);
			return AnnotationObject.ConvertRect(rect, pageHeightInDip);
		}

		void ImportFlagsToAnnotation(Annotation annotation)
		{
			AnnotationObject.ImportFlagsToAnnotation(annotation, this.Flags.Value);
		}

		void ExportAnnotationFlagsFromAnnotation(Annotation annotation)
		{
			FlagWriter<AnnotationFlag> flagWriter = new FlagWriter<AnnotationFlag>();
			flagWriter.SetFlagOnCondition(AnnotationFlag.Invisble, !annotation.IsDisplayedWhenNotSupported);
			flagWriter.SetFlagOnCondition(AnnotationFlag.Hidden, annotation.IsHidden);
			flagWriter.SetFlagOnCondition(AnnotationFlag.Print, annotation.IsPrintable);
			flagWriter.SetFlagOnCondition(AnnotationFlag.NoZoom, !annotation.IsZoomingWithPage);
			flagWriter.SetFlagOnCondition(AnnotationFlag.NoRotate, !annotation.IsRotatedWithPage);
			flagWriter.SetFlagOnCondition(AnnotationFlag.NoView, !annotation.IsVisibleInViewerUI);
			flagWriter.SetFlagOnCondition(AnnotationFlag.ReadOnly, annotation.IsReadOnly);
			flagWriter.SetFlagOnCondition(AnnotationFlag.Locked, annotation.IsLockedByPositionAndSize);
			flagWriter.SetFlagOnCondition(AnnotationFlag.ToggleNoView, annotation.IsTogglingVisibilityInViewerUI);
			flagWriter.SetFlagOnCondition(AnnotationFlag.LockedContents, annotation.IsContentLocked);
			this.Flags = new PdfInt(flagWriter.ResultFlags);
		}

		SingleStateAppearances CreateSingleStateAppearances(PostScriptReader reader, IRadFixedDocumentImportContext context)
		{
			SingleStateAppearances singleStateAppearances = null;
			if (this.Appearances.NormalAppearance != null && this.Appearances.NormalAppearance.SingleStateAppearance != null)
			{
				singleStateAppearances = new SingleStateAppearances();
				singleStateAppearances.NormalAppearance = context.GetFormSource(reader, this.Appearances.NormalAppearance.SingleStateAppearance);
				if (this.Appearances.DownAppearance != null && this.Appearances.DownAppearance.SingleStateAppearance != null)
				{
					singleStateAppearances.MouseDownAppearance = context.GetFormSource(reader, this.Appearances.DownAppearance.SingleStateAppearance);
				}
				if (this.Appearances.RolloverAppearance != null && this.Appearances.RolloverAppearance.SingleStateAppearance != null)
				{
					singleStateAppearances.MouseOverAppearance = context.GetFormSource(reader, this.Appearances.RolloverAppearance.SingleStateAppearance);
				}
			}
			return singleStateAppearances;
		}

		MultiStateAppearances CreateMultiStateAppearances(PostScriptReader reader, IRadFixedDocumentImportContext context)
		{
			MultiStateAppearances multiStateAppearances = new MultiStateAppearances();
			if (this.Appearances.NormalAppearance != null)
			{
				foreach (KeyValuePair<string, FormXObject> keyValuePair in this.Appearances.NormalAppearance.StateAppearances)
				{
					string key = keyValuePair.Key;
					FormXObject value = keyValuePair.Value;
					SingleStateAppearances singleStateAppearances = new SingleStateAppearances();
					multiStateAppearances.AddAppearance(key, singleStateAppearances);
					singleStateAppearances.NormalAppearance = context.GetFormSource(reader, value);
					FormXObject form;
					if (this.Appearances.DownAppearance != null && this.Appearances.DownAppearance.TryGetStateAppearance(key, out form))
					{
						singleStateAppearances.MouseDownAppearance = context.GetFormSource(reader, form);
					}
					FormXObject form2;
					if (this.Appearances.RolloverAppearance != null && this.Appearances.RolloverAppearance.TryGetStateAppearance(key, out form2))
					{
						singleStateAppearances.MouseOverAppearance = context.GetFormSource(reader, form2);
					}
				}
			}
			return multiStateAppearances;
		}

		Appearances CreateMultiStateAppearances(IPdfExportContext context, MultiStateAppearances multiStateAppearances)
		{
			Dictionary<string, FormXObject> dictionary = null;
			Dictionary<string, FormXObject> dictionary2 = null;
			Dictionary<string, FormXObject> dictionary3 = null;
			this.AppearanceState = new PdfName(multiStateAppearances.CurrentState);
			foreach (KeyValuePair<string, SingleStateAppearances> keyValuePair in multiStateAppearances)
			{
				string key = keyValuePair.Key;
				SingleStateAppearances value = keyValuePair.Value;
				if (value.NormalAppearance != null)
				{
					if (dictionary == null)
					{
						dictionary = new Dictionary<string, FormXObject>();
					}
					FormXObject appearanceXForm = AnnotationObject.GetAppearanceXForm(context, value.NormalAppearance);
					dictionary.Add(key, appearanceXForm);
					if (value.MouseDownAppearance != null)
					{
						if (dictionary2 == null)
						{
							dictionary2 = new Dictionary<string, FormXObject>();
						}
						FormXObject appearanceXForm2 = AnnotationObject.GetAppearanceXForm(context, value.MouseDownAppearance);
						dictionary2.Add(key, appearanceXForm2);
					}
					if (value.MouseOverAppearance != null)
					{
						if (dictionary3 == null)
						{
							dictionary3 = new Dictionary<string, FormXObject>();
						}
						FormXObject appearanceXForm3 = AnnotationObject.GetAppearanceXForm(context, value.MouseOverAppearance);
						dictionary3.Add(key, appearanceXForm3);
					}
				}
			}
			return this.CreateMultiStateAppearances(dictionary, dictionary2, dictionary3);
		}

		Appearances CreateMultiStateAppearances(Dictionary<string, FormXObject> normalMapping, Dictionary<string, FormXObject> downMapping, Dictionary<string, FormXObject> rolloverMapping)
		{
			if (normalMapping == null)
			{
				return null;
			}
			return new Appearances
			{
				NormalAppearance = new Appearance(normalMapping),
				DownAppearance = ((downMapping == null) ? null : new Appearance(downMapping)),
				RolloverAppearance = ((rolloverMapping == null) ? null : new Appearance(rolloverMapping))
			};
		}

		Appearances CreateSingleStateAppearances(IPdfExportContext context, SingleStateAppearances singleStateAppearances)
		{
			Appearances appearances = null;
			if (singleStateAppearances.NormalAppearance != null)
			{
				appearances = new Appearances();
				FormXObject appearanceXForm = AnnotationObject.GetAppearanceXForm(context, singleStateAppearances.NormalAppearance);
				appearances.NormalAppearance = new Appearance(appearanceXForm);
				if (singleStateAppearances.MouseDownAppearance != null)
				{
					FormXObject appearanceXForm2 = AnnotationObject.GetAppearanceXForm(context, singleStateAppearances.MouseDownAppearance);
					appearances.DownAppearance = new Appearance(appearanceXForm2);
				}
				if (singleStateAppearances.MouseOverAppearance != null)
				{
					FormXObject appearanceXForm3 = AnnotationObject.GetAppearanceXForm(context, singleStateAppearances.MouseOverAppearance);
					appearances.RolloverAppearance = new Appearance(appearanceXForm3);
				}
			}
			return appearances;
		}

		static FormXObject GetAppearanceXForm(IPdfExportContext context, FormSource appearanceSource)
		{
			ResourceEntry resource = context.GetResource(appearanceSource);
			return (FormXObject)resource.Resource.Content;
		}

		public const string RectName = "Rect";

		public const string AppearanceName = "AP";

		public const string AppearanceStateName = "AS";

		readonly DirectProperty<PdfArray> rect;

		readonly DirectProperty<PdfInt> flags;

		readonly DirectProperty<PdfName> appearanceState;

		readonly ReferenceProperty<BorderStyle> borderStyle;

		readonly ReferenceProperty<Appearances> appearances;

		readonly ReferenceProperty<Page> parentPage;
	}
}
