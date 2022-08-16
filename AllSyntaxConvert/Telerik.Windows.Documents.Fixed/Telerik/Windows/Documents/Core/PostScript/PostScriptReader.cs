using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Telerik.Windows.Documents.Core.PostScript.Data;
using Telerik.Windows.Documents.Core.PostScript.Encryption;
using Telerik.Windows.Documents.Core.PostScript.Operators;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Core.PostScript
{
	class PostScriptReader : ReaderBase, IPostScriptReader
	{
		public PostScriptReader(byte[] data)
			: base(data)
		{
			this.encryption = new EncryptionCollection();
			this.decryptedBuffer = new Queue<byte>();
		}

		public string Result { get; set; }

		public override long Position
		{
			get
			{
				return base.Position - (long)this.decryptedBuffer.Count;
			}
		}

		public Token ReadToken()
		{
			this.SkipUnusedCharacters();
			if (this.EndOfFile)
			{
				return Token.Unknown;
			}
			char c = (char)this.Peek(0);
			if (c <= '/')
			{
				if (c == '(')
				{
					return this.ReadLiteralString();
				}
				if (c == '/')
				{
					return this.ReadName();
				}
			}
			else
			{
				if (c != '>')
				{
					switch (c)
					{
					case '[':
						break;
					case '\\':
						goto IL_81;
					case ']':
						goto IL_6A;
					default:
						switch (c)
						{
						case '{':
							break;
						case '|':
							goto IL_81;
						case '}':
							goto IL_6A;
						default:
							goto IL_81;
						}
						break;
					}
					this.Read();
					return Token.ArrayStart;
					IL_6A:
					this.Read();
					return Token.ArrayEnd;
				}
				return this.ReadHexadecimalString();
			}
			IL_81:
			if (Characters.IsValidNumberChar(this))
			{
				return this.ReadNumber();
			}
			if (Characters.IsLetter((int)this.Peek(0)) || this.Peek(0) == 45 || this.Peek(0) == 124)
			{
				return this.ReadOperatorOrKeyword();
			}
			this.Read();
			return Token.Unknown;
		}

		internal void SkipUnusedCharacters()
		{
			PostScriptReaderHelper.SkipUnusedCharacters(this);
		}

		internal void GoToNextLine()
		{
			PostScriptReaderHelper.GoToNextLine(this, true);
		}

		internal void SkipWhiteSpaces()
		{
			PostScriptReaderHelper.SkipWhiteSpaces(this);
		}

		Token ReadOperatorOrKeyword()
		{
			this.Result = PostScriptReaderHelper.ReadKeyword(this);
			string result;
			if ((result = this.Result) != null && (result == "true" || result == "false"))
			{
				return Token.Boolean;
			}
			if (Keywords.IsKeyword(this.Result))
			{
				return Token.Keyword;
			}
			if (Operator.IsOperator(this.Result))
			{
				return Token.Operator;
			}
			return Token.Unknown;
		}

		Token ReadName()
		{
			this.Result = PostScriptReaderHelper.ReadName(this);
			return Token.Name;
		}

		Token ReadNumber()
		{
			this.Result = PostScriptReaderHelper.ReadNumber(this);
			if (!this.Result.Contains('.'))
			{
				return Token.Integer;
			}
			return Token.Real;
		}

		Token ReadHexadecimalString()
		{
			this.Result = PostScriptReaderHelper.GetString(PostScriptReaderHelper.ReadHexadecimalString(this));
			return Token.String;
		}

		Token ReadLiteralString()
		{
			this.Result = PostScriptReaderHelper.GetString(PostScriptReaderHelper.ReadLiteralString(this));
			return Token.String;
		}

		public override void BeginReadingBlock()
		{
			if (this.encryption.HasEncryption)
			{
				Guard.ThrowNotSupportedException();
			}
			base.BeginReadingBlock();
		}

		public override void EndReadingBlock()
		{
			if (this.encryption.HasEncryption)
			{
				Guard.ThrowNotSupportedException();
			}
			base.EndReadingBlock();
		}

		public override void Seek(long offset, SeekOrigin origin)
		{
			if (this.encryption.HasEncryption)
			{
				Guard.ThrowNotSupportedException();
			}
			base.Seek(offset, origin);
		}

		public override byte Read()
		{
			if (!this.encryption.HasEncryption)
			{
				byte b = base.Read();
				if (b == 13)
				{
					if (!this.EndOfFile && this.Peek(0) == 10)
					{
						this.Read();
					}
					b = 10;
				}
				return b;
			}
			if (this.decryptedBuffer.Count > 0)
			{
				return this.decryptedBuffer.Dequeue();
			}
			return this.encryption.Decrypt(base.Read());
		}

		public override byte Peek(int skip = 0)
		{
			byte b = 0;
			if (!this.encryption.HasEncryption)
			{
				b = base.Peek(skip);
				if (b == 13)
				{
					b = 10;
				}
				return b;
			}
			if (skip < this.decryptedBuffer.Count)
			{
				return this.decryptedBuffer.ElementAt(skip);
			}
			skip -= this.decryptedBuffer.Count;
			for (int i = 0; i <= skip; i++)
			{
				b = this.encryption.Decrypt(base.Read());
				this.decryptedBuffer.Enqueue(b);
			}
			return b;
		}

		public void PushEncryption(EncryptionBase encrypt)
		{
			this.encryption.PushEncryption(encrypt);
		}

		public void PopEncryption()
		{
			this.encryption.PopEncryption();
			if (!this.encryption.HasEncryption)
			{
				this.decryptedBuffer.Clear();
			}
		}

		byte IPostScriptReader.Read(bool appendLineFeed)
		{
			return this.Read();
		}

		readonly EncryptionCollection encryption;

		readonly Queue<byte> decryptedBuffer;
	}
}
