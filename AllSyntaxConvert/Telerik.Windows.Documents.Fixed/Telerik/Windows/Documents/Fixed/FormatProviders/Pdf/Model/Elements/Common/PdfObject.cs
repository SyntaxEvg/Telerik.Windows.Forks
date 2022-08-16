using System;
using System.Collections.Generic;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Export;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Import;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Import.Parser;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Data;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Types;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.Common
{
	abstract class PdfObject : PdfPrimitive
	{
		public PdfObject()
		{
			this.properties = new Dictionary<string, IPdfProperty>();
		}

		public override PdfElementType Type
		{
			get
			{
				return PdfElementType.PdfObject;
			}
		}

		internal Dictionary<string, IPdfProperty> Properties
		{
			get
			{
				return this.properties;
			}
		}

		public override void Write(PdfWriter writer, IPdfExportContext context)
		{
			PdfObject.WritePdfPropertiesDictionary(this, writer, context, true);
		}

		internal static void WritePdfPropertiesDictionary(PdfObject instance, PdfWriter writer, IPdfExportContext context, bool shouldWriteObjectDescriptor = true)
		{
			Guard.ThrowExceptionIfNull<PdfWriter>(writer, "writer");
			Guard.ThrowExceptionIfNull<IPdfExportContext>(context, "context");
			if (instance.Properties.Count == 0)
			{
				return;
			}
			writer.Write(PdfNames.PdfDictionaryStart);
			if (shouldWriteObjectDescriptor)
			{
				PdfObjectDescriptor pdfObjectDescriptor = PdfObjectDescriptors.GetPdfObjectDescriptor(instance);
				writer.WritePdfObjectDescriptor(context, pdfObjectDescriptor);
			}
			foreach (IPdfProperty pdfProperty in instance.Properties.Values)
			{
				if ((pdfProperty.Descriptor.IsRequired && pdfProperty.HasValue) || pdfProperty.HasNonDefaultValue || pdfProperty.Descriptor.AlwaysExport)
				{
					writer.WritePdfName(pdfProperty.Descriptor.Name);
					writer.WriteSeparator();
					PdfObject.WritePropertyValue(writer, context, pdfProperty);
					writer.WriteSeparator();
				}
			}
			writer.Write(PdfNames.PdfDictionaryEnd);
		}

		public virtual void Read(PostScriptReader reader, IPdfImportContext context)
		{
			Guard.ThrowExceptionIfNull<PostScriptReader>(reader, "reader");
			Guard.ThrowExceptionIfNull<IPdfImportContext>(context, "context");
			PdfDictionary pdfDictionary = reader.Read<PdfDictionary>(context, PdfElementType.Dictionary);
			if (pdfDictionary != null)
			{
				this.LoadFromDictionary(reader, context, pdfDictionary);
			}
		}

		public void Load(PostScriptReader reader, IPdfImportContext context, PdfPrimitive primitive)
		{
			Guard.ThrowExceptionIfNull<PostScriptReader>(reader, "reader");
			Guard.ThrowExceptionIfNull<IPdfImportContext>(context, "context");
			Guard.ThrowExceptionIfNull<PdfPrimitive>(primitive, "primitive");
			if (primitive.Type == PdfElementType.Dictionary)
			{
				this.LoadFromDictionary(reader, context, (PdfDictionary)primitive);
				return;
			}
			this.LoadOverride(reader, context, primitive);
		}

		protected virtual void LoadOverride(PostScriptReader reader, IPdfImportContext context, PdfPrimitive primitive)
		{
		}

		protected static void WritePropertyValue(PdfWriter writer, IPdfExportContext context, IPdfProperty property)
		{
			PdfPrimitive value = property.GetValue();
			switch (property.Descriptor.Restrictions)
			{
			case PdfPropertyRestrictions.MustBeIndirectReference:
				PdfPrimitive.WriteIndirectReference(writer, context, value);
				return;
			case PdfPropertyRestrictions.ContainsOnlyIndirectReferences:
				PdfObject.WriteIndirectReferencesCollection(writer, context, value);
				return;
			}
			if (value.ExportAs == PdfElementType.PdfStreamObject)
			{
				PdfPrimitive.WriteIndirectReference(writer, context, value);
				return;
			}
			value.Write(writer, context);
		}

		protected DirectProperty<T> RegisterDirectProperty<T>(PdfPropertyDescriptor descriptor) where T : PdfPrimitive
		{
			DirectProperty<T> directProperty = new DirectProperty<T>(descriptor);
			this.RegisterPdfProperty(directProperty);
			return directProperty;
		}

		protected DirectProperty<T> RegisterDirectProperty<T>(PdfPropertyDescriptor descriptor, T initialValue) where T : PdfPrimitive
		{
			DirectProperty<T> directProperty = new DirectProperty<T>(descriptor, initialValue);
			this.RegisterPdfProperty(directProperty);
			return directProperty;
		}

		protected ReferenceProperty<T> RegisterReferenceProperty<T>(PdfPropertyDescriptor descriptor) where T : PdfPrimitive
		{
			ReferenceProperty<T> referenceProperty = new ReferenceProperty<T>(descriptor);
			this.RegisterPdfProperty(referenceProperty);
			return referenceProperty;
		}

		protected ReferenceProperty<T> RegisterReferenceProperty<T>(PdfPropertyDescriptor descriptor, T initialValue) where T : PdfPrimitive
		{
			ReferenceProperty<T> referenceProperty = new ReferenceProperty<T>(descriptor, initialValue);
			this.RegisterPdfProperty(referenceProperty);
			return referenceProperty;
		}

		protected virtual void RegisterPdfProperty(IPdfProperty property)
		{
			this.RegisterPdfProperty(property.Descriptor.Name, property);
		}

		protected static void WriteIndirectReferencesCollection(PdfWriter writer, IPdfExportContext context, PdfPrimitive primitive)
		{
			Guard.ThrowExceptionIfNull<PdfWriter>(writer, "writer");
			Guard.ThrowExceptionIfNull<IPdfExportContext>(context, "context");
			PdfElementType type = primitive.Type;
			if (type != PdfElementType.Array)
			{
				return;
			}
			((PdfArray)primitive).Write(writer, context, true);
		}

		void RegisterPdfProperty(string name, IPdfProperty property)
		{
			this.properties[name] = property;
		}

		void LoadFromDictionary(PostScriptReader reader, IPdfImportContext context, PdfDictionary dictionary)
		{
			Guard.ThrowExceptionIfNull<PostScriptReader>(reader, "reader");
			Guard.ThrowExceptionIfNull<IPdfImportContext>(context, "context");
			Guard.ThrowExceptionIfNull<PdfDictionary>(dictionary, "dictionary");
			foreach (KeyValuePair<string, PdfPrimitive> keyValuePair in dictionary)
			{
				IPdfProperty pdfProperty;
				if (this.properties.TryGetValue(keyValuePair.Key, out pdfProperty))
				{
					pdfProperty.SetValue(reader, context, keyValuePair.Value);
				}
			}
		}

		readonly Dictionary<string, IPdfProperty> properties;
	}
}
