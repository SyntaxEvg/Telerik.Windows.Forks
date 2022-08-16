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
		public static CMapOld Identity { get; set; } = new CMapOld(true);

		public CMapOld()
		{
			this.codeRanges = new List<Tuple<CharCodeOld, CharCodeOld>>();
			this.cidToUnicodeMapping = new Dictionary<CharCodeOld, string>();
			this.ICCtoCIDMapping = new Dictionary<CharCodeOld, CharCodeOld>();
			this.notDef = new List<Tuple<CharCodeOld, Tuple<CharCodeOld, CharCodeOld>>>();
		}

		CMapOld(bool identity)
		{
			this.isIdentity = identity;
		}

		public CMapOld UseCMap { get; set; }

		CharCodeOld CreateCharCode(byte[] bytes, int offset, int count)
		{
			byte[] array = new byte[count];
			for (int i = 0; i < count; i++)
			{
				array[i] = bytes[offset + i];
			}
			return new CharCodeOld(array);
		}

		bool IsInRange(CharCodeOld cc)
		{
			foreach (Tuple<CharCodeOld, CharCodeOld> tuple in this.codeRanges)
			{
				if (cc.BytesCount == tuple.Item1.BytesCount && tuple.Item1 <= cc && cc <= tuple.Item2)
				{
					return true;
				}
			}
			return false;
		}

		CharCodeOld GetNotDef(CharCodeOld cc)
		{
			foreach (Tuple<CharCodeOld, Tuple<CharCodeOld, CharCodeOld>> tuple in this.notDef)
			{
				if (tuple.Item1 >= cc && cc <= tuple.Item2.Item1)
				{
					return tuple.Item2.Item2;
				}
			}
			return default(CharCodeOld);
		}

		Tuple<string, CharCodeOld> GetCIDtoUnicodeMapping(byte[] bytes, int offset, int count, bool checkRanges = true)
		{
			if (offset + count > bytes.Length)
			{
				return new Tuple<string, CharCodeOld>();
			}
			CharCodeOld charCodeOld = this.CreateCharCode(bytes, offset, count);
			bool flag = !checkRanges || this.IsInRange(charCodeOld);
			string item;
			if (flag && this.cidToUnicodeMapping.TryGetValue(charCodeOld, out item))
			{
				return new Tuple<string, CharCodeOld>(item, charCodeOld);
			}
			if (this.UseCMap != null && this.UseCMap.IsInRange(charCodeOld))
			{
				return this.UseCMap.GetCIDtoUnicodeMapping(bytes, offset, count, checkRanges);
			}
			return new Tuple<string, CharCodeOld>();
		}

		CharCodeOld GetICCtoCIDMapping(byte[] bytes, int offset, int count, bool checkRanges = true)
		{
			if (offset + count > bytes.Length)
			{
				return default(CharCodeOld);
			}
			CharCodeOld charCodeOld = this.CreateCharCode(bytes, offset, count);
			bool flag = !checkRanges || this.IsInRange(charCodeOld);
			CharCodeOld result;
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

		IEnumerable<CharCodeOld> GetIdentityCharIds(byte[] bytes)
		{
			List<CharCodeOld> list = new List<CharCodeOld>();
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
				CharCodeOld item = new CharCodeOld(array);
				list.Add(item);
			}
			return list.ToArray();
		}

		IEnumerable<Tuple<string, CharCodeOld>> GetIdentityToUnicode(byte[] bytes)
		{
			string res = Encoding.BigEndianUnicode.GetString(bytes, 0, bytes.Length);
			for (int i = 0; i < res.Length; i++)
			{
				yield return new Tuple<string, CharCodeOld>(res[i].ToString(), new CharCodeOld((ushort)res[i]));
			}
			yield break;
		}

		internal void AddCIDtoUnicodeMapping(CharCodeOld cid, string ch)
		{
			this.cidToUnicodeMapping[cid] = ch;
		}

		internal void AddICCtoCIDMapping(CharCodeOld icc, int cid)
		{
			this.ICCtoCIDMapping[icc] = new CharCodeOld(cid);
		}

		internal void AddNotDefRange(CharCodeOld begin, CharCodeOld end, int cid)
		{
			this.notDef.Add(new Tuple<CharCodeOld, Tuple<CharCodeOld, CharCodeOld>>(begin, new Tuple<CharCodeOld, CharCodeOld>(end, new CharCodeOld(cid))));
		}

		internal void AddCodeRange(CharCodeOld begin, CharCodeOld end)
		{
			this.codeRanges.Add(new Tuple<CharCodeOld, CharCodeOld>(begin, end));
		}

		IEnumerable<Tuple<string, CharCodeOld>> GetToUnicode(byte[] bytes)
		{
			int i = 0;
			while (i < bytes.Length)
			{
				bool found = false;
				for (int j = 1; j <= 4; j++)
				{
					Tuple<string, CharCodeOld> res = this.GetCIDtoUnicodeMapping(bytes, i, j, false);
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

		public IEnumerable<CharCodeOld> GetCharIds(byte[] bytes)
		{
			if (this.isIdentity)
			{
				return this.GetIdentityCharIds(bytes);
			}
			int i = 0;
			List<CharCodeOld> list = new List<CharCodeOld>();
			while (i < bytes.Length)
			{
				bool flag = false;
				for (int j = 1; j <= 4; j++)
				{
					CharCodeOld icctoCIDMapping = this.GetICCtoCIDMapping(bytes, i, j, false);
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

		public IEnumerable<Tuple<string, CharCodeOld>> ToUnicode(byte[] bytes)
		{
			if (this.isIdentity)
			{
				return this.GetIdentityToUnicode(bytes);
			}
			return this.GetToUnicode(bytes);
		}

		public bool TryGetToUnicode(CharCodeOld charCode, out string unicode)
		{
			unicode = null;
			if (!this.isIdentity)
			{
				return this.cidToUnicodeMapping.TryGetValue(charCode, out unicode);
			}
			Tuple<string, CharCodeOld> tuple = this.GetIdentityToUnicode(charCode.Bytes).FirstOrDefault<Tuple<string, CharCodeOld>>();
			if (tuple.IsEmpty())
			{
				return false;
			}
			unicode = tuple.Item1;
			return true;
		}

		Dictionary<CharCodeOld, CharCodeOld> ICCtoCIDMapping;

		Dictionary<CharCodeOld, string> cidToUnicodeMapping;

		List<Tuple<CharCodeOld, Tuple<CharCodeOld, CharCodeOld>>> notDef;

		List<Tuple<CharCodeOld, CharCodeOld>> codeRanges;

		bool isIdentity;
	}
}
