using System;

namespace Telerik.Windows.Documents.Fixed.Model.Actions
{
	public class UriAction : Action
	{
		public UriAction()
		{
			this.IncludeMouseCoordinates = new bool?(false);
		}

		public UriAction(Uri uri)
			: this()
		{
			this.Uri = uri;
		}

		public Uri Uri { get; set; }

		public bool? IncludeMouseCoordinates { get; set; }

		internal override ActionType ActionType
		{
			get
			{
				return ActionType.Uri;
			}
		}

		internal override Action Clone(RadFixedDocumentCloneContext context)
		{
			return new UriAction
			{
				Uri = this.Uri,
				IncludeMouseCoordinates = this.IncludeMouseCoordinates
			};
		}
	}
}
