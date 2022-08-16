using System;
using System.Collections.Generic;

namespace Telerik.Windows.Documents.Core.PostScript.Operators
{
	abstract class Operator
	{
		public static bool IsOperator(string str)
		{
			return Operator.operators.ContainsKey(str);
		}

		public static Operator FindOperator(string op)
		{
			Operator result;
			if (!Operator.operators.TryGetValue(op, out result))
			{
				return null;
			}
			return result;
		}

		static void InitializeOperators()
		{
			Operator.operators = new Dictionary<string, Operator>();
			Operator.operators["array"] = new Array();
			Operator.operators["begin"] = new Begin();
			Operator.operators["cleartomark"] = new ClearToMark();
			Operator.operators["closefile"] = new CloseFile();
			Operator.operators["currentdict"] = new CurrentDict();
			Operator.operators["currentfile"] = new CurrentFile();
			Operator.operators["def"] = new Def();
			Operator.operators["definefont"] = new DefineFont();
			Operator.operators["dict"] = new Dict();
			Operator.operators["dup"] = new Dup();
			Operator.operators["eexec"] = new EExec();
			Operator.operators["end"] = new End();
			Operator.operators["exch"] = new Exch();
			Operator.operators["for"] = new For();
			Operator.operators["get"] = new Get();
			Operator.operators["index"] = new Index();
			Operator.operators["mark"] = new Mark();
			Operator.operators["ND"] = new ND();
			Operator.operators["|-"] = new ND();
			Operator.operators["noaccess"] = new NoAccess();
			Operator.operators["NP"] = new NP();
			Operator.operators["|"] = new NP();
			Operator.operators["pop"] = new Pop();
			Operator.operators["put"] = new Put();
			Operator.operators["RD"] = new RD();
			Operator.operators["-|"] = new RD();
			Operator.operators["readstring"] = new ReadString();
			Operator.operators["string"] = new String();
			Operator.operators["copy"] = new Copy();
			Operator.operators["systemdict"] = new SystemDict();
			Operator.operators["known"] = new Known();
			Operator.operators["if"] = new If();
			Operator.operators["ifelse"] = new IfElse();
			Operator.operators["FontDirectory"] = new FontDirectory();
			Operator.operators["add"] = new Add();
			Operator.operators["sub"] = new Sub();
			Operator.operators["mul"] = new Mul();
			Operator.operators["div"] = new Div();
			Operator.operators["idiv"] = new IDiv();
			Operator.operators["mod"] = new Mod();
			Operator.operators["atan"] = new Atan();
			Operator.operators["exp"] = new Exp();
			Operator.operators["eq"] = new Eq();
			Operator.operators["ne"] = new Ne();
			Operator.operators["gt"] = new Gt();
			Operator.operators["ge"] = new Ge();
			Operator.operators["lt"] = new Lt();
			Operator.operators["le"] = new Le();
			Operator.operators["and"] = new And();
			Operator.operators["or"] = new Or();
			Operator.operators["xor"] = new Xor();
			Operator.operators["bitshift"] = new BitShift();
			Operator.operators["neg"] = new Neg();
			Operator.operators["abs"] = new Abs();
			Operator.operators["ceiling"] = new Ceiling();
			Operator.operators["floor"] = new Floor();
			Operator.operators["round"] = new Round();
			Operator.operators["truncate"] = new Truncate();
			Operator.operators["sqrt"] = new Sqrt();
			Operator.operators["sin"] = new Sin();
			Operator.operators["cos"] = new Cos();
			Operator.operators["ln"] = new Ln();
			Operator.operators["log"] = new Log();
			Operator.operators["cvi"] = new Cvi();
			Operator.operators["cvr"] = new Cvr();
			Operator.operators["true"] = new True();
			Operator.operators["false"] = new False();
			Operator.operators["roll"] = new Roll();
			Operator.operators["userdict"] = new UserDict();
		}

		static Operator()
		{
			Operator.InitializeOperators();
		}

		public abstract void Execute(Interpreter interpreter);

		internal const string Array = "array";

		internal const string Begin = "begin";

		internal const string ClearToMark = "cleartomark";

		internal const string CloseFile = "closefile";

		internal const string CurrentDict = "currentdict";

		internal const string CurrentFile = "currentfile";

		internal const string Def = "def";

		internal const string DefineFont = "definefont";

		internal const string Dict = "dict";

		internal const string Dup = "dup";

		internal const string EExec = "eexec";

		internal const string End = "end";

		internal const string Exch = "exch";

		internal const string For = "for";

		internal const string Get = "get";

		internal const string Index = "index";

		internal const string Put = "put";

		internal const string RD = "RD";

		internal const string RDAlternate = "-|";

		internal const string Mark = "mark";

		internal const string ND = "ND";

		internal const string NDAlternate = "|-";

		internal const string NoAccess = "noaccess";

		internal const string NP = "NP";

		internal const string NPAlternate = "|";

		internal const string ReadString = "readstring";

		internal const string String = "string";

		internal const string Pop = "pop";

		internal const string Copy = "copy";

		internal const string SystemDict = "systemdict";

		internal const string Known = "known";

		internal const string If = "if";

		internal const string IfElse = "ifelse";

		internal const string FontDirectory = "FontDirectory";

		internal const string Add = "add";

		internal const string Sub = "sub";

		internal const string Mul = "mul";

		internal const string Div = "div";

		internal const string IDiv = "idiv";

		internal const string Mod = "mod";

		internal const string Neg = "neg";

		internal const string Abs = "abs";

		internal const string Ceiling = "ceiling";

		internal const string Floor = "floor";

		internal const string Round = "round";

		internal const string Truncate = "truncate";

		internal const string Sqrt = "sqrt";

		internal const string Sin = "sin";

		internal const string Cos = "cos";

		internal const string Atan = "atan";

		internal const string Exp = "exp";

		internal const string Ln = "ln";

		internal const string Log = "log";

		internal const string Cvi = "cvi";

		internal const string Cvr = "cvr";

		internal const string Eq = "eq";

		internal const string Ne = "ne";

		internal const string Gt = "gt";

		internal const string Ge = "ge";

		internal const string Lt = "lt";

		internal const string Le = "le";

		internal const string And = "and";

		internal const string Or = "or";

		internal const string Xor = "xor";

		internal const string Not = "not";

		internal const string BitShift = "bitshift";

		internal const string True = "true";

		internal const string False = "false";

		internal const string Roll = "roll";

		internal const string UserDict = "userdict";

		static Dictionary<string, Operator> operators;
	}
}
