using System;
using System.Collections.Generic;
using System.IO;

namespace CsQuery.Implementation
{
	class CombinedStream : BaseStream
	{
		public CombinedStream(params Stream[] streams)
			: this((IEnumerable<Stream>)streams)
		{
		}

		public CombinedStream(IEnumerable<Stream> streams)
		{
			this._streams = streams.GetEnumerator();
			this._valid = this._streams.MoveNext();
		}

		public override bool CanRead
		{
			get
			{
				return this._valid && this._streams.Current.CanRead;
			}
		}

		public override int Read(byte[] buffer, int offset, int count)
		{
			if (count == 0)
			{
				return 0;
			}
			while (this._valid)
			{
				int num = this._streams.Current.Read(buffer, offset, count);
				if (num > 0)
				{
					return num;
				}
				this._valid = this._streams.MoveNext();
			}
			return 0;
		}

		bool _valid;

		readonly IEnumerator<Stream> _streams;
	}
}
