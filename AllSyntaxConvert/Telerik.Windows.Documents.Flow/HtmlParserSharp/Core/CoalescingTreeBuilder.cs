using System;
using System.Text;

namespace HtmlParserSharp.Core
{
	abstract class CoalescingTreeBuilder<T> : TreeBuilder<T> where T : class
	{
		protected override void AppendCharacters(T parent, char[] buf, int start, int length)
		{
			this.AppendCharacters(parent, new string(buf, start, length));
		}

		protected override void AppendCharacters(T parent, StringBuilder sb)
		{
			this.AppendCharacters(parent, sb.ToString());
		}

		protected override void AppendIsindexPrompt(T parent)
		{
			this.AppendCharacters(parent, "This is a searchable index. Enter search keywords: ");
		}

		protected abstract void AppendCharacters(T parent, string text);

		protected override void AppendComment(T parent, char[] buf, int start, int length)
		{
			this.AppendComment(parent, new string(buf, start, length));
		}

		protected abstract void AppendComment(T parent, string comment);

		protected override void AppendCommentToDocument(char[] buf, int start, int length)
		{
			this.AppendCommentToDocument(new string(buf, start, length));
		}

		protected abstract void AppendCommentToDocument(string comment);

		protected override void InsertFosterParentedCharacters(StringBuilder sb, T table, T stackParent)
		{
			this.InsertFosterParentedCharacters(sb.ToString(), table, stackParent);
		}

		protected abstract void InsertFosterParentedCharacters(string text, T table, T stackParent);
	}
}
