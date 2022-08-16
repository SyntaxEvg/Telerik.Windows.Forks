using System;

namespace CsQuery.Implementation
{
	class CSSStyleChangedArgs : EventArgs
	{
		public CSSStyleChangedArgs(bool hasStyleAttribute)
		{
			this.HasStyleAttribute = hasStyleAttribute;
		}

		public bool HasStyleAttribute { get; protected set; }
	}
}
