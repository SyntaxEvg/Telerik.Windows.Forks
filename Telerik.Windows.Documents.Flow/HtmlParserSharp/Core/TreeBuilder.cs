using System;
using System.Collections.Generic;
using System.Text;
using HtmlParserSharp.Common;

namespace HtmlParserSharp.Core
{
	abstract class TreeBuilder<T> : ITokenHandler, ITreeBuilderState<T> where T : class
	{
		public event EventHandler<DocumentModeEventArgs> DocumentModeDetected;

		public DoctypeExpectation DoctypeExpectation { get; set; }

		public bool IsScriptingEnabled { get; set; }

		protected int charBufferLen
		{
			get
			{
				return this.charBuffer.Length;
			}
		}

		public bool IsReportingDoctype { get; set; }

		public XmlViolationPolicy NamePolicy { get; set; }

		protected TreeBuilder()
		{
			this.fragment = false;
			this.IsReportingDoctype = true;
			this.DoctypeExpectation = DoctypeExpectation.Html;
			this.NamePolicy = XmlViolationPolicy.AlterInfoset;
			this.IsScriptingEnabled = false;
		}

		protected void Fatal()
		{
		}

		protected void Fatal(Exception e)
		{
			throw e;
		}

		internal void Fatal(string s)
		{
			throw new Exception(s);
		}

		public event EventHandler<ParserErrorEventArgs> ErrorEvent;

		void Err(string message)
		{
			if (this.ErrorEvent != null)
			{
				this.ErrNoCheck(message);
			}
		}

		void ErrNoCheck(string message)
		{
			this.ErrorEvent(this, new ParserErrorEventArgs(message, false));
		}

		void ErrStrayStartTag(string name)
		{
			this.Err("Stray end tag “" + name + "”.");
		}

		void ErrStrayEndTag(string name)
		{
			this.Err("Stray end tag “" + name + "”.");
		}

		void ErrUnclosedElements(int eltPos, string name)
		{
			this.Err("End tag “" + name + "” seen, but there were open elements.");
			this.ErrListUnclosedStartTags(eltPos);
		}

		void ErrUnclosedElementsImplied(int eltPos, string name)
		{
			this.Err("End tag “" + name + "” implied, but there were open elements.");
			this.ErrListUnclosedStartTags(eltPos);
		}

		void ErrUnclosedElementsCell(int eltPos)
		{
			this.Err("A table cell was implicitly closed, but there were open elements.");
			this.ErrListUnclosedStartTags(eltPos);
		}

		void ErrListUnclosedStartTags(int eltPos)
		{
			if (this.currentPtr != -1)
			{
				for (int i = this.currentPtr; i > eltPos; i--)
				{
					this.ReportUnclosedElementNameAndLocation(i);
				}
			}
		}

		void ErrEndWithUnclosedElements(string message)
		{
			if (this.ErrorEvent == null)
			{
				return;
			}
			this.ErrNoCheck(message);
			this.ErrListUnclosedStartTags(0);
		}

		void ReportUnclosedElementNameAndLocation(int pos)
		{
			StackNode<T> stackNode = this.stack[pos];
			if (stackNode.IsOptionalEndTag)
			{
				return;
			}
			TaintableLocator locator = stackNode.Locator;
			if (locator.IsTainted)
			{
				return;
			}
			locator.MarkTainted();
			this.ErrNoCheck("Unclosed element “" + stackNode.popName + "”.");
		}

		internal void Warn(string message)
		{
			if (this.ErrorEvent != null)
			{
				this.ErrorEvent(this, new ParserErrorEventArgs(message, true));
			}
		}

		public void StartTokenization(Tokenizer self)
		{
			this.tokenizer = self;
			this.stack = new StackNode<T>[64];
			this.listOfActiveFormattingElements = new StackNode<T>[64];
			this.needToDropLF = false;
			this.originalMode = InsertionMode.INITIAL;
			this.currentPtr = -1;
			this.listPtr = -1;
			this.formPointer = default(T);
			this.headPointer = default(T);
			this.deepTreeSurrogateParent = default(T);
			this.html4 = false;
			this.idLocations.Clear();
			this.Start(this.fragment);
			this.charBuffer = new StringBuilder();
			this.charBuffer.Clear();
			this.framesetOk = true;
			if (this.fragment)
			{
				T node;
				if (this.contextNode != null)
				{
					node = this.contextNode;
				}
				else
				{
					node = this.CreateHtmlElementSetAsRoot(this.tokenizer.EmptyAttributes());
				}
				StackNode<T> stackNode = new StackNode<T>(ElementName.HTML, node, (this.ErrorEvent == null) ? null : new TaintableLocator(this.tokenizer));
				this.currentPtr++;
				this.stack[this.currentPtr] = stackNode;
				this.ResetTheInsertionMode();
				if ("title" == this.contextName || "textarea" == this.contextName)
				{
					this.tokenizer.SetStateAndEndTagExpectation(TokenizerState.RCDATA, this.contextName);
				}
				else if ("style" == this.contextName || "xmp" == this.contextName || "iframe" == this.contextName || "noembed" == this.contextName || "noframes" == this.contextName || (this.IsScriptingEnabled && "noscript" == this.contextName))
				{
					this.tokenizer.SetStateAndEndTagExpectation(TokenizerState.RAWTEXT, this.contextName);
				}
				else if ("plaintext" == this.contextName)
				{
					this.tokenizer.SetStateAndEndTagExpectation(TokenizerState.PLAINTEXT, this.contextName);
				}
				else if ("script" == this.contextName)
				{
					this.tokenizer.SetStateAndEndTagExpectation(TokenizerState.SCRIPT_DATA, this.contextName);
				}
				else
				{
					this.tokenizer.SetStateAndEndTagExpectation(TokenizerState.DATA, this.contextName);
				}
				this.contextName = null;
				this.contextNode = default(T);
				return;
			}
			this.mode = InsertionMode.INITIAL;
		}

		public void Doctype([Local] string name, string publicIdentifier, string systemIdentifier, bool forceQuirks)
		{
			this.needToDropLF = false;
			if (!this.IsInForeign)
			{
				InsertionMode insertionMode = this.mode;
				if (insertionMode == InsertionMode.INITIAL)
				{
					if (this.IsReportingDoctype)
					{
						this.AppendDoctypeToDocument((name == null) ? "" : name, (publicIdentifier == null) ? string.Empty : publicIdentifier, (systemIdentifier == null) ? string.Empty : systemIdentifier);
					}
					switch (this.DoctypeExpectation)
					{
					case DoctypeExpectation.Html:
						if (this.IsQuirky(name, publicIdentifier, systemIdentifier, forceQuirks))
						{
							this.Err("Quirky doctype. Expected “<!DOCTYPE html>”.");
							this.DocumentModeInternal(DocumentMode.QuirksMode, publicIdentifier, systemIdentifier, false);
						}
						else if (this.IsAlmostStandards(publicIdentifier, systemIdentifier))
						{
							this.Err("Almost standards mode doctype. Expected “<!DOCTYPE html>”.");
							this.DocumentModeInternal(DocumentMode.AlmostStandardsMode, publicIdentifier, systemIdentifier, false);
						}
						else
						{
							if (("-//W3C//DTD HTML 4.0//EN" == publicIdentifier && (systemIdentifier == null || "http://www.w3.org/TR/REC-html40/strict.dtd" == systemIdentifier)) || ("-//W3C//DTD HTML 4.01//EN" == publicIdentifier && (systemIdentifier == null || "http://www.w3.org/TR/html4/strict.dtd" == systemIdentifier)) || ("-//W3C//DTD XHTML 1.0 Strict//EN" == publicIdentifier && "http://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd" == systemIdentifier) || ("-//W3C//DTD XHTML 1.1//EN" == publicIdentifier && "http://www.w3.org/TR/xhtml11/DTD/xhtml11.dtd" == systemIdentifier))
							{
								this.Warn("Obsolete doctype. Expected “<!DOCTYPE html>”.");
							}
							else if ((systemIdentifier != null && !("about:legacy-compat" == systemIdentifier)) || publicIdentifier != null)
							{
								this.Err("Legacy doctype. Expected “<!DOCTYPE html>”.");
							}
							this.DocumentModeInternal(DocumentMode.StandardsMode, publicIdentifier, systemIdentifier, false);
						}
						break;
					case DoctypeExpectation.Html401Transitional:
						this.html4 = true;
						this.tokenizer.TurnOnAdditionalHtml4Errors();
						if (this.IsQuirky(name, publicIdentifier, systemIdentifier, forceQuirks))
						{
							this.Err("Quirky doctype. Expected “<!DOCTYPE HTML PUBLIC \"-//W3C//DTD HTML 4.01 Transitional//EN\" \"http://www.w3.org/TR/html4/loose.dtd\">”.");
							this.DocumentModeInternal(DocumentMode.QuirksMode, publicIdentifier, systemIdentifier, true);
						}
						else if (this.IsAlmostStandards(publicIdentifier, systemIdentifier))
						{
							if ("-//W3C//DTD HTML 4.01 Transitional//EN" == publicIdentifier && systemIdentifier != null)
							{
								if ("http://www.w3.org/TR/html4/loose.dtd" != systemIdentifier)
								{
									this.Warn("The doctype did not contain the system identifier prescribed by the HTML 4.01 specification. Expected “<!DOCTYPE HTML PUBLIC \"-//W3C//DTD HTML 4.01 Transitional//EN\" \"http://www.w3.org/TR/html4/loose.dtd\">”.");
								}
							}
							else
							{
								this.Err("The doctype was not a non-quirky HTML 4.01 Transitional doctype. Expected “<!DOCTYPE HTML PUBLIC \"-//W3C//DTD HTML 4.01 Transitional//EN\" \"http://www.w3.org/TR/html4/loose.dtd\">”.");
							}
							this.DocumentModeInternal(DocumentMode.AlmostStandardsMode, publicIdentifier, systemIdentifier, true);
						}
						else
						{
							this.Err("The doctype was not the HTML 4.01 Transitional doctype. Expected “<!DOCTYPE HTML PUBLIC \"-//W3C//DTD HTML 4.01 Transitional//EN\" \"http://www.w3.org/TR/html4/loose.dtd\">”.");
							this.DocumentModeInternal(DocumentMode.StandardsMode, publicIdentifier, systemIdentifier, true);
						}
						break;
					case DoctypeExpectation.Html401Strict:
						this.html4 = true;
						this.tokenizer.TurnOnAdditionalHtml4Errors();
						if (this.IsQuirky(name, publicIdentifier, systemIdentifier, forceQuirks))
						{
							this.Err("Quirky doctype. Expected “<!DOCTYPE HTML PUBLIC \"-//W3C//DTD HTML 4.01//EN\" \"http://www.w3.org/TR/html4/strict.dtd\">”.");
							this.DocumentModeInternal(DocumentMode.QuirksMode, publicIdentifier, systemIdentifier, true);
						}
						else if (this.IsAlmostStandards(publicIdentifier, systemIdentifier))
						{
							this.Err("Almost standards mode doctype. Expected “<!DOCTYPE HTML PUBLIC \"-//W3C//DTD HTML 4.01//EN\" \"http://www.w3.org/TR/html4/strict.dtd\">”.");
							this.DocumentModeInternal(DocumentMode.AlmostStandardsMode, publicIdentifier, systemIdentifier, true);
						}
						else
						{
							if ("-//W3C//DTD HTML 4.01//EN" == publicIdentifier)
							{
								if ("http://www.w3.org/TR/html4/strict.dtd" != systemIdentifier)
								{
									this.Warn("The doctype did not contain the system identifier prescribed by the HTML 4.01 specification. Expected “<!DOCTYPE HTML PUBLIC \"-//W3C//DTD HTML 4.01//EN\" \"http://www.w3.org/TR/html4/strict.dtd\">”.");
								}
							}
							else
							{
								this.Err("The doctype was not the HTML 4.01 Strict doctype. Expected “<!DOCTYPE HTML PUBLIC \"-//W3C//DTD HTML 4.01//EN\" \"http://www.w3.org/TR/html4/strict.dtd\">”.");
							}
							this.DocumentModeInternal(DocumentMode.StandardsMode, publicIdentifier, systemIdentifier, true);
						}
						break;
					case DoctypeExpectation.Auto:
						this.html4 = this.IsHtml4Doctype(publicIdentifier);
						if (this.html4)
						{
							this.tokenizer.TurnOnAdditionalHtml4Errors();
						}
						if (this.IsQuirky(name, publicIdentifier, systemIdentifier, forceQuirks))
						{
							this.Err("Quirky doctype. Expected e.g. “<!DOCTYPE html>”.");
							this.DocumentModeInternal(DocumentMode.QuirksMode, publicIdentifier, systemIdentifier, this.html4);
						}
						else if (this.IsAlmostStandards(publicIdentifier, systemIdentifier))
						{
							if ("-//W3C//DTD HTML 4.01 Transitional//EN" == publicIdentifier)
							{
								if ("http://www.w3.org/TR/html4/loose.dtd" != systemIdentifier)
								{
									this.Warn("The doctype did not contain the system identifier prescribed by the HTML 4.01 specification. Expected “<!DOCTYPE HTML PUBLIC \"-//W3C//DTD HTML 4.01 Transitional//EN\" \"http://www.w3.org/TR/html4/loose.dtd\">”.");
								}
							}
							else
							{
								this.Err("Almost standards mode doctype. Expected e.g. “<!DOCTYPE html>”.");
							}
							this.DocumentModeInternal(DocumentMode.AlmostStandardsMode, publicIdentifier, systemIdentifier, this.html4);
						}
						else
						{
							if ("-//W3C//DTD HTML 4.01//EN" == publicIdentifier)
							{
								if ("http://www.w3.org/TR/html4/strict.dtd" != systemIdentifier)
								{
									this.Warn("The doctype did not contain the system identifier prescribed by the HTML 4.01 specification. Expected “<!DOCTYPE HTML PUBLIC \"-//W3C//DTD HTML 4.01//EN\" \"http://www.w3.org/TR/html4/strict.dtd\">”.");
								}
							}
							else if (publicIdentifier != null || systemIdentifier != null)
							{
								this.Err("Legacy doctype. Expected e.g. “<!DOCTYPE html>”.");
							}
							this.DocumentModeInternal(DocumentMode.StandardsMode, publicIdentifier, systemIdentifier, this.html4);
						}
						break;
					case DoctypeExpectation.NoDoctypeErrors:
						if (this.IsQuirky(name, publicIdentifier, systemIdentifier, forceQuirks))
						{
							this.DocumentModeInternal(DocumentMode.QuirksMode, publicIdentifier, systemIdentifier, false);
						}
						else if (this.IsAlmostStandards(publicIdentifier, systemIdentifier))
						{
							this.DocumentModeInternal(DocumentMode.AlmostStandardsMode, publicIdentifier, systemIdentifier, false);
						}
						else
						{
							this.DocumentModeInternal(DocumentMode.StandardsMode, publicIdentifier, systemIdentifier, false);
						}
						break;
					}
					this.mode = InsertionMode.BEFORE_HTML;
					return;
				}
			}
			this.Err("Stray doctype.");
		}

		bool IsHtml4Doctype(string publicIdentifier)
		{
			return publicIdentifier != null && Array.BinarySearch<string>(TreeBuilderConstants.HTML4_PUBLIC_IDS, publicIdentifier) > -1;
		}

		public void Comment(char[] buf, int start, int length)
		{
			this.needToDropLF = false;
			if (!this.WantsComments)
			{
				return;
			}
			if (!this.IsInForeign)
			{
				InsertionMode insertionMode = this.mode;
				switch (insertionMode)
				{
				case InsertionMode.INITIAL:
				case InsertionMode.BEFORE_HTML:
					break;
				default:
					switch (insertionMode)
					{
					case InsertionMode.AFTER_BODY:
						this.FlushCharacters();
						this.AppendComment(this.stack[0].node, buf, start, length);
						return;
					case InsertionMode.IN_FRAMESET:
					case InsertionMode.AFTER_FRAMESET:
						goto IL_73;
					case InsertionMode.AFTER_AFTER_BODY:
					case InsertionMode.AFTER_AFTER_FRAMESET:
						break;
					default:
						goto IL_73;
					}
					break;
				}
				this.AppendCommentToDocument(buf, start, length);
				return;
			}
			IL_73:
			this.FlushCharacters();
			this.AppendComment(this.stack[this.currentPtr].node, buf, start, length);
		}

		public void Characters(char[] buf, int start, int length)
		{
			if (this.needToDropLF)
			{
				this.needToDropLF = false;
				if (buf[start] == '\n')
				{
					start++;
					length--;
					if (length == 0)
					{
						return;
					}
				}
			}
			InsertionMode insertionMode = this.mode;
			switch (insertionMode)
			{
			case InsertionMode.IN_BODY:
			case InsertionMode.IN_CAPTION:
			case InsertionMode.IN_CELL:
				if (!this.IsInForeignButNotHtmlOrMathTextIntegrationPoint)
				{
					this.ReconstructTheActiveFormattingElements();
				}
				break;
			case InsertionMode.IN_TABLE:
			case InsertionMode.IN_TABLE_BODY:
			case InsertionMode.IN_ROW:
				this.AccumulateCharactersForced(buf, start, length);
				return;
			case InsertionMode.IN_COLUMN_GROUP:
				goto IL_78;
			default:
				if (insertionMode != InsertionMode.TEXT)
				{
					goto IL_78;
				}
				break;
			}
			this.AccumulateCharacters(buf, start, length);
			return;
			IL_78:
			int num = start + length;
			int i = start;
			while (i < num)
			{
				char c = buf[i];
				switch (c)
				{
				case '\t':
				case '\n':
				case '\f':
				case '\r':
					goto IL_AC;
				case '\v':
					goto IL_183;
				default:
					if (c == ' ')
					{
						goto IL_AC;
					}
					goto IL_183;
				}
				IL_492:
				i++;
				continue;
				IL_AC:
				switch (this.mode)
				{
				case InsertionMode.INITIAL:
				case InsertionMode.BEFORE_HTML:
				case InsertionMode.BEFORE_HEAD:
					start = i + 1;
					goto IL_492;
				case InsertionMode.IN_HEAD:
				case InsertionMode.IN_HEAD_NOSCRIPT:
				case InsertionMode.AFTER_HEAD:
				case InsertionMode.IN_COLUMN_GROUP:
				case InsertionMode.IN_SELECT:
				case InsertionMode.IN_SELECT_IN_TABLE:
				case InsertionMode.IN_FRAMESET:
				case InsertionMode.AFTER_FRAMESET:
					goto IL_492;
				case InsertionMode.IN_BODY:
				case InsertionMode.IN_CAPTION:
				case InsertionMode.IN_CELL:
				case InsertionMode.FRAMESET_OK:
					if (start < i)
					{
						this.AccumulateCharacters(buf, start, i - start);
						start = i;
					}
					if (!this.IsInForeignButNotHtmlOrMathTextIntegrationPoint)
					{
						this.FlushCharacters();
						this.ReconstructTheActiveFormattingElements();
						goto IL_492;
					}
					goto IL_492;
				case InsertionMode.IN_TABLE:
				case InsertionMode.IN_TABLE_BODY:
				case InsertionMode.IN_ROW:
					this.AccumulateCharactersForced(buf, i, 1);
					start = i + 1;
					goto IL_492;
				case InsertionMode.AFTER_BODY:
				case InsertionMode.AFTER_AFTER_BODY:
				case InsertionMode.AFTER_AFTER_FRAMESET:
					if (start < i)
					{
						this.AccumulateCharacters(buf, start, i - start);
						start = i;
					}
					this.FlushCharacters();
					this.ReconstructTheActiveFormattingElements();
					goto IL_492;
				}
				IL_183:
				switch (this.mode)
				{
				case InsertionMode.INITIAL:
					switch (this.DoctypeExpectation)
					{
					case DoctypeExpectation.Html:
						this.Err("Non-space characters found without seeing a doctype first. Expected “<!DOCTYPE html>”.");
						break;
					case DoctypeExpectation.Html401Transitional:
						this.Err("Non-space characters found without seeing a doctype first. Expected “<!DOCTYPE HTML PUBLIC \"-//W3C//DTD HTML 4.01 Transitional//EN\" \"http://www.w3.org/TR/html4/loose.dtd\">”.");
						break;
					case DoctypeExpectation.Html401Strict:
						this.Err("Non-space characters found without seeing a doctype first. Expected “<!DOCTYPE HTML PUBLIC \"-//W3C//DTD HTML 4.01//EN\" \"http://www.w3.org/TR/html4/strict.dtd\">”.");
						break;
					case DoctypeExpectation.Auto:
						this.Err("Non-space characters found without seeing a doctype first. Expected e.g. “<!DOCTYPE html>”.");
						break;
					}
					this.DocumentModeInternal(DocumentMode.QuirksMode, null, null, false);
					this.mode = InsertionMode.BEFORE_HTML;
					i--;
					goto IL_492;
				case InsertionMode.BEFORE_HTML:
					this.AppendHtmlElementToDocumentAndPush();
					this.mode = InsertionMode.BEFORE_HEAD;
					i--;
					goto IL_492;
				case InsertionMode.BEFORE_HEAD:
					if (start < i)
					{
						this.AccumulateCharacters(buf, start, i - start);
						start = i;
					}
					this.FlushCharacters();
					this.AppendToCurrentNodeAndPushHeadElement(HtmlAttributes.EMPTY_ATTRIBUTES);
					this.mode = InsertionMode.IN_HEAD;
					i--;
					goto IL_492;
				case InsertionMode.IN_HEAD:
					if (start < i)
					{
						this.AccumulateCharacters(buf, start, i - start);
						start = i;
					}
					this.FlushCharacters();
					this.Pop();
					this.mode = InsertionMode.AFTER_HEAD;
					i--;
					goto IL_492;
				case InsertionMode.IN_HEAD_NOSCRIPT:
					if (start < i)
					{
						this.AccumulateCharacters(buf, start, i - start);
						start = i;
					}
					this.Err("Non-space character inside “noscript” inside “head”.");
					this.FlushCharacters();
					this.Pop();
					this.mode = InsertionMode.IN_HEAD;
					i--;
					goto IL_492;
				case InsertionMode.AFTER_HEAD:
					if (start < i)
					{
						this.AccumulateCharacters(buf, start, i - start);
						start = i;
					}
					this.FlushCharacters();
					this.AppendToCurrentNodeAndPushBodyElement();
					this.mode = InsertionMode.FRAMESET_OK;
					i--;
					goto IL_492;
				case InsertionMode.IN_BODY:
				case InsertionMode.IN_CAPTION:
				case InsertionMode.IN_CELL:
					if (start < i)
					{
						this.AccumulateCharacters(buf, start, i - start);
						start = i;
					}
					if (!this.IsInForeignButNotHtmlOrMathTextIntegrationPoint)
					{
						this.FlushCharacters();
						this.ReconstructTheActiveFormattingElements();
						goto IL_492;
					}
					goto IL_492;
				case InsertionMode.IN_TABLE:
				case InsertionMode.IN_TABLE_BODY:
				case InsertionMode.IN_ROW:
					this.AccumulateCharactersForced(buf, i, 1);
					start = i + 1;
					goto IL_492;
				case InsertionMode.IN_COLUMN_GROUP:
					if (start < i)
					{
						this.AccumulateCharacters(buf, start, i - start);
						start = i;
					}
					if (this.currentPtr == 0)
					{
						this.Err("Non-space in “colgroup” when parsing fragment.");
						start = i + 1;
						goto IL_492;
					}
					this.FlushCharacters();
					this.Pop();
					this.mode = InsertionMode.IN_TABLE;
					i--;
					goto IL_492;
				case InsertionMode.IN_SELECT:
				case InsertionMode.IN_SELECT_IN_TABLE:
				case InsertionMode.TEXT:
					goto IL_492;
				case InsertionMode.AFTER_BODY:
					this.Err("Non-space character after body.");
					this.Fatal();
					this.mode = (this.framesetOk ? InsertionMode.FRAMESET_OK : InsertionMode.IN_BODY);
					i--;
					goto IL_492;
				case InsertionMode.IN_FRAMESET:
					if (start < i)
					{
						this.AccumulateCharacters(buf, start, i - start);
						start = i;
					}
					this.Err("Non-space in “frameset”.");
					start = i + 1;
					goto IL_492;
				case InsertionMode.AFTER_FRAMESET:
					if (start < i)
					{
						this.AccumulateCharacters(buf, start, i - start);
						start = i;
					}
					this.Err("Non-space after “frameset”.");
					start = i + 1;
					goto IL_492;
				case InsertionMode.AFTER_AFTER_BODY:
					this.Err("Non-space character in page trailer.");
					this.mode = (this.framesetOk ? InsertionMode.FRAMESET_OK : InsertionMode.IN_BODY);
					i--;
					goto IL_492;
				case InsertionMode.AFTER_AFTER_FRAMESET:
					this.Err("Non-space character in page trailer.");
					this.mode = InsertionMode.IN_FRAMESET;
					i--;
					goto IL_492;
				case InsertionMode.FRAMESET_OK:
					this.framesetOk = false;
					this.mode = InsertionMode.IN_BODY;
					i--;
					goto IL_492;
				default:
					goto IL_492;
				}
			}
			if (start < num)
			{
				this.AccumulateCharacters(buf, start, num - start);
			}
		}

		public void ZeroOriginatingReplacementCharacter()
		{
			if (this.mode == InsertionMode.TEXT)
			{
				this.AccumulateCharacters(TreeBuilderConstants.REPLACEMENT_CHARACTER, 0, 1);
				return;
			}
			if (this.currentPtr >= 0)
			{
				if (this.IsSpecialParentInForeign(this.stack[this.currentPtr]))
				{
					return;
				}
				this.AccumulateCharacters(TreeBuilderConstants.REPLACEMENT_CHARACTER, 0, 1);
			}
		}

		public void Eof()
		{
			this.FlushCharacters();
			for (;;)
			{
				if (!this.IsInForeign)
				{
					switch (this.mode)
					{
					case InsertionMode.INITIAL:
						switch (this.DoctypeExpectation)
						{
						case DoctypeExpectation.Html:
							this.Err("End of file seen without seeing a doctype first. Expected “<!DOCTYPE html>”.");
							break;
						case DoctypeExpectation.Html401Transitional:
							this.Err("End of file seen without seeing a doctype first. Expected “<!DOCTYPE HTML PUBLIC \"-//W3C//DTD HTML 4.01 Transitional//EN\" \"http://www.w3.org/TR/html4/loose.dtd\">”.");
							break;
						case DoctypeExpectation.Html401Strict:
							this.Err("End of file seen without seeing a doctype first. Expected “<!DOCTYPE HTML PUBLIC \"-//W3C//DTD HTML 4.01//EN\" \"http://www.w3.org/TR/html4/strict.dtd\">”.");
							break;
						case DoctypeExpectation.Auto:
							this.Err("End of file seen without seeing a doctype first. Expected e.g. “<!DOCTYPE html>”.");
							break;
						}
						this.DocumentModeInternal(DocumentMode.QuirksMode, null, null, false);
						this.mode = InsertionMode.BEFORE_HTML;
						continue;
					case InsertionMode.BEFORE_HTML:
						this.AppendHtmlElementToDocumentAndPush();
						this.mode = InsertionMode.BEFORE_HEAD;
						continue;
					case InsertionMode.BEFORE_HEAD:
						this.AppendToCurrentNodeAndPushHeadElement(HtmlAttributes.EMPTY_ATTRIBUTES);
						this.mode = InsertionMode.IN_HEAD;
						continue;
					case InsertionMode.IN_HEAD:
						if (this.ErrorEvent != null && this.currentPtr > 1)
						{
							this.ErrEndWithUnclosedElements("End of file seen and there were open elements.");
						}
						while (this.currentPtr > 0)
						{
							this.PopOnEof();
						}
						this.mode = InsertionMode.AFTER_HEAD;
						continue;
					case InsertionMode.IN_HEAD_NOSCRIPT:
						this.ErrEndWithUnclosedElements("End of file seen and there were open elements.");
						while (this.currentPtr > 1)
						{
							this.PopOnEof();
						}
						this.mode = InsertionMode.IN_HEAD;
						continue;
					case InsertionMode.AFTER_HEAD:
						this.AppendToCurrentNodeAndPushBodyElement();
						this.mode = InsertionMode.IN_BODY;
						continue;
					case InsertionMode.IN_BODY:
					case InsertionMode.IN_CAPTION:
					case InsertionMode.IN_CELL:
					case InsertionMode.FRAMESET_OK:
						goto IL_1A9;
					case InsertionMode.IN_TABLE:
					case InsertionMode.IN_TABLE_BODY:
					case InsertionMode.IN_ROW:
					case InsertionMode.IN_SELECT:
					case InsertionMode.IN_SELECT_IN_TABLE:
					case InsertionMode.IN_FRAMESET:
						goto IL_24F;
					case InsertionMode.IN_COLUMN_GROUP:
						if (this.currentPtr != 0)
						{
							this.PopOnEof();
							this.mode = InsertionMode.IN_TABLE;
							continue;
						}
						break;
					case InsertionMode.TEXT:
						if (this.ErrorEvent != null)
						{
							this.Err("End of file seen when expecting text or an end tag.");
							this.ErrListUnclosedStartTags(0);
						}
						if (this.originalMode == InsertionMode.AFTER_HEAD)
						{
							this.PopOnEof();
						}
						this.PopOnEof();
						this.mode = this.originalMode;
						continue;
					}
					break;
				}
				this.Err("End of file in a foreign namespace context.");
			}
			IL_273:
			while (this.currentPtr > 0)
			{
				this.PopOnEof();
			}
			if (!this.fragment)
			{
				this.PopOnEof();
			}
			return;
			goto IL_273;
			IL_1A9:
			int i = this.currentPtr;
			while (i >= 0)
			{
				DispatchGroup group = this.stack[i].Group;
				DispatchGroup dispatchGroup = group;
				if (dispatchGroup <= DispatchGroup.LI)
				{
					if (dispatchGroup != DispatchGroup.BODY && dispatchGroup != DispatchGroup.LI)
					{
						goto IL_1F8;
					}
				}
				else if (dispatchGroup != DispatchGroup.HTML && dispatchGroup != DispatchGroup.P)
				{
					switch (dispatchGroup)
					{
					case DispatchGroup.TBODY_OR_THEAD_OR_TFOOT:
					case DispatchGroup.TD_OR_TH:
					case DispatchGroup.DD_OR_DT:
						goto IL_205;
					}
					goto IL_1F8;
				}
				IL_205:
				i--;
				continue;
				IL_1F8:
				this.ErrEndWithUnclosedElements("End of file seen and there were open elements.");
				break;
			}
			goto IL_273;
			IL_24F:
			if (this.ErrorEvent != null && this.currentPtr > 0)
			{
				this.ErrEndWithUnclosedElements("End of file seen and there were open elements.");
				goto IL_273;
			}
			goto IL_273;
		}

		public void EndTokenization()
		{
			this.formPointer = default(T);
			this.headPointer = default(T);
			this.deepTreeSurrogateParent = default(T);
			if (this.stack != null)
			{
				while (this.currentPtr > -1)
				{
					this.currentPtr--;
				}
				this.stack = null;
			}
			if (this.listOfActiveFormattingElements != null)
			{
				while (this.listPtr > -1)
				{
					this.listPtr--;
				}
				this.listOfActiveFormattingElements = null;
			}
			this.idLocations.Clear();
			this.charBuffer = null;
			this.End();
		}

		public void StartTag(ElementName elementName, HtmlAttributes attributes, bool selfClosing)
		{
			this.FlushCharacters();
			if (this.ErrorEvent != null)
			{
				string id = attributes.Id;
				if (id != null)
				{
					Locator locator;
					bool flag = this.idLocations.TryGetValue(id, out locator);
					if (flag)
					{
						this.Err("Duplicate ID “" + id + "”.");
						this.Warn("The first occurrence of ID “" + id + "” was here.");
					}
					else
					{
						this.idLocations[id] = new Locator(this.tokenizer);
					}
				}
			}
			this.needToDropLF = false;
			DispatchGroup group;
			string name;
			string ns;
			int num;
			DispatchGroup dispatchGroup17;
			for (;;)
			{
				group = elementName.Group;
				name = elementName.name;
				if (this.IsInForeign)
				{
					StackNode<T> stackNode = this.stack[this.currentPtr];
					ns = stackNode.ns;
					if (!stackNode.IsHtmlIntegrationPoint && (!(ns == "http://www.w3.org/1998/Math/MathML") || ((stackNode.Group != DispatchGroup.MI_MO_MN_MS_MTEXT || group == DispatchGroup.MGLYPH_OR_MALIGNMARK) && (stackNode.Group != DispatchGroup.ANNOTATION_XML || group != DispatchGroup.SVG))))
					{
						DispatchGroup dispatchGroup = group;
						if (dispatchGroup <= DispatchGroup.NOBR)
						{
							switch (dispatchGroup)
							{
							case DispatchGroup.BODY:
							case DispatchGroup.BR:
								break;
							default:
								if (dispatchGroup != DispatchGroup.LI)
								{
									switch (dispatchGroup)
									{
									case DispatchGroup.META:
									case DispatchGroup.HEAD:
									case DispatchGroup.HR:
									case DispatchGroup.NOBR:
										goto IL_196;
									}
									goto Block_12;
								}
								break;
							}
						}
						else if (dispatchGroup <= DispatchGroup.TABLE)
						{
							if (dispatchGroup != DispatchGroup.P && dispatchGroup != DispatchGroup.TABLE)
							{
								break;
							}
						}
						else
						{
							switch (dispatchGroup)
							{
							case DispatchGroup.DD_OR_DT:
							case DispatchGroup.H1_OR_H2_OR_H3_OR_H4_OR_H5_OR_H6:
							case DispatchGroup.PRE_OR_LISTING:
							case DispatchGroup.B_OR_BIG_OR_CODE_OR_EM_OR_I_OR_S_OR_SMALL_OR_STRIKE_OR_STRONG_OR_TT_OR_U:
							case DispatchGroup.UL_OR_OL_OR_DL:
							case DispatchGroup.EMBED_OR_IMG:
							case DispatchGroup.DIV_OR_BLOCKQUOTE_OR_CENTER_OR_MENU:
							case DispatchGroup.RUBY_OR_SPAN_OR_SUB_OR_SUP_OR_VAR:
								break;
							case DispatchGroup.MARQUEE_OR_APPLET:
							case DispatchGroup.IFRAME:
							case DispatchGroup.AREA_OR_WBR:
							case DispatchGroup.ADDRESS_OR_ARTICLE_OR_ASIDE_OR_DETAILS_OR_DIR_OR_FIGCAPTION_OR_FIGURE_OR_FOOTER_OR_HEADER_OR_HGROUP_OR_NAV_OR_SECTION_OR_SUMMARY:
								goto IL_22F;
							default:
								if (dispatchGroup != DispatchGroup.FONT)
								{
									goto Block_17;
								}
								if (attributes.Contains(AttributeName.COLOR) || attributes.Contains(AttributeName.FACE) || attributes.Contains(AttributeName.SIZE))
								{
									this.Err("HTML start tag “" + name + "” in a foreign namespace context.");
									while (!this.IsSpecialParentInForeign(this.stack[this.currentPtr]))
									{
										this.Pop();
									}
									continue;
								}
								goto IL_22F;
							}
						}
						IL_196:
						this.Err("HTML start tag “" + name + "” in a foreign namespace context.");
						while (!this.IsSpecialParentInForeign(this.stack[this.currentPtr]))
						{
							this.Pop();
						}
						continue;
					}
				}
				switch (this.mode)
				{
				case InsertionMode.INITIAL:
					switch (this.DoctypeExpectation)
					{
					case DoctypeExpectation.Html:
						this.Err("Start tag seen without seeing a doctype first. Expected “<!DOCTYPE html>”.");
						break;
					case DoctypeExpectation.Html401Transitional:
						this.Err("Start tag seen without seeing a doctype first. Expected “<!DOCTYPE HTML PUBLIC \"-//W3C//DTD HTML 4.01 Transitional//EN\" \"http://www.w3.org/TR/html4/loose.dtd\">”.");
						break;
					case DoctypeExpectation.Html401Strict:
						this.Err("Start tag seen without seeing a doctype first. Expected “<!DOCTYPE HTML PUBLIC \"-//W3C//DTD HTML 4.01//EN\" \"http://www.w3.org/TR/html4/strict.dtd\">”.");
						break;
					case DoctypeExpectation.Auto:
						this.Err("Start tag seen without seeing a doctype first. Expected e.g. “<!DOCTYPE html>”.");
						break;
					}
					this.DocumentModeInternal(DocumentMode.QuirksMode, null, null, false);
					this.mode = InsertionMode.BEFORE_HTML;
					continue;
				case InsertionMode.BEFORE_HTML:
				{
					DispatchGroup dispatchGroup2 = group;
					if (dispatchGroup2 == DispatchGroup.HTML)
					{
						goto Block_138;
					}
					this.AppendHtmlElementToDocumentAndPush();
					this.mode = InsertionMode.BEFORE_HEAD;
					continue;
				}
				case InsertionMode.BEFORE_HEAD:
				{
					DispatchGroup dispatchGroup3 = group;
					if (dispatchGroup3 == DispatchGroup.HEAD)
					{
						goto IL_197B;
					}
					if (dispatchGroup3 == DispatchGroup.HTML)
					{
						goto Block_141;
					}
					this.AppendToCurrentNodeAndPushHeadElement(HtmlAttributes.EMPTY_ATTRIBUTES);
					this.mode = InsertionMode.IN_HEAD;
					continue;
				}
				case InsertionMode.IN_HEAD:
					goto IL_1251;
				case InsertionMode.IN_HEAD_NOSCRIPT:
					goto IL_13FD;
				case InsertionMode.AFTER_HEAD:
				{
					DispatchGroup dispatchGroup4 = group;
					if (dispatchGroup4 <= DispatchGroup.HEAD)
					{
						switch (dispatchGroup4)
						{
						case DispatchGroup.BASE:
							goto IL_1A90;
						case DispatchGroup.BODY:
							goto IL_1A4B;
						default:
							if (dispatchGroup4 == DispatchGroup.FRAMESET)
							{
								goto IL_1A78;
							}
							switch (dispatchGroup4)
							{
							case DispatchGroup.LINK_OR_BASEFONT_OR_BGSOUND:
								goto IL_1ABA;
							case DispatchGroup.META:
								goto IL_1AE4;
							case DispatchGroup.HEAD:
								goto IL_1BEB;
							}
							break;
						}
					}
					else
					{
						switch (dispatchGroup4)
						{
						case DispatchGroup.HTML:
							goto IL_1A29;
						case DispatchGroup.NOBR:
							break;
						case DispatchGroup.NOFRAMES:
							goto IL_1B57;
						default:
							switch (dispatchGroup4)
							{
							case DispatchGroup.SCRIPT:
								goto IL_1B15;
							case DispatchGroup.SELECT:
								break;
							case DispatchGroup.STYLE:
								goto IL_1B57;
							default:
								if (dispatchGroup4 == DispatchGroup.TITLE)
								{
									goto IL_1BA5;
								}
								break;
							}
							break;
						}
					}
					this.AppendToCurrentNodeAndPushBodyElement();
					this.mode = InsertionMode.FRAMESET_OK;
					continue;
				}
				case InsertionMode.IN_BODY:
					goto IL_930;
				case InsertionMode.IN_TABLE:
					goto IL_458;
				case InsertionMode.IN_CAPTION:
					goto IL_6CB;
				case InsertionMode.IN_COLUMN_GROUP:
				{
					DispatchGroup dispatchGroup5 = group;
					if (dispatchGroup5 == DispatchGroup.COL)
					{
						goto IL_153C;
					}
					if (dispatchGroup5 == DispatchGroup.HTML)
					{
						goto Block_111;
					}
					if (this.currentPtr == 0)
					{
						goto Block_113;
					}
					this.Pop();
					this.mode = InsertionMode.IN_TABLE;
					continue;
				}
				case InsertionMode.IN_TABLE_BODY:
				{
					DispatchGroup dispatchGroup6 = group;
					switch (dispatchGroup6)
					{
					case DispatchGroup.CAPTION:
					case DispatchGroup.COL:
					case DispatchGroup.COLGROUP:
						break;
					default:
						switch (dispatchGroup6)
						{
						case DispatchGroup.TR:
							goto IL_32D;
						case DispatchGroup.XMP:
							goto IL_3C1;
						case DispatchGroup.TBODY_OR_THEAD_OR_TFOOT:
							break;
						case DispatchGroup.TD_OR_TH:
							this.Err("“" + name + "” start tag in table body.");
							this.ClearStackBackTo(this.FindLastInTableScopeOrRootTbodyTheadTfoot());
							this.AppendToCurrentNodeAndPushElement(ElementName.TR, HtmlAttributes.EMPTY_ATTRIBUTES);
							this.mode = InsertionMode.IN_ROW;
							continue;
						default:
							goto IL_3C1;
						}
						break;
					}
					num = this.FindLastInTableScopeOrRootTbodyTheadTfoot();
					if (num == 0)
					{
						goto Block_28;
					}
					this.ClearStackBackTo(num);
					this.Pop();
					this.mode = InsertionMode.IN_TABLE;
					continue;
				}
				case InsertionMode.IN_ROW:
					break;
				case InsertionMode.IN_CELL:
					goto IL_764;
				case InsertionMode.IN_SELECT:
					goto IL_15F4;
				case InsertionMode.IN_SELECT_IN_TABLE:
				{
					DispatchGroup dispatchGroup7 = group;
					if (dispatchGroup7 != DispatchGroup.CAPTION)
					{
						switch (dispatchGroup7)
						{
						case DispatchGroup.TABLE:
						case DispatchGroup.TR:
						case DispatchGroup.TBODY_OR_THEAD_OR_TFOOT:
						case DispatchGroup.TD_OR_TH:
							break;
						case DispatchGroup.TEXTAREA:
						case DispatchGroup.TITLE:
						case DispatchGroup.XMP:
							goto IL_15F4;
						default:
							goto IL_15F4;
						}
					}
					this.Err("“" + name + "” start tag with “select” open.");
					num = this.FindLastInTableScope("select");
					if (num == 2147483647)
					{
						goto Block_116;
					}
					while (this.currentPtr >= num)
					{
						this.Pop();
					}
					this.ResetTheInsertionMode();
					continue;
				}
				case InsertionMode.AFTER_BODY:
				{
					DispatchGroup dispatchGroup8 = group;
					if (dispatchGroup8 == DispatchGroup.HTML)
					{
						goto Block_131;
					}
					this.ErrStrayStartTag(name);
					this.mode = (this.framesetOk ? InsertionMode.FRAMESET_OK : InsertionMode.IN_BODY);
					continue;
				}
				case InsertionMode.IN_FRAMESET:
					goto IL_17E3;
				case InsertionMode.AFTER_FRAMESET:
					goto IL_181E;
				case InsertionMode.AFTER_AFTER_BODY:
				{
					DispatchGroup dispatchGroup9 = group;
					if (dispatchGroup9 == DispatchGroup.HTML)
					{
						goto Block_152;
					}
					this.ErrStrayStartTag(name);
					this.Fatal();
					this.mode = (this.framesetOk ? InsertionMode.FRAMESET_OK : InsertionMode.IN_BODY);
					continue;
				}
				case InsertionMode.AFTER_AFTER_FRAMESET:
					goto IL_1C5D;
				case InsertionMode.TEXT:
					goto IL_1CCB;
				case InsertionMode.FRAMESET_OK:
					goto IL_7C4;
				default:
					continue;
				}
				IL_3C1:
				DispatchGroup dispatchGroup10 = group;
				switch (dispatchGroup10)
				{
				case DispatchGroup.CAPTION:
				case DispatchGroup.COL:
				case DispatchGroup.COLGROUP:
					break;
				default:
					switch (dispatchGroup10)
					{
					case DispatchGroup.TR:
					case DispatchGroup.TBODY_OR_THEAD_OR_TFOOT:
						break;
					case DispatchGroup.XMP:
						goto IL_458;
					case DispatchGroup.TD_OR_TH:
						goto IL_3F6;
					default:
						goto IL_458;
					}
					break;
				}
				num = this.FindLastOrRoot(DispatchGroup.TR);
				if (num == 0)
				{
					goto Block_31;
				}
				this.ClearStackBackTo(num);
				this.Pop();
				this.mode = InsertionMode.IN_TABLE_BODY;
				continue;
				IL_458:
				DispatchGroup dispatchGroup11 = group;
				switch (dispatchGroup11)
				{
				case DispatchGroup.CAPTION:
					goto IL_4BC;
				case DispatchGroup.COL:
					this.ClearStackBackTo(this.FindLastOrRoot(DispatchGroup.TABLE));
					this.AppendToCurrentNodeAndPushElement(ElementName.COLGROUP, HtmlAttributes.EMPTY_ATTRIBUTES);
					this.mode = InsertionMode.IN_COLUMN_GROUP;
					continue;
				case DispatchGroup.COLGROUP:
					goto IL_4E7;
				case DispatchGroup.FORM:
					goto IL_67D;
				case DispatchGroup.FRAME:
				case DispatchGroup.FRAMESET:
				case DispatchGroup.IMAGE:
					break;
				case DispatchGroup.INPUT:
					if (Portability.LowerCaseLiteralEqualsIgnoreAsciiCaseString("hidden", attributes.GetValue(AttributeName.TYPE)))
					{
						goto Block_38;
					}
					goto IL_6CB;
				default:
					switch (dispatchGroup11)
					{
					case DispatchGroup.SCRIPT:
						goto IL_5EA;
					case DispatchGroup.STYLE:
						goto IL_61B;
					case DispatchGroup.TABLE:
						this.Err("Start tag for “table” seen but the previous “table” is still open.");
						num = this.FindLastInTableScope(name);
						if (num != 2147483647)
						{
							this.GenerateImpliedEndTags();
							if (this.ErrorEvent != null && !this.IsCurrent("table"))
							{
								this.Err("Unclosed elements on stack.");
							}
							while (this.currentPtr >= num)
							{
								this.Pop();
							}
							this.ResetTheInsertionMode();
							continue;
						}
						goto IL_1CCB;
					case DispatchGroup.TR:
					case DispatchGroup.TD_OR_TH:
						this.ClearStackBackTo(this.FindLastOrRoot(DispatchGroup.TABLE));
						this.AppendToCurrentNodeAndPushElement(ElementName.TBODY, HtmlAttributes.EMPTY_ATTRIBUTES);
						this.mode = InsertionMode.IN_TABLE_BODY;
						continue;
					case DispatchGroup.TBODY_OR_THEAD_OR_TFOOT:
						goto IL_538;
					}
					break;
				}
				this.Err("Start tag “" + name + "” seen in “table”.");
				IL_6CB:
				DispatchGroup dispatchGroup12 = group;
				switch (dispatchGroup12)
				{
				case DispatchGroup.CAPTION:
				case DispatchGroup.COL:
				case DispatchGroup.COLGROUP:
					break;
				default:
					switch (dispatchGroup12)
					{
					case DispatchGroup.TR:
					case DispatchGroup.TBODY_OR_THEAD_OR_TFOOT:
					case DispatchGroup.TD_OR_TH:
						break;
					case DispatchGroup.XMP:
						goto IL_764;
					default:
						goto IL_764;
					}
					break;
				}
				this.ErrStrayStartTag(name);
				num = this.FindLastInTableScope("caption");
				if (num != 2147483647)
				{
					this.GenerateImpliedEndTags();
					if (this.ErrorEvent != null && this.currentPtr != num)
					{
						this.Err("Unclosed elements on stack.");
					}
					while (this.currentPtr >= num)
					{
						this.Pop();
					}
					this.ClearTheListOfActiveFormattingElementsUpToTheLastMarker();
					this.mode = InsertionMode.IN_TABLE;
					continue;
				}
				goto IL_1CCB;
				IL_764:
				DispatchGroup dispatchGroup13 = group;
				switch (dispatchGroup13)
				{
				case DispatchGroup.CAPTION:
				case DispatchGroup.COL:
				case DispatchGroup.COLGROUP:
					break;
				default:
					switch (dispatchGroup13)
					{
					case DispatchGroup.TR:
					case DispatchGroup.TBODY_OR_THEAD_OR_TFOOT:
					case DispatchGroup.TD_OR_TH:
						break;
					case DispatchGroup.XMP:
						goto IL_7C4;
					default:
						goto IL_7C4;
					}
					break;
				}
				num = this.FindLastInTableScopeTdTh();
				if (num == 2147483647)
				{
					goto Block_48;
				}
				this.CloseTheCell(num);
				continue;
				IL_930:
				switch (group)
				{
				case DispatchGroup.A:
					goto IL_C65;
				case DispatchGroup.BASE:
				case DispatchGroup.LINK_OR_BASEFONT_OR_BGSOUND:
				case DispatchGroup.META:
				case DispatchGroup.SCRIPT:
				case DispatchGroup.STYLE:
				case DispatchGroup.TITLE:
				case DispatchGroup.COMMAND:
				{
					IL_1251:
					DispatchGroup dispatchGroup14 = group;
					if (dispatchGroup14 <= DispatchGroup.NOSCRIPT)
					{
						if (dispatchGroup14 == DispatchGroup.BASE)
						{
							goto IL_12E7;
						}
						switch (dispatchGroup14)
						{
						case DispatchGroup.LINK_OR_BASEFONT_OR_BGSOUND:
						case DispatchGroup.META:
							goto IL_13FD;
						case DispatchGroup.HEAD:
							goto IL_13DB;
						case DispatchGroup.HTML:
							goto IL_12C5;
						case DispatchGroup.NOFRAMES:
							goto IL_13AA;
						case DispatchGroup.NOSCRIPT:
							goto IL_132F;
						}
					}
					else
					{
						switch (dispatchGroup14)
						{
						case DispatchGroup.SCRIPT:
							goto IL_1379;
						case DispatchGroup.SELECT:
							break;
						case DispatchGroup.STYLE:
							goto IL_13AA;
						default:
							if (dispatchGroup14 == DispatchGroup.TITLE)
							{
								goto IL_12FA;
							}
							if (dispatchGroup14 == DispatchGroup.COMMAND)
							{
								goto IL_12E7;
							}
							break;
						}
					}
					this.Pop();
					this.mode = InsertionMode.AFTER_HEAD;
					continue;
				}
				case DispatchGroup.BODY:
					goto IL_A68;
				case DispatchGroup.BR:
				case DispatchGroup.EMBED_OR_IMG:
				case DispatchGroup.AREA_OR_WBR:
					goto IL_E16;
				case DispatchGroup.BUTTON:
					num = this.FindLastInScope(name);
					if (num != 2147483647)
					{
						this.Err("“button” start tag seen when there was an open “button” element in scope.");
						this.GenerateImpliedEndTags();
						if (this.ErrorEvent != null && !this.IsCurrent(name))
						{
							this.ErrUnclosedElementsImplied(num, name);
						}
						while (this.currentPtr >= num)
						{
							this.Pop();
						}
						continue;
					}
					goto IL_D97;
				case DispatchGroup.CAPTION:
				case DispatchGroup.COL:
				case DispatchGroup.COLGROUP:
				case DispatchGroup.FRAME:
				case DispatchGroup.FRAMESET:
				case DispatchGroup.HEAD:
				case DispatchGroup.TR:
				case DispatchGroup.TBODY_OR_THEAD_OR_TFOOT:
				case DispatchGroup.TD_OR_TH:
					goto IL_1212;
				case DispatchGroup.FORM:
					goto IL_B50;
				case DispatchGroup.IMAGE:
					this.Err("Saw a start tag “image”.");
					elementName = ElementName.IMG;
					continue;
				case DispatchGroup.INPUT:
				case DispatchGroup.KEYGEN:
					goto IL_E5F;
				case DispatchGroup.ISINDEX:
					goto IL_E7F;
				case DispatchGroup.LI:
				case DispatchGroup.DD_OR_DT:
					goto IL_B82;
				case DispatchGroup.MATH:
					goto IL_11BA;
				case DispatchGroup.SVG:
					goto IL_11E6;
				case DispatchGroup.HR:
					goto IL_E2F;
				case DispatchGroup.HTML:
					goto IL_A46;
				case DispatchGroup.NOBR:
					goto IL_CFF;
				case DispatchGroup.NOFRAMES:
				case DispatchGroup.IFRAME:
				case DispatchGroup.NOEMBED:
					goto IL_10A0;
				case DispatchGroup.NOSCRIPT:
					goto IL_1082;
				case DispatchGroup.OPTGROUP:
				case DispatchGroup.OPTION:
					goto IL_112A;
				case DispatchGroup.P:
				case DispatchGroup.UL_OR_OL_OR_DL:
				case DispatchGroup.DIV_OR_BLOCKQUOTE_OR_CENTER_OR_MENU:
				case DispatchGroup.ADDRESS_OR_ARTICLE_OR_ASIDE_OR_DETAILS_OR_DIR_OR_FIGCAPTION_OR_FIGURE_OR_FOOTER_OR_HEADER_OR_HGROUP_OR_NAV_OR_SECTION_OR_SUMMARY:
					goto IL_AC4;
				case DispatchGroup.PLAINTEXT:
					goto IL_C42;
				case DispatchGroup.SELECT:
					goto IL_10D1;
				case DispatchGroup.TABLE:
					goto IL_DF1;
				case DispatchGroup.TEXTAREA:
					goto IL_1003;
				case DispatchGroup.XMP:
					goto IL_1045;
				case DispatchGroup.H1_OR_H2_OR_H3_OR_H4_OR_H5_OR_H6:
					goto IL_ADA;
				case DispatchGroup.MARQUEE_OR_APPLET:
					goto IL_DD5;
				case DispatchGroup.PRE_OR_LISTING:
					goto IL_B33;
				case DispatchGroup.B_OR_BIG_OR_CODE_OR_EM_OR_I_OR_S_OR_SMALL_OR_STRIKE_OR_STRONG_OR_TT_OR_U:
				case DispatchGroup.FONT:
					goto IL_CDC;
				case DispatchGroup.RT_OR_RP:
					goto IL_1153;
				case DispatchGroup.PARAM_OR_SOURCE_OR_TRACK:
					goto IL_E1C;
				case DispatchGroup.FIELDSET:
					goto IL_B17;
				case DispatchGroup.OUTPUT_OR_LABEL:
					goto IL_121F;
				case DispatchGroup.OBJECT:
					goto IL_DB3;
				}
				goto Block_60;
				IL_7C4:
				DispatchGroup dispatchGroup15 = group;
				if (dispatchGroup15 <= DispatchGroup.LI)
				{
					switch (dispatchGroup15)
					{
					case DispatchGroup.BR:
					case DispatchGroup.BUTTON:
						break;
					default:
						switch (dispatchGroup15)
						{
						case DispatchGroup.FRAMESET:
							goto IL_878;
						case DispatchGroup.IMAGE:
						case DispatchGroup.ISINDEX:
							goto IL_930;
						case DispatchGroup.INPUT:
						case DispatchGroup.LI:
							break;
						default:
							goto IL_930;
						}
						break;
					}
				}
				else if (dispatchGroup15 != DispatchGroup.HR)
				{
					switch (dispatchGroup15)
					{
					case DispatchGroup.SELECT:
					case DispatchGroup.TABLE:
					case DispatchGroup.TEXTAREA:
					case DispatchGroup.XMP:
					case DispatchGroup.DD_OR_DT:
					case DispatchGroup.MARQUEE_OR_APPLET:
					case DispatchGroup.PRE_OR_LISTING:
					case DispatchGroup.IFRAME:
					case DispatchGroup.EMBED_OR_IMG:
					case DispatchGroup.AREA_OR_WBR:
						break;
					case DispatchGroup.STYLE:
					case DispatchGroup.TITLE:
					case DispatchGroup.TR:
					case DispatchGroup.TBODY_OR_THEAD_OR_TFOOT:
					case DispatchGroup.TD_OR_TH:
					case DispatchGroup.H1_OR_H2_OR_H3_OR_H4_OR_H5_OR_H6:
					case DispatchGroup.B_OR_BIG_OR_CODE_OR_EM_OR_I_OR_S_OR_SMALL_OR_STRIKE_OR_STRONG_OR_TT_OR_U:
					case DispatchGroup.UL_OR_OL_OR_DL:
						goto IL_930;
					default:
						switch (dispatchGroup15)
						{
						case DispatchGroup.OBJECT:
						case DispatchGroup.KEYGEN:
							break;
						case DispatchGroup.FONT:
							goto IL_930;
						default:
							goto IL_930;
						}
						break;
					}
				}
				if (this.mode == InsertionMode.FRAMESET_OK && (group != DispatchGroup.INPUT || !Portability.LowerCaseLiteralEqualsIgnoreAsciiCaseString("hidden", attributes.GetValue(AttributeName.TYPE))))
				{
					this.framesetOk = false;
					this.mode = InsertionMode.IN_BODY;
					goto IL_930;
				}
				goto IL_930;
				IL_13FD:
				DispatchGroup dispatchGroup16 = group;
				switch (dispatchGroup16)
				{
				case DispatchGroup.LINK_OR_BASEFONT_OR_BGSOUND:
					goto IL_1464;
				case DispatchGroup.MATH:
				case DispatchGroup.SVG:
				case (DispatchGroup)21:
				case DispatchGroup.HR:
				case DispatchGroup.NOBR:
					break;
				case DispatchGroup.META:
					goto IL_1477;
				case DispatchGroup.HEAD:
					goto IL_14C2;
				case DispatchGroup.HTML:
					goto IL_1442;
				case DispatchGroup.NOFRAMES:
					goto IL_1491;
				case DispatchGroup.NOSCRIPT:
					goto IL_14D2;
				default:
					if (dispatchGroup16 == DispatchGroup.STYLE)
					{
						goto IL_1491;
					}
					break;
				}
				this.Err("Bad start tag in “" + name + "” in “head”.");
				this.Pop();
				this.mode = InsertionMode.IN_HEAD;
				continue;
				IL_15F4:
				dispatchGroup17 = group;
				if (dispatchGroup17 <= DispatchGroup.HTML)
				{
					if (dispatchGroup17 != DispatchGroup.INPUT)
					{
						goto Block_119;
					}
				}
				else
				{
					switch (dispatchGroup17)
					{
					case DispatchGroup.OPTGROUP:
						goto IL_1693;
					case DispatchGroup.OPTION:
						goto IL_1670;
					case DispatchGroup.P:
					case DispatchGroup.PLAINTEXT:
					case DispatchGroup.STYLE:
					case DispatchGroup.TABLE:
						goto IL_178A;
					case DispatchGroup.SCRIPT:
						goto IL_1759;
					case DispatchGroup.SELECT:
						goto IL_16C9;
					case DispatchGroup.TEXTAREA:
						break;
					default:
						if (dispatchGroup17 != DispatchGroup.KEYGEN)
						{
							goto Block_122;
						}
						break;
					}
				}
				this.Err("“" + name + "” start tag seen in “select∁D.");
				num = this.FindLastInTableScope("select");
				if (num == 2147483647)
				{
					goto Block_129;
				}
				while (this.currentPtr >= num)
				{
					this.Pop();
				}
				this.ResetTheInsertionMode();
			}
			Block_12:
			Block_17:
			IL_22F:
			if ("http://www.w3.org/2000/svg" == ns)
			{
				attributes.AdjustForSvg();
				if (selfClosing)
				{
					this.AppendVoidElementToCurrentMayFosterSVG(elementName, attributes);
					selfClosing = false;
				}
				else
				{
					this.AppendToCurrentNodeAndPushElementMayFosterSVG(elementName, attributes);
				}
				attributes = null;
				goto IL_1CCB;
			}
			attributes.AdjustForMath();
			if (selfClosing)
			{
				this.AppendVoidElementToCurrentMayFosterMathML(elementName, attributes);
				selfClosing = false;
			}
			else
			{
				this.AppendToCurrentNodeAndPushElementMayFosterMathML(elementName, attributes);
			}
			attributes = null;
			goto IL_1CCB;
			IL_32D:
			this.ClearStackBackTo(this.FindLastInTableScopeOrRootTbodyTheadTfoot());
			this.AppendToCurrentNodeAndPushElement(elementName, attributes);
			this.mode = InsertionMode.IN_ROW;
			attributes = null;
			goto IL_1CCB;
			Block_28:
			this.ErrStrayStartTag(name);
			goto IL_1CCB;
			IL_3F6:
			this.ClearStackBackTo(this.FindLastOrRoot(DispatchGroup.TR));
			this.AppendToCurrentNodeAndPushElement(elementName, attributes);
			this.mode = InsertionMode.IN_CELL;
			this.InsertMarker();
			attributes = null;
			goto IL_1CCB;
			Block_31:
			this.Err("No table row to close.");
			goto IL_1CCB;
			IL_4BC:
			this.ClearStackBackTo(this.FindLastOrRoot(DispatchGroup.TABLE));
			this.InsertMarker();
			this.AppendToCurrentNodeAndPushElement(elementName, attributes);
			this.mode = InsertionMode.IN_CAPTION;
			attributes = null;
			goto IL_1CCB;
			IL_4E7:
			this.ClearStackBackTo(this.FindLastOrRoot(DispatchGroup.TABLE));
			this.AppendToCurrentNodeAndPushElement(elementName, attributes);
			this.mode = InsertionMode.IN_COLUMN_GROUP;
			attributes = null;
			goto IL_1CCB;
			IL_538:
			this.ClearStackBackTo(this.FindLastOrRoot(DispatchGroup.TABLE));
			this.AppendToCurrentNodeAndPushElement(elementName, attributes);
			this.mode = InsertionMode.IN_TABLE_BODY;
			attributes = null;
			goto IL_1CCB;
			IL_5EA:
			this.AppendToCurrentNodeAndPushElement(elementName, attributes);
			this.originalMode = this.mode;
			this.mode = InsertionMode.TEXT;
			this.tokenizer.SetStateAndEndTagExpectation(TokenizerState.SCRIPT_DATA, elementName);
			attributes = null;
			goto IL_1CCB;
			IL_61B:
			this.AppendToCurrentNodeAndPushElement(elementName, attributes);
			this.originalMode = this.mode;
			this.mode = InsertionMode.TEXT;
			this.tokenizer.SetStateAndEndTagExpectation(TokenizerState.RAWTEXT, elementName);
			attributes = null;
			goto IL_1CCB;
			Block_38:
			this.AppendVoidElementToCurrent(name, attributes, this.formPointer);
			selfClosing = false;
			attributes = null;
			goto IL_1CCB;
			IL_67D:
			if (this.formPointer != null)
			{
				this.Err("Saw a “form” start tag, but there was already an active “form” element. Nested forms are not allowed. Ignoring the tag.");
				goto IL_1CCB;
			}
			this.Err("Start tag “form” seen in “table”.");
			this.AppendVoidFormToCurrent(attributes);
			attributes = null;
			goto IL_1CCB;
			Block_48:
			this.Err("No cell to close.");
			goto IL_1CCB;
			IL_878:
			if (this.mode != InsertionMode.FRAMESET_OK)
			{
				this.ErrStrayStartTag(name);
				goto IL_1CCB;
			}
			if (this.currentPtr == 0 || this.stack[1].Group != DispatchGroup.BODY)
			{
				this.ErrStrayStartTag(name);
				goto IL_1CCB;
			}
			this.Err("“frameset” start tag seen.");
			this.DetachFromParent(this.stack[1].node);
			while (this.currentPtr > 0)
			{
				this.Pop();
			}
			this.AppendToCurrentNodeAndPushElement(elementName, attributes);
			this.mode = InsertionMode.IN_FRAMESET;
			attributes = null;
			goto IL_1CCB;
			Block_60:
			goto IL_123B;
			IL_A46:
			this.ErrStrayStartTag(name);
			if (!this.fragment)
			{
				this.AddAttributesToHtml(attributes);
				attributes = null;
				goto IL_1CCB;
			}
			goto IL_1CCB;
			IL_A68:
			if (this.currentPtr == 0 || this.stack[1].Group != DispatchGroup.BODY)
			{
				this.ErrStrayStartTag(name);
				goto IL_1CCB;
			}
			this.Err("“body” start tag found but the “body” element is already open.");
			this.framesetOk = false;
			if (this.mode == InsertionMode.FRAMESET_OK)
			{
				this.mode = InsertionMode.IN_BODY;
			}
			if (this.AddAttributesToBody(attributes))
			{
				attributes = null;
				goto IL_1CCB;
			}
			goto IL_1CCB;
			IL_AC4:
			this.ImplicitlyCloseP();
			this.AppendToCurrentNodeAndPushElementMayFoster(elementName, attributes);
			attributes = null;
			goto IL_1CCB;
			IL_ADA:
			this.ImplicitlyCloseP();
			if (this.stack[this.currentPtr].Group == DispatchGroup.H1_OR_H2_OR_H3_OR_H4_OR_H5_OR_H6)
			{
				this.Err("Heading cannot be a child of another heading.");
				this.Pop();
			}
			this.AppendToCurrentNodeAndPushElementMayFoster(elementName, attributes);
			attributes = null;
			goto IL_1CCB;
			IL_B17:
			this.ImplicitlyCloseP();
			this.AppendToCurrentNodeAndPushElementMayFoster(elementName, attributes, this.formPointer);
			attributes = null;
			goto IL_1CCB;
			IL_B33:
			this.ImplicitlyCloseP();
			this.AppendToCurrentNodeAndPushElementMayFoster(elementName, attributes);
			this.needToDropLF = true;
			attributes = null;
			goto IL_1CCB;
			IL_B50:
			if (this.formPointer != null)
			{
				this.Err("Saw a “form” start tag, but there was already an active “form” element. Nested forms are not allowed. Ignoring the tag.");
				goto IL_1CCB;
			}
			this.ImplicitlyCloseP();
			this.AppendToCurrentNodeAndPushFormElementMayFoster(attributes);
			attributes = null;
			goto IL_1CCB;
			IL_B82:
			num = this.currentPtr;
			StackNode<T> stackNode2;
			for (;;)
			{
				stackNode2 = this.stack[num];
				if (stackNode2.Group == group)
				{
					break;
				}
				if (stackNode2.IsScoping || (stackNode2.IsSpecial && stackNode2.name != "p" && stackNode2.name != "address" && stackNode2.name != "div"))
				{
					goto IL_C2C;
				}
				num--;
			}
			this.GenerateImpliedEndTagsExceptFor(stackNode2.name);
			if (this.ErrorEvent != null && num != this.currentPtr)
			{
				this.ErrUnclosedElementsImplied(num, name);
			}
			while (this.currentPtr >= num)
			{
				this.Pop();
			}
			IL_C2C:
			this.ImplicitlyCloseP();
			this.AppendToCurrentNodeAndPushElementMayFoster(elementName, attributes);
			attributes = null;
			goto IL_1CCB;
			IL_C42:
			this.ImplicitlyCloseP();
			this.AppendToCurrentNodeAndPushElementMayFoster(elementName, attributes);
			this.tokenizer.SetStateAndEndTagExpectation(TokenizerState.PLAINTEXT, elementName);
			attributes = null;
			goto IL_1CCB;
			IL_C65:
			int num2 = this.FindInListOfActiveFormattingElementsContainsBetweenEndAndLastMarker("a");
			if (num2 != -1)
			{
				this.Err("An “a” start tag seen with already an active “a” element.");
				StackNode<T> stackNode3 = this.listOfActiveFormattingElements[num2];
				stackNode3.Retain();
				this.AdoptionAgencyEndTag("a");
				this.RemoveFromStack(stackNode3);
				num2 = this.FindInListOfActiveFormattingElements(stackNode3);
				if (num2 != -1)
				{
					this.RemoveFromListOfActiveFormattingElements(num2);
				}
				stackNode3.Release();
			}
			this.ReconstructTheActiveFormattingElements();
			this.AppendToCurrentNodeAndPushFormattingElementMayFoster(elementName, attributes);
			attributes = null;
			goto IL_1CCB;
			IL_CDC:
			this.ReconstructTheActiveFormattingElements();
			this.MaybeForgetEarlierDuplicateFormattingElement(elementName.name, attributes);
			this.AppendToCurrentNodeAndPushFormattingElementMayFoster(elementName, attributes);
			attributes = null;
			goto IL_1CCB;
			IL_CFF:
			this.ReconstructTheActiveFormattingElements();
			if (2147483647 != this.FindLastInScope("nobr"))
			{
				this.Err("“nobr” start tag seen when there was an open “nobr” element in scope.");
				this.AdoptionAgencyEndTag("nobr");
				this.ReconstructTheActiveFormattingElements();
			}
			this.AppendToCurrentNodeAndPushFormattingElementMayFoster(elementName, attributes);
			attributes = null;
			goto IL_1CCB;
			IL_D97:
			this.ReconstructTheActiveFormattingElements();
			this.AppendToCurrentNodeAndPushElementMayFoster(elementName, attributes, this.formPointer);
			attributes = null;
			goto IL_1CCB;
			IL_DB3:
			this.ReconstructTheActiveFormattingElements();
			this.AppendToCurrentNodeAndPushElementMayFoster(elementName, attributes, this.formPointer);
			this.InsertMarker();
			attributes = null;
			goto IL_1CCB;
			IL_DD5:
			this.ReconstructTheActiveFormattingElements();
			this.AppendToCurrentNodeAndPushElementMayFoster(elementName, attributes);
			this.InsertMarker();
			attributes = null;
			goto IL_1CCB;
			IL_DF1:
			if (!this.quirks)
			{
				this.ImplicitlyCloseP();
			}
			this.AppendToCurrentNodeAndPushElementMayFoster(elementName, attributes);
			this.mode = InsertionMode.IN_TABLE;
			attributes = null;
			goto IL_1CCB;
			IL_E16:
			this.ReconstructTheActiveFormattingElements();
			IL_E1C:
			this.AppendVoidElementToCurrentMayFoster(elementName, attributes);
			selfClosing = false;
			attributes = null;
			goto IL_1CCB;
			IL_E2F:
			this.ImplicitlyCloseP();
			this.AppendVoidElementToCurrentMayFoster(elementName, attributes);
			selfClosing = false;
			attributes = null;
			goto IL_1CCB;
			IL_E5F:
			this.ReconstructTheActiveFormattingElements();
			this.AppendVoidElementToCurrentMayFoster(name, attributes, this.formPointer);
			selfClosing = false;
			attributes = null;
			goto IL_1CCB;
			IL_E7F:
			this.Err("“isindex” seen.");
			if (this.formPointer == null)
			{
				this.ImplicitlyCloseP();
				HtmlAttributes htmlAttributes = new HtmlAttributes(0);
				int index = attributes.GetIndex(AttributeName.ACTION);
				if (index > -1)
				{
					htmlAttributes.AddAttribute(AttributeName.ACTION, attributes.GetValue(index), XmlViolationPolicy.Allow);
				}
				this.AppendToCurrentNodeAndPushFormElementMayFoster(htmlAttributes);
				this.AppendVoidElementToCurrentMayFoster(ElementName.HR, HtmlAttributes.EMPTY_ATTRIBUTES);
				this.AppendToCurrentNodeAndPushElementMayFoster(ElementName.LABEL, HtmlAttributes.EMPTY_ATTRIBUTES);
				int index2 = attributes.GetIndex(AttributeName.PROMPT);
				if (index2 > -1)
				{
					char[] array = attributes.GetValue(index2).ToCharArray();
					this.AppendCharacters(this.stack[this.currentPtr].node, array, 0, array.Length);
				}
				else
				{
					this.AppendIsindexPrompt(this.stack[this.currentPtr].node);
				}
				HtmlAttributes htmlAttributes2 = new HtmlAttributes(0);
				htmlAttributes2.AddAttribute(AttributeName.NAME, "isindex", XmlViolationPolicy.Allow);
				for (int i = 0; i < attributes.Length; i++)
				{
					AttributeName attributeName = attributes.GetAttributeName(i);
					if (!(AttributeName.NAME == attributeName) && !(AttributeName.PROMPT == attributeName) && AttributeName.ACTION != attributeName)
					{
						htmlAttributes2.AddAttribute(attributeName, attributes.GetValue(i), XmlViolationPolicy.Allow);
					}
				}
				attributes.ClearWithoutReleasingContents();
				this.AppendVoidElementToCurrentMayFoster("input", htmlAttributes2, this.formPointer);
				this.Pop();
				this.AppendVoidElementToCurrentMayFoster(ElementName.HR, HtmlAttributes.EMPTY_ATTRIBUTES);
				this.Pop();
				selfClosing = false;
				goto IL_1CCB;
			}
			goto IL_1CCB;
			IL_1003:
			this.AppendToCurrentNodeAndPushElementMayFoster(elementName, attributes, this.formPointer);
			this.tokenizer.SetStateAndEndTagExpectation(TokenizerState.RCDATA, elementName);
			this.originalMode = this.mode;
			this.mode = InsertionMode.TEXT;
			this.needToDropLF = true;
			attributes = null;
			goto IL_1CCB;
			IL_1045:
			this.ImplicitlyCloseP();
			this.ReconstructTheActiveFormattingElements();
			this.AppendToCurrentNodeAndPushElementMayFoster(elementName, attributes);
			this.originalMode = this.mode;
			this.mode = InsertionMode.TEXT;
			this.tokenizer.SetStateAndEndTagExpectation(TokenizerState.RAWTEXT, elementName);
			attributes = null;
			goto IL_1CCB;
			IL_1082:
			if (!this.IsScriptingEnabled)
			{
				this.ReconstructTheActiveFormattingElements();
				this.AppendToCurrentNodeAndPushElementMayFoster(elementName, attributes);
				attributes = null;
				goto IL_1CCB;
			}
			IL_10A0:
			this.AppendToCurrentNodeAndPushElementMayFoster(elementName, attributes);
			this.originalMode = this.mode;
			this.mode = InsertionMode.TEXT;
			this.tokenizer.SetStateAndEndTagExpectation(TokenizerState.RAWTEXT, elementName);
			attributes = null;
			goto IL_1CCB;
			IL_10D1:
			this.ReconstructTheActiveFormattingElements();
			this.AppendToCurrentNodeAndPushElementMayFoster(elementName, attributes, this.formPointer);
			switch (this.mode)
			{
			case InsertionMode.IN_TABLE:
			case InsertionMode.IN_CAPTION:
			case InsertionMode.IN_COLUMN_GROUP:
			case InsertionMode.IN_TABLE_BODY:
			case InsertionMode.IN_ROW:
			case InsertionMode.IN_CELL:
				this.mode = InsertionMode.IN_SELECT_IN_TABLE;
				break;
			default:
				this.mode = InsertionMode.IN_SELECT;
				break;
			}
			attributes = null;
			goto IL_1CCB;
			IL_112A:
			if (this.IsCurrent("option"))
			{
				this.Pop();
			}
			this.ReconstructTheActiveFormattingElements();
			this.AppendToCurrentNodeAndPushElementMayFoster(elementName, attributes);
			attributes = null;
			goto IL_1CCB;
			IL_1153:
			num = this.FindLastInScope("ruby");
			if (num != 2147483647)
			{
				this.GenerateImpliedEndTags();
			}
			if (num != this.currentPtr && this.ErrorEvent != null)
			{
				if (num != 2147483647)
				{
					this.Err("Start tag “" + name + "” seen without a “ruby” element being open.");
				}
				else
				{
					this.Err("Unclosed children in “ruby”.");
				}
			}
			this.AppendToCurrentNodeAndPushElementMayFoster(elementName, attributes);
			attributes = null;
			goto IL_1CCB;
			IL_11BA:
			this.ReconstructTheActiveFormattingElements();
			attributes.AdjustForMath();
			if (selfClosing)
			{
				this.AppendVoidElementToCurrentMayFosterMathML(elementName, attributes);
				selfClosing = false;
			}
			else
			{
				this.AppendToCurrentNodeAndPushElementMayFosterMathML(elementName, attributes);
			}
			attributes = null;
			goto IL_1CCB;
			IL_11E6:
			this.ReconstructTheActiveFormattingElements();
			attributes.AdjustForSvg();
			if (selfClosing)
			{
				this.AppendVoidElementToCurrentMayFosterSVG(elementName, attributes);
				selfClosing = false;
			}
			else
			{
				this.AppendToCurrentNodeAndPushElementMayFosterSVG(elementName, attributes);
			}
			attributes = null;
			goto IL_1CCB;
			IL_1212:
			this.ErrStrayStartTag(name);
			goto IL_1CCB;
			IL_121F:
			this.ReconstructTheActiveFormattingElements();
			this.AppendToCurrentNodeAndPushElementMayFoster(elementName, attributes, this.formPointer);
			attributes = null;
			goto IL_1CCB;
			IL_123B:
			this.ReconstructTheActiveFormattingElements();
			this.AppendToCurrentNodeAndPushElementMayFoster(elementName, attributes);
			attributes = null;
			goto IL_1CCB;
			IL_12C5:
			this.ErrStrayStartTag(name);
			if (!this.fragment)
			{
				this.AddAttributesToHtml(attributes);
				attributes = null;
				goto IL_1CCB;
			}
			goto IL_1CCB;
			IL_12E7:
			this.AppendVoidElementToCurrentMayFoster(elementName, attributes);
			selfClosing = false;
			attributes = null;
			goto IL_1CCB;
			IL_12FA:
			this.AppendToCurrentNodeAndPushElementMayFoster(elementName, attributes);
			this.originalMode = this.mode;
			this.mode = InsertionMode.TEXT;
			this.tokenizer.SetStateAndEndTagExpectation(TokenizerState.RCDATA, elementName);
			attributes = null;
			goto IL_1CCB;
			IL_132F:
			if (this.IsScriptingEnabled)
			{
				this.AppendToCurrentNodeAndPushElement(elementName, attributes);
				this.originalMode = this.mode;
				this.mode = InsertionMode.TEXT;
				this.tokenizer.SetStateAndEndTagExpectation(TokenizerState.RAWTEXT, elementName);
			}
			else
			{
				this.AppendToCurrentNodeAndPushElementMayFoster(elementName, attributes);
				this.mode = InsertionMode.IN_HEAD_NOSCRIPT;
			}
			attributes = null;
			goto IL_1CCB;
			IL_1379:
			this.AppendToCurrentNodeAndPushElementMayFoster(elementName, attributes);
			this.originalMode = this.mode;
			this.mode = InsertionMode.TEXT;
			this.tokenizer.SetStateAndEndTagExpectation(TokenizerState.SCRIPT_DATA, elementName);
			attributes = null;
			goto IL_1CCB;
			IL_13AA:
			this.AppendToCurrentNodeAndPushElementMayFoster(elementName, attributes);
			this.originalMode = this.mode;
			this.mode = InsertionMode.TEXT;
			this.tokenizer.SetStateAndEndTagExpectation(TokenizerState.RAWTEXT, elementName);
			attributes = null;
			goto IL_1CCB;
			IL_13DB:
			this.Err("Start tag for “head” seen when “head” was already open.");
			goto IL_1CCB;
			IL_1442:
			this.ErrStrayStartTag(name);
			if (!this.fragment)
			{
				this.AddAttributesToHtml(attributes);
				attributes = null;
				goto IL_1CCB;
			}
			goto IL_1CCB;
			IL_1464:
			this.AppendVoidElementToCurrentMayFoster(elementName, attributes);
			selfClosing = false;
			attributes = null;
			goto IL_1CCB;
			IL_1477:
			this.CheckMetaCharset(attributes);
			this.AppendVoidElementToCurrentMayFoster(elementName, attributes);
			selfClosing = false;
			attributes = null;
			goto IL_1CCB;
			IL_1491:
			this.AppendToCurrentNodeAndPushElement(elementName, attributes);
			this.originalMode = this.mode;
			this.mode = InsertionMode.TEXT;
			this.tokenizer.SetStateAndEndTagExpectation(TokenizerState.RAWTEXT, elementName);
			attributes = null;
			goto IL_1CCB;
			IL_14C2:
			this.Err("Start tag for “head” seen when “head” was already open.");
			goto IL_1CCB;
			IL_14D2:
			this.Err("Start tag for “noscript” seen when “noscript” was already open.");
			goto IL_1CCB;
			Block_111:
			this.ErrStrayStartTag(name);
			if (!this.fragment)
			{
				this.AddAttributesToHtml(attributes);
				attributes = null;
				goto IL_1CCB;
			}
			goto IL_1CCB;
			IL_153C:
			this.AppendVoidElementToCurrentMayFoster(elementName, attributes);
			selfClosing = false;
			attributes = null;
			goto IL_1CCB;
			Block_113:
			this.Err("Garbage in “colgroup” fragment.");
			Block_116:
			goto IL_1CCB;
			Block_119:
			if (dispatchGroup17 == DispatchGroup.HTML)
			{
				this.ErrStrayStartTag(name);
				if (!this.fragment)
				{
					this.AddAttributesToHtml(attributes);
					attributes = null;
					goto IL_1CCB;
				}
				goto IL_1CCB;
			}
			Block_122:
			goto IL_178A;
			IL_1670:
			if (this.IsCurrent("option"))
			{
				this.Pop();
			}
			this.AppendToCurrentNodeAndPushElement(elementName, attributes);
			attributes = null;
			goto IL_1CCB;
			IL_1693:
			if (this.IsCurrent("option"))
			{
				this.Pop();
			}
			if (this.IsCurrent("optgroup"))
			{
				this.Pop();
			}
			this.AppendToCurrentNodeAndPushElement(elementName, attributes);
			attributes = null;
			goto IL_1CCB;
			IL_16C9:
			this.Err("“select” start tag where end tag expected.");
			num = this.FindLastInTableScope(name);
			if (num == 2147483647)
			{
				this.Err("No “select” in table scope.");
			}
			else
			{
				while (this.currentPtr >= num)
				{
					this.Pop();
				}
				this.ResetTheInsertionMode();
			}
			Block_129:
			goto IL_1CCB;
			IL_1759:
			this.AppendToCurrentNodeAndPushElementMayFoster(elementName, attributes);
			this.originalMode = this.mode;
			this.mode = InsertionMode.TEXT;
			this.tokenizer.SetStateAndEndTagExpectation(TokenizerState.SCRIPT_DATA, elementName);
			attributes = null;
			goto IL_1CCB;
			IL_178A:
			this.ErrStrayStartTag(name);
			goto IL_1CCB;
			Block_131:
			this.ErrStrayStartTag(name);
			if (!this.fragment)
			{
				this.AddAttributesToHtml(attributes);
				attributes = null;
				goto IL_1CCB;
			}
			goto IL_1CCB;
			IL_17E3:
			switch (group)
			{
			case DispatchGroup.FRAME:
				this.AppendVoidElementToCurrentMayFoster(elementName, attributes);
				selfClosing = false;
				attributes = null;
				goto IL_1CCB;
			case DispatchGroup.FRAMESET:
				this.AppendToCurrentNodeAndPushElement(elementName, attributes);
				attributes = null;
				goto IL_1CCB;
			}
			IL_181E:
			switch (group)
			{
			case DispatchGroup.HTML:
				this.ErrStrayStartTag(name);
				if (!this.fragment)
				{
					this.AddAttributesToHtml(attributes);
					attributes = null;
					goto IL_1CCB;
				}
				goto IL_1CCB;
			case DispatchGroup.NOFRAMES:
				this.AppendToCurrentNodeAndPushElement(elementName, attributes);
				this.originalMode = this.mode;
				this.mode = InsertionMode.TEXT;
				this.tokenizer.SetStateAndEndTagExpectation(TokenizerState.RAWTEXT, elementName);
				attributes = null;
				goto IL_1CCB;
			}
			this.ErrStrayStartTag(name);
			goto IL_1CCB;
			Block_138:
			if (attributes == HtmlAttributes.EMPTY_ATTRIBUTES)
			{
				this.AppendHtmlElementToDocumentAndPush();
			}
			else
			{
				this.AppendHtmlElementToDocumentAndPush(attributes);
			}
			this.mode = InsertionMode.BEFORE_HEAD;
			attributes = null;
			goto IL_1CCB;
			Block_141:
			this.ErrStrayStartTag(name);
			if (!this.fragment)
			{
				this.AddAttributesToHtml(attributes);
				attributes = null;
				goto IL_1CCB;
			}
			goto IL_1CCB;
			IL_197B:
			this.AppendToCurrentNodeAndPushHeadElement(attributes);
			this.mode = InsertionMode.IN_HEAD;
			attributes = null;
			goto IL_1CCB;
			IL_1A29:
			this.ErrStrayStartTag(name);
			if (!this.fragment)
			{
				this.AddAttributesToHtml(attributes);
				attributes = null;
				goto IL_1CCB;
			}
			goto IL_1CCB;
			IL_1A4B:
			if (attributes.Length == 0)
			{
				this.AppendToCurrentNodeAndPushBodyElement();
			}
			else
			{
				this.AppendToCurrentNodeAndPushBodyElement(attributes);
			}
			this.framesetOk = false;
			this.mode = InsertionMode.IN_BODY;
			attributes = null;
			goto IL_1CCB;
			IL_1A78:
			this.AppendToCurrentNodeAndPushElement(elementName, attributes);
			this.mode = InsertionMode.IN_FRAMESET;
			attributes = null;
			goto IL_1CCB;
			IL_1A90:
			this.Err("“base” element outside “head”.");
			this.PushHeadPointerOntoStack();
			this.AppendVoidElementToCurrentMayFoster(elementName, attributes);
			selfClosing = false;
			this.Pop();
			attributes = null;
			goto IL_1CCB;
			IL_1ABA:
			this.Err("“link” element outside “head”.");
			this.PushHeadPointerOntoStack();
			this.AppendVoidElementToCurrentMayFoster(elementName, attributes);
			selfClosing = false;
			this.Pop();
			attributes = null;
			goto IL_1CCB;
			IL_1AE4:
			this.Err("“meta” element outside “head”.");
			this.CheckMetaCharset(attributes);
			this.PushHeadPointerOntoStack();
			this.AppendVoidElementToCurrentMayFoster(elementName, attributes);
			selfClosing = false;
			this.Pop();
			attributes = null;
			goto IL_1CCB;
			IL_1B15:
			this.Err("“script” element between “head” and “body”.");
			this.PushHeadPointerOntoStack();
			this.AppendToCurrentNodeAndPushElement(elementName, attributes);
			this.originalMode = this.mode;
			this.mode = InsertionMode.TEXT;
			this.tokenizer.SetStateAndEndTagExpectation(TokenizerState.SCRIPT_DATA, elementName);
			attributes = null;
			goto IL_1CCB;
			IL_1B57:
			this.Err("“" + name + "” element between “head” and “body”.");
			this.PushHeadPointerOntoStack();
			this.AppendToCurrentNodeAndPushElement(elementName, attributes);
			this.originalMode = this.mode;
			this.mode = InsertionMode.TEXT;
			this.tokenizer.SetStateAndEndTagExpectation(TokenizerState.RAWTEXT, elementName);
			attributes = null;
			goto IL_1CCB;
			IL_1BA5:
			this.Err("“title” element outside “head”.");
			this.PushHeadPointerOntoStack();
			this.AppendToCurrentNodeAndPushElement(elementName, attributes);
			this.originalMode = this.mode;
			this.mode = InsertionMode.TEXT;
			this.tokenizer.SetStateAndEndTagExpectation(TokenizerState.RCDATA, elementName);
			attributes = null;
			goto IL_1CCB;
			IL_1BEB:
			this.ErrStrayStartTag(name);
			goto IL_1CCB;
			Block_152:
			this.ErrStrayStartTag(name);
			if (!this.fragment)
			{
				this.AddAttributesToHtml(attributes);
				attributes = null;
				goto IL_1CCB;
			}
			goto IL_1CCB;
			IL_1C5D:
			switch (group)
			{
			case DispatchGroup.HTML:
				this.ErrStrayStartTag(name);
				if (!this.fragment)
				{
					this.AddAttributesToHtml(attributes);
					attributes = null;
					goto IL_1CCB;
				}
				goto IL_1CCB;
			case DispatchGroup.NOFRAMES:
				this.AppendToCurrentNodeAndPushElementMayFoster(elementName, attributes);
				this.originalMode = this.mode;
				this.mode = InsertionMode.TEXT;
				this.tokenizer.SetStateAndEndTagExpectation(TokenizerState.SCRIPT_DATA, elementName);
				attributes = null;
				goto IL_1CCB;
			}
			this.ErrStrayStartTag(name);
			IL_1CCB:
			if (selfClosing)
			{
				if (this.AllowSelfClosingTags)
				{
					this.EndTag(elementName);
					return;
				}
				if (this.ErrorEvent != null)
				{
					this.Err("Self-closing syntax (“/>”) used on a non-void HTML element. Ignoring the slash and treating as a start tag.");
				}
			}
		}

		bool IsSpecialParentInForeign(StackNode<T> stackNode)
		{
			string ns = stackNode.ns;
			return "http://www.w3.org/1999/xhtml" == ns || stackNode.IsHtmlIntegrationPoint || ("http://www.w3.org/1998/Math/MathML" == ns && stackNode.Group == DispatchGroup.MI_MO_MN_MS_MTEXT);
		}

		public static string ExtractCharsetFromContent(string attributeValue)
		{
			CharsetState charsetState = CharsetState.CHARSET_INITIAL;
			int num = -1;
			int num2 = -1;
			char[] array = attributeValue.ToCharArray();
			for (int i = 0; i < array.Length; i++)
			{
				char c = array[i];
				switch (charsetState)
				{
				case CharsetState.CHARSET_INITIAL:
				{
					char c2 = c;
					if (c2 == 'C' || c2 == 'c')
					{
						charsetState = CharsetState.CHARSET_C;
					}
					break;
				}
				case CharsetState.CHARSET_C:
				{
					char c3 = c;
					if (c3 == 'H' || c3 == 'h')
					{
						charsetState = CharsetState.CHARSET_H;
					}
					else
					{
						charsetState = CharsetState.CHARSET_INITIAL;
					}
					break;
				}
				case CharsetState.CHARSET_H:
				{
					char c4 = c;
					if (c4 == 'A' || c4 == 'a')
					{
						charsetState = CharsetState.CHARSET_A;
					}
					else
					{
						charsetState = CharsetState.CHARSET_INITIAL;
					}
					break;
				}
				case CharsetState.CHARSET_A:
				{
					char c5 = c;
					if (c5 == 'R' || c5 == 'r')
					{
						charsetState = CharsetState.CHARSET_R;
					}
					else
					{
						charsetState = CharsetState.CHARSET_INITIAL;
					}
					break;
				}
				case CharsetState.CHARSET_R:
				{
					char c6 = c;
					if (c6 == 'S' || c6 == 's')
					{
						charsetState = CharsetState.CHARSET_S;
					}
					else
					{
						charsetState = CharsetState.CHARSET_INITIAL;
					}
					break;
				}
				case CharsetState.CHARSET_S:
				{
					char c7 = c;
					if (c7 == 'E' || c7 == 'e')
					{
						charsetState = CharsetState.CHARSET_E;
					}
					else
					{
						charsetState = CharsetState.CHARSET_INITIAL;
					}
					break;
				}
				case CharsetState.CHARSET_E:
				{
					char c8 = c;
					if (c8 == 'T' || c8 == 't')
					{
						charsetState = CharsetState.CHARSET_T;
					}
					else
					{
						charsetState = CharsetState.CHARSET_INITIAL;
					}
					break;
				}
				case CharsetState.CHARSET_T:
				{
					char c9 = c;
					switch (c9)
					{
					case '\t':
					case '\n':
					case '\f':
					case '\r':
						goto IL_20D;
					case '\v':
						break;
					default:
						if (c9 == ' ')
						{
							goto IL_20D;
						}
						if (c9 == '=')
						{
							charsetState = CharsetState.CHARSET_EQUALS;
							goto IL_20D;
						}
						break;
					}
					return null;
				}
				case CharsetState.CHARSET_EQUALS:
				{
					char c10 = c;
					switch (c10)
					{
					case '\t':
					case '\n':
					case '\f':
					case '\r':
						goto IL_20D;
					case '\v':
						break;
					default:
						switch (c10)
						{
						case ' ':
							goto IL_20D;
						case '!':
							break;
						case '"':
							num = i + 1;
							charsetState = CharsetState.CHARSET_DOUBLE_QUOTED;
							goto IL_20D;
						default:
							if (c10 == '\'')
							{
								num = i + 1;
								charsetState = CharsetState.CHARSET_SINGLE_QUOTED;
								goto IL_20D;
							}
							break;
						}
						break;
					}
					num = i;
					charsetState = CharsetState.CHARSET_UNQUOTED;
					break;
				}
				case CharsetState.CHARSET_SINGLE_QUOTED:
				{
					char c11 = c;
					if (c11 == '\'')
					{
						num2 = i;
						goto IL_21D;
					}
					break;
				}
				case CharsetState.CHARSET_DOUBLE_QUOTED:
				{
					char c12 = c;
					if (c12 == '"')
					{
						num2 = i;
						goto IL_21D;
					}
					break;
				}
				case CharsetState.CHARSET_UNQUOTED:
				{
					char c13 = c;
					switch (c13)
					{
					case '\t':
					case '\n':
					case '\f':
					case '\r':
						break;
					case '\v':
						goto IL_20D;
					default:
						if (c13 != ' ' && c13 != ';')
						{
							goto IL_20D;
						}
						break;
					}
					num2 = i;
					goto IL_21D;
				}
				}
				IL_20D:;
			}
			IL_21D:
			string result = null;
			if (num != -1)
			{
				if (num2 == -1)
				{
					num2 = array.Length;
				}
				result = new string(array, num, num2 - num);
			}
			return result;
		}

		void CheckMetaCharset(HtmlAttributes attributes)
		{
			string value = attributes.GetValue(AttributeName.CHARSET);
			if (value != null)
			{
				if (this.tokenizer.InternalEncodingDeclaration(value))
				{
					this.RequestSuspension();
				}
				return;
			}
			if (!Portability.LowerCaseLiteralEqualsIgnoreAsciiCaseString("content-type", attributes.GetValue(AttributeName.HTTP_EQUIV)))
			{
				return;
			}
			string value2 = attributes.GetValue(AttributeName.CONTENT);
			if (value2 != null)
			{
				string text = TreeBuilder<T>.ExtractCharsetFromContent(value2);
				if (text != null && this.tokenizer.InternalEncodingDeclaration(text))
				{
					this.RequestSuspension();
				}
			}
		}

		public void EndTag(ElementName elementName)
		{
			this.FlushCharacters();
			this.needToDropLF = false;
			DispatchGroup group = elementName.Group;
			string name = elementName.name;
			int num;
			DispatchGroup dispatchGroup4;
			for (;;)
			{
				if (this.IsInForeign)
				{
					if (this.ErrorEvent != null && this.stack[this.currentPtr].name != name)
					{
						this.Err(string.Concat(new string[]
						{
							"End tag “",
							name,
							"” did not match the name of the current open element (“",
							this.stack[this.currentPtr].popName,
							"”)."
						}));
					}
					num = this.currentPtr;
					while (!(this.stack[num].name == name))
					{
						if (this.stack[--num].ns == "http://www.w3.org/1999/xhtml")
						{
							goto IL_DE;
						}
					}
					break;
				}
				IL_DE:
				switch (this.mode)
				{
				case InsertionMode.INITIAL:
					switch (this.DoctypeExpectation)
					{
					case DoctypeExpectation.Html:
						this.Err("End tag seen without seeing a doctype first. Expected “<!DOCTYPE html>”.");
						break;
					case DoctypeExpectation.Html401Transitional:
						this.Err("End tag seen without seeing a doctype first. Expected “<!DOCTYPE HTML PUBLIC \"-//W3C//DTD HTML 4.01 Transitional//EN\" \"http://www.w3.org/TR/html4/loose.dtd\">”.");
						break;
					case DoctypeExpectation.Html401Strict:
						this.Err("End tag seen without seeing a doctype first. Expected “<!DOCTYPE HTML PUBLIC \"-//W3C//DTD HTML 4.01//EN\" \"http://www.w3.org/TR/html4/strict.dtd\">”.");
						break;
					case DoctypeExpectation.Auto:
						this.Err("End tag seen without seeing a doctype first. Expected e.g. “<!DOCTYPE html>”.");
						break;
					}
					this.DocumentModeInternal(DocumentMode.QuirksMode, null, null, false);
					this.mode = InsertionMode.BEFORE_HTML;
					continue;
				case InsertionMode.BEFORE_HTML:
				{
					DispatchGroup dispatchGroup = group;
					switch (dispatchGroup)
					{
					case DispatchGroup.BODY:
					case DispatchGroup.BR:
						break;
					default:
						if (dispatchGroup != DispatchGroup.HEAD && dispatchGroup != DispatchGroup.HTML)
						{
							goto IL_E5B;
						}
						break;
					}
					this.AppendHtmlElementToDocumentAndPush();
					this.mode = InsertionMode.BEFORE_HEAD;
					continue;
				}
				case InsertionMode.BEFORE_HEAD:
				{
					DispatchGroup dispatchGroup2 = group;
					switch (dispatchGroup2)
					{
					case DispatchGroup.BODY:
					case DispatchGroup.BR:
						break;
					default:
						if (dispatchGroup2 != DispatchGroup.HEAD && dispatchGroup2 != DispatchGroup.HTML)
						{
							goto IL_E9A;
						}
						break;
					}
					this.AppendToCurrentNodeAndPushHeadElement(HtmlAttributes.EMPTY_ATTRIBUTES);
					this.mode = InsertionMode.IN_HEAD;
					continue;
				}
				case InsertionMode.IN_HEAD:
				{
					DispatchGroup dispatchGroup3 = group;
					switch (dispatchGroup3)
					{
					case DispatchGroup.BODY:
					case DispatchGroup.BR:
						break;
					default:
						if (dispatchGroup3 == DispatchGroup.HEAD)
						{
							goto IL_EC4;
						}
						if (dispatchGroup3 != DispatchGroup.HTML)
						{
							goto Block_126;
						}
						break;
					}
					this.Pop();
					this.mode = InsertionMode.AFTER_HEAD;
					continue;
				}
				case InsertionMode.IN_HEAD_NOSCRIPT:
					dispatchGroup4 = group;
					if (dispatchGroup4 != DispatchGroup.BR)
					{
						goto Block_127;
					}
					this.ErrStrayEndTag(name);
					this.Pop();
					this.mode = InsertionMode.IN_HEAD;
					continue;
				case InsertionMode.AFTER_HEAD:
				{
					DispatchGroup dispatchGroup5 = group;
					switch (dispatchGroup5)
					{
					case DispatchGroup.BODY:
					case DispatchGroup.BR:
						break;
					default:
						if (dispatchGroup5 != DispatchGroup.HTML)
						{
							goto IL_F56;
						}
						break;
					}
					this.AppendToCurrentNodeAndPushBodyElement();
					this.mode = InsertionMode.FRAMESET_OK;
					continue;
				}
				case InsertionMode.IN_BODY:
				case InsertionMode.FRAMESET_OK:
					goto IL_585;
				case InsertionMode.IN_TABLE:
					goto IL_2FF;
				case InsertionMode.IN_CAPTION:
					goto IL_392;
				case InsertionMode.IN_COLUMN_GROUP:
					switch (group)
					{
					case DispatchGroup.COL:
						goto IL_B99;
					case DispatchGroup.COLGROUP:
						goto IL_B77;
					default:
						if (this.currentPtr == 0)
						{
							goto Block_97;
						}
						this.Pop();
						this.mode = InsertionMode.IN_TABLE;
						continue;
					}
					break;
				case InsertionMode.IN_TABLE_BODY:
					break;
				case InsertionMode.IN_ROW:
				{
					DispatchGroup dispatchGroup6 = group;
					switch (dispatchGroup6)
					{
					case DispatchGroup.BODY:
					case DispatchGroup.CAPTION:
					case DispatchGroup.COL:
					case DispatchGroup.COLGROUP:
						goto IL_24A;
					case DispatchGroup.BR:
					case DispatchGroup.BUTTON:
						break;
					default:
						if (dispatchGroup6 == DispatchGroup.HTML)
						{
							goto IL_24A;
						}
						switch (dispatchGroup6)
						{
						case DispatchGroup.TABLE:
							num = this.FindLastOrRoot(DispatchGroup.TR);
							if (num == 0)
							{
								goto Block_11;
							}
							this.ClearStackBackTo(num);
							this.Pop();
							this.mode = InsertionMode.IN_TABLE_BODY;
							continue;
						case DispatchGroup.TR:
							goto IL_1A2;
						case DispatchGroup.TBODY_OR_THEAD_OR_TFOOT:
							if (this.FindLastInTableScope(name) == 2147483647)
							{
								goto Block_12;
							}
							num = this.FindLastOrRoot(DispatchGroup.TR);
							if (num == 0)
							{
								goto Block_13;
							}
							this.ClearStackBackTo(num);
							this.Pop();
							this.mode = InsertionMode.IN_TABLE_BODY;
							continue;
						case DispatchGroup.TD_OR_TH:
							goto IL_24A;
						}
						break;
					}
					break;
				}
				case InsertionMode.IN_CELL:
					goto IL_4A7;
				case InsertionMode.IN_SELECT:
					goto IL_C4D;
				case InsertionMode.IN_SELECT_IN_TABLE:
				{
					DispatchGroup dispatchGroup7 = group;
					if (dispatchGroup7 != DispatchGroup.CAPTION)
					{
						switch (dispatchGroup7)
						{
						case DispatchGroup.TABLE:
						case DispatchGroup.TR:
						case DispatchGroup.TBODY_OR_THEAD_OR_TFOOT:
						case DispatchGroup.TD_OR_TH:
							goto IL_BF7;
						}
						goto Block_99;
					}
					IL_BF7:
					this.Err("“" + name + "” end tag with “select” open.");
					if (this.FindLastInTableScope(name) == 2147483647)
					{
						return;
					}
					num = this.FindLastInTableScope("select");
					if (num == 2147483647)
					{
						return;
					}
					while (this.currentPtr >= num)
					{
						this.Pop();
					}
					this.ResetTheInsertionMode();
					continue;
				}
				case InsertionMode.AFTER_BODY:
				{
					DispatchGroup dispatchGroup8 = group;
					if (dispatchGroup8 == DispatchGroup.HTML)
					{
						goto Block_111;
					}
					this.Err("Saw an end tag after “body” had been closed.");
					this.mode = (this.framesetOk ? InsertionMode.FRAMESET_OK : InsertionMode.IN_BODY);
					continue;
				}
				case InsertionMode.IN_FRAMESET:
					goto IL_D57;
				case InsertionMode.AFTER_FRAMESET:
					goto IL_DA2;
				case InsertionMode.AFTER_AFTER_BODY:
					this.ErrStrayEndTag(name);
					this.mode = (this.framesetOk ? InsertionMode.FRAMESET_OK : InsertionMode.IN_BODY);
					continue;
				case InsertionMode.AFTER_AFTER_FRAMESET:
					this.ErrStrayEndTag(name);
					this.mode = InsertionMode.IN_FRAMESET;
					continue;
				case InsertionMode.TEXT:
					goto IL_F91;
				default:
					continue;
				}
				DispatchGroup dispatchGroup9 = group;
				switch (dispatchGroup9)
				{
				case DispatchGroup.BODY:
				case DispatchGroup.CAPTION:
				case DispatchGroup.COL:
				case DispatchGroup.COLGROUP:
					goto IL_2F7;
				case DispatchGroup.BR:
				case DispatchGroup.BUTTON:
					goto IL_2FF;
				default:
					if (dispatchGroup9 == DispatchGroup.HTML)
					{
						goto IL_2F7;
					}
					switch (dispatchGroup9)
					{
					case DispatchGroup.TABLE:
						num = this.FindLastInTableScopeOrRootTbodyTheadTfoot();
						if (num == 0)
						{
							goto Block_18;
						}
						this.ClearStackBackTo(num);
						this.Pop();
						this.mode = InsertionMode.IN_TABLE;
						continue;
					case DispatchGroup.TEXTAREA:
					case DispatchGroup.TITLE:
					case DispatchGroup.XMP:
						goto IL_2FF;
					case DispatchGroup.TR:
					case DispatchGroup.TD_OR_TH:
						goto IL_2F7;
					case DispatchGroup.TBODY_OR_THEAD_OR_TFOOT:
						goto IL_2A4;
					default:
						goto IL_2FF;
					}
					break;
				}
				IL_392:
				DispatchGroup dispatchGroup10 = group;
				switch (dispatchGroup10)
				{
				case DispatchGroup.BODY:
				case DispatchGroup.COL:
				case DispatchGroup.COLGROUP:
					goto IL_49F;
				case DispatchGroup.BR:
				case DispatchGroup.BUTTON:
					goto IL_4A7;
				case DispatchGroup.CAPTION:
					goto IL_3EA;
				default:
					if (dispatchGroup10 == DispatchGroup.HTML)
					{
						goto IL_49F;
					}
					switch (dispatchGroup10)
					{
					case DispatchGroup.TABLE:
						this.Err("“table” closed but “caption” was still open.");
						num = this.FindLastInTableScope("caption");
						if (num == 2147483647)
						{
							return;
						}
						this.GenerateImpliedEndTags();
						if (this.ErrorEvent != null && this.currentPtr != num)
						{
							this.ErrUnclosedElements(num, name);
						}
						while (this.currentPtr >= num)
						{
							this.Pop();
						}
						this.ClearTheListOfActiveFormattingElementsUpToTheLastMarker();
						this.mode = InsertionMode.IN_TABLE;
						continue;
					case DispatchGroup.TEXTAREA:
					case DispatchGroup.TITLE:
					case DispatchGroup.XMP:
						goto IL_4A7;
					case DispatchGroup.TR:
					case DispatchGroup.TBODY_OR_THEAD_OR_TFOOT:
					case DispatchGroup.TD_OR_TH:
						goto IL_49F;
					default:
						goto IL_4A7;
					}
					break;
				}
				IL_2FF:
				DispatchGroup dispatchGroup11 = group;
				switch (dispatchGroup11)
				{
				case DispatchGroup.BODY:
				case DispatchGroup.CAPTION:
				case DispatchGroup.COL:
				case DispatchGroup.COLGROUP:
					goto IL_383;
				case DispatchGroup.BR:
				case DispatchGroup.BUTTON:
					break;
				default:
					if (dispatchGroup11 == DispatchGroup.HTML)
					{
						goto IL_383;
					}
					switch (dispatchGroup11)
					{
					case DispatchGroup.TABLE:
						goto IL_351;
					case DispatchGroup.TR:
					case DispatchGroup.TBODY_OR_THEAD_OR_TFOOT:
					case DispatchGroup.TD_OR_TH:
						goto IL_383;
					}
					break;
				}
				this.ErrStrayEndTag(name);
				goto IL_392;
				IL_4A7:
				DispatchGroup dispatchGroup12 = group;
				switch (dispatchGroup12)
				{
				case DispatchGroup.BODY:
				case DispatchGroup.CAPTION:
				case DispatchGroup.COL:
				case DispatchGroup.COLGROUP:
					goto IL_57D;
				case DispatchGroup.BR:
				case DispatchGroup.BUTTON:
					break;
				default:
					if (dispatchGroup12 == DispatchGroup.HTML)
					{
						goto IL_57D;
					}
					switch (dispatchGroup12)
					{
					case DispatchGroup.TABLE:
					case DispatchGroup.TR:
					case DispatchGroup.TBODY_OR_THEAD_OR_TFOOT:
						if (this.FindLastInTableScope(name) == 2147483647)
						{
							goto Block_42;
						}
						this.CloseTheCell(this.FindLastInTableScopeTdTh());
						continue;
					case DispatchGroup.TD_OR_TH:
						goto IL_4FF;
					}
					break;
				}
				IL_585:
				switch (group)
				{
				case DispatchGroup.A:
				case DispatchGroup.NOBR:
				case DispatchGroup.B_OR_BIG_OR_CODE_OR_EM_OR_I_OR_S_OR_SMALL_OR_STRIKE_OR_STRONG_OR_TT_OR_U:
				case DispatchGroup.FONT:
					goto IL_ADF;
				case DispatchGroup.BODY:
					goto IL_69A;
				case DispatchGroup.BR:
					goto IL_A65;
				case DispatchGroup.BUTTON:
				case DispatchGroup.PRE_OR_LISTING:
				case DispatchGroup.UL_OR_OL_OR_DL:
				case DispatchGroup.DIV_OR_BLOCKQUOTE_OR_CENTER_OR_MENU:
				case DispatchGroup.ADDRESS_OR_ARTICLE_OR_ASIDE_OR_DETAILS_OR_DIR_OR_FIGCAPTION_OR_FIGURE_OR_FOOTER_OR_HEADER_OR_HGROUP_OR_NAV_OR_SECTION_OR_SUMMARY:
				case DispatchGroup.FIELDSET:
					goto IL_7B4;
				case DispatchGroup.FORM:
					goto IL_7FD;
				case DispatchGroup.IMAGE:
				case DispatchGroup.INPUT:
				case DispatchGroup.ISINDEX:
				case DispatchGroup.HR:
				case DispatchGroup.NOFRAMES:
				case DispatchGroup.SELECT:
				case DispatchGroup.TABLE:
				case DispatchGroup.TEXTAREA:
				case DispatchGroup.IFRAME:
				case DispatchGroup.EMBED_OR_IMG:
				case DispatchGroup.AREA_OR_WBR:
				case DispatchGroup.PARAM_OR_SOURCE_OR_TRACK:
				case DispatchGroup.NOEMBED:
				case DispatchGroup.KEYGEN:
					goto IL_AC7;
				case DispatchGroup.LI:
					goto IL_903;
				case DispatchGroup.HTML:
					if (!this.IsSecondOnStackBody())
					{
						goto Block_50;
					}
					if (this.ErrorEvent != null)
					{
						int i = 0;
						while (i <= this.currentPtr)
						{
							DispatchGroup group2 = this.stack[i].Group;
							if (group2 <= DispatchGroup.LI)
							{
								if (group2 != DispatchGroup.BODY && group2 != DispatchGroup.LI)
								{
									goto IL_78A;
								}
							}
							else if (group2 != DispatchGroup.HTML && group2 != DispatchGroup.P)
							{
								switch (group2)
								{
								case DispatchGroup.TBODY_OR_THEAD_OR_TFOOT:
								case DispatchGroup.TD_OR_TH:
								case DispatchGroup.DD_OR_DT:
									goto IL_797;
								}
								goto IL_78A;
							}
							IL_797:
							i++;
							continue;
							IL_78A:
							this.ErrEndWithUnclosedElements("End tag for “html” seen but there were unclosed elements.");
							break;
						}
					}
					this.mode = InsertionMode.AFTER_BODY;
					continue;
				case DispatchGroup.NOSCRIPT:
					goto IL_ACF;
				case DispatchGroup.P:
					goto IL_85D;
				case DispatchGroup.DD_OR_DT:
					goto IL_951;
				case DispatchGroup.H1_OR_H2_OR_H3_OR_H4_OR_H5_OR_H6:
					goto IL_9CE;
				case DispatchGroup.MARQUEE_OR_APPLET:
				case DispatchGroup.OBJECT:
					goto IL_A16;
				}
				goto Block_43;
			}
			while (this.currentPtr >= num)
			{
				this.Pop();
			}
			return;
			IL_1A2:
			num = this.FindLastOrRoot(DispatchGroup.TR);
			if (num == 0)
			{
				this.Err("No table row to close.");
				return;
			}
			this.ClearStackBackTo(num);
			this.Pop();
			this.mode = InsertionMode.IN_TABLE_BODY;
			return;
			Block_11:
			this.Err("No table row to close.");
			return;
			Block_12:
			this.ErrStrayEndTag(name);
			return;
			Block_13:
			this.Err("No table row to close.");
			return;
			IL_24A:
			this.ErrStrayEndTag(name);
			return;
			IL_2A4:
			num = this.FindLastOrRoot(name);
			if (num == 0)
			{
				this.ErrStrayEndTag(name);
				return;
			}
			this.ClearStackBackTo(num);
			this.Pop();
			this.mode = InsertionMode.IN_TABLE;
			return;
			Block_18:
			this.ErrStrayEndTag(name);
			return;
			IL_2F7:
			this.ErrStrayEndTag(name);
			return;
			IL_351:
			num = this.FindLast("table");
			if (num == 2147483647)
			{
				this.ErrStrayEndTag(name);
				return;
			}
			while (this.currentPtr >= num)
			{
				this.Pop();
			}
			this.ResetTheInsertionMode();
			return;
			IL_383:
			this.ErrStrayEndTag(name);
			return;
			IL_3EA:
			num = this.FindLastInTableScope("caption");
			if (num == 2147483647)
			{
				return;
			}
			this.GenerateImpliedEndTags();
			if (this.ErrorEvent != null && this.currentPtr != num)
			{
				this.ErrUnclosedElements(num, name);
			}
			while (this.currentPtr >= num)
			{
				this.Pop();
			}
			this.ClearTheListOfActiveFormattingElementsUpToTheLastMarker();
			this.mode = InsertionMode.IN_TABLE;
			return;
			IL_49F:
			this.ErrStrayEndTag(name);
			return;
			IL_4FF:
			num = this.FindLastInTableScope(name);
			if (num == 2147483647)
			{
				this.ErrStrayEndTag(name);
				return;
			}
			this.GenerateImpliedEndTags();
			if (this.ErrorEvent != null && !this.IsCurrent(name))
			{
				this.ErrUnclosedElements(num, name);
			}
			while (this.currentPtr >= num)
			{
				this.Pop();
			}
			this.ClearTheListOfActiveFormattingElementsUpToTheLastMarker();
			this.mode = InsertionMode.IN_ROW;
			return;
			Block_42:
			this.ErrStrayEndTag(name);
			return;
			IL_57D:
			this.ErrStrayEndTag(name);
			return;
			Block_43:
			goto IL_AE9;
			IL_69A:
			if (!this.IsSecondOnStackBody())
			{
				this.ErrStrayEndTag(name);
				return;
			}
			if (this.ErrorEvent != null)
			{
				int j = 2;
				while (j <= this.currentPtr)
				{
					DispatchGroup group3 = this.stack[j].Group;
					if (group3 <= DispatchGroup.P)
					{
						if (group3 != DispatchGroup.LI)
						{
							switch (group3)
							{
							case DispatchGroup.OPTGROUP:
							case DispatchGroup.OPTION:
							case DispatchGroup.P:
								break;
							default:
								goto IL_705;
							}
						}
					}
					else
					{
						switch (group3)
						{
						case DispatchGroup.TBODY_OR_THEAD_OR_TFOOT:
						case DispatchGroup.TD_OR_TH:
						case DispatchGroup.DD_OR_DT:
							break;
						default:
							if (group3 != DispatchGroup.RT_OR_RP)
							{
								goto IL_705;
							}
							break;
						}
					}
					j++;
					continue;
					IL_705:
					this.ErrEndWithUnclosedElements("End tag for “body” seen but there were unclosed elements.");
					break;
				}
			}
			this.mode = InsertionMode.AFTER_BODY;
			return;
			Block_50:
			this.ErrStrayEndTag(name);
			return;
			IL_7B4:
			num = this.FindLastInScope(name);
			if (num == 2147483647)
			{
				this.ErrStrayEndTag(name);
				return;
			}
			this.GenerateImpliedEndTags();
			if (this.ErrorEvent != null && !this.IsCurrent(name))
			{
				this.ErrUnclosedElements(num, name);
			}
			while (this.currentPtr >= num)
			{
				this.Pop();
			}
			return;
			IL_7FD:
			if (this.formPointer == null)
			{
				this.ErrStrayEndTag(name);
				return;
			}
			this.formPointer = default(T);
			num = this.FindLastInScope(name);
			if (num == 2147483647)
			{
				this.ErrStrayEndTag(name);
				return;
			}
			this.GenerateImpliedEndTags();
			if (this.ErrorEvent != null && !this.IsCurrent(name))
			{
				this.ErrUnclosedElements(num, name);
			}
			this.RemoveFromStack(num);
			return;
			IL_85D:
			num = this.FindLastInButtonScope("p");
			if (num == 2147483647)
			{
				this.Err("No “p” element in scope but a “p” end tag seen.");
				if (this.IsInForeign)
				{
					this.Err("HTML start tag “" + name + "” in a foreign namespace context.");
					while (this.stack[this.currentPtr].ns != "http://www.w3.org/1999/xhtml")
					{
						this.Pop();
					}
				}
				this.AppendVoidElementToCurrentMayFoster(elementName, HtmlAttributes.EMPTY_ATTRIBUTES);
				return;
			}
			this.GenerateImpliedEndTagsExceptFor("p");
			if (this.ErrorEvent != null && num != this.currentPtr)
			{
				this.ErrUnclosedElements(num, name);
			}
			while (this.currentPtr >= num)
			{
				this.Pop();
			}
			return;
			IL_903:
			num = this.FindLastInListScope(name);
			if (num == 2147483647)
			{
				this.Err("No “li” element in list scope but a “li” end tag seen.");
				return;
			}
			this.GenerateImpliedEndTagsExceptFor(name);
			if (this.ErrorEvent != null && num != this.currentPtr)
			{
				this.ErrUnclosedElements(num, name);
			}
			while (this.currentPtr >= num)
			{
				this.Pop();
			}
			return;
			IL_951:
			num = this.FindLastInScope(name);
			if (num == 2147483647)
			{
				this.Err(string.Concat(new string[] { "No “", name, "” element in scope but a “", name, "” end tag seen." }));
				return;
			}
			this.GenerateImpliedEndTagsExceptFor(name);
			if (this.ErrorEvent != null && num != this.currentPtr)
			{
				this.ErrUnclosedElements(num, name);
			}
			while (this.currentPtr >= num)
			{
				this.Pop();
			}
			return;
			IL_9CE:
			num = this.FindLastInScopeHn();
			if (num == 2147483647)
			{
				this.ErrStrayEndTag(name);
				return;
			}
			this.GenerateImpliedEndTags();
			if (this.ErrorEvent != null && !this.IsCurrent(name))
			{
				this.ErrUnclosedElements(num, name);
			}
			while (this.currentPtr >= num)
			{
				this.Pop();
			}
			return;
			IL_A16:
			num = this.FindLastInScope(name);
			if (num == 2147483647)
			{
				this.ErrStrayEndTag(name);
				return;
			}
			this.GenerateImpliedEndTags();
			if (this.ErrorEvent != null && !this.IsCurrent(name))
			{
				this.ErrUnclosedElements(num, name);
			}
			while (this.currentPtr >= num)
			{
				this.Pop();
			}
			this.ClearTheListOfActiveFormattingElementsUpToTheLastMarker();
			return;
			IL_A65:
			this.Err("End tag “br”.");
			if (this.IsInForeign)
			{
				this.Err("HTML start tag “" + name + "” in a foreign namespace context.");
				while (this.stack[this.currentPtr].ns != "http://www.w3.org/1999/xhtml")
				{
					this.Pop();
				}
			}
			this.ReconstructTheActiveFormattingElements();
			this.AppendVoidElementToCurrentMayFoster(elementName, HtmlAttributes.EMPTY_ATTRIBUTES);
			return;
			IL_AC7:
			this.ErrStrayEndTag(name);
			return;
			IL_ACF:
			if (this.IsScriptingEnabled)
			{
				this.ErrStrayEndTag(name);
				return;
			}
			IL_ADF:
			if (this.AdoptionAgencyEndTag(name))
			{
				return;
			}
			IL_AE9:
			if (this.IsCurrent(name))
			{
				this.Pop();
				return;
			}
			num = this.currentPtr;
			for (;;)
			{
				StackNode<T> stackNode = this.stack[num];
				if (stackNode.name == name)
				{
					break;
				}
				if (stackNode.IsSpecial)
				{
					goto Block_94;
				}
				num--;
			}
			this.GenerateImpliedEndTags();
			if (this.ErrorEvent != null && !this.IsCurrent(name))
			{
				this.ErrUnclosedElements(num, name);
			}
			while (this.currentPtr >= num)
			{
				this.Pop();
			}
			return;
			Block_94:
			this.ErrStrayEndTag(name);
			return;
			IL_B77:
			if (this.currentPtr == 0)
			{
				this.Err("Garbage in “colgroup” fragment.");
				return;
			}
			this.Pop();
			this.mode = InsertionMode.IN_TABLE;
			return;
			IL_B99:
			this.ErrStrayEndTag(name);
			return;
			Block_97:
			this.Err("Garbage in “colgroup” fragment.");
			return;
			Block_99:
			IL_C4D:
			DispatchGroup dispatchGroup13 = group;
			switch (dispatchGroup13)
			{
			case DispatchGroup.OPTGROUP:
				if (this.IsCurrent("option") && "optgroup" == this.stack[this.currentPtr - 1].name)
				{
					this.Pop();
				}
				if (this.IsCurrent("optgroup"))
				{
					this.Pop();
					return;
				}
				this.ErrStrayEndTag(name);
				return;
			case DispatchGroup.OPTION:
				if (this.IsCurrent("option"))
				{
					this.Pop();
					return;
				}
				this.ErrStrayEndTag(name);
				return;
			default:
				if (dispatchGroup13 != DispatchGroup.SELECT)
				{
					this.ErrStrayEndTag(name);
					return;
				}
				num = this.FindLastInTableScope("select");
				if (num == 2147483647)
				{
					this.ErrStrayEndTag(name);
					return;
				}
				while (this.currentPtr >= num)
				{
					this.Pop();
				}
				this.ResetTheInsertionMode();
				return;
			}
			Block_111:
			if (this.fragment)
			{
				this.ErrStrayEndTag(name);
				return;
			}
			this.mode = InsertionMode.AFTER_AFTER_BODY;
			return;
			IL_D57:
			DispatchGroup dispatchGroup14 = group;
			if (dispatchGroup14 != DispatchGroup.FRAMESET)
			{
				this.ErrStrayEndTag(name);
				return;
			}
			if (this.currentPtr == 0)
			{
				this.ErrStrayEndTag(name);
				return;
			}
			this.Pop();
			if (!this.fragment && !this.IsCurrent("frameset"))
			{
				this.mode = InsertionMode.AFTER_FRAMESET;
				return;
			}
			return;
			IL_DA2:
			DispatchGroup dispatchGroup15 = group;
			if (dispatchGroup15 == DispatchGroup.HTML)
			{
				this.mode = InsertionMode.AFTER_AFTER_FRAMESET;
				return;
			}
			this.ErrStrayEndTag(name);
			return;
			IL_E5B:
			this.ErrStrayEndTag(name);
			return;
			IL_E9A:
			this.ErrStrayEndTag(name);
			return;
			Block_126:
			this.ErrStrayEndTag(name);
			return;
			IL_EC4:
			this.Pop();
			this.mode = InsertionMode.AFTER_HEAD;
			return;
			Block_127:
			if (dispatchGroup4 == DispatchGroup.NOSCRIPT)
			{
				this.Pop();
				this.mode = InsertionMode.IN_HEAD;
				return;
			}
			this.ErrStrayEndTag(name);
			return;
			IL_F56:
			this.ErrStrayEndTag(name);
			return;
			IL_F91:
			this.Pop();
			if (this.originalMode == InsertionMode.AFTER_HEAD)
			{
				this.SilentPop();
			}
			this.mode = this.originalMode;
		}

		int FindLastInTableScopeOrRootTbodyTheadTfoot()
		{
			for (int i = this.currentPtr; i > 0; i--)
			{
				if (this.stack[i].Group == DispatchGroup.TBODY_OR_THEAD_OR_TFOOT)
				{
					return i;
				}
			}
			return 0;
		}

		int FindLast([Local] string name)
		{
			for (int i = this.currentPtr; i > 0; i--)
			{
				if (this.stack[i].name == name)
				{
					return i;
				}
			}
			return int.MaxValue;
		}

		int FindLastInTableScope([Local] string name)
		{
			for (int i = this.currentPtr; i > 0; i--)
			{
				if (this.stack[i].name == name)
				{
					return i;
				}
				if (this.stack[i].name == "table")
				{
					return int.MaxValue;
				}
			}
			return int.MaxValue;
		}

		int FindLastInButtonScope([Local] string name)
		{
			for (int i = this.currentPtr; i > 0; i--)
			{
				if (this.stack[i].name == name)
				{
					return i;
				}
				if (this.stack[i].IsScoping || this.stack[i].name == "button")
				{
					return int.MaxValue;
				}
			}
			return int.MaxValue;
		}

		int FindLastInScope([Local] string name)
		{
			for (int i = this.currentPtr; i > 0; i--)
			{
				if (this.stack[i].name == name)
				{
					return i;
				}
				if (this.stack[i].IsScoping)
				{
					return int.MaxValue;
				}
			}
			return int.MaxValue;
		}

		int FindLastInListScope([Local] string name)
		{
			for (int i = this.currentPtr; i > 0; i--)
			{
				if (this.stack[i].name == name)
				{
					return i;
				}
				if (this.stack[i].IsScoping || this.stack[i].name == "ul" || this.stack[i].name == "ol")
				{
					return int.MaxValue;
				}
			}
			return int.MaxValue;
		}

		int FindLastInScopeHn()
		{
			for (int i = this.currentPtr; i > 0; i--)
			{
				if (this.stack[i].Group == DispatchGroup.H1_OR_H2_OR_H3_OR_H4_OR_H5_OR_H6)
				{
					return i;
				}
				if (this.stack[i].IsScoping)
				{
					return int.MaxValue;
				}
			}
			return int.MaxValue;
		}

		void GenerateImpliedEndTagsExceptFor([Local] string name)
		{
			for (;;)
			{
				StackNode<T> stackNode = this.stack[this.currentPtr];
				DispatchGroup group = stackNode.Group;
				if (group <= DispatchGroup.P)
				{
					if (group != DispatchGroup.LI)
					{
						switch (group)
						{
						case DispatchGroup.OPTGROUP:
						case DispatchGroup.OPTION:
						case DispatchGroup.P:
							goto IL_40;
						}
						break;
					}
				}
				else if (group != DispatchGroup.DD_OR_DT && group != DispatchGroup.RT_OR_RP)
				{
					return;
				}
				IL_40:
				if (stackNode.name == name)
				{
					return;
				}
				this.Pop();
			}
		}

		void GenerateImpliedEndTags()
		{
			for (;;)
			{
				DispatchGroup group = this.stack[this.currentPtr].Group;
				if (group <= DispatchGroup.P)
				{
					if (group != DispatchGroup.LI)
					{
						switch (group)
						{
						case DispatchGroup.OPTGROUP:
						case DispatchGroup.OPTION:
						case DispatchGroup.P:
							goto IL_3E;
						}
						break;
					}
				}
				else if (group != DispatchGroup.DD_OR_DT && group != DispatchGroup.RT_OR_RP)
				{
					return;
				}
				IL_3E:
				this.Pop();
			}
		}

		bool IsSecondOnStackBody()
		{
			return this.currentPtr >= 1 && this.stack[1].Group == DispatchGroup.BODY;
		}

		void DocumentModeInternal(DocumentMode m, string publicIdentifier, string systemIdentifier, bool html4SpecificAdditionalErrorChecks)
		{
			this.quirks = m == DocumentMode.QuirksMode;
			if (this.DocumentModeDetected != null)
			{
				this.DocumentModeDetected(this, new DocumentModeEventArgs(m, publicIdentifier, systemIdentifier, html4SpecificAdditionalErrorChecks));
			}
			this.ReceiveDocumentMode(m, publicIdentifier, systemIdentifier, html4SpecificAdditionalErrorChecks);
		}

		bool IsAlmostStandards(string publicIdentifier, string systemIdentifier)
		{
			if (Portability.LowerCaseLiteralEqualsIgnoreAsciiCaseString("-//w3c//dtd xhtml 1.0 transitional//en", publicIdentifier))
			{
				return true;
			}
			if (Portability.LowerCaseLiteralEqualsIgnoreAsciiCaseString("-//w3c//dtd xhtml 1.0 frameset//en", publicIdentifier))
			{
				return true;
			}
			if (systemIdentifier != null)
			{
				if (Portability.LowerCaseLiteralEqualsIgnoreAsciiCaseString("-//w3c//dtd html 4.01 transitional//en", publicIdentifier))
				{
					return true;
				}
				if (Portability.LowerCaseLiteralEqualsIgnoreAsciiCaseString("-//w3c//dtd html 4.01 frameset//en", publicIdentifier))
				{
					return true;
				}
			}
			return false;
		}

		bool IsQuirky([Local] string name, string publicIdentifier, string systemIdentifier, bool forceQuirks)
		{
			if (forceQuirks)
			{
				return true;
			}
			if (name != "html")
			{
				return true;
			}
			if (publicIdentifier != null)
			{
				for (int i = 0; i < TreeBuilderConstants.QUIRKY_PUBLIC_IDS.Length; i++)
				{
					if (Portability.LowerCaseLiteralIsPrefixOfIgnoreAsciiCaseString(TreeBuilderConstants.QUIRKY_PUBLIC_IDS[i], publicIdentifier))
					{
						return true;
					}
				}
				if (Portability.LowerCaseLiteralEqualsIgnoreAsciiCaseString("-//w3o//dtd w3 html strict 3.0//en//", publicIdentifier) || Portability.LowerCaseLiteralEqualsIgnoreAsciiCaseString("-/w3c/dtd html 4.0 transitional/en", publicIdentifier) || Portability.LowerCaseLiteralEqualsIgnoreAsciiCaseString("html", publicIdentifier))
				{
					return true;
				}
			}
			if (systemIdentifier == null)
			{
				if (Portability.LowerCaseLiteralEqualsIgnoreAsciiCaseString("-//w3c//dtd html 4.01 transitional//en", publicIdentifier))
				{
					return true;
				}
				if (Portability.LowerCaseLiteralEqualsIgnoreAsciiCaseString("-//w3c//dtd html 4.01 frameset//en", publicIdentifier))
				{
					return true;
				}
			}
			else if (Portability.LowerCaseLiteralEqualsIgnoreAsciiCaseString("http://www.ibm.com/data/dtd/v11/ibmxhtml1-transitional.dtd", systemIdentifier))
			{
				return true;
			}
			return false;
		}

		void CloseTheCell(int eltPos)
		{
			this.GenerateImpliedEndTags();
			if (this.ErrorEvent != null && eltPos != this.currentPtr)
			{
				this.ErrUnclosedElementsCell(eltPos);
			}
			while (this.currentPtr >= eltPos)
			{
				this.Pop();
			}
			this.ClearTheListOfActiveFormattingElementsUpToTheLastMarker();
			this.mode = InsertionMode.IN_ROW;
		}

		int FindLastInTableScopeTdTh()
		{
			for (int i = this.currentPtr; i > 0; i--)
			{
				string name = this.stack[i].name;
				if ("td" == name || "th" == name)
				{
					return i;
				}
				if (name == "table")
				{
					return int.MaxValue;
				}
			}
			return int.MaxValue;
		}

		void ClearStackBackTo(int eltPos)
		{
			while (this.currentPtr > eltPos)
			{
				this.Pop();
			}
		}

		void ResetTheInsertionMode()
		{
			int i = this.currentPtr;
			while (i >= 0)
			{
				StackNode<T> stackNode = this.stack[i];
				string name = stackNode.name;
				string ns = stackNode.ns;
				if (i == 0)
				{
					if (this.contextNamespace == "http://www.w3.org/1999/xhtml" && (this.contextName == "td" || this.contextName == "th"))
					{
						this.mode = (this.framesetOk ? InsertionMode.FRAMESET_OK : InsertionMode.IN_BODY);
						return;
					}
					name = this.contextName;
					ns = this.contextNamespace;
				}
				if ("select" == name)
				{
					this.mode = InsertionMode.IN_SELECT;
					return;
				}
				if ("td" == name || "th" == name)
				{
					this.mode = InsertionMode.IN_CELL;
					return;
				}
				if ("tr" == name)
				{
					this.mode = InsertionMode.IN_ROW;
					return;
				}
				if ("tbody" == name || "thead" == name || "tfoot" == name)
				{
					this.mode = InsertionMode.IN_TABLE_BODY;
					return;
				}
				if ("caption" == name)
				{
					this.mode = InsertionMode.IN_CAPTION;
					return;
				}
				if ("colgroup" == name)
				{
					this.mode = InsertionMode.IN_COLUMN_GROUP;
					return;
				}
				if ("table" == name)
				{
					this.mode = InsertionMode.IN_TABLE;
					return;
				}
				if ("http://www.w3.org/1999/xhtml" != ns)
				{
					this.mode = (this.framesetOk ? InsertionMode.FRAMESET_OK : InsertionMode.IN_BODY);
					return;
				}
				if ("head" == name)
				{
					this.mode = (this.framesetOk ? InsertionMode.FRAMESET_OK : InsertionMode.IN_BODY);
					return;
				}
				if ("body" == name)
				{
					this.mode = (this.framesetOk ? InsertionMode.FRAMESET_OK : InsertionMode.IN_BODY);
					return;
				}
				if ("frameset" == name)
				{
					this.mode = InsertionMode.IN_FRAMESET;
					return;
				}
				if ("html" == name)
				{
					if (this.headPointer == null)
					{
						this.mode = InsertionMode.BEFORE_HEAD;
						return;
					}
					this.mode = InsertionMode.AFTER_HEAD;
					return;
				}
				else
				{
					if (i == 0)
					{
						this.mode = (this.framesetOk ? InsertionMode.FRAMESET_OK : InsertionMode.IN_BODY);
						return;
					}
					i--;
				}
			}
		}

		void ImplicitlyCloseP()
		{
			int num = this.FindLastInButtonScope("p");
			if (num == 2147483647)
			{
				return;
			}
			this.GenerateImpliedEndTagsExceptFor("p");
			if (this.ErrorEvent != null && num != this.currentPtr)
			{
				this.ErrUnclosedElementsImplied(num, "p");
			}
			while (this.currentPtr >= num)
			{
				this.Pop();
			}
		}

		bool ClearLastStackSlot()
		{
			this.stack[this.currentPtr] = null;
			return true;
		}

		bool ClearLastListSlot()
		{
			this.listOfActiveFormattingElements[this.listPtr] = null;
			return true;
		}

		void Push(StackNode<T> node)
		{
			this.currentPtr++;
			if (this.currentPtr == this.stack.Length)
			{
				StackNode<T>[] destinationArray = new StackNode<T>[this.stack.Length + 64];
				Array.Copy(this.stack, destinationArray, this.stack.Length);
				this.stack = destinationArray;
			}
			this.stack[this.currentPtr] = node;
			this.ElementPushed(node.ns, node.popName, node.node);
		}

		void SilentPush(StackNode<T> node)
		{
			this.currentPtr++;
			if (this.currentPtr == this.stack.Length)
			{
				StackNode<T>[] destinationArray = new StackNode<T>[this.stack.Length + 64];
				Array.Copy(this.stack, destinationArray, this.stack.Length);
				this.stack = destinationArray;
			}
			this.stack[this.currentPtr] = node;
		}

		void Append(StackNode<T> node)
		{
			this.listPtr++;
			if (this.listPtr == this.listOfActiveFormattingElements.Length)
			{
				StackNode<T>[] destinationArray = new StackNode<T>[this.listOfActiveFormattingElements.Length + 64];
				Array.Copy(this.listOfActiveFormattingElements, destinationArray, this.listOfActiveFormattingElements.Length);
				this.listOfActiveFormattingElements = destinationArray;
			}
			this.listOfActiveFormattingElements[this.listPtr] = node;
		}

		void InsertMarker()
		{
			this.Append(null);
		}

		void ClearTheListOfActiveFormattingElementsUpToTheLastMarker()
		{
			while (this.listPtr > -1)
			{
				if (this.listOfActiveFormattingElements[this.listPtr] == null)
				{
					this.listPtr--;
					return;
				}
				this.listOfActiveFormattingElements[this.listPtr].Release();
				this.listPtr--;
			}
		}

		bool IsCurrent([Local] string name)
		{
			return name == this.stack[this.currentPtr].name;
		}

		void RemoveFromStack(int pos)
		{
			if (this.currentPtr == pos)
			{
				this.Pop();
				return;
			}
			this.Fatal();
			this.stack[pos].Release();
			Array.Copy(this.stack, pos + 1, this.stack, pos, this.currentPtr - pos);
			this.currentPtr--;
		}

		void RemoveFromStack(StackNode<T> node)
		{
			if (this.stack[this.currentPtr] == node)
			{
				this.Pop();
				return;
			}
			int num = this.currentPtr - 1;
			while (num >= 0 && this.stack[num] != node)
			{
				num--;
			}
			if (num == -1)
			{
				return;
			}
			this.Fatal();
			node.Release();
			Array.Copy(this.stack, num + 1, this.stack, num, this.currentPtr - num);
			this.currentPtr--;
		}

		void RemoveFromListOfActiveFormattingElements(int pos)
		{
			this.listOfActiveFormattingElements[pos].Release();
			if (pos == this.listPtr)
			{
				this.listPtr--;
				return;
			}
			Array.Copy(this.listOfActiveFormattingElements, pos + 1, this.listOfActiveFormattingElements, pos, this.listPtr - pos);
			this.listPtr--;
		}

		bool AdoptionAgencyEndTag([Local] string name)
		{
			for (int i = 0; i < 8; i++)
			{
				int j;
				for (j = this.listPtr; j > -1; j--)
				{
					StackNode<T> stackNode = this.listOfActiveFormattingElements[j];
					if (stackNode == null)
					{
						j = -1;
						break;
					}
					if (stackNode.name == name)
					{
						break;
					}
				}
				if (j == -1)
				{
					return false;
				}
				StackNode<T> stackNode2 = this.listOfActiveFormattingElements[j];
				int k = this.currentPtr;
				bool flag = true;
				while (k > -1)
				{
					StackNode<T> stackNode3 = this.stack[k];
					if (stackNode3 == stackNode2)
					{
						break;
					}
					if (stackNode3.IsScoping)
					{
						flag = false;
					}
					k--;
				}
				if (k == -1)
				{
					this.Err("No element “" + name + "” to close.");
					this.RemoveFromListOfActiveFormattingElements(j);
					return true;
				}
				if (!flag)
				{
					this.Err("No element “" + name + "” to close.");
					return true;
				}
				if (this.ErrorEvent != null && k != this.currentPtr)
				{
					this.Err("End tag “" + name + "” violates nesting rules.");
				}
				int l;
				for (l = k + 1; l <= this.currentPtr; l++)
				{
					StackNode<T> stackNode4 = this.stack[l];
					if (stackNode4.IsSpecial)
					{
						break;
					}
				}
				if (l > this.currentPtr)
				{
					while (this.currentPtr >= k)
					{
						this.Pop();
					}
					this.RemoveFromListOfActiveFormattingElements(j);
					return true;
				}
				StackNode<T> stackNode5 = this.stack[k - 1];
				StackNode<T> stackNode6 = this.stack[l];
				int bookmark = j;
				int num = l;
				StackNode<T> stackNode7 = stackNode6;
				for (int m = 0; m < 3; m++)
				{
					num--;
					StackNode<T> stackNode8 = this.stack[num];
					int num2 = this.FindInListOfActiveFormattingElements(stackNode8);
					if (num2 == -1)
					{
						this.RemoveFromStack(num);
						l--;
					}
					else
					{
						if (num == k)
						{
							break;
						}
						if (num == l)
						{
							bookmark = num2 + 1;
						}
						T node = this.CreateElement("http://www.w3.org/1999/xhtml", stackNode8.name, stackNode8.attributes.CloneAttributes());
						StackNode<T> stackNode9 = new StackNode<T>(stackNode8.Flags, stackNode8.ns, stackNode8.name, node, stackNode8.popName, stackNode8.attributes, stackNode8.Locator);
						stackNode8.DropAttributes();
						this.stack[num] = stackNode9;
						stackNode9.Retain();
						this.listOfActiveFormattingElements[num2] = stackNode9;
						stackNode8.Release();
						stackNode8.Release();
						stackNode8 = stackNode9;
						this.DetachFromParent(stackNode7.node);
						this.AppendElement(stackNode7.node, stackNode8.node);
						stackNode7 = stackNode8;
					}
				}
				if (stackNode5.IsFosterParenting)
				{
					this.Fatal();
					this.DetachFromParent(stackNode7.node);
					this.InsertIntoFosterParent(stackNode7.node);
				}
				else
				{
					this.DetachFromParent(stackNode7.node);
					this.AppendElement(stackNode7.node, stackNode5.node);
				}
				T t = this.CreateElement("http://www.w3.org/1999/xhtml", stackNode2.name, stackNode2.attributes.CloneAttributes());
				StackNode<T> stackNode10 = new StackNode<T>(stackNode2.Flags, stackNode2.ns, stackNode2.name, t, stackNode2.popName, stackNode2.attributes, (this.ErrorEvent == null) ? null : new TaintableLocator(this.tokenizer));
				stackNode2.DropAttributes();
				this.AppendChildrenToNewParent(stackNode6.node, t);
				this.AppendElement(t, stackNode6.node);
				this.RemoveFromListOfActiveFormattingElements(j);
				this.InsertIntoListOfActiveFormattingElements(stackNode10, bookmark);
				this.RemoveFromStack(k);
				this.InsertIntoStack(stackNode10, l);
			}
			return true;
		}

		void InsertIntoStack(StackNode<T> node, int position)
		{
			if (position == this.currentPtr + 1)
			{
				this.Push(node);
				return;
			}
			Array.Copy(this.stack, position, this.stack, position + 1, this.currentPtr - position + 1);
			this.currentPtr++;
			this.stack[position] = node;
		}

		void InsertIntoListOfActiveFormattingElements(StackNode<T> formattingClone, int bookmark)
		{
			formattingClone.Retain();
			if (bookmark <= this.listPtr)
			{
				Array.Copy(this.listOfActiveFormattingElements, bookmark, this.listOfActiveFormattingElements, bookmark + 1, this.listPtr - bookmark + 1);
			}
			this.listPtr++;
			this.listOfActiveFormattingElements[bookmark] = formattingClone;
		}

		int FindInListOfActiveFormattingElements(StackNode<T> node)
		{
			for (int i = this.listPtr; i >= 0; i--)
			{
				if (node == this.listOfActiveFormattingElements[i])
				{
					return i;
				}
			}
			return -1;
		}

		int FindInListOfActiveFormattingElementsContainsBetweenEndAndLastMarker([Local] string name)
		{
			for (int i = this.listPtr; i >= 0; i--)
			{
				StackNode<T> stackNode = this.listOfActiveFormattingElements[i];
				if (stackNode == null)
				{
					return -1;
				}
				if (stackNode.name == name)
				{
					return i;
				}
			}
			return -1;
		}

		void MaybeForgetEarlierDuplicateFormattingElement([Local] string name, HtmlAttributes attributes)
		{
			int pos = -1;
			int num = 0;
			for (int i = this.listPtr; i >= 0; i--)
			{
				StackNode<T> stackNode = this.listOfActiveFormattingElements[i];
				if (stackNode == null)
				{
					break;
				}
				if (stackNode.name == name && stackNode.attributes.Equals(attributes))
				{
					pos = i;
					num++;
				}
			}
			if (num >= 3)
			{
				this.RemoveFromListOfActiveFormattingElements(pos);
			}
		}

		int FindLastOrRoot([Local] string name)
		{
			for (int i = this.currentPtr; i > 0; i--)
			{
				if (this.stack[i].name == name)
				{
					return i;
				}
			}
			return 0;
		}

		int FindLastOrRoot(DispatchGroup group)
		{
			for (int i = this.currentPtr; i > 0; i--)
			{
				if (this.stack[i].Group == group)
				{
					return i;
				}
			}
			return 0;
		}

		bool AddAttributesToBody(HtmlAttributes attributes)
		{
			this.CheckAttributes(attributes, "http://www.w3.org/1999/xhtml");
			if (this.currentPtr >= 1)
			{
				StackNode<T> stackNode = this.stack[1];
				if (stackNode.Group == DispatchGroup.BODY)
				{
					this.AddAttributesToElement(stackNode.node, attributes);
					return true;
				}
			}
			return false;
		}

		void AddAttributesToHtml(HtmlAttributes attributes)
		{
			this.CheckAttributes(attributes, "http://www.w3.org/1999/xhtml");
			this.AddAttributesToElement(this.stack[0].node, attributes);
		}

		void PushHeadPointerOntoStack()
		{
			this.Fatal();
			this.SilentPush(new StackNode<T>(ElementName.HEAD, this.headPointer, (this.ErrorEvent == null) ? null : new TaintableLocator(this.tokenizer)));
		}

		void ReconstructTheActiveFormattingElements()
		{
			if (this.listPtr == -1)
			{
				return;
			}
			StackNode<T> stackNode = this.listOfActiveFormattingElements[this.listPtr];
			if (stackNode == null || this.IsInStack(stackNode))
			{
				return;
			}
			int i = this.listPtr;
			do
			{
				i--;
				if (i == -1 || this.listOfActiveFormattingElements[i] == null)
				{
					break;
				}
			}
			while (!this.IsInStack(this.listOfActiveFormattingElements[i]));
			IL_103:
			while (i < this.listPtr)
			{
				i++;
				StackNode<T> stackNode2 = this.listOfActiveFormattingElements[i];
				T t = this.CreateElement("http://www.w3.org/1999/xhtml", stackNode2.name, stackNode2.attributes.CloneAttributes());
				StackNode<T> stackNode3 = new StackNode<T>(stackNode2.Flags, stackNode2.ns, stackNode2.name, t, stackNode2.popName, stackNode2.attributes, stackNode2.Locator);
				stackNode2.DropAttributes();
				StackNode<T> stackNode4 = this.stack[this.currentPtr];
				if (stackNode4.IsFosterParenting)
				{
					this.InsertIntoFosterParent(t);
				}
				else
				{
					this.AppendElement(t, stackNode4.node);
				}
				this.Push(stackNode3);
				this.listOfActiveFormattingElements[i] = stackNode3;
				stackNode2.Release();
				stackNode3.Retain();
			}
			return;
			goto IL_103;
		}

		void InsertIntoFosterParent(T child)
		{
			int num = this.FindLastOrRoot(DispatchGroup.TABLE);
			StackNode<T> stackNode = this.stack[num];
			T node = stackNode.node;
			if (num == 0)
			{
				this.AppendElement(child, node);
				return;
			}
			this.InsertFosterParentedChild(child, node, this.stack[num - 1].node);
		}

		bool IsInStack(StackNode<T> node)
		{
			for (int i = this.currentPtr; i >= 0; i--)
			{
				if (this.stack[i] == node)
				{
					return true;
				}
			}
			return false;
		}

		void Pop()
		{
			StackNode<T> stackNode = this.stack[this.currentPtr];
			this.currentPtr--;
			this.ElementPopped(stackNode.ns, stackNode.popName, stackNode.node);
			stackNode.Release();
		}

		void SilentPop()
		{
			StackNode<T> stackNode = this.stack[this.currentPtr];
			this.currentPtr--;
			stackNode.Release();
		}

		void PopOnEof()
		{
			StackNode<T> stackNode = this.stack[this.currentPtr];
			this.currentPtr--;
			this.MarkMalformedIfScript(stackNode.node);
			this.ElementPopped(stackNode.ns, stackNode.popName, stackNode.node);
			stackNode.Release();
		}

		void CheckAttributes(HtmlAttributes attributes, [NsUri] string ns)
		{
			if (this.ErrorEvent != null)
			{
				int xmlnsLength = attributes.XmlnsLength;
				for (int i = 0; i < xmlnsLength; i++)
				{
					AttributeName xmlnsAttributeName = attributes.GetXmlnsAttributeName(i);
					if (xmlnsAttributeName == AttributeName.XMLNS)
					{
						if (this.html4)
						{
							this.Err("Attribute “xmlns” not allowed here. (HTML4-only error.)");
						}
						else
						{
							string xmlnsValue = attributes.GetXmlnsValue(i);
							if (ns != xmlnsValue)
							{
								this.Err(string.Concat(new string[] { "Bad value “", xmlnsValue, "” for the attribute “xmlns” (only “", ns, "” permitted here)." }));
								switch (this.NamePolicy)
								{
								case XmlViolationPolicy.Allow:
								case XmlViolationPolicy.AlterInfoset:
									this.Warn("Attribute “xmlns” is not serializable as XML 1.0.");
									break;
								case XmlViolationPolicy.Fatal:
									this.Fatal("Attribute “xmlns” is not serializable as XML 1.0.");
									break;
								}
							}
						}
					}
					else if (ns != "http://www.w3.org/1999/xhtml" && xmlnsAttributeName == AttributeName.XMLNS_XLINK)
					{
						string xmlnsValue2 = attributes.GetXmlnsValue(i);
						if ("http://www.w3.org/1999/xlink" != xmlnsValue2)
						{
							this.Err("Bad value “" + xmlnsValue2 + "” for the attribute “xmlns:link” (only “http://www.w3.org/1999/xlink” permitted here).");
							switch (this.NamePolicy)
							{
							case XmlViolationPolicy.Allow:
							case XmlViolationPolicy.AlterInfoset:
								this.Warn("Attribute “xmlns:xlink” with a value other than “http://www.w3.org/1999/xlink” is not serializable as XML 1.0 without changing document semantics.");
								break;
							case XmlViolationPolicy.Fatal:
								this.Fatal("Attribute “xmlns:xlink” with a value other than “http://www.w3.org/1999/xlink” is not serializable as XML 1.0 without changing document semantics.");
								break;
							}
						}
					}
					else
					{
						this.Err("Attribute “" + attributes.GetXmlnsLocalName(i) + "” not allowed here.");
						switch (this.NamePolicy)
						{
						case XmlViolationPolicy.Allow:
						case XmlViolationPolicy.AlterInfoset:
							this.Warn("Attribute with the local name “" + attributes.GetXmlnsLocalName(i) + "” is not serializable as XML 1.0.");
							break;
						case XmlViolationPolicy.Fatal:
							this.Fatal("Attribute with the local name “" + attributes.GetXmlnsLocalName(i) + "” is not serializable as XML 1.0.");
							break;
						}
					}
				}
			}
			attributes.ProcessNonNcNames<T>(this, this.NamePolicy);
		}

		string CheckPopName([Local] string name)
		{
			if (NCName.IsNCName(name))
			{
				return name;
			}
			switch (this.NamePolicy)
			{
			case XmlViolationPolicy.Allow:
				this.Warn("Element name “" + name + "” cannot be represented as XML 1.0.");
				return name;
			case XmlViolationPolicy.Fatal:
				this.Fatal("Element name “" + name + "” cannot be represented as XML 1.0.");
				break;
			case XmlViolationPolicy.AlterInfoset:
				this.Warn("Element name “" + name + "” cannot be represented as XML 1.0.");
				return NCName.EscapeName(name);
			}
			return null;
		}

		void AppendHtmlElementToDocumentAndPush(HtmlAttributes attributes)
		{
			this.CheckAttributes(attributes, "http://www.w3.org/1999/xhtml");
			T node = this.CreateHtmlElementSetAsRoot(attributes);
			StackNode<T> node2 = new StackNode<T>(ElementName.HTML, node, (this.ErrorEvent == null) ? null : new TaintableLocator(this.tokenizer));
			this.Push(node2);
		}

		void AppendHtmlElementToDocumentAndPush()
		{
			this.AppendHtmlElementToDocumentAndPush(this.tokenizer.EmptyAttributes());
		}

		void AppendToCurrentNodeAndPushHeadElement(HtmlAttributes attributes)
		{
			this.CheckAttributes(attributes, "http://www.w3.org/1999/xhtml");
			T t = this.CreateElement("http://www.w3.org/1999/xhtml", "head", attributes);
			this.AppendElement(t, this.stack[this.currentPtr].node);
			this.headPointer = t;
			StackNode<T> node = new StackNode<T>(ElementName.HEAD, t, (this.ErrorEvent == null) ? null : new TaintableLocator(this.tokenizer));
			this.Push(node);
		}

		void AppendToCurrentNodeAndPushBodyElement(HtmlAttributes attributes)
		{
			this.AppendToCurrentNodeAndPushElement(ElementName.BODY, attributes);
		}

		void AppendToCurrentNodeAndPushBodyElement()
		{
			this.AppendToCurrentNodeAndPushBodyElement(this.tokenizer.EmptyAttributes());
		}

		void AppendToCurrentNodeAndPushFormElementMayFoster(HtmlAttributes attributes)
		{
			this.CheckAttributes(attributes, "http://www.w3.org/1999/xhtml");
			T t = this.CreateElement("http://www.w3.org/1999/xhtml", "form", attributes);
			this.formPointer = t;
			StackNode<T> stackNode = this.stack[this.currentPtr];
			if (stackNode.IsFosterParenting)
			{
				this.Fatal();
				this.InsertIntoFosterParent(t);
			}
			else
			{
				this.AppendElement(t, stackNode.node);
			}
			StackNode<T> node = new StackNode<T>(ElementName.FORM, t, (this.ErrorEvent == null) ? null : new TaintableLocator(this.tokenizer));
			this.Push(node);
		}

		void AppendToCurrentNodeAndPushFormattingElementMayFoster(ElementName elementName, HtmlAttributes attributes)
		{
			this.CheckAttributes(attributes, "http://www.w3.org/1999/xhtml");
			T t = this.CreateElement("http://www.w3.org/1999/xhtml", elementName.name, attributes);
			StackNode<T> stackNode = this.stack[this.currentPtr];
			if (stackNode.IsFosterParenting)
			{
				this.Fatal();
				this.InsertIntoFosterParent(t);
			}
			else
			{
				this.AppendElement(t, stackNode.node);
			}
			StackNode<T> stackNode2 = new StackNode<T>(elementName, t, attributes.CloneAttributes(), (this.ErrorEvent == null) ? null : new TaintableLocator(this.tokenizer));
			this.Push(stackNode2);
			this.Append(stackNode2);
			stackNode2.Retain();
		}

		void AppendToCurrentNodeAndPushElement(ElementName elementName, HtmlAttributes attributes)
		{
			this.CheckAttributes(attributes, "http://www.w3.org/1999/xhtml");
			T t = this.CreateElement("http://www.w3.org/1999/xhtml", elementName.name, attributes);
			this.AppendElement(t, this.stack[this.currentPtr].node);
			StackNode<T> node = new StackNode<T>(elementName, t, (this.ErrorEvent == null) ? null : new TaintableLocator(this.tokenizer));
			this.Push(node);
		}

		void AppendToCurrentNodeAndPushElementMayFoster(ElementName elementName, HtmlAttributes attributes)
		{
			string text = elementName.name;
			this.CheckAttributes(attributes, "http://www.w3.org/1999/xhtml");
			if (elementName.IsCustom)
			{
				text = this.CheckPopName(text);
			}
			T t = this.CreateElement("http://www.w3.org/1999/xhtml", text, attributes);
			StackNode<T> stackNode = this.stack[this.currentPtr];
			if (stackNode.IsFosterParenting)
			{
				this.Fatal();
				this.InsertIntoFosterParent(t);
			}
			else
			{
				this.AppendElement(t, stackNode.node);
			}
			StackNode<T> node = new StackNode<T>(elementName, t, text, (this.ErrorEvent == null) ? null : new TaintableLocator(this.tokenizer));
			this.Push(node);
		}

		void AppendToCurrentNodeAndPushElementMayFosterMathML(ElementName elementName, HtmlAttributes attributes)
		{
			string text = elementName.name;
			this.CheckAttributes(attributes, "http://www.w3.org/1998/Math/MathML");
			if (elementName.IsCustom)
			{
				text = this.CheckPopName(text);
			}
			T t = this.CreateElement("http://www.w3.org/1998/Math/MathML", text, attributes);
			StackNode<T> stackNode = this.stack[this.currentPtr];
			if (stackNode.IsFosterParenting)
			{
				this.Fatal();
				this.InsertIntoFosterParent(t);
			}
			else
			{
				this.AppendElement(t, stackNode.node);
			}
			bool markAsIntegrationPoint = false;
			if (ElementName.ANNOTATION_XML == elementName && this.AnnotationXmlEncodingPermitsHtml(attributes))
			{
				markAsIntegrationPoint = true;
			}
			StackNode<T> node = new StackNode<T>(elementName, t, text, markAsIntegrationPoint, (this.ErrorEvent == null) ? null : new TaintableLocator(this.tokenizer));
			this.Push(node);
		}

		bool AnnotationXmlEncodingPermitsHtml(HtmlAttributes attributes)
		{
			string value = attributes.GetValue(AttributeName.ENCODING);
			return value != null && (Portability.LowerCaseLiteralEqualsIgnoreAsciiCaseString("application/xhtml+xml", value) || Portability.LowerCaseLiteralEqualsIgnoreAsciiCaseString("text/html", value));
		}

		void AppendToCurrentNodeAndPushElementMayFosterSVG(ElementName elementName, HtmlAttributes attributes)
		{
			string text = elementName.camelCaseName;
			this.CheckAttributes(attributes, "http://www.w3.org/2000/svg");
			if (elementName.IsCustom)
			{
				text = this.CheckPopName(text);
			}
			T t = this.CreateElement("http://www.w3.org/2000/svg", text, attributes);
			StackNode<T> stackNode = this.stack[this.currentPtr];
			if (stackNode.IsFosterParenting)
			{
				this.Fatal();
				this.InsertIntoFosterParent(t);
			}
			else
			{
				this.AppendElement(t, stackNode.node);
			}
			StackNode<T> node = new StackNode<T>(elementName, text, t, (this.ErrorEvent == null) ? null : new TaintableLocator(this.tokenizer));
			this.Push(node);
		}

		void AppendToCurrentNodeAndPushElementMayFoster(ElementName elementName, HtmlAttributes attributes, T form)
		{
			this.CheckAttributes(attributes, "http://www.w3.org/1999/xhtml");
			T t = this.CreateElement("http://www.w3.org/1999/xhtml", elementName.name, attributes, this.fragment ? default(T) : form);
			StackNode<T> stackNode = this.stack[this.currentPtr];
			if (stackNode.IsFosterParenting)
			{
				this.Fatal();
				this.InsertIntoFosterParent(t);
			}
			else
			{
				this.AppendElement(t, stackNode.node);
			}
			StackNode<T> node = new StackNode<T>(elementName, t, (this.ErrorEvent == null) ? null : new TaintableLocator(this.tokenizer));
			this.Push(node);
		}

		void AppendVoidElementToCurrentMayFoster([Local] string name, HtmlAttributes attributes, T form)
		{
			this.CheckAttributes(attributes, "http://www.w3.org/1999/xhtml");
			T t = this.CreateElement("http://www.w3.org/1999/xhtml", name, attributes, this.fragment ? default(T) : form);
			StackNode<T> stackNode = this.stack[this.currentPtr];
			if (stackNode.IsFosterParenting)
			{
				this.Fatal();
				this.InsertIntoFosterParent(t);
			}
			else
			{
				this.AppendElement(t, stackNode.node);
			}
			this.ElementPushed("http://www.w3.org/1999/xhtml", name, t);
			this.ElementPopped("http://www.w3.org/1999/xhtml", name, t);
		}

		void AppendVoidElementToCurrentMayFoster(ElementName elementName, HtmlAttributes attributes)
		{
			string name = elementName.name;
			this.CheckAttributes(attributes, "http://www.w3.org/1999/xhtml");
			if (elementName.IsCustom)
			{
				name = this.CheckPopName(name);
			}
			T t = this.CreateElement("http://www.w3.org/1999/xhtml", name, attributes);
			StackNode<T> stackNode = this.stack[this.currentPtr];
			if (stackNode.IsFosterParenting)
			{
				this.Fatal();
				this.InsertIntoFosterParent(t);
			}
			else
			{
				this.AppendElement(t, stackNode.node);
			}
			this.ElementPushed("http://www.w3.org/1999/xhtml", name, t);
			this.ElementPopped("http://www.w3.org/1999/xhtml", name, t);
		}

		void AppendVoidElementToCurrentMayFosterSVG(ElementName elementName, HtmlAttributes attributes)
		{
			string name = elementName.camelCaseName;
			this.CheckAttributes(attributes, "http://www.w3.org/2000/svg");
			if (elementName.IsCustom)
			{
				name = this.CheckPopName(name);
			}
			T t = this.CreateElement("http://www.w3.org/2000/svg", name, attributes);
			StackNode<T> stackNode = this.stack[this.currentPtr];
			if (stackNode.IsFosterParenting)
			{
				this.Fatal();
				this.InsertIntoFosterParent(t);
			}
			else
			{
				this.AppendElement(t, stackNode.node);
			}
			this.ElementPushed("http://www.w3.org/2000/svg", name, t);
			this.ElementPopped("http://www.w3.org/2000/svg", name, t);
		}

		void AppendVoidElementToCurrentMayFosterMathML(ElementName elementName, HtmlAttributes attributes)
		{
			string name = elementName.name;
			this.CheckAttributes(attributes, "http://www.w3.org/1998/Math/MathML");
			if (elementName.IsCustom)
			{
				name = this.CheckPopName(name);
			}
			T t = this.CreateElement("http://www.w3.org/1998/Math/MathML", name, attributes);
			StackNode<T> stackNode = this.stack[this.currentPtr];
			if (stackNode.IsFosterParenting)
			{
				this.Fatal();
				this.InsertIntoFosterParent(t);
			}
			else
			{
				this.AppendElement(t, stackNode.node);
			}
			this.ElementPushed("http://www.w3.org/1998/Math/MathML", name, t);
			this.ElementPopped("http://www.w3.org/1998/Math/MathML", name, t);
		}

		void AppendVoidElementToCurrent([Local] string name, HtmlAttributes attributes, T form)
		{
			this.CheckAttributes(attributes, "http://www.w3.org/1999/xhtml");
			T t = this.CreateElement("http://www.w3.org/1999/xhtml", name, attributes, this.fragment ? default(T) : form);
			StackNode<T> stackNode = this.stack[this.currentPtr];
			this.AppendElement(t, stackNode.node);
			this.ElementPushed("http://www.w3.org/1999/xhtml", name, t);
			this.ElementPopped("http://www.w3.org/1999/xhtml", name, t);
		}

		void AppendVoidFormToCurrent(HtmlAttributes attributes)
		{
			this.CheckAttributes(attributes, "http://www.w3.org/1999/xhtml");
			T t = this.CreateElement("http://www.w3.org/1999/xhtml", "form", attributes);
			this.formPointer = t;
			StackNode<T> stackNode = this.stack[this.currentPtr];
			this.AppendElement(t, stackNode.node);
			this.ElementPushed("http://www.w3.org/1999/xhtml", "form", t);
			this.ElementPopped("http://www.w3.org/1999/xhtml", "form", t);
		}

		void AccumulateCharactersForced(char[] buf, int start, int length)
		{
			this.charBuffer.Append(buf, start, length);
		}

		protected virtual void AccumulateCharacters(char[] buf, int start, int length)
		{
			this.AppendCharacters(this.stack[this.currentPtr].node, buf, start, length);
		}

		protected void RequestSuspension()
		{
			this.tokenizer.RequestSuspension();
		}

		protected abstract T CreateElement([NsUri] string ns, [Local] string name, HtmlAttributes attributes);

		protected virtual T CreateElement([NsUri] string ns, [Local] string name, HtmlAttributes attributes, T form)
		{
			return this.CreateElement("http://www.w3.org/1999/xhtml", name, attributes);
		}

		protected abstract T CreateHtmlElementSetAsRoot(HtmlAttributes attributes);

		protected abstract void DetachFromParent(T element);

		protected abstract bool HasChildren(T element);

		protected abstract void AppendElement(T child, T newParent);

		protected abstract void AppendChildrenToNewParent(T oldParent, T newParent);

		protected abstract void InsertFosterParentedChild(T child, T table, T stackParent);

		protected abstract void InsertFosterParentedCharacters(StringBuilder sb, T table, T stackParent);

		protected abstract void AppendCharacters(T parent, char[] buf, int start, int length);

		protected abstract void AppendCharacters(T parent, StringBuilder sb);

		protected abstract void AppendIsindexPrompt(T parent);

		protected abstract void AppendComment(T parent, char[] buf, int start, int length);

		protected abstract void AppendCommentToDocument(char[] buf, int start, int length);

		protected abstract void AddAttributesToElement(T element, HtmlAttributes attributes);

		protected void MarkMalformedIfScript(T elt)
		{
		}

		protected virtual void Start(bool fragmentMode)
		{
		}

		protected virtual void End()
		{
		}

		protected virtual void AppendDoctypeToDocument([Local] string name, string publicIdentifier, string systemIdentifier)
		{
		}

		protected virtual void ElementPushed([NsUri] string ns, [Local] string name, T node)
		{
		}

		protected virtual void ElementPopped([NsUri] string ns, [Local] string name, T node)
		{
		}

		protected virtual void ReceiveDocumentMode(DocumentMode m, string publicIdentifier, string systemIdentifier, bool html4SpecificAdditionalErrorChecks)
		{
		}

		public bool WantsComments { get; set; }

		public bool AllowSelfClosingTags { get; set; }

		public void SetFragmentContext([Local] string context)
		{
			this.contextName = context;
			this.contextNamespace = "http://www.w3.org/1999/xhtml";
			this.contextNode = default(T);
			this.fragment = this.contextName != null;
			this.quirks = false;
		}

		public bool IsCDataSectionAllowed
		{
			get
			{
				return this.IsInForeign;
			}
		}

		bool IsInForeign
		{
			get
			{
				return this.currentPtr >= 0 && this.stack[this.currentPtr].ns != "http://www.w3.org/1999/xhtml";
			}
		}

		bool IsInForeignButNotHtmlOrMathTextIntegrationPoint
		{
			get
			{
				return this.currentPtr >= 0 && !this.IsSpecialParentInForeign(this.stack[this.currentPtr]);
			}
		}

		public void SetFragmentContext([Local] string context, [NsUri] string ns, T node, bool quirks)
		{
			this.contextName = context;
			this.contextNamespace = ns;
			this.contextNode = node;
			this.fragment = this.contextName != null;
			this.quirks = quirks;
		}

		protected T CurrentNode()
		{
			return this.stack[this.currentPtr].node;
		}

		public void FlushCharacters()
		{
			if (this.charBufferLen > 0)
			{
				if ((this.mode == InsertionMode.IN_TABLE || this.mode == InsertionMode.IN_TABLE_BODY || this.mode == InsertionMode.IN_ROW) && this.CharBufferContainsNonWhitespace())
				{
					this.Err("Misplaced non-space characters insided a table.");
					this.ReconstructTheActiveFormattingElements();
					if (!this.stack[this.currentPtr].IsFosterParenting)
					{
						this.AppendCharacters(this.CurrentNode(), this.charBuffer);
						this.charBuffer.Clear();
						return;
					}
					int num = this.FindLastOrRoot(DispatchGroup.TABLE);
					StackNode<T> stackNode = this.stack[num];
					T node = stackNode.node;
					if (num == 0)
					{
						this.AppendCharacters(node, this.charBuffer);
						this.charBuffer.Clear();
						return;
					}
					this.InsertFosterParentedCharacters(this.charBuffer, node, this.stack[num - 1].node);
					this.charBuffer.Clear();
					return;
				}
				else
				{
					this.AppendCharacters(this.CurrentNode(), this.charBuffer);
					this.charBuffer.Clear();
				}
			}
		}

		bool CharBufferContainsNonWhitespace()
		{
			for (int i = 0; i < this.charBufferLen; i++)
			{
				char c = this.charBuffer[i];
				switch (c)
				{
				case '\t':
				case '\n':
				case '\f':
				case '\r':
					break;
				case '\v':
					return true;
				default:
					if (c != ' ')
					{
						return true;
					}
					break;
				}
			}
			return false;
		}

		public ITreeBuilderState<T> NewSnapshot()
		{
			StackNode<T>[] array = new StackNode<T>[this.listPtr + 1];
			for (int i = 0; i < array.Length; i++)
			{
				StackNode<T> stackNode = this.listOfActiveFormattingElements[i];
				if (stackNode != null)
				{
					StackNode<T> stackNode2 = new StackNode<T>(stackNode.Flags, stackNode.ns, stackNode.name, stackNode.node, stackNode.popName, stackNode.attributes.CloneAttributes(), stackNode.Locator);
					array[i] = stackNode2;
				}
				else
				{
					array[i] = null;
				}
			}
			StackNode<T>[] array2 = new StackNode<T>[this.currentPtr + 1];
			for (int j = 0; j < array2.Length; j++)
			{
				StackNode<T> stackNode3 = this.stack[j];
				int num = this.FindInListOfActiveFormattingElements(stackNode3);
				if (num == -1)
				{
					StackNode<T> stackNode4 = new StackNode<T>(stackNode3.Flags, stackNode3.ns, stackNode3.name, stackNode3.node, stackNode3.popName, null, stackNode3.Locator);
					array2[j] = stackNode4;
				}
				else
				{
					array2[j] = array[num];
					array2[j].Retain();
				}
			}
			return new StateSnapshot<T>(array2, array, this.formPointer, this.headPointer, this.deepTreeSurrogateParent, this.mode, this.originalMode, this.framesetOk, this.needToDropLF, this.quirks);
		}

		public bool SnapshotMatches(ITreeBuilderState<T> snapshot)
		{
			StackNode<T>[] array = snapshot.Stack;
			int num = snapshot.Stack.Length;
			StackNode<T>[] array2 = snapshot.ListOfActiveFormattingElements;
			int num2 = snapshot.ListOfActiveFormattingElements.Length;
			if (num != this.currentPtr + 1 || num2 != this.listPtr + 1 || this.formPointer != snapshot.FormPointer || this.headPointer != snapshot.HeadPointer || this.deepTreeSurrogateParent != snapshot.DeepTreeSurrogateParent || this.mode != snapshot.Mode || this.originalMode != snapshot.OriginalMode || this.framesetOk != snapshot.IsFramesetOk || this.needToDropLF != snapshot.IsNeedToDropLF || this.quirks != snapshot.IsQuirks)
			{
				return false;
			}
			for (int i = num2 - 1; i >= 0; i--)
			{
				if (array2[i] != null || this.listOfActiveFormattingElements[i] != null)
				{
					if (array2[i] == null || this.listOfActiveFormattingElements[i] == null)
					{
						return false;
					}
					if (array2[i].node != this.listOfActiveFormattingElements[i].node)
					{
						return false;
					}
				}
			}
			for (int j = num - 1; j >= 0; j--)
			{
				if (array[j].node != this.stack[j].node)
				{
					return false;
				}
			}
			return true;
		}

		public void LoadState(ITreeBuilderState<T> snapshot)
		{
			StackNode<T>[] array = snapshot.Stack;
			int num = snapshot.Stack.Length;
			StackNode<T>[] array2 = snapshot.ListOfActiveFormattingElements;
			int num2 = snapshot.ListOfActiveFormattingElements.Length;
			for (int i = 0; i <= this.listPtr; i++)
			{
				if (this.listOfActiveFormattingElements[i] != null)
				{
					this.listOfActiveFormattingElements[i].Release();
				}
			}
			if (this.listOfActiveFormattingElements.Length < num2)
			{
				this.listOfActiveFormattingElements = new StackNode<T>[num2];
			}
			this.listPtr = num2 - 1;
			for (int j = 0; j <= this.currentPtr; j++)
			{
				this.stack[j].Release();
			}
			if (this.stack.Length < num)
			{
				this.stack = new StackNode<T>[num];
			}
			this.currentPtr = num - 1;
			for (int k = 0; k < num2; k++)
			{
				StackNode<T> stackNode = array2[k];
				if (stackNode != null)
				{
					StackNode<T> stackNode2 = new StackNode<T>(stackNode.Flags, stackNode.ns, stackNode.name, stackNode.node, stackNode.popName, stackNode.attributes.CloneAttributes(), stackNode.Locator);
					this.listOfActiveFormattingElements[k] = stackNode2;
				}
				else
				{
					this.listOfActiveFormattingElements[k] = null;
				}
			}
			for (int l = 0; l < num; l++)
			{
				StackNode<T> stackNode3 = array[l];
				int num3 = this.FindInArray(stackNode3, array2);
				if (num3 == -1)
				{
					StackNode<T> stackNode4 = new StackNode<T>(stackNode3.Flags, stackNode3.ns, stackNode3.name, stackNode3.node, stackNode3.popName, null, stackNode3.Locator);
					this.stack[l] = stackNode4;
				}
				else
				{
					this.stack[l] = this.listOfActiveFormattingElements[num3];
					this.stack[l].Retain();
				}
			}
			this.formPointer = snapshot.FormPointer;
			this.headPointer = snapshot.HeadPointer;
			this.deepTreeSurrogateParent = snapshot.DeepTreeSurrogateParent;
			this.mode = snapshot.Mode;
			this.originalMode = snapshot.OriginalMode;
			this.framesetOk = snapshot.IsFramesetOk;
			this.needToDropLF = snapshot.IsNeedToDropLF;
			this.quirks = snapshot.IsQuirks;
		}

		int FindInArray(StackNode<T> node, StackNode<T>[] arr)
		{
			for (int i = this.listPtr; i >= 0; i--)
			{
				if (node == arr[i])
				{
					return i;
				}
			}
			return -1;
		}

		public T FormPointer
		{
			get
			{
				return this.formPointer;
			}
		}

		public T HeadPointer
		{
			get
			{
				return this.headPointer;
			}
		}

		public T DeepTreeSurrogateParent
		{
			get
			{
				return this.deepTreeSurrogateParent;
			}
		}

		public StackNode<T>[] ListOfActiveFormattingElements
		{
			get
			{
				return this.listOfActiveFormattingElements;
			}
		}

		public StackNode<T>[] Stack
		{
			get
			{
				return this.stack;
			}
		}

		public InsertionMode Mode
		{
			get
			{
				return this.mode;
			}
		}

		public InsertionMode OriginalMode
		{
			get
			{
				return this.originalMode;
			}
		}

		public bool IsFramesetOk
		{
			get
			{
				return this.framesetOk;
			}
		}

		public bool IsNeedToDropLF
		{
			get
			{
				return this.needToDropLF;
			}
		}

		public bool IsQuirks
		{
			get
			{
				return this.quirks;
			}
		}

		InsertionMode mode;

		InsertionMode originalMode;

		bool framesetOk = true;

		protected Tokenizer tokenizer;

		bool needToDropLF;

		bool fragment;

		[Local]
		string contextName;

		[NsUri]
		string contextNamespace;

		T contextNode;

		StackNode<T>[] stack;

		int currentPtr = -1;

		StackNode<T>[] listOfActiveFormattingElements;

		int listPtr = -1;

		T formPointer;

		T headPointer;

		T deepTreeSurrogateParent;

		protected StringBuilder charBuffer;

		bool quirks;

		readonly Dictionary<string, Locator> idLocations = new Dictionary<string, Locator>();

		bool html4;
	}
}
