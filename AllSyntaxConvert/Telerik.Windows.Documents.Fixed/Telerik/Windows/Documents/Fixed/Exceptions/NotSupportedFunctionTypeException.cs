using System;
using System.Runtime.Serialization;
using System.Security;
using Telerik.Windows.Documents.Exceptions;

namespace Telerik.Windows.Documents.Fixed.Exceptions
{
	[Serializable]
	public class NotSupportedFunctionTypeException : NotSupportedFeatureException
	{
		public NotSupportedFunctionTypeException()
			: base("Function type is not supported.")
		{
		}

		public NotSupportedFunctionTypeException(string message)
			: base(message)
		{
		}

		public NotSupportedFunctionTypeException(int functionType)
			: this(functionType, string.Format("Function type {0} is not supported.", functionType))
		{
		}

		public NotSupportedFunctionTypeException(int functionType, string message)
			: base(message)
		{
			this.FunctionType = functionType;
		}

		public NotSupportedFunctionTypeException(string message, Exception cause)
			: base(message, cause)
		{
		}

		protected NotSupportedFunctionTypeException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
			this.FunctionType = info.GetInt32("FunctionType");
		}

		public int FunctionType { get; set; }

		[SecurityCritical]
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			info.AddValue("FunctionType", this.FunctionType);
			base.GetObjectData(info, context);
		}

		const string DefaultMessage = "Function type is not supported.";

		const string FunctionTypeNotSupportedMessage = "Function type {0} is not supported.";
	}
}
