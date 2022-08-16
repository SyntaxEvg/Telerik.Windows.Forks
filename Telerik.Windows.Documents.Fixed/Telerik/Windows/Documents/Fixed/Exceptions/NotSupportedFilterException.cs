using System;
using System.Runtime.Serialization;
using System.Security;
using Telerik.Windows.Documents.Exceptions;

namespace Telerik.Windows.Documents.Fixed.Exceptions
{
	[Serializable]
	public class NotSupportedFilterException : NotSupportedFeatureException
	{
		public NotSupportedFilterException()
			: base("Filter type not supported.")
		{
		}

		public NotSupportedFilterException(string filterName)
			: this(filterName, string.Format("{0} is not supported.", filterName))
		{
		}

		public NotSupportedFilterException(string filterName, string message)
			: base(message)
		{
			this.FilterName = filterName;
		}

		public NotSupportedFilterException(string message, Exception cause)
			: base(message, cause)
		{
		}

		protected NotSupportedFilterException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
			this.FilterName = info.GetString("FilterName");
		}

		public string FilterName { get; set; }

		[SecurityCritical]
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			info.AddValue("FilterName", this.FilterName);
			base.GetObjectData(info, context);
		}

		const string DefaultMessage = "Filter type not supported.";

		const string FilterTypeNotSupportedMessage = "{0} is not supported.";
	}
}
