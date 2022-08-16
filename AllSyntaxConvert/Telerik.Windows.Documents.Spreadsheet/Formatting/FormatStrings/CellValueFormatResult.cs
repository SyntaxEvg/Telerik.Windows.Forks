using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Media;
using Telerik.Windows.Documents.Spreadsheet.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.Formatting.FormatStrings
{
	public class CellValueFormatResult
	{
		public CultureInfo Culture
		{
			get
			{
				return this.culture;
			}
		}

		public Color? Foreground
		{
			get
			{
				return this.foreground;
			}
		}

		public Predicate<double> Condition
		{
			get
			{
				return this.condition;
			}
		}

		public IEnumerable<CellValueFormatResultItem> Infos
		{
			get
			{
				return this.infos;
			}
		}

		public string InfosText
		{
			get
			{
				return this.infosText;
			}
		}

		public string VisibleInfosText
		{
			get
			{
				return this.visibleInfosText;
			}
		}

		internal string ExtendedVisibleInfosText
		{
			get
			{
				return this.extendedVisibleInfosText;
			}
		}

		internal bool ContainsExpandableInfo
		{
			get
			{
				return this.containsExpandableInfo;
			}
		}

		public CellValueFormatResult(IEnumerable<CellValueFormatResultItem> infos, Color? foreground, Predicate<double> condition, CultureInfo culture)
		{
			Guard.ThrowExceptionIfNullOrEmpty<CellValueFormatResultItem>(infos, "infos");
			Guard.ThrowExceptionIfContainsNull<CellValueFormatResultItem>(infos, "infos");
			this.infos = new List<CellValueFormatResultItem>(infos);
			this.infosText = infos.Aggregate(string.Empty, (string res, CellValueFormatResultItem next) => res + next.Text);
			this.visibleInfosText = infos.Aggregate(string.Empty, (string res, CellValueFormatResultItem next) => res + ((!next.IsTransparent) ? next.Text : string.Empty));
			this.extendedVisibleInfosText = infos.Aggregate(string.Empty, delegate(string res, CellValueFormatResultItem next)
			{
				if (next.IsTransparent)
				{
					return res + " ";
				}
				if (next.ShouldExpand)
				{
					return res;
				}
				return res + next.Text;
			});
			this.containsExpandableInfo = infos.Any((CellValueFormatResultItem fspi) => fspi.ShouldExpand);
			this.foreground = foreground;
			this.condition = condition;
			this.culture = culture;
		}

		public CellValueFormatResult(IEnumerable<CellValueFormatResultItem> infos)
			: this(infos, null, null, null)
		{
		}

		readonly CultureInfo culture;

		readonly Color? foreground;

		readonly Predicate<double> condition;

		readonly IEnumerable<CellValueFormatResultItem> infos;

		readonly string infosText;

		readonly string visibleInfosText;

		readonly string extendedVisibleInfosText;

		readonly bool containsExpandableInfo;
	}
}
