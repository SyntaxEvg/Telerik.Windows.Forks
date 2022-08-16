using System;

namespace HtmlParserSharp.Core
{
	interface ITreeBuilderState<T> where T : class
	{
		StackNode<T>[] Stack { get; }

		StackNode<T>[] ListOfActiveFormattingElements { get; }

		T FormPointer { get; }

		T HeadPointer { get; }

		T DeepTreeSurrogateParent { get; }

		InsertionMode Mode { get; }

		InsertionMode OriginalMode { get; }

		bool IsFramesetOk { get; }

		bool IsNeedToDropLF { get; }

		bool IsQuirks { get; }
	}
}
