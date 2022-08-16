using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using Telerik.Windows.Documents.Flow.FormatProviders.Rtf.Exceptions;
using Telerik.Windows.Documents.Flow.FormatProviders.Rtf.Import.Model;
using Telerik.Windows.Documents.Flow.FormatProviders.Rtf.Utilities;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Rtf.Import.Parser
{
	sealed class RtfParser : RtfParserBase, IDisposable
	{
		public RtfParser()
		{
		}

		public RtfParser(params RtfParserListenerBase[] listeners)
			: base(listeners)
		{
		}

		public void Dispose()
		{
			this.hexDecodingBuffer.Dispose();
		}

		protected override void DoParse(RtfSource rtfTextSource)
		{
			base.NotifyParseBegin();
			try
			{
				this.ParseRtf(rtfTextSource.Reader);
				base.NotifyParseSuccess();
			}
			catch (RtfException reason)
			{
				base.NotifyParseFail(reason);
				throw;
			}
			finally
			{
				base.NotifyParseEnd();
			}
		}

		static bool IsASCIILetter(int character)
		{
			return (character >= 97 && character <= 122) || (character >= 65 && character <= 90);
		}

		static bool IsHexDigit(int character)
		{
			return (character >= 48 && character <= 57) || (character >= 97 && character <= 102) || (character >= 65 && character <= 70);
		}

		static bool IsDigit(int character)
		{
			return character >= 48 && character <= 57;
		}

		static int ReadOneByte(RtfReader reader)
		{
			int num = reader.Read();
			if (num == -1)
			{
				throw new RtfUnicodeEncodingException("UnexpectedEndOfFile");
			}
			return num;
		}

		static int PeekNextChar(RtfReader reader, bool mandatory)
		{
			int num = reader.Peek();
			if (mandatory && num == -1)
			{
				throw new RtfMultiByteEncodingException("EndOfFileInvalidCharacter");
			}
			return num;
		}

		static void UpdateEncoding(RtfReader reader, RtfTag tag)
		{
			string name;
			if ((name = tag.Name) != null)
			{
				if (name == "ansi")
				{
					RtfParser.UpdateEncoding(reader, CharSetHelper.AnsiCodePage);
					return;
				}
				if (name == "mac")
				{
					RtfParser.UpdateEncoding(reader, 10000);
					return;
				}
				if (name == "pc")
				{
					RtfParser.UpdateEncoding(reader, 437);
					return;
				}
				if (name == "pca")
				{
					RtfParser.UpdateEncoding(reader, 850);
					return;
				}
				if (!(name == "ansicpg"))
				{
					return;
				}
				RtfParser.UpdateEncoding(reader, tag.ValueAsNumber);
			}
		}

		static void UpdateEncoding(RtfReader reader, int codePage)
		{
			reader.CodePageDecoder.CurrentCodePage = codePage;
		}

		static char ReadOneChar(RtfReader reader)
		{
			return (char)reader.Read();
		}

		void ParseRtf(RtfReader reader)
		{
			this.curText = new StringBuilder();
			this.unicodeSkipCountStack.Clear();
			this.codePageStack.Clear();
			this.unicodeSkipCount = 1;
			this.level = 0;
			this.tagCountAtLastGroupStart = 0;
			this.tagCount = 0;
			this.fontTableStartLevel = -1;
			this.targetFont = null;
			this.expectingThemeFont = false;
			this.fontToCodePageMapping.Clear();
			this.hexDecodingBuffer.SetLength(0L);
			int num = 0;
			int num2 = RtfParser.PeekNextChar(reader, false);
			bool flag = false;
			while (num2 != -1)
			{
				int num3 = 0;
				bool flag2 = false;
				int num4 = num2;
				switch (num4)
				{
				case 9:
					reader.Read();
					this.HandleTag(reader, new RtfTag("tab"));
					break;
				case 10:
				case 13:
					reader.Read();
					break;
				case 11:
				case 12:
					goto IL_39C;
				default:
					if (num4 != 92)
					{
						switch (num4)
						{
						case 123:
							reader.Read();
							this.FlushText();
							base.NotifyGroupBegin();
							this.tagCountAtLastGroupStart = this.tagCount;
							this.unicodeSkipCountStack.Push(this.unicodeSkipCount);
							this.codePageStack.Push(reader.CodePageDecoder.CurrentCodePage);
							this.level++;
							break;
						case 124:
							goto IL_39C;
						case 125:
							reader.Read();
							this.FlushText();
							if (this.level <= 0)
							{
								throw new RtfBraceNestingException("ToManyBraces");
							}
							this.unicodeSkipCount = this.unicodeSkipCountStack.Pop();
							if (this.fontTableStartLevel == this.level)
							{
								this.fontTableStartLevel = -1;
								this.targetFont = null;
								this.expectingThemeFont = false;
							}
							RtfParser.UpdateEncoding(reader, this.codePageStack.Pop());
							this.level--;
							base.NotifyGroupEnd();
							num++;
							break;
						default:
							goto IL_39C;
						}
					}
					else
					{
						if (!flag)
						{
							reader.Read();
						}
						int num5 = RtfParser.PeekNextChar(reader, true);
						int num6 = num5;
						if (num6 <= 42)
						{
							if (num6 <= 13)
							{
								if (num6 != 10 && num6 != 13)
								{
									goto IL_28D;
								}
								reader.Read();
								this.HandleTag(reader, new RtfTag("par"));
								break;
							}
							else if (num6 != 39)
							{
								if (num6 != 42)
								{
									goto IL_28D;
								}
							}
							else
							{
								reader.Read();
								char c = (char)RtfParser.ReadOneByte(reader);
								char c2 = (char)RtfParser.ReadOneByte(reader);
								if (!RtfParser.IsHexDigit((int)c))
								{
									throw new RtfHexEncodingException(string.Format("InvalidFirstHexDigit: {0}", c));
								}
								if (!RtfParser.IsHexDigit((int)c2))
								{
									throw new RtfHexEncodingException(string.Format("InvalidFirstHexDigit: {0}", c2));
								}
								int num7 = int.Parse(string.Format("{0}{1}{2}", string.Empty, c, c2), NumberStyles.HexNumber);
								this.hexDecodingBuffer.WriteByte((byte)num7);
								num3 = RtfParser.PeekNextChar(reader, false);
								flag2 = true;
								bool flag3 = true;
								if (num3 == 92)
								{
									reader.Read();
									flag = true;
									int num8 = RtfParser.PeekNextChar(reader, false);
									if (num8 == 39)
									{
										flag3 = false;
									}
								}
								if (flag3)
								{
									this.DecodeCurrentHexBuffer(reader);
									break;
								}
								break;
							}
						}
						else
						{
							if (num6 > 58)
							{
								if (num6 != 92)
								{
									if (num6 == 95)
									{
										goto IL_261;
									}
									switch (num6)
									{
									case 123:
									case 125:
										break;
									case 124:
									case 126:
										goto IL_261;
									default:
										goto IL_28D;
									}
								}
								this.curText.Append(RtfParser.ReadOneChar(reader));
								break;
							}
							if (num6 != 45 && num6 != 58)
							{
								goto IL_28D;
							}
						}
						IL_261:
						this.HandleTag(reader, new RtfTag(string.Format("{0}{1}", string.Empty, RtfParser.ReadOneChar(reader))));
						break;
						IL_28D:
						this.ParseTag(reader);
					}
					break;
				}
				IL_3AE:
				if (this.level == 0 && base.IgnoreContentAfterRootGroup)
				{
					break;
				}
				if (flag2)
				{
					num2 = num3;
					continue;
				}
				num2 = RtfParser.PeekNextChar(reader, false);
				flag = false;
				continue;
				IL_39C:
				this.curText.Append(RtfParser.ReadOneChar(reader));
				goto IL_3AE;
			}
			reader.Close();
			if (this.level > 0)
			{
				throw new RtfBraceNestingException("ToFewBraces");
			}
			if (num == 0)
			{
				throw new RtfEmptyDocumentException("NoRtfContent");
			}
			this.curText = null;
		}

		void ParseTag(RtfReader reader)
		{
			StringBuilder stringBuilder = new StringBuilder();
			StringBuilder stringBuilder2 = null;
			bool flag = true;
			bool flag2 = false;
			int num = RtfParser.PeekNextChar(reader, true);
			while (!flag2)
			{
				if (flag && RtfParser.IsASCIILetter(num))
				{
					stringBuilder.Append(RtfParser.ReadOneChar(reader));
				}
				else if (RtfParser.IsDigit(num) || (num == 45 && stringBuilder2 == null))
				{
					flag = false;
					if (stringBuilder2 == null)
					{
						stringBuilder2 = new StringBuilder();
					}
					stringBuilder2.Append(RtfParser.ReadOneChar(reader));
				}
				else
				{
					flag2 = true;
					RtfTag tag;
					if (stringBuilder2 != null && stringBuilder2.Length > 0)
					{
						tag = new RtfTag(stringBuilder.ToString(), stringBuilder2.ToString());
					}
					else
					{
						tag = new RtfTag(stringBuilder.ToString());
					}
					bool flag3 = this.HandleTag(reader, tag);
					if (num == 32 && !flag3)
					{
						reader.Read();
					}
				}
				if (!flag2)
				{
					num = RtfParser.PeekNextChar(reader, true);
				}
			}
		}

		bool HandleTag(RtfReader reader, RtfTag tag)
		{
			if (this.level == 0)
			{
				throw new RtfStructureException(string.Format("TagOnRootLevel: {0}", tag.ToString()));
			}
			if (this.tagCount < 4)
			{
				RtfParser.UpdateEncoding(reader, tag);
			}
			string name = tag.Name;
			bool flag = this.expectingThemeFont;
			if (this.tagCountAtLastGroupStart == this.tagCount)
			{
				string key;
				switch (key = name)
				{
				case "flomajor":
				case "fhimajor":
				case "fdbmajor":
				case "fbimajor":
				case "flominor":
				case "fhiminor":
				case "fdbminor":
				case "fbiminor":
					this.expectingThemeFont = true;
					break;
				}
				flag = true;
			}
			string a;
			if (flag && (a = name) != null)
			{
				if (!(a == "f"))
				{
					if (a == "fonttbl")
					{
						this.fontTableStartLevel = this.level;
					}
				}
				else if (this.fontTableStartLevel > 0)
				{
					this.targetFont = tag.FullName;
					this.expectingThemeFont = false;
				}
			}
			if (this.targetFont != null && "fcharset".Equals(name))
			{
				int valueAsNumber = tag.ValueAsNumber;
				int codePage = CharSetHelper.GetCodePage(valueAsNumber);
				this.fontToCodePageMapping[this.targetFont] = codePage;
				RtfParser.UpdateEncoding(reader, codePage);
			}
			if (this.fontToCodePageMapping.Count > 0 && "f".Equals(name) && this.fontToCodePageMapping.ContainsKey(tag.FullName))
			{
				RtfParser.UpdateEncoding(reader, this.fontToCodePageMapping[tag.FullName]);
			}
			bool result = false;
			string a2;
			if ((a2 = name) != null)
			{
				if (a2 == "u")
				{
					this.curText.Append((char)tag.ValueAsNumber);
					int i = 0;
					while (i < this.unicodeSkipCount)
					{
						int num2 = RtfParser.PeekNextChar(reader, true);
						int num3 = num2;
						if (num3 <= 13)
						{
							if (num3 != 10 && num3 != 13)
							{
								goto IL_2CF;
							}
							goto IL_283;
						}
						else
						{
							if (num3 == 32)
							{
								goto IL_283;
							}
							if (num3 != 92)
							{
								switch (num3)
								{
								case 123:
								case 125:
									i = this.unicodeSkipCount;
									break;
								case 124:
									goto IL_2CF;
								default:
									goto IL_2CF;
								}
							}
							else
							{
								reader.Read();
								result = true;
								int num4 = RtfParser.ReadOneByte(reader);
								int num5 = num4;
								if (num5 == 39)
								{
									RtfParser.ReadOneByte(reader);
									RtfParser.ReadOneByte(reader);
								}
							}
						}
						IL_2D9:
						i++;
						continue;
						IL_283:
						reader.Read();
						result = true;
						if (i == 0)
						{
							i--;
							goto IL_2D9;
						}
						goto IL_2D9;
						IL_2CF:
						reader.Read();
						result = true;
						goto IL_2D9;
					}
					goto IL_366;
				}
				if (!(a2 == "uc"))
				{
					if (a2 == "bin")
					{
						if (RtfParser.PeekNextChar(reader, true) == 32)
						{
							reader.Read();
						}
						if (tag.ValueAsNumber > 0)
						{
							RtfBinary bin = new RtfBinary(reader.ReadBinary(tag.ValueAsNumber));
							base.NotifyBinaryFound(bin);
							goto IL_366;
						}
						goto IL_366;
					}
				}
				else
				{
					int valueAsNumber2 = tag.ValueAsNumber;
					if (valueAsNumber2 < 0 || valueAsNumber2 > 10)
					{
						throw new RtfUnicodeEncodingException(string.Format("InvalidUnicodeSkipCount: {0}", tag.ToString()));
					}
					this.unicodeSkipCount = valueAsNumber2;
					goto IL_366;
				}
			}
			this.FlushText();
			base.NotifyTagFound(tag);
			IL_366:
			this.tagCount++;
			return result;
		}

		void DecodeCurrentHexBuffer(RtfReader reader)
		{
			this.hexDecodingBuffer.Seek(0L, SeekOrigin.Begin);
			int b;
			while ((b = this.hexDecodingBuffer.ReadByte()) != -1)
			{
				this.curText.Append(reader.CodePageDecoder.Convert(b));
			}
			this.hexDecodingBuffer.SetLength(0L);
		}

		void FlushText()
		{
			if (this.curText.Length > 0)
			{
				if (this.level == 0)
				{
					throw new RtfStructureException(string.Format("TextOnRootLevel: {0}", this.curText.ToString()));
				}
				base.NotifyTextFound(new RtfText(this.curText.ToString()));
				this.curText.Remove(0, this.curText.Length);
			}
		}

		const int EndOfFile = -1;

		readonly Stack<int> unicodeSkipCountStack = new Stack<int>();

		readonly Stack<int> codePageStack = new Stack<int>();

		readonly Dictionary<string, int> fontToCodePageMapping = new Dictionary<string, int>();

		readonly MemoryStream hexDecodingBuffer = new MemoryStream();

		StringBuilder curText;

		int unicodeSkipCount;

		int level;

		int tagCountAtLastGroupStart;

		int tagCount;

		int fontTableStartLevel;

		string targetFont;

		bool expectingThemeFont;
	}
}
