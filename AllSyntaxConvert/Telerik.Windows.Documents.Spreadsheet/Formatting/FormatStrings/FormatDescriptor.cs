using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Media;

namespace Telerik.Windows.Documents.Spreadsheet.Formatting.FormatStrings
{
	class FormatDescriptor
	{
		public CultureInfo Culture { get; internal set; }

		public Color? Foreground { get; internal set; }

		public Predicate<double> Condition { get; internal set; }

		internal int ItemsCount
		{
			get
			{
				return this.items.Count;
			}
		}

		public IEnumerable<FormatDescriptorItem> Items
		{
			get
			{
				return this.items.AsEnumerable<FormatDescriptorItem>();
			}
		}

		internal string Text
		{
			get
			{
				StringBuilder stringBuilder = new StringBuilder();
				foreach (FormatDescriptorItem formatDescriptorItem in this.Items)
				{
					stringBuilder.Append(formatDescriptorItem.Text);
				}
				return stringBuilder.ToString();
			}
		}

		internal bool ShouldExpand
		{
			get
			{
				return this.Items.Any((FormatDescriptorItem fspi) => fspi.ShouldExpand);
			}
		}

		public FormatDescriptor()
		{
			this.Condition = FormatHelper.DefaultFormatCondition;
			this.Foreground = FormatHelper.DefaultFormatForeground;
			this.Culture = FormatHelper.DefaultFormatCulture;
			this.items = new List<FormatDescriptorItem>();
		}

		internal void AddItem(FormatDescriptorItem item)
		{
			this.items.Add(item);
		}

		readonly List<FormatDescriptorItem> items;
	}
}
