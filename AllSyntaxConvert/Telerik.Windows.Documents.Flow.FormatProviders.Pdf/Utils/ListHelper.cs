using System;
using System.Collections.Generic;
using System.Linq;
using Telerik.Windows.Documents.Fixed.Model.Editing.Lists;
using Telerik.Windows.Documents.Flow.Model.Lists;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Pdf.Utils
{
	class ListHelper
	{
		public ListHelper()
		{
			this.styleToConverter = new Dictionary<NumberingStyle, INumberingStyleConverter>();
			this.RegisterNumberingStyle(NumberingStyle.Decimal, new NumberingStyleConverter(new Func<int, string>(BulletNumberingFormats.GetNumber)));
			this.RegisterNumberingStyle(NumberingStyle.LowerLetter, new NumberingStyleConverter(new Func<int, string>(BulletNumberingFormats.GetLowerLetter)));
			this.RegisterNumberingStyle(NumberingStyle.LowerRoman, new NumberingStyleConverter(new Func<int, string>(BulletNumberingFormats.GetLowerRomanNumber)));
			this.RegisterNumberingStyle(NumberingStyle.UpperRoman, new NumberingStyleConverter(new Func<int, string>(BulletNumberingFormats.GetUpperRomanNumber)));
			this.RegisterNumberingStyle(NumberingStyle.UpperLetter, new NumberingStyleConverter(new Func<int, string>(BulletNumberingFormats.GetUpperLetter)));
		}

		public void RegisterNumberingStyle(NumberingStyle numberingStyle, INumberingStyleConverter converter)
		{
			Guard.ThrowExceptionIfNull<INumberingStyleConverter>(converter, "converter");
			this.styleToConverter[numberingStyle] = converter;
		}

		public IBulletNumberingFormat CreateBulletFormat(Telerik.Windows.Documents.Flow.Model.Lists.ListLevel listLevel)
		{
			NumberingStyle[] levelsNumberingStyles = ListHelper.GetNumberingStyles(listLevel.OwnerList).ToArray<NumberingStyle>();
			return new TextFormattedBullet(this, levelsNumberingStyles, listLevel.NumberTextFormat ?? string.Empty);
		}

		public string GetNumberText(NumberingStyle numberingStyle, int number)
		{
			INumberingStyleConverter numberingStyleConverter;
			if (!this.styleToConverter.TryGetValue(numberingStyle, out numberingStyleConverter))
			{
				numberingStyleConverter = ListHelper.emptyConverter;
			}
			return numberingStyleConverter.ConvertNumberToText(number);
		}

		static IEnumerable<NumberingStyle> GetNumberingStyles(Telerik.Windows.Documents.Flow.Model.Lists.List list)
		{
			foreach (Telerik.Windows.Documents.Flow.Model.Lists.ListLevel level in list.Levels)
			{
				yield return level.NumberingStyle;
			}
			yield break;
		}

		static readonly INumberingStyleConverter emptyConverter = new NumberingStyleConverter((int number) => string.Empty);

		readonly Dictionary<NumberingStyle, INumberingStyleConverter> styleToConverter;
	}
}
