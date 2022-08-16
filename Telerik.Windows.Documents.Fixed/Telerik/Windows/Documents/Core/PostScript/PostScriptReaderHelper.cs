using System;
using System.Collections.Generic;
using System.Text;
using Telerik.Windows.Documents.Core.PostScript.Data;

namespace Telerik.Windows.Documents.Core.PostScript
{
	static class PostScriptReaderHelper
	{
		static PostScriptReaderHelper()
		{
			for (int i = 0; i < 10; i++)
			{
				PostScriptReaderHelper.charValues[48 + i] = i;
			}
			for (int j = 0; j < 6; j++)
			{
				PostScriptReaderHelper.charValues[97 + j] = 10 + j;
				PostScriptReaderHelper.charValues[65 + j] = 10 + j;
			}
		}

		public static void GoToNextLine(IPostScriptReader reader, bool appendLineFeed = true)
		{
			while (!reader.EndOfFile && reader.Read(appendLineFeed) != 10)
			{
			}
		}

		public static void SkipUnusedCharacters(IPostScriptReader reader)
		{
			PostScriptReaderHelper.SkipWhiteSpaces(reader);
			while (!reader.EndOfFile && reader.Peek(0) == 37)
			{
				PostScriptReaderHelper.GoToNextLine(reader, true);
				PostScriptReaderHelper.SkipWhiteSpaces(reader);
			}
		}

		public static void SkipWhiteSpaces(IPostScriptReader reader)
		{
			while (!reader.EndOfFile && Characters.IsWhiteSpace((int)reader.Peek(0)))
			{
				reader.Read(true);
			}
		}

		public static string GetString(byte[] bytes)
		{
			return Encoding.UTF8.GetString(bytes, 0, bytes.Length);
		}

		public static string ReadNumber(IPostScriptReader reader)
		{
			StringBuilder stringBuilder = new StringBuilder();
			while (!reader.EndOfFile && Characters.IsValidNumberChar(reader))
			{
				stringBuilder.Append((char)reader.Read(true));
			}
			return stringBuilder.ToString();
		}

		public static string ReadName(IPostScriptReader reader)
		{
			StringBuilder stringBuilder = new StringBuilder();
			reader.Read(true);
			while (!reader.EndOfFile && !Characters.IsDelimiter((int)reader.Peek(0)))
			{
				char c = (char)reader.Read(true);
				if (c == '#')
				{
					string value;
					if (PostScriptReaderHelper.TryReadHexChar(reader, out value))
					{
						stringBuilder.Append(value);
					}
					else
					{
						stringBuilder.Append(c);
						stringBuilder.Append(value);
					}
				}
				else
				{
					stringBuilder.Append(c);
				}
			}
			return stringBuilder.ToString();
		}

		public static string ReadKeyword(IPostScriptReader reader)
		{
			StringBuilder stringBuilder = new StringBuilder();
			while (!reader.EndOfFile && !Characters.IsDelimiter((int)reader.Peek(0)) && !char.IsNumber((char)reader.Peek(0)))
			{
				stringBuilder.Append((char)reader.Read(true));
			}
			return stringBuilder.ToString();
		}

		public static byte[] ReadHexadecimalString(IPostScriptReader reader)
		{
			StringBuilder stringBuilder = new StringBuilder();
			reader.Read(true);
			while (!reader.EndOfFile && reader.Peek(0) != 62)
			{
				if (Characters.IsHexChar((int)reader.Peek(0)))
				{
					stringBuilder.Append((char)reader.Read(true));
				}
				else
				{
					reader.Read(true);
				}
			}
			if (!reader.EndOfFile)
			{
				reader.Read(true);
			}
			if (stringBuilder.Length % 2 == 1)
			{
				stringBuilder.Append("0");
			}
			return PostScriptReaderHelper.GetBytesFromHexString(stringBuilder.ToString());
		}

		public static byte[] ReadLiteralString(IPostScriptReader reader)
		{
			List<byte> list = new List<byte>();
			reader.Read(true);
			int i = 1;
			while (i > 0)
			{
				if (reader.Peek(0) == 92)
				{
					if (PostScriptReaderHelper.IsValidEscape((int)reader.Peek(1)))
					{
						reader.Read(true);
						if (reader.Peek(0) != 13 && reader.Peek(0) != 10)
						{
							list.Add((byte)PostScriptReaderHelper.GetSymbolFromEscapeSymbol((int)reader.Read(true)));
						}
						else
						{
							PostScriptReaderHelper.SkipWhiteSpaces(reader);
						}
					}
					else if (Characters.IsOctalChar((int)reader.Peek(1)))
					{
						int num = 3;
						reader.Read(true);
						List<byte> list2 = new List<byte>();
						while (Characters.IsOctalChar((int)reader.Peek(0)))
						{
							if (num <= 0)
							{
								break;
							}
							list2.Add(reader.Read(true) - 48);
							num--;
						}
						while (list2.Count < 3)
						{
							list2.Insert(0, 0);
						}
						int num2 = 0;
						int num3 = 1;
						for (int j = 2; j >= 0; j--)
						{
							num2 += (int)list2[j] * num3;
							num3 *= 8;
						}
						list.Add((byte)num2);
					}
					else
					{
						reader.Read(true);
					}
				}
				else
				{
					if (reader.Peek(0) == 40)
					{
						i++;
					}
					else if (reader.Peek(0) == 41)
					{
						i--;
						if (i == 0)
						{
							break;
						}
					}
					list.Add(reader.Read(true));
				}
			}
			reader.Read(true);
			return list.ToArray();
		}

		public static byte[] GetBytesFromHexString(string hexString)
		{
			if (string.IsNullOrEmpty(hexString))
			{
				return new byte[0];
			}
			if (hexString.Length % 2 != 0)
			{
				hexString = "0" + hexString;
			}
			byte[] array = new byte[hexString.Length >> 1];
			int i = 0;
			int length = hexString.Length;
			while (i < length)
			{
				array[i >> 1] = (byte)((PostScriptReaderHelper.charValues[(int)hexString[i]] << 4) | PostScriptReaderHelper.charValues[(int)hexString[i + 1]]);
				i += 2;
			}
			return array;
		}

		public static bool IsValidEscape(int b)
		{
			if (b <= 92)
			{
				if (b <= 13)
				{
					if (b != 10 && b != 13)
					{
						return false;
					}
				}
				else
				{
					switch (b)
					{
					case 40:
					case 41:
						break;
					default:
						if (b != 92)
						{
							return false;
						}
						break;
					}
				}
			}
			else if (b <= 102)
			{
				if (b != 98 && b != 102)
				{
					return false;
				}
			}
			else if (b != 110)
			{
				switch (b)
				{
				case 114:
				case 116:
					break;
				case 115:
					return false;
				default:
					return false;
				}
			}
			return true;
		}

		public static char GetSymbolFromEscapeSymbol(int symbol)
		{
			if (symbol <= 98)
			{
				switch (symbol)
				{
				case 40:
					return '(';
				case 41:
					return ')';
				default:
					if (symbol == 92)
					{
						return '\\';
					}
					if (symbol == 98)
					{
						return '\b';
					}
					break;
				}
			}
			else
			{
				if (symbol == 102)
				{
					return '\f';
				}
				if (symbol == 110)
				{
					return '\n';
				}
				switch (symbol)
				{
				case 114:
					return '\r';
				case 116:
					return '\t';
				}
			}
			return '\0';
		}

		static bool TryReadHexChar(IPostScriptReader reader, out string result)
		{
			StringBuilder stringBuilder = new StringBuilder();
			for (int i = 0; i < 2; i++)
			{
				char c = (char)reader.Read(true);
				if (reader.EndOfFile || Characters.IsDelimiter((int)c) || !Characters.IsHexChar((int)c))
				{
					result = stringBuilder.ToString();
					return false;
				}
				stringBuilder.Append(c);
			}
			char c2 = (char)PostScriptReaderHelper.GetBytesFromHexString(stringBuilder.ToString())[0];
			result = c2.ToString();
			return true;
		}

		static int[] charValues = new int[128];
	}
}
