using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace PreMailer.Net
{
	class CssParser
	{
		public SortedList<string, StyleClass> Styles
		{
			get
			{
				return this._scc;
			}
			set
			{
				this._scc = value;
			}
		}

		public CssParser()
		{
			this._styleSheets = new List<string>();
			this._scc = new SortedList<string, StyleClass>();
		}

		public void AddStyleSheet(string styleSheetContent)
		{
			this._styleSheets.Add(styleSheetContent);
			this.ProcessStyleSheet(styleSheetContent);
		}

		public string GetStyleSheet(int index)
		{
			return this._styleSheets[index];
		}

		public StyleClass ParseStyleClass(string className, string style)
		{
			StyleClass styleClass = new StyleClass
			{
				Name = className
			};
			this.FillStyleClass(styleClass, className, style);
			return styleClass;
		}

		void ProcessStyleSheet(string styleSheetContent)
		{
			string text = this.CleanUp(styleSheetContent);
			string[] array = text.Split(new char[] { '}' });
			foreach (string s in array)
			{
				if (this.CleanUp(s).IndexOf('{') > -1)
				{
					this.FillStyleClassFromBlock(s);
				}
			}
		}

		void FillStyleClassFromBlock(string s)
		{
			string[] array = s.Split(new char[] { '{' });
			string text = this.CleanUp(array[0]).Trim();
			IEnumerable<string> enumerable = from x in text.Split(new char[] { ',' })
				select x.Trim();
			foreach (string text2 in enumerable)
			{
				StyleClass styleClass;
				if (this._scc.ContainsKey(text2))
				{
					styleClass = this._scc[text2];
					this._scc.Remove(text2);
				}
				else
				{
					styleClass = new StyleClass();
				}
				this.FillStyleClass(styleClass, text2, array[1]);
				this._scc.Add(styleClass.Name, styleClass);
			}
		}

		void FillStyleClass(StyleClass sc, string styleName, string style)
		{
			sc.Name = styleName;
			string[] array = this.CleanUp(style).Split(new char[] { ';' });
			foreach (string text in array)
			{
				if (text.Contains(":"))
				{
					string key = text.Split(new char[] { ':' })[0].Trim();
					if (sc.Attributes.ContainsKey(key))
					{
						sc.Attributes.Remove(key);
					}
					sc.Attributes.Add(key, text.Split(new char[] { ':' })[1].Trim().ToLower());
				}
			}
		}

		string CleanUp(string s)
		{
			Regex regex = new Regex("(/\\*(.|[\r\n])*?\\*/)|(//.*)");
			string text = regex.Replace(s, "");
			return text.Replace("\r", "").Replace("\n", "");
		}

		readonly List<string> _styleSheets;

		SortedList<string, StyleClass> _scc;
	}
}
