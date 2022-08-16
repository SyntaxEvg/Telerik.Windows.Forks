using System;
using Telerik.Windows.Documents.Flow.Model.Styles;

namespace Telerik.Windows.Documents.Flow.Model.Lists
{
	public sealed class List
	{
		public List()
		{
			this.levels = new ListLevelCollection(this);
			this.multilevelType = MultilevelType.HybridMultilevel;
			this.id = -1;
		}

		public int Id
		{
			get
			{
				return this.id;
			}
			internal set
			{
				this.id = value;
			}
		}

		public string StyleId
		{
			get
			{
				return this.styleId;
			}
			set
			{
				this.CheckIsValidStyle(value);
				this.styleId = value;
				if (this.Id > -1 && this.Document != null)
				{
					Style style = this.Document.StyleRepository.GetStyle(this.styleId);
					if (style != null)
					{
						style.ParagraphProperties.ListId.LocalValue = new int?(this.Id);
					}
				}
			}
		}

		public ListLevelCollection Levels
		{
			get
			{
				return this.levels;
			}
		}

		public RadFlowDocument Document { get; internal set; }

		public MultilevelType MultilevelType
		{
			get
			{
				return this.multilevelType;
			}
			set
			{
				this.multilevelType = value;
			}
		}

		internal string Name { get; set; }

		public List Clone()
		{
			List list = new List();
			list.MultilevelType = this.MultilevelType;
			list.Id = this.Id;
			list.StyleId = this.StyleId;
			list.Name = this.Name;
			for (int i = 0; i < this.Levels.Count; i++)
			{
				list.Levels[i] = this.Levels[i].Clone(list);
			}
			return list;
		}

		void CheckIsValidStyle(string value)
		{
			if (string.IsNullOrEmpty(value) || this.Document == null)
			{
				return;
			}
			Style style = this.Document.StyleRepository.GetStyle(value);
			if (style == null)
			{
				return;
			}
			if (style.StyleType != StyleType.Numbering)
			{
				throw new ArgumentException(string.Format("Style of type {0} can not be assigned to list.", style.StyleType));
			}
		}

		readonly ListLevelCollection levels;

		MultilevelType multilevelType;

		int id;

		string styleId;
	}
}
