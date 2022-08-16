using System;
using System.Collections;
using System.Collections.Generic;
using Telerik.Documents.SpreadsheetStreaming.Core;
using Telerik.Documents.SpreadsheetStreaming.ImportExport;
using Telerik.Documents.SpreadsheetStreaming.Utilities;

namespace Telerik.Documents.SpreadsheetStreaming.Model.Themes
{
	class SpreadThemeColorScheme : NamedObjectBase, IEnumerable<SpreadThemeColor>, IEnumerable
	{
		public SpreadThemeColorScheme(string name, SpreadColor background1, SpreadColor text1, SpreadColor background2, SpreadColor text2, SpreadColor accent1, SpreadColor accent2, SpreadColor accent3, SpreadColor accent4, SpreadColor accent5, SpreadColor accent6, SpreadColor hyperlink, SpreadColor followedHyperlink)
			: base(name)
		{
			this.themeColorTypeToColor = new Dictionary<SpreadThemeColorType, SpreadThemeColor>();
			this.AddThemeColor(SpreadThemeColorType.Light1, background1);
			this.AddThemeColor(SpreadThemeColorType.Dark1, text1);
			this.AddThemeColor(SpreadThemeColorType.Light2, background2);
			this.AddThemeColor(SpreadThemeColorType.Dark2, text2);
			this.AddThemeColor(SpreadThemeColorType.Accent1, accent1);
			this.AddThemeColor(SpreadThemeColorType.Accent2, accent2);
			this.AddThemeColor(SpreadThemeColorType.Accent3, accent3);
			this.AddThemeColor(SpreadThemeColorType.Accent4, accent4);
			this.AddThemeColor(SpreadThemeColorType.Accent5, accent5);
			this.AddThemeColor(SpreadThemeColorType.Accent6, accent6);
			this.AddThemeColor(SpreadThemeColorType.Hyperlink, hyperlink);
			this.AddThemeColor(SpreadThemeColorType.FollowedHyperlink, followedHyperlink);
		}

		public SpreadThemeColor this[SpreadThemeColorType colorType]
		{
			get
			{
				return this.themeColorTypeToColor[colorType];
			}
		}

		public double GetTintAndShade(SpreadThemeColorType themeColorType, SpreadColorShadeType colorShadeType)
		{
			SpreadColor color = this[themeColorType].Color;
			return ColorsHelper.GetTintAndShadeForColorAndIndex(color, colorShadeType);
		}

		public override bool Equals(object obj)
		{
			SpreadThemeColorScheme spreadThemeColorScheme = obj as SpreadThemeColorScheme;
			if (spreadThemeColorScheme == null || base.Name != spreadThemeColorScheme.Name)
			{
				return false;
			}
			foreach (KeyValuePair<SpreadThemeColorType, SpreadThemeColor> keyValuePair in this.themeColorTypeToColor)
			{
				if (!ObjectExtensions.EqualsOfT<SpreadThemeColor>(keyValuePair.Value, spreadThemeColorScheme[keyValuePair.Key]))
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
				foreach (KeyValuePair<SpreadThemeColorType, SpreadThemeColor> keyValuePair in this.themeColorTypeToColor)
				{
					this.hashCode = new int?(ObjectExtensions.CombineHashCodes(this.hashCode.Value, keyValuePair.Value.GetHashCode()));
				}
			}
			return this.hashCode.Value;
		}

		public IEnumerator<SpreadThemeColor> GetEnumerator()
		{
			return this.themeColorTypeToColor.Values.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<SpreadThemeColor>)this).GetEnumerator();
		}

		void AddThemeColor(SpreadThemeColorType themeColorType, SpreadColor color)
		{
			this.themeColorTypeToColor.Add(themeColorType, new SpreadThemeColor(color, themeColorType));
		}

		readonly Dictionary<SpreadThemeColorType, SpreadThemeColor> themeColorTypeToColor;

		int? hashCode;
	}
}
