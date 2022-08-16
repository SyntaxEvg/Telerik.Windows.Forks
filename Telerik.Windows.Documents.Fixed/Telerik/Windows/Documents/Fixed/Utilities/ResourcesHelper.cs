using System;
using System.IO;
using System.Reflection;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Fixed.Utilities
{
	static class ResourcesHelper
	{
		public static Stream GetApplicationResourceStream(string resourceName)
		{
			Guard.ThrowExceptionIfNullOrEmpty(resourceName, "resourceName");
			Stream manifestResourceStream;
			try
			{
				manifestResourceStream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName);
			}
			catch
			{
				throw new InvalidOperationException("Cannot access the resource file.");
			}
			return manifestResourceStream;
		}
	}
}
