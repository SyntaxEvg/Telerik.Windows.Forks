using System;
using System.IO;
using System.Linq;
using System.Text;
using Telerik.Windows.Documents.Core.PostScript;
using Telerik.Windows.Documents.Core.PostScript.Data;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.DocumentModel.Data;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.PdfReader.Readers
{
	abstract class PdfReaderBase : IPostScriptReader
	{
		public PdfReaderBase(Stream stream)
		{
			this.Stream = stream;
			this.ReadBufferedChars();
		}

		public Stream Stream { get; set; }

		public Token TokenType { get; protected set; }

		public long Position
		{
			get
			{
				return this.Stream.Position - (long)this.extraReadBytes;
			}
		}

		public string Result
		{
			get
			{
				return this.token;
			}
		}

		public byte[] BytesToken
		{
			get
			{
				return this.strToken;
			}
		}

		public bool EndOfFile
		{
			get
			{
				return this.Position >= this.Stream.Length;
			}
		}

		public byte Read(bool appendLineFeed = true)
		{
			byte result = this.currentByte;
			this.ReadInternal(appendLineFeed);
			return result;
		}

		public byte Peek(int skip = 0)
		{
			switch (skip)
			{
			case 0:
				return this.currentByte;
			case 1:
				return this.nextByte;
			default:
				throw new NotSupportedException();
			}
		}

		public string ReadLine()
		{
			StringBuilder stringBuilder = new StringBuilder();
			while (this.Peek(0) != 10)
			{
				stringBuilder.Append((char)this.Read(true));
			}
			this.Read(true);
			return stringBuilder.ToString();
		}

		public Token ReadName()
		{
			this.SetToken(PostScriptReaderHelper.ReadName(this));
			return Token.Name;
		}

		public Token ReadNumber()
		{
			string text = PostScriptReaderHelper.ReadNumber(this);
			bool flag = text.Contains('.');
			this.SetToken(text.ToString());
			return this.TokenType = (flag ? Token.Real : Token.Integer);
		}

		public Token ReadHexadecimalString()
		{
			this.strToken = PostScriptReaderHelper.ReadHexadecimalString(this);
			return this.TokenType = Token.String;
		}

		public Token ReadLiteralString()
		{
			this.strToken = PostScriptReaderHelper.ReadLiteralString(this);
			return this.TokenType = Token.String;
		}

		public long Seek(long offset, SeekOrigin origin)
		{
			if (origin == SeekOrigin.Current)
			{
				if (offset < 0L && Math.Abs(offset) > this.Stream.Position)
				{
					offset = 0L;
					origin = SeekOrigin.Begin;
				}
				else if (offset > 0L && this.Stream.Position + offset > this.Stream.Length)
				{
					offset = 0L;
					origin = SeekOrigin.End;
				}
			}
			long result = this.Stream.Seek(offset, origin);
			this.ReadBufferedChars();
			return result;
		}

		public abstract Token ReadToken();

		public int ReadInternal(bool appendLineFeed = true)
		{
			if (this.EndOfFile)
			{
				Guard.ThrowPositionOutOfRangeException();
			}
			this.currentByte = this.nextByte;
			if (this.Stream.Position == this.Stream.Length)
			{
				this.extraReadBytes--;
			}
			this.nextByte = (byte)this.Stream.ReadByte();
			if (this.currentByte == 13)
			{
				if (this.nextByte == 10)
				{
					this.ReadInternal(true);
				}
				else
				{
					this.currentByte = 10;
				}
			}
			return (int)this.currentByte;
		}

		public void SetToken(string tok)
		{
			this.token = tok;
		}

		public void SkipUnusedCharacters()
		{
			PostScriptReaderHelper.SkipUnusedCharacters(this);
		}

		public void GoToNextLine(bool appendLineFeed = true)
		{
			PostScriptReaderHelper.GoToNextLine(this, appendLineFeed);
		}

		public void SkipWhiteSpaces()
		{
			PostScriptReaderHelper.SkipWhiteSpaces(this);
		}

		public byte[] ReadRawData(long offset, StreamPart part)
		{
			part.Seek(0L, SeekOrigin.Begin);
			byte[] result = part.ReadAllBytes();
			this.Seek(offset + part.Length, SeekOrigin.Begin);
			return result;
		}

		protected void ReadBufferedChars()
		{
			if (!this.EndOfFile)
			{
				this.ReadInternal(true);
				if (!this.EndOfFile)
				{
					this.ReadInternal(true);
					this.extraReadBytes = 2;
					return;
				}
				this.extraReadBytes = 1;
			}
		}

		int extraReadBytes;

		string token;

		byte[] strToken;

		byte currentByte;

		byte nextByte;
	}
}
