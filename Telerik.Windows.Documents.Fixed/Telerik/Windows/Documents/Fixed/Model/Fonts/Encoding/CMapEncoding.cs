using System;
using System.Collections.Generic;
using System.Linq;
using Telerik.Windows.Documents.Fixed.Model.Common;
using Telerik.Windows.Documents.Fixed.Model.Data;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Fixed.Model.Fonts.Encoding
{
	class CMapEncoding
	{
		public CMapEncoding()
		{
			this.unicodeMapping = new Dictionary<CharCode, string>();
			this.cidSystemInfo = new PdfProperty<CidSystemInfo>(() => Telerik.Windows.Documents.Fixed.Model.Fonts.Encoding.CidSystemInfo.Default);
			this.cMapName = new PdfProperty<string>(() => "Adobe-Identity-UCS");
			this.cMapType = new PdfProperty<int>(() => 2);
		}

		public PdfProperty<CidSystemInfo> CidSystemInfo
		{
			get
			{
				return this.cidSystemInfo;
			}
		}

		public PdfProperty<string> CMapName
		{
			get
			{
				return this.cMapName;
			}
		}

		public PdfProperty<int> CMapType
		{
			get
			{
				return this.cMapType;
			}
		}

		public IEnumerable<Tuple<CharCode, string>> CharCodeToUnicodeMappings
		{
			get
			{
				return from kv in this.unicodeMapping
					select new Tuple<CharCode, string>(kv.Key, kv.Value);
			}
		}

		public void AddUnicodeMapping(CharCode code, string unicode)
		{
			Guard.ThrowExceptionIfNull<CharCode>(code, "code");
			Guard.ThrowExceptionIfNull<string>(unicode, "unicode");
			if (!this.unicodeMapping.ContainsKey(code))
			{
				this.unicodeMapping.Add(code, unicode);
			}
		}

		public bool TryGetToUnicode(CharCode code, out string unicode)
		{
			Guard.ThrowExceptionIfNull<CharCode>(code, "code");
			return this.unicodeMapping.TryGetValue(code, out unicode);
		}

		readonly Dictionary<CharCode, string> unicodeMapping;

		readonly PdfProperty<CidSystemInfo> cidSystemInfo;

		readonly PdfProperty<string> cMapName;

		readonly PdfProperty<int> cMapType;
	}
}
