using System;
using System.IO;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.DocumentModel.Data;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.PdfReader;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.PdfReader.Parsers;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.Utils;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.DocumentModel.Core
{
	[PdfClass(TypeName = "ObjStm")]
	class ObjectStreamOld : PdfStreamOld
	{
		public ObjectStreamOld(PdfContentManager contentManager)
			: base(contentManager)
		{
			this.n = base.CreateInstantLoadProperty<PdfIntOld>(new PdfPropertyDescriptor
			{
				Name = "N",
				IsRequired = true
			});
			this.first = base.CreateInstantLoadProperty<PdfIntOld>(new PdfPropertyDescriptor
			{
				Name = "First",
				IsRequired = true
			});
		}

		public PdfIntOld N
		{
			get
			{
				return this.n.GetValue();
			}
			set
			{
				this.n.SetValue(value);
			}
		}

		public PdfIntOld First
		{
			get
			{
				return this.first.GetValue();
			}
			set
			{
				this.first.SetValue(value);
			}
		}

		public override void Load(PdfDataStream stream)
		{
			Guard.ThrowExceptionIfNull<PdfDataStream>(stream, "stream");
			base.Load(stream);
			byte[] buffer = stream.ReadData(base.ContentManager);
			this.parser = new ObjectStreamParser(base.ContentManager, new MemoryStream(buffer));
			this.offsets = this.parser.ReadOffsets(this.N.Value, this.First.Value);
		}

		public IndirectObjectOld ReadIndirectObject(int fromIndex)
		{
			return new IndirectObjectOld
			{
				Value = this.parser.ReadPdfObject((long)this.offsets[fromIndex])
			};
		}

		readonly InstantLoadProperty<PdfIntOld> n;

		readonly InstantLoadProperty<PdfIntOld> first;

		int[] offsets;

		ObjectStreamParser parser;
	}
}
