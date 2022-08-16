using System;
using System.IO;
using System.Text;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Data;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Types;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Utilities;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Export
{
	class PdfWriter
	{
		public PdfWriter(Stream output)
		{
			Guard.ThrowExceptionIfNull<Stream>(output, "output");
			this.output = output;
			this.encoding = PdfEncoding.Encoding;
		}

		public Stream OutputStream
		{
			get
			{
				return this.output;
			}
		}

		public long Position
		{
			get
			{
				return this.output.Position;
			}
		}

		public long Seek(int offset, SeekOrigin seekOrigin)
		{
			return this.output.Seek((long)offset, seekOrigin);
		}

		public void WriteSeparator()
		{
			this.WriteRaw(32);
		}

		public void Write(string value)
		{
			byte[] bytes = this.encoding.GetBytes(value);
			this.WriteRaw(bytes);
		}

		public void Write(string value, params object[] args)
		{
			this.Write(string.Format(value, args));
		}

		public virtual void WriteLine()
		{
			this.WriteRaw(PdfWriter.newLineToken);
		}

		public void WriteLine(string line)
		{
			this.Write(line);
			this.WriteLine();
		}

		public void WriteLine(string line, params object[] arg)
		{
			this.WriteLine(string.Format(line, arg));
		}

		public void Write(byte[] data)
		{
			this.WriteRaw(data);
		}

		public void WriteDocumentStart()
		{
			this.WriteLine("%PDF-1.7");
			this.WriteLine("%úûüý");
		}

		public void WriteDocumentEnd()
		{
			this.WriteLine("%%EOF");
		}

		public void WritePdfName(string name)
		{
			Guard.ThrowExceptionIfNull<string>(name, "name");
			this.Write("{0}{1}", new object[] { "/", name });
		}

		public void WritePdfObjectDescriptor(IPdfExportContext context, PdfObjectDescriptor descriptor)
		{
			Guard.ThrowExceptionIfNull<IPdfExportContext>(context, "context");
			Guard.ThrowExceptionIfNull<PdfObjectDescriptor>(descriptor, "descriptor");
			if (!string.IsNullOrEmpty(descriptor.Type))
			{
				this.WritePdfName("Type");
				this.WriteSeparator();
				this.WritePdfName(descriptor.Type);
				this.WriteSeparator();
			}
			if (descriptor.SubType != null)
			{
				this.WritePdfName(descriptor.SubTypeProperty);
				this.WriteSeparator();
				descriptor.SubType.Write(this, context);
				this.WriteSeparator();
			}
		}

		public void WriteIndirectReference(IndirectReference reference)
		{
			this.Write("{0} {1} {2}", new object[] { reference.ObjectNumber, reference.GenerationNumber, "R" });
		}

		void WriteRaw(byte b)
		{
			this.output.WriteByte(b);
		}

		void WriteRaw(byte[] data)
		{
			this.output.Write(data, 0, data.Length);
		}

		const byte SpaceToken = 32;

		static readonly byte[] newLineToken = new byte[] { 13, 10 };

		readonly Stream output;

		readonly Encoding encoding;
	}
}
