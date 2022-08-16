using System;
using System.Linq;
using CsQuery;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Html.Import
{
	class StyleNamesResolver
	{
		public StyleNamesResolver(CQ cq)
		{
			Guard.ThrowExceptionIfNull<CQ>(cq, "cq");
			this.cq = cq;
		}

		public StyleNamesInfo Resolve()
		{
			StyleNamesInfo styleNamesInfo = new StyleNamesInfo();
			styleNamesInfo.CharacterStyles = (from s in this.cq.Select("span[class]")
				select s.Classes.First<string>()).Distinct<string>().ToList<string>();
			styleNamesInfo.ParagraphStyles = (from p in this.cq.Select("p[class]")
				select p.Classes.First<string>()).Distinct<string>().ToList<string>();
			styleNamesInfo.TableStyles = (from t in this.cq.Select("table[class]")
				select t.Classes.First<string>()).Distinct<string>().ToList<string>();
			return styleNamesInfo;
		}

		readonly CQ cq;
	}
}
