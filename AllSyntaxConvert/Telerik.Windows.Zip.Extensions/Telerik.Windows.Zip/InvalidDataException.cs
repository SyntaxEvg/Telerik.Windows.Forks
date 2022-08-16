using System;

namespace Telerik.Windows.Zip
{
	public sealed class InvalidDataException : Exception
	{
		public InvalidDataException()
			: base("Invalid data")
		{
		}

		public InvalidDataException(string message)
			: base(message)
		{
		}

		public InvalidDataException(string message, Exception innerException)
			: base(message, innerException)
		{
		}
	}
}
