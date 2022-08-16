using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using CsQuery.ExtensionMethods.Internal;
using CsQuery.HtmlParser;
using CsQuery.StringScanner;
using CsQuery.Utility;

namespace CsQuery.Implementation
{
	class CSSStyleDeclaration : IDictionary<string, string>, ICollection<KeyValuePair<string, string>>, IEnumerable<KeyValuePair<string, string>>, IEnumerable, ICSSStyleDeclaration
	{
		public CSSStyleDeclaration()
		{
		}

		public CSSStyleDeclaration(string cssText)
		{
			this.SetStyles(cssText, true);
		}

		public CSSStyleDeclaration(string cssText, bool validate)
		{
			this.SetStyles(cssText, validate);
		}

		public CSSStyleDeclaration(ICSSRule parentRule)
		{
			this.ParentRule = parentRule;
		}

		protected IDictionary<ushort, string> Styles
		{
			get
			{
				if (this._Styles == null)
				{
					this._Styles = new Dictionary<ushort, string>();
					if (this.QuickSetValue != null)
					{
						this.AddStyles(this.QuickSetValue, false);
						this.QuickSetValue = null;
					}
				}
				return this._Styles;
			}
			set
			{
				this._Styles = value;
			}
		}

		string QuickSetValue
		{
			get
			{
				return this._QuickSetValue;
			}
			set
			{
				this._QuickSetValue = value;
				this.DoOnHasStyleAttributeChanged(this.HasStyleAttribute);
			}
		}

		public ICSSRule ParentRule { get; protected set; }

		public event EventHandler<CSSStyleChangedArgs> OnHasStylesChanged;

		public int Length
		{
			get
			{
				if (!this.HasStyleAttribute)
				{
					return 0;
				}
				return this.Styles.Count;
			}
		}

		public string CssText
		{
			get
			{
				return this.ToString();
			}
			set
			{
				this.SetStyles(value);
			}
		}

		public bool HasStyles
		{
			get
			{
				return this.HasStyleAttribute && this.Styles.Count > 0;
			}
		}

		public bool HasStyleAttribute
		{
			get
			{
				return this.QuickSetValue != null || this._Styles != null;
			}
		}

		public int Count
		{
			get
			{
				if (this.HasStyleAttribute)
				{
					return this.Styles.Count;
				}
				return 0;
			}
		}

		public bool IsReadOnly
		{
			get
			{
				return false;
			}
		}

		public ICollection<string> Keys
		{
			get
			{
				List<string> list = new List<string>();
				if (this.HasStyleAttribute)
				{
					foreach (KeyValuePair<ushort, string> keyValuePair in this.Styles)
					{
						list.Add(HtmlData.TokenName(keyValuePair.Key));
					}
				}
				return list;
			}
		}

		public ICollection<string> Values
		{
			get
			{
				if (this.HasStyleAttribute)
				{
					return this.Styles.Values;
				}
				return new List<string>();
			}
		}

		public string this[string name]
		{
			get
			{
				return this.GetStyle(name);
			}
			set
			{
				this.SetStyle(name, value, true);
			}
		}

		public string this[string name, bool strict]
		{
			set
			{
				this.SetStyle(name, value, strict);
			}
		}

		public string Height
		{
			get
			{
				return this.GetStyle("height");
			}
			set
			{
				this.SetStyle("height", value, true);
			}
		}

		public string Width
		{
			get
			{
				return this.GetStyle("width");
			}
			set
			{
				this.SetStyle("width", value, true);
			}
		}

		public CSSStyleDeclaration Clone()
		{
			CSSStyleDeclaration cssstyleDeclaration = new CSSStyleDeclaration();
			if (this.QuickSetValue != null)
			{
				cssstyleDeclaration.QuickSetValue = this.QuickSetValue;
			}
			else
			{
				IDictionary<ushort, string> dictionary = new Dictionary<ushort, string>();
				foreach (KeyValuePair<ushort, string> item in this.Styles)
				{
					dictionary.Add(item);
				}
				cssstyleDeclaration.Styles = dictionary;
			}
			return cssstyleDeclaration;
		}

		public void SetStyles(string styles)
		{
			this.SetStyles(styles, true);
		}

		public void SetStyles(string styles, bool strict)
		{
			this._Styles = null;
			if (!strict)
			{
				this.QuickSetValue = styles;
				return;
			}
			this.AddStyles(styles, strict);
		}

		public void AddStyles(string styles, bool strict)
		{
			foreach (string text in styles.SplitClean(';'))
			{
				int num = text.IndexOf(":");
				if (num > 0)
				{
					string name = text.Substring(0, num).Trim();
					string value = text.Substring(num + 1).Trim();
					if (!strict)
					{
						this.SetRaw(name, value);
					}
					else
					{
						this.Add(name, value);
					}
				}
			}
		}

		public bool Remove(string name)
		{
			return this.HasStyleAttribute && this.Styles.Remove(HtmlData.Tokenize(name));
		}

		public bool RemoveStyle(string name)
		{
			return this.Remove(name);
		}

		public void Add(string name, string value)
		{
			this.SetStyle(name, value, true);
		}

		public void Clear()
		{
			if (this.HasStyleAttribute)
			{
				this.Styles.Clear();
			}
		}

		public bool HasStyle(string styleName)
		{
			return this.HasStyleAttribute && this.Styles.ContainsKey(HtmlData.Tokenize(styleName));
		}

		public void SetRaw(string name, string value)
		{
			bool hasStyleAttribute = this.HasStyleAttribute;
			this.Styles[HtmlData.Tokenize(name)] = value;
			this.DoOnHasStyleAttributeChanged(hasStyleAttribute);
		}

		public bool TryGetValue(string name, out string value)
		{
			if (this.HasStyleAttribute)
			{
				return this.Styles.TryGetValue(HtmlData.Tokenize(name), out value);
			}
			value = null;
			return false;
		}

		public string GetStyle(string name)
		{
			string result = null;
			if (this.HasStyleAttribute)
			{
				this.Styles.TryGetValue(HtmlData.Tokenize(name), out result);
			}
			return result;
		}

		public void SetStyle(string name, string value)
		{
			this.SetStyle(name, value, true);
		}

		public void SetStyle(string name, string value, bool strict)
		{
			name = Support.FromCamelCase(name);
			if (value == null)
			{
				this.Remove(name);
				return;
			}
			value = value.Trim().Replace(";", string.Empty);
			name = name.Trim();
			CssStyle cssStyle = null;
			if (!HtmlStyles.StyleDefs.TryGetValue(name, out cssStyle))
			{
				if (strict)
				{
					throw new ArgumentException("The style '" + name + "' is not valid (strict mode)");
				}
			}
			else
			{
				switch (cssStyle.Type)
				{
				case CSSStyleType.Unit:
					value = this.ValidateUnitString(name, value);
					goto IL_10D;
				case CSSStyleType.Option:
					break;
				case CSSStyleType.UnitOption:
					if (cssStyle.Options.Contains(value))
					{
						goto IL_10D;
					}
					try
					{
						value = this.ValidateUnitString(name, value);
						goto IL_10D;
					}
					catch
					{
						throw new ArgumentException("No valid unit data or option provided for attribue '" + name + "'. Valid options are: " + this.OptionList(cssStyle));
					}
					break;
				default:
					goto IL_10D;
				}
				if (!cssStyle.Options.Contains(value))
				{
					throw new ArgumentException(string.Concat(new string[]
					{
						"The value '",
						value,
						"' is not allowed for attribute '",
						name,
						"'. Valid options are: ",
						this.OptionList(cssStyle)
					}));
				}
			}
			IL_10D:
			this.SetRaw(name, value);
		}

		public double? NumberPart(string style)
		{
			string style2 = this.GetStyle(style);
			if (style2 == null)
			{
				return null;
			}
			IStringScanner stringScanner = Scanner.Create(style2);
			string s;
			double value;
			if (stringScanner.TryGet(MatchFunctions.Number(false), out s) && double.TryParse(s, out value))
			{
				return new double?(value);
			}
			return null;
		}

		public override string ToString()
		{
			string text = (this.HasStyleAttribute ? "" : null);
			if (this.HasStyleAttribute)
			{
				if (this.QuickSetValue != null)
				{
					return this.QuickSetValue;
				}
				bool flag = true;
				foreach (KeyValuePair<ushort, string> keyValuePair in this.Styles)
				{
					if (!flag)
					{
						text += " ";
					}
					else
					{
						flag = false;
					}
					string text2 = text;
					text = string.Concat(new string[]
					{
						text2,
						HtmlData.TokenName(keyValuePair.Key),
						": ",
						keyValuePair.Value,
						";"
					});
				}
			}
			return text;
		}

		public IEnumerator<KeyValuePair<string, string>> GetEnumerator()
		{
			return this.stylesEnumerable().GetEnumerator();
		}

		protected string OptionList(CssStyle style)
		{
			string text = "";
			foreach (string text2 in style.Options)
			{
				string text3 = text;
				text = string.Concat(new string[]
				{
					text3,
					(text == string.Empty) ? string.Empty : ",",
					"'",
					text2,
					"'"
				});
			}
			return text;
		}

		protected string ValidateUnitString(string name, string value)
		{
			int i = 0;
			value = value.Trim();
			StringBuilder stringBuilder = new StringBuilder();
			string value2 = ((name == "opacity") ? "" : "px");
			if (string.IsNullOrEmpty(value))
			{
				return null;
			}
			int length = value.Length;
			while (i < length)
			{
				char c = value[i];
				if (HtmlData.NumberChars.Contains(c))
				{
					stringBuilder.Append(c);
					i++;
				}
				else
				{
					IL_76:
					while (i > length && value[i] == ' ')
					{
						i++;
					}
					string text = value.Substring(i).Trim();
					if (text != string.Empty)
					{
						if (!HtmlData.Units.Contains(text))
						{
							throw new ArgumentException("Invalid unit data type for attribute, data: '" + value + "'");
						}
						value2 = text;
					}
					if (stringBuilder.Length == 0)
					{
						throw new ArgumentException("No data provided for attribute, data: '" + value + "'");
					}
					stringBuilder.Append(value2);
					return stringBuilder.ToString();
				}
			}
			goto IL_76;
		}

		IEnumerable<KeyValuePair<string, string>> stylesEnumerable()
		{
			if (this.HasStyleAttribute)
			{
				foreach (KeyValuePair<ushort, string> kvp in this.Styles)
				{
					KeyValuePair<ushort, string> keyValuePair = kvp;
					string key = HtmlData.TokenName(keyValuePair.Key).ToLower();
					KeyValuePair<ushort, string> keyValuePair2 = kvp;
					yield return new KeyValuePair<string, string>(key, keyValuePair2.Value);
				}
			}
			yield break;
		}

		void DoOnHasStyleAttributeChanged(bool hadStyleAttribute)
		{
			if (hadStyleAttribute != this.HasStyleAttribute)
			{
				EventHandler<CSSStyleChangedArgs> onHasStylesChanged = this.OnHasStylesChanged;
				if (onHasStylesChanged != null)
				{
					CSSStyleChangedArgs e = new CSSStyleChangedArgs(this.HasStyleAttribute);
					onHasStylesChanged(this, e);
				}
			}
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		bool IDictionary<string, string>.ContainsKey(string key)
		{
			return this.HasStyleAttribute && this.Styles.ContainsKey(HtmlData.Tokenize(key));
		}

		void ICollection<KeyValuePair<string, string>>.Add(KeyValuePair<string, string> item)
		{
			this.Add(item.Key, item.Value);
		}

		bool ICollection<KeyValuePair<string, string>>.Contains(KeyValuePair<string, string> item)
		{
			return this.HasStyleAttribute && this.Styles.Contains(new KeyValuePair<ushort, string>(HtmlData.Tokenize(item.Key), item.Value));
		}

		void ICollection<KeyValuePair<string, string>>.CopyTo(KeyValuePair<string, string>[] array, int arrayIndex)
		{
			if (this.HasStyleAttribute && this.HasStyles)
			{
				array = new KeyValuePair<string, string>[this.Styles.Count];
				int num = 0;
				foreach (KeyValuePair<ushort, string> keyValuePair in this.Styles)
				{
					array[num++] = new KeyValuePair<string, string>(HtmlData.TokenName(keyValuePair.Key).ToLower(), keyValuePair.Value);
				}
			}
		}

		bool ICollection<KeyValuePair<string, string>>.Remove(KeyValuePair<string, string> item)
		{
			if (this.HasStyleAttribute)
			{
				KeyValuePair<ushort, string> item2 = new KeyValuePair<ushort, string>(HtmlData.Tokenize(item.Key), item.Value);
				return this.Styles.Remove(item2);
			}
			return false;
		}

		IEnumerator<KeyValuePair<string, string>> IEnumerable<KeyValuePair<string, string>>.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		IDictionary<ushort, string> _Styles;

		string _QuickSetValue;
	}
}
