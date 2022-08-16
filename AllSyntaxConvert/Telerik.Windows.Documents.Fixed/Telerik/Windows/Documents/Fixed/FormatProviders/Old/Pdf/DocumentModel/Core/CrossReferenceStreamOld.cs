using System;
using System.IO;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.DocumentModel.Core.Encryption;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.DocumentModel.Data;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.DocumentModel.PdfConverters;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.PdfReader;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.Utils;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.DocumentModel.Core
{
	class CrossReferenceStreamOld : PdfStreamOld
	{
		public CrossReferenceStreamOld(PdfContentManager contentManager)
			: base(contentManager)
		{
			this.index = base.CreateLoadOnDemandProperty<PdfArrayOld>(new PdfPropertyDescriptor
			{
				Name = "Index"
			});
			this.w = base.CreateLoadOnDemandProperty<PdfArrayOld>(new PdfPropertyDescriptor
			{
				Name = "W"
			});
			this.size = base.CreateInstantLoadProperty<PdfIntOld>(new PdfPropertyDescriptor
			{
				Name = "Size"
			});
			this.prev = base.CreateInstantLoadProperty<PdfIntOld>(new PdfPropertyDescriptor
			{
				Name = "Prev"
			});
			this.root = base.CreateLoadOnDemandProperty<DocumentCatalogOld>(new PdfPropertyDescriptor
			{
				Name = "Root"
			});
			this.id = base.CreateLoadOnDemandProperty<PdfArrayOld>(new PdfPropertyDescriptor
			{
				Name = "ID"
			});
			this.encrypt = base.CreateLoadOnDemandProperty<EncryptOld>(new PdfPropertyDescriptor
			{
				Name = "Encrypt"
			}, Converters.EncryptConverter);
		}

		public PdfArrayOld Index
		{
			get
			{
				return this.index.GetValue();
			}
			set
			{
				this.index.SetValue(value);
			}
		}

		public PdfArrayOld W
		{
			get
			{
				return this.w.GetValue();
			}
			set
			{
				this.w.SetValue(value);
			}
		}

		public PdfIntOld Size
		{
			get
			{
				return this.size.GetValue();
			}
			set
			{
				this.size.SetValue(value);
			}
		}

		public PdfIntOld Prev
		{
			get
			{
				return this.prev.GetValue();
			}
			set
			{
				this.prev.SetValue(value);
			}
		}

		public LoadOnDemandProperty<DocumentCatalogOld> Root
		{
			get
			{
				return this.root;
			}
		}

		public PdfArrayOld ID
		{
			get
			{
				return this.id.GetValue();
			}
			set
			{
				this.id.SetValue(value);
			}
		}

		public EncryptOld Encrypt
		{
			get
			{
				return this.encrypt.GetValue();
			}
			set
			{
				this.encrypt.SetValue(value);
			}
		}

		public void Append(CrossReferenceStreamOld stream, byte[] data)
		{
			Guard.ThrowExceptionIfNull<CrossReferenceStreamOld>(stream, "stream");
			int num;
			stream.W.TryGetInt(0, out num);
			int num2;
			stream.W.TryGetInt(1, out num2);
			int num3;
			stream.W.TryGetInt(2, out num3);
			int num4 = num + num2 + num3;
			byte[] array = new byte[num4];
			if (stream.Index == null)
			{
				int num5 = 0;
				int value = stream.Size.Value;
				using (MemoryStream memoryStream = new MemoryStream(data))
				{
					for (int i = 0; i < value; i++)
					{
						if (memoryStream.Read(array, 0, num4) == num4)
						{
							base.ContentManager.CrossReferences.AddCrossReferenceEntry(num5 + i, stream.GetCrossReferenceEntry(array));
						}
					}
					return;
				}
			}
			using (MemoryStream memoryStream2 = new MemoryStream(data))
			{
				for (int j = 0; j < stream.Index.Count; j += 2)
				{
					int num5;
					stream.Index.TryGetInt(j, out num5);
					int value;
					stream.Index.TryGetInt(j + 1, out value);
					for (int k = 0; k < value; k++)
					{
						if (memoryStream2.Read(array, 0, num4) == num4)
						{
							base.ContentManager.CrossReferences.AddCrossReferenceEntry(num5 + k, stream.GetCrossReferenceEntry(array));
						}
					}
				}
			}
		}

		CrossReferenceEntryOld GetCrossReferenceEntry(byte[] bytes)
		{
			int num = 0;
			int num2;
			this.W.TryGetInt(0, out num2);
			int num3;
			this.W.TryGetInt(1, out num3);
			int count;
			this.W.TryGetInt(2, out count);
			CrossReferenceEntryOld crossReferenceEntryOld = new CrossReferenceEntryOld();
			crossReferenceEntryOld.Type = (CrossReferenceEntryTypeOld)BytesHelperOld.GetInt(bytes, num, num2);
			num += num2;
			crossReferenceEntryOld.Field2 = (long)BytesHelperOld.GetInt(bytes, num, num3);
			num += num3;
			crossReferenceEntryOld.Field3 = BytesHelperOld.GetInt(bytes, num, count);
			return crossReferenceEntryOld;
		}

		readonly LoadOnDemandProperty<PdfArrayOld> index;

		readonly LoadOnDemandProperty<PdfArrayOld> w;

		readonly InstantLoadProperty<PdfIntOld> size;

		readonly InstantLoadProperty<PdfIntOld> prev;

		readonly LoadOnDemandProperty<DocumentCatalogOld> root;

		readonly LoadOnDemandProperty<PdfArrayOld> id;

		readonly LoadOnDemandProperty<EncryptOld> encrypt;
	}
}
