using System;
using HtmlParserSharp.Common;

namespace HtmlParserSharp.Core
{
	sealed class HtmlAttributes : IEquatable<HtmlAttributes>
	{
		public HtmlAttributes(int mode)
		{
			this.mode = mode;
			this.length = 0;
			this.names = new AttributeName[5];
			this.values = new string[5];
			this.idValue = null;
			this.xmlnsLength = 0;
			this.xmlnsNames = HtmlAttributes.EMPTY_ATTRIBUTENAMES;
			this.xmlnsValues = HtmlAttributes.EMPTY_stringS;
		}

		public int GetIndex(AttributeName name)
		{
			for (int i = 0; i < this.length; i++)
			{
				if (this.names[i] == name)
				{
					return i;
				}
			}
			return -1;
		}

		public int GetIndex(string qName)
		{
			for (int i = 0; i < this.length; i++)
			{
				if (this.names[i].GetQName(this.mode) == qName)
				{
					return i;
				}
			}
			return -1;
		}

		public int GetIndex(string uri, string localName)
		{
			for (int i = 0; i < this.length; i++)
			{
				if (this.names[i].GetLocal(this.mode) == localName && this.names[i].GetUri(this.mode) == uri)
				{
					return i;
				}
			}
			return -1;
		}

		public string GetType(string qName)
		{
			int index = this.GetIndex(qName);
			if (index == -1)
			{
				return null;
			}
			return this.GetType(index);
		}

		public string GetType(string uri, string localName)
		{
			int index = this.GetIndex(uri, localName);
			if (index == -1)
			{
				return null;
			}
			return this.GetType(index);
		}

		public string GetValue(string qName)
		{
			int index = this.GetIndex(qName);
			if (index == -1)
			{
				return null;
			}
			return this.GetValue(index);
		}

		public string GetValue(string uri, string localName)
		{
			int index = this.GetIndex(uri, localName);
			if (index == -1)
			{
				return null;
			}
			return this.GetValue(index);
		}

		public int Length
		{
			get
			{
				return this.length;
			}
		}

		[Local]
		public string GetLocalName(int index)
		{
			if (index < this.length && index >= 0)
			{
				return this.names[index].GetLocal(this.mode);
			}
			return null;
		}

		public string GetQName(int index)
		{
			if (index < this.length && index >= 0)
			{
				return this.names[index].GetQName(this.mode);
			}
			return null;
		}

		public string GetType(int index)
		{
			if (index >= this.length || index < 0)
			{
				return null;
			}
			if (!(this.names[index] == AttributeName.ID))
			{
				return "CDATA";
			}
			return "ID";
		}

		public AttributeName GetAttributeName(int index)
		{
			if (index < this.length && index >= 0)
			{
				return this.names[index];
			}
			return null;
		}

		[NsUri]
		public string GetURI(int index)
		{
			if (index < this.length && index >= 0)
			{
				return this.names[index].GetUri(this.mode);
			}
			return null;
		}

		[Prefix]
		public string GetPrefix(int index)
		{
			if (index < this.length && index >= 0)
			{
				return this.names[index].GetPrefix(this.mode);
			}
			return null;
		}

		public string GetValue(int index)
		{
			if (index < this.length && index >= 0)
			{
				return this.values[index];
			}
			return null;
		}

		public string GetValue(AttributeName name)
		{
			int index = this.GetIndex(name);
			if (index == -1)
			{
				return null;
			}
			return this.GetValue(index);
		}

		public string Id
		{
			get
			{
				return this.idValue;
			}
		}

		public int XmlnsLength
		{
			get
			{
				return this.xmlnsLength;
			}
		}

		[Local]
		public string GetXmlnsLocalName(int index)
		{
			if (index < this.xmlnsLength && index >= 0)
			{
				return this.xmlnsNames[index].GetLocal(this.mode);
			}
			return null;
		}

		[NsUri]
		public string GetXmlnsURI(int index)
		{
			if (index < this.xmlnsLength && index >= 0)
			{
				return this.xmlnsNames[index].GetUri(this.mode);
			}
			return null;
		}

		public string GetXmlnsValue(int index)
		{
			if (index < this.xmlnsLength && index >= 0)
			{
				return this.xmlnsValues[index];
			}
			return null;
		}

		public int GetXmlnsIndex(AttributeName name)
		{
			for (int i = 0; i < this.xmlnsLength; i++)
			{
				if (this.xmlnsNames[i] == name)
				{
					return i;
				}
			}
			return -1;
		}

		public string GetXmlnsValue(AttributeName name)
		{
			int xmlnsIndex = this.GetXmlnsIndex(name);
			if (xmlnsIndex == -1)
			{
				return null;
			}
			return this.GetXmlnsValue(xmlnsIndex);
		}

		public AttributeName GetXmlnsAttributeName(int index)
		{
			if (index < this.xmlnsLength && index >= 0)
			{
				return this.xmlnsNames[index];
			}
			return null;
		}

		internal void AddAttribute(AttributeName name, string value, XmlViolationPolicy xmlnsPolicy)
		{
			if (name == AttributeName.ID)
			{
				this.idValue = value;
			}
			if (name.IsXmlns)
			{
				if (this.xmlnsNames.Length == this.xmlnsLength)
				{
					int num = ((this.xmlnsLength == 0) ? 2 : (this.xmlnsLength << 1));
					AttributeName[] destinationArray = new AttributeName[num];
					Array.Copy(this.xmlnsNames, destinationArray, this.xmlnsNames.Length);
					this.xmlnsNames = destinationArray;
					string[] destinationArray2 = new string[num];
					Array.Copy(this.xmlnsValues, destinationArray2, this.xmlnsValues.Length);
					this.xmlnsValues = destinationArray2;
				}
				this.xmlnsNames[this.xmlnsLength] = name;
				this.xmlnsValues[this.xmlnsLength] = value;
				this.xmlnsLength++;
				switch (xmlnsPolicy)
				{
				case XmlViolationPolicy.Fatal:
					throw new Exception("Saw an xmlns attribute.");
				case XmlViolationPolicy.AlterInfoset:
					return;
				}
			}
			if (this.names.Length == this.length)
			{
				int num2 = this.length << 1;
				AttributeName[] destinationArray3 = new AttributeName[num2];
				Array.Copy(this.names, destinationArray3, this.names.Length);
				this.names = destinationArray3;
				string[] destinationArray4 = new string[num2];
				Array.Copy(this.values, destinationArray4, this.values.Length);
				this.values = destinationArray4;
			}
			this.names[this.length] = name;
			this.values[this.length] = value;
			this.length++;
		}

		internal void Clear(int m)
		{
			for (int i = 0; i < this.length; i++)
			{
				this.names[i] = null;
				this.values[i] = null;
			}
			this.length = 0;
			this.mode = m;
			this.idValue = null;
			for (int j = 0; j < this.xmlnsLength; j++)
			{
				this.xmlnsNames[j] = null;
				this.xmlnsValues[j] = null;
			}
			this.xmlnsLength = 0;
		}

		internal void ClearWithoutReleasingContents()
		{
			for (int i = 0; i < this.length; i++)
			{
				this.names[i] = null;
				this.values[i] = null;
			}
			this.length = 0;
		}

		public bool Contains(AttributeName name)
		{
			for (int i = 0; i < this.length; i++)
			{
				if (name.Equals(this.names[i]))
				{
					return true;
				}
			}
			for (int j = 0; j < this.xmlnsLength; j++)
			{
				if (name.Equals(this.xmlnsNames[j]))
				{
					return true;
				}
			}
			return false;
		}

		public void AdjustForMath()
		{
			this.mode = 1;
		}

		public void AdjustForSvg()
		{
			this.mode = 2;
		}

		public HtmlAttributes CloneAttributes()
		{
			HtmlAttributes htmlAttributes = new HtmlAttributes(0);
			for (int i = 0; i < this.length; i++)
			{
				htmlAttributes.AddAttribute(this.names[i].CloneAttributeName(), this.values[i], XmlViolationPolicy.Allow);
			}
			for (int j = 0; j < this.xmlnsLength; j++)
			{
				htmlAttributes.AddAttribute(this.xmlnsNames[j], this.xmlnsValues[j], XmlViolationPolicy.Allow);
			}
			return htmlAttributes;
		}

		public bool Equals(HtmlAttributes other)
		{
			int num = other.Length;
			if (this.length != num)
			{
				return false;
			}
			for (int i = 0; i < this.length; i++)
			{
				bool flag = false;
				string local = this.names[i].GetLocal(0);
				for (int j = 0; j < num; j++)
				{
					if (local == other.names[j].GetLocal(0))
					{
						flag = true;
						if (this.values[i] != other.values[j])
						{
							return false;
						}
					}
				}
				if (!flag)
				{
					return false;
				}
			}
			return true;
		}

		internal void ProcessNonNcNames<T>(TreeBuilder<T> treeBuilder, XmlViolationPolicy namePolicy) where T : class
		{
			for (int i = 0; i < this.length; i++)
			{
				AttributeName attributeName = this.names[i];
				if (!attributeName.IsNcName(this.mode))
				{
					string local = attributeName.GetLocal(this.mode);
					switch (namePolicy)
					{
					case XmlViolationPolicy.Allow:
						break;
					case XmlViolationPolicy.Fatal:
						treeBuilder.Fatal("Attribute “" + local + "” is not serializable as XML 1.0.");
						goto IL_8F;
					case XmlViolationPolicy.AlterInfoset:
						this.names[i] = AttributeName.Create(NCName.EscapeName(local));
						break;
					default:
						goto IL_8F;
					}
					if (attributeName != AttributeName.XML_LANG)
					{
						treeBuilder.Warn("Attribute “" + local + "” is not serializable as XML 1.0.");
					}
				}
				IL_8F:;
			}
		}

		public void Merge(HtmlAttributes attributes)
		{
			int num = attributes.Length;
			for (int i = 0; i < num; i++)
			{
				AttributeName attributeName = attributes.GetAttributeName(i);
				if (!this.Contains(attributeName))
				{
					this.AddAttribute(attributeName, attributes.GetValue(i), XmlViolationPolicy.Allow);
				}
			}
		}

		static readonly AttributeName[] EMPTY_ATTRIBUTENAMES = new AttributeName[0];

		static readonly string[] EMPTY_stringS = new string[0];

		public static readonly HtmlAttributes EMPTY_ATTRIBUTES = new HtmlAttributes(0);

		int mode;

		int length;

		AttributeName[] names;

		string[] values;

		string idValue;

		int xmlnsLength;

		AttributeName[] xmlnsNames;

		string[] xmlnsValues;
	}
}
