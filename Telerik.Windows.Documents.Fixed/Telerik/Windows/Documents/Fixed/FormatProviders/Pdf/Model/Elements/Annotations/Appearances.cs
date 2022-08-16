using System;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Export;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.Common;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Types;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.Annotations
{
	class Appearances : PdfObject
	{
		public Appearance NormalAppearance { get; set; }

		public Appearance RolloverAppearance { get; set; }

		public Appearance DownAppearance { get; set; }

		public override void Write(PdfWriter writer, IPdfExportContext context)
		{
			PdfDictionary pdfDictionary = new PdfDictionary();
			if (this.NormalAppearance != null)
			{
				pdfDictionary["N"] = this.GetAppearanceReference(this.NormalAppearance, context);
			}
			if (this.RolloverAppearance != null)
			{
				pdfDictionary["R"] = this.GetAppearanceReference(this.RolloverAppearance, context);
			}
			if (this.DownAppearance != null)
			{
				pdfDictionary["D"] = this.GetAppearanceReference(this.DownAppearance, context);
			}
			pdfDictionary.Write(writer, context);
		}

		IndirectReference GetAppearanceReference(Appearance appearance, IPdfExportContext context)
		{
			if (appearance.SingleStateAppearance == null)
			{
				return context.CreateIndirectObject(appearance).Reference;
			}
			return context.CreateIndirectObject(appearance.SingleStateAppearance).Reference;
		}

		public const string NormalAppearanceKey = "N";

		public const string RolloverAppearanceKey = "R";

		public const string DownAppearanceKey = "D";
	}
}
