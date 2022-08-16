using System;
using HtmlParserSharp.Common;

namespace HtmlParserSharp.Core
{
	sealed class AttributeName
	{
		static string[] COMPUTE_QNAME(string[] local, string[] prefix)
		{
			string[] array = new string[4];
			for (int i = 0; i < array.Length; i++)
			{
				if (prefix[i] == null)
				{
					array[i] = local[i];
				}
				else
				{
					array[i] = string.Intern(prefix[i] + ':' + local[i]);
				}
			}
			return array;
		}

		[Local]
		static string[] SVG_DIFFERENT([Local] string name, [Local] string camel)
		{
			return new string[] { name, name, camel, name };
		}

		[Local]
		static string[] MATH_DIFFERENT([Local] string name, [Local] string camel)
		{
			return new string[] { name, camel, name, name };
		}

		[Local]
		static string[] COLONIFIED_LOCAL([Local] string name, [Local] string suffix)
		{
			return new string[] { name, suffix, suffix, name };
		}

		[Local]
		static string[] SAME_LOCAL([Local] string name)
		{
			return new string[] { name, name, name, name };
		}

		internal static AttributeName NameByBuffer(char[] buf, int offset, int length, bool checkNcName)
		{
			int value = AttributeName.BufToHash(buf, length);
			int num = Array.BinarySearch<int>(AttributeName.ATTRIBUTE_HASHES, value);
			if (num < 0)
			{
				return AttributeName.CreateAttributeName(Portability.NewLocalNameFromBuffer(buf, offset, length), checkNcName);
			}
			AttributeName attributeName = AttributeName.ATTRIBUTE_NAMES[num];
			string text = attributeName.GetLocal(0);
			if (!Portability.LocalEqualsBuffer(text, buf, offset, length))
			{
				return AttributeName.CreateAttributeName(Portability.NewLocalNameFromBuffer(buf, offset, length), checkNcName);
			}
			return attributeName;
		}

		static int BufToHash(char[] buf, int len)
		{
			int num = 0;
			int num2 = len << 5;
			num2 += (int)(buf[0] - '`');
			int num3 = len;
			int num4 = 0;
			while (num4 < 4 && num3 > 0)
			{
				num3--;
				num2 <<= 5;
				num2 += (int)(buf[num3] - '`');
				num <<= 6;
				num += (int)(buf[num4] - '_');
				num4++;
			}
			return num2 ^ num;
		}

		AttributeName([NsUri] string[] uri, [Local] string[] local, [Prefix] string[] prefix, int flags)
		{
			this.uri = uri;
			this.local = local;
			this.prefix = prefix;
			this.qName = AttributeName.COMPUTE_QNAME(local, prefix);
			this.flags = flags;
		}

		static AttributeName CreateAttributeName([Local] string name, bool checkNcName)
		{
			int num = 15;
			if (name.StartsWith("xmlns:"))
			{
				num = 16;
			}
			else if (checkNcName && !NCName.IsNCName(name))
			{
				num = 0;
			}
			return new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL(name), AttributeName.ALL_NO_PREFIX, num);
		}

		public AttributeName CloneAttributeName()
		{
			return this;
		}

		internal static AttributeName Create(string name)
		{
			return new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL(name), AttributeName.ALL_NO_PREFIX, 15);
		}

		public bool IsNcName(int mode)
		{
			return (this.flags & (1 << mode)) != 0;
		}

		public bool IsXmlns
		{
			get
			{
				return (this.flags & 16) != 0;
			}
		}

		internal bool IsCaseFolded
		{
			get
			{
				return (this.flags & 32) != 0;
			}
		}

		internal bool IsBoolean
		{
			get
			{
				return (this.flags & 64) != 0;
			}
		}

		public string GetQName(int mode)
		{
			return this.qName[mode];
		}

		[NsUri]
		public string GetUri(int mode)
		{
			return this.uri[mode];
		}

		[Local]
		public string GetLocal(int mode)
		{
			return this.local[mode];
		}

		[Prefix]
		public string GetPrefix(int mode)
		{
			return this.prefix[mode];
		}

		public override int GetHashCode()
		{
			string text = this.GetLocal(0);
			return AttributeName.BufToHash(text.ToCharArray(), text.Length);
		}

		public override bool Equals(object obj)
		{
			AttributeName attributeName = obj as AttributeName;
			return attributeName != null && this.GetLocal(0) == attributeName.GetLocal(0);
		}

		public static bool operator ==(AttributeName a, AttributeName b)
		{
			return object.ReferenceEquals(a, b) || (a != null && b != null && a.Equals(b));
		}

		public static bool operator !=(AttributeName a, AttributeName b)
		{
			return !(a == b);
		}

		// Note: this type is marked as 'beforefieldinit'.
		static AttributeName()
		{
			string[] all_NO_PREFIX = new string[4];
			AttributeName.ALL_NO_PREFIX = all_NO_PREFIX;
			string[] array = new string[4];
			array[1] = "xmlns";
			array[2] = "xmlns";
			AttributeName.XMLNS_PREFIX = array;
			string[] array2 = new string[4];
			array2[1] = "xlink";
			array2[2] = "xlink";
			AttributeName.XLINK_PREFIX = array2;
			string[] array3 = new string[4];
			array3[1] = "xml";
			array3[2] = "xml";
			AttributeName.XML_PREFIX = array3;
			AttributeName.LANG_PREFIX = new string[] { null, null, null, "xml" };
			AttributeName.D = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("d"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.K = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("k"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.R = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("r"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.X = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("x"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.Y = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("y"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.Z = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("z"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.BY = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("by"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.CX = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("cx"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.CY = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("cy"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.DX = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("dx"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.DY = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("dy"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.G2 = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("g2"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.G1 = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("g1"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.FX = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("fx"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.FY = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("fy"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.K4 = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("k4"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.K2 = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("k2"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.K3 = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("k3"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.K1 = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("k1"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.ID = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("id"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.IN = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("in"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.U2 = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("u2"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.U1 = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("u1"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.RT = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("rt"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.RX = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("rx"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.RY = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("ry"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.TO = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("to"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.Y2 = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("y2"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.Y1 = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("y1"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.X1 = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("x1"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.X2 = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("x2"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.ALT = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("alt"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.DIR = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("dir"), AttributeName.ALL_NO_PREFIX, 47);
			AttributeName.DUR = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("dur"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.END = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("end"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.FOR = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("for"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.IN2 = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("in2"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.MAX = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("max"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.MIN = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("min"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.LOW = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("low"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.REL = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("rel"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.REV = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("rev"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.SRC = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("src"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.AXIS = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("axis"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.ABBR = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("abbr"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.BBOX = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("bbox"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.CITE = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("cite"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.CODE = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("code"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.BIAS = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("bias"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.COLS = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("cols"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.CLIP = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("clip"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.CHAR = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("char"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.BASE = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("base"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.EDGE = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("edge"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.DATA = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("data"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.FILL = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("fill"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.FROM = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("from"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.FORM = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("form"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.FACE = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("face"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.HIGH = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("high"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.HREF = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("href"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.OPEN = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("open"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.ICON = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("icon"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.NAME = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("name"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.MODE = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("mode"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.MASK = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("mask"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.LINK = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("link"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.LANG = new AttributeName(AttributeName.LANG_NS, AttributeName.SAME_LOCAL("lang"), AttributeName.LANG_PREFIX, 15);
			AttributeName.LOOP = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("loop"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.LIST = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("list"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.TYPE = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("type"), AttributeName.ALL_NO_PREFIX, 47);
			AttributeName.WHEN = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("when"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.WRAP = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("wrap"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.TEXT = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("text"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.PATH = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("path"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.PING = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("ping"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.REFX = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SVG_DIFFERENT("refx", "refX"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.REFY = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SVG_DIFFERENT("refy", "refY"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.SIZE = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("size"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.SEED = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("seed"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.ROWS = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("rows"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.SPAN = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("span"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.STEP = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("step"), AttributeName.ALL_NO_PREFIX, 47);
			AttributeName.ROLE = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("role"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.XREF = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("xref"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.ASYNC = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("async"), AttributeName.ALL_NO_PREFIX, 111);
			AttributeName.ALINK = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("alink"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.ALIGN = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("align"), AttributeName.ALL_NO_PREFIX, 47);
			AttributeName.CLOSE = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("close"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.COLOR = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("color"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.CLASS = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("class"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.CLEAR = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("clear"), AttributeName.ALL_NO_PREFIX, 47);
			AttributeName.BEGIN = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("begin"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.DEPTH = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("depth"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.DEFER = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("defer"), AttributeName.ALL_NO_PREFIX, 111);
			AttributeName.FENCE = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("fence"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.FRAME = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("frame"), AttributeName.ALL_NO_PREFIX, 47);
			AttributeName.ISMAP = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("ismap"), AttributeName.ALL_NO_PREFIX, 111);
			AttributeName.ONEND = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("onend"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.INDEX = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("index"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.ORDER = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("order"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.OTHER = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("other"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.ONCUT = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("oncut"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.NARGS = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("nargs"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.MEDIA = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("media"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.LABEL = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("label"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.LOCAL = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("local"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.WIDTH = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("width"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.TITLE = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("title"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.VLINK = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("vlink"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.VALUE = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("value"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.SLOPE = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("slope"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.SHAPE = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("shape"), AttributeName.ALL_NO_PREFIX, 47);
			AttributeName.SCOPE = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("scope"), AttributeName.ALL_NO_PREFIX, 47);
			AttributeName.SCALE = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("scale"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.SPEED = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("speed"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.STYLE = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("style"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.RULES = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("rules"), AttributeName.ALL_NO_PREFIX, 47);
			AttributeName.STEMH = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("stemh"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.STEMV = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("stemv"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.START = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("start"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.XMLNS = new AttributeName(AttributeName.XMLNS_NS, AttributeName.SAME_LOCAL("xmlns"), AttributeName.ALL_NO_PREFIX, 16);
			AttributeName.ACCEPT = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("accept"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.ACCENT = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("accent"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.ASCENT = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("ascent"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.ACTIVE = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("active"), AttributeName.ALL_NO_PREFIX, 111);
			AttributeName.ALTIMG = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("altimg"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.ACTION = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("action"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.BORDER = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("border"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.CURSOR = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("cursor"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.COORDS = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("coords"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.FILTER = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("filter"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.FORMAT = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("format"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.HIDDEN = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("hidden"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.HSPACE = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("hspace"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.HEIGHT = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("height"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.ONMOVE = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("onmove"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.ONLOAD = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("onload"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.ONDRAG = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("ondrag"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.ORIGIN = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("origin"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.ONZOOM = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("onzoom"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.ONHELP = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("onhelp"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.ONSTOP = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("onstop"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.ONDROP = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("ondrop"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.ONBLUR = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("onblur"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.OBJECT = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("object"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.OFFSET = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("offset"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.ORIENT = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("orient"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.ONCOPY = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("oncopy"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.NOWRAP = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("nowrap"), AttributeName.ALL_NO_PREFIX, 111);
			AttributeName.NOHREF = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("nohref"), AttributeName.ALL_NO_PREFIX, 111);
			AttributeName.MACROS = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("macros"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.METHOD = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("method"), AttributeName.ALL_NO_PREFIX, 47);
			AttributeName.LOWSRC = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("lowsrc"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.LSPACE = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("lspace"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.LQUOTE = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("lquote"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.USEMAP = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("usemap"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.WIDTHS = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("widths"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.TARGET = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("target"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.VALUES = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("values"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.VALIGN = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("valign"), AttributeName.ALL_NO_PREFIX, 47);
			AttributeName.VSPACE = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("vspace"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.POSTER = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("poster"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.POINTS = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("points"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.PROMPT = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("prompt"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.SCOPED = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("scoped"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.STRING = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("string"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.SCHEME = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("scheme"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.STROKE = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("stroke"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.RADIUS = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("radius"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.RESULT = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("result"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.REPEAT = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("repeat"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.RSPACE = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("rspace"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.ROTATE = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("rotate"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.RQUOTE = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("rquote"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.ALTTEXT = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("alttext"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.ARCHIVE = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("archive"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.AZIMUTH = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("azimuth"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.CLOSURE = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("closure"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.CHECKED = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("checked"), AttributeName.ALL_NO_PREFIX, 111);
			AttributeName.CLASSID = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("classid"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.CHAROFF = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("charoff"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.BGCOLOR = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("bgcolor"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.COLSPAN = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("colspan"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.CHARSET = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("charset"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.COMPACT = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("compact"), AttributeName.ALL_NO_PREFIX, 111);
			AttributeName.CONTENT = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("content"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.ENCTYPE = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("enctype"), AttributeName.ALL_NO_PREFIX, 47);
			AttributeName.DATASRC = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("datasrc"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.DATAFLD = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("datafld"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.DECLARE = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("declare"), AttributeName.ALL_NO_PREFIX, 111);
			AttributeName.DISPLAY = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("display"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.DIVISOR = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("divisor"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.DEFAULT = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("default"), AttributeName.ALL_NO_PREFIX, 111);
			AttributeName.DESCENT = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("descent"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.KERNING = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("kerning"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.HANGING = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("hanging"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.HEADERS = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("headers"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.ONPASTE = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("onpaste"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.ONCLICK = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("onclick"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.OPTIMUM = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("optimum"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.ONBEGIN = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("onbegin"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.ONKEYUP = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("onkeyup"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.ONFOCUS = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("onfocus"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.ONERROR = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("onerror"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.ONINPUT = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("oninput"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.ONABORT = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("onabort"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.ONSTART = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("onstart"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.ONRESET = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("onreset"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.OPACITY = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("opacity"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.NOSHADE = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("noshade"), AttributeName.ALL_NO_PREFIX, 111);
			AttributeName.MINSIZE = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("minsize"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.MAXSIZE = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("maxsize"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.LARGEOP = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("largeop"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.UNICODE = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("unicode"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.TARGETX = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SVG_DIFFERENT("targetx", "targetX"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.TARGETY = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SVG_DIFFERENT("targety", "targetY"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.VIEWBOX = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SVG_DIFFERENT("viewbox", "viewBox"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.VERSION = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("version"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.PATTERN = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("pattern"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.PROFILE = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("profile"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.SPACING = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("spacing"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.RESTART = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("restart"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.ROWSPAN = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("rowspan"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.SANDBOX = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("sandbox"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.SUMMARY = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("summary"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.STANDBY = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("standby"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.REPLACE = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("replace"), AttributeName.ALL_NO_PREFIX, 47);
			AttributeName.AUTOPLAY = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("autoplay"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.ADDITIVE = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("additive"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.CALCMODE = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SVG_DIFFERENT("calcmode", "calcMode"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.CODETYPE = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("codetype"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.CODEBASE = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("codebase"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.CONTROLS = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("controls"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.BEVELLED = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("bevelled"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.BASELINE = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("baseline"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.EXPONENT = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("exponent"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.EDGEMODE = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SVG_DIFFERENT("edgemode", "edgeMode"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.ENCODING = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("encoding"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.GLYPHREF = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SVG_DIFFERENT("glyphref", "glyphRef"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.DATETIME = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("datetime"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.DISABLED = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("disabled"), AttributeName.ALL_NO_PREFIX, 111);
			AttributeName.FONTSIZE = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("fontsize"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.KEYTIMES = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SVG_DIFFERENT("keytimes", "keyTimes"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.PANOSE_1 = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("panose-1"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.HREFLANG = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("hreflang"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.ONRESIZE = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("onresize"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.ONCHANGE = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("onchange"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.ONBOUNCE = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("onbounce"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.ONUNLOAD = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("onunload"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.ONFINISH = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("onfinish"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.ONSCROLL = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("onscroll"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.OPERATOR = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("operator"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.OVERFLOW = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("overflow"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.ONSUBMIT = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("onsubmit"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.ONREPEAT = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("onrepeat"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.ONSELECT = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("onselect"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.NOTATION = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("notation"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.NORESIZE = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("noresize"), AttributeName.ALL_NO_PREFIX, 111);
			AttributeName.MANIFEST = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("manifest"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.MATHSIZE = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("mathsize"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.MULTIPLE = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("multiple"), AttributeName.ALL_NO_PREFIX, 111);
			AttributeName.LONGDESC = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("longdesc"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.LANGUAGE = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("language"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.TEMPLATE = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("template"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.TABINDEX = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("tabindex"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.READONLY = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("readonly"), AttributeName.ALL_NO_PREFIX, 111);
			AttributeName.SELECTED = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("selected"), AttributeName.ALL_NO_PREFIX, 111);
			AttributeName.ROWLINES = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("rowlines"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.SEAMLESS = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("seamless"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.ROWALIGN = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("rowalign"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.STRETCHY = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("stretchy"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.REQUIRED = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("required"), AttributeName.ALL_NO_PREFIX, 111);
			AttributeName.XML_BASE = new AttributeName(AttributeName.XML_NS, AttributeName.COLONIFIED_LOCAL("xml:base", "base"), AttributeName.XML_PREFIX, 6);
			AttributeName.XML_LANG = new AttributeName(AttributeName.XML_NS, AttributeName.COLONIFIED_LOCAL("xml:lang", "lang"), AttributeName.XML_PREFIX, 6);
			AttributeName.X_HEIGHT = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("x-height"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.ARIA_OWNS = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("aria-owns"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.AUTOFOCUS = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("autofocus"), AttributeName.ALL_NO_PREFIX, 111);
			AttributeName.ARIA_SORT = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("aria-sort"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.ACCESSKEY = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("accesskey"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.ARIA_BUSY = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("aria-busy"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.ARIA_GRAB = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("aria-grab"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.AMPLITUDE = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("amplitude"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.ARIA_LIVE = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("aria-live"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.CLIP_RULE = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("clip-rule"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.CLIP_PATH = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("clip-path"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.EQUALROWS = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("equalrows"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.ELEVATION = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("elevation"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.DIRECTION = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("direction"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.DRAGGABLE = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("draggable"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.FILTERRES = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SVG_DIFFERENT("filterres", "filterRes"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.FILL_RULE = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("fill-rule"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.FONTSTYLE = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("fontstyle"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.FONT_SIZE = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("font-size"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.KEYPOINTS = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SVG_DIFFERENT("keypoints", "keyPoints"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.HIDEFOCUS = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("hidefocus"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.ONMESSAGE = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("onmessage"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.INTERCEPT = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("intercept"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.ONDRAGEND = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("ondragend"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.ONMOVEEND = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("onmoveend"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.ONINVALID = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("oninvalid"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.ONKEYDOWN = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("onkeydown"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.ONFOCUSIN = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("onfocusin"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.ONMOUSEUP = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("onmouseup"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.INPUTMODE = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("inputmode"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.ONROWEXIT = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("onrowexit"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.MATHCOLOR = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("mathcolor"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.MASKUNITS = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SVG_DIFFERENT("maskunits", "maskUnits"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.MAXLENGTH = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("maxlength"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.LINEBREAK = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("linebreak"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.TRANSFORM = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("transform"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.V_HANGING = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("v-hanging"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.VALUETYPE = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("valuetype"), AttributeName.ALL_NO_PREFIX, 47);
			AttributeName.POINTSATZ = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SVG_DIFFERENT("pointsatz", "pointsAtZ"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.POINTSATX = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SVG_DIFFERENT("pointsatx", "pointsAtX"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.POINTSATY = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SVG_DIFFERENT("pointsaty", "pointsAtY"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.SYMMETRIC = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("symmetric"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.SCROLLING = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("scrolling"), AttributeName.ALL_NO_PREFIX, 47);
			AttributeName.REPEATDUR = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SVG_DIFFERENT("repeatdur", "repeatDur"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.SELECTION = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("selection"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.SEPARATOR = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("separator"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.XML_SPACE = new AttributeName(AttributeName.XML_NS, AttributeName.COLONIFIED_LOCAL("xml:space", "space"), AttributeName.XML_PREFIX, 6);
			AttributeName.AUTOSUBMIT = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("autosubmit"), AttributeName.ALL_NO_PREFIX, 111);
			AttributeName.ALPHABETIC = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("alphabetic"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.ACTIONTYPE = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("actiontype"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.ACCUMULATE = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("accumulate"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.ARIA_LEVEL = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("aria-level"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.COLUMNSPAN = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("columnspan"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.CAP_HEIGHT = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("cap-height"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.BACKGROUND = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("background"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.GLYPH_NAME = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("glyph-name"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.GROUPALIGN = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("groupalign"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.FONTFAMILY = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("fontfamily"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.FONTWEIGHT = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("fontweight"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.FONT_STYLE = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("font-style"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.KEYSPLINES = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SVG_DIFFERENT("keysplines", "keySplines"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.HTTP_EQUIV = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("http-equiv"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.ONACTIVATE = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("onactivate"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.OCCURRENCE = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("occurrence"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.IRRELEVANT = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("irrelevant"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.ONDBLCLICK = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("ondblclick"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.ONDRAGDROP = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("ondragdrop"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.ONKEYPRESS = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("onkeypress"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.ONROWENTER = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("onrowenter"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.ONDRAGOVER = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("ondragover"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.ONFOCUSOUT = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("onfocusout"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.ONMOUSEOUT = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("onmouseout"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.NUMOCTAVES = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SVG_DIFFERENT("numoctaves", "numOctaves"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.MARKER_MID = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("marker-mid"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.MARKER_END = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("marker-end"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.TEXTLENGTH = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SVG_DIFFERENT("textlength", "textLength"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.VISIBILITY = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("visibility"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.VIEWTARGET = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SVG_DIFFERENT("viewtarget", "viewTarget"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.VERT_ADV_Y = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("vert-adv-y"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.PATHLENGTH = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SVG_DIFFERENT("pathlength", "pathLength"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.REPEAT_MAX = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("repeat-max"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.RADIOGROUP = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("radiogroup"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.STOP_COLOR = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("stop-color"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.SEPARATORS = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("separators"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.REPEAT_MIN = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("repeat-min"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.ROWSPACING = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("rowspacing"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.ZOOMANDPAN = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SVG_DIFFERENT("zoomandpan", "zoomAndPan"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.XLINK_TYPE = new AttributeName(AttributeName.XLINK_NS, AttributeName.COLONIFIED_LOCAL("xlink:type", "type"), AttributeName.XLINK_PREFIX, 6);
			AttributeName.XLINK_ROLE = new AttributeName(AttributeName.XLINK_NS, AttributeName.COLONIFIED_LOCAL("xlink:role", "role"), AttributeName.XLINK_PREFIX, 6);
			AttributeName.XLINK_HREF = new AttributeName(AttributeName.XLINK_NS, AttributeName.COLONIFIED_LOCAL("xlink:href", "href"), AttributeName.XLINK_PREFIX, 6);
			AttributeName.XLINK_SHOW = new AttributeName(AttributeName.XLINK_NS, AttributeName.COLONIFIED_LOCAL("xlink:show", "show"), AttributeName.XLINK_PREFIX, 6);
			AttributeName.ACCENTUNDER = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("accentunder"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.ARIA_SECRET = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("aria-secret"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.ARIA_ATOMIC = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("aria-atomic"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.ARIA_HIDDEN = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("aria-hidden"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.ARIA_FLOWTO = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("aria-flowto"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.ARABIC_FORM = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("arabic-form"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.CELLPADDING = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("cellpadding"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.CELLSPACING = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("cellspacing"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.COLUMNWIDTH = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("columnwidth"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.CROSSORIGIN = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("crossorigin"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.COLUMNALIGN = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("columnalign"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.COLUMNLINES = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("columnlines"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.CONTEXTMENU = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("contextmenu"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.BASEPROFILE = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SVG_DIFFERENT("baseprofile", "baseProfile"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.FONT_FAMILY = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("font-family"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.FRAMEBORDER = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("frameborder"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.FILTERUNITS = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SVG_DIFFERENT("filterunits", "filterUnits"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.FLOOD_COLOR = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("flood-color"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.FONT_WEIGHT = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("font-weight"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.HORIZ_ADV_X = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("horiz-adv-x"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.ONDRAGLEAVE = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("ondragleave"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.ONMOUSEMOVE = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("onmousemove"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.ORIENTATION = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("orientation"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.ONMOUSEDOWN = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("onmousedown"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.ONMOUSEOVER = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("onmouseover"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.ONDRAGENTER = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("ondragenter"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.IDEOGRAPHIC = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("ideographic"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.ONBEFORECUT = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("onbeforecut"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.ONFORMINPUT = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("onforminput"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.ONDRAGSTART = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("ondragstart"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.ONMOVESTART = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("onmovestart"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.MARKERUNITS = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SVG_DIFFERENT("markerunits", "markerUnits"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.MATHVARIANT = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("mathvariant"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.MARGINWIDTH = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("marginwidth"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.MARKERWIDTH = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SVG_DIFFERENT("markerwidth", "markerWidth"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.TEXT_ANCHOR = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("text-anchor"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.TABLEVALUES = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SVG_DIFFERENT("tablevalues", "tableValues"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.SCRIPTLEVEL = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("scriptlevel"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.REPEATCOUNT = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SVG_DIFFERENT("repeatcount", "repeatCount"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.STITCHTILES = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SVG_DIFFERENT("stitchtiles", "stitchTiles"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.STARTOFFSET = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SVG_DIFFERENT("startoffset", "startOffset"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.SCROLLDELAY = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("scrolldelay"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.XMLNS_XLINK = new AttributeName(AttributeName.XMLNS_NS, AttributeName.COLONIFIED_LOCAL("xmlns:xlink", "xlink"), AttributeName.XMLNS_PREFIX, 16);
			AttributeName.XLINK_TITLE = new AttributeName(AttributeName.XLINK_NS, AttributeName.COLONIFIED_LOCAL("xlink:title", "title"), AttributeName.XLINK_PREFIX, 6);
			AttributeName.ARIA_INVALID = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("aria-invalid"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.ARIA_PRESSED = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("aria-pressed"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.ARIA_CHECKED = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("aria-checked"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.AUTOCOMPLETE = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("autocomplete"), AttributeName.ALL_NO_PREFIX, 47);
			AttributeName.ARIA_SETSIZE = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("aria-setsize"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.ARIA_CHANNEL = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("aria-channel"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.EQUALCOLUMNS = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("equalcolumns"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.DISPLAYSTYLE = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("displaystyle"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.DATAFORMATAS = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("dataformatas"), AttributeName.ALL_NO_PREFIX, 47);
			AttributeName.FILL_OPACITY = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("fill-opacity"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.FONT_VARIANT = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("font-variant"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.FONT_STRETCH = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("font-stretch"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.FRAMESPACING = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("framespacing"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.KERNELMATRIX = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SVG_DIFFERENT("kernelmatrix", "kernelMatrix"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.ONDEACTIVATE = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("ondeactivate"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.ONROWSDELETE = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("onrowsdelete"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.ONMOUSELEAVE = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("onmouseleave"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.ONFORMCHANGE = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("onformchange"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.ONCELLCHANGE = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("oncellchange"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.ONMOUSEWHEEL = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("onmousewheel"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.ONMOUSEENTER = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("onmouseenter"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.ONAFTERPRINT = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("onafterprint"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.ONBEFORECOPY = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("onbeforecopy"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.MARGINHEIGHT = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("marginheight"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.MARKERHEIGHT = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SVG_DIFFERENT("markerheight", "markerHeight"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.MARKER_START = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("marker-start"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.MATHEMATICAL = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("mathematical"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.LENGTHADJUST = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SVG_DIFFERENT("lengthadjust", "lengthAdjust"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.UNSELECTABLE = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("unselectable"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.UNICODE_BIDI = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("unicode-bidi"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.UNITS_PER_EM = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("units-per-em"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.WORD_SPACING = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("word-spacing"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.WRITING_MODE = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("writing-mode"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.V_ALPHABETIC = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("v-alphabetic"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.PATTERNUNITS = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SVG_DIFFERENT("patternunits", "patternUnits"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.SPREADMETHOD = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SVG_DIFFERENT("spreadmethod", "spreadMethod"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.SURFACESCALE = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SVG_DIFFERENT("surfacescale", "surfaceScale"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.STROKE_WIDTH = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("stroke-width"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.REPEAT_START = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("repeat-start"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.STDDEVIATION = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SVG_DIFFERENT("stddeviation", "stdDeviation"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.STOP_OPACITY = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("stop-opacity"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.ARIA_CONTROLS = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("aria-controls"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.ARIA_HASPOPUP = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("aria-haspopup"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.ACCENT_HEIGHT = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("accent-height"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.ARIA_VALUENOW = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("aria-valuenow"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.ARIA_RELEVANT = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("aria-relevant"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.ARIA_POSINSET = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("aria-posinset"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.ARIA_VALUEMAX = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("aria-valuemax"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.ARIA_READONLY = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("aria-readonly"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.ARIA_SELECTED = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("aria-selected"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.ARIA_REQUIRED = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("aria-required"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.ARIA_EXPANDED = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("aria-expanded"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.ARIA_DISABLED = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("aria-disabled"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.ATTRIBUTETYPE = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SVG_DIFFERENT("attributetype", "attributeType"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.ATTRIBUTENAME = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SVG_DIFFERENT("attributename", "attributeName"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.ARIA_DATATYPE = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("aria-datatype"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.ARIA_VALUEMIN = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("aria-valuemin"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.BASEFREQUENCY = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SVG_DIFFERENT("basefrequency", "baseFrequency"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.COLUMNSPACING = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("columnspacing"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.COLOR_PROFILE = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("color-profile"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.CLIPPATHUNITS = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SVG_DIFFERENT("clippathunits", "clipPathUnits"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.DEFINITIONURL = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.MATH_DIFFERENT("definitionurl", "definitionURL"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.GRADIENTUNITS = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SVG_DIFFERENT("gradientunits", "gradientUnits"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.FLOOD_OPACITY = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("flood-opacity"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.ONAFTERUPDATE = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("onafterupdate"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.ONERRORUPDATE = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("onerrorupdate"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.ONBEFOREPASTE = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("onbeforepaste"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.ONLOSECAPTURE = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("onlosecapture"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.ONCONTEXTMENU = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("oncontextmenu"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.ONSELECTSTART = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("onselectstart"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.ONBEFOREPRINT = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("onbeforeprint"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.MOVABLELIMITS = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("movablelimits"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.LINETHICKNESS = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("linethickness"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.UNICODE_RANGE = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("unicode-range"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.THINMATHSPACE = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("thinmathspace"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.VERT_ORIGIN_X = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("vert-origin-x"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.VERT_ORIGIN_Y = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("vert-origin-y"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.V_IDEOGRAPHIC = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("v-ideographic"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.PRESERVEALPHA = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SVG_DIFFERENT("preservealpha", "preserveAlpha"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.SCRIPTMINSIZE = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("scriptminsize"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.SPECIFICATION = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("specification"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.XLINK_ACTUATE = new AttributeName(AttributeName.XLINK_NS, AttributeName.COLONIFIED_LOCAL("xlink:actuate", "actuate"), AttributeName.XLINK_PREFIX, 6);
			AttributeName.XLINK_ARCROLE = new AttributeName(AttributeName.XLINK_NS, AttributeName.COLONIFIED_LOCAL("xlink:arcrole", "arcrole"), AttributeName.XLINK_PREFIX, 6);
			AttributeName.ACCEPT_CHARSET = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("accept-charset"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.ALIGNMENTSCOPE = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("alignmentscope"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.ARIA_MULTILINE = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("aria-multiline"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.BASELINE_SHIFT = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("baseline-shift"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.HORIZ_ORIGIN_X = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("horiz-origin-x"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.HORIZ_ORIGIN_Y = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("horiz-origin-y"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.ONBEFOREUPDATE = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("onbeforeupdate"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.ONFILTERCHANGE = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("onfilterchange"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.ONROWSINSERTED = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("onrowsinserted"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.ONBEFOREUNLOAD = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("onbeforeunload"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.MATHBACKGROUND = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("mathbackground"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.LETTER_SPACING = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("letter-spacing"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.LIGHTING_COLOR = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("lighting-color"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.THICKMATHSPACE = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("thickmathspace"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.TEXT_RENDERING = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("text-rendering"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.V_MATHEMATICAL = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("v-mathematical"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.POINTER_EVENTS = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("pointer-events"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.PRIMITIVEUNITS = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SVG_DIFFERENT("primitiveunits", "primitiveUnits"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.SYSTEMLANGUAGE = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SVG_DIFFERENT("systemlanguage", "systemLanguage"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.STROKE_LINECAP = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("stroke-linecap"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.SUBSCRIPTSHIFT = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("subscriptshift"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.STROKE_OPACITY = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("stroke-opacity"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.ARIA_DROPEFFECT = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("aria-dropeffect"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.ARIA_LABELLEDBY = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("aria-labelledby"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.ARIA_TEMPLATEID = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("aria-templateid"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.COLOR_RENDERING = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("color-rendering"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.CONTENTEDITABLE = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("contenteditable"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.DIFFUSECONSTANT = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SVG_DIFFERENT("diffuseconstant", "diffuseConstant"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.ONDATAAVAILABLE = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("ondataavailable"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.ONCONTROLSELECT = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("oncontrolselect"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.IMAGE_RENDERING = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("image-rendering"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.MEDIUMMATHSPACE = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("mediummathspace"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.TEXT_DECORATION = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("text-decoration"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.SHAPE_RENDERING = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("shape-rendering"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.STROKE_LINEJOIN = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("stroke-linejoin"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.REPEAT_TEMPLATE = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("repeat-template"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.ARIA_DESCRIBEDBY = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("aria-describedby"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.CONTENTSTYLETYPE = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SVG_DIFFERENT("contentstyletype", "contentStyleType"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.FONT_SIZE_ADJUST = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("font-size-adjust"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.KERNELUNITLENGTH = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SVG_DIFFERENT("kernelunitlength", "kernelUnitLength"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.ONBEFOREACTIVATE = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("onbeforeactivate"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.ONPROPERTYCHANGE = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("onpropertychange"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.ONDATASETCHANGED = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("ondatasetchanged"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.MASKCONTENTUNITS = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SVG_DIFFERENT("maskcontentunits", "maskContentUnits"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.PATTERNTRANSFORM = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SVG_DIFFERENT("patterntransform", "patternTransform"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.REQUIREDFEATURES = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SVG_DIFFERENT("requiredfeatures", "requiredFeatures"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.RENDERING_INTENT = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("rendering-intent"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.SPECULAREXPONENT = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SVG_DIFFERENT("specularexponent", "specularExponent"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.SPECULARCONSTANT = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SVG_DIFFERENT("specularconstant", "specularConstant"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.SUPERSCRIPTSHIFT = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("superscriptshift"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.STROKE_DASHARRAY = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("stroke-dasharray"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.XCHANNELSELECTOR = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SVG_DIFFERENT("xchannelselector", "xChannelSelector"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.YCHANNELSELECTOR = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SVG_DIFFERENT("ychannelselector", "yChannelSelector"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.ARIA_AUTOCOMPLETE = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("aria-autocomplete"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.CONTENTSCRIPTTYPE = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SVG_DIFFERENT("contentscripttype", "contentScriptType"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.ENABLE_BACKGROUND = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("enable-background"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.DOMINANT_BASELINE = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("dominant-baseline"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.GRADIENTTRANSFORM = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SVG_DIFFERENT("gradienttransform", "gradientTransform"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.ONBEFORDEACTIVATE = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("onbefordeactivate"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.ONDATASETCOMPLETE = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("ondatasetcomplete"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.OVERLINE_POSITION = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("overline-position"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.ONBEFOREEDITFOCUS = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("onbeforeeditfocus"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.LIMITINGCONEANGLE = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SVG_DIFFERENT("limitingconeangle", "limitingConeAngle"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.VERYTHINMATHSPACE = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("verythinmathspace"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.STROKE_DASHOFFSET = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("stroke-dashoffset"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.STROKE_MITERLIMIT = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("stroke-miterlimit"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.ALIGNMENT_BASELINE = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("alignment-baseline"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.ONREADYSTATECHANGE = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("onreadystatechange"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.OVERLINE_THICKNESS = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("overline-thickness"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.UNDERLINE_POSITION = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("underline-position"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.VERYTHICKMATHSPACE = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("verythickmathspace"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.REQUIREDEXTENSIONS = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SVG_DIFFERENT("requiredextensions", "requiredExtensions"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.COLOR_INTERPOLATION = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("color-interpolation"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.UNDERLINE_THICKNESS = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("underline-thickness"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.PRESERVEASPECTRATIO = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SVG_DIFFERENT("preserveaspectratio", "preserveAspectRatio"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.PATTERNCONTENTUNITS = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SVG_DIFFERENT("patterncontentunits", "patternContentUnits"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.ARIA_MULTISELECTABLE = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("aria-multiselectable"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.SCRIPTSIZEMULTIPLIER = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("scriptsizemultiplier"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.ARIA_ACTIVEDESCENDANT = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("aria-activedescendant"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.VERYVERYTHINMATHSPACE = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("veryverythinmathspace"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.VERYVERYTHICKMATHSPACE = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("veryverythickmathspace"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.STRIKETHROUGH_POSITION = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("strikethrough-position"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.STRIKETHROUGH_THICKNESS = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("strikethrough-thickness"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.EXTERNALRESOURCESREQUIRED = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SVG_DIFFERENT("externalresourcesrequired", "externalResourcesRequired"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.GLYPH_ORIENTATION_VERTICAL = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("glyph-orientation-vertical"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.COLOR_INTERPOLATION_FILTERS = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("color-interpolation-filters"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.GLYPH_ORIENTATION_HORIZONTAL = new AttributeName(AttributeName.ALL_NO_NS, AttributeName.SAME_LOCAL("glyph-orientation-horizontal"), AttributeName.ALL_NO_PREFIX, 15);
			AttributeName.ATTRIBUTE_NAMES = new AttributeName[]
			{
				AttributeName.D,
				AttributeName.K,
				AttributeName.R,
				AttributeName.X,
				AttributeName.Y,
				AttributeName.Z,
				AttributeName.BY,
				AttributeName.CX,
				AttributeName.CY,
				AttributeName.DX,
				AttributeName.DY,
				AttributeName.G2,
				AttributeName.G1,
				AttributeName.FX,
				AttributeName.FY,
				AttributeName.K4,
				AttributeName.K2,
				AttributeName.K3,
				AttributeName.K1,
				AttributeName.ID,
				AttributeName.IN,
				AttributeName.U2,
				AttributeName.U1,
				AttributeName.RT,
				AttributeName.RX,
				AttributeName.RY,
				AttributeName.TO,
				AttributeName.Y2,
				AttributeName.Y1,
				AttributeName.X1,
				AttributeName.X2,
				AttributeName.ALT,
				AttributeName.DIR,
				AttributeName.DUR,
				AttributeName.END,
				AttributeName.FOR,
				AttributeName.IN2,
				AttributeName.MAX,
				AttributeName.MIN,
				AttributeName.LOW,
				AttributeName.REL,
				AttributeName.REV,
				AttributeName.SRC,
				AttributeName.AXIS,
				AttributeName.ABBR,
				AttributeName.BBOX,
				AttributeName.CITE,
				AttributeName.CODE,
				AttributeName.BIAS,
				AttributeName.COLS,
				AttributeName.CLIP,
				AttributeName.CHAR,
				AttributeName.BASE,
				AttributeName.EDGE,
				AttributeName.DATA,
				AttributeName.FILL,
				AttributeName.FROM,
				AttributeName.FORM,
				AttributeName.FACE,
				AttributeName.HIGH,
				AttributeName.HREF,
				AttributeName.OPEN,
				AttributeName.ICON,
				AttributeName.NAME,
				AttributeName.MODE,
				AttributeName.MASK,
				AttributeName.LINK,
				AttributeName.LANG,
				AttributeName.LIST,
				AttributeName.TYPE,
				AttributeName.WHEN,
				AttributeName.WRAP,
				AttributeName.TEXT,
				AttributeName.PATH,
				AttributeName.PING,
				AttributeName.REFX,
				AttributeName.REFY,
				AttributeName.SIZE,
				AttributeName.SEED,
				AttributeName.ROWS,
				AttributeName.SPAN,
				AttributeName.STEP,
				AttributeName.ROLE,
				AttributeName.XREF,
				AttributeName.ASYNC,
				AttributeName.ALINK,
				AttributeName.ALIGN,
				AttributeName.CLOSE,
				AttributeName.COLOR,
				AttributeName.CLASS,
				AttributeName.CLEAR,
				AttributeName.BEGIN,
				AttributeName.DEPTH,
				AttributeName.DEFER,
				AttributeName.FENCE,
				AttributeName.FRAME,
				AttributeName.ISMAP,
				AttributeName.ONEND,
				AttributeName.INDEX,
				AttributeName.ORDER,
				AttributeName.OTHER,
				AttributeName.ONCUT,
				AttributeName.NARGS,
				AttributeName.MEDIA,
				AttributeName.LABEL,
				AttributeName.LOCAL,
				AttributeName.WIDTH,
				AttributeName.TITLE,
				AttributeName.VLINK,
				AttributeName.VALUE,
				AttributeName.SLOPE,
				AttributeName.SHAPE,
				AttributeName.SCOPE,
				AttributeName.SCALE,
				AttributeName.SPEED,
				AttributeName.STYLE,
				AttributeName.RULES,
				AttributeName.STEMH,
				AttributeName.STEMV,
				AttributeName.START,
				AttributeName.XMLNS,
				AttributeName.ACCEPT,
				AttributeName.ACCENT,
				AttributeName.ASCENT,
				AttributeName.ACTIVE,
				AttributeName.ALTIMG,
				AttributeName.ACTION,
				AttributeName.BORDER,
				AttributeName.CURSOR,
				AttributeName.COORDS,
				AttributeName.FILTER,
				AttributeName.FORMAT,
				AttributeName.HIDDEN,
				AttributeName.HSPACE,
				AttributeName.HEIGHT,
				AttributeName.ONMOVE,
				AttributeName.ONLOAD,
				AttributeName.ONDRAG,
				AttributeName.ORIGIN,
				AttributeName.ONZOOM,
				AttributeName.ONHELP,
				AttributeName.ONSTOP,
				AttributeName.ONDROP,
				AttributeName.ONBLUR,
				AttributeName.OBJECT,
				AttributeName.OFFSET,
				AttributeName.ORIENT,
				AttributeName.ONCOPY,
				AttributeName.NOWRAP,
				AttributeName.NOHREF,
				AttributeName.MACROS,
				AttributeName.METHOD,
				AttributeName.LOWSRC,
				AttributeName.LSPACE,
				AttributeName.LQUOTE,
				AttributeName.USEMAP,
				AttributeName.WIDTHS,
				AttributeName.TARGET,
				AttributeName.VALUES,
				AttributeName.VALIGN,
				AttributeName.VSPACE,
				AttributeName.POSTER,
				AttributeName.POINTS,
				AttributeName.PROMPT,
				AttributeName.SCOPED,
				AttributeName.STRING,
				AttributeName.SCHEME,
				AttributeName.STROKE,
				AttributeName.RADIUS,
				AttributeName.RESULT,
				AttributeName.REPEAT,
				AttributeName.RSPACE,
				AttributeName.ROTATE,
				AttributeName.RQUOTE,
				AttributeName.ALTTEXT,
				AttributeName.ARCHIVE,
				AttributeName.AZIMUTH,
				AttributeName.CLOSURE,
				AttributeName.CHECKED,
				AttributeName.CLASSID,
				AttributeName.CHAROFF,
				AttributeName.BGCOLOR,
				AttributeName.COLSPAN,
				AttributeName.CHARSET,
				AttributeName.COMPACT,
				AttributeName.CONTENT,
				AttributeName.ENCTYPE,
				AttributeName.DATASRC,
				AttributeName.DATAFLD,
				AttributeName.DECLARE,
				AttributeName.DISPLAY,
				AttributeName.DIVISOR,
				AttributeName.DEFAULT,
				AttributeName.DESCENT,
				AttributeName.KERNING,
				AttributeName.HANGING,
				AttributeName.HEADERS,
				AttributeName.ONPASTE,
				AttributeName.ONCLICK,
				AttributeName.OPTIMUM,
				AttributeName.ONBEGIN,
				AttributeName.ONKEYUP,
				AttributeName.ONFOCUS,
				AttributeName.ONERROR,
				AttributeName.ONINPUT,
				AttributeName.ONABORT,
				AttributeName.ONSTART,
				AttributeName.ONRESET,
				AttributeName.OPACITY,
				AttributeName.NOSHADE,
				AttributeName.MINSIZE,
				AttributeName.MAXSIZE,
				AttributeName.LARGEOP,
				AttributeName.UNICODE,
				AttributeName.TARGETX,
				AttributeName.TARGETY,
				AttributeName.VIEWBOX,
				AttributeName.VERSION,
				AttributeName.PATTERN,
				AttributeName.PROFILE,
				AttributeName.SPACING,
				AttributeName.RESTART,
				AttributeName.ROWSPAN,
				AttributeName.SANDBOX,
				AttributeName.SUMMARY,
				AttributeName.STANDBY,
				AttributeName.REPLACE,
				AttributeName.AUTOPLAY,
				AttributeName.ADDITIVE,
				AttributeName.CALCMODE,
				AttributeName.CODETYPE,
				AttributeName.CODEBASE,
				AttributeName.CONTROLS,
				AttributeName.BEVELLED,
				AttributeName.BASELINE,
				AttributeName.EXPONENT,
				AttributeName.EDGEMODE,
				AttributeName.ENCODING,
				AttributeName.GLYPHREF,
				AttributeName.DATETIME,
				AttributeName.DISABLED,
				AttributeName.FONTSIZE,
				AttributeName.KEYTIMES,
				AttributeName.PANOSE_1,
				AttributeName.HREFLANG,
				AttributeName.ONRESIZE,
				AttributeName.ONCHANGE,
				AttributeName.ONBOUNCE,
				AttributeName.ONUNLOAD,
				AttributeName.ONFINISH,
				AttributeName.ONSCROLL,
				AttributeName.OPERATOR,
				AttributeName.OVERFLOW,
				AttributeName.ONSUBMIT,
				AttributeName.ONREPEAT,
				AttributeName.ONSELECT,
				AttributeName.NOTATION,
				AttributeName.NORESIZE,
				AttributeName.MANIFEST,
				AttributeName.MATHSIZE,
				AttributeName.MULTIPLE,
				AttributeName.LONGDESC,
				AttributeName.LANGUAGE,
				AttributeName.TEMPLATE,
				AttributeName.TABINDEX,
				AttributeName.READONLY,
				AttributeName.SELECTED,
				AttributeName.ROWLINES,
				AttributeName.SEAMLESS,
				AttributeName.ROWALIGN,
				AttributeName.STRETCHY,
				AttributeName.REQUIRED,
				AttributeName.XML_BASE,
				AttributeName.XML_LANG,
				AttributeName.X_HEIGHT,
				AttributeName.ARIA_OWNS,
				AttributeName.AUTOFOCUS,
				AttributeName.ARIA_SORT,
				AttributeName.ACCESSKEY,
				AttributeName.ARIA_BUSY,
				AttributeName.ARIA_GRAB,
				AttributeName.AMPLITUDE,
				AttributeName.ARIA_LIVE,
				AttributeName.CLIP_RULE,
				AttributeName.CLIP_PATH,
				AttributeName.EQUALROWS,
				AttributeName.ELEVATION,
				AttributeName.DIRECTION,
				AttributeName.DRAGGABLE,
				AttributeName.FILTERRES,
				AttributeName.FILL_RULE,
				AttributeName.FONTSTYLE,
				AttributeName.FONT_SIZE,
				AttributeName.KEYPOINTS,
				AttributeName.HIDEFOCUS,
				AttributeName.ONMESSAGE,
				AttributeName.INTERCEPT,
				AttributeName.ONDRAGEND,
				AttributeName.ONMOVEEND,
				AttributeName.ONINVALID,
				AttributeName.ONKEYDOWN,
				AttributeName.ONFOCUSIN,
				AttributeName.ONMOUSEUP,
				AttributeName.INPUTMODE,
				AttributeName.ONROWEXIT,
				AttributeName.MATHCOLOR,
				AttributeName.MASKUNITS,
				AttributeName.MAXLENGTH,
				AttributeName.LINEBREAK,
				AttributeName.TRANSFORM,
				AttributeName.V_HANGING,
				AttributeName.VALUETYPE,
				AttributeName.POINTSATZ,
				AttributeName.POINTSATX,
				AttributeName.POINTSATY,
				AttributeName.SYMMETRIC,
				AttributeName.SCROLLING,
				AttributeName.REPEATDUR,
				AttributeName.SELECTION,
				AttributeName.SEPARATOR,
				AttributeName.XML_SPACE,
				AttributeName.AUTOSUBMIT,
				AttributeName.ALPHABETIC,
				AttributeName.ACTIONTYPE,
				AttributeName.ACCUMULATE,
				AttributeName.ARIA_LEVEL,
				AttributeName.COLUMNSPAN,
				AttributeName.CAP_HEIGHT,
				AttributeName.BACKGROUND,
				AttributeName.GLYPH_NAME,
				AttributeName.GROUPALIGN,
				AttributeName.FONTFAMILY,
				AttributeName.FONTWEIGHT,
				AttributeName.FONT_STYLE,
				AttributeName.KEYSPLINES,
				AttributeName.HTTP_EQUIV,
				AttributeName.ONACTIVATE,
				AttributeName.OCCURRENCE,
				AttributeName.IRRELEVANT,
				AttributeName.ONDBLCLICK,
				AttributeName.ONDRAGDROP,
				AttributeName.ONKEYPRESS,
				AttributeName.ONROWENTER,
				AttributeName.ONDRAGOVER,
				AttributeName.ONFOCUSOUT,
				AttributeName.ONMOUSEOUT,
				AttributeName.NUMOCTAVES,
				AttributeName.MARKER_MID,
				AttributeName.MARKER_END,
				AttributeName.TEXTLENGTH,
				AttributeName.VISIBILITY,
				AttributeName.VIEWTARGET,
				AttributeName.VERT_ADV_Y,
				AttributeName.PATHLENGTH,
				AttributeName.REPEAT_MAX,
				AttributeName.RADIOGROUP,
				AttributeName.STOP_COLOR,
				AttributeName.SEPARATORS,
				AttributeName.REPEAT_MIN,
				AttributeName.ROWSPACING,
				AttributeName.ZOOMANDPAN,
				AttributeName.XLINK_TYPE,
				AttributeName.XLINK_ROLE,
				AttributeName.XLINK_HREF,
				AttributeName.XLINK_SHOW,
				AttributeName.ACCENTUNDER,
				AttributeName.ARIA_SECRET,
				AttributeName.ARIA_ATOMIC,
				AttributeName.ARIA_HIDDEN,
				AttributeName.ARIA_FLOWTO,
				AttributeName.ARABIC_FORM,
				AttributeName.CELLPADDING,
				AttributeName.CELLSPACING,
				AttributeName.COLUMNWIDTH,
				AttributeName.CROSSORIGIN,
				AttributeName.COLUMNALIGN,
				AttributeName.COLUMNLINES,
				AttributeName.CONTEXTMENU,
				AttributeName.BASEPROFILE,
				AttributeName.FONT_FAMILY,
				AttributeName.FRAMEBORDER,
				AttributeName.FILTERUNITS,
				AttributeName.FLOOD_COLOR,
				AttributeName.FONT_WEIGHT,
				AttributeName.HORIZ_ADV_X,
				AttributeName.ONDRAGLEAVE,
				AttributeName.ONMOUSEMOVE,
				AttributeName.ORIENTATION,
				AttributeName.ONMOUSEDOWN,
				AttributeName.ONMOUSEOVER,
				AttributeName.ONDRAGENTER,
				AttributeName.IDEOGRAPHIC,
				AttributeName.ONBEFORECUT,
				AttributeName.ONFORMINPUT,
				AttributeName.ONDRAGSTART,
				AttributeName.ONMOVESTART,
				AttributeName.MARKERUNITS,
				AttributeName.MATHVARIANT,
				AttributeName.MARGINWIDTH,
				AttributeName.MARKERWIDTH,
				AttributeName.TEXT_ANCHOR,
				AttributeName.TABLEVALUES,
				AttributeName.SCRIPTLEVEL,
				AttributeName.REPEATCOUNT,
				AttributeName.STITCHTILES,
				AttributeName.STARTOFFSET,
				AttributeName.SCROLLDELAY,
				AttributeName.XMLNS_XLINK,
				AttributeName.XLINK_TITLE,
				AttributeName.ARIA_INVALID,
				AttributeName.ARIA_PRESSED,
				AttributeName.ARIA_CHECKED,
				AttributeName.AUTOCOMPLETE,
				AttributeName.ARIA_SETSIZE,
				AttributeName.ARIA_CHANNEL,
				AttributeName.EQUALCOLUMNS,
				AttributeName.DISPLAYSTYLE,
				AttributeName.DATAFORMATAS,
				AttributeName.FILL_OPACITY,
				AttributeName.FONT_VARIANT,
				AttributeName.FONT_STRETCH,
				AttributeName.FRAMESPACING,
				AttributeName.KERNELMATRIX,
				AttributeName.ONDEACTIVATE,
				AttributeName.ONROWSDELETE,
				AttributeName.ONMOUSELEAVE,
				AttributeName.ONFORMCHANGE,
				AttributeName.ONCELLCHANGE,
				AttributeName.ONMOUSEWHEEL,
				AttributeName.ONMOUSEENTER,
				AttributeName.ONAFTERPRINT,
				AttributeName.ONBEFORECOPY,
				AttributeName.MARGINHEIGHT,
				AttributeName.MARKERHEIGHT,
				AttributeName.MARKER_START,
				AttributeName.MATHEMATICAL,
				AttributeName.LENGTHADJUST,
				AttributeName.UNSELECTABLE,
				AttributeName.UNICODE_BIDI,
				AttributeName.UNITS_PER_EM,
				AttributeName.WORD_SPACING,
				AttributeName.WRITING_MODE,
				AttributeName.V_ALPHABETIC,
				AttributeName.PATTERNUNITS,
				AttributeName.SPREADMETHOD,
				AttributeName.SURFACESCALE,
				AttributeName.STROKE_WIDTH,
				AttributeName.REPEAT_START,
				AttributeName.STDDEVIATION,
				AttributeName.STOP_OPACITY,
				AttributeName.ARIA_CONTROLS,
				AttributeName.ARIA_HASPOPUP,
				AttributeName.ACCENT_HEIGHT,
				AttributeName.ARIA_VALUENOW,
				AttributeName.ARIA_RELEVANT,
				AttributeName.ARIA_POSINSET,
				AttributeName.ARIA_VALUEMAX,
				AttributeName.ARIA_READONLY,
				AttributeName.ARIA_SELECTED,
				AttributeName.ARIA_REQUIRED,
				AttributeName.ARIA_EXPANDED,
				AttributeName.ARIA_DISABLED,
				AttributeName.ATTRIBUTETYPE,
				AttributeName.ATTRIBUTENAME,
				AttributeName.ARIA_DATATYPE,
				AttributeName.ARIA_VALUEMIN,
				AttributeName.BASEFREQUENCY,
				AttributeName.COLUMNSPACING,
				AttributeName.COLOR_PROFILE,
				AttributeName.CLIPPATHUNITS,
				AttributeName.DEFINITIONURL,
				AttributeName.GRADIENTUNITS,
				AttributeName.FLOOD_OPACITY,
				AttributeName.ONAFTERUPDATE,
				AttributeName.ONERRORUPDATE,
				AttributeName.ONBEFOREPASTE,
				AttributeName.ONLOSECAPTURE,
				AttributeName.ONCONTEXTMENU,
				AttributeName.ONSELECTSTART,
				AttributeName.ONBEFOREPRINT,
				AttributeName.MOVABLELIMITS,
				AttributeName.LINETHICKNESS,
				AttributeName.UNICODE_RANGE,
				AttributeName.THINMATHSPACE,
				AttributeName.VERT_ORIGIN_X,
				AttributeName.VERT_ORIGIN_Y,
				AttributeName.V_IDEOGRAPHIC,
				AttributeName.PRESERVEALPHA,
				AttributeName.SCRIPTMINSIZE,
				AttributeName.SPECIFICATION,
				AttributeName.XLINK_ACTUATE,
				AttributeName.XLINK_ARCROLE,
				AttributeName.ACCEPT_CHARSET,
				AttributeName.ALIGNMENTSCOPE,
				AttributeName.ARIA_MULTILINE,
				AttributeName.BASELINE_SHIFT,
				AttributeName.HORIZ_ORIGIN_X,
				AttributeName.HORIZ_ORIGIN_Y,
				AttributeName.ONBEFOREUPDATE,
				AttributeName.ONFILTERCHANGE,
				AttributeName.ONROWSINSERTED,
				AttributeName.ONBEFOREUNLOAD,
				AttributeName.MATHBACKGROUND,
				AttributeName.LETTER_SPACING,
				AttributeName.LIGHTING_COLOR,
				AttributeName.THICKMATHSPACE,
				AttributeName.TEXT_RENDERING,
				AttributeName.V_MATHEMATICAL,
				AttributeName.POINTER_EVENTS,
				AttributeName.PRIMITIVEUNITS,
				AttributeName.SYSTEMLANGUAGE,
				AttributeName.STROKE_LINECAP,
				AttributeName.SUBSCRIPTSHIFT,
				AttributeName.STROKE_OPACITY,
				AttributeName.ARIA_DROPEFFECT,
				AttributeName.ARIA_LABELLEDBY,
				AttributeName.ARIA_TEMPLATEID,
				AttributeName.COLOR_RENDERING,
				AttributeName.CONTENTEDITABLE,
				AttributeName.DIFFUSECONSTANT,
				AttributeName.ONDATAAVAILABLE,
				AttributeName.ONCONTROLSELECT,
				AttributeName.IMAGE_RENDERING,
				AttributeName.MEDIUMMATHSPACE,
				AttributeName.TEXT_DECORATION,
				AttributeName.SHAPE_RENDERING,
				AttributeName.STROKE_LINEJOIN,
				AttributeName.REPEAT_TEMPLATE,
				AttributeName.ARIA_DESCRIBEDBY,
				AttributeName.CONTENTSTYLETYPE,
				AttributeName.FONT_SIZE_ADJUST,
				AttributeName.KERNELUNITLENGTH,
				AttributeName.ONBEFOREACTIVATE,
				AttributeName.ONPROPERTYCHANGE,
				AttributeName.ONDATASETCHANGED,
				AttributeName.MASKCONTENTUNITS,
				AttributeName.PATTERNTRANSFORM,
				AttributeName.REQUIREDFEATURES,
				AttributeName.RENDERING_INTENT,
				AttributeName.SPECULAREXPONENT,
				AttributeName.SPECULARCONSTANT,
				AttributeName.SUPERSCRIPTSHIFT,
				AttributeName.STROKE_DASHARRAY,
				AttributeName.XCHANNELSELECTOR,
				AttributeName.YCHANNELSELECTOR,
				AttributeName.ARIA_AUTOCOMPLETE,
				AttributeName.CONTENTSCRIPTTYPE,
				AttributeName.ENABLE_BACKGROUND,
				AttributeName.DOMINANT_BASELINE,
				AttributeName.GRADIENTTRANSFORM,
				AttributeName.ONBEFORDEACTIVATE,
				AttributeName.ONDATASETCOMPLETE,
				AttributeName.OVERLINE_POSITION,
				AttributeName.ONBEFOREEDITFOCUS,
				AttributeName.LIMITINGCONEANGLE,
				AttributeName.VERYTHINMATHSPACE,
				AttributeName.STROKE_DASHOFFSET,
				AttributeName.STROKE_MITERLIMIT,
				AttributeName.ALIGNMENT_BASELINE,
				AttributeName.ONREADYSTATECHANGE,
				AttributeName.OVERLINE_THICKNESS,
				AttributeName.UNDERLINE_POSITION,
				AttributeName.VERYTHICKMATHSPACE,
				AttributeName.REQUIREDEXTENSIONS,
				AttributeName.COLOR_INTERPOLATION,
				AttributeName.UNDERLINE_THICKNESS,
				AttributeName.PRESERVEASPECTRATIO,
				AttributeName.PATTERNCONTENTUNITS,
				AttributeName.ARIA_MULTISELECTABLE,
				AttributeName.SCRIPTSIZEMULTIPLIER,
				AttributeName.ARIA_ACTIVEDESCENDANT,
				AttributeName.VERYVERYTHINMATHSPACE,
				AttributeName.VERYVERYTHICKMATHSPACE,
				AttributeName.STRIKETHROUGH_POSITION,
				AttributeName.STRIKETHROUGH_THICKNESS,
				AttributeName.EXTERNALRESOURCESREQUIRED,
				AttributeName.GLYPH_ORIENTATION_VERTICAL,
				AttributeName.COLOR_INTERPOLATION_FILTERS,
				AttributeName.GLYPH_ORIENTATION_HORIZONTAL
			};
			AttributeName.ATTRIBUTE_HASHES = new int[]
			{
				1153, 1383, 1601, 1793, 1827, 1857, 68600, 69146, 69177, 70237,
				70270, 71572, 71669, 72415, 72444, 74846, 74904, 74943, 75001, 75276,
				75590, 84742, 84839, 85575, 85963, 85992, 87204, 88074, 88171, 89130,
				89163, 3207892, 3283895, 3284791, 3338752, 3358197, 3369562, 3539124, 3562402, 3574260,
				3670335, 3696933, 3721879, 135280021, 135346322, 136317019, 136475749, 136548517, 136652214, 136884919,
				136902418, 136942992, 137292068, 139120259, 139785574, 142250603, 142314056, 142331176, 142519584, 144752417,
				145106895, 146147200, 146765926, 148805544, 149655723, 149809441, 150018784, 150445028, 150813181, 150923321,
				152528754, 152536216, 152647366, 152962785, 155219321, 155654904, 157317483, 157350248, 157437941, 157447478,
				157604838, 157685404, 157894402, 158315188, 166078431, 169409980, 169700259, 169856932, 170007032, 170409695,
				170466488, 170513710, 170608367, 173028944, 173896963, 176090625, 176129212, 179390001, 179489057, 179627464,
				179840468, 179849042, 180004216, 181779081, 183027151, 183645319, 183698797, 185922012, 185997252, 188312483,
				188675799, 190977533, 190992569, 191006194, 191033518, 191038774, 191096249, 191166163, 191194426, 191522106,
				191568039, 200104642, 202506661, 202537381, 202602917, 203070590, 203120766, 203389054, 203690071, 203971238,
				203986524, 209040857, 209125756, 212055489, 212322418, 212746849, 213002877, 213055164, 213088023, 213259873,
				213273386, 213435118, 213437318, 213438231, 213493071, 213532268, 213542834, 213584431, 213659891, 215285828,
				215880731, 216112976, 216684637, 217369699, 217565298, 217576549, 218186795, 219743185, 220082234, 221623802,
				221986406, 222283890, 223089542, 223138630, 223311265, 224547358, 224587256, 224589550, 224655650, 224785518,
				224810917, 224813302, 225429618, 225432950, 225440869, 236107233, 236709921, 236838947, 237117095, 237143271,
				237172455, 237209953, 237354143, 237372743, 237668065, 237703073, 237714273, 239743521, 240512803, 240522627,
				240560417, 240656513, 241015715, 241062755, 241065383, 243523041, 245865199, 246261793, 246556195, 246774817,
				246923491, 246928419, 246981667, 247014847, 247058369, 247112833, 247118177, 247119137, 247128739, 247316903,
				249533729, 250235623, 250269543, 251402351, 252339047, 253260911, 253293679, 254844367, 255547879, 256077281,
				256345377, 258124199, 258354465, 258605063, 258744193, 258845603, 258856961, 258926689, 269869248, 270174334,
				270709417, 270778994, 270781796, 271102503, 271478858, 271490090, 272870654, 273335275, 273369140, 273924313,
				274108530, 274116736, 276818662, 277476156, 279156579, 279349675, 280108533, 280128712, 280132869, 280162403,
				280280292, 280413430, 280506130, 280677397, 280678580, 280686710, 280689066, 282736758, 283110901, 283275116,
				283823226, 283890012, 284479340, 284606461, 286700477, 286798916, 291557706, 291665349, 291804100, 292138018,
				292166446, 292418738, 292451039, 300298041, 300374839, 300597935, 303073389, 303083839, 303266673, 303354997,
				303430688, 303576261, 303724281, 303819694, 304242723, 304382625, 306247792, 307227811, 307468786, 307724489,
				309671175, 310252031, 310358241, 310373094, 311015256, 313357609, 313683893, 313701861, 313706996, 313707317,
				313710350, 314027746, 314038181, 314091299, 314205627, 314233813, 316741830, 316797986, 317486755, 317794164,
				320076137, 322657125, 322887778, 323506876, 323572412, 323605180, 325060058, 325320188, 325398738, 325541490,
				325671619, 333868843, 336806130, 337212108, 337282686, 337285434, 337585223, 338036037, 338298087, 338566051,
				340943551, 341190970, 342995704, 343352124, 343912673, 344585053, 346977248, 347218098, 347262163, 347278576,
				347438191, 347655959, 347684788, 347726430, 347727772, 347776035, 347776629, 349500753, 350880161, 350887073,
				353384123, 355496998, 355906922, 355979793, 356545959, 358637867, 358905016, 359164318, 359247286, 359350571,
				359579447, 365560330, 367399355, 367420285, 367510727, 368013212, 370234760, 370353345, 370710317, 371074566,
				371122285, 371194213, 371448425, 371448430, 371545055, 371593469, 371596922, 371758751, 371964792, 372151328,
				376550136, 376710172, 376795771, 376826271, 376906556, 380514830, 380774774, 380775037, 381030322, 381136500,
				381281631, 381282269, 381285504, 381330595, 381331422, 381335911, 381336484, 383907298, 383917408, 384595009,
				384595013, 387799894, 387823201, 392581647, 392584937, 392742684, 392906485, 393003349, 400644707, 400973830,
				404428547, 404432113, 404432865, 404469244, 404478897, 404694860, 406887479, 408294949, 408789955, 410022510,
				410467324, 410586448, 410945965, 411845275, 414327152, 414327932, 414329781, 414346257, 414346439, 414639928,
				414835998, 414894517, 414986533, 417465377, 417465381, 417492216, 418259232, 419310946, 420103495, 420242342,
				420380455, 420658662, 420717432, 423183880, 424539259, 425929170, 425972964, 426050649, 426126450, 426142833,
				426607922, 437289840, 437347469, 437412335, 437423943, 437455540, 437462252, 437597991, 437617485, 437986305,
				437986507, 437986828, 437987072, 438015591, 438034813, 438038966, 438179623, 438347971, 438483573, 438547062,
				438895551, 441592676, 442032555, 443548979, 447881379, 447881655, 447881895, 447887844, 448416189, 448445746,
				448449012, 450942191, 452816744, 453668677, 454434495, 456610076, 456642844, 456738709, 457544600, 459451897,
				459680944, 468058810, 468083581, 470964084, 471470955, 471567278, 472267822, 481177859, 481210627, 481435874,
				481455115, 481485378, 481490218, 485105638, 486005878, 486383494, 487988916, 488103783, 490661867, 491574090,
				491578272, 493041952, 493441205, 493582844, 493716979, 504577572, 504740359, 505091638, 505592418, 505656212,
				509516275, 514998531, 515571132, 515594682, 518712698, 521362273, 526592419, 526807354, 527348842, 538294791,
				539214049, 544689535, 545535009, 548544752, 548563346, 548595116, 551679010, 558034099, 560329411, 560356209,
				560671018, 560671152, 560692590, 560845442, 569212097, 569474241, 572252718, 572768481, 575326764, 576174758,
				576190819, 582099184, 582099438, 582372519, 582558889, 586552164, 591325418, 594231990, 594243961, 605711268,
				615672071, 616086845, 621792370, 624879850, 627432831, 640040548, 654392808, 658675477, 659420283, 672891587,
				694768102, 705890982, 725543146, 759097578, 761686526, 795383908, 843809551, 878105336, 908643300, 945213471
			};
		}

		public const int NCNAME_HTML = 1;

		public const int NCNAME_FOREIGN = 6;

		public const int NCNAME_LANG = 8;

		public const int IS_XMLNS = 16;

		public const int CASE_FOLDED = 32;

		public const int BOOLEAN = 64;

		public const int HTML = 0;

		public const int MATHML = 1;

		public const int SVG = 2;

		public const int HTML_LANG = 3;

		[NsUri]
		static readonly string[] ALL_NO_NS = new string[] { "", "", "", "" };

		[NsUri]
		static readonly string[] XMLNS_NS = new string[] { "", "http://www.w3.org/2000/xmlns/", "http://www.w3.org/2000/xmlns/", "" };

		[NsUri]
		static readonly string[] XML_NS = new string[] { "", "http://www.w3.org/XML/1998/namespace", "http://www.w3.org/XML/1998/namespace", "" };

		[NsUri]
		static readonly string[] XLINK_NS = new string[] { "", "http://www.w3.org/1999/xlink", "http://www.w3.org/1999/xlink", "" };

		[NsUri]
		static readonly string[] LANG_NS = new string[] { "", "", "", "http://www.w3.org/XML/1998/namespace" };

		[Prefix]
		static readonly string[] ALL_NO_PREFIX;

		[Prefix]
		static readonly string[] XMLNS_PREFIX;

		[Prefix]
		static readonly string[] XLINK_PREFIX;

		[Prefix]
		static readonly string[] XML_PREFIX;

		[Prefix]
		static readonly string[] LANG_PREFIX;

		[NsUri]
		readonly string[] uri;

		[Local]
		readonly string[] local;

		[Prefix]
		readonly string[] prefix;

		readonly int flags;

		readonly string[] qName;

		public static readonly AttributeName D;

		public static readonly AttributeName K;

		public static readonly AttributeName R;

		public static readonly AttributeName X;

		public static readonly AttributeName Y;

		public static readonly AttributeName Z;

		public static readonly AttributeName BY;

		public static readonly AttributeName CX;

		public static readonly AttributeName CY;

		public static readonly AttributeName DX;

		public static readonly AttributeName DY;

		public static readonly AttributeName G2;

		public static readonly AttributeName G1;

		public static readonly AttributeName FX;

		public static readonly AttributeName FY;

		public static readonly AttributeName K4;

		public static readonly AttributeName K2;

		public static readonly AttributeName K3;

		public static readonly AttributeName K1;

		public static readonly AttributeName ID;

		public static readonly AttributeName IN;

		public static readonly AttributeName U2;

		public static readonly AttributeName U1;

		public static readonly AttributeName RT;

		public static readonly AttributeName RX;

		public static readonly AttributeName RY;

		public static readonly AttributeName TO;

		public static readonly AttributeName Y2;

		public static readonly AttributeName Y1;

		public static readonly AttributeName X1;

		public static readonly AttributeName X2;

		public static readonly AttributeName ALT;

		public static readonly AttributeName DIR;

		public static readonly AttributeName DUR;

		public static readonly AttributeName END;

		public static readonly AttributeName FOR;

		public static readonly AttributeName IN2;

		public static readonly AttributeName MAX;

		public static readonly AttributeName MIN;

		public static readonly AttributeName LOW;

		public static readonly AttributeName REL;

		public static readonly AttributeName REV;

		public static readonly AttributeName SRC;

		public static readonly AttributeName AXIS;

		public static readonly AttributeName ABBR;

		public static readonly AttributeName BBOX;

		public static readonly AttributeName CITE;

		public static readonly AttributeName CODE;

		public static readonly AttributeName BIAS;

		public static readonly AttributeName COLS;

		public static readonly AttributeName CLIP;

		public static readonly AttributeName CHAR;

		public static readonly AttributeName BASE;

		public static readonly AttributeName EDGE;

		public static readonly AttributeName DATA;

		public static readonly AttributeName FILL;

		public static readonly AttributeName FROM;

		public static readonly AttributeName FORM;

		public static readonly AttributeName FACE;

		public static readonly AttributeName HIGH;

		public static readonly AttributeName HREF;

		public static readonly AttributeName OPEN;

		public static readonly AttributeName ICON;

		public static readonly AttributeName NAME;

		public static readonly AttributeName MODE;

		public static readonly AttributeName MASK;

		public static readonly AttributeName LINK;

		public static readonly AttributeName LANG;

		public static readonly AttributeName LOOP;

		public static readonly AttributeName LIST;

		public static readonly AttributeName TYPE;

		public static readonly AttributeName WHEN;

		public static readonly AttributeName WRAP;

		public static readonly AttributeName TEXT;

		public static readonly AttributeName PATH;

		public static readonly AttributeName PING;

		public static readonly AttributeName REFX;

		public static readonly AttributeName REFY;

		public static readonly AttributeName SIZE;

		public static readonly AttributeName SEED;

		public static readonly AttributeName ROWS;

		public static readonly AttributeName SPAN;

		public static readonly AttributeName STEP;

		public static readonly AttributeName ROLE;

		public static readonly AttributeName XREF;

		public static readonly AttributeName ASYNC;

		public static readonly AttributeName ALINK;

		public static readonly AttributeName ALIGN;

		public static readonly AttributeName CLOSE;

		public static readonly AttributeName COLOR;

		public static readonly AttributeName CLASS;

		public static readonly AttributeName CLEAR;

		public static readonly AttributeName BEGIN;

		public static readonly AttributeName DEPTH;

		public static readonly AttributeName DEFER;

		public static readonly AttributeName FENCE;

		public static readonly AttributeName FRAME;

		public static readonly AttributeName ISMAP;

		public static readonly AttributeName ONEND;

		public static readonly AttributeName INDEX;

		public static readonly AttributeName ORDER;

		public static readonly AttributeName OTHER;

		public static readonly AttributeName ONCUT;

		public static readonly AttributeName NARGS;

		public static readonly AttributeName MEDIA;

		public static readonly AttributeName LABEL;

		public static readonly AttributeName LOCAL;

		public static readonly AttributeName WIDTH;

		public static readonly AttributeName TITLE;

		public static readonly AttributeName VLINK;

		public static readonly AttributeName VALUE;

		public static readonly AttributeName SLOPE;

		public static readonly AttributeName SHAPE;

		public static readonly AttributeName SCOPE;

		public static readonly AttributeName SCALE;

		public static readonly AttributeName SPEED;

		public static readonly AttributeName STYLE;

		public static readonly AttributeName RULES;

		public static readonly AttributeName STEMH;

		public static readonly AttributeName STEMV;

		public static readonly AttributeName START;

		public static readonly AttributeName XMLNS;

		public static readonly AttributeName ACCEPT;

		public static readonly AttributeName ACCENT;

		public static readonly AttributeName ASCENT;

		public static readonly AttributeName ACTIVE;

		public static readonly AttributeName ALTIMG;

		public static readonly AttributeName ACTION;

		public static readonly AttributeName BORDER;

		public static readonly AttributeName CURSOR;

		public static readonly AttributeName COORDS;

		public static readonly AttributeName FILTER;

		public static readonly AttributeName FORMAT;

		public static readonly AttributeName HIDDEN;

		public static readonly AttributeName HSPACE;

		public static readonly AttributeName HEIGHT;

		public static readonly AttributeName ONMOVE;

		public static readonly AttributeName ONLOAD;

		public static readonly AttributeName ONDRAG;

		public static readonly AttributeName ORIGIN;

		public static readonly AttributeName ONZOOM;

		public static readonly AttributeName ONHELP;

		public static readonly AttributeName ONSTOP;

		public static readonly AttributeName ONDROP;

		public static readonly AttributeName ONBLUR;

		public static readonly AttributeName OBJECT;

		public static readonly AttributeName OFFSET;

		public static readonly AttributeName ORIENT;

		public static readonly AttributeName ONCOPY;

		public static readonly AttributeName NOWRAP;

		public static readonly AttributeName NOHREF;

		public static readonly AttributeName MACROS;

		public static readonly AttributeName METHOD;

		public static readonly AttributeName LOWSRC;

		public static readonly AttributeName LSPACE;

		public static readonly AttributeName LQUOTE;

		public static readonly AttributeName USEMAP;

		public static readonly AttributeName WIDTHS;

		public static readonly AttributeName TARGET;

		public static readonly AttributeName VALUES;

		public static readonly AttributeName VALIGN;

		public static readonly AttributeName VSPACE;

		public static readonly AttributeName POSTER;

		public static readonly AttributeName POINTS;

		public static readonly AttributeName PROMPT;

		public static readonly AttributeName SCOPED;

		public static readonly AttributeName STRING;

		public static readonly AttributeName SCHEME;

		public static readonly AttributeName STROKE;

		public static readonly AttributeName RADIUS;

		public static readonly AttributeName RESULT;

		public static readonly AttributeName REPEAT;

		public static readonly AttributeName RSPACE;

		public static readonly AttributeName ROTATE;

		public static readonly AttributeName RQUOTE;

		public static readonly AttributeName ALTTEXT;

		public static readonly AttributeName ARCHIVE;

		public static readonly AttributeName AZIMUTH;

		public static readonly AttributeName CLOSURE;

		public static readonly AttributeName CHECKED;

		public static readonly AttributeName CLASSID;

		public static readonly AttributeName CHAROFF;

		public static readonly AttributeName BGCOLOR;

		public static readonly AttributeName COLSPAN;

		public static readonly AttributeName CHARSET;

		public static readonly AttributeName COMPACT;

		public static readonly AttributeName CONTENT;

		public static readonly AttributeName ENCTYPE;

		public static readonly AttributeName DATASRC;

		public static readonly AttributeName DATAFLD;

		public static readonly AttributeName DECLARE;

		public static readonly AttributeName DISPLAY;

		public static readonly AttributeName DIVISOR;

		public static readonly AttributeName DEFAULT;

		public static readonly AttributeName DESCENT;

		public static readonly AttributeName KERNING;

		public static readonly AttributeName HANGING;

		public static readonly AttributeName HEADERS;

		public static readonly AttributeName ONPASTE;

		public static readonly AttributeName ONCLICK;

		public static readonly AttributeName OPTIMUM;

		public static readonly AttributeName ONBEGIN;

		public static readonly AttributeName ONKEYUP;

		public static readonly AttributeName ONFOCUS;

		public static readonly AttributeName ONERROR;

		public static readonly AttributeName ONINPUT;

		public static readonly AttributeName ONABORT;

		public static readonly AttributeName ONSTART;

		public static readonly AttributeName ONRESET;

		public static readonly AttributeName OPACITY;

		public static readonly AttributeName NOSHADE;

		public static readonly AttributeName MINSIZE;

		public static readonly AttributeName MAXSIZE;

		public static readonly AttributeName LARGEOP;

		public static readonly AttributeName UNICODE;

		public static readonly AttributeName TARGETX;

		public static readonly AttributeName TARGETY;

		public static readonly AttributeName VIEWBOX;

		public static readonly AttributeName VERSION;

		public static readonly AttributeName PATTERN;

		public static readonly AttributeName PROFILE;

		public static readonly AttributeName SPACING;

		public static readonly AttributeName RESTART;

		public static readonly AttributeName ROWSPAN;

		public static readonly AttributeName SANDBOX;

		public static readonly AttributeName SUMMARY;

		public static readonly AttributeName STANDBY;

		public static readonly AttributeName REPLACE;

		public static readonly AttributeName AUTOPLAY;

		public static readonly AttributeName ADDITIVE;

		public static readonly AttributeName CALCMODE;

		public static readonly AttributeName CODETYPE;

		public static readonly AttributeName CODEBASE;

		public static readonly AttributeName CONTROLS;

		public static readonly AttributeName BEVELLED;

		public static readonly AttributeName BASELINE;

		public static readonly AttributeName EXPONENT;

		public static readonly AttributeName EDGEMODE;

		public static readonly AttributeName ENCODING;

		public static readonly AttributeName GLYPHREF;

		public static readonly AttributeName DATETIME;

		public static readonly AttributeName DISABLED;

		public static readonly AttributeName FONTSIZE;

		public static readonly AttributeName KEYTIMES;

		public static readonly AttributeName PANOSE_1;

		public static readonly AttributeName HREFLANG;

		public static readonly AttributeName ONRESIZE;

		public static readonly AttributeName ONCHANGE;

		public static readonly AttributeName ONBOUNCE;

		public static readonly AttributeName ONUNLOAD;

		public static readonly AttributeName ONFINISH;

		public static readonly AttributeName ONSCROLL;

		public static readonly AttributeName OPERATOR;

		public static readonly AttributeName OVERFLOW;

		public static readonly AttributeName ONSUBMIT;

		public static readonly AttributeName ONREPEAT;

		public static readonly AttributeName ONSELECT;

		public static readonly AttributeName NOTATION;

		public static readonly AttributeName NORESIZE;

		public static readonly AttributeName MANIFEST;

		public static readonly AttributeName MATHSIZE;

		public static readonly AttributeName MULTIPLE;

		public static readonly AttributeName LONGDESC;

		public static readonly AttributeName LANGUAGE;

		public static readonly AttributeName TEMPLATE;

		public static readonly AttributeName TABINDEX;

		public static readonly AttributeName READONLY;

		public static readonly AttributeName SELECTED;

		public static readonly AttributeName ROWLINES;

		public static readonly AttributeName SEAMLESS;

		public static readonly AttributeName ROWALIGN;

		public static readonly AttributeName STRETCHY;

		public static readonly AttributeName REQUIRED;

		public static readonly AttributeName XML_BASE;

		public static readonly AttributeName XML_LANG;

		public static readonly AttributeName X_HEIGHT;

		public static readonly AttributeName ARIA_OWNS;

		public static readonly AttributeName AUTOFOCUS;

		public static readonly AttributeName ARIA_SORT;

		public static readonly AttributeName ACCESSKEY;

		public static readonly AttributeName ARIA_BUSY;

		public static readonly AttributeName ARIA_GRAB;

		public static readonly AttributeName AMPLITUDE;

		public static readonly AttributeName ARIA_LIVE;

		public static readonly AttributeName CLIP_RULE;

		public static readonly AttributeName CLIP_PATH;

		public static readonly AttributeName EQUALROWS;

		public static readonly AttributeName ELEVATION;

		public static readonly AttributeName DIRECTION;

		public static readonly AttributeName DRAGGABLE;

		public static readonly AttributeName FILTERRES;

		public static readonly AttributeName FILL_RULE;

		public static readonly AttributeName FONTSTYLE;

		public static readonly AttributeName FONT_SIZE;

		public static readonly AttributeName KEYPOINTS;

		public static readonly AttributeName HIDEFOCUS;

		public static readonly AttributeName ONMESSAGE;

		public static readonly AttributeName INTERCEPT;

		public static readonly AttributeName ONDRAGEND;

		public static readonly AttributeName ONMOVEEND;

		public static readonly AttributeName ONINVALID;

		public static readonly AttributeName ONKEYDOWN;

		public static readonly AttributeName ONFOCUSIN;

		public static readonly AttributeName ONMOUSEUP;

		public static readonly AttributeName INPUTMODE;

		public static readonly AttributeName ONROWEXIT;

		public static readonly AttributeName MATHCOLOR;

		public static readonly AttributeName MASKUNITS;

		public static readonly AttributeName MAXLENGTH;

		public static readonly AttributeName LINEBREAK;

		public static readonly AttributeName TRANSFORM;

		public static readonly AttributeName V_HANGING;

		public static readonly AttributeName VALUETYPE;

		public static readonly AttributeName POINTSATZ;

		public static readonly AttributeName POINTSATX;

		public static readonly AttributeName POINTSATY;

		public static readonly AttributeName SYMMETRIC;

		public static readonly AttributeName SCROLLING;

		public static readonly AttributeName REPEATDUR;

		public static readonly AttributeName SELECTION;

		public static readonly AttributeName SEPARATOR;

		public static readonly AttributeName XML_SPACE;

		public static readonly AttributeName AUTOSUBMIT;

		public static readonly AttributeName ALPHABETIC;

		public static readonly AttributeName ACTIONTYPE;

		public static readonly AttributeName ACCUMULATE;

		public static readonly AttributeName ARIA_LEVEL;

		public static readonly AttributeName COLUMNSPAN;

		public static readonly AttributeName CAP_HEIGHT;

		public static readonly AttributeName BACKGROUND;

		public static readonly AttributeName GLYPH_NAME;

		public static readonly AttributeName GROUPALIGN;

		public static readonly AttributeName FONTFAMILY;

		public static readonly AttributeName FONTWEIGHT;

		public static readonly AttributeName FONT_STYLE;

		public static readonly AttributeName KEYSPLINES;

		public static readonly AttributeName HTTP_EQUIV;

		public static readonly AttributeName ONACTIVATE;

		public static readonly AttributeName OCCURRENCE;

		public static readonly AttributeName IRRELEVANT;

		public static readonly AttributeName ONDBLCLICK;

		public static readonly AttributeName ONDRAGDROP;

		public static readonly AttributeName ONKEYPRESS;

		public static readonly AttributeName ONROWENTER;

		public static readonly AttributeName ONDRAGOVER;

		public static readonly AttributeName ONFOCUSOUT;

		public static readonly AttributeName ONMOUSEOUT;

		public static readonly AttributeName NUMOCTAVES;

		public static readonly AttributeName MARKER_MID;

		public static readonly AttributeName MARKER_END;

		public static readonly AttributeName TEXTLENGTH;

		public static readonly AttributeName VISIBILITY;

		public static readonly AttributeName VIEWTARGET;

		public static readonly AttributeName VERT_ADV_Y;

		public static readonly AttributeName PATHLENGTH;

		public static readonly AttributeName REPEAT_MAX;

		public static readonly AttributeName RADIOGROUP;

		public static readonly AttributeName STOP_COLOR;

		public static readonly AttributeName SEPARATORS;

		public static readonly AttributeName REPEAT_MIN;

		public static readonly AttributeName ROWSPACING;

		public static readonly AttributeName ZOOMANDPAN;

		public static readonly AttributeName XLINK_TYPE;

		public static readonly AttributeName XLINK_ROLE;

		public static readonly AttributeName XLINK_HREF;

		public static readonly AttributeName XLINK_SHOW;

		public static readonly AttributeName ACCENTUNDER;

		public static readonly AttributeName ARIA_SECRET;

		public static readonly AttributeName ARIA_ATOMIC;

		public static readonly AttributeName ARIA_HIDDEN;

		public static readonly AttributeName ARIA_FLOWTO;

		public static readonly AttributeName ARABIC_FORM;

		public static readonly AttributeName CELLPADDING;

		public static readonly AttributeName CELLSPACING;

		public static readonly AttributeName COLUMNWIDTH;

		public static readonly AttributeName CROSSORIGIN;

		public static readonly AttributeName COLUMNALIGN;

		public static readonly AttributeName COLUMNLINES;

		public static readonly AttributeName CONTEXTMENU;

		public static readonly AttributeName BASEPROFILE;

		public static readonly AttributeName FONT_FAMILY;

		public static readonly AttributeName FRAMEBORDER;

		public static readonly AttributeName FILTERUNITS;

		public static readonly AttributeName FLOOD_COLOR;

		public static readonly AttributeName FONT_WEIGHT;

		public static readonly AttributeName HORIZ_ADV_X;

		public static readonly AttributeName ONDRAGLEAVE;

		public static readonly AttributeName ONMOUSEMOVE;

		public static readonly AttributeName ORIENTATION;

		public static readonly AttributeName ONMOUSEDOWN;

		public static readonly AttributeName ONMOUSEOVER;

		public static readonly AttributeName ONDRAGENTER;

		public static readonly AttributeName IDEOGRAPHIC;

		public static readonly AttributeName ONBEFORECUT;

		public static readonly AttributeName ONFORMINPUT;

		public static readonly AttributeName ONDRAGSTART;

		public static readonly AttributeName ONMOVESTART;

		public static readonly AttributeName MARKERUNITS;

		public static readonly AttributeName MATHVARIANT;

		public static readonly AttributeName MARGINWIDTH;

		public static readonly AttributeName MARKERWIDTH;

		public static readonly AttributeName TEXT_ANCHOR;

		public static readonly AttributeName TABLEVALUES;

		public static readonly AttributeName SCRIPTLEVEL;

		public static readonly AttributeName REPEATCOUNT;

		public static readonly AttributeName STITCHTILES;

		public static readonly AttributeName STARTOFFSET;

		public static readonly AttributeName SCROLLDELAY;

		public static readonly AttributeName XMLNS_XLINK;

		public static readonly AttributeName XLINK_TITLE;

		public static readonly AttributeName ARIA_INVALID;

		public static readonly AttributeName ARIA_PRESSED;

		public static readonly AttributeName ARIA_CHECKED;

		public static readonly AttributeName AUTOCOMPLETE;

		public static readonly AttributeName ARIA_SETSIZE;

		public static readonly AttributeName ARIA_CHANNEL;

		public static readonly AttributeName EQUALCOLUMNS;

		public static readonly AttributeName DISPLAYSTYLE;

		public static readonly AttributeName DATAFORMATAS;

		public static readonly AttributeName FILL_OPACITY;

		public static readonly AttributeName FONT_VARIANT;

		public static readonly AttributeName FONT_STRETCH;

		public static readonly AttributeName FRAMESPACING;

		public static readonly AttributeName KERNELMATRIX;

		public static readonly AttributeName ONDEACTIVATE;

		public static readonly AttributeName ONROWSDELETE;

		public static readonly AttributeName ONMOUSELEAVE;

		public static readonly AttributeName ONFORMCHANGE;

		public static readonly AttributeName ONCELLCHANGE;

		public static readonly AttributeName ONMOUSEWHEEL;

		public static readonly AttributeName ONMOUSEENTER;

		public static readonly AttributeName ONAFTERPRINT;

		public static readonly AttributeName ONBEFORECOPY;

		public static readonly AttributeName MARGINHEIGHT;

		public static readonly AttributeName MARKERHEIGHT;

		public static readonly AttributeName MARKER_START;

		public static readonly AttributeName MATHEMATICAL;

		public static readonly AttributeName LENGTHADJUST;

		public static readonly AttributeName UNSELECTABLE;

		public static readonly AttributeName UNICODE_BIDI;

		public static readonly AttributeName UNITS_PER_EM;

		public static readonly AttributeName WORD_SPACING;

		public static readonly AttributeName WRITING_MODE;

		public static readonly AttributeName V_ALPHABETIC;

		public static readonly AttributeName PATTERNUNITS;

		public static readonly AttributeName SPREADMETHOD;

		public static readonly AttributeName SURFACESCALE;

		public static readonly AttributeName STROKE_WIDTH;

		public static readonly AttributeName REPEAT_START;

		public static readonly AttributeName STDDEVIATION;

		public static readonly AttributeName STOP_OPACITY;

		public static readonly AttributeName ARIA_CONTROLS;

		public static readonly AttributeName ARIA_HASPOPUP;

		public static readonly AttributeName ACCENT_HEIGHT;

		public static readonly AttributeName ARIA_VALUENOW;

		public static readonly AttributeName ARIA_RELEVANT;

		public static readonly AttributeName ARIA_POSINSET;

		public static readonly AttributeName ARIA_VALUEMAX;

		public static readonly AttributeName ARIA_READONLY;

		public static readonly AttributeName ARIA_SELECTED;

		public static readonly AttributeName ARIA_REQUIRED;

		public static readonly AttributeName ARIA_EXPANDED;

		public static readonly AttributeName ARIA_DISABLED;

		public static readonly AttributeName ATTRIBUTETYPE;

		public static readonly AttributeName ATTRIBUTENAME;

		public static readonly AttributeName ARIA_DATATYPE;

		public static readonly AttributeName ARIA_VALUEMIN;

		public static readonly AttributeName BASEFREQUENCY;

		public static readonly AttributeName COLUMNSPACING;

		public static readonly AttributeName COLOR_PROFILE;

		public static readonly AttributeName CLIPPATHUNITS;

		public static readonly AttributeName DEFINITIONURL;

		public static readonly AttributeName GRADIENTUNITS;

		public static readonly AttributeName FLOOD_OPACITY;

		public static readonly AttributeName ONAFTERUPDATE;

		public static readonly AttributeName ONERRORUPDATE;

		public static readonly AttributeName ONBEFOREPASTE;

		public static readonly AttributeName ONLOSECAPTURE;

		public static readonly AttributeName ONCONTEXTMENU;

		public static readonly AttributeName ONSELECTSTART;

		public static readonly AttributeName ONBEFOREPRINT;

		public static readonly AttributeName MOVABLELIMITS;

		public static readonly AttributeName LINETHICKNESS;

		public static readonly AttributeName UNICODE_RANGE;

		public static readonly AttributeName THINMATHSPACE;

		public static readonly AttributeName VERT_ORIGIN_X;

		public static readonly AttributeName VERT_ORIGIN_Y;

		public static readonly AttributeName V_IDEOGRAPHIC;

		public static readonly AttributeName PRESERVEALPHA;

		public static readonly AttributeName SCRIPTMINSIZE;

		public static readonly AttributeName SPECIFICATION;

		public static readonly AttributeName XLINK_ACTUATE;

		public static readonly AttributeName XLINK_ARCROLE;

		public static readonly AttributeName ACCEPT_CHARSET;

		public static readonly AttributeName ALIGNMENTSCOPE;

		public static readonly AttributeName ARIA_MULTILINE;

		public static readonly AttributeName BASELINE_SHIFT;

		public static readonly AttributeName HORIZ_ORIGIN_X;

		public static readonly AttributeName HORIZ_ORIGIN_Y;

		public static readonly AttributeName ONBEFOREUPDATE;

		public static readonly AttributeName ONFILTERCHANGE;

		public static readonly AttributeName ONROWSINSERTED;

		public static readonly AttributeName ONBEFOREUNLOAD;

		public static readonly AttributeName MATHBACKGROUND;

		public static readonly AttributeName LETTER_SPACING;

		public static readonly AttributeName LIGHTING_COLOR;

		public static readonly AttributeName THICKMATHSPACE;

		public static readonly AttributeName TEXT_RENDERING;

		public static readonly AttributeName V_MATHEMATICAL;

		public static readonly AttributeName POINTER_EVENTS;

		public static readonly AttributeName PRIMITIVEUNITS;

		public static readonly AttributeName SYSTEMLANGUAGE;

		public static readonly AttributeName STROKE_LINECAP;

		public static readonly AttributeName SUBSCRIPTSHIFT;

		public static readonly AttributeName STROKE_OPACITY;

		public static readonly AttributeName ARIA_DROPEFFECT;

		public static readonly AttributeName ARIA_LABELLEDBY;

		public static readonly AttributeName ARIA_TEMPLATEID;

		public static readonly AttributeName COLOR_RENDERING;

		public static readonly AttributeName CONTENTEDITABLE;

		public static readonly AttributeName DIFFUSECONSTANT;

		public static readonly AttributeName ONDATAAVAILABLE;

		public static readonly AttributeName ONCONTROLSELECT;

		public static readonly AttributeName IMAGE_RENDERING;

		public static readonly AttributeName MEDIUMMATHSPACE;

		public static readonly AttributeName TEXT_DECORATION;

		public static readonly AttributeName SHAPE_RENDERING;

		public static readonly AttributeName STROKE_LINEJOIN;

		public static readonly AttributeName REPEAT_TEMPLATE;

		public static readonly AttributeName ARIA_DESCRIBEDBY;

		public static readonly AttributeName CONTENTSTYLETYPE;

		public static readonly AttributeName FONT_SIZE_ADJUST;

		public static readonly AttributeName KERNELUNITLENGTH;

		public static readonly AttributeName ONBEFOREACTIVATE;

		public static readonly AttributeName ONPROPERTYCHANGE;

		public static readonly AttributeName ONDATASETCHANGED;

		public static readonly AttributeName MASKCONTENTUNITS;

		public static readonly AttributeName PATTERNTRANSFORM;

		public static readonly AttributeName REQUIREDFEATURES;

		public static readonly AttributeName RENDERING_INTENT;

		public static readonly AttributeName SPECULAREXPONENT;

		public static readonly AttributeName SPECULARCONSTANT;

		public static readonly AttributeName SUPERSCRIPTSHIFT;

		public static readonly AttributeName STROKE_DASHARRAY;

		public static readonly AttributeName XCHANNELSELECTOR;

		public static readonly AttributeName YCHANNELSELECTOR;

		public static readonly AttributeName ARIA_AUTOCOMPLETE;

		public static readonly AttributeName CONTENTSCRIPTTYPE;

		public static readonly AttributeName ENABLE_BACKGROUND;

		public static readonly AttributeName DOMINANT_BASELINE;

		public static readonly AttributeName GRADIENTTRANSFORM;

		public static readonly AttributeName ONBEFORDEACTIVATE;

		public static readonly AttributeName ONDATASETCOMPLETE;

		public static readonly AttributeName OVERLINE_POSITION;

		public static readonly AttributeName ONBEFOREEDITFOCUS;

		public static readonly AttributeName LIMITINGCONEANGLE;

		public static readonly AttributeName VERYTHINMATHSPACE;

		public static readonly AttributeName STROKE_DASHOFFSET;

		public static readonly AttributeName STROKE_MITERLIMIT;

		public static readonly AttributeName ALIGNMENT_BASELINE;

		public static readonly AttributeName ONREADYSTATECHANGE;

		public static readonly AttributeName OVERLINE_THICKNESS;

		public static readonly AttributeName UNDERLINE_POSITION;

		public static readonly AttributeName VERYTHICKMATHSPACE;

		public static readonly AttributeName REQUIREDEXTENSIONS;

		public static readonly AttributeName COLOR_INTERPOLATION;

		public static readonly AttributeName UNDERLINE_THICKNESS;

		public static readonly AttributeName PRESERVEASPECTRATIO;

		public static readonly AttributeName PATTERNCONTENTUNITS;

		public static readonly AttributeName ARIA_MULTISELECTABLE;

		public static readonly AttributeName SCRIPTSIZEMULTIPLIER;

		public static readonly AttributeName ARIA_ACTIVEDESCENDANT;

		public static readonly AttributeName VERYVERYTHINMATHSPACE;

		public static readonly AttributeName VERYVERYTHICKMATHSPACE;

		public static readonly AttributeName STRIKETHROUGH_POSITION;

		public static readonly AttributeName STRIKETHROUGH_THICKNESS;

		public static readonly AttributeName EXTERNALRESOURCESREQUIRED;

		public static readonly AttributeName GLYPH_ORIENTATION_VERTICAL;

		public static readonly AttributeName COLOR_INTERPOLATION_FILTERS;

		public static readonly AttributeName GLYPH_ORIENTATION_HORIZONTAL;

		static readonly AttributeName[] ATTRIBUTE_NAMES;

		static readonly int[] ATTRIBUTE_HASHES;
	}
}
