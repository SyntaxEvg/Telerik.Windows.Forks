using System;
using HtmlParserSharp.Common;

namespace HtmlParserSharp.Core
{
	sealed class ElementName
	{
		public int Flags
		{
			get
			{
				return this.flags;
			}
		}

		public DispatchGroup Group
		{
			get
			{
				return (DispatchGroup)(this.flags & 127);
			}
		}

		public bool IsCustom
		{
			get
			{
				return (this.flags & 1073741824) != 0;
			}
		}

		internal static ElementName ElementNameByBuffer(char[] buf, int offset, int length)
		{
			int value = ElementName.BufToHash(buf, length);
			int num = Array.BinarySearch<int>(ElementName.ELEMENT_HASHES, value);
			if (num < 0)
			{
				return new ElementName(Portability.NewLocalNameFromBuffer(buf, offset, length));
			}
			ElementName elementName = ElementName.ELEMENT_NAMES[num];
			string local = elementName.name;
			if (!Portability.LocalEqualsBuffer(local, buf, offset, length))
			{
				return new ElementName(Portability.NewLocalNameFromBuffer(buf, offset, length));
			}
			return elementName;
		}

		static int BufToHash(char[] buf, int len)
		{
			int num = len << 5;
			num += (int)(buf[0] - '`');
			int num2 = len;
			int num3 = 0;
			while (num3 < 4 && num2 > 0)
			{
				num2--;
				num <<= 5;
				num += (int)(buf[num2] - '`');
				num3++;
			}
			return num;
		}

		ElementName([Local] string name, [Local] string camelCaseName, int flags)
		{
			this.name = name;
			this.camelCaseName = camelCaseName;
			this.flags = flags;
		}

		internal ElementName([Local] string name)
		{
			this.name = name;
			this.camelCaseName = name;
			this.flags = 1073741824;
		}

		public ElementName CloneElementName()
		{
			return this;
		}

		public const int GROUP_MASK = 127;

		public const int CUSTOM = 1073741824;

		public const int SPECIAL = 536870912;

		public const int FOSTER_PARENTING = 268435456;

		public const int SCOPING = 134217728;

		public const int SCOPING_AS_SVG = 67108864;

		public const int SCOPING_AS_MATHML = 33554432;

		public const int HTML_INTEGRATION_POINT = 16777216;

		public const int OPTIONAL_END_TAG = 8388608;

		public static readonly ElementName NULL_ELEMENT_NAME = new ElementName(null);

		[Local]
		public readonly string name;

		[Local]
		public readonly string camelCaseName;

		public readonly int flags;

		public static readonly ElementName A = new ElementName("a", "a", 1);

		public static readonly ElementName B = new ElementName("b", "b", 45);

		public static readonly ElementName G = new ElementName("g", "g", 0);

		public static readonly ElementName I = new ElementName("i", "i", 45);

		public static readonly ElementName P = new ElementName("p", "p", 545259549);

		public static readonly ElementName Q = new ElementName("q", "q", 0);

		public static readonly ElementName S = new ElementName("s", "s", 45);

		public static readonly ElementName U = new ElementName("u", "u", 45);

		public static readonly ElementName BR = new ElementName("br", "br", 536870916);

		public static readonly ElementName CI = new ElementName("ci", "ci", 0);

		public static readonly ElementName CN = new ElementName("cn", "cn", 0);

		public static readonly ElementName DD = new ElementName("dd", "dd", 545259561);

		public static readonly ElementName DL = new ElementName("dl", "dl", 536870958);

		public static readonly ElementName DT = new ElementName("dt", "dt", 545259561);

		public static readonly ElementName EM = new ElementName("em", "em", 45);

		public static readonly ElementName EQ = new ElementName("eq", "eq", 0);

		public static readonly ElementName FN = new ElementName("fn", "fn", 0);

		public static readonly ElementName H1 = new ElementName("h1", "h1", 536870954);

		public static readonly ElementName H2 = new ElementName("h2", "h2", 536870954);

		public static readonly ElementName H3 = new ElementName("h3", "h3", 536870954);

		public static readonly ElementName H4 = new ElementName("h4", "h4", 536870954);

		public static readonly ElementName H5 = new ElementName("h5", "h5", 536870954);

		public static readonly ElementName H6 = new ElementName("h6", "h6", 536870954);

		public static readonly ElementName GT = new ElementName("gt", "gt", 0);

		public static readonly ElementName HR = new ElementName("hr", "hr", 536870934);

		public static readonly ElementName IN = new ElementName("in", "in", 0);

		public static readonly ElementName LI = new ElementName("li", "li", 545259535);

		public static readonly ElementName LN = new ElementName("ln", "ln", 0);

		public static readonly ElementName LT = new ElementName("lt", "lt", 0);

		public static readonly ElementName MI = new ElementName("mi", "mi", 33554489);

		public static readonly ElementName MN = new ElementName("mn", "mn", 33554489);

		public static readonly ElementName MO = new ElementName("mo", "mo", 33554489);

		public static readonly ElementName MS = new ElementName("ms", "ms", 33554489);

		public static readonly ElementName OL = new ElementName("ol", "ol", 536870958);

		public static readonly ElementName OR = new ElementName("or", "or", 0);

		public static readonly ElementName PI = new ElementName("pi", "pi", 0);

		public static readonly ElementName RP = new ElementName("rp", "rp", 8388661);

		public static readonly ElementName RT = new ElementName("rt", "rt", 8388661);

		public static readonly ElementName TD = new ElementName("td", "td", 679477288);

		public static readonly ElementName TH = new ElementName("th", "th", 679477288);

		public static readonly ElementName TR = new ElementName("tr", "tr", 813695013);

		public static readonly ElementName TT = new ElementName("tt", "tt", 45);

		public static readonly ElementName UL = new ElementName("ul", "ul", 536870958);

		public static readonly ElementName AND = new ElementName("and", "and", 0);

		public static readonly ElementName ARG = new ElementName("arg", "arg", 0);

		public static readonly ElementName ABS = new ElementName("abs", "abs", 0);

		public static readonly ElementName BIG = new ElementName("big", "big", 45);

		public static readonly ElementName BDO = new ElementName("bdo", "bdo", 0);

		public static readonly ElementName CSC = new ElementName("csc", "csc", 0);

		public static readonly ElementName COL = new ElementName("col", "col", 536870919);

		public static readonly ElementName COS = new ElementName("cos", "cos", 0);

		public static readonly ElementName COT = new ElementName("cot", "cot", 0);

		public static readonly ElementName DEL = new ElementName("del", "del", 0);

		public static readonly ElementName DFN = new ElementName("dfn", "dfn", 0);

		public static readonly ElementName DIR = new ElementName("dir", "dir", 536870963);

		public static readonly ElementName DIV = new ElementName("div", "div", 536870962);

		public static readonly ElementName EXP = new ElementName("exp", "exp", 0);

		public static readonly ElementName GCD = new ElementName("gcd", "gcd", 0);

		public static readonly ElementName GEQ = new ElementName("geq", "geq", 0);

		public static readonly ElementName IMG = new ElementName("img", "img", 536870960);

		public static readonly ElementName INS = new ElementName("ins", "ins", 0);

		public static readonly ElementName INT = new ElementName("int", "int", 0);

		public static readonly ElementName KBD = new ElementName("kbd", "kbd", 0);

		public static readonly ElementName LOG = new ElementName("log", "log", 0);

		public static readonly ElementName LCM = new ElementName("lcm", "lcm", 0);

		public static readonly ElementName LEQ = new ElementName("leq", "leq", 0);

		public static readonly ElementName MTD = new ElementName("mtd", "mtd", 0);

		public static readonly ElementName MIN = new ElementName("min", "min", 0);

		public static readonly ElementName MAP = new ElementName("map", "map", 0);

		public static readonly ElementName MTR = new ElementName("mtr", "mtr", 0);

		public static readonly ElementName MAX = new ElementName("max", "max", 0);

		public static readonly ElementName NEQ = new ElementName("neq", "neq", 0);

		public static readonly ElementName NOT = new ElementName("not", "not", 0);

		public static readonly ElementName NAV = new ElementName("nav", "nav", 536870963);

		public static readonly ElementName PRE = new ElementName("pre", "pre", 536870956);

		public static readonly ElementName REM = new ElementName("rem", "rem", 0);

		public static readonly ElementName SUB = new ElementName("sub", "sub", 52);

		public static readonly ElementName SEC = new ElementName("sec", "sec", 0);

		public static readonly ElementName SVG = new ElementName("svg", "svg", 19);

		public static readonly ElementName SUM = new ElementName("sum", "sum", 0);

		public static readonly ElementName SIN = new ElementName("sin", "sin", 0);

		public static readonly ElementName SEP = new ElementName("sep", "sep", 0);

		public static readonly ElementName SUP = new ElementName("sup", "sup", 52);

		public static readonly ElementName SET = new ElementName("set", "set", 0);

		public static readonly ElementName TAN = new ElementName("tan", "tan", 0);

		public static readonly ElementName USE = new ElementName("use", "use", 0);

		public static readonly ElementName VAR = new ElementName("var", "var", 52);

		public static readonly ElementName WBR = new ElementName("wbr", "wbr", 536870961);

		public static readonly ElementName XMP = new ElementName("xmp", "xmp", 38);

		public static readonly ElementName XOR = new ElementName("xor", "xor", 0);

		public static readonly ElementName AREA = new ElementName("area", "area", 536870961);

		public static readonly ElementName ABBR = new ElementName("abbr", "abbr", 0);

		public static readonly ElementName BASE = new ElementName("base", "base", 536870914);

		public static readonly ElementName BVAR = new ElementName("bvar", "bvar", 0);

		public static readonly ElementName BODY = new ElementName("body", "body", 545259523);

		public static readonly ElementName CARD = new ElementName("card", "card", 0);

		public static readonly ElementName CODE = new ElementName("code", "code", 45);

		public static readonly ElementName CITE = new ElementName("cite", "cite", 0);

		public static readonly ElementName CSCH = new ElementName("csch", "csch", 0);

		public static readonly ElementName COSH = new ElementName("cosh", "cosh", 0);

		public static readonly ElementName COTH = new ElementName("coth", "coth", 0);

		public static readonly ElementName CURL = new ElementName("curl", "curl", 0);

		public static readonly ElementName DESC = new ElementName("desc", "desc", 67108923);

		public static readonly ElementName DIFF = new ElementName("diff", "diff", 0);

		public static readonly ElementName DEFS = new ElementName("defs", "defs", 0);

		public static readonly ElementName FORM = new ElementName("form", "form", 536870921);

		public static readonly ElementName FONT = new ElementName("font", "font", 64);

		public static readonly ElementName GRAD = new ElementName("grad", "grad", 0);

		public static readonly ElementName HEAD = new ElementName("head", "head", 545259540);

		public static readonly ElementName HTML = new ElementName("html", "html", 679477271);

		public static readonly ElementName LINE = new ElementName("line", "line", 0);

		public static readonly ElementName LINK = new ElementName("link", "link", 536870928);

		public static readonly ElementName LIST = new ElementName("list", "list", 0);

		public static readonly ElementName META = new ElementName("meta", "meta", 536870930);

		public static readonly ElementName MSUB = new ElementName("msub", "msub", 0);

		public static readonly ElementName MODE = new ElementName("mode", "mode", 0);

		public static readonly ElementName MATH = new ElementName("math", "math", 17);

		public static readonly ElementName MARK = new ElementName("mark", "mark", 0);

		public static readonly ElementName MASK = new ElementName("mask", "mask", 0);

		public static readonly ElementName MEAN = new ElementName("mean", "mean", 0);

		public static readonly ElementName MSUP = new ElementName("msup", "msup", 0);

		public static readonly ElementName MENU = new ElementName("menu", "menu", 536870962);

		public static readonly ElementName MROW = new ElementName("mrow", "mrow", 0);

		public static readonly ElementName NONE = new ElementName("none", "none", 0);

		public static readonly ElementName NOBR = new ElementName("nobr", "nobr", 24);

		public static readonly ElementName NEST = new ElementName("nest", "nest", 0);

		public static readonly ElementName PATH = new ElementName("path", "path", 0);

		public static readonly ElementName PLUS = new ElementName("plus", "plus", 0);

		public static readonly ElementName RULE = new ElementName("rule", "rule", 0);

		public static readonly ElementName REAL = new ElementName("real", "real", 0);

		public static readonly ElementName RELN = new ElementName("reln", "reln", 0);

		public static readonly ElementName RECT = new ElementName("rect", "rect", 0);

		public static readonly ElementName ROOT = new ElementName("root", "root", 0);

		public static readonly ElementName RUBY = new ElementName("ruby", "ruby", 52);

		public static readonly ElementName SECH = new ElementName("sech", "sech", 0);

		public static readonly ElementName SINH = new ElementName("sinh", "sinh", 0);

		public static readonly ElementName SPAN = new ElementName("span", "span", 52);

		public static readonly ElementName SAMP = new ElementName("samp", "samp", 0);

		public static readonly ElementName STOP = new ElementName("stop", "stop", 0);

		public static readonly ElementName SDEV = new ElementName("sdev", "sdev", 0);

		public static readonly ElementName TIME = new ElementName("time", "time", 0);

		public static readonly ElementName TRUE = new ElementName("true", "true", 0);

		public static readonly ElementName TREF = new ElementName("tref", "tref", 0);

		public static readonly ElementName TANH = new ElementName("tanh", "tanh", 0);

		public static readonly ElementName TEXT = new ElementName("text", "text", 0);

		public static readonly ElementName VIEW = new ElementName("view", "view", 0);

		public static readonly ElementName ASIDE = new ElementName("aside", "aside", 536870963);

		public static readonly ElementName AUDIO = new ElementName("audio", "audio", 0);

		public static readonly ElementName APPLY = new ElementName("apply", "apply", 0);

		public static readonly ElementName EMBED = new ElementName("embed", "embed", 536870960);

		public static readonly ElementName FRAME = new ElementName("frame", "frame", 536870922);

		public static readonly ElementName FALSE = new ElementName("false", "false", 0);

		public static readonly ElementName FLOOR = new ElementName("floor", "floor", 0);

		public static readonly ElementName GLYPH = new ElementName("glyph", "glyph", 0);

		public static readonly ElementName HKERN = new ElementName("hkern", "hkern", 0);

		public static readonly ElementName IMAGE = new ElementName("image", "image", 536870924);

		public static readonly ElementName IDENT = new ElementName("ident", "ident", 0);

		public static readonly ElementName INPUT = new ElementName("input", "input", 536870925);

		public static readonly ElementName LABEL = new ElementName("label", "label", 62);

		public static readonly ElementName LIMIT = new ElementName("limit", "limit", 0);

		public static readonly ElementName MFRAC = new ElementName("mfrac", "mfrac", 0);

		public static readonly ElementName MPATH = new ElementName("mpath", "mpath", 0);

		public static readonly ElementName METER = new ElementName("meter", "meter", 0);

		public static readonly ElementName MOVER = new ElementName("mover", "mover", 0);

		public static readonly ElementName MINUS = new ElementName("minus", "minus", 0);

		public static readonly ElementName MROOT = new ElementName("mroot", "mroot", 0);

		public static readonly ElementName MSQRT = new ElementName("msqrt", "msqrt", 0);

		public static readonly ElementName MTEXT = new ElementName("mtext", "mtext", 33554489);

		public static readonly ElementName NOTIN = new ElementName("notin", "notin", 0);

		public static readonly ElementName PIECE = new ElementName("piece", "piece", 0);

		public static readonly ElementName PARAM = new ElementName("param", "param", 536870967);

		public static readonly ElementName POWER = new ElementName("power", "power", 0);

		public static readonly ElementName REALS = new ElementName("reals", "reals", 0);

		public static readonly ElementName STYLE = new ElementName("style", "style", 536870945);

		public static readonly ElementName SMALL = new ElementName("small", "small", 45);

		public static readonly ElementName THEAD = new ElementName("thead", "thead", 813695015);

		public static readonly ElementName TABLE = new ElementName("table", "table", 939524130);

		public static readonly ElementName TITLE = new ElementName("title", "title", 603979812);

		public static readonly ElementName TRACK = new ElementName("track", "track", 55);

		public static readonly ElementName TSPAN = new ElementName("tspan", "tspan", 0);

		public static readonly ElementName TIMES = new ElementName("times", "times", 0);

		public static readonly ElementName TFOOT = new ElementName("tfoot", "tfoot", 813695015);

		public static readonly ElementName TBODY = new ElementName("tbody", "tbody", 813695015);

		public static readonly ElementName UNION = new ElementName("union", "union", 0);

		public static readonly ElementName VKERN = new ElementName("vkern", "vkern", 0);

		public static readonly ElementName VIDEO = new ElementName("video", "video", 0);

		public static readonly ElementName ARCSEC = new ElementName("arcsec", "arcsec", 0);

		public static readonly ElementName ARCCSC = new ElementName("arccsc", "arccsc", 0);

		public static readonly ElementName ARCTAN = new ElementName("arctan", "arctan", 0);

		public static readonly ElementName ARCSIN = new ElementName("arcsin", "arcsin", 0);

		public static readonly ElementName ARCCOS = new ElementName("arccos", "arccos", 0);

		public static readonly ElementName APPLET = new ElementName("applet", "applet", 671088683);

		public static readonly ElementName ARCCOT = new ElementName("arccot", "arccot", 0);

		public static readonly ElementName APPROX = new ElementName("approx", "approx", 0);

		public static readonly ElementName BUTTON = new ElementName("button", "button", 536870917);

		public static readonly ElementName CIRCLE = new ElementName("circle", "circle", 0);

		public static readonly ElementName CENTER = new ElementName("center", "center", 536870962);

		public static readonly ElementName CURSOR = new ElementName("cursor", "cursor", 0);

		public static readonly ElementName CANVAS = new ElementName("canvas", "canvas", 0);

		public static readonly ElementName DIVIDE = new ElementName("divide", "divide", 0);

		public static readonly ElementName DEGREE = new ElementName("degree", "degree", 0);

		public static readonly ElementName DOMAIN = new ElementName("domain", "domain", 0);

		public static readonly ElementName EXISTS = new ElementName("exists", "exists", 0);

		public static readonly ElementName FETILE = new ElementName("fetile", "feTile", 0);

		public static readonly ElementName FIGURE = new ElementName("figure", "figure", 536870963);

		public static readonly ElementName FORALL = new ElementName("forall", "forall", 0);

		public static readonly ElementName FILTER = new ElementName("filter", "filter", 0);

		public static readonly ElementName FOOTER = new ElementName("footer", "footer", 536870963);

		public static readonly ElementName HGROUP = new ElementName("hgroup", "hgroup", 536870963);

		public static readonly ElementName HEADER = new ElementName("header", "header", 536870963);

		public static readonly ElementName IFRAME = new ElementName("iframe", "iframe", 536870959);

		public static readonly ElementName KEYGEN = new ElementName("keygen", "keygen", 536870977);

		public static readonly ElementName LAMBDA = new ElementName("lambda", "lambda", 0);

		public static readonly ElementName LEGEND = new ElementName("legend", "legend", 0);

		public static readonly ElementName MSPACE = new ElementName("mspace", "mspace", 0);

		public static readonly ElementName MTABLE = new ElementName("mtable", "mtable", 0);

		public static readonly ElementName MSTYLE = new ElementName("mstyle", "mstyle", 0);

		public static readonly ElementName MGLYPH = new ElementName("mglyph", "mglyph", 56);

		public static readonly ElementName MEDIAN = new ElementName("median", "median", 0);

		public static readonly ElementName MUNDER = new ElementName("munder", "munder", 0);

		public static readonly ElementName MARKER = new ElementName("marker", "marker", 0);

		public static readonly ElementName MERROR = new ElementName("merror", "merror", 0);

		public static readonly ElementName MOMENT = new ElementName("moment", "moment", 0);

		public static readonly ElementName MATRIX = new ElementName("matrix", "matrix", 0);

		public static readonly ElementName OPTION = new ElementName("option", "option", 8388636);

		public static readonly ElementName OBJECT = new ElementName("object", "object", 671088703);

		public static readonly ElementName OUTPUT = new ElementName("output", "output", 62);

		public static readonly ElementName PRIMES = new ElementName("primes", "primes", 0);

		public static readonly ElementName SOURCE = new ElementName("source", "source", 55);

		public static readonly ElementName STRIKE = new ElementName("strike", "strike", 45);

		public static readonly ElementName STRONG = new ElementName("strong", "strong", 45);

		public static readonly ElementName SWITCH = new ElementName("switch", "switch", 0);

		public static readonly ElementName SYMBOL = new ElementName("symbol", "symbol", 0);

		public static readonly ElementName SELECT = new ElementName("select", "select", 536870944);

		public static readonly ElementName SUBSET = new ElementName("subset", "subset", 0);

		public static readonly ElementName SCRIPT = new ElementName("script", "script", 536870943);

		public static readonly ElementName TBREAK = new ElementName("tbreak", "tbreak", 0);

		public static readonly ElementName VECTOR = new ElementName("vector", "vector", 0);

		public static readonly ElementName ARTICLE = new ElementName("article", "article", 536870963);

		public static readonly ElementName ANIMATE = new ElementName("animate", "animate", 0);

		public static readonly ElementName ARCSECH = new ElementName("arcsech", "arcsech", 0);

		public static readonly ElementName ARCCSCH = new ElementName("arccsch", "arccsch", 0);

		public static readonly ElementName ARCTANH = new ElementName("arctanh", "arctanh", 0);

		public static readonly ElementName ARCSINH = new ElementName("arcsinh", "arcsinh", 0);

		public static readonly ElementName ARCCOSH = new ElementName("arccosh", "arccosh", 0);

		public static readonly ElementName ARCCOTH = new ElementName("arccoth", "arccoth", 0);

		public static readonly ElementName ACRONYM = new ElementName("acronym", "acronym", 0);

		public static readonly ElementName ADDRESS = new ElementName("address", "address", 536870963);

		public static readonly ElementName BGSOUND = new ElementName("bgsound", "bgsound", 536870928);

		public static readonly ElementName COMMAND = new ElementName("command", "command", 536870966);

		public static readonly ElementName COMPOSE = new ElementName("compose", "compose", 0);

		public static readonly ElementName CEILING = new ElementName("ceiling", "ceiling", 0);

		public static readonly ElementName CSYMBOL = new ElementName("csymbol", "csymbol", 0);

		public static readonly ElementName CAPTION = new ElementName("caption", "caption", 671088646);

		public static readonly ElementName DISCARD = new ElementName("discard", "discard", 0);

		public static readonly ElementName DECLARE = new ElementName("declare", "declare", 0);

		public static readonly ElementName DETAILS = new ElementName("details", "details", 536870963);

		public static readonly ElementName ELLIPSE = new ElementName("ellipse", "ellipse", 0);

		public static readonly ElementName FEFUNCA = new ElementName("fefunca", "feFuncA", 0);

		public static readonly ElementName FEFUNCB = new ElementName("fefuncb", "feFuncB", 0);

		public static readonly ElementName FEBLEND = new ElementName("feblend", "feBlend", 0);

		public static readonly ElementName FEFLOOD = new ElementName("feflood", "feFlood", 0);

		public static readonly ElementName FEIMAGE = new ElementName("feimage", "feImage", 0);

		public static readonly ElementName FEMERGE = new ElementName("femerge", "feMerge", 0);

		public static readonly ElementName FEFUNCG = new ElementName("fefuncg", "feFuncG", 0);

		public static readonly ElementName FEFUNCR = new ElementName("fefuncr", "feFuncR", 0);

		public static readonly ElementName HANDLER = new ElementName("handler", "handler", 0);

		public static readonly ElementName INVERSE = new ElementName("inverse", "inverse", 0);

		public static readonly ElementName IMPLIES = new ElementName("implies", "implies", 0);

		public static readonly ElementName ISINDEX = new ElementName("isindex", "isindex", 536870926);

		public static readonly ElementName LOGBASE = new ElementName("logbase", "logbase", 0);

		public static readonly ElementName LISTING = new ElementName("listing", "listing", 536870956);

		public static readonly ElementName MFENCED = new ElementName("mfenced", "mfenced", 0);

		public static readonly ElementName MPADDED = new ElementName("mpadded", "mpadded", 0);

		public static readonly ElementName MARQUEE = new ElementName("marquee", "marquee", 671088683);

		public static readonly ElementName MACTION = new ElementName("maction", "maction", 0);

		public static readonly ElementName MSUBSUP = new ElementName("msubsup", "msubsup", 0);

		public static readonly ElementName NOEMBED = new ElementName("noembed", "noembed", 536870972);

		public static readonly ElementName POLYGON = new ElementName("polygon", "polygon", 0);

		public static readonly ElementName PATTERN = new ElementName("pattern", "pattern", 0);

		public static readonly ElementName PRODUCT = new ElementName("product", "product", 0);

		public static readonly ElementName SETDIFF = new ElementName("setdiff", "setdiff", 0);

		public static readonly ElementName SECTION = new ElementName("section", "section", 536870963);

		public static readonly ElementName SUMMARY = new ElementName("summary", "summary", 536870963);

		public static readonly ElementName TENDSTO = new ElementName("tendsto", "tendsto", 0);

		public static readonly ElementName UPLIMIT = new ElementName("uplimit", "uplimit", 0);

		public static readonly ElementName ALTGLYPH = new ElementName("altglyph", "altGlyph", 0);

		public static readonly ElementName BASEFONT = new ElementName("basefont", "basefont", 536870928);

		public static readonly ElementName CLIPPATH = new ElementName("clippath", "clipPath", 0);

		public static readonly ElementName CODOMAIN = new ElementName("codomain", "codomain", 0);

		public static readonly ElementName COLGROUP = new ElementName("colgroup", "colgroup", 545259528);

		public static readonly ElementName EMPTYSET = new ElementName("emptyset", "emptyset", 0);

		public static readonly ElementName FACTOROF = new ElementName("factorof", "factorof", 0);

		public static readonly ElementName FIELDSET = new ElementName("fieldset", "fieldset", 536870973);

		public static readonly ElementName FRAMESET = new ElementName("frameset", "frameset", 536870923);

		public static readonly ElementName FEOFFSET = new ElementName("feoffset", "feOffset", 0);

		public static readonly ElementName GLYPHREF = new ElementName("glyphref", "glyphRef", 0);

		public static readonly ElementName INTERVAL = new ElementName("interval", "interval", 0);

		public static readonly ElementName INTEGERS = new ElementName("integers", "integers", 0);

		public static readonly ElementName INFINITY = new ElementName("infinity", "infinity", 0);

		public static readonly ElementName LISTENER = new ElementName("listener", "listener", 0);

		public static readonly ElementName LOWLIMIT = new ElementName("lowlimit", "lowlimit", 0);

		public static readonly ElementName METADATA = new ElementName("metadata", "metadata", 0);

		public static readonly ElementName MENCLOSE = new ElementName("menclose", "menclose", 0);

		public static readonly ElementName MENUITEM = new ElementName("menuitem", "menuitem", 536870978);

		public static readonly ElementName MPHANTOM = new ElementName("mphantom", "mphantom", 0);

		public static readonly ElementName NOFRAMES = new ElementName("noframes", "noframes", 536870937);

		public static readonly ElementName NOSCRIPT = new ElementName("noscript", "noscript", 536870938);

		public static readonly ElementName OPTGROUP = new ElementName("optgroup", "optgroup", 545259547);

		public static readonly ElementName POLYLINE = new ElementName("polyline", "polyline", 0);

		public static readonly ElementName PREFETCH = new ElementName("prefetch", "prefetch", 0);

		public static readonly ElementName PROGRESS = new ElementName("progress", "progress", 0);

		public static readonly ElementName PRSUBSET = new ElementName("prsubset", "prsubset", 0);

		public static readonly ElementName QUOTIENT = new ElementName("quotient", "quotient", 0);

		public static readonly ElementName SELECTOR = new ElementName("selector", "selector", 0);

		public static readonly ElementName TEXTAREA = new ElementName("textarea", "textarea", 536870947);

		public static readonly ElementName TEXTPATH = new ElementName("textpath", "textPath", 0);

		public static readonly ElementName VARIANCE = new ElementName("variance", "variance", 0);

		public static readonly ElementName ANIMATION = new ElementName("animation", "animation", 0);

		public static readonly ElementName CONJUGATE = new ElementName("conjugate", "conjugate", 0);

		public static readonly ElementName CONDITION = new ElementName("condition", "condition", 0);

		public static readonly ElementName COMPLEXES = new ElementName("complexes", "complexes", 0);

		public static readonly ElementName FONT_FACE = new ElementName("font-face", "font-face", 0);

		public static readonly ElementName FACTORIAL = new ElementName("factorial", "factorial", 0);

		public static readonly ElementName INTERSECT = new ElementName("intersect", "intersect", 0);

		public static readonly ElementName IMAGINARY = new ElementName("imaginary", "imaginary", 0);

		public static readonly ElementName LAPLACIAN = new ElementName("laplacian", "laplacian", 0);

		public static readonly ElementName MATRIXROW = new ElementName("matrixrow", "matrixrow", 0);

		public static readonly ElementName NOTSUBSET = new ElementName("notsubset", "notsubset", 0);

		public static readonly ElementName OTHERWISE = new ElementName("otherwise", "otherwise", 0);

		public static readonly ElementName PIECEWISE = new ElementName("piecewise", "piecewise", 0);

		public static readonly ElementName PLAINTEXT = new ElementName("plaintext", "plaintext", 536870942);

		public static readonly ElementName RATIONALS = new ElementName("rationals", "rationals", 0);

		public static readonly ElementName SEMANTICS = new ElementName("semantics", "semantics", 0);

		public static readonly ElementName TRANSPOSE = new ElementName("transpose", "transpose", 0);

		public static readonly ElementName ANNOTATION = new ElementName("annotation", "annotation", 0);

		public static readonly ElementName BLOCKQUOTE = new ElementName("blockquote", "blockquote", 536870962);

		public static readonly ElementName DIVERGENCE = new ElementName("divergence", "divergence", 0);

		public static readonly ElementName EULERGAMMA = new ElementName("eulergamma", "eulergamma", 0);

		public static readonly ElementName EQUIVALENT = new ElementName("equivalent", "equivalent", 0);

		public static readonly ElementName FIGCAPTION = new ElementName("figcaption", "figcaption", 536870963);

		public static readonly ElementName IMAGINARYI = new ElementName("imaginaryi", "imaginaryi", 0);

		public static readonly ElementName MALIGNMARK = new ElementName("malignmark", "malignmark", 56);

		public static readonly ElementName MUNDEROVER = new ElementName("munderover", "munderover", 0);

		public static readonly ElementName MLABELEDTR = new ElementName("mlabeledtr", "mlabeledtr", 0);

		public static readonly ElementName NOTANUMBER = new ElementName("notanumber", "notanumber", 0);

		public static readonly ElementName SOLIDCOLOR = new ElementName("solidcolor", "solidcolor", 0);

		public static readonly ElementName ALTGLYPHDEF = new ElementName("altglyphdef", "altGlyphDef", 0);

		public static readonly ElementName DETERMINANT = new ElementName("determinant", "determinant", 0);

		public static readonly ElementName FEMERGENODE = new ElementName("femergenode", "feMergeNode", 0);

		public static readonly ElementName FECOMPOSITE = new ElementName("fecomposite", "feComposite", 0);

		public static readonly ElementName FESPOTLIGHT = new ElementName("fespotlight", "feSpotLight", 0);

		public static readonly ElementName MALIGNGROUP = new ElementName("maligngroup", "maligngroup", 0);

		public static readonly ElementName MPRESCRIPTS = new ElementName("mprescripts", "mprescripts", 0);

		public static readonly ElementName MOMENTABOUT = new ElementName("momentabout", "momentabout", 0);

		public static readonly ElementName NOTPRSUBSET = new ElementName("notprsubset", "notprsubset", 0);

		public static readonly ElementName PARTIALDIFF = new ElementName("partialdiff", "partialdiff", 0);

		public static readonly ElementName ALTGLYPHITEM = new ElementName("altglyphitem", "altGlyphItem", 0);

		public static readonly ElementName ANIMATECOLOR = new ElementName("animatecolor", "animateColor", 0);

		public static readonly ElementName DATATEMPLATE = new ElementName("datatemplate", "datatemplate", 0);

		public static readonly ElementName EXPONENTIALE = new ElementName("exponentiale", "exponentiale", 0);

		public static readonly ElementName FETURBULENCE = new ElementName("feturbulence", "feTurbulence", 0);

		public static readonly ElementName FEPOINTLIGHT = new ElementName("fepointlight", "fePointLight", 0);

		public static readonly ElementName FEMORPHOLOGY = new ElementName("femorphology", "feMorphology", 0);

		public static readonly ElementName OUTERPRODUCT = new ElementName("outerproduct", "outerproduct", 0);

		public static readonly ElementName ANIMATEMOTION = new ElementName("animatemotion", "animateMotion", 0);

		public static readonly ElementName COLOR_PROFILE = new ElementName("color-profile", "color-profile", 0);

		public static readonly ElementName FONT_FACE_SRC = new ElementName("font-face-src", "font-face-src", 0);

		public static readonly ElementName FONT_FACE_URI = new ElementName("font-face-uri", "font-face-uri", 0);

		public static readonly ElementName FOREIGNOBJECT = new ElementName("foreignobject", "foreignObject", 67108923);

		public static readonly ElementName FECOLORMATRIX = new ElementName("fecolormatrix", "feColorMatrix", 0);

		public static readonly ElementName MISSING_GLYPH = new ElementName("missing-glyph", "missing-glyph", 0);

		public static readonly ElementName MMULTISCRIPTS = new ElementName("mmultiscripts", "mmultiscripts", 0);

		public static readonly ElementName SCALARPRODUCT = new ElementName("scalarproduct", "scalarproduct", 0);

		public static readonly ElementName VECTORPRODUCT = new ElementName("vectorproduct", "vectorproduct", 0);

		public static readonly ElementName ANNOTATION_XML = new ElementName("annotation-xml", "annotation-xml", 33554490);

		public static readonly ElementName DEFINITION_SRC = new ElementName("definition-src", "definition-src", 0);

		public static readonly ElementName FONT_FACE_NAME = new ElementName("font-face-name", "font-face-name", 0);

		public static readonly ElementName FEGAUSSIANBLUR = new ElementName("fegaussianblur", "feGaussianBlur", 0);

		public static readonly ElementName FEDISTANTLIGHT = new ElementName("fedistantlight", "feDistantLight", 0);

		public static readonly ElementName LINEARGRADIENT = new ElementName("lineargradient", "linearGradient", 0);

		public static readonly ElementName NATURALNUMBERS = new ElementName("naturalnumbers", "naturalnumbers", 0);

		public static readonly ElementName RADIALGRADIENT = new ElementName("radialgradient", "radialGradient", 0);

		public static readonly ElementName ANIMATETRANSFORM = new ElementName("animatetransform", "animateTransform", 0);

		public static readonly ElementName CARTESIANPRODUCT = new ElementName("cartesianproduct", "cartesianproduct", 0);

		public static readonly ElementName FONT_FACE_FORMAT = new ElementName("font-face-format", "font-face-format", 0);

		public static readonly ElementName FECONVOLVEMATRIX = new ElementName("feconvolvematrix", "feConvolveMatrix", 0);

		public static readonly ElementName FEDIFFUSELIGHTING = new ElementName("fediffuselighting", "feDiffuseLighting", 0);

		public static readonly ElementName FEDISPLACEMENTMAP = new ElementName("fedisplacementmap", "feDisplacementMap", 0);

		public static readonly ElementName FESPECULARLIGHTING = new ElementName("fespecularlighting", "feSpecularLighting", 0);

		public static readonly ElementName DOMAINOFAPPLICATION = new ElementName("domainofapplication", "domainofapplication", 0);

		public static readonly ElementName FECOMPONENTTRANSFER = new ElementName("fecomponenttransfer", "feComponentTransfer", 0);

		static readonly ElementName[] ELEMENT_NAMES = new ElementName[]
		{
			ElementName.A,
			ElementName.B,
			ElementName.G,
			ElementName.I,
			ElementName.P,
			ElementName.Q,
			ElementName.S,
			ElementName.U,
			ElementName.BR,
			ElementName.CI,
			ElementName.CN,
			ElementName.DD,
			ElementName.DL,
			ElementName.DT,
			ElementName.EM,
			ElementName.EQ,
			ElementName.FN,
			ElementName.H1,
			ElementName.H2,
			ElementName.H3,
			ElementName.H4,
			ElementName.H5,
			ElementName.H6,
			ElementName.GT,
			ElementName.HR,
			ElementName.IN,
			ElementName.LI,
			ElementName.LN,
			ElementName.LT,
			ElementName.MI,
			ElementName.MN,
			ElementName.MO,
			ElementName.MS,
			ElementName.OL,
			ElementName.OR,
			ElementName.PI,
			ElementName.RP,
			ElementName.RT,
			ElementName.TD,
			ElementName.TH,
			ElementName.TR,
			ElementName.TT,
			ElementName.UL,
			ElementName.AND,
			ElementName.ARG,
			ElementName.ABS,
			ElementName.BIG,
			ElementName.BDO,
			ElementName.CSC,
			ElementName.COL,
			ElementName.COS,
			ElementName.COT,
			ElementName.DEL,
			ElementName.DFN,
			ElementName.DIR,
			ElementName.DIV,
			ElementName.EXP,
			ElementName.GCD,
			ElementName.GEQ,
			ElementName.IMG,
			ElementName.INS,
			ElementName.INT,
			ElementName.KBD,
			ElementName.LOG,
			ElementName.LCM,
			ElementName.LEQ,
			ElementName.MTD,
			ElementName.MIN,
			ElementName.MAP,
			ElementName.MTR,
			ElementName.MAX,
			ElementName.NEQ,
			ElementName.NOT,
			ElementName.NAV,
			ElementName.PRE,
			ElementName.REM,
			ElementName.SUB,
			ElementName.SEC,
			ElementName.SVG,
			ElementName.SUM,
			ElementName.SIN,
			ElementName.SEP,
			ElementName.SUP,
			ElementName.SET,
			ElementName.TAN,
			ElementName.USE,
			ElementName.VAR,
			ElementName.WBR,
			ElementName.XMP,
			ElementName.XOR,
			ElementName.AREA,
			ElementName.ABBR,
			ElementName.BASE,
			ElementName.BVAR,
			ElementName.BODY,
			ElementName.CARD,
			ElementName.CODE,
			ElementName.CITE,
			ElementName.CSCH,
			ElementName.COSH,
			ElementName.COTH,
			ElementName.CURL,
			ElementName.DESC,
			ElementName.DIFF,
			ElementName.DEFS,
			ElementName.FORM,
			ElementName.FONT,
			ElementName.GRAD,
			ElementName.HEAD,
			ElementName.HTML,
			ElementName.LINE,
			ElementName.LINK,
			ElementName.LIST,
			ElementName.META,
			ElementName.MSUB,
			ElementName.MODE,
			ElementName.MATH,
			ElementName.MARK,
			ElementName.MASK,
			ElementName.MEAN,
			ElementName.MSUP,
			ElementName.MENU,
			ElementName.MROW,
			ElementName.NONE,
			ElementName.NOBR,
			ElementName.NEST,
			ElementName.PATH,
			ElementName.PLUS,
			ElementName.RULE,
			ElementName.REAL,
			ElementName.RELN,
			ElementName.RECT,
			ElementName.ROOT,
			ElementName.RUBY,
			ElementName.SECH,
			ElementName.SINH,
			ElementName.SPAN,
			ElementName.SAMP,
			ElementName.STOP,
			ElementName.SDEV,
			ElementName.TIME,
			ElementName.TRUE,
			ElementName.TREF,
			ElementName.TANH,
			ElementName.TEXT,
			ElementName.VIEW,
			ElementName.ASIDE,
			ElementName.AUDIO,
			ElementName.APPLY,
			ElementName.EMBED,
			ElementName.FRAME,
			ElementName.FALSE,
			ElementName.FLOOR,
			ElementName.GLYPH,
			ElementName.HKERN,
			ElementName.IMAGE,
			ElementName.IDENT,
			ElementName.INPUT,
			ElementName.LABEL,
			ElementName.LIMIT,
			ElementName.MFRAC,
			ElementName.MPATH,
			ElementName.METER,
			ElementName.MOVER,
			ElementName.MINUS,
			ElementName.MROOT,
			ElementName.MSQRT,
			ElementName.MTEXT,
			ElementName.NOTIN,
			ElementName.PIECE,
			ElementName.PARAM,
			ElementName.POWER,
			ElementName.REALS,
			ElementName.STYLE,
			ElementName.SMALL,
			ElementName.THEAD,
			ElementName.TABLE,
			ElementName.TITLE,
			ElementName.TRACK,
			ElementName.TSPAN,
			ElementName.TIMES,
			ElementName.TFOOT,
			ElementName.TBODY,
			ElementName.UNION,
			ElementName.VKERN,
			ElementName.VIDEO,
			ElementName.ARCSEC,
			ElementName.ARCCSC,
			ElementName.ARCTAN,
			ElementName.ARCSIN,
			ElementName.ARCCOS,
			ElementName.APPLET,
			ElementName.ARCCOT,
			ElementName.APPROX,
			ElementName.BUTTON,
			ElementName.CIRCLE,
			ElementName.CENTER,
			ElementName.CURSOR,
			ElementName.CANVAS,
			ElementName.DIVIDE,
			ElementName.DEGREE,
			ElementName.DOMAIN,
			ElementName.EXISTS,
			ElementName.FETILE,
			ElementName.FIGURE,
			ElementName.FORALL,
			ElementName.FILTER,
			ElementName.FOOTER,
			ElementName.HGROUP,
			ElementName.HEADER,
			ElementName.IFRAME,
			ElementName.KEYGEN,
			ElementName.LAMBDA,
			ElementName.LEGEND,
			ElementName.MSPACE,
			ElementName.MTABLE,
			ElementName.MSTYLE,
			ElementName.MGLYPH,
			ElementName.MEDIAN,
			ElementName.MUNDER,
			ElementName.MARKER,
			ElementName.MERROR,
			ElementName.MOMENT,
			ElementName.MATRIX,
			ElementName.OPTION,
			ElementName.OBJECT,
			ElementName.OUTPUT,
			ElementName.PRIMES,
			ElementName.SOURCE,
			ElementName.STRIKE,
			ElementName.STRONG,
			ElementName.SWITCH,
			ElementName.SYMBOL,
			ElementName.SELECT,
			ElementName.SUBSET,
			ElementName.SCRIPT,
			ElementName.TBREAK,
			ElementName.VECTOR,
			ElementName.ARTICLE,
			ElementName.ANIMATE,
			ElementName.ARCSECH,
			ElementName.ARCCSCH,
			ElementName.ARCTANH,
			ElementName.ARCSINH,
			ElementName.ARCCOSH,
			ElementName.ARCCOTH,
			ElementName.ACRONYM,
			ElementName.ADDRESS,
			ElementName.BGSOUND,
			ElementName.COMMAND,
			ElementName.COMPOSE,
			ElementName.CEILING,
			ElementName.CSYMBOL,
			ElementName.CAPTION,
			ElementName.DISCARD,
			ElementName.DECLARE,
			ElementName.DETAILS,
			ElementName.ELLIPSE,
			ElementName.FEFUNCA,
			ElementName.FEFUNCB,
			ElementName.FEBLEND,
			ElementName.FEFLOOD,
			ElementName.FEIMAGE,
			ElementName.FEMERGE,
			ElementName.FEFUNCG,
			ElementName.FEFUNCR,
			ElementName.HANDLER,
			ElementName.INVERSE,
			ElementName.IMPLIES,
			ElementName.ISINDEX,
			ElementName.LOGBASE,
			ElementName.LISTING,
			ElementName.MFENCED,
			ElementName.MPADDED,
			ElementName.MARQUEE,
			ElementName.MACTION,
			ElementName.MSUBSUP,
			ElementName.NOEMBED,
			ElementName.POLYGON,
			ElementName.PATTERN,
			ElementName.PRODUCT,
			ElementName.SETDIFF,
			ElementName.SECTION,
			ElementName.SUMMARY,
			ElementName.TENDSTO,
			ElementName.UPLIMIT,
			ElementName.ALTGLYPH,
			ElementName.BASEFONT,
			ElementName.CLIPPATH,
			ElementName.CODOMAIN,
			ElementName.COLGROUP,
			ElementName.EMPTYSET,
			ElementName.FACTOROF,
			ElementName.FIELDSET,
			ElementName.FRAMESET,
			ElementName.FEOFFSET,
			ElementName.GLYPHREF,
			ElementName.INTERVAL,
			ElementName.INTEGERS,
			ElementName.INFINITY,
			ElementName.LISTENER,
			ElementName.LOWLIMIT,
			ElementName.METADATA,
			ElementName.MENCLOSE,
			ElementName.MENUITEM,
			ElementName.MPHANTOM,
			ElementName.NOFRAMES,
			ElementName.NOSCRIPT,
			ElementName.OPTGROUP,
			ElementName.POLYLINE,
			ElementName.PREFETCH,
			ElementName.PROGRESS,
			ElementName.PRSUBSET,
			ElementName.QUOTIENT,
			ElementName.SELECTOR,
			ElementName.TEXTAREA,
			ElementName.TEXTPATH,
			ElementName.VARIANCE,
			ElementName.ANIMATION,
			ElementName.CONJUGATE,
			ElementName.CONDITION,
			ElementName.COMPLEXES,
			ElementName.FONT_FACE,
			ElementName.FACTORIAL,
			ElementName.INTERSECT,
			ElementName.IMAGINARY,
			ElementName.LAPLACIAN,
			ElementName.MATRIXROW,
			ElementName.NOTSUBSET,
			ElementName.OTHERWISE,
			ElementName.PIECEWISE,
			ElementName.PLAINTEXT,
			ElementName.RATIONALS,
			ElementName.SEMANTICS,
			ElementName.TRANSPOSE,
			ElementName.ANNOTATION,
			ElementName.BLOCKQUOTE,
			ElementName.DIVERGENCE,
			ElementName.EULERGAMMA,
			ElementName.EQUIVALENT,
			ElementName.FIGCAPTION,
			ElementName.IMAGINARYI,
			ElementName.MALIGNMARK,
			ElementName.MUNDEROVER,
			ElementName.MLABELEDTR,
			ElementName.NOTANUMBER,
			ElementName.SOLIDCOLOR,
			ElementName.ALTGLYPHDEF,
			ElementName.DETERMINANT,
			ElementName.FEMERGENODE,
			ElementName.FECOMPOSITE,
			ElementName.FESPOTLIGHT,
			ElementName.MALIGNGROUP,
			ElementName.MPRESCRIPTS,
			ElementName.MOMENTABOUT,
			ElementName.NOTPRSUBSET,
			ElementName.PARTIALDIFF,
			ElementName.ALTGLYPHITEM,
			ElementName.ANIMATECOLOR,
			ElementName.DATATEMPLATE,
			ElementName.EXPONENTIALE,
			ElementName.FETURBULENCE,
			ElementName.FEPOINTLIGHT,
			ElementName.FEMORPHOLOGY,
			ElementName.OUTERPRODUCT,
			ElementName.ANIMATEMOTION,
			ElementName.COLOR_PROFILE,
			ElementName.FONT_FACE_SRC,
			ElementName.FONT_FACE_URI,
			ElementName.FOREIGNOBJECT,
			ElementName.FECOLORMATRIX,
			ElementName.MISSING_GLYPH,
			ElementName.MMULTISCRIPTS,
			ElementName.SCALARPRODUCT,
			ElementName.VECTORPRODUCT,
			ElementName.ANNOTATION_XML,
			ElementName.DEFINITION_SRC,
			ElementName.FONT_FACE_NAME,
			ElementName.FEGAUSSIANBLUR,
			ElementName.FEDISTANTLIGHT,
			ElementName.LINEARGRADIENT,
			ElementName.NATURALNUMBERS,
			ElementName.RADIALGRADIENT,
			ElementName.ANIMATETRANSFORM,
			ElementName.CARTESIANPRODUCT,
			ElementName.FONT_FACE_FORMAT,
			ElementName.FECONVOLVEMATRIX,
			ElementName.FEDIFFUSELIGHTING,
			ElementName.FEDISPLACEMENTMAP,
			ElementName.FESPECULARLIGHTING,
			ElementName.DOMAINOFAPPLICATION,
			ElementName.FECOMPONENTTRANSFER
		};

		static readonly int[] ELEMENT_HASHES = new int[]
		{
			1057, 1090, 1255, 1321, 1552, 1585, 1651, 1717, 68162, 68899,
			69059, 69764, 70020, 70276, 71077, 71205, 72134, 72232, 72264, 72296,
			72328, 72360, 72392, 73351, 74312, 75209, 78124, 78284, 78476, 79149,
			79309, 79341, 79469, 81295, 81487, 82224, 84498, 84626, 86164, 86292,
			86612, 86676, 87445, 3183041, 3186241, 3198017, 3218722, 3226754, 3247715, 3256803,
			3263971, 3264995, 3289252, 3291332, 3295524, 3299620, 3326725, 3379303, 3392679, 3448233,
			3460553, 3461577, 3510347, 3546604, 3552364, 3556524, 3576461, 3586349, 3588141, 3590797,
			3596333, 3622062, 3625454, 3627054, 3675728, 3749042, 3771059, 3771571, 3776211, 3782323,
			3782963, 3784883, 3785395, 3788979, 3815476, 3839605, 3885110, 3917911, 3948984, 3951096,
			135304769, 135858241, 136498210, 136906434, 137138658, 137512995, 137531875, 137548067, 137629283, 137645539,
			137646563, 137775779, 138529956, 138615076, 139040932, 140954086, 141179366, 141690439, 142738600, 143013512,
			146979116, 147175724, 147475756, 147902637, 147936877, 148017645, 148131885, 148228141, 148229165, 148309165,
			148395629, 148551853, 148618829, 149076462, 149490158, 149572782, 151277616, 151639440, 153268914, 153486514,
			153563314, 153750706, 153763314, 153914034, 154406067, 154417459, 154600979, 154678323, 154680979, 154866835,
			155366708, 155375188, 155391572, 155465780, 155869364, 158045494, 168988979, 169321621, 169652752, 173151309,
			174240818, 174247297, 174669292, 175391532, 176638123, 177380397, 177879204, 177886734, 180753473, 181020073,
			181503558, 181686320, 181999237, 181999311, 182048201, 182074866, 182078003, 182083764, 182920847, 184716457,
			184976961, 185145071, 187281445, 187872052, 188100653, 188875944, 188919873, 188920457, 189107250, 189203987,
			189371817, 189414886, 189567458, 190266670, 191318187, 191337609, 202479203, 202493027, 202835587, 202843747,
			203013219, 203036048, 203045987, 203177552, 203898516, 204648562, 205067918, 205078130, 205096654, 205689142,
			205690439, 205988909, 207213161, 207794484, 207800999, 208023602, 208213644, 208213647, 210261490, 210310273,
			210940978, 213325049, 213946445, 214055079, 215125040, 215134273, 215135028, 215237420, 215418148, 215553166,
			215553394, 215563858, 215627949, 215754324, 217529652, 217713834, 217732628, 218731945, 221417045, 221424946,
			221493746, 221515401, 221658189, 221908140, 221910626, 221921586, 222659762, 225001091, 236105833, 236113965,
			236194995, 236195427, 236206132, 236206387, 236211683, 236212707, 236381647, 236571826, 237124271, 238172205,
			238210544, 238270764, 238435405, 238501172, 239224867, 239257644, 239710497, 240307721, 241208789, 241241557,
			241318060, 241319404, 241343533, 241344069, 241405397, 241765845, 243864964, 244502085, 244946220, 245109902,
			247647266, 247707956, 248648814, 248648836, 248682161, 248986932, 249058914, 249697357, 252132601, 252135604,
			252317348, 255007012, 255278388, 255641645, 256365156, 257566121, 269763372, 271202790, 271863856, 272049197,
			272127474, 274339449, 274939471, 275388004, 275388005, 275388006, 275977800, 278267602, 278513831, 278712622,
			281613765, 281683369, 282120228, 282250732, 282498697, 282508942, 283743649, 283787570, 284710386, 285391148,
			285478533, 285854898, 285873762, 286931113, 288964227, 289445441, 289689648, 291671489, 303512884, 305319975,
			305610036, 305764101, 308448294, 308675890, 312085683, 312264750, 315032867, 316391000, 317331042, 317902135,
			318950711, 319447220, 321499182, 322538804, 323145200, 337067316, 337826293, 339905989, 340833697, 341457068,
			342310196, 345302593, 349554733, 349771471, 349786245, 350819405, 356072847, 370349192, 373962798, 375558638,
			375574835, 376053993, 383276530, 383373833, 383407586, 384439906, 386079012, 404133513, 404307343, 407031852,
			408072233, 409112005, 409608425, 409771500, 419040932, 437730612, 439529766, 442616365, 442813037, 443157674,
			443295316, 450118444, 450482697, 456789668, 459935396, 471217869, 474073645, 476230702, 476665218, 476717289,
			483014825, 485083298, 489306281, 538364390, 540675748, 543819186, 543958612, 576960820, 577242548, 610515252,
			642202932, 644420819
		};
	}
}
