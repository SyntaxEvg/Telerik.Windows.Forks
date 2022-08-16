using System;
using System.Collections.Generic;
using System.Linq;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Html.Model.Styles
{
	class HtmlStyleRepository
	{
		public HtmlStyleRepository()
		{
			this.styles = new Dictionary<string, List<Selector>>();
		}

		public Selector DefaultParagraphStyle { get; set; }

		public Selector DefaultCharacterStyle { get; set; }

		public void RegisterStyle(string styleId, Selector style)
		{
			Guard.ThrowExceptionIfNullOrEmpty(styleId, "styleId");
			Guard.ThrowExceptionIfNull<Selector>(style, "style");
			if (!this.styles.ContainsKey(styleId))
			{
				this.styles.Add(styleId, new List<Selector>());
			}
			this.styles[styleId].Add(style);
		}

		public bool ContainsStyle(string styleId, string elementType)
		{
			return this.styles.ContainsKey(styleId) && (from s in this.styles[styleId]
				where s.ForElementType == elementType
				select s).FirstOrDefault<Selector>() != null;
		}

		public bool ContainsStyle(string styleId)
		{
			return this.styles.ContainsKey(styleId) && this.styles[styleId].Any<Selector>();
		}

		public Selector GetStyle(string styleId)
		{
			Guard.ThrowExceptionIfNullOrEmpty(styleId, "styleId");
			return this.styles[styleId].FirstOrDefault<Selector>();
		}

		public Selector GetStyle(string styleId, string elementType)
		{
			Guard.ThrowExceptionIfNullOrEmpty(styleId, "styleId");
			return (from s in this.styles[styleId]
				where s.ForElementType == elementType
				select s).FirstOrDefault<Selector>();
		}

		public bool TryGetStyle(string styleId, out Selector selector)
		{
			Guard.ThrowExceptionIfNullOrEmpty(styleId, "styleId");
			if (this.styles.ContainsKey(styleId))
			{
				selector = this.styles[styleId].FirstOrDefault<Selector>();
				return true;
			}
			selector = null;
			return false;
		}

		public IEnumerable<Selector> GetStyles()
		{
			foreach (List<Selector> list in this.styles.Values)
			{
				foreach (Selector style in list)
				{
					yield return style;
				}
			}
			yield break;
		}

		readonly Dictionary<string, List<Selector>> styles;
	}
}
