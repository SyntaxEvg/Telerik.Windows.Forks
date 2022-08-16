using System;

namespace Telerik.Windows.Documents.Flow.Model.Collections
{
	public sealed class SectionCollection : DocumentElementCollection<Section, RadFlowDocument>
	{
		internal SectionCollection(RadFlowDocument parent)
			: base(parent)
		{
		}

		public Section AddSection()
		{
			Section section = new Section(base.Owner.Document);
			base.Add(section);
			return section;
		}
	}
}
