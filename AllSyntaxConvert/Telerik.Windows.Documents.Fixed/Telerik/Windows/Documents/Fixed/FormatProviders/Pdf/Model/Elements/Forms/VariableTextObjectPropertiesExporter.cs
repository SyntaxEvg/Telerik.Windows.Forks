using System;
using System.IO;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Export;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Export.ContentElementWriters;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Types;
using Telerik.Windows.Documents.Fixed.Model.Editing.Flow;
using Telerik.Windows.Documents.Fixed.Model.InteractiveForms;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.Forms
{
	class VariableTextObjectPropertiesExporter<T> where T : IVariableTextPropertiesObject
	{
		protected VariableTextObjectPropertiesExporter(T node, IPdfExportContext exportContext)
		{
			this.node = node;
			this.context = exportContext;
		}

		protected T Node
		{
			get
			{
				return this.node;
			}
		}

		protected IPdfExportContext Context
		{
			get
			{
				return this.context;
			}
		}

		protected void ExportVariableProperties(VariableTextProperties textProperties)
		{
			using (MemoryStream memoryStream = new MemoryStream())
			{
				PdfSingleLineWriter writer = new PdfSingleLineWriter(memoryStream);
				IPdfContentExportContext acroFormContentExportContext = this.Context.AcroFormContentExportContext;
				ContentElementWriters.TextPropertiesWriter.Write(writer, acroFormContentExportContext, textProperties.PropertiesOwner);
				PdfLiteralString defaultAppearance = new PdfLiteralString(memoryStream.ToArray());
				T t = this.Node;
				t.SetDefaultAppearance(defaultAppearance);
			}
			this.ExportQuadding(textProperties.HorizontalAlignment);
		}

		void ExportQuadding(HorizontalAlignment alignment)
		{
			PdfInt quadding;
			switch (alignment)
			{
			case HorizontalAlignment.Right:
				quadding = new PdfInt(2);
				break;
			case HorizontalAlignment.Center:
				quadding = new PdfInt(1);
				break;
			default:
				quadding = new PdfInt(0);
				break;
			}
			T t = this.Node;
			t.SetQuadding(quadding);
		}

		readonly T node;

		readonly IPdfExportContext context;
	}
}
