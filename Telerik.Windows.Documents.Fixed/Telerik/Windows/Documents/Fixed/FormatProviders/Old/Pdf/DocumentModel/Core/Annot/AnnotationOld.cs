using System;
using System.Collections.Generic;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.DocumentModel.Core.XObjects;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.DocumentModel.Data;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.DocumentModel.PdfConverters;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.PdfReader;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.Utils;
using Telerik.Windows.Documents.Fixed.Model.Annotations;
using Telerik.Windows.Documents.Fixed.Model.Internal;
using Telerik.Windows.Documents.Fixed.Model.Internal.Annotations;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.DocumentModel.Core.Annot
{
	[PdfClass]
	abstract class AnnotationOld : PdfObjectOld
	{
		public AnnotationOld(PdfContentManager contentManager)
			: base(contentManager)
		{
			this.rect = base.CreateLoadOnDemandProperty<PdfArrayOld>(new PdfPropertyDescriptor
			{
				Name = "Rect",
				IsRequired = true
			});
			this.flags = base.CreateInstantLoadProperty<PdfIntOld>(new PdfPropertyDescriptor
			{
				Name = "F"
			}, new PdfIntOld(contentManager, 0));
			this.appearances = base.CreateLoadOnDemandProperty<AppearancesOld>(new PdfPropertyDescriptor
			{
				Name = "AP"
			}, Converters.PdfDictionaryToPdfObjectConverter);
			this.appearanceState = base.CreateInstantLoadProperty<PdfNameOld>(new PdfPropertyDescriptor
			{
				Name = "AS"
			});
		}

		public PdfArrayOld Rect
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

		public PdfIntOld Flags
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

		public AppearancesOld Appearances
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

		public PdfNameOld AppearanceState
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

		public abstract AnnotationType Type { get; }

		public bool TryGetAppearanceContent(out AnnotationAppearancesOld appearances)
		{
			if (this.Appearances != null && this.Appearances.NormalAppearance != null)
			{
				if (this.AppearanceState == null)
				{
					Container normalAppearance = this.ImportSingleStateAppearanceContent(this.Appearances.NormalAppearance);
					Container mouseDownAppearance = this.ImportSingleStateAppearanceContent(this.Appearances.DownAppearance);
					Container mouseOverAppearance = this.ImportSingleStateAppearanceContent(this.Appearances.RolloverAppearance);
					appearances = new SingleStateAppearancesOld(normalAppearance, mouseDownAppearance, mouseOverAppearance);
				}
				else
				{
					IEnumerable<KeyValuePair<string, SingleStateAppearancesOld>> statesToAppearances = this.ImportMultiStateAppearances();
					appearances = new MultiStateAppearancesOld(this.AppearanceState.Value, statesToAppearances);
				}
				return true;
			}
			appearances = null;
			return false;
		}

		protected virtual PdfResourceOld GetDefaultAppearanceResource()
		{
			return new PdfResourceOld(base.ContentManager);
		}

		IEnumerable<KeyValuePair<string, SingleStateAppearancesOld>> ImportMultiStateAppearances()
		{
			foreach (string state in this.Appearances.NormalAppearance.States)
			{
				Container normalContent = this.ImportSingleStateAppearanceContent(this.Appearances.NormalAppearance, state);
				Container mouseDownContent = this.ImportSingleStateAppearanceContent(this.Appearances.DownAppearance, state);
				Container mouseOverContent = this.ImportSingleStateAppearanceContent(this.Appearances.RolloverAppearance, state);
				yield return new KeyValuePair<string, SingleStateAppearancesOld>(state, new SingleStateAppearancesOld(normalContent, mouseDownContent, mouseOverContent));
			}
			yield break;
		}

		Container ImportAppearanceContent(XForm appearanceObject)
		{
			if (appearanceObject == null)
			{
				return null;
			}
			PdfResourceOld defaultAppearanceResource = this.GetDefaultAppearanceResource();
			PdfContext pdfContext = new PdfContext(base.ContentManager, appearanceObject.BBox.ToRect());
			Container container = pdfContext.CreateXForm(defaultAppearanceResource, appearanceObject, false);
			container.BoundingRect = appearanceObject.BBox.ToRect();
			PdfElementToFixedElementTranslator.ToSLCoordinates(container);
			return container;
		}

		Container ImportSingleStateAppearanceContent(AppearanceOld appearance)
		{
			if (appearance == null)
			{
				return null;
			}
			return this.ImportAppearanceContent(appearance.SingleStateAppearance);
		}

		Container ImportSingleStateAppearanceContent(AppearanceOld appearance, string state)
		{
			Container result = null;
			XForm appearanceObject;
			if (appearance != null && appearance.TryGetStateAppearance(state, out appearanceObject))
			{
				result = this.ImportAppearanceContent(appearanceObject);
			}
			return result;
		}

		readonly LoadOnDemandProperty<PdfArrayOld> rect;

		readonly InstantLoadProperty<PdfIntOld> flags;

		readonly LoadOnDemandProperty<AppearancesOld> appearances;

		readonly InstantLoadProperty<PdfNameOld> appearanceState;
	}
}
