using System;
using System.Collections.Generic;
using System.Linq;
using Telerik.Windows.Documents.Flow.Model.Lists;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Html.Model.Elements.Lists
{
	class HtmlListManager
	{
		public HtmlListManager()
		{
			this.bulletListInfos = new List<BulletListInfo>();
			this.Initialize();
		}

		internal ListIndexes ListIndexes
		{
			get
			{
				return this.listIndexes;
			}
			set
			{
				this.listIndexes = value;
			}
		}

		internal BulletListInfo GetBulletListInfoBySymbol(string bulletSymbol)
		{
			return (from b in this.bulletListInfos
				where b.BulletSymbol == bulletSymbol
				select b).FirstOrDefault<BulletListInfo>();
		}

		internal BulletListInfo GetBulletListInfoByHtmlName(string htmlName)
		{
			return (from b in this.bulletListInfos
				where b.HtmlName == htmlName
				select b).FirstOrDefault<BulletListInfo>();
		}

		void Initialize()
		{
			this.bulletListInfos.Add(new BulletListInfo("disc", ListFactory.BulletSymbols[0], ListFactory.BulletsFontFamily[0]));
			this.bulletListInfos.Add(new BulletListInfo("circle", ListFactory.BulletSymbols[1], ListFactory.BulletsFontFamily[1]));
			this.bulletListInfos.Add(new BulletListInfo("square", ListFactory.BulletSymbols[2], ListFactory.BulletsFontFamily[2]));
		}

		readonly List<BulletListInfo> bulletListInfos;

		ListIndexes listIndexes;
	}
}
