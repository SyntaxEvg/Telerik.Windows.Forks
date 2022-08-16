using System;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Html.Model.Styles
{
	class HtmlStylePropertyDescriptor
	{
		public HtmlStylePropertyDescriptor(string name, bool isInheritable)
			: this(name, isInheritable, false, false)
		{
		}

		public HtmlStylePropertyDescriptor(string name, bool isInheritable, bool applyAsLocalValueIfRelative = false, bool isBubblingInheritable = false)
		{
			Guard.ThrowExceptionIfNullOrEmpty(name, "name");
			this.name = name;
			this.isInheritable = isInheritable;
			this.applyAsLocalValueIfRelative = applyAsLocalValueIfRelative;
			this.isBubblingInheritable = isBubblingInheritable;
		}

		public string Name
		{
			get
			{
				return this.name;
			}
		}

		public bool IsInheritable
		{
			get
			{
				return this.isInheritable;
			}
		}

		public bool ApplyAsLocalValueIfRelative
		{
			get
			{
				return this.applyAsLocalValueIfRelative;
			}
		}

		public bool IsBubblingInheritable
		{
			get
			{
				return this.isBubblingInheritable;
			}
		}

		readonly string name;

		readonly bool isInheritable;

		readonly bool applyAsLocalValueIfRelative;

		readonly bool isBubblingInheritable;
	}
}
