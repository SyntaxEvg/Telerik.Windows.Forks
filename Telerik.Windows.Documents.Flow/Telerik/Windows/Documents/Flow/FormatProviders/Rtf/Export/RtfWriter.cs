using System;
using System.Diagnostics;
using System.IO;
using Telerik.Windows.Documents.Flow.FormatProviders.Rtf.Exceptions;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Rtf.Export
{
	class RtfWriter : IDisposable
	{
		public RtfWriter(Stream o)
		{
			this.writer = o;
			this.textWriter = new StreamWriter(this.writer);
		}

		public void WriteTag(string tagName)
		{
			this.InnerWrite("\\");
			this.InnerWrite(tagName);
			this.shouldAddSpace = true;
		}

		public void WriteTag(string tagName, int parameter)
		{
			this.WriteTag(tagName);
			this.InnerWrite(parameter.ToString());
			this.shouldAddSpace = true;
		}

		public void WriteGroupStart()
		{
			this.InnerWrite("{");
			this.groupLevel++;
			this.shouldAddSpace = false;
		}

		public void WriteGroupStart(string tagName, bool isExtensionGroup)
		{
			this.WriteGroupStart();
			if (isExtensionGroup)
			{
				this.InnerWrite("\\*");
			}
			this.WriteTag(tagName);
		}

		public void WriteGroupStart(string tagName, int parameter, bool isExtensionGroup)
		{
			this.WriteGroupStart();
			if (isExtensionGroup)
			{
				this.InnerWrite("\\*");
			}
			this.WriteTag(tagName, parameter);
		}

		public RtfGroupContext WriteGroup()
		{
			this.WriteGroupStart();
			return new RtfGroupContext(this);
		}

		public RtfGroupContext WriteGroup(string tagName, bool isExtensionGroup = false)
		{
			this.WriteGroupStart(tagName, isExtensionGroup);
			return new RtfGroupContext(this);
		}

		public RtfGroupContext WriteGroup(string tagName, int parameter, bool isExtensionGroup = false)
		{
			this.WriteGroupStart(tagName, parameter, isExtensionGroup);
			return new RtfGroupContext(this);
		}

		public void WriteGroupEnd()
		{
			this.groupLevel--;
			if (this.groupLevel < 0)
			{
				throw new RtfException("Group level reached negative value");
			}
			this.InnerWrite("}");
			this.shouldAddSpace = false;
		}

		public void WriteText(string text)
		{
			if (text == null || text.Length == 0)
			{
				return;
			}
			if (this.shouldAddSpace)
			{
				this.InnerWriteChar(' ');
				this.shouldAddSpace = false;
			}
			foreach (char c in text)
			{
				this.WriteChar(c);
			}
		}

		public void WriteChar(char c)
		{
			if (c < '\0' || c > '\u007f')
			{
				this.WriteUnicodeChar(c);
				return;
			}
			switch (c)
			{
			case '\t':
				this.WriteTag("tab");
				this.InnerWriteChar(' ');
				return;
			case '\n':
				this.WriteTag("tab");
				this.InnerWriteChar(' ');
				return;
			default:
				if (c != '\\')
				{
					switch (c)
					{
					case '{':
					case '}':
						goto IL_60;
					}
					this.InnerWriteChar(c);
					return;
				}
				IL_60:
				this.InnerWriteChar('\\');
				this.InnerWriteChar(c);
				return;
			}
		}

		public void WriteHex(int number)
		{
			this.InnerWrite(string.Format("\\'{0:x2}", number));
		}

		public void WriteHex(byte[] bytes)
		{
			if (this.shouldAddSpace)
			{
				this.InnerWriteChar(' ');
				this.shouldAddSpace = false;
			}
			int i = 0;
			int num = bytes.Length;
			while (i < num)
			{
				this.textWriter.Write(RtfWriter.HexChars[bytes[i] >> 4]);
				this.textWriter.Write(RtfWriter.HexChars[(int)(bytes[i] & 15)]);
				i++;
			}
		}

		[Conditional("DEBUG")]
		public void DebugWriteNewLine()
		{
			this.InnerWrite(Environment.NewLine);
		}

		public void Flush()
		{
			this.textWriter.Flush();
			this.writer.Flush();
		}

		public void Dispose()
		{
			this.textWriter.Dispose();
			this.writer.Dispose();
		}

		void InnerWrite(string text)
		{
			this.textWriter.Write(text);
		}

		void InnerWriteChar(char c)
		{
			this.textWriter.Write(c);
		}

		void WriteUnicodeChar(char c)
		{
			short num = (short)c;
			this.textWriter.Write(string.Format("\\u{0}{1}", num, RtfWriter.UnicodeSubstituteChar));
		}

		static readonly string HexChars = "0123456789abcdef";

		static readonly string UnicodeSubstituteChar = "?";

		readonly Stream writer;

		readonly StreamWriter textWriter;

		int groupLevel;

		bool shouldAddSpace;
	}
}
