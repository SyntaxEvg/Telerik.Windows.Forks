using System;
using System.Windows;

namespace Telerik.Windows.Documents.Licensing
{
	static class AssemblyProtection
	{
		public static bool IsValid()
		{
			return true;
		}

		public static bool ValidatePassPhrase()
		{
			Application application = Application.Current;
			if (application == null)
			{
				return !(Application.ResourceAssembly == null) && Application.ResourceAssembly.GetName().Name == "MyApp";
			}
			if (application.Resources.Contains("Telerik.Windows.Controls.Key"))
			{
				string text = application.Resources["Telerik.Windows.Controls.Key"] as string;
				if (text != null && text == "MyApp")
				{
					return true;
				}
			}
			return false;
		}

		internal const string ApplicationName = "MyApp";

		const string Key = "Telerik.Windows.Controls.Key";
	}
}
