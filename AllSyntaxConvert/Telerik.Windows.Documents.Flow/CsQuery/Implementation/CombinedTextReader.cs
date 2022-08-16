using System;
using System.IO;
using System.Runtime.Remoting;
using System.Security;
using System.Text;

namespace CsQuery.Implementation
{
	class CombinedTextReader : TextReader
	{
		public CombinedTextReader(params TextReader[] readers)
		{
			this.Count = readers.Length;
			this.Readers = readers;
			this.CurrentIndex = 0;
		}

		public override void Close()
		{
			foreach (TextReader textReader in this.Readers)
			{
				textReader.Close();
			}
		}

		protected override void Dispose(bool disposing)
		{
			foreach (TextReader textReader in this.Readers)
			{
				textReader.Dispose();
			}
		}

		public override int Peek()
		{
			int num = this.Current.Peek();
			while (num < 0 && this.NextReader())
			{
				num = this.Current.Peek();
			}
			return num;
		}

		public override int Read()
		{
			int num = this.Current.Read();
			if (num >= 0 || !this.NextReader())
			{
				return num;
			}
			return this.Current.Read();
		}

		public override int Read(char[] buffer, int index, int count)
		{
			int num = this.Current.Read(buffer, index, count);
			int num2 = count - num;
			index += num;
			while (num2 > 0 && this.NextReader())
			{
				num = this.Current.Read(buffer, index, num2);
				num2 -= num;
				index += num;
			}
			return count - num2;
		}

		public override int ReadBlock(char[] buffer, int index, int count)
		{
			int num = this.Current.ReadBlock(buffer, index, count);
			int num2 = count - num;
			index += num;
			while (num2 > 0 && this.NextReader())
			{
				num = this.Current.ReadBlock(buffer, index, num2);
				num2 -= num;
				index += num;
			}
			return count - num2;
		}

		public override string ReadLine()
		{
			string text = this.Current.ReadLine();
			while (text == null && this.NextReader())
			{
				text = this.Current.ReadLine();
			}
			return text;
		}

		public override string ReadToEnd()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(this.Current.ReadToEnd() ?? "");
			while (this.NextReader())
			{
				stringBuilder.Append(this.Current.ReadToEnd() ?? "");
			}
			return stringBuilder.ToString();
		}

		[SecurityCritical]
		public override ObjRef CreateObjRef(Type requestedType)
		{
			throw new NotImplementedException();
		}

		[SecurityCritical]
		public override object InitializeLifetimeService()
		{
			throw new NotImplementedException();
		}

		bool NextReader()
		{
			if (this.CurrentIndex < this.Count - 1)
			{
				this.CurrentIndex++;
				return true;
			}
			return false;
		}

		TextReader Current
		{
			get
			{
				return this.Readers[this.CurrentIndex];
			}
		}

		public override int GetHashCode()
		{
			int num = 0;
			foreach (TextReader textReader in this.Readers)
			{
				num += textReader.GetHashCode();
			}
			return num;
		}

		public override bool Equals(object obj)
		{
			CombinedTextReader combinedTextReader = obj as CombinedTextReader;
			if (combinedTextReader != null && combinedTextReader.Readers.Length == this.Readers.Length)
			{
				int num = 0;
				foreach (TextReader obj2 in combinedTextReader.Readers)
				{
					if (!this.Readers[num++].Equals(obj2))
					{
						return false;
					}
				}
				return true;
			}
			return false;
		}

		protected TextReader[] Readers;

		int CurrentIndex;

		int Count;
	}
}
