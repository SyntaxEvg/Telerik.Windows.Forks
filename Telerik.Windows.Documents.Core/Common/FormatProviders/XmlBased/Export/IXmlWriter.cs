using System;

namespace Telerik.Windows.Documents.Common.FormatProviders.XmlBased.Export
{
	interface IXmlWriter
	{
		void WriteElementStart(string name);

		void WriteAttribute(string name, string value);

		void WriteAttribute(string ns, string name, string value);

		void WriteElementStart(string prefix, string name, string ns);
	}
}
