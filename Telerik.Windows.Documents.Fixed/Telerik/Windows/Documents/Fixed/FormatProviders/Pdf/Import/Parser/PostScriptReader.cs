using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Telerik.Windows.Documents.Data.StateMachine;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Import.Parser.Keywords;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Import.Parser.PrimitiveParsers;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Types;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Import.Parser
{
	class PostScriptReader
	{
		public PostScriptReader(Stream stream, KeywordCollection keywords)
		{
			Guard.ThrowExceptionIfNull<Stream>(stream, "stream");
			this.collectionValuesStack = new Stack<Stack<PdfPrimitive>>();
			this.reader = new Reader(stream);
			this.numberParser = new NumberParser(this);
			this.hexStringParser = new HexStringParser(this);
			this.literalStringParser = new LiteralStringParser(this);
			this.arrayParser = new ArrayParser(this);
			this.dictionaryParser = new DictionaryParser(this);
			this.nameParser = new NameParser(this);
			this.keywordParser = new KeywordParser(this, keywords);
			this.stateMachine = this.BuildStateMachine();
		}

		public Reader Reader
		{
			get
			{
				return this.reader;
			}
		}

		public PdfPrimitive[] Read(IPdfImportContext context)
		{
			return this.Read(context, this.Reader.Length);
		}

		public PdfPrimitive[] Read(IPdfImportContext context, long endPosition)
		{
			Guard.ThrowExceptionIfNull<IPdfImportContext>(context, "context");
			this.PushCollection();
			PostScriptReaderArgs arguments = new PostScriptReaderArgs(context, this.reader.Read());
			this.stateMachine.Start(this.stateMachine.States.GetStateByName("Initial"), arguments);
			while (this.Reader.Position < endPosition)
			{
				arguments = new PostScriptReaderArgs(context, this.reader.Read());
				this.stateMachine.GoToNextState(arguments);
			}
			this.stateMachine.Stop(new PostScriptReaderArgs(context, 0));
			Stack<PdfPrimitive> source = this.PopCollection();
			return source.Reverse<PdfPrimitive>().ToArray<PdfPrimitive>();
		}

		public T Read<T>(IPdfImportContext context, PdfElementType tokenType) where T : PdfPrimitive
		{
			Guard.ThrowExceptionIfNull<IPdfImportContext>(context, "context");
			this.PushCollection();
			this.lastTokenType = null;
			bool flag = false;
			int count = this.collectionValuesStack.Count;
			PostScriptReaderArgs arguments = new PostScriptReaderArgs(context, this.reader.Read());
			this.stateMachine.Start(this.stateMachine.States.GetStateByName("Initial"), arguments);
			this.reader.CacheLength();
			while (!this.reader.EndOfFile && !flag)
			{
				arguments = new PostScriptReaderArgs(context, this.reader.Read());
				this.stateMachine.GoToNextState(arguments);
				flag = this.lastTokenType == tokenType && this.collectionValuesStack.Count == count;
				if (this.reader.EndOfFile && !flag)
				{
					this.stateMachine.GoToState(this.stateMachine.States.GetStateByName("Initial"), arguments);
				}
			}
			this.reader.ClearCachedLength();
			Stack<PdfPrimitive> stack = this.PopCollection();
			return (T)((object)stack.Pop());
		}

		public void ClearCollections()
		{
			this.collectionValuesStack.Clear();
		}

		public Stack<PdfPrimitive> PeekCollection()
		{
			return this.collectionValuesStack.Peek();
		}

		public void PushCollection()
		{
			this.collectionValuesStack.Push(new Stack<PdfPrimitive>());
		}

		public Stack<PdfPrimitive> PopCollection()
		{
			return this.collectionValuesStack.Pop();
		}

		public void PushToken(PdfPrimitive primitive)
		{
			if (primitive != null)
			{
				this.lastTokenType = new PdfElementType?(primitive.Type);
			}
			this.PeekCollection().Push(primitive);
		}

		public PdfPrimitive PopToken()
		{
			if (this.PeekCollection().Count > 0)
			{
				return this.PeekCollection().Pop();
			}
			return null;
		}

		StateMachine<PostScriptReaderArgs> BuildStateMachine()
		{
			StateMachine<PostScriptReaderArgs> stateMachine = new StateMachine<PostScriptReaderArgs>();
			State<PostScriptReaderArgs> state = new State<PostScriptReaderArgs>("Initial", null, null, null, null, null);
			string name = "Sign";
			Action<PostScriptReaderArgs> onEnterAction = delegate(PostScriptReaderArgs b)
			{
				this.sign = new byte?(b.Byte);
			};
			Action<PostScriptReaderArgs> onStayAction = delegate(PostScriptReaderArgs b)
			{
				this.sign = new byte?(b.Byte);
			};
			State<PostScriptReaderArgs> state2 = new State<PostScriptReaderArgs>(name, onEnterAction, delegate(PostScriptReaderArgs b)
			{
				if (!Characters.IsNumberCharacter(b.Byte))
				{
					this.sign = null;
				}
			}, onStayAction, null, null);
			string name2 = "Number";
			Action<PostScriptReaderArgs> onEnterAction2 = delegate(PostScriptReaderArgs b)
			{
				if (this.sign != null)
				{
					this.numberParser.Start((char)this.sign.Value);
					this.sign = null;
				}
				this.numberParser.Append(b);
			};
			onStayAction = new Action<PostScriptReaderArgs>(this.numberParser.Append);
			State<PostScriptReaderArgs> state3 = new State<PostScriptReaderArgs>(name2, onEnterAction2, delegate(PostScriptReaderArgs b)
			{
				this.numberParser.Complete();
			}, onStayAction, null, delegate(PostScriptReaderArgs b)
			{
				this.numberParser.Complete();
			});
			State<PostScriptReaderArgs> state4 = new State<PostScriptReaderArgs>("HexStringOrDictionaryStart", null, null, null, null, null);
			string name3 = "LiteralString";
			onStayAction = new Action<PostScriptReaderArgs>(this.literalStringParser.Append);
			State<PostScriptReaderArgs> state5 = new State<PostScriptReaderArgs>(name3, null, new Action<PostScriptReaderArgs>(this.literalStringParser.Complete), onStayAction, null, null);
			State<PostScriptReaderArgs> state6 = new State<PostScriptReaderArgs>("ArrayStart", delegate(PostScriptReaderArgs b)
			{
				this.arrayParser.Start();
			}, null, delegate(PostScriptReaderArgs b)
			{
				this.arrayParser.Start();
			}, null, null);
			State<PostScriptReaderArgs> state7 = new State<PostScriptReaderArgs>("ArrayEnd", delegate(PostScriptReaderArgs b)
			{
				this.arrayParser.End();
			}, null, delegate(PostScriptReaderArgs b)
			{
				this.arrayParser.End();
			}, null, null);
			State<PostScriptReaderArgs> state8 = new State<PostScriptReaderArgs>("DictionaryStart", delegate(PostScriptReaderArgs b)
			{
				this.dictionaryParser.Start();
			}, null, null, null, null);
			State<PostScriptReaderArgs> state9 = new State<PostScriptReaderArgs>("DictionaryEndMiddle", null, null, null, null, null);
			State<PostScriptReaderArgs> state10 = new State<PostScriptReaderArgs>("DictionaryEnd", delegate(PostScriptReaderArgs b)
			{
				this.dictionaryParser.End();
			}, null, null, null, null);
			State<PostScriptReaderArgs> state11 = new State<PostScriptReaderArgs>("NameStart", null, delegate(PostScriptReaderArgs b)
			{
				if (!Characters.IsValidNameCharacter(b.Byte))
				{
					this.nameParser.Complete();
				}
			}, null, null, null);
			string name4 = "Name";
			Action<PostScriptReaderArgs> onEnterAction3 = new Action<PostScriptReaderArgs>(this.nameParser.Append);
			onStayAction = new Action<PostScriptReaderArgs>(this.nameParser.Append);
			State<PostScriptReaderArgs> state12 = new State<PostScriptReaderArgs>(name4, onEnterAction3, delegate(PostScriptReaderArgs b)
			{
				this.nameParser.Complete();
			}, onStayAction, null, delegate(PostScriptReaderArgs b)
			{
				this.nameParser.Complete();
			});
			string name5 = "Keyword";
			Action<PostScriptReaderArgs> onEnterAction4 = new Action<PostScriptReaderArgs>(this.keywordParser.Append);
			onStayAction = new Action<PostScriptReaderArgs>(this.keywordParser.Append);
			State<PostScriptReaderArgs> state13 = new State<PostScriptReaderArgs>(name5, onEnterAction4, new Action<PostScriptReaderArgs>(this.keywordParser.Complete), onStayAction, null, new Action<PostScriptReaderArgs>(this.keywordParser.Complete));
			string name6 = "HexString";
			Action<PostScriptReaderArgs> onEnterAction5 = new Action<PostScriptReaderArgs>(this.hexStringParser.Append);
			onStayAction = new Action<PostScriptReaderArgs>(this.hexStringParser.Append);
			State<PostScriptReaderArgs> state14 = new State<PostScriptReaderArgs>(name6, onEnterAction5, new Action<PostScriptReaderArgs>(this.hexStringParser.Complete), onStayAction, null, null);
			State<PostScriptReaderArgs> state15 = new State<PostScriptReaderArgs>("Comment", null, null, null, null, null);
			State<PostScriptReaderArgs> state16 = new State<PostScriptReaderArgs>("EmptyHexString", new Action<PostScriptReaderArgs>(this.hexStringParser.Complete), null, null, null, null);
			stateMachine.States.Add(state);
			stateMachine.States.Add(state2);
			stateMachine.States.Add(state3);
			stateMachine.States.Add(state4);
			stateMachine.States.Add(state5);
			stateMachine.States.Add(state6);
			stateMachine.States.Add(state7);
			stateMachine.States.Add(state8);
			stateMachine.States.Add(state9);
			stateMachine.States.Add(state10);
			stateMachine.States.Add(state11);
			stateMachine.States.Add(state12);
			stateMachine.States.Add(state13);
			stateMachine.States.Add(state14);
			stateMachine.States.Add(state15);
			stateMachine.States.Add(state16);
			Transition<PostScriptReaderArgs> transition = new Transition<PostScriptReaderArgs>(state, state3, (PostScriptReaderArgs args) => Characters.IsNumberCharacter(args.Byte));
			Transition<PostScriptReaderArgs> transition2 = new Transition<PostScriptReaderArgs>(state, state2, (PostScriptReaderArgs args) => Characters.IsSign(args.Byte));
			Transition<PostScriptReaderArgs> transition3 = new Transition<PostScriptReaderArgs>(state, state4, (PostScriptReaderArgs args) => Characters.IsHexStringOrDictionaryStart(args.Byte));
			Transition<PostScriptReaderArgs> transition4 = new Transition<PostScriptReaderArgs>(state, state5, (PostScriptReaderArgs args) => Characters.IsLiteralStringStart(args.Byte));
			Transition<PostScriptReaderArgs> transition5 = new Transition<PostScriptReaderArgs>(state, state6, (PostScriptReaderArgs args) => Characters.IsArrayStart(args.Byte));
			Transition<PostScriptReaderArgs> transition6 = new Transition<PostScriptReaderArgs>(state, state7, (PostScriptReaderArgs args) => Characters.IsArrayEnd(args.Byte));
			Transition<PostScriptReaderArgs> transition7 = new Transition<PostScriptReaderArgs>(state, state9, (PostScriptReaderArgs args) => Characters.IsHexStringOrDictionaryEnd(args.Byte));
			Transition<PostScriptReaderArgs> transition8 = new Transition<PostScriptReaderArgs>(state, state11, (PostScriptReaderArgs args) => Characters.IsNameStart(args.Byte));
			Transition<PostScriptReaderArgs> transition9 = new Transition<PostScriptReaderArgs>(state, state13, (PostScriptReaderArgs args) => Characters.IsKeywordStartCharacter(args.Byte));
			Transition<PostScriptReaderArgs> transition10 = new Transition<PostScriptReaderArgs>(state, state15, (PostScriptReaderArgs args) => Characters.IsCommentStart(args.Byte));
			stateMachine.Transitions.AddTransition(transition);
			stateMachine.Transitions.AddTransition(transition2);
			stateMachine.Transitions.AddTransition(transition3);
			stateMachine.Transitions.AddTransition(transition4);
			stateMachine.Transitions.AddTransition(transition5);
			stateMachine.Transitions.AddTransition(transition6);
			stateMachine.Transitions.AddTransition(transition7);
			stateMachine.Transitions.AddTransition(transition8);
			stateMachine.Transitions.AddTransition(transition9);
			stateMachine.Transitions.AddTransition(transition10);
			Transition<PostScriptReaderArgs> transition11 = new Transition<PostScriptReaderArgs>(state2, state, (PostScriptReaderArgs args) => !Characters.IsNumberCharacter(args.Byte) && !Characters.IsSign(args.Byte));
			Transition<PostScriptReaderArgs> transition12 = new Transition<PostScriptReaderArgs>(state2, (PostScriptReaderArgs args) => Characters.IsSign(args.Byte));
			Transition<PostScriptReaderArgs> transition13 = new Transition<PostScriptReaderArgs>(state2, state3, (PostScriptReaderArgs args) => Characters.IsNumberCharacter(args.Byte));
			stateMachine.Transitions.AddTransition(transition13);
			stateMachine.Transitions.AddTransition(transition12);
			stateMachine.Transitions.AddTransition(transition11);
			Transition<PostScriptReaderArgs> transition14 = new Transition<PostScriptReaderArgs>(state3, state4, (PostScriptReaderArgs args) => Characters.IsHexStringOrDictionaryStart(args.Byte));
			Transition<PostScriptReaderArgs> transition15 = new Transition<PostScriptReaderArgs>(state3, state5, (PostScriptReaderArgs args) => Characters.IsLiteralStringStart(args.Byte));
			Transition<PostScriptReaderArgs> transition16 = new Transition<PostScriptReaderArgs>(state3, state6, (PostScriptReaderArgs args) => Characters.IsArrayStart(args.Byte));
			Transition<PostScriptReaderArgs> transition17 = new Transition<PostScriptReaderArgs>(state3, state7, (PostScriptReaderArgs args) => Characters.IsArrayEnd(args.Byte));
			Transition<PostScriptReaderArgs> transition18 = new Transition<PostScriptReaderArgs>(state3, state9, (PostScriptReaderArgs args) => Characters.IsHexStringOrDictionaryEnd(args.Byte));
			Transition<PostScriptReaderArgs> transition19 = new Transition<PostScriptReaderArgs>(state3, state11, (PostScriptReaderArgs args) => Characters.IsNameStart(args.Byte));
			Transition<PostScriptReaderArgs> transition20 = new Transition<PostScriptReaderArgs>(state3, new Func<PostScriptReaderArgs, bool>(this.numberParser.IsNumber));
			Transition<PostScriptReaderArgs> transition21 = new Transition<PostScriptReaderArgs>(state3, state15, (PostScriptReaderArgs args) => Characters.IsCommentStart(args.Byte));
			Transition<PostScriptReaderArgs> transition22 = new Transition<PostScriptReaderArgs>(state3, state, (PostScriptReaderArgs args) => !this.numberParser.IsNumber(args));
			stateMachine.Transitions.AddTransition(transition14);
			stateMachine.Transitions.AddTransition(transition15);
			stateMachine.Transitions.AddTransition(transition16);
			stateMachine.Transitions.AddTransition(transition17);
			stateMachine.Transitions.AddTransition(transition18);
			stateMachine.Transitions.AddTransition(transition19);
			stateMachine.Transitions.AddTransition(transition20);
			stateMachine.Transitions.AddTransition(transition21);
			stateMachine.Transitions.AddTransition(transition22);
			Transition<PostScriptReaderArgs> transition23 = new Transition<PostScriptReaderArgs>(state4, state8, (PostScriptReaderArgs args) => Characters.IsHexStringOrDictionaryStart(args.Byte));
			Transition<PostScriptReaderArgs> transition24 = new Transition<PostScriptReaderArgs>(state4, state16, (PostScriptReaderArgs args) => Characters.IsHexStringOrDictionaryEnd(args.Byte));
			Transition<PostScriptReaderArgs> transition25 = new Transition<PostScriptReaderArgs>(state4, state14, (PostScriptReaderArgs args) => true);
			stateMachine.Transitions.AddTransition(transition23);
			stateMachine.Transitions.AddTransition(transition24);
			stateMachine.Transitions.AddTransition(transition25);
			Transition<PostScriptReaderArgs> transition26 = new Transition<PostScriptReaderArgs>(state16, state3, (PostScriptReaderArgs args) => Characters.IsNumberCharacter(args.Byte));
			Transition<PostScriptReaderArgs> transition27 = new Transition<PostScriptReaderArgs>(state16, state4, (PostScriptReaderArgs args) => Characters.IsHexStringOrDictionaryStart(args.Byte));
			Transition<PostScriptReaderArgs> transition28 = new Transition<PostScriptReaderArgs>(state16, state5, (PostScriptReaderArgs args) => Characters.IsLiteralStringStart(args.Byte));
			Transition<PostScriptReaderArgs> transition29 = new Transition<PostScriptReaderArgs>(state16, state6, (PostScriptReaderArgs args) => Characters.IsArrayStart(args.Byte));
			Transition<PostScriptReaderArgs> transition30 = new Transition<PostScriptReaderArgs>(state16, state7, (PostScriptReaderArgs args) => Characters.IsArrayEnd(args.Byte));
			Transition<PostScriptReaderArgs> transition31 = new Transition<PostScriptReaderArgs>(state16, state9, (PostScriptReaderArgs args) => Characters.IsHexStringOrDictionaryEnd(args.Byte));
			Transition<PostScriptReaderArgs> transition32 = new Transition<PostScriptReaderArgs>(state16, state11, (PostScriptReaderArgs args) => Characters.IsNameStart(args.Byte));
			Transition<PostScriptReaderArgs> transition33 = new Transition<PostScriptReaderArgs>(state16, state13, (PostScriptReaderArgs args) => Characters.IsKeywordStartCharacter(args.Byte));
			Transition<PostScriptReaderArgs> transition34 = new Transition<PostScriptReaderArgs>(state16, state15, (PostScriptReaderArgs args) => Characters.IsCommentStart(args.Byte));
			stateMachine.Transitions.AddTransition(transition26);
			stateMachine.Transitions.AddTransition(transition27);
			stateMachine.Transitions.AddTransition(transition28);
			stateMachine.Transitions.AddTransition(transition29);
			stateMachine.Transitions.AddTransition(transition30);
			stateMachine.Transitions.AddTransition(transition31);
			stateMachine.Transitions.AddTransition(transition32);
			stateMachine.Transitions.AddTransition(transition33);
			stateMachine.Transitions.AddTransition(transition34);
			Transition<PostScriptReaderArgs> transition35 = new Transition<PostScriptReaderArgs>(state5, state, new Func<PostScriptReaderArgs, bool>(this.literalStringParser.IsLiteralStringEnd));
			Transition<PostScriptReaderArgs> transition36 = new Transition<PostScriptReaderArgs>(state5, (PostScriptReaderArgs b) => !this.literalStringParser.IsLiteralStringEnd(b));
			stateMachine.Transitions.AddTransition(transition35);
			stateMachine.Transitions.AddTransition(transition36);
			Transition<PostScriptReaderArgs> transition37 = new Transition<PostScriptReaderArgs>(state6, state2, (PostScriptReaderArgs args) => Characters.IsSign(args.Byte));
			Transition<PostScriptReaderArgs> transition38 = new Transition<PostScriptReaderArgs>(state6, state3, (PostScriptReaderArgs args) => Characters.IsNumberCharacter(args.Byte));
			Transition<PostScriptReaderArgs> transition39 = new Transition<PostScriptReaderArgs>(state6, state4, (PostScriptReaderArgs args) => Characters.IsHexStringOrDictionaryStart(args.Byte));
			Transition<PostScriptReaderArgs> transition40 = new Transition<PostScriptReaderArgs>(state6, state5, (PostScriptReaderArgs args) => Characters.IsLiteralStringStart(args.Byte));
			Transition<PostScriptReaderArgs> transition41 = new Transition<PostScriptReaderArgs>(state6, (PostScriptReaderArgs args) => Characters.IsArrayStart(args.Byte));
			Transition<PostScriptReaderArgs> transition42 = new Transition<PostScriptReaderArgs>(state6, state7, (PostScriptReaderArgs args) => Characters.IsArrayEnd(args.Byte));
			Transition<PostScriptReaderArgs> transition43 = new Transition<PostScriptReaderArgs>(state6, state11, (PostScriptReaderArgs args) => Characters.IsNameStart(args.Byte));
			Transition<PostScriptReaderArgs> transition44 = new Transition<PostScriptReaderArgs>(state6, state13, (PostScriptReaderArgs args) => Characters.IsKeywordStartCharacter(args.Byte));
			Transition<PostScriptReaderArgs> transition45 = new Transition<PostScriptReaderArgs>(state6, state15, (PostScriptReaderArgs args) => Characters.IsCommentStart(args.Byte));
			stateMachine.Transitions.AddTransition(transition38);
			stateMachine.Transitions.AddTransition(transition37);
			stateMachine.Transitions.AddTransition(transition39);
			stateMachine.Transitions.AddTransition(transition40);
			stateMachine.Transitions.AddTransition(transition41);
			stateMachine.Transitions.AddTransition(transition42);
			stateMachine.Transitions.AddTransition(transition43);
			stateMachine.Transitions.AddTransition(transition44);
			stateMachine.Transitions.AddTransition(transition45);
			Transition<PostScriptReaderArgs> transition46 = new Transition<PostScriptReaderArgs>(state7, state3, (PostScriptReaderArgs args) => Characters.IsNumberCharacter(args.Byte));
			Transition<PostScriptReaderArgs> transition47 = new Transition<PostScriptReaderArgs>(state7, state4, (PostScriptReaderArgs args) => Characters.IsHexStringOrDictionaryStart(args.Byte));
			Transition<PostScriptReaderArgs> transition48 = new Transition<PostScriptReaderArgs>(state7, state5, (PostScriptReaderArgs args) => Characters.IsLiteralStringStart(args.Byte));
			Transition<PostScriptReaderArgs> transition49 = new Transition<PostScriptReaderArgs>(state7, state6, (PostScriptReaderArgs args) => Characters.IsArrayStart(args.Byte));
			Transition<PostScriptReaderArgs> transition50 = new Transition<PostScriptReaderArgs>(state7, (PostScriptReaderArgs args) => Characters.IsArrayEnd(args.Byte));
			Transition<PostScriptReaderArgs> transition51 = new Transition<PostScriptReaderArgs>(state7, state9, (PostScriptReaderArgs args) => Characters.IsHexStringOrDictionaryEnd(args.Byte));
			Transition<PostScriptReaderArgs> transition52 = new Transition<PostScriptReaderArgs>(state7, state11, (PostScriptReaderArgs args) => Characters.IsNameStart(args.Byte));
			Transition<PostScriptReaderArgs> transition53 = new Transition<PostScriptReaderArgs>(state7, state13, (PostScriptReaderArgs args) => Characters.IsKeywordStartCharacter(args.Byte));
			Transition<PostScriptReaderArgs> transition54 = new Transition<PostScriptReaderArgs>(state7, state15, (PostScriptReaderArgs args) => Characters.IsCommentStart(args.Byte));
			stateMachine.Transitions.AddTransition(transition46);
			stateMachine.Transitions.AddTransition(transition47);
			stateMachine.Transitions.AddTransition(transition48);
			stateMachine.Transitions.AddTransition(transition49);
			stateMachine.Transitions.AddTransition(transition50);
			stateMachine.Transitions.AddTransition(transition51);
			stateMachine.Transitions.AddTransition(transition52);
			stateMachine.Transitions.AddTransition(transition53);
			stateMachine.Transitions.AddTransition(transition54);
			Transition<PostScriptReaderArgs> transition55 = new Transition<PostScriptReaderArgs>(state8, state3, (PostScriptReaderArgs args) => Characters.IsNumberCharacter(args.Byte));
			Transition<PostScriptReaderArgs> transition56 = new Transition<PostScriptReaderArgs>(state8, state4, (PostScriptReaderArgs args) => Characters.IsHexStringOrDictionaryStart(args.Byte));
			Transition<PostScriptReaderArgs> transition57 = new Transition<PostScriptReaderArgs>(state8, state5, (PostScriptReaderArgs args) => Characters.IsLiteralStringStart(args.Byte));
			Transition<PostScriptReaderArgs> transition58 = new Transition<PostScriptReaderArgs>(state8, state6, (PostScriptReaderArgs args) => Characters.IsArrayStart(args.Byte));
			Transition<PostScriptReaderArgs> transition59 = new Transition<PostScriptReaderArgs>(state8, state11, (PostScriptReaderArgs args) => Characters.IsNameStart(args.Byte));
			Transition<PostScriptReaderArgs> transition60 = new Transition<PostScriptReaderArgs>(state8, state13, (PostScriptReaderArgs args) => Characters.IsKeywordStartCharacter(args.Byte));
			Transition<PostScriptReaderArgs> transition61 = new Transition<PostScriptReaderArgs>(state8, state15, (PostScriptReaderArgs args) => Characters.IsCommentStart(args.Byte));
			Transition<PostScriptReaderArgs> transition62 = new Transition<PostScriptReaderArgs>(state8, state9, (PostScriptReaderArgs args) => Characters.IsHexStringOrDictionaryEnd(args.Byte));
			stateMachine.Transitions.AddTransition(transition55);
			stateMachine.Transitions.AddTransition(transition56);
			stateMachine.Transitions.AddTransition(transition57);
			stateMachine.Transitions.AddTransition(transition58);
			stateMachine.Transitions.AddTransition(transition59);
			stateMachine.Transitions.AddTransition(transition60);
			stateMachine.Transitions.AddTransition(transition61);
			stateMachine.Transitions.AddTransition(transition62);
			Transition<PostScriptReaderArgs> transition63 = new Transition<PostScriptReaderArgs>(state9, state10, (PostScriptReaderArgs args) => Characters.IsHexStringOrDictionaryEnd(args.Byte));
			stateMachine.Transitions.AddTransition(transition63);
			Transition<PostScriptReaderArgs> transition64 = new Transition<PostScriptReaderArgs>(state10, state3, (PostScriptReaderArgs args) => Characters.IsNumberCharacter(args.Byte));
			Transition<PostScriptReaderArgs> transition65 = new Transition<PostScriptReaderArgs>(state10, state4, (PostScriptReaderArgs args) => Characters.IsHexStringOrDictionaryStart(args.Byte));
			Transition<PostScriptReaderArgs> transition66 = new Transition<PostScriptReaderArgs>(state10, state5, (PostScriptReaderArgs args) => Characters.IsLiteralStringStart(args.Byte));
			Transition<PostScriptReaderArgs> transition67 = new Transition<PostScriptReaderArgs>(state10, state6, (PostScriptReaderArgs args) => Characters.IsArrayStart(args.Byte));
			Transition<PostScriptReaderArgs> transition68 = new Transition<PostScriptReaderArgs>(state10, state7, (PostScriptReaderArgs args) => Characters.IsArrayEnd(args.Byte));
			Transition<PostScriptReaderArgs> transition69 = new Transition<PostScriptReaderArgs>(state10, state9, (PostScriptReaderArgs args) => Characters.IsHexStringOrDictionaryEnd(args.Byte));
			Transition<PostScriptReaderArgs> transition70 = new Transition<PostScriptReaderArgs>(state10, state11, (PostScriptReaderArgs args) => Characters.IsNameStart(args.Byte));
			Transition<PostScriptReaderArgs> transition71 = new Transition<PostScriptReaderArgs>(state10, state13, (PostScriptReaderArgs args) => Characters.IsKeywordStartCharacter(args.Byte));
			Transition<PostScriptReaderArgs> transition72 = new Transition<PostScriptReaderArgs>(state10, state15, (PostScriptReaderArgs args) => Characters.IsCommentStart(args.Byte));
			stateMachine.Transitions.AddTransition(transition64);
			stateMachine.Transitions.AddTransition(transition65);
			stateMachine.Transitions.AddTransition(transition66);
			stateMachine.Transitions.AddTransition(transition67);
			stateMachine.Transitions.AddTransition(transition68);
			stateMachine.Transitions.AddTransition(transition69);
			stateMachine.Transitions.AddTransition(transition70);
			stateMachine.Transitions.AddTransition(transition71);
			stateMachine.Transitions.AddTransition(transition72);
			Transition<PostScriptReaderArgs> transition73 = new Transition<PostScriptReaderArgs>(state11, state12, (PostScriptReaderArgs args) => Characters.IsValidNameCharacter(args.Byte));
			Transition<PostScriptReaderArgs> transition74 = new Transition<PostScriptReaderArgs>(state11, state, (PostScriptReaderArgs args) => !Characters.IsValidNameCharacter(args.Byte));
			stateMachine.Transitions.AddTransition(transition73);
			stateMachine.Transitions.AddTransition(transition74);
			Transition<PostScriptReaderArgs> transition75 = new Transition<PostScriptReaderArgs>(state12, state4, (PostScriptReaderArgs args) => Characters.IsHexStringOrDictionaryStart(args.Byte));
			Transition<PostScriptReaderArgs> transition76 = new Transition<PostScriptReaderArgs>(state12, state5, (PostScriptReaderArgs args) => Characters.IsLiteralStringStart(args.Byte));
			Transition<PostScriptReaderArgs> transition77 = new Transition<PostScriptReaderArgs>(state12, state6, (PostScriptReaderArgs args) => Characters.IsArrayStart(args.Byte));
			Transition<PostScriptReaderArgs> transition78 = new Transition<PostScriptReaderArgs>(state12, state7, (PostScriptReaderArgs args) => Characters.IsArrayEnd(args.Byte));
			Transition<PostScriptReaderArgs> transition79 = new Transition<PostScriptReaderArgs>(state12, state9, (PostScriptReaderArgs args) => Characters.IsHexStringOrDictionaryEnd(args.Byte));
			Transition<PostScriptReaderArgs> transition80 = new Transition<PostScriptReaderArgs>(state12, state11, (PostScriptReaderArgs args) => Characters.IsNameStart(args.Byte));
			Transition<PostScriptReaderArgs> transition81 = new Transition<PostScriptReaderArgs>(state12, state15, (PostScriptReaderArgs args) => Characters.IsCommentStart(args.Byte));
			Transition<PostScriptReaderArgs> transition82 = new Transition<PostScriptReaderArgs>(state12, (PostScriptReaderArgs args) => Characters.IsValidNameCharacter(args.Byte));
			Transition<PostScriptReaderArgs> transition83 = new Transition<PostScriptReaderArgs>(state12, state, (PostScriptReaderArgs args) => !Characters.IsValidNameCharacter(args.Byte));
			stateMachine.Transitions.AddTransition(transition75);
			stateMachine.Transitions.AddTransition(transition76);
			stateMachine.Transitions.AddTransition(transition77);
			stateMachine.Transitions.AddTransition(transition78);
			stateMachine.Transitions.AddTransition(transition79);
			stateMachine.Transitions.AddTransition(transition80);
			stateMachine.Transitions.AddTransition(transition81);
			stateMachine.Transitions.AddTransition(transition82);
			stateMachine.Transitions.AddTransition(transition83);
			Transition<PostScriptReaderArgs> transition84 = new Transition<PostScriptReaderArgs>(state13, state4, (PostScriptReaderArgs args) => Characters.IsHexStringOrDictionaryStart(args.Byte));
			Transition<PostScriptReaderArgs> transition85 = new Transition<PostScriptReaderArgs>(state13, state5, (PostScriptReaderArgs args) => Characters.IsLiteralStringStart(args.Byte));
			Transition<PostScriptReaderArgs> transition86 = new Transition<PostScriptReaderArgs>(state13, state6, (PostScriptReaderArgs args) => Characters.IsArrayStart(args.Byte));
			Transition<PostScriptReaderArgs> transition87 = new Transition<PostScriptReaderArgs>(state13, state7, (PostScriptReaderArgs args) => Characters.IsArrayEnd(args.Byte));
			Transition<PostScriptReaderArgs> transition88 = new Transition<PostScriptReaderArgs>(state13, state9, (PostScriptReaderArgs args) => Characters.IsHexStringOrDictionaryEnd(args.Byte));
			Transition<PostScriptReaderArgs> transition89 = new Transition<PostScriptReaderArgs>(state13, state11, (PostScriptReaderArgs args) => Characters.IsNameStart(args.Byte));
			Transition<PostScriptReaderArgs> transition90 = new Transition<PostScriptReaderArgs>(state13, state13, (PostScriptReaderArgs args) => Characters.IsKeywordCharacter(args.Byte));
			Transition<PostScriptReaderArgs> transition91 = new Transition<PostScriptReaderArgs>(state13, state15, (PostScriptReaderArgs args) => Characters.IsCommentStart(args.Byte));
			Transition<PostScriptReaderArgs> transition92 = new Transition<PostScriptReaderArgs>(state13, state, (PostScriptReaderArgs args) => true);
			stateMachine.Transitions.AddTransition(transition84);
			stateMachine.Transitions.AddTransition(transition85);
			stateMachine.Transitions.AddTransition(transition86);
			stateMachine.Transitions.AddTransition(transition87);
			stateMachine.Transitions.AddTransition(transition88);
			stateMachine.Transitions.AddTransition(transition89);
			stateMachine.Transitions.AddTransition(transition90);
			stateMachine.Transitions.AddTransition(transition91);
			stateMachine.Transitions.AddTransition(transition92);
			Transition<PostScriptReaderArgs> transition93 = new Transition<PostScriptReaderArgs>(state14, state, (PostScriptReaderArgs args) => Characters.IsHexStringOrDictionaryEnd(args.Byte));
			Transition<PostScriptReaderArgs> transition94 = new Transition<PostScriptReaderArgs>(state14, (PostScriptReaderArgs b) => true);
			stateMachine.Transitions.AddTransition(transition93);
			stateMachine.Transitions.AddTransition(transition94);
			Transition<PostScriptReaderArgs> transition95 = new Transition<PostScriptReaderArgs>(state15, state, (PostScriptReaderArgs args) => Characters.IsLineFeed(args.Byte));
			stateMachine.Transitions.AddTransition(transition95);
			return stateMachine;
		}

		const string InitialState = "Initial";

		readonly Reader reader;

		readonly StateMachine<PostScriptReaderArgs> stateMachine;

		readonly Stack<Stack<PdfPrimitive>> collectionValuesStack;

		readonly NumberParser numberParser;

		readonly HexStringParser hexStringParser;

		readonly LiteralStringParser literalStringParser;

		readonly ArrayParser arrayParser;

		readonly DictionaryParser dictionaryParser;

		readonly NameParser nameParser;

		readonly KeywordParser keywordParser;

		PdfElementType? lastTokenType;

		byte? sign;
	}
}
