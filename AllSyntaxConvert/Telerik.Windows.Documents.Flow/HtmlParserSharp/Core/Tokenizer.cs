using System;
using HtmlParserSharp.Common;

namespace HtmlParserSharp.Core
{
	class Tokenizer : ILocator
	{
		public ITokenHandler TokenHandler { get; set; }

		public event EventHandler<EncodingDetectedEventArgs> EncodingDeclared;

		public event EventHandler<ParserErrorEventArgs> ErrorEvent;

		public Tokenizer(ITokenHandler tokenHandler, bool newAttributesEachTime)
		{
			this.TokenHandler = tokenHandler;
			this.newAttributesEachTime = newAttributesEachTime;
			this.bmpChar = new char[1];
			this.astralChar = new char[2];
			this.tagName = null;
			this.attributeName = null;
			this.doctypeName = null;
			this.publicIdentifier = null;
			this.systemIdentifier = null;
			this.attributes = null;
		}

		public Tokenizer(ITokenHandler tokenHandler)
		{
			this.TokenHandler = tokenHandler;
			this.newAttributesEachTime = false;
			this.bmpChar = new char[1];
			this.astralChar = new char[2];
			this.tagName = null;
			this.attributeName = null;
			this.doctypeName = null;
			this.publicIdentifier = null;
			this.systemIdentifier = null;
			this.attributes = null;
		}

		public bool IsMappingLangToXmlLang
		{
			get
			{
				return this.mappingLangToXmlLang == 3;
			}
			set
			{
				this.mappingLangToXmlLang = (value ? 3 : 0);
			}
		}

		public XmlViolationPolicy CommentPolicy
		{
			get
			{
				return this.commentPolicy;
			}
			set
			{
				this.commentPolicy = value;
			}
		}

		public XmlViolationPolicy ContentNonXmlCharPolicy
		{
			set
			{
				if (value != XmlViolationPolicy.Allow)
				{
					throw new ArgumentException("Must use ErrorReportingTokenizer to set contentNonXmlCharPolicy to non-ALLOW.");
				}
			}
		}

		public XmlViolationPolicy ContentSpacePolicy
		{
			get
			{
				return this.contentSpacePolicy;
			}
			set
			{
				this.contentSpacePolicy = value;
			}
		}

		public XmlViolationPolicy XmlnsPolicy
		{
			get
			{
				return this.xmlnsPolicy;
			}
			set
			{
				if (value == XmlViolationPolicy.Fatal)
				{
					throw new ArgumentException("Can't use FATAL here.");
				}
				this.xmlnsPolicy = value;
			}
		}

		public XmlViolationPolicy NamePolicy
		{
			get
			{
				return this.namePolicy;
			}
			set
			{
				this.namePolicy = value;
			}
		}

		public bool Html4ModeCompatibleWithXhtml1Schemata
		{
			get
			{
				return this.html4ModeCompatibleWithXhtml1Schemata;
			}
			set
			{
				this.html4ModeCompatibleWithXhtml1Schemata = value;
			}
		}

		public void SetStateAndEndTagExpectation(TokenizerState specialTokenizerState, [Local] string endTagExpectation)
		{
			this.stateSave = specialTokenizerState;
			if (specialTokenizerState == TokenizerState.DATA)
			{
				return;
			}
			char[] array = endTagExpectation.ToCharArray();
			this.endTagExpectation = ElementName.ElementNameByBuffer(array, 0, array.Length);
			this.EndTagExpectationToArray();
		}

		public void SetStateAndEndTagExpectation(TokenizerState specialTokenizerState, ElementName endTagExpectation)
		{
			this.stateSave = specialTokenizerState;
			this.endTagExpectation = endTagExpectation;
			this.EndTagExpectationToArray();
		}

		void EndTagExpectationToArray()
		{
			DispatchGroup group = this.endTagExpectation.Group;
			switch (group)
			{
			case DispatchGroup.NOFRAMES:
				this.endTagExpectationAsArray = Tokenizer.NOFRAMES_ARR;
				break;
			case DispatchGroup.NOSCRIPT:
				this.endTagExpectationAsArray = Tokenizer.NOSCRIPT_ARR;
				return;
			case DispatchGroup.OPTGROUP:
			case DispatchGroup.OPTION:
			case DispatchGroup.P:
			case DispatchGroup.SELECT:
			case DispatchGroup.TABLE:
			case DispatchGroup.TR:
				break;
			case DispatchGroup.PLAINTEXT:
				this.endTagExpectationAsArray = Tokenizer.PLAINTEXT_ARR;
				return;
			case DispatchGroup.SCRIPT:
				this.endTagExpectationAsArray = Tokenizer.SCRIPT_ARR;
				return;
			case DispatchGroup.STYLE:
				this.endTagExpectationAsArray = Tokenizer.STYLE_ARR;
				return;
			case DispatchGroup.TEXTAREA:
				this.endTagExpectationAsArray = Tokenizer.TEXTAREA_ARR;
				return;
			case DispatchGroup.TITLE:
				this.endTagExpectationAsArray = Tokenizer.TITLE_ARR;
				return;
			case DispatchGroup.XMP:
				this.endTagExpectationAsArray = Tokenizer.XMP_ARR;
				return;
			default:
				if (group == DispatchGroup.IFRAME)
				{
					this.endTagExpectationAsArray = Tokenizer.IFRAME_ARR;
					return;
				}
				if (group != DispatchGroup.NOEMBED)
				{
					return;
				}
				this.endTagExpectationAsArray = Tokenizer.NOEMBED_ARR;
				return;
			}
		}

		public int LineNumber
		{
			get
			{
				return this.line;
			}
			set
			{
				this.line = value;
			}
		}

		public int ColumnNumber
		{
			get
			{
				return -1;
			}
		}

		public void NotifyAboutMetaBoundary()
		{
			this.metaBoundaryPassed = true;
		}

		internal void TurnOnAdditionalHtml4Errors()
		{
			this.html4 = true;
		}

		internal HtmlAttributes EmptyAttributes()
		{
			if (this.newAttributesEachTime)
			{
				return new HtmlAttributes(this.mappingLangToXmlLang);
			}
			return HtmlAttributes.EMPTY_ATTRIBUTES;
		}

		void ClearStrBufAndAppend(char c)
		{
			this.strBuf[0] = c;
			this.strBufLen = 1;
		}

		void ClearStrBuf()
		{
			this.strBufLen = 0;
		}

		void AppendStrBuf(char c)
		{
			if (this.strBufLen == this.strBuf.Length)
			{
				char[] dst = new char[this.strBuf.Length + 1024];
				Buffer.BlockCopy(this.strBuf, 0, dst, 0, this.strBuf.Length << 1);
				this.strBuf = dst;
			}
			this.strBuf[this.strBufLen++] = c;
		}

		void StrBufToDoctypeName()
		{
			this.doctypeName = Portability.NewLocalNameFromBuffer(this.strBuf, 0, this.strBufLen);
		}

		void EmitStrBuf()
		{
			if (this.strBufLen > 0)
			{
				this.TokenHandler.Characters(this.strBuf, 0, this.strBufLen);
			}
		}

		void ClearLongStrBuf()
		{
			this.longStrBufLen = 0;
		}

		void ClearLongStrBufAndAppend(char c)
		{
			this.longStrBuf[0] = c;
			this.longStrBufLen = 1;
		}

		void AppendLongStrBuf(char c)
		{
			if (this.longStrBufLen == this.longStrBuf.Length)
			{
				char[] dst = new char[this.longStrBufLen + (this.longStrBufLen >> 1)];
				Buffer.BlockCopy(this.longStrBuf, 0, dst, 0, this.longStrBuf.Length << 1);
				this.longStrBuf = dst;
			}
			this.longStrBuf[this.longStrBufLen++] = c;
		}

		void AppendSecondHyphenToBogusComment()
		{
			switch (this.commentPolicy)
			{
			case XmlViolationPolicy.Allow:
				break;
			case XmlViolationPolicy.Fatal:
				this.Fatal("The document is not mappable to XML 1.0 due to two consecutive hyphens in a comment.");
				return;
			case XmlViolationPolicy.AlterInfoset:
				this.AppendLongStrBuf(' ');
				break;
			default:
				return;
			}
			this.Warn("The document is not mappable to XML 1.0 due to two consecutive hyphens in a comment.");
			this.AppendLongStrBuf('-');
		}

		void MaybeAppendSpaceToBogusComment()
		{
			switch (this.commentPolicy)
			{
			case XmlViolationPolicy.Allow:
				break;
			case XmlViolationPolicy.Fatal:
				this.Fatal("The document is not mappable to XML 1.0 due to a trailing hyphen in a comment.");
				return;
			case XmlViolationPolicy.AlterInfoset:
				this.AppendLongStrBuf(' ');
				break;
			default:
				return;
			}
			this.Warn("The document is not mappable to XML 1.0 due to a trailing hyphen in a comment.");
		}

		void AdjustDoubleHyphenAndAppendToLongStrBufAndErr(char c)
		{
			this.ErrConsecutiveHyphens();
			switch (this.commentPolicy)
			{
			case XmlViolationPolicy.Allow:
				break;
			case XmlViolationPolicy.Fatal:
				this.Fatal("The document is not mappable to XML 1.0 due to two consecutive hyphens in a comment.");
				return;
			case XmlViolationPolicy.AlterInfoset:
				this.longStrBufLen--;
				this.AppendLongStrBuf(' ');
				this.AppendLongStrBuf('-');
				break;
			default:
				return;
			}
			this.Warn("The document is not mappable to XML 1.0 due to two consecutive hyphens in a comment.");
			this.AppendLongStrBuf(c);
		}

		void AppendLongStrBuf(char[] buffer, int offset, int length)
		{
			int num = this.longStrBufLen + length;
			if (this.longStrBuf.Length < num)
			{
				char[] dst = new char[num + (num >> 1)];
				Buffer.BlockCopy(this.longStrBuf, 0, dst, 0, this.longStrBuf.Length << 1);
				this.longStrBuf = dst;
			}
			Buffer.BlockCopy(buffer, offset << 1, this.longStrBuf, this.longStrBufLen << 1, length << 1);
			this.longStrBufLen = num;
		}

		void AppendStrBufToLongStrBuf()
		{
			this.AppendLongStrBuf(this.strBuf, 0, this.strBufLen);
		}

		string LongStrBufToString()
		{
			return new string(this.longStrBuf, 0, this.longStrBufLen);
		}

		void EmitComment(int provisionalHyphens, int pos)
		{
			if (this.wantsComments)
			{
				this.TokenHandler.Comment(this.longStrBuf, 0, this.longStrBufLen - provisionalHyphens);
			}
			this.cstart = pos + 1;
		}

		protected void FlushChars(char[] buf, int pos)
		{
			if (pos > this.cstart)
			{
				this.TokenHandler.Characters(buf, this.cstart, pos - this.cstart);
			}
			this.cstart = int.MaxValue;
		}

		public void Fatal(string message)
		{
			throw new Exception(message);
		}

		public void Err(string message)
		{
			if (this.ErrorEvent == null)
			{
				return;
			}
			this.ErrorEvent(this, new ParserErrorEventArgs(message, false));
		}

		public void ErrTreeBuilder(string message)
		{
			this.Err(message);
		}

		public void Warn(string message)
		{
			if (this.ErrorEvent == null)
			{
				return;
			}
			this.ErrorEvent(this, new ParserErrorEventArgs(message, true));
		}

		void ResetAttributes()
		{
			if (this.newAttributesEachTime)
			{
				this.attributes = null;
				return;
			}
			this.attributes.Clear(this.mappingLangToXmlLang);
		}

		void StrBufToElementNameString()
		{
			this.tagName = ElementName.ElementNameByBuffer(this.strBuf, 0, this.strBufLen);
		}

		TokenizerState EmitCurrentTagToken(bool selfClosing, int pos)
		{
			this.cstart = pos + 1;
			this.MaybeErrSlashInEndTag(selfClosing);
			this.stateSave = TokenizerState.DATA;
			HtmlAttributes attrs = this.attributes ?? HtmlAttributes.EMPTY_ATTRIBUTES;
			if (this.endTag)
			{
				this.MaybeErrAttributesOnEndTag(attrs);
				this.TokenHandler.EndTag(this.tagName);
			}
			else
			{
				this.TokenHandler.StartTag(this.tagName, attrs, selfClosing);
			}
			this.tagName = null;
			this.ResetAttributes();
			return this.stateSave;
		}

		void AttributeNameComplete()
		{
			this.attributeName = AttributeName.NameByBuffer(this.strBuf, 0, this.strBufLen, this.namePolicy != XmlViolationPolicy.Allow);
			if (this.attributes == null)
			{
				this.attributes = new HtmlAttributes(this.mappingLangToXmlLang);
			}
			if (this.attributes.Contains(this.attributeName))
			{
				this.ErrDuplicateAttribute();
				this.attributeName = null;
			}
		}

		void AddAttributeWithoutValue()
		{
			this.NoteAttributeWithoutValue();
			if (this.metaBoundaryPassed && AttributeName.CHARSET == this.attributeName && ElementName.META == this.tagName)
			{
				this.Err("A “charset” attribute on a “meta” element found after the first 512 bytes.");
			}
			if (this.attributeName != null)
			{
				if (this.html4)
				{
					if (this.attributeName.IsBoolean)
					{
						if (this.html4ModeCompatibleWithXhtml1Schemata)
						{
							this.attributes.AddAttribute(this.attributeName, this.attributeName.GetLocal(0), this.xmlnsPolicy);
						}
						else
						{
							this.attributes.AddAttribute(this.attributeName, "", this.xmlnsPolicy);
						}
					}
					else if (AttributeName.BORDER != this.attributeName)
					{
						this.Err("Attribute value omitted for a non-bool attribute. (HTML4-only error.)");
						this.attributes.AddAttribute(this.attributeName, "", this.xmlnsPolicy);
					}
				}
				else
				{
					if (AttributeName.SRC == this.attributeName || AttributeName.HREF == this.attributeName)
					{
						this.Warn("Attribute “" + this.attributeName.GetLocal(0) + "” without an explicit value seen. The attribute may be dropped by IE7.");
					}
					this.attributes.AddAttribute(this.attributeName, string.Empty, this.xmlnsPolicy);
				}
				this.attributeName = null;
			}
		}

		void AddAttributeWithValue()
		{
			if (this.metaBoundaryPassed && ElementName.META == this.tagName && AttributeName.CHARSET == this.attributeName)
			{
				this.Err("A “charset” attribute on a “meta” element found after the first 512 bytes.");
			}
			if (this.attributeName != null)
			{
				string str = this.LongStrBufToString();
				if (!this.endTag && this.html4 && this.html4ModeCompatibleWithXhtml1Schemata && this.attributeName.IsCaseFolded)
				{
					str = Tokenizer.NewAsciiLowerCaseStringFromString(str);
				}
				this.attributes.AddAttribute(this.attributeName, str, this.xmlnsPolicy);
				this.attributeName = null;
			}
		}

		static string NewAsciiLowerCaseStringFromString(string str)
		{
			if (str == null)
			{
				return null;
			}
			char[] array = new char[str.Length];
			for (int i = 0; i < str.Length; i++)
			{
				char c = str[i];
				if (c >= 'A' && c <= 'Z')
				{
					c += ' ';
				}
				array[i] = c;
			}
			return new string(array);
		}

		protected void StartErrorReporting()
		{
		}

		public void Start()
		{
			this.InitializeWithoutStarting();
			this.TokenHandler.StartTokenization(this);
			this.StartErrorReporting();
		}

		public bool TokenizeBuffer(UTF16Buffer buffer)
		{
			TokenizerState tokenizerState = this.stateSave;
			TokenizerState returnState = this.returnStateSave;
			char c = '\0';
			this.shouldSuspend = false;
			this.lastCR = false;
			int start = buffer.Start;
			int num = start - 1;
			TokenizerState tokenizerState2 = tokenizerState;
			switch (tokenizerState2)
			{
			case TokenizerState.SCRIPT_DATA:
			case TokenizerState.RAWTEXT:
			case TokenizerState.SCRIPT_DATA_ESCAPED:
			case TokenizerState.PLAINTEXT:
				break;
			case TokenizerState.ATTRIBUTE_VALUE_DOUBLE_QUOTED:
			case TokenizerState.ATTRIBUTE_VALUE_SINGLE_QUOTED:
			case TokenizerState.ATTRIBUTE_VALUE_UNQUOTED:
				goto IL_C0;
			default:
				switch (tokenizerState2)
				{
				case TokenizerState.CDATA_SECTION:
				case TokenizerState.SCRIPT_DATA_ESCAPE_START:
				case TokenizerState.SCRIPT_DATA_ESCAPE_START_DASH:
				case TokenizerState.SCRIPT_DATA_ESCAPED_DASH:
				case TokenizerState.SCRIPT_DATA_ESCAPED_DASH_DASH:
				case TokenizerState.SCRIPT_DATA_DOUBLE_ESCAPE_START:
				case TokenizerState.SCRIPT_DATA_DOUBLE_ESCAPED:
				case TokenizerState.SCRIPT_DATA_DOUBLE_ESCAPED_LESS_THAN_SIGN:
				case TokenizerState.SCRIPT_DATA_DOUBLE_ESCAPED_DASH:
				case TokenizerState.SCRIPT_DATA_DOUBLE_ESCAPED_DASH_DASH:
				case TokenizerState.SCRIPT_DATA_DOUBLE_ESCAPE_END:
					break;
				case TokenizerState.CDATA_RSQB:
				case TokenizerState.CDATA_RSQB_RSQB:
				case TokenizerState.SCRIPT_DATA_LESS_THAN_SIGN:
				case TokenizerState.BOGUS_COMMENT_HYPHEN:
				case TokenizerState.RAWTEXT_RCDATA_LESS_THAN_SIGN:
				case TokenizerState.SCRIPT_DATA_ESCAPED_LESS_THAN_SIGN:
					goto IL_C0;
				default:
					switch (tokenizerState2)
					{
					case TokenizerState.DATA:
					case TokenizerState.RCDATA:
						break;
					default:
						goto IL_C0;
					}
					break;
				}
				break;
			}
			this.cstart = start;
			goto IL_CB;
			IL_C0:
			this.cstart = int.MaxValue;
			IL_CB:
			num = this.StateLoop(tokenizerState, c, num, buffer.Buffer, false, returnState, buffer.End);
			if (num == buffer.End)
			{
				buffer.Start = num;
			}
			else
			{
				buffer.Start = num + 1;
			}
			return this.lastCR;
		}

		int StateLoop(TokenizerState state, char c, int pos, char[] buf, bool reconsume, TokenizerState returnState, int endPos)
		{
			//for (;;)
			//{
			//	IL_00:
			//	char c10;
			//	switch (state)
			//	{
			//	case TokenizerState.SCRIPT_DATA:
			//		for (;;)
			//		{
			//			if (reconsume)
			//			{
			//				reconsume = false;
			//			}
			//			else
			//			{
			//				if (++pos == endPos)
			//				{
			//					goto IL_30EB;
			//				}
			//				c = buf[pos];
			//			}
			//			char c2 = c;
			//			if (c2 <= '\n')
			//			{
			//				if (c2 != '\0')
			//				{
			//					if (c2 == '\n')
			//					{
			//						this.SilentLineFeed();
			//					}
			//				}
			//				else
			//				{
			//					this.EmitReplacementCharacter(buf, pos);
			//				}
			//			}
			//			else
			//			{
			//				if (c2 == '\r')
			//				{
			//					goto IL_1E6D;
			//				}
			//				if (c2 == '<')
			//				{
			//					break;
			//				}
			//			}
			//		}
			//		this.FlushChars(buf, pos);
			//		returnState = state;
			//		state = TokenizerState.SCRIPT_DATA_LESS_THAN_SIGN;
			//		goto IL_1E83;
			//	case TokenizerState.RAWTEXT:
			//		for (;;)
			//		{
			//			if (reconsume)
			//			{
			//				reconsume = false;
			//			}
			//			else
			//			{
			//				if (++pos == endPos)
			//				{
			//					goto IL_30EB;
			//				}
			//				c = buf[pos];
			//			}
			//			char c3 = c;
			//			if (c3 <= '\n')
			//			{
			//				if (c3 != '\0')
			//				{
			//					if (c3 == '\n')
			//					{
			//						this.SilentLineFeed();
			//					}
			//				}
			//				else
			//				{
			//					this.EmitReplacementCharacter(buf, pos);
			//				}
			//			}
			//			else
			//			{
			//				if (c3 == '\r')
			//				{
			//					goto IL_1B24;
			//				}
			//				if (c3 == '<')
			//				{
			//					break;
			//				}
			//			}
			//		}
			//		this.FlushChars(buf, pos);
			//		returnState = state;
			//		state = TokenizerState.RAWTEXT_RCDATA_LESS_THAN_SIGN;
			//		goto IL_1B3A;
			//	case TokenizerState.SCRIPT_DATA_ESCAPED:
			//		goto IL_1FE7;
			//	case TokenizerState.ATTRIBUTE_VALUE_DOUBLE_QUOTED:
			//		goto IL_7AF;
			//	case TokenizerState.ATTRIBUTE_VALUE_SINGLE_QUOTED:
			//		for (;;)
			//		{
			//			if (reconsume)
			//			{
			//				reconsume = false;
			//			}
			//			else
			//			{
			//				if (++pos == endPos)
			//				{
			//					goto IL_30EB;
			//				}
			//				c = buf[pos];
			//			}
			//			char c4 = c;
			//			if (c4 <= '\n')
			//			{
			//				if (c4 != '\0')
			//				{
			//					if (c4 == '\n')
			//					{
			//						this.AppendLongStrBufLineFeed();
			//						continue;
			//					}
			//				}
			//				else
			//				{
			//					c = '\ufffd';
			//				}
			//			}
			//			else
			//			{
			//				if (c4 == '\r')
			//				{
			//					goto IL_114A;
			//				}
			//				switch (c4)
			//				{
			//				case '&':
			//					goto IL_1132;
			//				case '\'':
			//					goto IL_1123;
			//				}
			//			}
			//			this.AppendLongStrBuf(c);
			//		}
			//		IL_1123:
			//		this.AddAttributeWithValue();
			//		state = TokenizerState.AFTER_ATTRIBUTE_VALUE_QUOTED;
			//		continue;
			//		IL_1132:
			//		this.ClearStrBufAndAppend(c);
			//		this.SetAdditionalAndRememberAmpersandLocation('\'');
			//		returnState = state;
			//		state = TokenizerState.CONSUME_CHARACTER_REFERENCE;
			//		goto IL_1173;
			//	case TokenizerState.ATTRIBUTE_VALUE_UNQUOTED:
			//		for (;;)
			//		{
			//			if (reconsume)
			//			{
			//				reconsume = false;
			//			}
			//			else
			//			{
			//				if (++pos == endPos)
			//				{
			//					goto IL_30EB;
			//				}
			//				c = buf[pos];
			//			}
			//			char c5 = c;
			//			if (c5 <= '"')
			//			{
			//				if (c5 == '\0')
			//				{
			//					c = '\ufffd';
			//					goto IL_A32;
			//				}
			//				switch (c5)
			//				{
			//				case '\t':
			//				case '\f':
			//					goto IL_9E1;
			//				case '\n':
			//					goto IL_9DB;
			//				case '\v':
			//					break;
			//				case '\r':
			//					goto IL_9C6;
			//				default:
			//					switch (c5)
			//					{
			//					case ' ':
			//						goto IL_9E1;
			//					case '"':
			//						goto IL_A32;
			//					}
			//					break;
			//				}
			//			}
			//			else
			//			{
			//				switch (c5)
			//				{
			//				case '&':
			//					goto IL_9F0;
			//				case '\'':
			//					goto IL_A32;
			//				default:
			//					switch (c5)
			//					{
			//					case '<':
			//					case '=':
			//						goto IL_A32;
			//					case '>':
			//						goto IL_A0B;
			//					default:
			//						if (c5 == '`')
			//						{
			//							goto IL_A32;
			//						}
			//						break;
			//					}
			//					break;
			//				}
			//			}
			//			IL_A39:
			//			this.ErrHtml4NonNameInUnquotedAttribute(c);
			//			this.AppendLongStrBuf(c);
			//			continue;
			//			IL_A32:
			//			this.ErrUnquotedAttributeValOrNull(c);
			//			goto IL_A39;
			//		}
			//		IL_9E1:
			//		this.AddAttributeWithValue();
			//		state = TokenizerState.BEFORE_ATTRIBUTE_NAME;
			//		continue;
			//		IL_9DB:
			//		this.SilentLineFeed();
			//		goto IL_9E1;
			//		IL_9F0:
			//		this.ClearStrBufAndAppend(c);
			//		this.SetAdditionalAndRememberAmpersandLocation('>');
			//		returnState = state;
			//		state = TokenizerState.CONSUME_CHARACTER_REFERENCE;
			//		continue;
			//		IL_A0B:
			//		this.AddAttributeWithValue();
			//		state = this.EmitCurrentTagToken(false, pos);
			//		if (this.shouldSuspend)
			//		{
			//			goto Block_86;
			//		}
			//		continue;
			//	case TokenizerState.PLAINTEXT:
			//		goto IL_18F8;
			//	case TokenizerState.TAG_OPEN:
			//		goto IL_2B0;
			//	case TokenizerState.CLOSE_TAG_OPEN:
			//	{
			//		if (++pos == endPos)
			//		{
			//			goto IL_30EB;
			//		}
			//		c = buf[pos];
			//		char c6 = c;
			//		if (c6 <= '\n')
			//		{
			//			if (c6 != '\0')
			//			{
			//				if (c6 == '\n')
			//				{
			//					this.SilentLineFeed();
			//					this.ErrGarbageAfterLtSlash();
			//					this.ClearLongStrBufAndAppend('\n');
			//					state = TokenizerState.BOGUS_COMMENT;
			//					continue;
			//				}
			//			}
			//			else
			//			{
			//				c = '\ufffd';
			//			}
			//		}
			//		else
			//		{
			//			if (c6 == '\r')
			//			{
			//				goto IL_199B;
			//			}
			//			if (c6 == '>')
			//			{
			//				this.ErrLtSlashGt();
			//				this.cstart = pos + 1;
			//				state = TokenizerState.DATA;
			//				continue;
			//			}
			//		}
			//		if (c >= 'A' && c <= 'Z')
			//		{
			//			c += ' ';
			//		}
			//		if (c >= 'a' && c <= 'z')
			//		{
			//			this.endTag = true;
			//			this.ClearStrBufAndAppend(c);
			//			state = TokenizerState.TAG_NAME;
			//			continue;
			//		}
			//		this.ErrGarbageAfterLtSlash();
			//		this.ClearLongStrBufAndAppend(c);
			//		state = TokenizerState.BOGUS_COMMENT;
			//		continue;
			//	}
			//	case TokenizerState.TAG_NAME:
			//		break;
			//	case TokenizerState.BEFORE_ATTRIBUTE_NAME:
			//		goto IL_482;
			//	case TokenizerState.ATTRIBUTE_NAME:
			//		goto IL_56C;
			//	case TokenizerState.AFTER_ATTRIBUTE_NAME:
			//		while (++pos != endPos)
			//		{
			//			c = buf[pos];
			//			char c7 = c;
			//			if (c7 <= '"')
			//			{
			//				if (c7 == '\0')
			//				{
			//					c = '\ufffd';
			//					goto IL_B22;
			//				}
			//				switch (c7)
			//				{
			//				case '\t':
			//				case '\f':
			//					continue;
			//				case '\n':
			//					this.SilentLineFeed();
			//					continue;
			//				case '\v':
			//					break;
			//				case '\r':
			//					goto IL_ACD;
			//				default:
			//					switch (c7)
			//					{
			//					case ' ':
			//						continue;
			//					case '"':
			//						goto IL_B22;
			//					}
			//					break;
			//				}
			//			}
			//			else
			//			{
			//				if (c7 == '\'')
			//				{
			//					goto IL_B22;
			//				}
			//				if (c7 == '/')
			//				{
			//					this.AddAttributeWithoutValue();
			//					state = TokenizerState.SELF_CLOSING_START_TAG;
			//					goto IL_00;
			//				}
			//				switch (c7)
			//				{
			//				case '<':
			//					goto IL_B22;
			//				case '=':
			//					state = TokenizerState.BEFORE_ATTRIBUTE_VALUE;
			//					goto IL_00;
			//				case '>':
			//					this.AddAttributeWithoutValue();
			//					state = this.EmitCurrentTagToken(false, pos);
			//					if (this.shouldSuspend)
			//					{
			//						goto Block_95;
			//					}
			//					goto IL_00;
			//				}
			//			}
			//			IL_B29:
			//			this.AddAttributeWithoutValue();
			//			if (c >= 'A' && c <= 'Z')
			//			{
			//				c += ' ';
			//			}
			//			this.ClearStrBufAndAppend(c);
			//			state = TokenizerState.ATTRIBUTE_NAME;
			//			goto IL_00;
			//			IL_B22:
			//			this.ErrQuoteOrLtInAttributeNameOrNull(c);
			//			goto IL_B29;
			//		}
			//		goto IL_30EB;
			//	case TokenizerState.BEFORE_ATTRIBUTE_VALUE:
			//		goto IL_68C;
			//	case TokenizerState.AFTER_ATTRIBUTE_VALUE_QUOTED:
			//		goto IL_842;
			//	case TokenizerState.BOGUS_COMMENT:
			//		for (;;)
			//		{
			//			if (reconsume)
			//			{
			//				reconsume = false;
			//			}
			//			else
			//			{
			//				if (++pos == endPos)
			//				{
			//					goto IL_30EB;
			//				}
			//				c = buf[pos];
			//			}
			//			char c8 = c;
			//			if (c8 <= '\n')
			//			{
			//				if (c8 != '\0')
			//				{
			//					if (c8 == '\n')
			//					{
			//						this.AppendLongStrBufLineFeed();
			//						continue;
			//					}
			//				}
			//				else
			//				{
			//					c = '\ufffd';
			//				}
			//			}
			//			else
			//			{
			//				if (c8 == '\r')
			//				{
			//					goto IL_1D5A;
			//				}
			//				if (c8 == '-')
			//				{
			//					goto IL_1D4D;
			//				}
			//				if (c8 == '>')
			//				{
			//					break;
			//				}
			//			}
			//			this.AppendLongStrBuf(c);
			//		}
			//		this.EmitComment(0, pos);
			//		state = TokenizerState.DATA;
			//		continue;
			//		IL_1D4D:
			//		this.AppendLongStrBuf(c);
			//		state = TokenizerState.BOGUS_COMMENT_HYPHEN;
			//		goto IL_1D80;
			//	case TokenizerState.MARKUP_DECLARATION_OPEN:
			//		if (++pos != endPos)
			//		{
			//			c = buf[pos];
			//			char c9 = c;
			//			if (c9 <= 'D')
			//			{
			//				if (c9 == '-')
			//				{
			//					this.ClearLongStrBufAndAppend(c);
			//					state = TokenizerState.MARKUP_DECLARATION_HYPHEN;
			//					goto IL_BE8;
			//				}
			//				if (c9 != 'D')
			//				{
			//					goto IL_BD0;
			//				}
			//			}
			//			else if (c9 != '[')
			//			{
			//				if (c9 != 'd')
			//				{
			//					goto IL_BD0;
			//				}
			//			}
			//			else
			//			{
			//				if (this.TokenHandler.IsCDataSectionAllowed)
			//				{
			//					this.ClearLongStrBufAndAppend(c);
			//					this.index = 0;
			//					state = TokenizerState.CDATA_START;
			//					continue;
			//				}
			//				goto IL_BD0;
			//			}
			//			this.ClearLongStrBufAndAppend(c);
			//			this.index = 0;
			//			state = TokenizerState.MARKUP_DECLARATION_OCTYPE;
			//			continue;
			//			IL_BD0:
			//			this.ErrBogusComment();
			//			this.ClearLongStrBuf();
			//			state = TokenizerState.BOGUS_COMMENT;
			//			reconsume = true;
			//			continue;
			//		}
			//		goto IL_30EB;
			//	case TokenizerState.DOCTYPE:
			//		goto IL_252A;
			//	case TokenizerState.BEFORE_DOCTYPE_NAME:
			//		goto IL_259D;
			//	case TokenizerState.DOCTYPE_NAME:
			//		goto IL_264B;
			//	case TokenizerState.AFTER_DOCTYPE_NAME:
			//		goto IL_26FE;
			//	case TokenizerState.BEFORE_DOCTYPE_PUBLIC_IDENTIFIER:
			//		goto IL_2901;
			//	case TokenizerState.DOCTYPE_PUBLIC_IDENTIFIER_DOUBLE_QUOTED:
			//		goto IL_29BE;
			//	case TokenizerState.DOCTYPE_PUBLIC_IDENTIFIER_SINGLE_QUOTED:
			//		while (++pos != endPos)
			//		{
			//			c = buf[pos];
			//			c10 = c;
			//			if (c10 <= '\n')
			//			{
			//				if (c10 != '\0')
			//				{
			//					if (c10 == '\n')
			//					{
			//						this.AppendLongStrBufLineFeed();
			//						continue;
			//					}
			//				}
			//				else
			//				{
			//					c = '\ufffd';
			//				}
			//			}
			//			else
			//			{
			//				if (c10 == '\r')
			//				{
			//					goto IL_3075;
			//				}
			//				if (c10 == '\'')
			//				{
			//					this.publicIdentifier = this.LongStrBufToString();
			//					state = TokenizerState.AFTER_DOCTYPE_PUBLIC_IDENTIFIER;
			//					goto IL_00;
			//				}
			//				if (c10 == '>')
			//				{
			//					this.ErrGtInPublicId();
			//					this.forceQuirks = true;
			//					this.publicIdentifier = this.LongStrBufToString();
			//					this.EmitDoctypeToken(pos);
			//					state = TokenizerState.DATA;
			//					goto IL_00;
			//				}
			//			}
			//			this.AppendLongStrBuf(c);
			//		}
			//		goto IL_30EB;
			//	case TokenizerState.AFTER_DOCTYPE_PUBLIC_IDENTIFIER:
			//		goto IL_2A62;
			//	case TokenizerState.BEFORE_DOCTYPE_SYSTEM_IDENTIFIER:
			//		goto IL_2E93;
			//	case TokenizerState.DOCTYPE_SYSTEM_IDENTIFIER_DOUBLE_QUOTED:
			//		goto IL_2BD9;
			//	case TokenizerState.DOCTYPE_SYSTEM_IDENTIFIER_SINGLE_QUOTED:
			//		goto IL_2F50;
			//	case TokenizerState.AFTER_DOCTYPE_SYSTEM_IDENTIFIER:
			//		while (++pos != endPos)
			//		{
			//			c = buf[pos];
			//			c10 = c;
			//			switch (c10)
			//			{
			//			case '\t':
			//			case '\f':
			//				continue;
			//			case '\n':
			//				this.SilentLineFeed();
			//				continue;
			//			case '\v':
			//				break;
			//			case '\r':
			//				goto IL_2CC2;
			//			default:
			//				if (c10 == ' ')
			//				{
			//					continue;
			//				}
			//				if (c10 == '>')
			//				{
			//					this.EmitDoctypeToken(pos);
			//					state = TokenizerState.DATA;
			//					goto IL_00;
			//				}
			//				break;
			//			}
			//			this.BogusDoctypeWithoutQuirks();
			//			state = TokenizerState.BOGUS_DOCTYPE;
			//			goto IL_2CF2;
			//		}
			//		goto IL_30EB;
			//	case TokenizerState.BOGUS_DOCTYPE:
			//		goto IL_2CF2;
			//	case TokenizerState.COMMENT_START:
			//		goto IL_C2A;
			//	case TokenizerState.COMMENT_START_DASH:
			//		if (++pos != endPos)
			//		{
			//			c = buf[pos];
			//			char c11 = c;
			//			if (c11 <= '\n')
			//			{
			//				if (c11 != '\0')
			//				{
			//					if (c11 == '\n')
			//					{
			//						this.AppendLongStrBufLineFeed();
			//						state = TokenizerState.COMMENT;
			//						continue;
			//					}
			//				}
			//				else
			//				{
			//					c = '\ufffd';
			//				}
			//			}
			//			else
			//			{
			//				if (c11 == '\r')
			//				{
			//					goto IL_F33;
			//				}
			//				if (c11 == '-')
			//				{
			//					this.AppendLongStrBuf(c);
			//					state = TokenizerState.COMMENT_END;
			//					continue;
			//				}
			//				if (c11 == '>')
			//				{
			//					this.ErrPrematureEndOfComment();
			//					this.EmitComment(1, pos);
			//					state = TokenizerState.DATA;
			//					continue;
			//				}
			//			}
			//			this.AppendLongStrBuf(c);
			//			state = TokenizerState.COMMENT;
			//			continue;
			//		}
			//		goto IL_30EB;
			//	case TokenizerState.COMMENT:
			//		goto IL_CBE;
			//	case TokenizerState.COMMENT_END_DASH:
			//		goto IL_D23;
			//	case TokenizerState.COMMENT_END:
			//		goto IL_D9A;
			//	case TokenizerState.COMMENT_END_BANG:
			//		while (++pos != endPos)
			//		{
			//			c = buf[pos];
			//			char c12 = c;
			//			if (c12 <= '\n')
			//			{
			//				if (c12 != '\0')
			//				{
			//					if (c12 == '\n')
			//					{
			//						this.AppendLongStrBufLineFeed();
			//						continue;
			//					}
			//				}
			//				else
			//				{
			//					c = '\ufffd';
			//				}
			//			}
			//			else
			//			{
			//				if (c12 == '\r')
			//				{
			//					goto IL_EA2;
			//				}
			//				if (c12 == '-')
			//				{
			//					this.AppendLongStrBuf(c);
			//					state = TokenizerState.COMMENT_END_DASH;
			//					goto IL_00;
			//				}
			//				if (c12 == '>')
			//				{
			//					this.EmitComment(3, pos);
			//					state = TokenizerState.DATA;
			//					goto IL_00;
			//				}
			//			}
			//			this.AppendLongStrBuf(c);
			//			state = TokenizerState.COMMENT;
			//			goto IL_00;
			//		}
			//		goto IL_30EB;
			//	case TokenizerState.NON_DATA_END_TAG_NAME:
			//		goto IL_1B8E;
			//	case TokenizerState.MARKUP_DECLARATION_HYPHEN:
			//		goto IL_BE8;
			//	case TokenizerState.MARKUP_DECLARATION_OCTYPE:
			//		while (++pos != endPos)
			//		{
			//			c = buf[pos];
			//			if (this.index >= 6)
			//			{
			//				state = TokenizerState.DOCTYPE;
			//				reconsume = true;
			//				goto IL_252A;
			//			}
			//			char c13 = c;
			//			if (c >= 'A' && c <= 'Z')
			//			{
			//				c13 += ' ';
			//			}
			//			if (c13 != Tokenizer.OCTYPE[this.index])
			//			{
			//				this.ErrBogusComment();
			//				state = TokenizerState.BOGUS_COMMENT;
			//				reconsume = true;
			//				goto IL_00;
			//			}
			//			this.AppendLongStrBuf(c);
			//			this.index++;
			//		}
			//		goto IL_30EB;
			//	case TokenizerState.DOCTYPE_UBLIC:
			//		goto IL_27BA;
			//	case TokenizerState.DOCTYPE_YSTEM:
			//	{
			//		if (++pos == endPos)
			//		{
			//			goto IL_30EB;
			//		}
			//		c = buf[pos];
			//		if (this.index >= 5)
			//		{
			//			state = TokenizerState.AFTER_DOCTYPE_SYSTEM_KEYWORD;
			//			reconsume = true;
			//			goto IL_2DB6;
			//		}
			//		char c14 = c;
			//		if (c >= 'A' && c <= 'Z')
			//		{
			//			c14 += ' ';
			//		}
			//		if (c14 != Tokenizer.YSTEM[this.index])
			//		{
			//			this.BogusDoctype();
			//			state = TokenizerState.BOGUS_DOCTYPE;
			//			reconsume = true;
			//			continue;
			//		}
			//		this.index++;
			//		continue;
			//	}
			//	case TokenizerState.AFTER_DOCTYPE_PUBLIC_KEYWORD:
			//		goto IL_2824;
			//	case TokenizerState.BETWEEN_DOCTYPE_PUBLIC_AND_SYSTEM_IDENTIFIERS:
			//		goto IL_2B29;
			//	case TokenizerState.AFTER_DOCTYPE_SYSTEM_KEYWORD:
			//		goto IL_2DB6;
			//	case TokenizerState.CONSUME_CHARACTER_REFERENCE:
			//		goto IL_1173;
			//	case TokenizerState.CONSUME_NCR:
			//	{
			//		if (++pos == endPos)
			//		{
			//			goto IL_30EB;
			//		}
			//		c = buf[pos];
			//		this.prevValue = -1;
			//		this.value = 0;
			//		this.seenDigits = false;
			//		char c15 = c;
			//		if (c15 == 'X' || c15 == 'x')
			//		{
			//			this.AppendStrBuf(c);
			//			state = TokenizerState.HEX_NCR_LOOP;
			//			continue;
			//		}
			//		state = TokenizerState.DECIMAL_NRC_LOOP;
			//		reconsume = true;
			//		goto IL_1639;
			//	}
			//	case TokenizerState.CHARACTER_REFERENCE_TAIL:
			//		goto IL_131E;
			//	case TokenizerState.HEX_NCR_LOOP:
			//		while (++pos != endPos)
			//		{
			//			c = buf[pos];
			//			if (this.value < this.prevValue)
			//			{
			//				this.value = 1114112;
			//			}
			//			this.prevValue = this.value;
			//			if (c >= '0' && c <= '9')
			//			{
			//				this.seenDigits = true;
			//				this.value *= 16;
			//				this.value += (int)(c - '0');
			//			}
			//			else if (c >= 'A' && c <= 'F')
			//			{
			//				this.seenDigits = true;
			//				this.value *= 16;
			//				this.value += (int)(c - 'A' + '\n');
			//			}
			//			else if (c >= 'a' && c <= 'f')
			//			{
			//				this.seenDigits = true;
			//				this.value *= 16;
			//				this.value += (int)(c - 'a' + '\n');
			//			}
			//			else if (c == ';')
			//			{
			//				if (this.seenDigits)
			//				{
			//					if ((returnState & (TokenizerState)240) != (TokenizerState)0)
			//					{
			//						this.cstart = pos + 1;
			//					}
			//					state = TokenizerState.HANDLE_NCR_VALUE;
			//					goto IL_00;
			//				}
			//				this.ErrNoDigitsInNCR();
			//				this.AppendStrBuf(';');
			//				this.EmitOrAppendStrBuf(returnState);
			//				if ((returnState & (TokenizerState)240) != (TokenizerState)0)
			//				{
			//					this.cstart = pos + 1;
			//				}
			//				state = returnState;
			//				goto IL_00;
			//			}
			//			else
			//			{
			//				if (!this.seenDigits)
			//				{
			//					this.ErrNoDigitsInNCR();
			//					this.EmitOrAppendStrBuf(returnState);
			//					if ((returnState & (TokenizerState)240) != (TokenizerState)0)
			//					{
			//						this.cstart = pos;
			//					}
			//					state = returnState;
			//					reconsume = true;
			//					goto IL_00;
			//				}
			//				this.ErrCharRefLacksSemicolon();
			//				if ((returnState & (TokenizerState)240) != (TokenizerState)0)
			//				{
			//					this.cstart = pos;
			//				}
			//				state = TokenizerState.HANDLE_NCR_VALUE;
			//				reconsume = true;
			//				goto IL_00;
			//			}
			//		}
			//		goto IL_30EB;
			//	case TokenizerState.DECIMAL_NRC_LOOP:
			//		goto IL_1639;
			//	case TokenizerState.HANDLE_NCR_VALUE:
			//		goto IL_1759;
			//	case TokenizerState.HANDLE_NCR_VALUE_RECONSUME:
			//	case (TokenizerState)75:
			//	case (TokenizerState)76:
			//	case (TokenizerState)77:
			//	case (TokenizerState)78:
			//	case (TokenizerState)79:
			//	case (TokenizerState)80:
			//	case (TokenizerState)81:
			//	case (TokenizerState)82:
			//	case (TokenizerState)83:
			//	case (TokenizerState)84:
			//	case (TokenizerState)85:
			//	case (TokenizerState)86:
			//	case (TokenizerState)87:
			//	case (TokenizerState)88:
			//	case (TokenizerState)89:
			//	case (TokenizerState)90:
			//	case (TokenizerState)91:
			//	case (TokenizerState)92:
			//	case (TokenizerState)93:
			//	case (TokenizerState)94:
			//	case (TokenizerState)95:
			//	case (TokenizerState)96:
			//	case (TokenizerState)97:
			//	case (TokenizerState)98:
			//	case (TokenizerState)99:
			//	case (TokenizerState)100:
			//	case (TokenizerState)101:
			//	case (TokenizerState)102:
			//	case (TokenizerState)103:
			//	case (TokenizerState)104:
			//	case (TokenizerState)105:
			//	case (TokenizerState)106:
			//	case (TokenizerState)107:
			//	case (TokenizerState)108:
			//	case (TokenizerState)109:
			//	case (TokenizerState)110:
			//	case (TokenizerState)111:
			//	case (TokenizerState)112:
			//	case (TokenizerState)113:
			//	case (TokenizerState)114:
			//	case (TokenizerState)115:
			//	case (TokenizerState)116:
			//	case (TokenizerState)117:
			//	case (TokenizerState)118:
			//	case (TokenizerState)119:
			//	case (TokenizerState)120:
			//	case (TokenizerState)121:
			//	case (TokenizerState)122:
			//	case (TokenizerState)123:
			//	case (TokenizerState)124:
			//	case (TokenizerState)125:
			//	case (TokenizerState)126:
			//	case (TokenizerState)127:
			//		continue;
			//	case TokenizerState.CHARACTER_REFERENCE_HILO_LOOKUP:
			//		goto IL_1285;
			//	case TokenizerState.SELF_CLOSING_START_TAG:
			//		goto IL_8E2;
			//	case TokenizerState.CDATA_START:
			//		while (++pos != endPos)
			//		{
			//			c = buf[pos];
			//			if (this.index >= 6)
			//			{
			//				this.cstart = pos;
			//				state = TokenizerState.CDATA_SECTION;
			//				reconsume = true;
			//				goto IL_FCC;
			//			}
			//			if (c != Tokenizer.CDATA_LSQB[this.index])
			//			{
			//				this.ErrBogusComment();
			//				state = TokenizerState.BOGUS_COMMENT;
			//				reconsume = true;
			//				goto IL_00;
			//			}
			//			this.AppendLongStrBuf(c);
			//			this.index++;
			//		}
			//		goto IL_30EB;
			//	case TokenizerState.CDATA_SECTION:
			//		goto IL_FCC;
			//	case TokenizerState.CDATA_RSQB:
			//		goto IL_103A;
			//	case TokenizerState.CDATA_RSQB_RSQB:
			//		goto IL_1081;
			//	case TokenizerState.SCRIPT_DATA_LESS_THAN_SIGN:
			//		goto IL_1E83;
			//	case TokenizerState.SCRIPT_DATA_ESCAPE_START:
			//		goto IL_1EFE;
			//	case TokenizerState.SCRIPT_DATA_ESCAPE_START_DASH:
			//		goto IL_1F2B;
			//	case TokenizerState.SCRIPT_DATA_ESCAPED_DASH:
			//		goto IL_2066;
			//	case TokenizerState.SCRIPT_DATA_ESCAPED_DASH_DASH:
			//		goto IL_1F58;
			//	case TokenizerState.BOGUS_COMMENT_HYPHEN:
			//		goto IL_1D80;
			//	case TokenizerState.RAWTEXT_RCDATA_LESS_THAN_SIGN:
			//		goto IL_1B3A;
			//	case TokenizerState.SCRIPT_DATA_ESCAPED_LESS_THAN_SIGN:
			//		goto IL_20EB;
			//	case TokenizerState.SCRIPT_DATA_DOUBLE_ESCAPE_START:
			//		goto IL_2178;
			//	case TokenizerState.SCRIPT_DATA_DOUBLE_ESCAPED:
			//		goto IL_223A;
			//	case TokenizerState.SCRIPT_DATA_DOUBLE_ESCAPED_LESS_THAN_SIGN:
			//		goto IL_23BD;
			//	case TokenizerState.SCRIPT_DATA_DOUBLE_ESCAPED_DASH:
			//		goto IL_22B0;
			//	case TokenizerState.SCRIPT_DATA_DOUBLE_ESCAPED_DASH_DASH:
			//		goto IL_232F;
			//	case TokenizerState.SCRIPT_DATA_DOUBLE_ESCAPE_END:
			//		goto IL_23F2;
			//	case TokenizerState.PROCESSING_INSTRUCTION:
			//		while (++pos != endPos)
			//		{
			//			c = buf[pos];
			//			c10 = c;
			//			if (c10 == '?')
			//			{
			//				state = TokenizerState.PROCESSING_INSTRUCTION_QUESTION_MARK;
			//			}
			//		}
			//		continue;
			//	case TokenizerState.PROCESSING_INSTRUCTION_QUESTION_MARK:
			//		if (++pos == endPos)
			//		{
			//			goto IL_30EB;
			//		}
			//		c = buf[pos];
			//		c10 = c;
			//		if (c10 == '>')
			//		{
			//			state = TokenizerState.DATA;
			//			continue;
			//		}
			//		state = TokenizerState.PROCESSING_INSTRUCTION;
			//		continue;
			//	case TokenizerState.DATA:
			//		for (;;)
			//		{
			//			if (reconsume)
			//			{
			//				reconsume = false;
			//			}
			//			else
			//			{
			//				if (++pos == endPos)
			//				{
			//					goto IL_30EB;
			//				}
			//				c = buf[pos];
			//			}
			//			c10 = c;
			//			if (c10 <= '\n')
			//			{
			//				if (c10 != '\0')
			//				{
			//					if (c10 == '\n')
			//					{
			//						this.SilentLineFeed();
			//					}
			//				}
			//				else
			//				{
			//					this.EmitReplacementCharacter(buf, pos);
			//				}
			//			}
			//			else
			//			{
			//				if (c10 == '\r')
			//				{
			//					goto IL_297;
			//				}
			//				if (c10 == '&')
			//				{
			//					break;
			//				}
			//				if (c10 == '<')
			//				{
			//					goto IL_27A;
			//				}
			//			}
			//		}
			//		this.FlushChars(buf, pos);
			//		this.ClearStrBufAndAppend(c);
			//		this.SetAdditionalAndRememberAmpersandLocation('\0');
			//		returnState = state;
			//		state = TokenizerState.CONSUME_CHARACTER_REFERENCE;
			//		continue;
			//		IL_27A:
			//		this.FlushChars(buf, pos);
			//		state = TokenizerState.TAG_OPEN;
			//		goto IL_2B0;
			//	case TokenizerState.RCDATA:
			//		for (;;)
			//		{
			//			if (reconsume)
			//			{
			//				reconsume = false;
			//			}
			//			else
			//			{
			//				if (++pos == endPos)
			//				{
			//					goto IL_30EB;
			//				}
			//				c = buf[pos];
			//			}
			//			char c16 = c;
			//			if (c16 <= '\n')
			//			{
			//				if (c16 != '\0')
			//				{
			//					if (c16 == '\n')
			//					{
			//						this.SilentLineFeed();
			//					}
			//				}
			//				else
			//				{
			//					this.EmitReplacementCharacter(buf, pos);
			//				}
			//			}
			//			else
			//			{
			//				if (c16 == '\r')
			//				{
			//					goto IL_1AB0;
			//				}
			//				if (c16 == '&')
			//				{
			//					break;
			//				}
			//				if (c16 == '<')
			//				{
			//					goto IL_1A8D;
			//				}
			//			}
			//		}
			//		this.FlushChars(buf, pos);
			//		this.ClearStrBufAndAppend(c);
			//		this.additional = '\0';
			//		returnState = state;
			//		state = TokenizerState.CONSUME_CHARACTER_REFERENCE;
			//		continue;
			//		IL_1A8D:
			//		this.FlushChars(buf, pos);
			//		returnState = state;
			//		state = TokenizerState.RAWTEXT_RCDATA_LESS_THAN_SIGN;
			//		continue;
			//	default:
			//		continue;
			//	}
			//	IL_3B0:
			//	while (++pos != endPos)
			//	{
			//		c = buf[pos];
			//		char c17 = c;
			//		if (c17 <= '\r')
			//		{
			//			if (c17 != '\0')
			//			{
			//				switch (c17)
			//				{
			//				case '\t':
			//				case '\f':
			//					goto IL_423;
			//				case '\n':
			//					this.SilentLineFeed();
			//					goto IL_423;
			//				case '\r':
			//					goto IL_408;
			//				}
			//			}
			//			else
			//			{
			//				c = '\ufffd';
			//			}
			//		}
			//		else
			//		{
			//			if (c17 == ' ')
			//			{
			//				goto IL_423;
			//			}
			//			if (c17 == '/')
			//			{
			//				this.StrBufToElementNameString();
			//				state = TokenizerState.SELF_CLOSING_START_TAG;
			//				goto IL_00;
			//			}
			//			if (c17 == '>')
			//			{
			//				this.StrBufToElementNameString();
			//				state = this.EmitCurrentTagToken(false, pos);
			//				if (this.shouldSuspend)
			//				{
			//					break;
			//				}
			//				goto IL_00;
			//			}
			//		}
			//		if (c >= 'A' && c <= 'Z')
			//		{
			//			c += ' ';
			//		}
			//		this.AppendStrBuf(c);
			//		continue;
			//		IL_423:
			//		this.StrBufToElementNameString();
			//		state = TokenizerState.BEFORE_ATTRIBUTE_NAME;
			//		goto IL_482;
			//	}
			//	goto IL_30EB;
			//	char c18;
			//	for (;;)
			//	{
			//		IL_482:
			//		if (reconsume)
			//		{
			//			reconsume = false;
			//		}
			//		else
			//		{
			//			if (++pos == endPos)
			//			{
			//				goto IL_30EB;
			//			}
			//			c = buf[pos];
			//		}
			//		c18 = c;
			//		if (c18 > '"')
			//		{
			//			break;
			//		}
			//		if (c18 == '\0')
			//		{
			//			goto IL_542;
			//		}
			//		switch (c18)
			//		{
			//		case '\t':
			//		case '\f':
			//			break;
			//		case '\n':
			//			this.SilentLineFeed();
			//			break;
			//		case '\v':
			//			goto IL_550;
			//		case '\r':
			//			goto IL_509;
			//		default:
			//			switch (c18)
			//			{
			//			case ' ':
			//				continue;
			//			case '"':
			//				goto IL_549;
			//			}
			//			goto Block_32;
			//		}
			//	}
			//	if (c18 == '\'')
			//	{
			//		goto IL_549;
			//	}
			//	if (c18 == '/')
			//	{
			//		state = TokenizerState.SELF_CLOSING_START_TAG;
			//		continue;
			//	}
			//	switch (c18)
			//	{
			//	case '<':
			//	case '=':
			//		goto IL_549;
			//	case '>':
			//		state = this.EmitCurrentTagToken(false, pos);
			//		if (this.shouldSuspend)
			//		{
			//			goto Block_36;
			//		}
			//		continue;
			//	}
			//	IL_550:
			//	if (c >= 'A' && c <= 'Z')
			//	{
			//		c += ' ';
			//	}
			//	this.ClearStrBufAndAppend(c);
			//	state = TokenizerState.ATTRIBUTE_NAME;
			//	goto IL_56C;
			//	IL_549:
			//	this.ErrBadCharBeforeAttributeNameOrNull(c);
			//	goto IL_550;
			//	IL_542:
			//	c = '\ufffd';
			//	goto IL_549;
			//	Block_32:
			//	goto IL_550;
			//	IL_56C:
			//	while (++pos != endPos)
			//	{
			//		c = buf[pos];
			//		char c19 = c;
			//		if (c19 <= '"')
			//		{
			//			if (c19 != '\0')
			//			{
			//				switch (c19)
			//				{
			//				case '\t':
			//				case '\f':
			//					break;
			//				case '\n':
			//					this.SilentLineFeed();
			//					break;
			//				case '\v':
			//					goto IL_66F;
			//				case '\r':
			//					goto IL_5F0;
			//				default:
			//					switch (c19)
			//					{
			//					case ' ':
			//						break;
			//					case '!':
			//						goto IL_66F;
			//					case '"':
			//						goto IL_668;
			//					default:
			//						goto IL_66F;
			//					}
			//					break;
			//				}
			//				this.AttributeNameComplete();
			//				state = TokenizerState.AFTER_ATTRIBUTE_NAME;
			//				goto IL_00;
			//			}
			//			c = '\ufffd';
			//			goto IL_668;
			//		}
			//		else
			//		{
			//			if (c19 == '\'')
			//			{
			//				goto IL_668;
			//			}
			//			if (c19 == '/')
			//			{
			//				this.AttributeNameComplete();
			//				this.AddAttributeWithoutValue();
			//				state = TokenizerState.SELF_CLOSING_START_TAG;
			//				goto IL_00;
			//			}
			//			switch (c19)
			//			{
			//			case '<':
			//				goto IL_668;
			//			case '=':
			//				this.AttributeNameComplete();
			//				state = TokenizerState.BEFORE_ATTRIBUTE_VALUE;
			//				goto IL_68C;
			//			case '>':
			//				this.AttributeNameComplete();
			//				this.AddAttributeWithoutValue();
			//				state = this.EmitCurrentTagToken(false, pos);
			//				if (this.shouldSuspend)
			//				{
			//					goto Block_47;
			//				}
			//				goto IL_00;
			//			}
			//		}
			//		IL_66F:
			//		if (c >= 'A' && c <= 'Z')
			//		{
			//			c += ' ';
			//		}
			//		this.AppendStrBuf(c);
			//		continue;
			//		IL_668:
			//		this.ErrQuoteOrLtInAttributeNameOrNull(c);
			//		goto IL_66F;
			//	}
			//	goto IL_30EB;
			//	IL_68C:
			//	while (++pos != endPos)
			//	{
			//		c = buf[pos];
			//		char c20 = c;
			//		if (c20 <= '"')
			//		{
			//			if (c20 == '\0')
			//			{
			//				c = '\ufffd';
			//				goto IL_78C;
			//			}
			//			switch (c20)
			//			{
			//			case '\t':
			//			case '\f':
			//				continue;
			//			case '\n':
			//				this.SilentLineFeed();
			//				continue;
			//			case '\v':
			//				break;
			//			case '\r':
			//				goto IL_719;
			//			default:
			//				switch (c20)
			//				{
			//				case ' ':
			//					continue;
			//				case '"':
			//					this.ClearLongStrBuf();
			//					state = TokenizerState.ATTRIBUTE_VALUE_DOUBLE_QUOTED;
			//					goto IL_7AF;
			//				}
			//				break;
			//			}
			//		}
			//		else
			//		{
			//			switch (c20)
			//			{
			//			case '&':
			//				this.ClearLongStrBuf();
			//				state = TokenizerState.ATTRIBUTE_VALUE_UNQUOTED;
			//				this.NoteUnquotedAttributeValue();
			//				reconsume = true;
			//				goto IL_00;
			//			case '\'':
			//				this.ClearLongStrBuf();
			//				state = TokenizerState.ATTRIBUTE_VALUE_SINGLE_QUOTED;
			//				goto IL_00;
			//			default:
			//				switch (c20)
			//				{
			//				case '<':
			//				case '=':
			//					goto IL_78C;
			//				case '>':
			//					this.ErrAttributeValueMissing();
			//					this.AddAttributeWithoutValue();
			//					state = this.EmitCurrentTagToken(false, pos);
			//					if (this.shouldSuspend)
			//					{
			//						goto Block_58;
			//					}
			//					goto IL_00;
			//				default:
			//					if (c20 == '`')
			//					{
			//						goto IL_78C;
			//					}
			//					break;
			//				}
			//				break;
			//			}
			//		}
			//		IL_793:
			//		this.ErrHtml4NonNameInUnquotedAttribute(c);
			//		this.ClearLongStrBufAndAppend(c);
			//		state = TokenizerState.ATTRIBUTE_VALUE_UNQUOTED;
			//		this.NoteUnquotedAttributeValue();
			//		goto IL_00;
			//		IL_78C:
			//		this.ErrLtOrEqualsOrGraveInUnquotedAttributeOrNull(c);
			//		goto IL_793;
			//	}
			//	goto IL_30EB;
			//	for (;;)
			//	{
			//		IL_7AF:
			//		if (reconsume)
			//		{
			//			reconsume = false;
			//		}
			//		else
			//		{
			//			if (++pos == endPos)
			//			{
			//				goto IL_30EB;
			//			}
			//			c = buf[pos];
			//		}
			//		char c21 = c;
			//		if (c21 <= '\n')
			//		{
			//			if (c21 != '\0')
			//			{
			//				if (c21 == '\n')
			//				{
			//					this.AppendLongStrBufLineFeed();
			//					continue;
			//				}
			//			}
			//			else
			//			{
			//				c = '\ufffd';
			//			}
			//		}
			//		else
			//		{
			//			if (c21 == '\r')
			//			{
			//				goto IL_81C;
			//			}
			//			if (c21 == '"')
			//			{
			//				goto IL_7F5;
			//			}
			//			if (c21 == '&')
			//			{
			//				break;
			//			}
			//		}
			//		this.AppendLongStrBuf(c);
			//	}
			//	this.ClearStrBufAndAppend(c);
			//	this.SetAdditionalAndRememberAmpersandLocation('"');
			//	returnState = state;
			//	state = TokenizerState.CONSUME_CHARACTER_REFERENCE;
			//	continue;
			//	IL_7F5:
			//	this.AddAttributeWithValue();
			//	state = TokenizerState.AFTER_ATTRIBUTE_VALUE_QUOTED;
			//	goto IL_842;
			//	IL_CBE:
			//	while (++pos != endPos)
			//	{
			//		c = buf[pos];
			//		char c22 = c;
			//		if (c22 <= '\n')
			//		{
			//			if (c22 != '\0')
			//			{
			//				if (c22 == '\n')
			//				{
			//					this.AppendLongStrBufLineFeed();
			//					continue;
			//				}
			//			}
			//			else
			//			{
			//				c = '\ufffd';
			//			}
			//		}
			//		else
			//		{
			//			if (c22 == '\r')
			//			{
			//				goto IL_D00;
			//			}
			//			if (c22 == '-')
			//			{
			//				this.AppendLongStrBuf(c);
			//				state = TokenizerState.COMMENT_END_DASH;
			//				goto IL_D23;
			//			}
			//		}
			//		this.AppendLongStrBuf(c);
			//	}
			//	goto IL_30EB;
			//	IL_D9A:
			//	while (++pos != endPos)
			//	{
			//		c = buf[pos];
			//		char c23 = c;
			//		if (c23 <= '\r')
			//		{
			//			if (c23 != '\0')
			//			{
			//				if (c23 == '\n')
			//				{
			//					this.AdjustDoubleHyphenAndAppendToLongStrBufLineFeed();
			//					state = TokenizerState.COMMENT;
			//					goto IL_00;
			//				}
			//				if (c23 == '\r')
			//				{
			//					goto IL_DF8;
			//				}
			//			}
			//			else
			//			{
			//				c = '\ufffd';
			//			}
			//		}
			//		else
			//		{
			//			if (c23 == '!')
			//			{
			//				this.ErrHyphenHyphenBang();
			//				this.AppendLongStrBuf(c);
			//				state = TokenizerState.COMMENT_END_BANG;
			//				goto IL_00;
			//			}
			//			if (c23 == '-')
			//			{
			//				this.AdjustDoubleHyphenAndAppendToLongStrBufAndErr(c);
			//				continue;
			//			}
			//			if (c23 == '>')
			//			{
			//				this.EmitComment(2, pos);
			//				state = TokenizerState.DATA;
			//				goto IL_00;
			//			}
			//		}
			//		this.AdjustDoubleHyphenAndAppendToLongStrBufAndErr(c);
			//		state = TokenizerState.COMMENT;
			//		goto IL_00;
			//	}
			//	goto IL_30EB;
			//	for (;;)
			//	{
			//		IL_FCC:
			//		if (reconsume)
			//		{
			//			reconsume = false;
			//		}
			//		else
			//		{
			//			if (++pos == endPos)
			//			{
			//				goto IL_30EB;
			//			}
			//			c = buf[pos];
			//		}
			//		char c24 = c;
			//		if (c24 <= '\n')
			//		{
			//			if (c24 != '\0')
			//			{
			//				if (c24 == '\n')
			//				{
			//					this.SilentLineFeed();
			//				}
			//			}
			//			else
			//			{
			//				this.EmitReplacementCharacter(buf, pos);
			//			}
			//		}
			//		else
			//		{
			//			if (c24 == '\r')
			//			{
			//				goto IL_1024;
			//			}
			//			if (c24 == ']')
			//			{
			//				break;
			//			}
			//		}
			//	}
			//	this.FlushChars(buf, pos);
			//	state = TokenizerState.CDATA_RSQB;
			//	goto IL_103A;
			//	IL_131E:
			//	while (++pos != endPos)
			//	{
			//		c = buf[pos];
			//		if (c == '\0')
			//		{
			//			break;
			//		}
			//		this.entCol++;
			//		while (this.hi >= this.lo)
			//		{
			//			if (this.entCol == NamedCharacters.NAMES[this.lo].Length)
			//			{
			//				this.candidate = this.lo;
			//				this.strBufMark = this.strBufLen;
			//				this.lo++;
			//			}
			//			else
			//			{
			//				if (this.entCol > NamedCharacters.NAMES[this.lo].Length)
			//				{
			//					break;
			//				}
			//				if (c <= NamedCharacters.NAMES[this.lo][this.entCol])
			//				{
			//					while (this.hi >= this.lo)
			//					{
			//						if (this.entCol != NamedCharacters.NAMES[this.hi].Length)
			//						{
			//							if (this.entCol > NamedCharacters.NAMES[this.hi].Length)
			//							{
			//								break;
			//							}
			//							if (c < NamedCharacters.NAMES[this.hi][this.entCol])
			//							{
			//								this.hi--;
			//								continue;
			//							}
			//						}
			//						if (this.hi >= this.lo)
			//						{
			//							this.AppendStrBuf(c);
			//							goto IL_131E;
			//						}
			//						break;
			//					}
			//					break;
			//				}
			//				this.lo++;
			//			}
			//		}
			//		if (this.candidate == -1)
			//		{
			//			this.ErrNoNamedCharacterMatch();
			//			this.EmitOrAppendStrBuf(returnState);
			//			if ((returnState & (TokenizerState)240) != (TokenizerState)0)
			//			{
			//				this.cstart = pos;
			//			}
			//			state = returnState;
			//			reconsume = true;
			//			goto IL_00;
			//		}
			//		string text = NamedCharacters.NAMES[this.candidate];
			//		if (text.Length == 0 || text[text.Length - 1] != ';')
			//		{
			//			if ((returnState & (TokenizerState)240) == (TokenizerState)0)
			//			{
			//				char c25;
			//				if (this.strBufMark == this.strBufLen)
			//				{
			//					c25 = c;
			//				}
			//				else
			//				{
			//					c25 = this.strBuf[this.strBufMark];
			//				}
			//				if (c25 == '=' || (c25 >= '0' && c25 <= '9') || (c25 >= 'A' && c25 <= 'Z') || (c25 >= 'a' && c25 <= 'z'))
			//				{
			//					this.ErrNoNamedCharacterMatch();
			//					this.AppendStrBufToLongStrBuf();
			//					state = returnState;
			//					reconsume = true;
			//					goto IL_00;
			//				}
			//			}
			//			if ((returnState & (TokenizerState)240) == (TokenizerState)0)
			//			{
			//				this.ErrUnescapedAmpersandInterpretedAsCharacterReference();
			//			}
			//			else
			//			{
			//				this.ErrNotSemicolonTerminated();
			//			}
			//		}
			//		char[] array = NamedCharacters.VALUES[this.candidate];
			//		if (array.Length == 1)
			//		{
			//			this.EmitOrAppendOne(array, returnState);
			//		}
			//		else
			//		{
			//			this.EmitOrAppendTwo(array, returnState);
			//		}
			//		if (this.strBufMark < this.strBufLen)
			//		{
			//			if ((returnState & (TokenizerState)240) == (TokenizerState)0)
			//			{
			//				for (int i = this.strBufMark; i < this.strBufLen; i++)
			//				{
			//					this.AppendLongStrBuf(this.strBuf[i]);
			//				}
			//			}
			//			else
			//			{
			//				this.TokenHandler.Characters(this.strBuf, this.strBufMark, this.strBufLen - this.strBufMark);
			//			}
			//		}
			//		if ((returnState & (TokenizerState)240) != (TokenizerState)0)
			//		{
			//			this.cstart = pos;
			//		}
			//		state = returnState;
			//		reconsume = true;
			//		goto IL_00;
			//	}
			//	goto IL_30EB;
			//	for (;;)
			//	{
			//		IL_1639:
			//		if (reconsume)
			//		{
			//			reconsume = false;
			//		}
			//		else
			//		{
			//			if (++pos == endPos)
			//			{
			//				goto IL_30EB;
			//			}
			//			c = buf[pos];
			//		}
			//		if (this.value < this.prevValue)
			//		{
			//			this.value = 1114112;
			//		}
			//		this.prevValue = this.value;
			//		if (c < '0' || c > '9')
			//		{
			//			break;
			//		}
			//		this.seenDigits = true;
			//		this.value *= 10;
			//		this.value += (int)(c - '0');
			//	}
			//	if (c == ';')
			//	{
			//		if (this.seenDigits)
			//		{
			//			if ((returnState & (TokenizerState)240) != (TokenizerState)0)
			//			{
			//				this.cstart = pos + 1;
			//			}
			//			state = TokenizerState.HANDLE_NCR_VALUE;
			//			goto IL_1759;
			//		}
			//		this.ErrNoDigitsInNCR();
			//		this.AppendStrBuf(';');
			//		this.EmitOrAppendStrBuf(returnState);
			//		if ((returnState & (TokenizerState)240) != (TokenizerState)0)
			//		{
			//			this.cstart = pos + 1;
			//		}
			//		state = returnState;
			//		continue;
			//	}
			//	else
			//	{
			//		if (!this.seenDigits)
			//		{
			//			this.ErrNoDigitsInNCR();
			//			this.EmitOrAppendStrBuf(returnState);
			//			if ((returnState & (TokenizerState)240) != (TokenizerState)0)
			//			{
			//				this.cstart = pos;
			//			}
			//			state = returnState;
			//			reconsume = true;
			//			continue;
			//		}
			//		this.ErrCharRefLacksSemicolon();
			//		if ((returnState & (TokenizerState)240) != (TokenizerState)0)
			//		{
			//			this.cstart = pos;
			//		}
			//		state = TokenizerState.HANDLE_NCR_VALUE;
			//		reconsume = true;
			//		goto IL_1759;
			//	}
			//	IL_1B8E:
			//	while (++pos != endPos)
			//	{
			//		c = buf[pos];
			//		if (this.index >= this.endTagExpectationAsArray.Length)
			//		{
			//			this.endTag = true;
			//			this.tagName = this.endTagExpectation;
			//			char c26 = c;
			//			if (c26 <= ' ')
			//			{
			//				switch (c26)
			//				{
			//				case '\t':
			//				case '\f':
			//					break;
			//				case '\n':
			//					this.SilentLineFeed();
			//					break;
			//				case '\v':
			//					goto IL_1CB9;
			//				case '\r':
			//					goto IL_1C78;
			//				default:
			//					if (c26 != ' ')
			//					{
			//						goto IL_1CB9;
			//					}
			//					break;
			//				}
			//				state = TokenizerState.BEFORE_ATTRIBUTE_NAME;
			//				goto IL_00;
			//			}
			//			if (c26 == '/')
			//			{
			//				state = TokenizerState.SELF_CLOSING_START_TAG;
			//				goto IL_00;
			//			}
			//			if (c26 == '>')
			//			{
			//				state = this.EmitCurrentTagToken(false, pos);
			//				if (this.shouldSuspend)
			//				{
			//					break;
			//				}
			//				goto IL_00;
			//			}
			//			IL_1CB9:
			//			this.ErrWarnLtSlashInRcdata();
			//			this.TokenHandler.Characters(Tokenizer.LT_SOLIDUS, 0, 2);
			//			this.EmitStrBuf();
			//			if (c == '\0')
			//			{
			//				this.EmitReplacementCharacter(buf, pos);
			//			}
			//			else
			//			{
			//				this.cstart = pos;
			//			}
			//			state = returnState;
			//			goto IL_00;
			//		}
			//		char c27 = this.endTagExpectationAsArray[this.index];
			//		char c28 = c;
			//		if (c >= 'A' && c <= 'Z')
			//		{
			//			c28 += ' ';
			//		}
			//		if (c28 != c27)
			//		{
			//			this.ErrHtml4LtSlashInRcdata(c28);
			//			this.TokenHandler.Characters(Tokenizer.LT_SOLIDUS, 0, 2);
			//			this.EmitStrBuf();
			//			this.cstart = pos;
			//			state = returnState;
			//			reconsume = true;
			//			goto IL_00;
			//		}
			//		this.AppendStrBuf(c);
			//		this.index++;
			//	}
			//	goto IL_30EB;
			//	IL_1D80:
			//	while (++pos != endPos)
			//	{
			//		c = buf[pos];
			//		char c29 = c;
			//		if (c29 <= '\n')
			//		{
			//			if (c29 != '\0')
			//			{
			//				if (c29 == '\n')
			//				{
			//					this.AppendLongStrBufLineFeed();
			//					state = TokenizerState.BOGUS_COMMENT;
			//					goto IL_00;
			//				}
			//			}
			//			else
			//			{
			//				c = '\ufffd';
			//			}
			//		}
			//		else
			//		{
			//			if (c29 == '\r')
			//			{
			//				goto IL_1DDD;
			//			}
			//			if (c29 == '-')
			//			{
			//				this.AppendSecondHyphenToBogusComment();
			//				continue;
			//			}
			//			if (c29 == '>')
			//			{
			//				this.MaybeAppendSpaceToBogusComment();
			//				this.EmitComment(0, pos);
			//				state = TokenizerState.DATA;
			//				goto IL_00;
			//			}
			//		}
			//		this.AppendLongStrBuf(c);
			//		state = TokenizerState.BOGUS_COMMENT;
			//		goto IL_00;
			//	}
			//	goto IL_30EB;
			//	IL_1F58:
			//	while (++pos != endPos)
			//	{
			//		c = buf[pos];
			//		char c30 = c;
			//		if (c30 <= '\n')
			//		{
			//			if (c30 == '\0')
			//			{
			//				this.EmitReplacementCharacter(buf, pos);
			//				state = TokenizerState.SCRIPT_DATA_ESCAPED;
			//				goto IL_1FE7;
			//			}
			//			if (c30 == '\n')
			//			{
			//				this.SilentLineFeed();
			//			}
			//		}
			//		else
			//		{
			//			if (c30 == '\r')
			//			{
			//				goto IL_1FCD;
			//			}
			//			if (c30 == '-')
			//			{
			//				continue;
			//			}
			//			switch (c30)
			//			{
			//			case '<':
			//				this.FlushChars(buf, pos);
			//				state = TokenizerState.SCRIPT_DATA_ESCAPED_LESS_THAN_SIGN;
			//				goto IL_00;
			//			case '>':
			//				state = TokenizerState.SCRIPT_DATA;
			//				goto IL_00;
			//			}
			//		}
			//		state = TokenizerState.SCRIPT_DATA_ESCAPED;
			//		goto IL_1FE7;
			//	}
			//	goto IL_30EB;
			//	for (;;)
			//	{
			//		IL_1FE7:
			//		if (reconsume)
			//		{
			//			reconsume = false;
			//		}
			//		else
			//		{
			//			if (++pos == endPos)
			//			{
			//				goto IL_30EB;
			//			}
			//			c = buf[pos];
			//		}
			//		char c31 = c;
			//		if (c31 <= '\n')
			//		{
			//			if (c31 != '\0')
			//			{
			//				if (c31 == '\n')
			//				{
			//					this.SilentLineFeed();
			//				}
			//			}
			//			else
			//			{
			//				this.EmitReplacementCharacter(buf, pos);
			//			}
			//		}
			//		else
			//		{
			//			if (c31 == '\r')
			//			{
			//				goto IL_2050;
			//			}
			//			if (c31 == '-')
			//			{
			//				goto IL_202D;
			//			}
			//			if (c31 == '<')
			//			{
			//				break;
			//			}
			//		}
			//	}
			//	this.FlushChars(buf, pos);
			//	state = TokenizerState.SCRIPT_DATA_ESCAPED_LESS_THAN_SIGN;
			//	continue;
			//	IL_202D:
			//	state = TokenizerState.SCRIPT_DATA_ESCAPED_DASH;
			//	goto IL_2066;
			//	IL_2178:
			//	while (++pos != endPos)
			//	{
			//		c = buf[pos];
			//		if (this.index >= 6)
			//		{
			//			char c32 = c;
			//			if (c32 <= ' ')
			//			{
			//				switch (c32)
			//				{
			//				case '\t':
			//				case '\f':
			//					goto IL_2229;
			//				case '\n':
			//					this.SilentLineFeed();
			//					goto IL_2229;
			//				case '\v':
			//					break;
			//				case '\r':
			//					goto IL_2211;
			//				default:
			//					if (c32 == ' ')
			//					{
			//						goto IL_2229;
			//					}
			//					break;
			//				}
			//			}
			//			else if (c32 == '/' || c32 == '>')
			//			{
			//				goto IL_2229;
			//			}
			//			reconsume = true;
			//			state = TokenizerState.SCRIPT_DATA_ESCAPED;
			//			goto IL_00;
			//			IL_2229:
			//			state = TokenizerState.SCRIPT_DATA_DOUBLE_ESCAPED;
			//			goto IL_223A;
			//		}
			//		char c33 = c;
			//		if (c >= 'A' && c <= 'Z')
			//		{
			//			c33 += ' ';
			//		}
			//		if (c33 != Tokenizer.SCRIPT_ARR[this.index])
			//		{
			//			reconsume = true;
			//			state = TokenizerState.SCRIPT_DATA_ESCAPED;
			//			goto IL_00;
			//		}
			//		this.index++;
			//	}
			//	goto IL_30EB;
			//	for (;;)
			//	{
			//		IL_223A:
			//		if (reconsume)
			//		{
			//			reconsume = false;
			//		}
			//		else
			//		{
			//			if (++pos == endPos)
			//			{
			//				goto IL_30EB;
			//			}
			//			c = buf[pos];
			//		}
			//		char c34 = c;
			//		if (c34 <= '\n')
			//		{
			//			if (c34 != '\0')
			//			{
			//				if (c34 == '\n')
			//				{
			//					this.SilentLineFeed();
			//				}
			//			}
			//			else
			//			{
			//				this.EmitReplacementCharacter(buf, pos);
			//			}
			//		}
			//		else
			//		{
			//			if (c34 == '\r')
			//			{
			//				goto IL_229A;
			//			}
			//			if (c34 == '-')
			//			{
			//				goto IL_2280;
			//			}
			//			if (c34 == '<')
			//			{
			//				break;
			//			}
			//		}
			//	}
			//	state = TokenizerState.SCRIPT_DATA_DOUBLE_ESCAPED_LESS_THAN_SIGN;
			//	continue;
			//	IL_2280:
			//	state = TokenizerState.SCRIPT_DATA_DOUBLE_ESCAPED_DASH;
			//	goto IL_22B0;
			//	IL_232F:
			//	while (++pos != endPos)
			//	{
			//		c = buf[pos];
			//		char c35 = c;
			//		if (c35 <= '\n')
			//		{
			//			if (c35 == '\0')
			//			{
			//				this.EmitReplacementCharacter(buf, pos);
			//				state = TokenizerState.SCRIPT_DATA_DOUBLE_ESCAPED;
			//				goto IL_00;
			//			}
			//			if (c35 == '\n')
			//			{
			//				this.SilentLineFeed();
			//			}
			//		}
			//		else
			//		{
			//			if (c35 == '\r')
			//			{
			//				goto IL_239C;
			//			}
			//			if (c35 == '-')
			//			{
			//				continue;
			//			}
			//			switch (c35)
			//			{
			//			case '<':
			//				state = TokenizerState.SCRIPT_DATA_DOUBLE_ESCAPED_LESS_THAN_SIGN;
			//				goto IL_23BD;
			//			case '>':
			//				state = TokenizerState.SCRIPT_DATA;
			//				goto IL_00;
			//			}
			//		}
			//		state = TokenizerState.SCRIPT_DATA_DOUBLE_ESCAPED;
			//		goto IL_00;
			//	}
			//	goto IL_30EB;
			//	IL_23F2:
			//	while (++pos != endPos)
			//	{
			//		c = buf[pos];
			//		if (this.index >= 6)
			//		{
			//			char c36 = c;
			//			if (c36 <= ' ')
			//			{
			//				switch (c36)
			//				{
			//				case '\t':
			//				case '\f':
			//					break;
			//				case '\n':
			//					this.SilentLineFeed();
			//					break;
			//				case '\v':
			//					goto IL_24AB;
			//				case '\r':
			//					goto IL_248C;
			//				default:
			//					if (c36 != ' ')
			//					{
			//						goto IL_24AB;
			//					}
			//					break;
			//				}
			//			}
			//			else if (c36 != '/' && c36 != '>')
			//			{
			//				goto IL_24AB;
			//			}
			//			state = TokenizerState.SCRIPT_DATA_ESCAPED;
			//			goto IL_00;
			//			IL_24AB:
			//			reconsume = true;
			//			state = TokenizerState.SCRIPT_DATA_DOUBLE_ESCAPED;
			//			goto IL_00;
			//		}
			//		char c37 = c;
			//		if (c >= 'A' && c <= 'Z')
			//		{
			//			c37 += ' ';
			//		}
			//		if (c37 != Tokenizer.SCRIPT_ARR[this.index])
			//		{
			//			reconsume = true;
			//			state = TokenizerState.SCRIPT_DATA_DOUBLE_ESCAPED;
			//			goto IL_00;
			//		}
			//		this.index++;
			//	}
			//	goto IL_30EB;
			//	for (;;)
			//	{
			//		IL_259D:
			//		if (reconsume)
			//		{
			//			reconsume = false;
			//		}
			//		else
			//		{
			//			if (++pos == endPos)
			//			{
			//				goto IL_30EB;
			//			}
			//			c = buf[pos];
			//		}
			//		c10 = c;
			//		if (c10 <= '\r')
			//		{
			//			if (c10 != '\0')
			//			{
			//				switch (c10)
			//				{
			//				case '\t':
			//				case '\f':
			//					continue;
			//				case '\n':
			//					this.SilentLineFeed();
			//					continue;
			//				case '\r':
			//					goto IL_25F5;
			//				}
			//				goto Block_401;
			//			}
			//			goto IL_2628;
			//		}
			//		else if (c10 != ' ')
			//		{
			//			break;
			//		}
			//	}
			//	if (c10 == '>')
			//	{
			//		this.ErrNamelessDoctype();
			//		this.forceQuirks = true;
			//		this.EmitDoctypeToken(pos);
			//		state = TokenizerState.DATA;
			//		continue;
			//	}
			//	IL_262F:
			//	if (c >= 'A' && c <= 'Z')
			//	{
			//		c += ' ';
			//	}
			//	this.ClearStrBufAndAppend(c);
			//	state = TokenizerState.DOCTYPE_NAME;
			//	goto IL_264B;
			//	IL_2628:
			//	c = '\ufffd';
			//	Block_401:
			//	goto IL_262F;
			//	IL_264B:
			//	while (++pos != endPos)
			//	{
			//		c = buf[pos];
			//		c10 = c;
			//		if (c10 <= '\r')
			//		{
			//			if (c10 != '\0')
			//			{
			//				switch (c10)
			//				{
			//				case '\t':
			//				case '\f':
			//					goto IL_26B5;
			//				case '\n':
			//					this.SilentLineFeed();
			//					goto IL_26B5;
			//				case '\r':
			//					goto IL_269A;
			//				}
			//			}
			//			else
			//			{
			//				c = '\ufffd';
			//			}
			//		}
			//		else
			//		{
			//			if (c10 == ' ')
			//			{
			//				goto IL_26B5;
			//			}
			//			if (c10 == '>')
			//			{
			//				this.StrBufToDoctypeName();
			//				this.EmitDoctypeToken(pos);
			//				state = TokenizerState.DATA;
			//				goto IL_00;
			//			}
			//		}
			//		if (c >= 'A' && c <= 'Z')
			//		{
			//			c += ' ';
			//		}
			//		this.AppendStrBuf(c);
			//		continue;
			//		IL_26B5:
			//		this.StrBufToDoctypeName();
			//		state = TokenizerState.AFTER_DOCTYPE_NAME;
			//		goto IL_26FE;
			//	}
			//	goto IL_30EB;
			//	IL_26FE:
			//	while (++pos != endPos)
			//	{
			//		c = buf[pos];
			//		c10 = c;
			//		if (c10 > '>')
			//		{
			//			if (c10 <= 'S')
			//			{
			//				if (c10 == 'P')
			//				{
			//					goto IL_278E;
			//				}
			//				if (c10 != 'S')
			//				{
			//					goto IL_27AB;
			//				}
			//			}
			//			else
			//			{
			//				if (c10 == 'p')
			//				{
			//					goto IL_278E;
			//				}
			//				if (c10 != 's')
			//				{
			//					goto IL_27AB;
			//				}
			//			}
			//			this.index = 0;
			//			state = TokenizerState.DOCTYPE_YSTEM;
			//			goto IL_00;
			//			IL_278E:
			//			this.index = 0;
			//			state = TokenizerState.DOCTYPE_UBLIC;
			//			goto IL_27BA;
			//		}
			//		switch (c10)
			//		{
			//		case '\t':
			//		case '\f':
			//			continue;
			//		case '\n':
			//			this.SilentLineFeed();
			//			continue;
			//		case '\v':
			//			break;
			//		case '\r':
			//			goto IL_2768;
			//		default:
			//			if (c10 == ' ')
			//			{
			//				continue;
			//			}
			//			if (c10 == '>')
			//			{
			//				this.EmitDoctypeToken(pos);
			//				state = TokenizerState.DATA;
			//				goto IL_00;
			//			}
			//			break;
			//		}
			//		IL_27AB:
			//		this.BogusDoctype();
			//		state = TokenizerState.BOGUS_DOCTYPE;
			//		goto IL_00;
			//	}
			//	goto IL_30EB;
			//	IL_27BA:
			//	while (++pos != endPos)
			//	{
			//		c = buf[pos];
			//		if (this.index >= 5)
			//		{
			//			state = TokenizerState.AFTER_DOCTYPE_PUBLIC_KEYWORD;
			//			reconsume = true;
			//			goto IL_2824;
			//		}
			//		char c38 = c;
			//		if (c >= 'A' && c <= 'Z')
			//		{
			//			c38 += ' ';
			//		}
			//		if (c38 != Tokenizer.UBLIC[this.index])
			//		{
			//			this.BogusDoctype();
			//			state = TokenizerState.BOGUS_DOCTYPE;
			//			reconsume = true;
			//			goto IL_00;
			//		}
			//		this.index++;
			//	}
			//	goto IL_30EB;
			//	IL_2901:
			//	while (++pos != endPos)
			//	{
			//		c = buf[pos];
			//		c10 = c;
			//		if (c10 <= '"')
			//		{
			//			switch (c10)
			//			{
			//			case '\t':
			//			case '\f':
			//				continue;
			//			case '\n':
			//				this.SilentLineFeed();
			//				continue;
			//			case '\v':
			//				break;
			//			case '\r':
			//				goto IL_2961;
			//			default:
			//				switch (c10)
			//				{
			//				case ' ':
			//					continue;
			//				case '"':
			//					this.ClearLongStrBuf();
			//					state = TokenizerState.DOCTYPE_PUBLIC_IDENTIFIER_DOUBLE_QUOTED;
			//					goto IL_29BE;
			//				}
			//				break;
			//			}
			//		}
			//		else
			//		{
			//			if (c10 == '\'')
			//			{
			//				this.ClearLongStrBuf();
			//				state = TokenizerState.DOCTYPE_PUBLIC_IDENTIFIER_SINGLE_QUOTED;
			//				goto IL_00;
			//			}
			//			if (c10 == '>')
			//			{
			//				this.ErrExpectedPublicId();
			//				this.forceQuirks = true;
			//				this.EmitDoctypeToken(pos);
			//				state = TokenizerState.DATA;
			//				goto IL_00;
			//			}
			//		}
			//		this.BogusDoctype();
			//		state = TokenizerState.BOGUS_DOCTYPE;
			//		goto IL_00;
			//	}
			//	goto IL_30EB;
			//	IL_29BE:
			//	while (++pos != endPos)
			//	{
			//		c = buf[pos];
			//		c10 = c;
			//		if (c10 <= '\n')
			//		{
			//			if (c10 != '\0')
			//			{
			//				if (c10 == '\n')
			//				{
			//					this.AppendLongStrBufLineFeed();
			//					continue;
			//				}
			//			}
			//			else
			//			{
			//				c = '\ufffd';
			//			}
			//		}
			//		else
			//		{
			//			if (c10 == '\r')
			//			{
			//				goto IL_2A39;
			//			}
			//			if (c10 == '"')
			//			{
			//				this.publicIdentifier = this.LongStrBufToString();
			//				state = TokenizerState.AFTER_DOCTYPE_PUBLIC_IDENTIFIER;
			//				goto IL_2A62;
			//			}
			//			if (c10 == '>')
			//			{
			//				this.ErrGtInPublicId();
			//				this.forceQuirks = true;
			//				this.publicIdentifier = this.LongStrBufToString();
			//				this.EmitDoctypeToken(pos);
			//				state = TokenizerState.DATA;
			//				goto IL_00;
			//			}
			//		}
			//		this.AppendLongStrBuf(c);
			//	}
			//	goto IL_30EB;
			//	IL_2B29:
			//	while (++pos != endPos)
			//	{
			//		c = buf[pos];
			//		c10 = c;
			//		if (c10 <= '"')
			//		{
			//			switch (c10)
			//			{
			//			case '\t':
			//			case '\f':
			//				continue;
			//			case '\n':
			//				this.SilentLineFeed();
			//				continue;
			//			case '\v':
			//				break;
			//			case '\r':
			//				goto IL_2B89;
			//			default:
			//				switch (c10)
			//				{
			//				case ' ':
			//					continue;
			//				case '"':
			//					this.ClearLongStrBuf();
			//					state = TokenizerState.DOCTYPE_SYSTEM_IDENTIFIER_DOUBLE_QUOTED;
			//					goto IL_2BD9;
			//				}
			//				break;
			//			}
			//		}
			//		else
			//		{
			//			if (c10 == '\'')
			//			{
			//				this.ClearLongStrBuf();
			//				state = TokenizerState.DOCTYPE_SYSTEM_IDENTIFIER_SINGLE_QUOTED;
			//				goto IL_00;
			//			}
			//			if (c10 == '>')
			//			{
			//				this.EmitDoctypeToken(pos);
			//				state = TokenizerState.DATA;
			//				goto IL_00;
			//			}
			//		}
			//		this.BogusDoctype();
			//		state = TokenizerState.BOGUS_DOCTYPE;
			//		goto IL_00;
			//	}
			//	goto IL_30EB;
			//	IL_2BD9:
			//	while (++pos != endPos)
			//	{
			//		c = buf[pos];
			//		c10 = c;
			//		if (c10 <= '\n')
			//		{
			//			if (c10 != '\0')
			//			{
			//				if (c10 == '\n')
			//				{
			//					this.AppendLongStrBufLineFeed();
			//					continue;
			//				}
			//			}
			//			else
			//			{
			//				c = '\ufffd';
			//			}
			//		}
			//		else
			//		{
			//			if (c10 == '\r')
			//			{
			//				goto IL_2C57;
			//			}
			//			if (c10 == '"')
			//			{
			//				this.systemIdentifier = this.LongStrBufToString();
			//				state = TokenizerState.AFTER_DOCTYPE_SYSTEM_IDENTIFIER;
			//				goto IL_00;
			//			}
			//			if (c10 == '>')
			//			{
			//				this.ErrGtInSystemId();
			//				this.forceQuirks = true;
			//				this.systemIdentifier = this.LongStrBufToString();
			//				this.EmitDoctypeToken(pos);
			//				state = TokenizerState.DATA;
			//				goto IL_00;
			//			}
			//		}
			//		this.AppendLongStrBuf(c);
			//	}
			//	goto IL_30EB;
			//	for (;;)
			//	{
			//		IL_2CF2:
			//		if (reconsume)
			//		{
			//			reconsume = false;
			//		}
			//		else
			//		{
			//			if (++pos == endPos)
			//			{
			//				goto IL_30EB;
			//			}
			//			c = buf[pos];
			//		}
			//		c10 = c;
			//		if (c10 != '\n')
			//		{
			//			if (c10 == '\r')
			//			{
			//				goto IL_2D36;
			//			}
			//			if (c10 == '>')
			//			{
			//				break;
			//			}
			//		}
			//		else
			//		{
			//			this.SilentLineFeed();
			//		}
			//	}
			//	this.EmitDoctypeToken(pos);
			//	state = TokenizerState.DATA;
			//	continue;
			//	IL_2E93:
			//	while (++pos != endPos)
			//	{
			//		c = buf[pos];
			//		c10 = c;
			//		if (c10 <= '"')
			//		{
			//			switch (c10)
			//			{
			//			case '\t':
			//			case '\f':
			//				continue;
			//			case '\n':
			//				this.SilentLineFeed();
			//				continue;
			//			case '\v':
			//				break;
			//			case '\r':
			//				goto IL_2EF3;
			//			default:
			//				switch (c10)
			//				{
			//				case ' ':
			//					continue;
			//				case '"':
			//					this.ClearLongStrBuf();
			//					state = TokenizerState.DOCTYPE_SYSTEM_IDENTIFIER_DOUBLE_QUOTED;
			//					goto IL_00;
			//				}
			//				break;
			//			}
			//		}
			//		else
			//		{
			//			if (c10 == '\'')
			//			{
			//				this.ClearLongStrBuf();
			//				state = TokenizerState.DOCTYPE_SYSTEM_IDENTIFIER_SINGLE_QUOTED;
			//				goto IL_2F50;
			//			}
			//			if (c10 == '>')
			//			{
			//				this.ErrExpectedSystemId();
			//				this.forceQuirks = true;
			//				this.EmitDoctypeToken(pos);
			//				state = TokenizerState.DATA;
			//				goto IL_00;
			//			}
			//		}
			//		this.BogusDoctype();
			//		state = TokenizerState.BOGUS_DOCTYPE;
			//		goto IL_00;
			//	}
			//	goto IL_30EB;
			//	IL_2F50:
			//	while (++pos != endPos)
			//	{
			//		c = buf[pos];
			//		c10 = c;
			//		if (c10 <= '\n')
			//		{
			//			if (c10 != '\0')
			//			{
			//				if (c10 == '\n')
			//				{
			//					this.AppendLongStrBufLineFeed();
			//					continue;
			//				}
			//			}
			//			else
			//			{
			//				c = '\ufffd';
			//			}
			//		}
			//		else
			//		{
			//			if (c10 == '\r')
			//			{
			//				goto IL_2FCE;
			//			}
			//			if (c10 == '\'')
			//			{
			//				this.systemIdentifier = this.LongStrBufToString();
			//				state = TokenizerState.AFTER_DOCTYPE_SYSTEM_IDENTIFIER;
			//				goto IL_00;
			//			}
			//			if (c10 == '>')
			//			{
			//				this.ErrGtInSystemId();
			//				this.forceQuirks = true;
			//				this.systemIdentifier = this.LongStrBufToString();
			//				this.EmitDoctypeToken(pos);
			//				state = TokenizerState.DATA;
			//				goto IL_00;
			//			}
			//		}
			//		this.AppendLongStrBuf(c);
			//	}
			//	goto IL_30EB;
			//	IL_2B0:
			//	if (++pos == endPos)
			//	{
			//		goto IL_30EB;
			//	}
			//	c = buf[pos];
			//	if (c >= 'A' && c <= 'Z')
			//	{
			//		this.endTag = false;
			//		this.ClearStrBufAndAppend(c + ' ');
			//		state = TokenizerState.TAG_NAME;
			//		goto IL_3B0;
			//	}
			//	if (c >= 'a' && c <= 'z')
			//	{
			//		this.endTag = false;
			//		this.ClearStrBufAndAppend(c);
			//		state = TokenizerState.TAG_NAME;
			//		goto IL_3B0;
			//	}
			//	char c39 = c;
			//	if (c39 == '!')
			//	{
			//		state = TokenizerState.MARKUP_DECLARATION_OPEN;
			//		continue;
			//	}
			//	if (c39 == '/')
			//	{
			//		state = TokenizerState.CLOSE_TAG_OPEN;
			//		continue;
			//	}
			//	switch (c39)
			//	{
			//	case '>':
			//		this.ErrLtGt();
			//		this.TokenHandler.Characters(Tokenizer.LT_GT, 0, 2);
			//		this.cstart = pos + 1;
			//		state = TokenizerState.DATA;
			//		continue;
			//	case '?':
			//		this.ErrProcessingInstruction();
			//		this.ClearLongStrBufAndAppend(c);
			//		state = TokenizerState.BOGUS_COMMENT;
			//		continue;
			//	default:
			//		this.ErrBadCharAfterLt(c);
			//		this.TokenHandler.Characters(Tokenizer.LT_GT, 0, 1);
			//		this.cstart = pos;
			//		state = TokenizerState.DATA;
			//		reconsume = true;
			//		continue;
			//	}
			//	IL_842:
			//	if (++pos != endPos)
			//	{
			//		c = buf[pos];
			//		char c40 = c;
			//		if (c40 <= ' ')
			//		{
			//			switch (c40)
			//			{
			//			case '\t':
			//			case '\f':
			//				break;
			//			case '\n':
			//				this.SilentLineFeed();
			//				break;
			//			case '\v':
			//				goto IL_8D0;
			//			case '\r':
			//				goto IL_892;
			//			default:
			//				if (c40 != ' ')
			//				{
			//					goto IL_8D0;
			//				}
			//				break;
			//			}
			//			state = TokenizerState.BEFORE_ATTRIBUTE_NAME;
			//			continue;
			//		}
			//		if (c40 == '/')
			//		{
			//			state = TokenizerState.SELF_CLOSING_START_TAG;
			//			goto IL_8E2;
			//		}
			//		if (c40 == '>')
			//		{
			//			state = this.EmitCurrentTagToken(false, pos);
			//			if (this.shouldSuspend)
			//			{
			//				goto Block_73;
			//			}
			//			continue;
			//		}
			//		IL_8D0:
			//		this.ErrNoSpaceBetweenAttributes();
			//		state = TokenizerState.BEFORE_ATTRIBUTE_NAME;
			//		reconsume = true;
			//		continue;
			//	}
			//	goto IL_30EB;
			//	IL_8E2:
			//	if (++pos == endPos)
			//	{
			//		goto IL_30EB;
			//	}
			//	c = buf[pos];
			//	char c41 = c;
			//	if (c41 != '>')
			//	{
			//		this.ErrSlashNotFollowedByGt();
			//		state = TokenizerState.BEFORE_ATTRIBUTE_NAME;
			//		reconsume = true;
			//		continue;
			//	}
			//	this.ErrHtml4XmlVoidSyntax();
			//	state = this.EmitCurrentTagToken(true, pos);
			//	if (this.shouldSuspend)
			//	{
			//		goto Block_76;
			//	}
			//	continue;
			//	IL_BE8:
			//	if (++pos == endPos)
			//	{
			//		goto IL_30EB;
			//	}
			//	c = buf[pos];
			//	char c42 = c;
			//	if (c42 == '\0')
			//	{
			//		goto IL_30EB;
			//	}
			//	if (c42 != '-')
			//	{
			//		this.ErrBogusComment();
			//		state = TokenizerState.BOGUS_COMMENT;
			//		reconsume = true;
			//		continue;
			//	}
			//	this.ClearLongStrBuf();
			//	state = TokenizerState.COMMENT_START;
			//	IL_C2A:
			//	if (++pos != endPos)
			//	{
			//		c = buf[pos];
			//		char c43 = c;
			//		if (c43 <= '\n')
			//		{
			//			if (c43 != '\0')
			//			{
			//				if (c43 == '\n')
			//				{
			//					this.AppendLongStrBufLineFeed();
			//					state = TokenizerState.COMMENT;
			//					goto IL_CBE;
			//				}
			//			}
			//			else
			//			{
			//				c = '\ufffd';
			//			}
			//		}
			//		else
			//		{
			//			if (c43 == '\r')
			//			{
			//				goto IL_C91;
			//			}
			//			if (c43 == '-')
			//			{
			//				this.AppendLongStrBuf(c);
			//				state = TokenizerState.COMMENT_START_DASH;
			//				continue;
			//			}
			//			if (c43 == '>')
			//			{
			//				this.ErrPrematureEndOfComment();
			//				this.EmitComment(0, pos);
			//				state = TokenizerState.DATA;
			//				continue;
			//			}
			//		}
			//		this.AppendLongStrBuf(c);
			//		state = TokenizerState.COMMENT;
			//		goto IL_CBE;
			//	}
			//	goto IL_30EB;
			//	IL_D23:
			//	if (++pos != endPos)
			//	{
			//		c = buf[pos];
			//		char c44 = c;
			//		if (c44 <= '\n')
			//		{
			//			if (c44 != '\0')
			//			{
			//				if (c44 == '\n')
			//				{
			//					this.AppendLongStrBufLineFeed();
			//					state = TokenizerState.COMMENT;
			//					continue;
			//				}
			//			}
			//			else
			//			{
			//				c = '\ufffd';
			//			}
			//		}
			//		else
			//		{
			//			if (c44 == '\r')
			//			{
			//				goto IL_D65;
			//			}
			//			if (c44 == '-')
			//			{
			//				this.AppendLongStrBuf(c);
			//				state = TokenizerState.COMMENT_END;
			//				goto IL_D9A;
			//			}
			//		}
			//		this.AppendLongStrBuf(c);
			//		state = TokenizerState.COMMENT;
			//		continue;
			//	}
			//	goto IL_30EB;
			//	IL_103A:
			//	if (++pos == endPos)
			//	{
			//		goto IL_30EB;
			//	}
			//	c = buf[pos];
			//	char c45 = c;
			//	if (c45 != ']')
			//	{
			//		this.TokenHandler.Characters(Tokenizer.RSQB_RSQB, 0, 1);
			//		this.cstart = pos;
			//		state = TokenizerState.CDATA_SECTION;
			//		reconsume = true;
			//		continue;
			//	}
			//	state = TokenizerState.CDATA_RSQB_RSQB;
			//	IL_1081:
			//	if (++pos == endPos)
			//	{
			//		goto IL_30EB;
			//	}
			//	c = buf[pos];
			//	char c46 = c;
			//	if (c46 == '>')
			//	{
			//		this.cstart = pos + 1;
			//		state = TokenizerState.DATA;
			//		continue;
			//	}
			//	this.TokenHandler.Characters(Tokenizer.RSQB_RSQB, 0, 2);
			//	this.cstart = pos;
			//	state = TokenizerState.CDATA_SECTION;
			//	reconsume = true;
			//	continue;
			//	IL_1173:
			//	if (++pos == endPos)
			//	{
			//		goto IL_30EB;
			//	}
			//	c = buf[pos];
			//	if (c == '\0')
			//	{
			//		goto IL_30EB;
			//	}
			//	char c47 = c;
			//	if (c47 <= ' ')
			//	{
			//		switch (c47)
			//		{
			//		case '\t':
			//		case '\n':
			//		case '\f':
			//		case '\r':
			//			break;
			//		case '\v':
			//			goto IL_1203;
			//		default:
			//			if (c47 != ' ')
			//			{
			//				goto IL_1203;
			//			}
			//			break;
			//		}
			//	}
			//	else
			//	{
			//		if (c47 == '#')
			//		{
			//			this.AppendStrBuf('#');
			//			state = TokenizerState.CONSUME_NCR;
			//			continue;
			//		}
			//		if (c47 != '&' && c47 != '<')
			//		{
			//			goto IL_1203;
			//		}
			//	}
			//	this.EmitOrAppendStrBuf(returnState);
			//	if ((returnState & (TokenizerState)240) != (TokenizerState)0)
			//	{
			//		this.cstart = pos;
			//	}
			//	state = returnState;
			//	reconsume = true;
			//	continue;
			//	IL_1203:
			//	if (c == this.additional)
			//	{
			//		this.EmitOrAppendStrBuf(returnState);
			//		state = returnState;
			//		reconsume = true;
			//		continue;
			//	}
			//	if (c >= 'a' && c <= 'z')
			//	{
			//		this.firstCharKey = (int)(c - 'a' + '\u001a');
			//	}
			//	else
			//	{
			//		if (c < 'A' || c > 'Z')
			//		{
			//			this.ErrNoNamedCharacterMatch();
			//			this.EmitOrAppendStrBuf(returnState);
			//			if ((returnState & (TokenizerState)240) != (TokenizerState)0)
			//			{
			//				this.cstart = pos;
			//			}
			//			state = returnState;
			//			reconsume = true;
			//			continue;
			//		}
			//		this.firstCharKey = (int)(c - 'A');
			//	}
			//	this.AppendStrBuf(c);
			//	state = TokenizerState.CHARACTER_REFERENCE_HILO_LOOKUP;
			//	IL_1285:
			//	if (++pos == endPos)
			//	{
			//		goto IL_30EB;
			//	}
			//	c = buf[pos];
			//	if (c == '\0')
			//	{
			//		goto IL_30EB;
			//	}
			//	int num = 0;
			//	if (c <= 'z')
			//	{
			//		int[] array2 = NamedCharactersAccel.HILO_ACCEL[(int)c];
			//		if (array2 != null)
			//		{
			//			num = array2[this.firstCharKey];
			//		}
			//	}
			//	if (num == 0)
			//	{
			//		this.ErrNoNamedCharacterMatch();
			//		this.EmitOrAppendStrBuf(returnState);
			//		if ((returnState & (TokenizerState)240) != (TokenizerState)0)
			//		{
			//			this.cstart = pos;
			//		}
			//		state = returnState;
			//		reconsume = true;
			//		continue;
			//	}
			//	this.AppendStrBuf(c);
			//	this.lo = num & 65535;
			//	this.hi = num >> 16;
			//	this.entCol = -1;
			//	this.candidate = -1;
			//	this.strBufMark = 0;
			//	state = TokenizerState.CHARACTER_REFERENCE_TAIL;
			//	goto IL_131E;
			//	IL_1759:
			//	this.HandleNcrValue(returnState);
			//	state = returnState;
			//	continue;
			//	IL_1B3A:
			//	if (++pos == endPos)
			//	{
			//		goto IL_30EB;
			//	}
			//	c = buf[pos];
			//	char c48 = c;
			//	if (c48 == '/')
			//	{
			//		this.index = 0;
			//		this.ClearStrBuf();
			//		state = TokenizerState.NON_DATA_END_TAG_NAME;
			//		goto IL_1B8E;
			//	}
			//	this.TokenHandler.Characters(Tokenizer.LT_GT, 0, 1);
			//	this.cstart = pos;
			//	state = returnState;
			//	reconsume = true;
			//	continue;
			//	IL_1E83:
			//	if (++pos == endPos)
			//	{
			//		goto IL_30EB;
			//	}
			//	c = buf[pos];
			//	char c49 = c;
			//	if (c49 != '!')
			//	{
			//		if (c49 == '/')
			//		{
			//			this.index = 0;
			//			this.ClearStrBuf();
			//			state = TokenizerState.NON_DATA_END_TAG_NAME;
			//			continue;
			//		}
			//		this.TokenHandler.Characters(Tokenizer.LT_GT, 0, 1);
			//		this.cstart = pos;
			//		state = TokenizerState.SCRIPT_DATA;
			//		reconsume = true;
			//		continue;
			//	}
			//	else
			//	{
			//		this.TokenHandler.Characters(Tokenizer.LT_GT, 0, 1);
			//		this.cstart = pos;
			//		state = TokenizerState.SCRIPT_DATA_ESCAPE_START;
			//	}
			//	IL_1EFE:
			//	if (++pos == endPos)
			//	{
			//		goto IL_30EB;
			//	}
			//	c = buf[pos];
			//	char c50 = c;
			//	if (c50 != '-')
			//	{
			//		state = TokenizerState.SCRIPT_DATA;
			//		reconsume = true;
			//		continue;
			//	}
			//	state = TokenizerState.SCRIPT_DATA_ESCAPE_START_DASH;
			//	IL_1F2B:
			//	if (++pos == endPos)
			//	{
			//		goto IL_30EB;
			//	}
			//	c = buf[pos];
			//	char c51 = c;
			//	if (c51 == '-')
			//	{
			//		state = TokenizerState.SCRIPT_DATA_ESCAPED_DASH_DASH;
			//		goto IL_1F58;
			//	}
			//	state = TokenizerState.SCRIPT_DATA;
			//	reconsume = true;
			//	continue;
			//	IL_2066:
			//	if (++pos != endPos)
			//	{
			//		c = buf[pos];
			//		char c52 = c;
			//		if (c52 <= '\n')
			//		{
			//			if (c52 == '\0')
			//			{
			//				this.EmitReplacementCharacter(buf, pos);
			//				state = TokenizerState.SCRIPT_DATA_ESCAPED;
			//				continue;
			//			}
			//			if (c52 == '\n')
			//			{
			//				this.SilentLineFeed();
			//			}
			//		}
			//		else
			//		{
			//			if (c52 == '\r')
			//			{
			//				goto IL_20CC;
			//			}
			//			if (c52 == '-')
			//			{
			//				state = TokenizerState.SCRIPT_DATA_ESCAPED_DASH_DASH;
			//				continue;
			//			}
			//			if (c52 == '<')
			//			{
			//				this.FlushChars(buf, pos);
			//				state = TokenizerState.SCRIPT_DATA_ESCAPED_LESS_THAN_SIGN;
			//				goto IL_20EB;
			//			}
			//		}
			//		state = TokenizerState.SCRIPT_DATA_ESCAPED;
			//		continue;
			//	}
			//	goto IL_30EB;
			//	IL_20EB:
			//	if (++pos == endPos)
			//	{
			//		goto IL_30EB;
			//	}
			//	c = buf[pos];
			//	char c53 = c;
			//	if (c53 == '/')
			//	{
			//		this.index = 0;
			//		this.ClearStrBuf();
			//		returnState = TokenizerState.SCRIPT_DATA_ESCAPED;
			//		state = TokenizerState.NON_DATA_END_TAG_NAME;
			//		continue;
			//	}
			//	if (c53 != 'S' && c53 != 's')
			//	{
			//		this.TokenHandler.Characters(Tokenizer.LT_GT, 0, 1);
			//		this.cstart = pos;
			//		reconsume = true;
			//		state = TokenizerState.SCRIPT_DATA_ESCAPED;
			//		continue;
			//	}
			//	this.TokenHandler.Characters(Tokenizer.LT_GT, 0, 1);
			//	this.cstart = pos;
			//	this.index = 1;
			//	state = TokenizerState.SCRIPT_DATA_DOUBLE_ESCAPE_START;
			//	goto IL_2178;
			//	IL_22B0:
			//	if (++pos != endPos)
			//	{
			//		c = buf[pos];
			//		char c54 = c;
			//		if (c54 <= '\n')
			//		{
			//			if (c54 == '\0')
			//			{
			//				this.EmitReplacementCharacter(buf, pos);
			//				state = TokenizerState.SCRIPT_DATA_DOUBLE_ESCAPED;
			//				continue;
			//			}
			//			if (c54 == '\n')
			//			{
			//				this.SilentLineFeed();
			//			}
			//		}
			//		else
			//		{
			//			if (c54 == '\r')
			//			{
			//				goto IL_230E;
			//			}
			//			if (c54 == '-')
			//			{
			//				state = TokenizerState.SCRIPT_DATA_DOUBLE_ESCAPED_DASH_DASH;
			//				goto IL_232F;
			//			}
			//			if (c54 == '<')
			//			{
			//				state = TokenizerState.SCRIPT_DATA_DOUBLE_ESCAPED_LESS_THAN_SIGN;
			//				continue;
			//			}
			//		}
			//		state = TokenizerState.SCRIPT_DATA_DOUBLE_ESCAPED;
			//		continue;
			//	}
			//	goto IL_30EB;
			//	IL_23BD:
			//	if (++pos == endPos)
			//	{
			//		goto IL_30EB;
			//	}
			//	c = buf[pos];
			//	char c55 = c;
			//	if (c55 == '/')
			//	{
			//		this.index = 0;
			//		state = TokenizerState.SCRIPT_DATA_DOUBLE_ESCAPE_END;
			//		goto IL_23F2;
			//	}
			//	reconsume = true;
			//	state = TokenizerState.SCRIPT_DATA_DOUBLE_ESCAPED;
			//	continue;
			//	IL_252A:
			//	if (reconsume)
			//	{
			//		reconsume = false;
			//	}
			//	else
			//	{
			//		if (++pos == endPos)
			//		{
			//			goto IL_30EB;
			//		}
			//		c = buf[pos];
			//	}
			//	this.InitDoctypeFields();
			//	c10 = c;
			//	switch (c10)
			//	{
			//	case '\t':
			//	case '\f':
			//		goto IL_258A;
			//	case '\n':
			//		this.SilentLineFeed();
			//		goto IL_258A;
			//	case '\v':
			//		break;
			//	case '\r':
			//		goto IL_2575;
			//	default:
			//		if (c10 == ' ')
			//		{
			//			goto IL_258A;
			//		}
			//		break;
			//	}
			//	this.ErrMissingSpaceBeforeDoctypeName();
			//	state = TokenizerState.BEFORE_DOCTYPE_NAME;
			//	reconsume = true;
			//	goto IL_259D;
			//	IL_258A:
			//	state = TokenizerState.BEFORE_DOCTYPE_NAME;
			//	goto IL_259D;
			//	IL_2824:
			//	if (reconsume)
			//	{
			//		reconsume = false;
			//	}
			//	else
			//	{
			//		if (++pos == endPos)
			//		{
			//			goto IL_30EB;
			//		}
			//		c = buf[pos];
			//	}
			//	c10 = c;
			//	if (c10 <= '"')
			//	{
			//		switch (c10)
			//		{
			//		case '\t':
			//		case '\f':
			//			break;
			//		case '\n':
			//			this.SilentLineFeed();
			//			break;
			//		case '\v':
			//			goto IL_28F2;
			//		case '\r':
			//			goto IL_288D;
			//		default:
			//			switch (c10)
			//			{
			//			case ' ':
			//				break;
			//			case '!':
			//				goto IL_28F2;
			//			case '"':
			//				this.ErrNoSpaceBetweenDoctypePublicKeywordAndQuote();
			//				this.ClearLongStrBuf();
			//				state = TokenizerState.DOCTYPE_PUBLIC_IDENTIFIER_DOUBLE_QUOTED;
			//				continue;
			//			default:
			//				goto IL_28F2;
			//			}
			//			break;
			//		}
			//		state = TokenizerState.BEFORE_DOCTYPE_PUBLIC_IDENTIFIER;
			//		goto IL_2901;
			//	}
			//	if (c10 == '\'')
			//	{
			//		this.ErrNoSpaceBetweenDoctypePublicKeywordAndQuote();
			//		this.ClearLongStrBuf();
			//		state = TokenizerState.DOCTYPE_PUBLIC_IDENTIFIER_SINGLE_QUOTED;
			//		continue;
			//	}
			//	if (c10 == '>')
			//	{
			//		this.ErrExpectedPublicId();
			//		this.forceQuirks = true;
			//		this.EmitDoctypeToken(pos);
			//		state = TokenizerState.DATA;
			//		continue;
			//	}
			//	IL_28F2:
			//	this.BogusDoctype();
			//	state = TokenizerState.BOGUS_DOCTYPE;
			//	continue;
			//	IL_2A62:
			//	if (++pos != endPos)
			//	{
			//		c = buf[pos];
			//		c10 = c;
			//		if (c10 <= '"')
			//		{
			//			switch (c10)
			//			{
			//			case '\t':
			//			case '\f':
			//				break;
			//			case '\n':
			//				this.SilentLineFeed();
			//				break;
			//			case '\v':
			//				goto IL_2B1A;
			//			case '\r':
			//				goto IL_2AC2;
			//			default:
			//				switch (c10)
			//				{
			//				case ' ':
			//					break;
			//				case '!':
			//					goto IL_2B1A;
			//				case '"':
			//					this.ErrNoSpaceBetweenPublicAndSystemIds();
			//					this.ClearLongStrBuf();
			//					state = TokenizerState.DOCTYPE_SYSTEM_IDENTIFIER_DOUBLE_QUOTED;
			//					continue;
			//				default:
			//					goto IL_2B1A;
			//				}
			//				break;
			//			}
			//			state = TokenizerState.BETWEEN_DOCTYPE_PUBLIC_AND_SYSTEM_IDENTIFIERS;
			//			goto IL_2B29;
			//		}
			//		if (c10 == '\'')
			//		{
			//			this.ErrNoSpaceBetweenPublicAndSystemIds();
			//			this.ClearLongStrBuf();
			//			state = TokenizerState.DOCTYPE_SYSTEM_IDENTIFIER_SINGLE_QUOTED;
			//			continue;
			//		}
			//		if (c10 == '>')
			//		{
			//			this.EmitDoctypeToken(pos);
			//			state = TokenizerState.DATA;
			//			continue;
			//		}
			//		IL_2B1A:
			//		this.BogusDoctype();
			//		state = TokenizerState.BOGUS_DOCTYPE;
			//		continue;
			//	}
			//	goto IL_30EB;
			//	IL_2DB6:
			//	if (reconsume)
			//	{
			//		reconsume = false;
			//	}
			//	else
			//	{
			//		if (++pos == endPos)
			//		{
			//			goto IL_30EB;
			//		}
			//		c = buf[pos];
			//	}
			//	c10 = c;
			//	if (c10 <= '"')
			//	{
			//		switch (c10)
			//		{
			//		case '\t':
			//		case '\f':
			//			break;
			//		case '\n':
			//			this.SilentLineFeed();
			//			break;
			//		case '\v':
			//			goto IL_2E84;
			//		case '\r':
			//			goto IL_2E1F;
			//		default:
			//			switch (c10)
			//			{
			//			case ' ':
			//				break;
			//			case '!':
			//				goto IL_2E84;
			//			case '"':
			//				this.ErrNoSpaceBetweenDoctypeSystemKeywordAndQuote();
			//				this.ClearLongStrBuf();
			//				state = TokenizerState.DOCTYPE_SYSTEM_IDENTIFIER_DOUBLE_QUOTED;
			//				continue;
			//			default:
			//				goto IL_2E84;
			//			}
			//			break;
			//		}
			//		state = TokenizerState.BEFORE_DOCTYPE_SYSTEM_IDENTIFIER;
			//		goto IL_2E93;
			//	}
			//	if (c10 == '\'')
			//	{
			//		this.ErrNoSpaceBetweenDoctypeSystemKeywordAndQuote();
			//		this.ClearLongStrBuf();
			//		state = TokenizerState.DOCTYPE_SYSTEM_IDENTIFIER_SINGLE_QUOTED;
			//		continue;
			//	}
			//	if (c10 == '>')
			//	{
			//		this.ErrExpectedPublicId();
			//		this.forceQuirks = true;
			//		this.EmitDoctypeToken(pos);
			//		state = TokenizerState.DATA;
			//		continue;
			//	}
			//	IL_2E84:
			//	this.BogusDoctype();
			//	state = TokenizerState.BOGUS_DOCTYPE;
			//}
			//for (;;)
			//{
			//	IL_18F8:
			//	if (reconsume)
			//	{
			//		reconsume = false;
			//	}
			//	else
			//	{
			//		if (++pos == endPos)
			//		{
			//			goto IL_30EB;
			//		}
			//		c = buf[pos];
			//	}
			//	char c56 = c;
			//	if (c56 != '\0')
			//	{
			//		if (c56 != '\n')
			//		{
			//			if (c56 == '\r')
			//			{
			//				break;
			//			}
			//		}
			//		else
			//		{
			//			this.SilentLineFeed();
			//		}
			//	}
			//	else
			//	{
			//		this.EmitPlaintextReplacementCharacter(buf, pos);
			//	}
			//}
			//this.EmitCarriageReturn(buf, pos);
			//goto IL_30EB;
			//IL_297:
			//this.EmitCarriageReturn(buf, pos);
			//goto IL_30EB;
			//IL_408:
			//this.SilentCarriageReturn();
			//this.StrBufToElementNameString();
			//state = TokenizerState.BEFORE_ATTRIBUTE_NAME;
			//goto IL_30EB;
			//IL_509:
			//this.SilentCarriageReturn();
			//Block_36:
			//goto IL_30EB;
			//IL_5F0:
			//this.SilentCarriageReturn();
			//this.AttributeNameComplete();
			//state = TokenizerState.AFTER_ATTRIBUTE_NAME;
			//Block_47:
			//goto IL_30EB;
			//IL_719:
			//this.SilentCarriageReturn();
			//Block_58:
			//goto IL_30EB;
			//IL_81C:
			//this.AppendLongStrBufCarriageReturn();
			//goto IL_30EB;
			//IL_892:
			//this.SilentCarriageReturn();
			//state = TokenizerState.BEFORE_ATTRIBUTE_NAME;
			//Block_73:
			//Block_76:
			//goto IL_30EB;
			//IL_9C6:
			//this.SilentCarriageReturn();
			//this.AddAttributeWithValue();
			//state = TokenizerState.BEFORE_ATTRIBUTE_NAME;
			//Block_86:
			//goto IL_30EB;
			//IL_ACD:
			//this.SilentCarriageReturn();
			//Block_95:
			//goto IL_30EB;
			//IL_C91:
			//this.AppendLongStrBufCarriageReturn();
			//state = TokenizerState.COMMENT;
			//goto IL_30EB;
			//IL_D00:
			//this.AppendLongStrBufCarriageReturn();
			//goto IL_30EB;
			//IL_D65:
			//this.AppendLongStrBufCarriageReturn();
			//state = TokenizerState.COMMENT;
			//goto IL_30EB;
			//IL_DF8:
			//this.AdjustDoubleHyphenAndAppendToLongStrBufCarriageReturn();
			//state = TokenizerState.COMMENT;
			//goto IL_30EB;
			//IL_EA2:
			//this.AppendLongStrBufCarriageReturn();
			//goto IL_30EB;
			//IL_F33:
			//this.AppendLongStrBufCarriageReturn();
			//state = TokenizerState.COMMENT;
			//goto IL_30EB;
			//IL_1024:
			//this.EmitCarriageReturn(buf, pos);
			//goto IL_30EB;
			//IL_114A:
			//this.AppendLongStrBufCarriageReturn();
			//goto IL_30EB;
			//IL_199B:
			//this.SilentCarriageReturn();
			//this.ErrGarbageAfterLtSlash();
			//this.ClearLongStrBufAndAppend('\n');
			//state = TokenizerState.BOGUS_COMMENT;
			//goto IL_30EB;
			//IL_1AB0:
			//this.EmitCarriageReturn(buf, pos);
			//goto IL_30EB;
			//IL_1B24:
			//this.EmitCarriageReturn(buf, pos);
			//goto IL_30EB;
			//IL_1C78:
			//this.SilentCarriageReturn();
			//state = TokenizerState.BEFORE_ATTRIBUTE_NAME;
			//goto IL_30EB;
			//IL_1D5A:
			//this.AppendLongStrBufCarriageReturn();
			//goto IL_30EB;
			//IL_1DDD:
			//this.AppendLongStrBufCarriageReturn();
			//state = TokenizerState.BOGUS_COMMENT;
			//goto IL_30EB;
			//IL_1E6D:
			//this.EmitCarriageReturn(buf, pos);
			//goto IL_30EB;
			//IL_1FCD:
			//this.EmitCarriageReturn(buf, pos);
			//state = TokenizerState.SCRIPT_DATA_ESCAPED;
			//goto IL_30EB;
			//IL_2050:
			//this.EmitCarriageReturn(buf, pos);
			//goto IL_30EB;
			//IL_20CC:
			//this.EmitCarriageReturn(buf, pos);
			//state = TokenizerState.SCRIPT_DATA_ESCAPED;
			//goto IL_30EB;
			//IL_2211:
			//this.EmitCarriageReturn(buf, pos);
			//state = TokenizerState.SCRIPT_DATA_DOUBLE_ESCAPED;
			//goto IL_30EB;
			//IL_229A:
			//this.EmitCarriageReturn(buf, pos);
			//goto IL_30EB;
			//IL_230E:
			//this.EmitCarriageReturn(buf, pos);
			//state = TokenizerState.SCRIPT_DATA_DOUBLE_ESCAPED;
			//goto IL_30EB;
			//IL_239C:
			//this.EmitCarriageReturn(buf, pos);
			//state = TokenizerState.SCRIPT_DATA_DOUBLE_ESCAPED;
			//goto IL_30EB;
			//IL_248C:
			//this.EmitCarriageReturn(buf, pos);
			//state = TokenizerState.SCRIPT_DATA_ESCAPED;
			//goto IL_30EB;
			//IL_2575:
			//this.SilentCarriageReturn();
			//state = TokenizerState.BEFORE_DOCTYPE_NAME;
			//goto IL_30EB;
			//IL_25F5:
			//this.SilentCarriageReturn();
			//goto IL_30EB;
			//IL_269A:
			//this.SilentCarriageReturn();
			//this.StrBufToDoctypeName();
			//state = TokenizerState.AFTER_DOCTYPE_NAME;
			//goto IL_30EB;
			//IL_2768:
			//this.SilentCarriageReturn();
			//goto IL_30EB;
			//IL_288D:
			//this.SilentCarriageReturn();
			//state = TokenizerState.BEFORE_DOCTYPE_PUBLIC_IDENTIFIER;
			//goto IL_30EB;
			//IL_2961:
			//this.SilentCarriageReturn();
			//goto IL_30EB;
			//IL_2A39:
			//this.AppendLongStrBufCarriageReturn();
			//goto IL_30EB;
			//IL_2AC2:
			//this.SilentCarriageReturn();
			//state = TokenizerState.BETWEEN_DOCTYPE_PUBLIC_AND_SYSTEM_IDENTIFIERS;
			//goto IL_30EB;
			//IL_2B89:
			//this.SilentCarriageReturn();
			//goto IL_30EB;
			//IL_2C57:
			//this.AppendLongStrBufCarriageReturn();
			//goto IL_30EB;
			//IL_2CC2:
			//this.SilentCarriageReturn();
			//goto IL_30EB;
			//IL_2D36:
			//this.SilentCarriageReturn();
			//goto IL_30EB;
			//IL_2E1F:
			//this.SilentCarriageReturn();
			//state = TokenizerState.BEFORE_DOCTYPE_SYSTEM_IDENTIFIER;
			//goto IL_30EB;
			//IL_2EF3:
			//this.SilentCarriageReturn();
			//goto IL_30EB;
			//IL_2FCE:
			//this.AppendLongStrBufCarriageReturn();
			//goto IL_30EB;
			//IL_3075:
			//this.AppendLongStrBufCarriageReturn();
			//IL_30EB:
			//this.FlushChars(buf, pos);
			//this.stateSave = state;
			//this.returnStateSave = returnState;
			return pos;
		}

		void InitDoctypeFields()
		{
			this.doctypeName = "";
			this.systemIdentifier = null;
			this.publicIdentifier = null;
			this.forceQuirks = false;
		}

		void AdjustDoubleHyphenAndAppendToLongStrBufCarriageReturn()
		{
			this.SilentCarriageReturn();
			this.AdjustDoubleHyphenAndAppendToLongStrBufAndErr('\n');
		}

		void AdjustDoubleHyphenAndAppendToLongStrBufLineFeed()
		{
			this.SilentLineFeed();
			this.AdjustDoubleHyphenAndAppendToLongStrBufAndErr('\n');
		}

		void AppendLongStrBufLineFeed()
		{
			this.SilentLineFeed();
			this.AppendLongStrBuf('\n');
		}

		void AppendLongStrBufCarriageReturn()
		{
			this.SilentCarriageReturn();
			this.AppendLongStrBuf('\n');
		}

		protected void SilentCarriageReturn()
		{
			this.line++;
			this.lastCR = true;
		}

		protected void SilentLineFeed()
		{
			this.line++;
		}

		void EmitCarriageReturn(char[] buf, int pos)
		{
			this.SilentCarriageReturn();
			this.FlushChars(buf, pos);
			this.TokenHandler.Characters(Tokenizer.LF, 0, 1);
			this.cstart = int.MaxValue;
		}

		void EmitReplacementCharacter(char[] buf, int pos)
		{
			this.FlushChars(buf, pos);
			this.TokenHandler.ZeroOriginatingReplacementCharacter();
			this.cstart = pos + 1;
		}

		void EmitPlaintextReplacementCharacter(char[] buf, int pos)
		{
			this.FlushChars(buf, pos);
			this.TokenHandler.Characters(Tokenizer.REPLACEMENT_CHARACTER, 0, 1);
			this.cstart = pos + 1;
		}

		void SetAdditionalAndRememberAmpersandLocation(char add)
		{
			this.additional = add;
			this.ampersandLocation = new Locator(this);
		}

		void BogusDoctype()
		{
			this.ErrBogusDoctype();
			this.forceQuirks = true;
		}

		void BogusDoctypeWithoutQuirks()
		{
			this.ErrBogusDoctype();
			this.forceQuirks = false;
		}

		void EmitOrAppendStrBuf(TokenizerState returnState)
		{
			if ((returnState & (TokenizerState)240) == (TokenizerState)0)
			{
				this.AppendStrBufToLongStrBuf();
				return;
			}
			this.EmitStrBuf();
		}

		void HandleNcrValue(TokenizerState returnState)
		{
			if (this.value <= 65535)
			{
				if (this.value >= 128 && this.value <= 159)
				{
					this.ErrNcrInC1Range();
					char[] val = NamedCharacters.WINDOWS_1252[this.value - 128];
					this.EmitOrAppendOne(val, returnState);
					return;
				}
				if (this.value == 12 && this.contentSpacePolicy != XmlViolationPolicy.Allow)
				{
					if (this.contentSpacePolicy == XmlViolationPolicy.AlterInfoset)
					{
						this.EmitOrAppendOne(Tokenizer.SPACE, returnState);
						return;
					}
					if (this.contentSpacePolicy == XmlViolationPolicy.Fatal)
					{
						this.Fatal("A character reference expanded to a form feed which is not legal XML 1.0 white space.");
						return;
					}
				}
				else
				{
					if (this.value == 0)
					{
						this.ErrNcrZero();
						this.EmitOrAppendOne(Tokenizer.REPLACEMENT_CHARACTER, returnState);
						return;
					}
					if ((this.value & 63488) == 55296)
					{
						this.ErrNcrSurrogate();
						this.EmitOrAppendOne(Tokenizer.REPLACEMENT_CHARACTER, returnState);
						return;
					}
					char c = (char)this.value;
					if (this.value == 13)
					{
						this.ErrNcrCr();
					}
					else if (this.value <= 8 || this.value == 11 || (this.value >= 14 && this.value <= 31))
					{
						c = this.ErrNcrControlChar(c);
					}
					else if (this.value >= 64976 && this.value <= 65007)
					{
						this.ErrNcrUnassigned();
					}
					else if ((this.value & 65534) == 65534)
					{
						c = this.ErrNcrNonCharacter(c);
					}
					else if (this.value >= 127 && this.value <= 159)
					{
						this.ErrNcrControlChar();
					}
					else
					{
						this.MaybeWarnPrivateUse(c);
					}
					this.bmpChar[0] = c;
					this.EmitOrAppendOne(this.bmpChar, returnState);
					return;
				}
			}
			else
			{
				if (this.value <= 1114111)
				{
					this.MaybeWarnPrivateUseAstral();
					if ((this.value & 65534) == 65534)
					{
						this.ErrAstralNonCharacter(this.value);
					}
					this.astralChar[0] = (char)(55232 + (this.value >> 10));
					this.astralChar[1] = (char)(56320 + (this.value & 1023));
					this.EmitOrAppendTwo(this.astralChar, returnState);
					return;
				}
				this.ErrNcrOutOfRange();
				this.EmitOrAppendOne(Tokenizer.REPLACEMENT_CHARACTER, returnState);
			}
		}

		public void Eof()
		{
			TokenizerState tokenizerState = this.stateSave;
			TokenizerState tokenizerState2 = this.returnStateSave;
			TokenizerState tokenizerState3;
			for (;;)
			{
				tokenizerState3 = tokenizerState;
				switch (tokenizerState3)
				{
				case TokenizerState.ATTRIBUTE_VALUE_DOUBLE_QUOTED:
				case TokenizerState.ATTRIBUTE_VALUE_SINGLE_QUOTED:
				case TokenizerState.ATTRIBUTE_VALUE_UNQUOTED:
					goto IL_1D4;
				case TokenizerState.PLAINTEXT:
				case TokenizerState.HANDLE_NCR_VALUE:
				case TokenizerState.HANDLE_NCR_VALUE_RECONSUME:
				case TokenizerState.CDATA_START:
				case TokenizerState.CDATA_SECTION:
				case TokenizerState.SCRIPT_DATA_ESCAPE_START:
				case TokenizerState.SCRIPT_DATA_ESCAPE_START_DASH:
				case TokenizerState.SCRIPT_DATA_ESCAPED_DASH:
				case TokenizerState.SCRIPT_DATA_ESCAPED_DASH_DASH:
					goto IL_69A;
				case TokenizerState.TAG_OPEN:
					goto IL_13A;
				case TokenizerState.CLOSE_TAG_OPEN:
					goto IL_18B;
				case TokenizerState.TAG_NAME:
					goto IL_1A8;
				case TokenizerState.BEFORE_ATTRIBUTE_NAME:
				case TokenizerState.AFTER_ATTRIBUTE_VALUE_QUOTED:
				case TokenizerState.SELF_CLOSING_START_TAG:
					goto IL_1B3;
				case TokenizerState.ATTRIBUTE_NAME:
					goto IL_1BE;
				case TokenizerState.AFTER_ATTRIBUTE_NAME:
				case TokenizerState.BEFORE_ATTRIBUTE_VALUE:
					goto IL_1C9;
				case TokenizerState.BOGUS_COMMENT:
					goto IL_1DF;
				case TokenizerState.MARKUP_DECLARATION_OPEN:
					goto IL_1FF;
				case TokenizerState.DOCTYPE:
				case TokenizerState.BEFORE_DOCTYPE_NAME:
					goto IL_2D5;
				case TokenizerState.DOCTYPE_NAME:
					goto IL_2EE;
				case TokenizerState.AFTER_DOCTYPE_NAME:
				case TokenizerState.BEFORE_DOCTYPE_PUBLIC_IDENTIFIER:
				case TokenizerState.DOCTYPE_UBLIC:
				case TokenizerState.DOCTYPE_YSTEM:
				case TokenizerState.AFTER_DOCTYPE_PUBLIC_KEYWORD:
				case TokenizerState.AFTER_DOCTYPE_SYSTEM_KEYWORD:
					goto IL_30D;
				case TokenizerState.DOCTYPE_PUBLIC_IDENTIFIER_DOUBLE_QUOTED:
				case TokenizerState.DOCTYPE_PUBLIC_IDENTIFIER_SINGLE_QUOTED:
					goto IL_326;
				case TokenizerState.AFTER_DOCTYPE_PUBLIC_IDENTIFIER:
				case TokenizerState.BEFORE_DOCTYPE_SYSTEM_IDENTIFIER:
				case TokenizerState.BETWEEN_DOCTYPE_PUBLIC_AND_SYSTEM_IDENTIFIERS:
					goto IL_34B;
				case TokenizerState.DOCTYPE_SYSTEM_IDENTIFIER_DOUBLE_QUOTED:
				case TokenizerState.DOCTYPE_SYSTEM_IDENTIFIER_SINGLE_QUOTED:
					goto IL_364;
				case TokenizerState.AFTER_DOCTYPE_SYSTEM_IDENTIFIER:
					goto IL_389;
				case TokenizerState.BOGUS_DOCTYPE:
					goto IL_3A2;
				case TokenizerState.COMMENT_START:
				case TokenizerState.COMMENT:
					goto IL_289;
				case TokenizerState.COMMENT_START_DASH:
				case TokenizerState.COMMENT_END_DASH:
					goto IL_2AF;
				case TokenizerState.COMMENT_END:
					goto IL_29C;
				case TokenizerState.COMMENT_END_BANG:
					goto IL_2C2;
				case TokenizerState.NON_DATA_END_TAG_NAME:
					goto IL_16E;
				case TokenizerState.MARKUP_DECLARATION_HYPHEN:
					goto IL_218;
				case TokenizerState.MARKUP_DECLARATION_OCTYPE:
					goto IL_22B;
				case TokenizerState.CONSUME_CHARACTER_REFERENCE:
					this.EmitOrAppendStrBuf(tokenizerState2);
					tokenizerState = tokenizerState2;
					continue;
				case TokenizerState.CONSUME_NCR:
				case TokenizerState.HEX_NCR_LOOP:
				case TokenizerState.DECIMAL_NRC_LOOP:
					if (!this.seenDigits)
					{
						this.ErrNoDigitsInNCR();
						this.EmitOrAppendStrBuf(tokenizerState2);
						tokenizerState = tokenizerState2;
						continue;
					}
					this.ErrCharRefLacksSemicolon();
					this.HandleNcrValue(tokenizerState2);
					tokenizerState = tokenizerState2;
					continue;
				case TokenizerState.CHARACTER_REFERENCE_TAIL:
				{
					for (;;)
					{
						IL_3D0:
						char c = '\0';
						this.entCol++;
						while (this.hi != -1 && this.entCol != NamedCharacters.NAMES[this.hi].Length)
						{
							if (this.entCol > NamedCharacters.NAMES[this.hi].Length)
							{
								goto IL_4F1;
							}
							if (c >= NamedCharacters.NAMES[this.hi][this.entCol])
							{
								break;
							}
							this.hi--;
						}
						while (this.hi >= this.lo)
						{
							if (this.entCol == NamedCharacters.NAMES[this.lo].Length)
							{
								this.candidate = this.lo;
								this.strBufMark = this.strBufLen;
								this.lo++;
							}
							else
							{
								if (this.entCol > NamedCharacters.NAMES[this.lo].Length)
								{
									break;
								}
								if (c > NamedCharacters.NAMES[this.lo][this.entCol])
								{
									this.lo++;
								}
								else
								{
									if (this.hi < this.lo)
									{
										break;
									}
									goto IL_3D0;
								}
							}
						}
						break;
					}
					IL_4F1:
					if (this.candidate == -1)
					{
						this.ErrNoNamedCharacterMatch();
						this.EmitOrAppendStrBuf(tokenizerState2);
						tokenizerState = tokenizerState2;
						continue;
					}
					string text = NamedCharacters.NAMES[this.candidate];
					if (text.Length == 0 || text[text.Length - 1] != ';')
					{
						if ((tokenizerState2 & (TokenizerState)240) == (TokenizerState)0)
						{
							char c2;
							if (this.strBufMark == this.strBufLen)
							{
								c2 = '\0';
							}
							else
							{
								c2 = this.strBuf[this.strBufMark];
							}
							if ((c2 >= '0' && c2 <= '9') || (c2 >= 'A' && c2 <= 'Z') || (c2 >= 'a' && c2 <= 'z'))
							{
								this.ErrNoNamedCharacterMatch();
								this.AppendStrBufToLongStrBuf();
								tokenizerState = tokenizerState2;
								continue;
							}
						}
						if ((tokenizerState2 & (TokenizerState)240) == (TokenizerState)0)
						{
							this.ErrUnescapedAmpersandInterpretedAsCharacterReference();
						}
						else
						{
							this.ErrNotSemicolonTerminated();
						}
					}
					char[] array = NamedCharacters.VALUES[this.candidate];
					if (array.Length == 1)
					{
						this.EmitOrAppendOne(array, tokenizerState2);
					}
					else
					{
						this.EmitOrAppendTwo(array, tokenizerState2);
					}
					if (this.strBufMark < this.strBufLen)
					{
						if ((tokenizerState2 & (TokenizerState)240) == (TokenizerState)0)
						{
							for (int i = this.strBufMark; i < this.strBufLen; i++)
							{
								this.AppendLongStrBuf(this.strBuf[i]);
							}
						}
						else
						{
							this.TokenHandler.Characters(this.strBuf, this.strBufMark, this.strBufLen - this.strBufMark);
						}
					}
					tokenizerState = tokenizerState2;
					continue;
				}
				case TokenizerState.CHARACTER_REFERENCE_HILO_LOOKUP:
					this.ErrNoNamedCharacterMatch();
					this.EmitOrAppendStrBuf(tokenizerState2);
					tokenizerState = tokenizerState2;
					continue;
				case TokenizerState.CDATA_RSQB:
					goto IL_674;
				case TokenizerState.CDATA_RSQB_RSQB:
					goto IL_688;
				case TokenizerState.SCRIPT_DATA_LESS_THAN_SIGN:
				case TokenizerState.SCRIPT_DATA_ESCAPED_LESS_THAN_SIGN:
					goto IL_123;
				case TokenizerState.BOGUS_COMMENT_HYPHEN:
					goto IL_1EC;
				case TokenizerState.RAWTEXT_RCDATA_LESS_THAN_SIGN:
					goto IL_157;
				}
				break;
			}
			if (tokenizerState3 != TokenizerState.DATA)
			{
				goto IL_69A;
			}
			goto IL_69A;
			IL_123:
			this.TokenHandler.Characters(Tokenizer.LT_GT, 0, 1);
			goto IL_69A;
			IL_13A:
			this.ErrEofAfterLt();
			this.TokenHandler.Characters(Tokenizer.LT_GT, 0, 1);
			goto IL_69A;
			IL_157:
			this.TokenHandler.Characters(Tokenizer.LT_GT, 0, 1);
			goto IL_69A;
			IL_16E:
			this.TokenHandler.Characters(Tokenizer.LT_SOLIDUS, 0, 2);
			this.EmitStrBuf();
			goto IL_69A;
			IL_18B:
			this.ErrEofAfterLt();
			this.TokenHandler.Characters(Tokenizer.LT_SOLIDUS, 0, 2);
			goto IL_69A;
			IL_1A8:
			this.ErrEofInTagName();
			goto IL_69A;
			IL_1B3:
			this.ErrEofWithoutGt();
			goto IL_69A;
			IL_1BE:
			this.ErrEofInAttributeName();
			goto IL_69A;
			IL_1C9:
			this.ErrEofWithoutGt();
			goto IL_69A;
			IL_1D4:
			this.ErrEofInAttributeValue();
			goto IL_69A;
			IL_1DF:
			this.EmitComment(0, 0);
			goto IL_69A;
			IL_1EC:
			this.MaybeAppendSpaceToBogusComment();
			this.EmitComment(0, 0);
			goto IL_69A;
			IL_1FF:
			this.ErrBogusComment();
			this.ClearLongStrBuf();
			this.EmitComment(0, 0);
			goto IL_69A;
			IL_218:
			this.ErrBogusComment();
			this.EmitComment(0, 0);
			goto IL_69A;
			IL_22B:
			if (this.index < 6)
			{
				this.ErrBogusComment();
				this.EmitComment(0, 0);
				goto IL_69A;
			}
			this.ErrEofInDoctype();
			this.doctypeName = "";
			if (this.systemIdentifier != null)
			{
				this.systemIdentifier = null;
			}
			if (this.publicIdentifier != null)
			{
				this.publicIdentifier = null;
			}
			this.forceQuirks = true;
			this.EmitDoctypeToken(0);
			goto IL_69A;
			IL_289:
			this.ErrEofInComment();
			this.EmitComment(0, 0);
			goto IL_69A;
			IL_29C:
			this.ErrEofInComment();
			this.EmitComment(2, 0);
			goto IL_69A;
			IL_2AF:
			this.ErrEofInComment();
			this.EmitComment(1, 0);
			goto IL_69A;
			IL_2C2:
			this.ErrEofInComment();
			this.EmitComment(3, 0);
			goto IL_69A;
			IL_2D5:
			this.ErrEofInDoctype();
			this.forceQuirks = true;
			this.EmitDoctypeToken(0);
			goto IL_69A;
			IL_2EE:
			this.ErrEofInDoctype();
			this.StrBufToDoctypeName();
			this.forceQuirks = true;
			this.EmitDoctypeToken(0);
			goto IL_69A;
			IL_30D:
			this.ErrEofInDoctype();
			this.forceQuirks = true;
			this.EmitDoctypeToken(0);
			goto IL_69A;
			IL_326:
			this.ErrEofInPublicId();
			this.forceQuirks = true;
			this.publicIdentifier = this.LongStrBufToString();
			this.EmitDoctypeToken(0);
			goto IL_69A;
			IL_34B:
			this.ErrEofInDoctype();
			this.forceQuirks = true;
			this.EmitDoctypeToken(0);
			goto IL_69A;
			IL_364:
			this.ErrEofInSystemId();
			this.forceQuirks = true;
			this.systemIdentifier = this.LongStrBufToString();
			this.EmitDoctypeToken(0);
			goto IL_69A;
			IL_389:
			this.ErrEofInDoctype();
			this.forceQuirks = true;
			this.EmitDoctypeToken(0);
			goto IL_69A;
			IL_3A2:
			this.EmitDoctypeToken(0);
			goto IL_69A;
			IL_674:
			this.TokenHandler.Characters(Tokenizer.RSQB_RSQB, 0, 1);
			goto IL_69A;
			IL_688:
			this.TokenHandler.Characters(Tokenizer.RSQB_RSQB, 0, 2);
			IL_69A:
			this.TokenHandler.Eof();
		}

		void EmitDoctypeToken(int pos)
		{
			this.cstart = pos + 1;
			this.TokenHandler.Doctype(this.doctypeName, this.publicIdentifier, this.systemIdentifier, this.forceQuirks);
			this.doctypeName = null;
			this.publicIdentifier = null;
			this.systemIdentifier = null;
		}

		public bool IsAlreadyComplainedAboutNonAscii
		{
			get
			{
				return true;
			}
		}

		public bool InternalEncodingDeclaration(string internalCharset)
		{
			bool result = false;
			if (this.EncodingDeclared != null)
			{
				foreach (Delegate @delegate in this.EncodingDeclared.GetInvocationList())
				{
					EncodingDetectedEventArgs encodingDetectedEventArgs = new EncodingDetectedEventArgs(internalCharset);
					@delegate.DynamicInvoke(new object[] { this, encodingDetectedEventArgs });
					if (encodingDetectedEventArgs.AcceptEncoding)
					{
						result = true;
					}
				}
			}
			return result;
		}

		void EmitOrAppendTwo(char[] val, TokenizerState returnState)
		{
			if ((returnState & (TokenizerState)240) == (TokenizerState)0)
			{
				this.AppendLongStrBuf(val[0]);
				this.AppendLongStrBuf(val[1]);
				return;
			}
			this.TokenHandler.Characters(val, 0, 2);
		}

		void EmitOrAppendOne(char[] val, TokenizerState returnState)
		{
			if ((returnState & (TokenizerState)240) == (TokenizerState)0)
			{
				this.AppendLongStrBuf(val[0]);
				return;
			}
			this.TokenHandler.Characters(val, 0, 1);
		}

		public void End()
		{
			this.strBuf = null;
			this.longStrBuf = null;
			this.doctypeName = null;
			this.systemIdentifier = null;
			this.publicIdentifier = null;
			this.tagName = null;
			this.attributeName = null;
			this.TokenHandler.EndTokenization();
			if (this.attributes != null)
			{
				this.attributes.Clear(this.mappingLangToXmlLang);
				this.attributes = null;
			}
		}

		public void RequestSuspension()
		{
			this.shouldSuspend = true;
		}

		public void BecomeConfident()
		{
			this.confident = true;
		}

		public bool IsNextCharOnNewLine
		{
			get
			{
				return false;
			}
		}

		public bool IsPrevCR
		{
			get
			{
				return this.lastCR;
			}
		}

		public int Line
		{
			get
			{
				return -1;
			}
		}

		public int Col
		{
			get
			{
				return -1;
			}
		}

		public bool IsInDataState
		{
			get
			{
				return this.stateSave == TokenizerState.DATA;
			}
		}

		public void ResetToDataState()
		{
			this.strBufLen = 0;
			this.longStrBufLen = 0;
			this.stateSave = TokenizerState.DATA;
			this.lastCR = false;
			this.index = 0;
			this.forceQuirks = false;
			this.additional = '\0';
			this.entCol = -1;
			this.firstCharKey = -1;
			this.lo = 0;
			this.hi = 0;
			this.candidate = -1;
			this.strBufMark = 0;
			this.prevValue = -1;
			this.value = 0;
			this.seenDigits = false;
			this.endTag = false;
			this.InitDoctypeFields();
			if (this.tagName != null)
			{
				this.tagName = null;
			}
			if (this.attributeName != null)
			{
				this.attributeName = null;
			}
			if (this.newAttributesEachTime && this.attributes != null)
			{
				this.attributes = null;
			}
		}

		public void LoadState(Tokenizer other)
		{
			this.strBufLen = other.strBufLen;
			if (this.strBufLen > this.strBuf.Length)
			{
				this.strBuf = new char[this.strBufLen];
			}
			Buffer.BlockCopy(other.strBuf, 0, this.strBuf, 0, this.strBufLen << 1);
			this.longStrBufLen = other.longStrBufLen;
			if (this.longStrBufLen > this.longStrBuf.Length)
			{
				this.longStrBuf = new char[this.longStrBufLen];
			}
			Buffer.BlockCopy(other.longStrBuf, 0, this.longStrBuf, 0, this.longStrBufLen << 1);
			this.stateSave = other.stateSave;
			this.returnStateSave = other.returnStateSave;
			this.endTagExpectation = other.endTagExpectation;
			this.endTagExpectationAsArray = other.endTagExpectationAsArray;
			this.lastCR = other.lastCR;
			this.index = other.index;
			this.forceQuirks = other.forceQuirks;
			this.additional = other.additional;
			this.entCol = other.entCol;
			this.firstCharKey = other.firstCharKey;
			this.lo = other.lo;
			this.hi = other.hi;
			this.candidate = other.candidate;
			this.strBufMark = other.strBufMark;
			this.prevValue = other.prevValue;
			this.value = other.value;
			this.seenDigits = other.seenDigits;
			this.endTag = other.endTag;
			this.shouldSuspend = false;
			if (other.doctypeName == null)
			{
				this.doctypeName = null;
			}
			else
			{
				this.doctypeName = other.doctypeName;
			}
			if (other.systemIdentifier == null)
			{
				this.systemIdentifier = null;
			}
			else
			{
				this.systemIdentifier = other.systemIdentifier;
			}
			if (other.publicIdentifier == null)
			{
				this.publicIdentifier = null;
			}
			else
			{
				this.publicIdentifier = other.publicIdentifier;
			}
			if (other.tagName == null)
			{
				this.tagName = null;
			}
			else
			{
				this.tagName = other.tagName.CloneElementName();
			}
			if (other.attributeName == null)
			{
				this.attributeName = null;
			}
			else
			{
				this.attributeName = other.attributeName.CloneAttributeName();
			}
			if (other.attributes == null)
			{
				this.attributes = null;
				return;
			}
			this.attributes = other.attributes.CloneAttributes();
		}

		public void InitializeWithoutStarting()
		{
			this.confident = false;
			this.strBuf = new char[64];
			this.longStrBuf = new char[1024];
			this.line = 1;
			this.html4 = false;
			this.metaBoundaryPassed = false;
			this.wantsComments = this.TokenHandler.WantsComments;
			if (!this.newAttributesEachTime)
			{
				this.attributes = new HtmlAttributes(this.mappingLangToXmlLang);
			}
			this.ResetToDataState();
		}

		protected void ErrGarbageAfterLtSlash()
		{
		}

		protected void ErrLtSlashGt()
		{
		}

		protected void ErrWarnLtSlashInRcdata()
		{
		}

		protected void ErrHtml4LtSlashInRcdata(char folded)
		{
		}

		protected void ErrCharRefLacksSemicolon()
		{
		}

		protected void ErrNoDigitsInNCR()
		{
		}

		protected void ErrGtInSystemId()
		{
		}

		protected void ErrGtInPublicId()
		{
		}

		protected void ErrNamelessDoctype()
		{
		}

		protected void ErrConsecutiveHyphens()
		{
		}

		protected void ErrPrematureEndOfComment()
		{
		}

		protected void ErrBogusComment()
		{
		}

		protected void ErrUnquotedAttributeValOrNull(char c)
		{
		}

		protected void ErrSlashNotFollowedByGt()
		{
		}

		protected void ErrHtml4XmlVoidSyntax()
		{
		}

		protected void ErrNoSpaceBetweenAttributes()
		{
		}

		protected void ErrHtml4NonNameInUnquotedAttribute(char c)
		{
		}

		protected void ErrLtOrEqualsOrGraveInUnquotedAttributeOrNull(char c)
		{
		}

		protected void ErrAttributeValueMissing()
		{
		}

		protected void ErrBadCharBeforeAttributeNameOrNull(char c)
		{
		}

		protected void ErrEqualsSignBeforeAttributeName()
		{
		}

		protected void ErrBadCharAfterLt(char c)
		{
		}

		protected void ErrLtGt()
		{
		}

		protected void ErrProcessingInstruction()
		{
		}

		protected void ErrUnescapedAmpersandInterpretedAsCharacterReference()
		{
		}

		protected void ErrNotSemicolonTerminated()
		{
		}

		protected void ErrNoNamedCharacterMatch()
		{
		}

		protected void ErrQuoteBeforeAttributeName(char c)
		{
		}

		protected void ErrQuoteOrLtInAttributeNameOrNull(char c)
		{
		}

		protected void ErrExpectedPublicId()
		{
		}

		protected void ErrBogusDoctype()
		{
		}

		protected void MaybeWarnPrivateUseAstral()
		{
		}

		protected void MaybeWarnPrivateUse(char ch)
		{
		}

		protected void MaybeErrAttributesOnEndTag(HtmlAttributes attrs)
		{
		}

		protected void MaybeErrSlashInEndTag(bool selfClosing)
		{
		}

		protected char ErrNcrNonCharacter(char ch)
		{
			return ch;
		}

		protected void ErrAstralNonCharacter(int ch)
		{
		}

		protected void ErrNcrSurrogate()
		{
		}

		protected char ErrNcrControlChar(char ch)
		{
			return ch;
		}

		protected void ErrNcrCr()
		{
		}

		protected void ErrNcrInC1Range()
		{
		}

		protected void ErrEofInPublicId()
		{
		}

		protected void ErrEofInComment()
		{
		}

		protected void ErrEofInDoctype()
		{
		}

		protected void ErrEofInAttributeValue()
		{
		}

		protected void ErrEofInAttributeName()
		{
		}

		protected void ErrEofWithoutGt()
		{
		}

		protected void ErrEofInTagName()
		{
		}

		protected void ErrEofInEndTag()
		{
		}

		protected void ErrEofAfterLt()
		{
		}

		protected void ErrNcrOutOfRange()
		{
		}

		protected void ErrNcrUnassigned()
		{
		}

		protected void ErrDuplicateAttribute()
		{
		}

		protected void ErrEofInSystemId()
		{
		}

		protected void ErrExpectedSystemId()
		{
		}

		protected void ErrMissingSpaceBeforeDoctypeName()
		{
		}

		protected void ErrHyphenHyphenBang()
		{
		}

		protected void ErrNcrControlChar()
		{
		}

		protected void ErrNcrZero()
		{
		}

		protected void ErrNoSpaceBetweenDoctypeSystemKeywordAndQuote()
		{
		}

		protected void ErrNoSpaceBetweenPublicAndSystemIds()
		{
		}

		protected void ErrNoSpaceBetweenDoctypePublicKeywordAndQuote()
		{
		}

		protected void NoteAttributeWithoutValue()
		{
		}

		protected void NoteUnquotedAttributeValue()
		{
		}

		public void SetTransitionBaseOffset(int offset)
		{
		}

		public bool IsSuspended
		{
			get
			{
				return this.shouldSuspend;
			}
		}

		const byte DATA_AND_RCDATA_MASK = 240;

		const int LEAD_OFFSET = 55232;

		const int BUFFER_GROW_BY = 1024;

		static readonly char[] LT_GT = new char[] { '<', '>' };

		static readonly char[] LT_SOLIDUS = new char[] { '<', '/' };

		static readonly char[] RSQB_RSQB = new char[] { ']', ']' };

		static readonly char[] REPLACEMENT_CHARACTER = new char[] { '\ufffd' };

		static readonly char[] SPACE = new char[] { ' ' };

		static readonly char[] LF = new char[] { '\n' };

		static readonly char[] CDATA_LSQB = "CDATA[".ToCharArray();

		static readonly char[] OCTYPE = "octype".ToCharArray();

		static readonly char[] UBLIC = "ublic".ToCharArray();

		static readonly char[] YSTEM = "ystem".ToCharArray();

		static readonly char[] TITLE_ARR = new char[] { 't', 'i', 't', 'l', 'e' };

		static readonly char[] SCRIPT_ARR = new char[] { 's', 'c', 'r', 'i', 'p', 't' };

		static readonly char[] STYLE_ARR = new char[] { 's', 't', 'y', 'l', 'e' };

		static readonly char[] PLAINTEXT_ARR = new char[] { 'p', 'l', 'a', 'i', 'n', 't', 'e', 'x', 't' };

		static readonly char[] XMP_ARR = new char[] { 'x', 'm', 'p' };

		static readonly char[] TEXTAREA_ARR = new char[] { 't', 'e', 'x', 't', 'a', 'r', 'e', 'a' };

		static readonly char[] IFRAME_ARR = new char[] { 'i', 'f', 'r', 'a', 'm', 'e' };

		static readonly char[] NOEMBED_ARR = new char[] { 'n', 'o', 'e', 'm', 'b', 'e', 'd' };

		static readonly char[] NOSCRIPT_ARR = new char[] { 'n', 'o', 's', 'c', 'r', 'i', 'p', 't' };

		static readonly char[] NOFRAMES_ARR = new char[] { 'n', 'o', 'f', 'r', 'a', 'm', 'e', 's' };

		protected bool lastCR;

		protected TokenizerState stateSave;

		TokenizerState returnStateSave;

		protected int index;

		bool forceQuirks;

		char additional;

		int entCol;

		int firstCharKey;

		int lo;

		int hi;

		int candidate;

		int strBufMark;

		int prevValue;

		protected int value;

		bool seenDigits;

		protected int cstart;

		char[] strBuf;

		int strBufLen;

		char[] longStrBuf;

		int longStrBufLen;

		readonly char[] bmpChar;

		readonly char[] astralChar;

		protected ElementName endTagExpectation;

		char[] endTagExpectationAsArray;

		protected bool endTag;

		ElementName tagName;

		protected AttributeName attributeName;

		bool wantsComments;

		protected bool html4;

		bool metaBoundaryPassed;

		[Local]
		string doctypeName;

		string publicIdentifier;

		string systemIdentifier;

		HtmlAttributes attributes;

		XmlViolationPolicy contentSpacePolicy = XmlViolationPolicy.AlterInfoset;

		XmlViolationPolicy commentPolicy = XmlViolationPolicy.AlterInfoset;

		XmlViolationPolicy xmlnsPolicy = XmlViolationPolicy.AlterInfoset;

		XmlViolationPolicy namePolicy = XmlViolationPolicy.AlterInfoset;

		bool html4ModeCompatibleWithXhtml1Schemata;

		readonly bool newAttributesEachTime;

		int mappingLangToXmlLang;

		bool shouldSuspend;

		protected bool confident;

		int line;

		protected Locator ampersandLocation;
	}
}
