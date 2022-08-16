using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Media;
using Telerik.Windows.Documents.Media;
using Telerik.Windows.Documents.Spreadsheet.Core;
using Telerik.Windows.Documents.Spreadsheet.Model;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.Theming
{
	public class ThemeColorScheme : NamedObjectBase, IEnumerable<ThemeColor>, IEnumerable
	{
		public ThemeColorScheme(string name, Color background1, Color text1, Color background2, Color text2, Color accent1, Color accent2, Color accent3, Color accent4, Color accent5, Color accent6, Color hyperlink, Color followedHyperlink)
			: base(name)
		{
			this.themeColorTypeToColor = new Dictionary<ThemeColorType, ThemeColor>();
			this.AddThemeColor(ThemeColorType.Background1, background1);
			this.AddThemeColor(ThemeColorType.Text1, text1);
			this.AddThemeColor(ThemeColorType.Background2, background2);
			this.AddThemeColor(ThemeColorType.Text2, text2);
			this.AddThemeColor(ThemeColorType.Accent1, accent1);
			this.AddThemeColor(ThemeColorType.Accent2, accent2);
			this.AddThemeColor(ThemeColorType.Accent3, accent3);
			this.AddThemeColor(ThemeColorType.Accent4, accent4);
			this.AddThemeColor(ThemeColorType.Accent5, accent5);
			this.AddThemeColor(ThemeColorType.Accent6, accent6);
			this.AddThemeColor(ThemeColorType.Hyperlink, hyperlink);
			this.AddThemeColor(ThemeColorType.FollowedHyperlink, followedHyperlink);
		}

		internal ThemeColorScheme(string name, Dictionary<ThemeColorType, ThemeColor> themeColorTypeToColor)
			: base(name)
		{
			Guard.ThrowExceptionIfNull<Dictionary<ThemeColorType, ThemeColor>>(themeColorTypeToColor, "themeColorTypeToColor");
			this.themeColorTypeToColor = themeColorTypeToColor;
		}

		public ThemeColor this[ThemeColorType colorType]
		{
			get
			{
				return this.themeColorTypeToColor[colorType];
			}
		}

		public double GetTintAndShade(ThemeColorType themeColorType, ColorShadeType colorShadeType)
		{
			Color color = this[themeColorType].Color;
			return ColorsHelper.GetTintAndShadeForColorAndIndex(color, colorShadeType);
		}

		public ThemeColorScheme Clone()
		{
			Dictionary<ThemeColorType, ThemeColor> dictionary = new Dictionary<ThemeColorType, ThemeColor>();
			foreach (KeyValuePair<ThemeColorType, ThemeColor> keyValuePair in this.themeColorTypeToColor)
			{
				dictionary.Add(keyValuePair.Key, keyValuePair.Value.Clone());
			}
			return new ThemeColorScheme(base.Name, dictionary);
		}

		public override bool Equals(object obj)
		{
			ThemeColorScheme themeColorScheme = obj as ThemeColorScheme;
			if (themeColorScheme == null || base.Name != themeColorScheme.Name)
			{
				return false;
			}
			foreach (KeyValuePair<ThemeColorType, ThemeColor> keyValuePair in this.themeColorTypeToColor)
			{
				if (!ObjectExtensions.EqualsOfT<ThemeColor>(keyValuePair.Value, themeColorScheme[keyValuePair.Key]))
				{
					return false;
				}
			}
			return true;
		}

		public override int GetHashCode()
		{
			if (this.hashCode == null)
			{
				this.hashCode = new int?(base.Name.GetHashCode());
				foreach (KeyValuePair<ThemeColorType, ThemeColor> keyValuePair in this.themeColorTypeToColor)
				{
					this.hashCode = new int?(ObjectExtensions.CombineHashCodes(this.hashCode.Value, keyValuePair.Value.GetHashCode()));
				}
			}
			return this.hashCode.Value;
		}

		public IEnumerator<ThemeColor> GetEnumerator()
		{
			return this.themeColorTypeToColor.Values.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<ThemeColor>)this).GetEnumerator();
		}

		void AddThemeColor(ThemeColorType themeColorType, Color color)
		{
			this.themeColorTypeToColor.Add(themeColorType, new ThemeColor(color, themeColorType));
		}

		readonly Dictionary<ThemeColorType, ThemeColor> themeColorTypeToColor;

		int? hashCode;
	}
}
