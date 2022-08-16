using System;
using System.IO;
using System.Text;
using System.Xml;
using CsQuery.Implementation;

namespace CsQuery.HtmlParser
{
	class BOMReader
	{
		public BOMReader(Stream stream)
		{
			this.DefaultEncoding = Encoding.UTF8;
			this.InputStream = stream;
			this.Parse();
		}

		public Encoding Encoding { get; protected set; }

		public Encoding GetEncoding(Encoding defaultEncoding)
		{
			return this.Encoding ?? defaultEncoding;
		}

		public Stream StreamWithoutBOM
		{
			get
			{
				if (!this.IsBOM)
				{
					return this.StreamWithBOM;
				}
				Stream result;
				if (this.InputStream.CanSeek)
				{
					this.InputStream.Position = (long)this.BomLength;
					result = this.InputStream;
				}
				else if (this.InputStream.CanRead)
				{
					MemoryStream memoryStream = new MemoryStream(this.Header, this.BomLength, this.BytesRead - this.BomLength);
					result = new CombinedStream(new Stream[] { memoryStream, this.InputStream });
				}
				else
				{
					result = new MemoryStream(this.Header, this.BomLength, this.BytesRead - this.BytesRead);
				}
				return result;
			}
		}

		public Stream StreamWithBOM
		{
			get
			{
				Stream result;
				if (this.InputStream.CanSeek)
				{
					this.InputStream.Position = 0L;
					result = this.InputStream;
				}
				else if (this.InputStream.CanRead)
				{
					MemoryStream memoryStream = new MemoryStream(this.Header, 0, this.BytesRead);
					result = new CombinedStream(new Stream[] { memoryStream, this.InputStream });
				}
				else
				{
					result = new MemoryStream(this.Header, 0, this.BytesRead);
				}
				return result;
			}
		}

		public Encoding DefaultEncoding { get; protected set; }

		public bool IsBOM { get; protected set; }

		public bool IsXML { get; protected set; }

		protected void Parse()
		{
			this.BytesRead = this.InputStream.Read(this.Header, 0, 5);
			this.Encoding = this.GetFileEncoding();
		}

		Encoding GetFileEncoding()
		{
			Encoding result = null;
			if (this.Matches(new byte[] { 239, 187, 191 }))
			{
				result = Encoding.UTF8;
				this.BomLength = 3;
				this.IsBOM = true;
			}
			else if (this.Matches(new byte[] { 0, 0, 254, 255 }))
			{
				result = new UTF32Encoding(true, true);
				this.BomLength = 4;
				this.IsBOM = true;
			}
			else
			{
				byte[] array = new byte[4];
				array[0] = byte.MaxValue;
				array[1] = 254;
				if (this.Matches(array))
				{
					result = Encoding.UTF32;
					this.BomLength = 4;
					this.IsBOM = true;
				}
				else if (this.Matches(new byte[] { 43, 47, 118 }))
				{
					result = Encoding.UTF7;
					this.BomLength = 3;
					this.IsBOM = true;
				}
				else if (this.Matches(new byte[] { 254, 255 }))
				{
					result = Encoding.BigEndianUnicode;
					this.BomLength = 2;
					this.IsBOM = true;
				}
				else if (this.Matches(new byte[] { 254, 255 }))
				{
					result = Encoding.Unicode;
					this.BomLength = 2;
					this.IsBOM = true;
				}
				else if (this.Matches(new byte[] { 60, 63, 120, 109, 108 }))
				{
					result = this.GetEncodingFromXML();
					this.BomLength = 0;
					this.IsBOM = true;
					this.IsXML = true;
				}
			}
			return result;
		}

		Encoding GetEncodingFromXML()
		{
			byte[] array = new byte[256];
			this.Header.CopyTo(array, 0);
			this.BytesRead += this.InputStream.Read(array, this.BytesRead, 256 - this.BytesRead);
			this.Header = array;
			string text = "";
			char c = '\0';
			for (int i = 0; i < this.BytesRead; i++)
			{
				char c2 = c;
				c = (char)this.Header[i];
				text += c;
				if (c2 == '?' && c == '>')
				{
					break;
				}
			}
			if (!string.IsNullOrEmpty(text))
			{
				XmlDocument xmlDocument = new XmlDocument();
				try
				{
					xmlDocument.LoadXml(text + "<root></root>");
					XmlDeclaration xmlDeclaration = (XmlDeclaration)xmlDocument.ChildNodes[0];
					return HtmlEncoding.GetEncoding(xmlDeclaration.Encoding);
				}
				catch
				{
				}
			}
			return null;
		}

		bool Matches(byte[] buffer)
		{
			for (int i = 0; i < buffer.Length; i++)
			{
				if (this.Header[i] != buffer[i])
				{
					return false;
				}
			}
			return true;
		}

		const int XmlBlockSize = 256;

		Stream InputStream;

		byte[] Header = new byte[5];

		int BytesRead;

		int BomLength;
	}
}
