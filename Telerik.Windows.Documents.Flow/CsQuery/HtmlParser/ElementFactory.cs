using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using CsQuery.Engine;
using CsQuery.Implementation;
using CsQuery.StringScanner;
using HtmlParserSharp.Common;
using HtmlParserSharp.Core;

namespace CsQuery.HtmlParser
{
	class ElementFactory
	{
		static ElementFactory()
		{
			ElementFactory.ConfigureDefaultContextMap();
		}

		public ElementFactory()
			: this(Config.DomIndexProvider)
		{
		}

		public ElementFactory(IDomIndexProvider domIndexProvider)
		{
			this.DomIndexProvider = domIndexProvider;
		}

		public static IDomDocument Create(Stream html, Encoding streamEncoding, HtmlParsingMode parsingMode = HtmlParsingMode.Auto, HtmlParsingOptions parsingOptions = HtmlParsingOptions.Default, DocType docType = DocType.Default)
		{
			return ElementFactory.GetNewParser(parsingMode, parsingOptions, docType).Parse(html, streamEncoding);
		}

		static ElementFactory GetNewParser()
		{
			return new ElementFactory();
		}

		static ElementFactory GetNewParser(HtmlParsingMode parsingMode, HtmlParsingOptions parsingOptions, DocType docType)
		{
			return new ElementFactory
			{
				HtmlParsingMode = parsingMode,
				DocType = ElementFactory.GetDocType(docType),
				HtmlParsingOptions = ElementFactory.MergeOptions(parsingOptions)
			};
		}

		public HtmlParsingMode HtmlParsingMode { get; set; }

		public HtmlParsingOptions HtmlParsingOptions { get; set; }

		public DocType DocType { get; set; }

		public string FragmentContext { get; set; }

		public IDomDocument Parse(Stream inputStream, Encoding encoding)
		{
			this.ActiveStream = inputStream;
			this.ActiveEncoding = encoding;
			byte[] buffer = new byte[4096];
			inputStream.Read(buffer, 0, 4096);
			MemoryStream memoryStream = new MemoryStream(buffer);
			if (memoryStream.Length == 0L)
			{
				return new DomFragment();
			}
			BOMReader bomreader = new BOMReader(memoryStream);
			Stream stream;
			if (bomreader.IsBOM)
			{
				if (this.ActiveEncoding == null || (bomreader.Encoding != null && (bomreader.Encoding.WebName == "utf-8" || bomreader.Encoding.WebName == "utf-16")))
				{
					this.ActiveEncoding = bomreader.Encoding;
				}
				stream = new CombinedStream(new Stream[] { bomreader.StreamWithoutBOM, inputStream });
			}
			else
			{
				memoryStream.Position = 0L;
				stream = new CombinedStream(new Stream[] { memoryStream, inputStream });
			}
			this.ActiveStreamReader = new StreamReader(stream, this.ActiveEncoding ?? Encoding.UTF8, false);
			if (this.HtmlParsingMode == HtmlParsingMode.Auto || (this.HtmlParsingMode == HtmlParsingMode.Fragment && string.IsNullOrEmpty(this.FragmentContext)))
			{
				string text;
				this.ActiveStreamReader = this.GetContextFromStream(this.ActiveStreamReader, out text);
				if (this.HtmlParsingMode == HtmlParsingMode.Auto)
				{
					string a;
					if ((a = text) != null)
					{
						if (a == "document")
						{
							this.HtmlParsingMode = HtmlParsingMode.Document;
							text = "";
							goto IL_17F;
						}
						if (a == "html")
						{
							this.HtmlParsingMode = HtmlParsingMode.Content;
							goto IL_17F;
						}
					}
					this.HtmlParsingMode = HtmlParsingMode.Fragment;
					this.HtmlParsingOptions = HtmlParsingOptions.AllowSelfClosingTags;
				}
				IL_17F:
				if (this.HtmlParsingMode == HtmlParsingMode.Fragment)
				{
					this.FragmentContext = text;
				}
			}
			this.Reset();
			this.Tokenize();
			if (this.ReEncode == ElementFactory.ReEncodeAction.ReEncode)
			{
				this.AlreadyReEncoded = true;
				if (this.ActiveStreamOffset >= 4096)
				{
					this.ActiveStreamReader = new StreamReader(this.ActiveStream, this.ActiveEncoding);
				}
				else
				{
					memoryStream = new MemoryStream(buffer);
					if (inputStream.CanRead)
					{
						stream = new CombinedStream(new Stream[] { memoryStream, inputStream });
					}
					else
					{
						stream = memoryStream;
					}
					this.ActiveStreamReader = new StreamReader(stream, this.ActiveEncoding);
				}
				this.Reset();
				this.Tokenize();
			}
			IDomIndexQueue domIndexQueue = this.treeBuilder.Document.DocumentIndex as IDomIndexQueue;
			if (domIndexQueue != null)
			{
				domIndexQueue.QueueChanges = true;
			}
			return this.treeBuilder.Document;
		}

		static HtmlParsingOptions MergeOptions(HtmlParsingOptions options)
		{
			if (options.HasFlag(HtmlParsingOptions.Default))
			{
				return Config.HtmlParsingOptions | (options & ~HtmlParsingOptions.Default);
			}
			return options;
		}

		static DocType GetDocType(DocType docType)
		{
			if (docType != DocType.Default)
			{
				return docType;
			}
			return Config.DocType;
		}

		void ConfigureTreeBuilderForParsingMode()
		{
			switch (this.HtmlParsingMode)
			{
			case HtmlParsingMode.Fragment:
				this.treeBuilder.DoctypeExpectation = DoctypeExpectation.Html;
				this.treeBuilder.SetFragmentContext(this.FragmentContext);
				this.HtmlParsingMode = HtmlParsingMode.Auto;
				return;
			case HtmlParsingMode.Content:
				this.treeBuilder.SetFragmentContext("body");
				this.treeBuilder.DoctypeExpectation = DoctypeExpectation.Html;
				return;
			case HtmlParsingMode.Document:
				this.treeBuilder.DoctypeExpectation = DoctypeExpectation.Auto;
				return;
			default:
				return;
			}
		}

		static void SetDefaultContext(string tags, string context)
		{
			string[] array = tags.Split(new char[] { ',' });
			foreach (string text in array)
			{
				ElementFactory.DefaultContext[text.Trim()] = context;
			}
		}

		string GetContext(string tag)
		{
			string result;
			if (ElementFactory.DefaultContext.TryGetValue(tag, out result))
			{
				return result;
			}
			return "body";
		}

		TextReader GetContextFromStream(TextReader reader, out string context)
		{
			int num = 0;
			string text = "";
			string text2 = "";
			int num2 = 0;
			char[] array = new char[1];
			bool flag = false;
			while (!flag && reader.Read(array, 0, 1) > 0)
			{
				char c = array[0];
				text2 += c;
				switch (num2)
				{
				case 0:
					if (c == '<')
					{
						num2 = 1;
					}
					break;
				case 1:
					if (CharacterData.IsType(c, CharacterType.HtmlTagOpenerEnd))
					{
						flag = true;
					}
					else
					{
						text += c;
					}
					break;
				}
				num++;
			}
			context = this.GetContext(text);
			return new CombinedTextReader(new TextReader[]
			{
				new StringReader(text2),
				reader
			});
		}

		void InitializeTreeBuilder()
		{
			this.treeBuilder = new CsQueryTreeBuilder(this.DomIndexProvider);
			this.treeBuilder.NamePolicy = XmlViolationPolicy.Allow;
			this.treeBuilder.WantsComments = !this.HtmlParsingOptions.HasFlag(HtmlParsingOptions.IgnoreComments);
			this.treeBuilder.AllowSelfClosingTags = this.HtmlParsingOptions.HasFlag(HtmlParsingOptions.AllowSelfClosingTags);
		}

		void Reset()
		{
			this.InitializeTreeBuilder();
			this.tokenizer = new Tokenizer(this.treeBuilder, false);
			this.tokenizer.EncodingDeclared += this.tokenizer_EncodingDeclared;
			this.ReEncode = ElementFactory.ReEncodeAction.None;
		}

		void tokenizer_EncodingDeclared(object sender, EncodingDetectedEventArgs e)
		{
			Encoding encoding;
			try
			{
				encoding = Encoding.GetEncoding(e.Encoding);
			}
			catch
			{
				return;
			}
			bool acceptEncoding = false;
			if (encoding != null && this.ActiveEncoding == null)
			{
				this.ActiveEncoding = encoding;
				if (!this.AlreadyReEncoded && this.ActiveStreamOffset < 4096)
				{
					this.ReEncode = ElementFactory.ReEncodeAction.ReEncode;
					acceptEncoding = true;
				}
				else
				{
					this.ReEncode = ElementFactory.ReEncodeAction.ChangeEncoding;
					acceptEncoding = false;
				}
			}
			e.AcceptEncoding = acceptEncoding;
		}

		void Tokenize()
		{
			if (this.ActiveStreamReader == null)
			{
				throw new ArgumentNullException("reader was null.");
			}
			this.ConfigureTreeBuilderForParsingMode();
			this.tokenizer.Start();
			bool flag = true;
			try
			{
				char[] array = new char[2048];
				UTF16Buffer utf16Buffer = new UTF16Buffer(array, 0, 0);
				bool lastWasCR = false;
				int num;
				if ((num = this.ActiveStreamReader.Read(array, 0, array.Length)) != 0)
				{
					int num2 = 0;
					int num3 = num;
					if (flag && array[0] == '\ufeff')
					{
						this.ActiveStreamOffset = -1;
						num2 = 1;
						num3--;
					}
					if (num3 > 0)
					{
						this.tokenizer.SetTransitionBaseOffset(this.ActiveStreamOffset);
						utf16Buffer.Start = num2;
						utf16Buffer.End = num2 + num3;
						while (utf16Buffer.HasMore && !this.tokenizer.IsSuspended)
						{
							utf16Buffer.Adjust(lastWasCR);
							lastWasCR = false;
							if (utf16Buffer.HasMore && !this.tokenizer.IsSuspended)
							{
								lastWasCR = this.tokenizer.TokenizeBuffer(utf16Buffer);
							}
						}
					}
					this.CheckForReEncode();
					this.ActiveStreamOffset = num3;
					while (!this.tokenizer.IsSuspended && (num = this.ActiveStreamReader.Read(array, 0, array.Length)) != 0)
					{
						this.tokenizer.SetTransitionBaseOffset(this.ActiveStreamOffset);
						utf16Buffer.Start = 0;
						utf16Buffer.End = num;
						while (utf16Buffer.HasMore && !this.tokenizer.IsSuspended)
						{
							utf16Buffer.Adjust(lastWasCR);
							lastWasCR = false;
							if (utf16Buffer.HasMore && !this.tokenizer.IsSuspended)
							{
								lastWasCR = this.tokenizer.TokenizeBuffer(utf16Buffer);
							}
						}
						this.ActiveStreamOffset += num;
						this.CheckForReEncode();
					}
				}
				if (!this.tokenizer.IsSuspended)
				{
					this.tokenizer.Eof();
				}
			}
			finally
			{
				this.tokenizer.End();
			}
		}

		void CheckForReEncode()
		{
			if (this.ReEncode == ElementFactory.ReEncodeAction.ChangeEncoding)
			{
				this.ActiveStreamReader = new StreamReader(this.ActiveStream, this.ActiveEncoding);
				this.ReEncode = ElementFactory.ReEncodeAction.None;
			}
		}

		static void ConfigureDefaultContextMap()
		{
			ElementFactory.DefaultContext = new Dictionary<string, string>(StringComparer.CurrentCultureIgnoreCase);
			ElementFactory.SetDefaultContext("tbody,thead,tfoot,colgroup,caption", "table");
			ElementFactory.SetDefaultContext("col", "colgroup");
			ElementFactory.SetDefaultContext("tr", "tbody");
			ElementFactory.SetDefaultContext("td,th", "tr");
			ElementFactory.SetDefaultContext("option,optgroup", "select");
			ElementFactory.SetDefaultContext("dt,dd", "dl");
			ElementFactory.SetDefaultContext("li", "ol");
			ElementFactory.SetDefaultContext("meta", "head");
			ElementFactory.SetDefaultContext("title", "head");
			ElementFactory.SetDefaultContext("head", "html");
			ElementFactory.SetDefaultContext("html", "document");
			ElementFactory.SetDefaultContext("!doctype", "document");
			ElementFactory.SetDefaultContext("body", "html");
		}

		const int tokenizerBlockChars = 2048;

		const int preprocessorBlockBytes = 4096;

		static IDictionary<string, string> DefaultContext;

		Tokenizer tokenizer;

		IDomIndexProvider DomIndexProvider;

		CsQueryTreeBuilder treeBuilder;

		bool AlreadyReEncoded;

		ElementFactory.ReEncodeAction ReEncode;

		Stream ActiveStream;

		TextReader ActiveStreamReader;

		Encoding ActiveEncoding;

		int ActiveStreamOffset;

		enum ReEncodeAction
		{
			None,
			ReEncode,
			ChangeEncoding
		}
	}
}
