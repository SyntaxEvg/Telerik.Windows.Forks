using System;

namespace Telerik.Windows.Zip
{
	enum DeflateBlockState
	{
		NeedMore,
		BlockDone,
		FinishStarted,
		FinishDone
	}
}
