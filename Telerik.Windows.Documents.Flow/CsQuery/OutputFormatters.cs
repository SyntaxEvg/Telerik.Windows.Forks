using System;
using CsQuery.Output;

namespace CsQuery
{
	static class OutputFormatters
	{
		public static IOutputFormatter Create(DomRenderingOptions options, IHtmlEncoder encoder)
		{
			return new FormatDefault(options, encoder ?? HtmlEncoders.Default);
		}

		public static IOutputFormatter Create(DomRenderingOptions options)
		{
			return new FormatDefault(options, HtmlEncoders.Default);
		}

		public static IOutputFormatter Create(IHtmlEncoder encoder)
		{
			return new FormatDefault(DomRenderingOptions.Default, encoder);
		}

		public static IOutputFormatter Default
		{
			get
			{
				return Config.OutputFormatter;
			}
		}

		public static IOutputFormatter HtmlEncodingNone
		{
			get
			{
				return OutputFormatters.Create(HtmlEncoders.None);
			}
		}

		public static IOutputFormatter HtmlEncodingBasic
		{
			get
			{
				return OutputFormatters.Create(HtmlEncoders.Basic);
			}
		}

		public static IOutputFormatter HtmlEncodingFull
		{
			get
			{
				return OutputFormatters.Create(HtmlEncoders.Full);
			}
		}

		public static IOutputFormatter HtmlEncodingMinimum
		{
			get
			{
				return OutputFormatters.Create(HtmlEncoders.Minimum);
			}
		}

		public static IOutputFormatter HtmlEncodingMinimumNbsp
		{
			get
			{
				return OutputFormatters.Create(HtmlEncoders.MinimumNbsp);
			}
		}

		public static IOutputFormatter PlainText
		{
			get
			{
				return new FormatPlainText();
			}
		}

		static void MergeOptions(ref DomRenderingOptions options)
		{
			if (options.HasFlag(DomRenderingOptions.Default))
			{
				options = Config.DomRenderingOptions | (options & ~DomRenderingOptions.Default);
			}
		}
	}
}
