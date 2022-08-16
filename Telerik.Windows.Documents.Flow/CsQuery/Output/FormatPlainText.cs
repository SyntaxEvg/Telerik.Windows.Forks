using System;
using System.IO;
using System.Text;
using CsQuery.StringScanner;

namespace CsQuery.Output
{
	class FormatPlainText : IOutputFormatter
	{
		public void Render(IDomObject node, TextWriter writer)
		{
			this.stringInfo = CharacterData.CreateStringInfo();
			StringBuilder stringBuilder = new StringBuilder();
			this.AddContents(stringBuilder, node, true);
			writer.Write(stringBuilder.ToString());
		}

		public string Render(IDomObject node)
		{
			string result;
			using (StringWriter stringWriter = new StringWriter())
			{
				this.Render(node, stringWriter);
				result = stringWriter.ToString();
			}
			return result;
		}

		protected void AddContents(StringBuilder sb, IDomObject node, bool skipWhitespace)
		{
			if (node.HasChildren)
			{
				foreach (IDomObject domObject in node.ChildNodes)
				{
					if (domObject.NodeType == NodeType.TEXT_NODE)
					{
						this.stringInfo.Target = domObject.NodeValue;
						if (this.stringInfo.Whitespace)
						{
							if (!skipWhitespace)
							{
								sb.Append(" ");
								skipWhitespace = true;
							}
						}
						else
						{
							string text = this.CleanFragment(domObject.Render());
							if (skipWhitespace)
							{
								text = text.TrimStart(new char[0]);
								skipWhitespace = false;
							}
							sb.Append(text);
						}
					}
					else if (domObject.NodeType == NodeType.ELEMENT_NODE)
					{
						IDomElement domElement = (IDomElement)domObject;
						if (domObject.NodeName != "HEAD" && domObject.NodeName != "STYLE" && domObject.NodeName != "SCRIPT")
						{
							string nodeName;
							if ((nodeName = domElement.NodeName) != null)
							{
								if (nodeName == "BR")
								{
									sb.Append(Environment.NewLine);
									continue;
								}
								if (nodeName == "PRE")
								{
									this.RemoveTrailingWhitespace(sb);
									sb.Append(Environment.NewLine);
									sb.Append(this.ToStandardLineEndings(domObject.TextContent));
									this.RemoveTrailingWhitespace(sb);
									sb.Append(Environment.NewLine);
									skipWhitespace = true;
									continue;
								}
								if (nodeName == "A")
								{
									sb.Append(domObject.TextContent + " (" + domObject["href"] + ")");
									continue;
								}
							}
							if (domElement.IsBlock && sb.Length > 0)
							{
								this.RemoveTrailingWhitespace(sb);
								sb.Append(Environment.NewLine);
							}
							this.AddContents(sb, domObject, domElement.IsBlock);
							this.RemoveTrailingWhitespace(sb);
							if (domElement.IsBlock)
							{
								sb.Append(Environment.NewLine);
								skipWhitespace = true;
							}
						}
					}
				}
			}
		}

		protected string ToStandardLineEndings(string text)
		{
			return text.Replace("\r\n", "\n").Replace("\n", "\r\n");
		}

		protected void RemoveTrailingWhitespace(StringBuilder sb)
		{
			int num = sb.Length - 1;
			int num2 = 0;
			while (num >= 0 && CharacterData.IsType(sb[num], CharacterType.Whitespace))
			{
				num--;
				num2++;
			}
			if (num < sb.Length - 1)
			{
				sb.Remove(num + 1, num2);
			}
		}

		protected string CleanFragment(string text)
		{
			ICharacterInfo characterInfo = CharacterData.CreateCharacterInfo();
			StringBuilder stringBuilder = new StringBuilder();
			int i = 0;
			bool flag = true;
			while (i < text.Length)
			{
				characterInfo.Target = text[i];
				if (!flag && !characterInfo.Whitespace)
				{
					flag = true;
				}
				if (flag)
				{
					if (characterInfo.Whitespace)
					{
						stringBuilder.Append(" ");
						flag = false;
					}
					else
					{
						stringBuilder.Append(text[i]);
					}
				}
				i++;
			}
			return stringBuilder.ToString();
		}

		IStringInfo stringInfo;
	}
}
