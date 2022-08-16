using System;
using Telerik.Windows.Documents.Fixed.Model.Actions;
using Telerik.Windows.Documents.Fixed.Model.Navigation;

namespace Telerik.Windows.Documents.Fixed.Model.Annotations
{
	public class Link : Annotation
	{
		internal Link()
		{
		}

		public Link(Destination destination)
		{
			this.Destination = destination;
		}

		public Link(Telerik.Windows.Documents.Fixed.Model.Actions.Action action)
		{
			this.Action = action;
		}

		public Destination Destination { get; internal set; }

		public Telerik.Windows.Documents.Fixed.Model.Actions.Action Action { get; internal set; }

		public override AnnotationType Type
		{
			get
			{
				return AnnotationType.Link;
			}
		}

		internal override Annotation CreateClonedInstance(RadFixedDocumentCloneContext cloneContext)
		{
			if (this.Action != null)
			{
				return new Link(this.Action.Clone(cloneContext));
			}
			Destination clonedDestination = cloneContext.GetClonedDestination(this.Destination);
			return new Link(clonedDestination);
		}
	}
}
