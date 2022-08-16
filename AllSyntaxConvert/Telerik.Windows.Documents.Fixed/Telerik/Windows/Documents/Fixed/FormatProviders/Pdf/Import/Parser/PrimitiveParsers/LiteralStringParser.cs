using System;
using System.Collections.Generic;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Types;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Import.Parser.PrimitiveParsers
{
	class LiteralStringParser
	{
		static LiteralStringParser()
		{
			LiteralStringParser.singleEscapedCharacterToByte.Add(110, 10);
			LiteralStringParser.singleEscapedCharacterToByte.Add(114, 13);
			LiteralStringParser.singleEscapedCharacterToByte.Add(116, 9);
			LiteralStringParser.singleEscapedCharacterToByte.Add(98, 8);
			LiteralStringParser.singleEscapedCharacterToByte.Add(102, 12);
			LiteralStringParser.singleEscapedCharacterToByte.Add(40, 40);
			LiteralStringParser.singleEscapedCharacterToByte.Add(41, 41);
			LiteralStringParser.singleEscapedCharacterToByte.Add(92, 92);
		}

		public LiteralStringParser(PostScriptReader parser)
		{
			Guard.ThrowExceptionIfNull<PostScriptReader>(parser, "parser");
			this.parser = parser;
			this.token = new List<byte>();
			this.octalSequenceStack = new Stack<byte>();
			this.escapedByteHandlers = new Func<byte, bool>[]
			{
				new Func<byte, bool>(this.TryHandleEscapedOctalSequence),
				new Func<byte, bool>(this.TryHandleEndOfLineSymbolsInEscapeMode),
				new Func<byte, bool>(this.TryHandleSingleEscapedSymbol)
			};
		}

		public void Append(PostScriptReaderArgs args)
		{
			byte @byte = args.Byte;
			if (this.isInEscapeMode)
			{
				this.HandleByteInEscapeMode(@byte);
				return;
			}
			this.HandleByteWhenNotEscaping(@byte);
		}

		public bool IsLiteralStringEnd(PostScriptReaderArgs args)
		{
			return args.Byte == 41 && this.openedParentheses == 0 && (!this.isInEscapeMode || this.octalSequenceStack.Count > 0 || this.hasReadCarriageReturn);
		}

		public void Complete(PostScriptReaderArgs args)
		{
			Guard.ThrowExceptionIfNull<PostScriptReaderArgs>(args, "args");
			if (this.hasReadCarriageReturn && !this.isInEscapeMode)
			{
				this.token.Add(10);
			}
			else if (this.octalSequenceStack.Count > 0)
			{
				this.AddAndClearAccumulatedOctalNumber();
			}
			byte[] data = this.token.ToArray();
			byte[] initialValue = args.Context.DecryptString(data);
			PdfLiteralString primitive = new PdfLiteralString(initialValue);
			this.parser.PushToken(primitive);
			this.Reset();
		}

		void HandleByteInEscapeMode(byte character)
		{
			bool flag = false;
			for (int i = 0; i < this.escapedByteHandlers.Length; i++)
			{
				Func<byte, bool> func = this.escapedByteHandlers[i];
				if (func(character))
				{
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				this.StopEscapingIgnoringPreviousBackslash(character);
			}
		}

		void HandleByteWhenNotEscaping(byte character)
		{
			this.EnsureAddingPreviouslyReadCariageReturn(character);
			if (character == 92)
			{
				this.isInEscapeMode = true;
				return;
			}
			if (!this.hasReadCarriageReturn)
			{
				this.AppendByteCountingBrackets(character);
			}
		}

		bool TryHandleEscapedOctalSequence(byte character)
		{
			bool flag = !this.hasReadCarriageReturn;
			if (flag)
			{
				bool flag2 = Characters.IsOctalChar(character);
				flag = flag2 || this.octalSequenceStack.Count > 0;
				if (flag2)
				{
					this.octalSequenceStack.Push(character);
					if (this.octalSequenceStack.Count == 3)
					{
						this.AddAndClearAccumulatedOctalNumber();
					}
				}
				else if (this.octalSequenceStack.Count > 0)
				{
					this.AddAndClearAccumulatedOctalNumber();
					this.HandleByteWhenNotEscaping(character);
				}
			}
			return flag;
		}

		bool TryHandleEndOfLineSymbolsInEscapeMode(byte character)
		{
			bool result = character == 10 || character == 13 || this.hasReadCarriageReturn;
			if (character == 10)
			{
				this.hasReadCarriageReturn = false;
				this.isInEscapeMode = false;
			}
			else if (character == 13)
			{
				if (this.hasReadCarriageReturn)
				{
					this.isInEscapeMode = false;
				}
				this.hasReadCarriageReturn = true;
			}
			else if (this.hasReadCarriageReturn)
			{
				this.hasReadCarriageReturn = false;
				this.isInEscapeMode = false;
				this.HandleByteWhenNotEscaping(character);
			}
			return result;
		}

		bool TryHandleSingleEscapedSymbol(byte character)
		{
			byte item;
			if (LiteralStringParser.singleEscapedCharacterToByte.TryGetValue(character, out item))
			{
				this.isInEscapeMode = false;
				this.token.Add(item);
				return true;
			}
			return false;
		}

		void AddAndClearAccumulatedOctalNumber()
		{
			this.isInEscapeMode = false;
			int num = 1;
			int num2 = 0;
			while (this.octalSequenceStack.Count > 0)
			{
				byte b = this.octalSequenceStack.Pop();
				int num3 = (int)(b - 48);
				num2 += num3 * num;
				num *= 8;
			}
			byte item = (byte)(num2 % 256);
			this.token.Add(item);
		}

		void StopEscapingIgnoringPreviousBackslash(byte character)
		{
			this.isInEscapeMode = false;
			this.HandleByteWhenNotEscaping(character);
		}

		void EnsureAddingPreviouslyReadCariageReturn(byte currentCharacter)
		{
			if (this.hasReadCarriageReturn && currentCharacter != 10)
			{
				this.token.Add(10);
			}
			this.hasReadCarriageReturn = currentCharacter == 13;
		}

		void AppendByteCountingBrackets(byte character)
		{
			if (character == 40)
			{
				this.openedParentheses++;
			}
			else if (character == 41)
			{
				this.openedParentheses--;
			}
			this.token.Add(character);
		}

		void Reset()
		{
			this.token.Clear();
			this.isInEscapeMode = false;
			this.hasReadCarriageReturn = false;
		}

		const byte LeftBracket = 40;

		const byte RightBracket = 41;

		const byte EscapingSlash = 92;

		const byte CarriageReturn = 13;

		const byte EndOfLineMarker = 10;

		const byte HorizontalTab = 9;

		const byte Backspace = 8;

		const byte FormFeed = 12;

		static readonly Dictionary<byte, byte> singleEscapedCharacterToByte = new Dictionary<byte, byte>();

		readonly PostScriptReader parser;

		readonly Func<byte, bool>[] escapedByteHandlers;

		readonly Stack<byte> octalSequenceStack;

		readonly List<byte> token;

		bool hasReadCarriageReturn;

		bool isInEscapeMode;

		int openedParentheses;
	}
}
