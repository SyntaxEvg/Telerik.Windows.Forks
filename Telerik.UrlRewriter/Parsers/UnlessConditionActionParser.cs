using System;

namespace Telerik.UrlRewriter.Parsers
{
	public class UnlessConditionActionParser : IfConditionActionParser
	{
		public override string Name
		{
			get
			{
				return "unless";
			}
		}
	}
}
