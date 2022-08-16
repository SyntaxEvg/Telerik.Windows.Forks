﻿using System;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Types;

namespace Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Core
{
	class OpenXmlAttribute<T> : ConvertedOpenXmlAttribute<T>
	{
		public OpenXmlAttribute(string name, bool isRequired = false)
			: this(name, null, isRequired)
		{
		}

		public OpenXmlAttribute(string name, T defaultValue, bool isRequired = false)
			: this(name, null, defaultValue, isRequired)
		{
		}

		public OpenXmlAttribute(string name, OpenXmlNamespace ns, bool isRequired = false)
			: this(name, ns, default(T), isRequired)
		{
		}

		public OpenXmlAttribute(string name, OpenXmlNamespace ns, T defaultValue, bool isRequired = false)
			: base(name, ns, new DefaultConverter<T>(), defaultValue, isRequired)
		{
		}
	}
}
