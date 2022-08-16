using System;
using System.IO;
using Telerik.Windows.Documents.Common.Model.Data;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Media
{
	public class ImageSource : IResource
	{
		public ImageSource(Stream stream, string extension)
			: this(extension)
		{
			Guard.ThrowExceptionIfNull<Stream>(stream, "stream");
			this.data = new byte[stream.Length];
			if (stream.CanSeek)
			{
				stream.Seek(0L, SeekOrigin.Begin);
			}
			stream.Read(this.data, 0, this.data.Length);
		}

		public ImageSource(byte[] data, string extension)
			: this(extension)
		{
			Guard.ThrowExceptionIfNull<byte[]>(data, "data");
			this.data = data;
		}

		internal ImageSource()
		{
		}

		ImageSource(string extension)
		{
			Guard.ThrowExceptionIfNullOrEmpty(extension, "extension");
			this.extension = PathExtension.StripExtension(extension).ToLowerInvariant();
		}

		string IResource.Extension
		{
			get
			{
				return this.Extension;
			}
		}

		public virtual string Extension
		{
			get
			{
				return this.extension;
			}
		}

		byte[] IResource.Data
		{
			get
			{
				return this.Data;
			}
		}

		public virtual byte[] Data
		{
			get
			{
				return this.data;
			}
		}

		internal int Id
		{
			get
			{
				return ((IResource)this).Id;
			}
		}

		string IResource.Name
		{
			get
			{
				return string.Format("image{0}.{1}", ((IResource)this).Id, this.Extension);
			}
		}

		int IResource.Id { get; set; }

		ResourceManager IResource.Owner { get; set; }

		internal virtual ImageSource Clone()
		{
			byte[] array = (byte[])this.Data.Clone();
			return new ImageSource(array, this.Extension);
		}

		const string NameFormat = "image{0}.{1}";

		readonly byte[] data;

		readonly string extension;
	}
}
