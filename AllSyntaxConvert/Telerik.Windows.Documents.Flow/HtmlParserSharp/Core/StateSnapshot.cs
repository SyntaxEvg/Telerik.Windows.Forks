using System;

namespace HtmlParserSharp.Core
{
	class StateSnapshot<T> : ITreeBuilderState<T> where T : class
	{
		public StackNode<T>[] Stack { get; set; }

		public StackNode<T>[] ListOfActiveFormattingElements { get; set; }

		public T FormPointer { get; set; }

		public T HeadPointer { get; set; }

		public T DeepTreeSurrogateParent { get; set; }

		public InsertionMode Mode { get; set; }

		public InsertionMode OriginalMode { get; set; }

		public bool IsFramesetOk { get; set; }

		public bool IsNeedToDropLF { get; set; }

		public bool IsQuirks { get; set; }

		internal StateSnapshot(StackNode<T>[] stack, StackNode<T>[] listOfActiveFormattingElements, T formPointer, T headPointer, T deepTreeSurrogateParent, InsertionMode mode, InsertionMode originalMode, bool framesetOk, bool needToDropLF, bool quirks)
		{
			this.Stack = stack;
			this.ListOfActiveFormattingElements = listOfActiveFormattingElements;
			this.FormPointer = formPointer;
			this.HeadPointer = headPointer;
			this.DeepTreeSurrogateParent = deepTreeSurrogateParent;
			this.Mode = mode;
			this.OriginalMode = originalMode;
			this.IsFramesetOk = framesetOk;
			this.IsNeedToDropLF = needToDropLF;
			this.IsQuirks = quirks;
		}
	}
}
