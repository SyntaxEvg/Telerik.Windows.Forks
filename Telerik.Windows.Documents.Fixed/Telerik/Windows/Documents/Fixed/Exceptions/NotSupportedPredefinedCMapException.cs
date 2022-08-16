using System;
using System.Runtime.Serialization;
using System.Security;
using Telerik.Windows.Documents.Exceptions;

namespace Telerik.Windows.Documents.Fixed.Exceptions
{
	[Serializable]
	public class NotSupportedPredefinedCMapException : NotSupportedFeatureException
	{
		public NotSupportedPredefinedCMapException()
			: base("Predefined CMap not supported.")
		{
		}

		public NotSupportedPredefinedCMapException(string cmapName)
			: this(cmapName, string.Format("Predefined CMap {0} is not supported.", cmapName))
		{
		}

		public NotSupportedPredefinedCMapException(string cmapName, string message)
			: base(message)
		{
			this.CMapName = cmapName;
		}

		public NotSupportedPredefinedCMapException(string message, Exception cause)
			: base(message, cause)
		{
		}

		protected NotSupportedPredefinedCMapException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
			this.CMapName = info.GetString("CMapName");
		}

		public string CMapName { get; set; }

		[SecurityCritical]
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			info.AddValue("CMapName", this.CMapName);
			base.GetObjectData(info, context);
		}

		const string DefaultMessage = "Predefined CMap not supported.";

		const string PredefinedCMapNotSupportedMessage = "Predefined CMap {0} is not supported.";
	}
}
