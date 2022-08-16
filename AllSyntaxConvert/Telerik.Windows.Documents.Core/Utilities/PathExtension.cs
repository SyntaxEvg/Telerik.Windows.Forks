using System;

namespace Telerik.Windows.Documents.Utilities
{
	static class PathExtension
	{
		public static string StripExtension(string extension)
		{
			extension = (extension.StartsWith(".") ? extension.Substring(1) : extension);
			for (int i = 0; i < extension.Length; i++)
			{
				if (!char.IsLetter(extension[i]) && !char.IsDigit(extension[i]))
				{
					extension = extension.Substring(0, i);
					break;
				}
			}
			return extension;
		}
	}
}
