using System;

namespace Telerik.Windows.Documents.Flow.Model.Collections
{
	public sealed class SectionCollection : DocumentElementCollection<global::Telerik.Windows.Documents.Flow.Model.Section, global::Telerik.Windows.Documents.Flow.Model.RadFlowDocument>
	{
		internal SectionCollection(global::Telerik.Windows.Documents.Flow.Model.RadFlowDocument parent)
			: base(parent)
		{
		}

		public global::Telerik.Windows.Documents.Flow.Model.Section AddSection()
		{
			global::Telerik.Windows.Documents.Flow.Model.Section section = new global::Telerik.Windows.Documents.Flow.Model.Section(base.Owner.Document);
			base.Add(section);
			return section;
		}
	}
}
