using System;

namespace Telerik.Windows.Documents.Fixed.Model.Actions
{
	public abstract class Action
	{
		internal abstract ActionType ActionType { get; }

		internal abstract Action Clone(RadFixedDocumentCloneContext context);
	}
}
