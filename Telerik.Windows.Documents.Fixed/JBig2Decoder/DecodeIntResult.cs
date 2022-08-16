using System;

namespace JBig2Decoder
{
	class DecodeIntResult
	{
		public DecodeIntResult(long intResult, bool booleanResult)
		{
			this._intResult = intResult;
			this._booleanResult = booleanResult;
		}

		public long intResult()
		{
			return this._intResult;
		}

		public bool booleanResult()
		{
			return this._booleanResult;
		}

		long _intResult;

		bool _booleanResult;
	}
}
