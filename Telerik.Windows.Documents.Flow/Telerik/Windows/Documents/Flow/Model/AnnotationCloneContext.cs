using System;
using System.Collections.Generic;

namespace Telerik.Windows.Documents.Flow.Model
{
	class AnnotationCloneContext<TStart, TEnd> where TStart : AnnotationRangeStartBase where TEnd : AnnotationRangeEndBase
	{
		public AnnotationCloneContext()
		{
			this.hangingAnnotationStarts = new Stack<TStart>();
			this.hangingAnnotationEnds = new Stack<TEnd>();
		}

		public void AddHangingAnnotationStart(TStart start)
		{
			this.hangingAnnotationStarts.Push(start);
		}

		public void AddHangingAnnotationEnd(TEnd end)
		{
			this.hangingAnnotationEnds.Push(end);
		}

		public TStart PopHangingAnnotationStart()
		{
			if (this.hangingAnnotationStarts.Count <= 0)
			{
				return default(TStart);
			}
			return this.hangingAnnotationStarts.Pop();
		}

		public Stack<TStart> GetHangingAnnotationStarts()
		{
			return this.hangingAnnotationStarts;
		}

		public Stack<TEnd> GetHangingAnnotationEnds()
		{
			return this.hangingAnnotationEnds;
		}

		readonly Stack<TStart> hangingAnnotationStarts;

		readonly Stack<TEnd> hangingAnnotationEnds;
	}
}
