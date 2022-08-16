using System;
using System.Runtime.Serialization;
using System.Security;
using Telerik.Windows.Documents.Exceptions;

namespace Telerik.Windows.Documents.Fixed.Exceptions
{
	[Serializable]
	public class NotSupportedShadingTypeException : NotSupportedFeatureException
	{
		public NotSupportedShadingTypeException()
			: base("Shading type is not supported.")
		{
		}

		public NotSupportedShadingTypeException(string message)
			: base(message)
		{
		}

		public NotSupportedShadingTypeException(int shadingType)
			: this(shadingType, string.Format("Shading type {0} is not supported.", shadingType))
		{
		}

		public NotSupportedShadingTypeException(int shadingType, string message)
			: base(message)
		{
			this.ShadingType = shadingType;
		}

		public NotSupportedShadingTypeException(string message, Exception cause)
			: base(message, cause)
		{
		}

		protected NotSupportedShadingTypeException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
			this.ShadingType = info.GetInt32("ShadingType");
		}

		public int ShadingType { get; set; }

		[SecurityCritical]
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			info.AddValue("ShadingType", this.ShadingType);
			base.GetObjectData(info, context);
		}

		const string DefaultMessage = "Shading type is not supported.";

		const string ShadingTypeNotSupportedMessage = "Shading type {0} is not supported.";
	}
}
