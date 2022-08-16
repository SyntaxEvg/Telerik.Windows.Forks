using System;
using Telerik.Windows.Documents.Fixed.Model.Navigation;

namespace Telerik.Windows.Documents.Fixed.Model.Actions
{
	public class GoToAction : Action
	{
		public Destination Destination { get; set; }

		internal override ActionType ActionType
		{
			get
			{
				return ActionType.GoTo;
			}
		}

		internal override Action Clone(RadFixedDocumentCloneContext cloneContext)
		{
			Destination clonedDestination = cloneContext.GetClonedDestination(this.Destination);
			return new GoToAction
			{
				Destination = clonedDestination
			};
		}
	}
}
