using System;
using System.Collections.Generic;
using System.Globalization;
using Telerik.Windows.Documents.Core.Fonts.Type1.Type1Format.Dictionaries;
using Telerik.Windows.Documents.Core.PostScript.Data;
using Telerik.Windows.Documents.Core.PostScript.Operators;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Core.PostScript
{
	class Interpreter
	{
		public Interpreter()
		{
			this.fonts = new Dictionary<string, Type1Font>();
			this.systemDict = new PostScriptDictionary();
			this.InitializeSystemDict();
		}

		internal OperandsCollection Operands { get; set; }

		internal Stack<PostScriptDictionary> DictionaryStack { get; set; }

		internal Stack<PostScriptArray> ArrayStack { get; set; }

		internal PostScriptDictionary CurrentDictionary
		{
			get
			{
				return this.DictionaryStack.Peek();
			}
		}

		internal PostScriptArray CurrentArray
		{
			get
			{
				return this.ArrayStack.Peek();
			}
		}

		internal PostScriptReader Reader { get; set; }

		internal PostScriptDictionary SystemDict
		{
			get
			{
				return this.systemDict;
			}
		}

		internal PostScriptArray RD
		{
			get
			{
				object obj;
				if (this.rd == null && (this.UserDict.TryGetValue("RD", out obj) || this.UserDict.TryGetValue("-|", out obj)))
				{
					this.rd = (PostScriptArray)obj;
				}
				return this.rd;
			}
			set
			{
				this.rd = value;
			}
		}

		internal PostScriptArray ND
		{
			get
			{
				object obj;
				if (this.nd == null && (this.UserDict.TryGetValue("ND", out obj) || this.UserDict.TryGetValue("|-", out obj)))
				{
					this.nd = (PostScriptArray)obj;
				}
				return this.nd;
			}
			set
			{
				this.nd = value;
			}
		}

		internal PostScriptArray NP
		{
			get
			{
				object obj;
				if (this.np == null && (this.UserDict.TryGetValue("NP", out obj) || this.UserDict.TryGetValue("|", out obj)))
				{
					this.np = (PostScriptArray)obj;
				}
				return this.np;
			}
			set
			{
				this.np = value;
			}
		}

		internal PostScriptDictionary UserDict { get; set; }

		public Dictionary<string, Type1Font> Fonts
		{
			get
			{
				return this.fonts;
			}
		}

		public void Execute(byte[] data)
		{
			this.Operands = new OperandsCollection();
			this.DictionaryStack = new Stack<PostScriptDictionary>();
			this.ArrayStack = new Stack<PostScriptArray>();
			this.UserDict = new PostScriptDictionary();
			this.Reader = new PostScriptReader(data);
			while (!this.Reader.EndOfFile)
			{
				Token token = this.Reader.ReadToken();
				if (token == Token.Operator)
				{
					Operator @operator = Operator.FindOperator(this.Reader.Result);
					@operator.Execute(this);
				}
				else if (token != Token.Unknown)
				{
					this.Operands.AddLast(Interpreter.ParseOperand(token, this.Reader));
				}
			}
		}

		internal void ExecuteProcedure(PostScriptArray proc)
		{
			Guard.ThrowExceptionIfNull<PostScriptArray>(proc, "proc");
			foreach (object obj in proc)
			{
				Operator @operator = obj as Operator;
				if (@operator != null)
				{
					@operator.Execute(this);
				}
				else
				{
					this.Operands.AddLast(obj);
				}
			}
		}

		void InitializeSystemDict()
		{
			this.systemDict["FontDirectory"] = new PostScriptDictionary();
		}

		static object ParseOperand(Token token, PostScriptReader reader)
		{
			switch (token)
			{
			case Token.Operator:
				return Operator.FindOperator(reader.Result);
			case Token.Integer:
				return Interpreter.ParseInt(reader.Result);
			case Token.Real:
				return Interpreter.ParseReal(reader.Result);
			case Token.Name:
				return reader.Result;
			case Token.ArrayStart:
				return Interpreter.ParseArray(reader);
			case Token.Keyword:
				return Keywords.GetValue(reader.Result);
			case Token.String:
				return new PostScriptString(reader.Result);
			case Token.Boolean:
				return Interpreter.ParseBool(reader.Result);
			case Token.DictionaryStart:
				return Interpreter.ParseDictionary(reader);
			}
			return null;
		}

		static object ParseBool(string b)
		{
			return b == "true";
		}

		static int ParseInt(string str)
		{
			return int.Parse(str, CultureInfo.InvariantCulture.NumberFormat);
		}

		static double ParseReal(string str)
		{
			return double.Parse(str, CultureInfo.InvariantCulture.NumberFormat);
		}

		static PostScriptArray ParseArray(PostScriptReader reader)
		{
			PostScriptArray postScriptArray = new PostScriptArray();
			Token token;
			while (!reader.EndOfFile && (token = reader.ReadToken()) != Token.ArrayEnd)
			{
				object obj = Interpreter.ParseOperand(token, reader);
				if (token != Token.Operator || obj != null)
				{
					postScriptArray.Add(obj);
				}
			}
			return postScriptArray;
		}

		static PostScriptDictionary ParseDictionary(PostScriptReader reader)
		{
			PostScriptDictionary postScriptDictionary = new PostScriptDictionary();
			Token token;
			while (!reader.EndOfFile && (token = reader.ReadToken()) != Token.ArrayEnd)
			{
				Token token2 = token;
				if (token2 == Token.Name || token2 == Token.String)
				{
					string result = reader.Result;
					postScriptDictionary[result] = Interpreter.ParseOperand(reader.ReadToken(), reader);
				}
				else
				{
					reader.ReadToken();
				}
			}
			return postScriptDictionary;
		}

		readonly Dictionary<string, Type1Font> fonts;

		readonly PostScriptDictionary systemDict;

		PostScriptArray rd;

		PostScriptArray nd;

		PostScriptArray np;
	}
}
