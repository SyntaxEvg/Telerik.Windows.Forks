using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Telerik.Windows.Documents.Core.Data;
using Telerik.Windows.Documents.Fixed.Model.Internal.Classes;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.Text
{
	class CMapOld
	{
		public static global::Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.Text.CMapOld Identity { get; private set; } = new global::Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.Text.CMapOld(true);

		public CMapOld()
		{
			this.codeRanges = new global::System.Collections.Generic.List<global::Telerik.Windows.Documents.Core.Data.Tuple<global::Telerik.Windows.Documents.Fixed.Model.Internal.Classes.CharCodeOld, global::Telerik.Windows.Documents.Fixed.Model.Internal.Classes.CharCodeOld>>();
			this.cidToUnicodeMapping = new global::System.Collections.Generic.Dictionary<global::Telerik.Windows.Documents.Fixed.Model.Internal.Classes.CharCodeOld, string>();
			this.ICCtoCIDMapping = new global::System.Collections.Generic.Dictionary<global::Telerik.Windows.Documents.Fixed.Model.Internal.Classes.CharCodeOld, global::Telerik.Windows.Documents.Fixed.Model.Internal.Classes.CharCodeOld>();
			this.notDef = new global::System.Collections.Generic.List<global::Telerik.Windows.Documents.Core.Data.Tuple<global::Telerik.Windows.Documents.Fixed.Model.Internal.Classes.CharCodeOld, global::Telerik.Windows.Documents.Core.Data.Tuple<global::Telerik.Windows.Documents.Fixed.Model.Internal.Classes.CharCodeOld, global::Telerik.Windows.Documents.Fixed.Model.Internal.Classes.CharCodeOld>>>();
		}

		private CMapOld(bool identity)
		{
			this.isIdentity = identity;
		}

		public global::Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.Text.CMapOld UseCMap { get; set; }

		private global::Telerik.Windows.Documents.Fixed.Model.Internal.Classes.CharCodeOld CreateCharCode(byte[] bytes, int offset, int count)
		{
			byte[] array = new byte[count];
			for (int i = 0; i < count; i++)
			{
				array[i] = bytes[offset + i];
			}
			return new global::Telerik.Windows.Documents.Fixed.Model.Internal.Classes.CharCodeOld(array);
		}

		private bool IsInRange(global::Telerik.Windows.Documents.Fixed.Model.Internal.Classes.CharCodeOld cc)
		{
			foreach (global::Telerik.Windows.Documents.Core.Data.Tuple<global::Telerik.Windows.Documents.Fixed.Model.Internal.Classes.CharCodeOld, global::Telerik.Windows.Documents.Fixed.Model.Internal.Classes.CharCodeOld> tuple in this.codeRanges)
			{
				if (cc.BytesCount == tuple.Item1.BytesCount && tuple.Item1 <= cc && cc <= tuple.Item2)
				{
					return true;
				}
			}
			return false;
		}

		private global::Telerik.Windows.Documents.Fixed.Model.Internal.Classes.CharCodeOld GetNotDef(global::Telerik.Windows.Documents.Fixed.Model.Internal.Classes.CharCodeOld cc)
		{
			foreach (global::Telerik.Windows.Documents.Core.Data.Tuple<global::Telerik.Windows.Documents.Fixed.Model.Internal.Classes.CharCodeOld, global::Telerik.Windows.Documents.Core.Data.Tuple<global::Telerik.Windows.Documents.Fixed.Model.Internal.Classes.CharCodeOld, global::Telerik.Windows.Documents.Fixed.Model.Internal.Classes.CharCodeOld>> tuple in this.notDef)
			{
				if (tuple.Item1 >= cc && cc <= tuple.Item2.Item1)
				{
					return tuple.Item2.Item2;
				}
			}
			return default(global::Telerik.Windows.Documents.Fixed.Model.Internal.Classes.CharCodeOld);
		}

		private global::Telerik.Windows.Documents.Core.Data.Tuple<string, global::Telerik.Windows.Documents.Fixed.Model.Internal.Classes.CharCodeOld> GetCIDtoUnicodeMapping(byte[] bytes, int offset, int count, bool checkRanges = true)
		{
			if (offset + count > bytes.Length)
			{
				return new global::Telerik.Windows.Documents.Core.Data.Tuple<string, global::Telerik.Windows.Documents.Fixed.Model.Internal.Classes.CharCodeOld>();
			}
			global::Telerik.Windows.Documents.Fixed.Model.Internal.Classes.CharCodeOld charCodeOld = this.CreateCharCode(bytes, offset, count);
			bool flag = !checkRanges || this.IsInRange(charCodeOld);
			string item;
			if (flag && this.cidToUnicodeMapping.TryGetValue(charCodeOld, out item))
			{
				return new global::Telerik.Windows.Documents.Core.Data.Tuple<string, global::Telerik.Windows.Documents.Fixed.Model.Internal.Classes.CharCodeOld>(item, charCodeOld);
			}
			if (this.UseCMap != null && this.UseCMap.IsInRange(charCodeOld))
			{
				return this.UseCMap.GetCIDtoUnicodeMapping(bytes, offset, count, checkRanges);
			}
			return new global::Telerik.Windows.Documents.Core.Data.Tuple<string, global::Telerik.Windows.Documents.Fixed.Model.Internal.Classes.CharCodeOld>();
		}

		private global::Telerik.Windows.Documents.Fixed.Model.Internal.Classes.CharCodeOld GetICCtoCIDMapping(byte[] bytes, int offset, int count, bool checkRanges = true)
		{
			if (offset + count > bytes.Length)
			{
				return default(global::Telerik.Windows.Documents.Fixed.Model.Internal.Classes.CharCodeOld);
			}
			global::Telerik.Windows.Documents.Fixed.Model.Internal.Classes.CharCodeOld charCodeOld = this.CreateCharCode(bytes, offset, count);
			bool flag = !checkRanges || this.IsInRange(charCodeOld);
			global::Telerik.Windows.Documents.Fixed.Model.Internal.Classes.CharCodeOld result;
			if (flag && this.ICCtoCIDMapping.TryGetValue(charCodeOld, out result))
			{
				return result;
			}
			if (this.UseCMap != null && this.UseCMap.IsInRange(charCodeOld))
			{
				return this.UseCMap.GetICCtoCIDMapping(bytes, offset, count, checkRanges);
			}
			return this.GetNotDef(charCodeOld);
		}

		private global::System.Collections.Generic.IEnumerable<global::Telerik.Windows.Documents.Fixed.Model.Internal.Classes.CharCodeOld> GetIdentityCharIds(byte[] bytes)
		{
			global::System.Collections.Generic.List<global::Telerik.Windows.Documents.Fixed.Model.Internal.Classes.CharCodeOld> list = new global::System.Collections.Generic.List<global::Telerik.Windows.Documents.Fixed.Model.Internal.Classes.CharCodeOld>();
			for (int i = 0; i < bytes.Length; i += 2)
			{
				byte[] array = new byte[2];
				if (i + 1 >= bytes.Length)
				{
					array[0] = 0;
					array[1] = bytes[i];
				}
				else
				{
					array[0] = bytes[i];
					array[1] = bytes[i + 1];
				}
				global::Telerik.Windows.Documents.Fixed.Model.Internal.Classes.CharCodeOld item = new global::Telerik.Windows.Documents.Fixed.Model.Internal.Classes.CharCodeOld(array);
				list.Add(item);
			}
			return list.ToArray();
		}

		private global::System.Collections.Generic.IEnumerable<global::Telerik.Windows.Documents.Core.Data.Tuple<string, global::Telerik.Windows.Documents.Fixed.Model.Internal.Classes.CharCodeOld>> GetIdentityToUnicode(byte[] bytes)
		{
			string res = global::System.Text.Encoding.BigEndianUnicode.GetString(bytes, 0, bytes.Length);
			for (int i = 0; i < res.Length; i++)
			{
				yield return new global::Telerik.Windows.Documents.Core.Data.Tuple<string, global::Telerik.Windows.Documents.Fixed.Model.Internal.Classes.CharCodeOld>(res[i].ToString(), new global::Telerik.Windows.Documents.Fixed.Model.Internal.Classes.CharCodeOld((ushort)res[i]));
			}
			yield break;
		}

		internal void AddCIDtoUnicodeMapping(global::Telerik.Windows.Documents.Fixed.Model.Internal.Classes.CharCodeOld cid, string ch)
		{
			this.cidToUnicodeMapping[cid] = ch;
		}

		internal void AddICCtoCIDMapping(global::Telerik.Windows.Documents.Fixed.Model.Internal.Classes.CharCodeOld icc, int cid)
		{
			this.ICCtoCIDMapping[icc] = new global::Telerik.Windows.Documents.Fixed.Model.Internal.Classes.CharCodeOld(cid);
		}

		internal void AddNotDefRange(global::Telerik.Windows.Documents.Fixed.Model.Internal.Classes.CharCodeOld begin, global::Telerik.Windows.Documents.Fixed.Model.Internal.Classes.CharCodeOld end, int cid)
		{
			this.notDef.Add(new global::Telerik.Windows.Documents.Core.Data.Tuple<global::Telerik.Windows.Documents.Fixed.Model.Internal.Classes.CharCodeOld, global::Telerik.Windows.Documents.Core.Data.Tuple<global::Telerik.Windows.Documents.Fixed.Model.Internal.Classes.CharCodeOld, global::Telerik.Windows.Documents.Fixed.Model.Internal.Classes.CharCodeOld>>(begin, new global::Telerik.Windows.Documents.Core.Data.Tuple<global::Telerik.Windows.Documents.Fixed.Model.Internal.Classes.CharCodeOld, global::Telerik.Windows.Documents.Fixed.Model.Internal.Classes.CharCodeOld>(end, new global::Telerik.Windows.Documents.Fixed.Model.Internal.Classes.CharCodeOld(cid))));
		}

		internal void AddCodeRange(global::Telerik.Windows.Documents.Fixed.Model.Internal.Classes.CharCodeOld begin, global::Telerik.Windows.Documents.Fixed.Model.Internal.Classes.CharCodeOld end)
		{
			this.codeRanges.Add(new global::Telerik.Windows.Documents.Core.Data.Tuple<global::Telerik.Windows.Documents.Fixed.Model.Internal.Classes.CharCodeOld, global::Telerik.Windows.Documents.Fixed.Model.Internal.Classes.CharCodeOld>(begin, end));
		}

		private global::System.Collections.Generic.IEnumerable<global::Telerik.Windows.Documents.Core.Data.Tuple<string, global::Telerik.Windows.Documents.Fixed.Model.Internal.Classes.CharCodeOld>> GetToUnicode(byte[] bytes)
		{
			int i = 0;
			while (i < bytes.Length)
			{
				bool found = false;
				for (int j = 1; j <= 4; j++)
				{
					global::Telerik.Windows.Documents.Core.Data.Tuple<string, global::Telerik.Windows.Documents.Fixed.Model.Internal.Classes.CharCodeOld> res = this.GetCIDtoUnicodeMapping(bytes, i, j, false);
					if (!res.IsEmpty())
					{
						yield return res;
						i += j;
						found = true;
						break;
					}
				}
				if (!found)
				{
					i++;
				}
			}
			yield break;
		}

		public global::System.Collections.Generic.IEnumerable<global::Telerik.Windows.Documents.Fixed.Model.Internal.Classes.CharCodeOld> GetCharIds(byte[] bytes)
		{
			if (this.isIdentity)
			{
				return this.GetIdentityCharIds(bytes);
			}
			int i = 0;
			global::System.Collections.Generic.List<global::Telerik.Windows.Documents.Fixed.Model.Internal.Classes.CharCodeOld> list = new global::System.Collections.Generic.List<global::Telerik.Windows.Documents.Fixed.Model.Internal.Classes.CharCodeOld>();
			while (i < bytes.Length)
			{
				bool flag = false;
				for (int j = 1; j <= 4; j++)
				{
					global::Telerik.Windows.Documents.Fixed.Model.Internal.Classes.CharCodeOld icctoCIDMapping = this.GetICCtoCIDMapping(bytes, i, j, false);
					if (!icctoCIDMapping.IsEmpty)
					{
						list.Add(icctoCIDMapping);
						i += j;
						flag = true;
						break;
					}
				}
				if (!flag)
				{
					i++;
				}
			}
			return list.ToArray();
		}

		public global::System.Collections.Generic.IEnumerable<global::Telerik.Windows.Documents.Core.Data.Tuple<string, global::Telerik.Windows.Documents.Fixed.Model.Internal.Classes.CharCodeOld>> ToUnicode(byte[] bytes)
		{
			if (this.isIdentity)
			{
				return this.GetIdentityToUnicode(bytes);
			}
			return this.GetToUnicode(bytes);
		}

		public bool TryGetToUnicode(global::Telerik.Windows.Documents.Fixed.Model.Internal.Classes.CharCodeOld charCode, out string unicode)
		{
			unicode = null;
			if (!this.isIdentity)
			{
				return this.cidToUnicodeMapping.TryGetValue(charCode, out unicode);
			}
			global::Telerik.Windows.Documents.Core.Data.Tuple<string, global::Telerik.Windows.Documents.Fixed.Model.Internal.Classes.CharCodeOld> tuple = this.GetIdentityToUnicode(charCode.Bytes).FirstOrDefault<global::Telerik.Windows.Documents.Core.Data.Tuple<string, global::Telerik.Windows.Documents.Fixed.Model.Internal.Classes.CharCodeOld>>();
			if (tuple.IsEmpty())
			{
				return false;
			}
			unicode = tuple.Item1;
			return true;
		}

		private global::System.Collections.Generic.Dictionary<global::Telerik.Windows.Documents.Fixed.Model.Internal.Classes.CharCodeOld, global::Telerik.Windows.Documents.Fixed.Model.Internal.Classes.CharCodeOld> ICCtoCIDMapping;

		private global::System.Collections.Generic.Dictionary<global::Telerik.Windows.Documents.Fixed.Model.Internal.Classes.CharCodeOld, string> cidToUnicodeMapping;

		private global::System.Collections.Generic.List<global::Telerik.Windows.Documents.Core.Data.Tuple<global::Telerik.Windows.Documents.Fixed.Model.Internal.Classes.CharCodeOld, global::Telerik.Windows.Documents.Core.Data.Tuple<global::Telerik.Windows.Documents.Fixed.Model.Internal.Classes.CharCodeOld, global::Telerik.Windows.Documents.Fixed.Model.Internal.Classes.CharCodeOld>>> notDef;

		private global::System.Collections.Generic.List<global::Telerik.Windows.Documents.Core.Data.Tuple<global::Telerik.Windows.Documents.Fixed.Model.Internal.Classes.CharCodeOld, global::Telerik.Windows.Documents.Fixed.Model.Internal.Classes.CharCodeOld>> codeRanges;

		private bool isIdentity;
	}
}
