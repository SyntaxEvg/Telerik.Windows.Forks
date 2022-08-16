using System;
using Telerik.Windows.Documents.Fixed.Model.Actions;
using Telerik.Windows.Documents.Fixed.Model.Annotations;
using Telerik.Windows.Documents.Fixed.Model.Navigation;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Fixed.Model.Collections
{
	public sealed class AnnotationCollection : DocumentElementCollection<Annotation, RadFixedPage>
	{
		public AnnotationCollection(RadFixedPage parent)
			: base(parent)
		{
		}

		public Link AddLink(Destination destination)
		{
			Link link = new Link(destination);
			return this.CreateLink(link);
		}

		public Link AddLink(Telerik.Windows.Documents.Fixed.Model.Actions.Action action)
		{
			Link link = new Link(action);
			return this.CreateLink(link);
		}

		Link CreateLink(Link link)
		{
			Guard.ThrowExceptionIfNull<Link>(link, "link");
			base.Add(link);
			return link;
		}
	}
}
