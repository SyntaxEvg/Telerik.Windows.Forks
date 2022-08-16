using System;
using System.Collections.Generic;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Export;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.Common;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.Objects;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Types;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.Annotations
{
	class Appearance : PdfObject
	{
		public override PdfElementType Type
		{
			get
			{
				return PdfElementType.IndirectObject;
			}
		}

		public Appearance(FormXObject singleStateAppearance)
		{
			Guard.ThrowExceptionIfNull<FormXObject>(singleStateAppearance, "singleStateAppearance");
			this.singleStateAppearance = singleStateAppearance;
		}

		public Appearance(Dictionary<string, FormXObject> stateToAppearanceMapping)
		{
			Guard.ThrowExceptionIfNull<Dictionary<string, FormXObject>>(stateToAppearanceMapping, "stateToAppearanceMapping");
			this.stateToAppearanceMapping = stateToAppearanceMapping;
		}

		public FormXObject SingleStateAppearance
		{
			get
			{
				return this.singleStateAppearance;
			}
		}

		public IEnumerable<KeyValuePair<string, FormXObject>> StateAppearances
		{
			get
			{
				if (this.stateToAppearanceMapping != null)
				{
					foreach (KeyValuePair<string, FormXObject> stateAppearance in this.stateToAppearanceMapping)
					{
						yield return stateAppearance;
					}
				}
				yield break;
			}
		}

		public bool TryGetStateAppearance(string state, out FormXObject appearance)
		{
			appearance = null;
			return this.stateToAppearanceMapping != null && this.stateToAppearanceMapping.TryGetValue(state, out appearance);
		}

		public override void Write(PdfWriter writer, IPdfExportContext context)
		{
			if (this.SingleStateAppearance == null)
			{
				PdfDictionary pdfDictionary = new PdfDictionary();
				foreach (KeyValuePair<string, FormXObject> keyValuePair in this.StateAppearances)
				{
					IndirectObject indirectObject = context.CreateIndirectObject(keyValuePair.Value);
					pdfDictionary[keyValuePair.Key] = indirectObject.Reference;
				}
				pdfDictionary.Write(writer, context);
				return;
			}
			throw new InvalidOperationException("FormXObject should be reused instead of writing the single-state appearance as separate indirect object!");
		}

		readonly FormXObject singleStateAppearance;

		readonly Dictionary<string, FormXObject> stateToAppearanceMapping;
	}
}
