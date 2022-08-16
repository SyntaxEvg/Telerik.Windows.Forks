using System;
using CsQuery.Output;

namespace CsQuery
{
	static class HtmlEncoders
	{
		public static IHtmlEncoder Default
		{
			get
			{
				return Config.HtmlEncoder;
			}
		}

		public static IHtmlEncoder Basic = new HtmlEncoderBasic();

		public static IHtmlEncoder Minimum = new HtmlEncoderMinimum();

		public static IHtmlEncoder MinimumNbsp = new HtmlEncoderMinimumNbsp();

		public static IHtmlEncoder None = new HtmlEncoderNone();

		public static IHtmlEncoder Full = new HtmlEncoderFull();
	}
}
