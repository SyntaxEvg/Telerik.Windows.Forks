using System;
using System.Collections.Generic;
using System.Linq;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Export;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Data;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.Common
{
	abstract class PdfArrayObject : PdfObject
	{
		public PdfArrayObject()
		{
			this.propertiesToIgnore = new HashSet<IPdfProperty>();
			this.propertyIndex = new Dictionary<IPdfProperty, int>();
		}

		public override void Write(PdfWriter writer, IPdfExportContext context)
		{
			Guard.ThrowExceptionIfNull<PdfWriter>(writer, "writer");
			Guard.ThrowExceptionIfNull<IPdfExportContext>(context, "context");
			writer.Write(PdfNames.PdfArrayStart);
			IOrderedEnumerable<IPdfProperty> orderedEnumerable = from prop in this.propertyIndex.Keys
				orderby this.propertyIndex[prop]
				select prop;
			bool flag = true;
			foreach (IPdfProperty pdfProperty in orderedEnumerable)
			{
				if (!this.ShouldIgnoreProperty(pdfProperty))
				{
					if (!flag)
					{
						writer.WriteSeparator();
					}
					flag = false;
					if (pdfProperty.HasValue)
					{
						PdfObject.WritePropertyValue(writer, context, pdfProperty);
					}
					else
					{
						writer.Write("null");
					}
				}
			}
			writer.Write(PdfNames.PdfArrayEnd);
		}

		protected override void RegisterPdfProperty(IPdfProperty property)
		{
			base.RegisterPdfProperty(property);
			this.propertyIndex[property] = base.Properties.Count - 1;
		}

		protected void IgnoreProperties(params IPdfProperty[] properties)
		{
			foreach (IPdfProperty property in properties)
			{
				this.IgnoreProperty(property);
			}
		}

		protected void IgnoreProperty(IPdfProperty property)
		{
			this.propertiesToIgnore.Add(property);
		}

		bool ShouldIgnoreProperty(IPdfProperty property)
		{
			return this.propertiesToIgnore.Contains(property);
		}

		readonly HashSet<IPdfProperty> propertiesToIgnore;

		readonly Dictionary<IPdfProperty, int> propertyIndex;
	}
}
