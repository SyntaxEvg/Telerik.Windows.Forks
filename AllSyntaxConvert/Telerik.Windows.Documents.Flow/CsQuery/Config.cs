using System;
using CsQuery.Engine;
using CsQuery.Output;
using HttpWebAdapters;

namespace CsQuery
{
	static class Config
	{
		static Config()
		{
			Config.DefaultConfig = new CsQueryConfig();
		}

		public static PseudoSelectors PseudoClassFilters
		{
			get
			{
				return PseudoSelectors.Items;
			}
		}

		public static DomRenderingOptions DomRenderingOptions
		{
			get
			{
				return Config.DefaultConfig.DomRenderingOptions;
			}
			set
			{
				Config.DefaultConfig.DomRenderingOptions = value;
			}
		}

		public static HtmlParsingOptions HtmlParsingOptions
		{
			get
			{
				return Config.DefaultConfig.HtmlParsingOptions;
			}
			set
			{
				Config.DefaultConfig.HtmlParsingOptions = value;
			}
		}

		public static IHtmlEncoder HtmlEncoder
		{
			get
			{
				return Config.DefaultConfig.HtmlEncoder;
			}
			set
			{
				Config.DefaultConfig.HtmlEncoder = value;
			}
		}

		public static IOutputFormatter OutputFormatter
		{
			get
			{
				return Config.DefaultConfig.OutputFormatter;
			}
			set
			{
				Config.DefaultConfig.OutputFormatter = value;
			}
		}

		public static Func<IOutputFormatter> GetOutputFormatter
		{
			get
			{
				return Config.DefaultConfig.GetOutputFormatter;
			}
			set
			{
				Config.DefaultConfig.GetOutputFormatter = value;
			}
		}

		public static IHttpWebRequestFactory WebRequestFactory
		{
			get
			{
				return Config.DefaultConfig.WebRequestFactory;
			}
		}

		public static DocType DocType
		{
			get
			{
				return Config.DefaultConfig.DocType;
			}
			set
			{
				Config.DefaultConfig.DocType = value;
			}
		}

		public static Type DynamicObjectType
		{
			get
			{
				return Config.DefaultConfig.DynamicObjectType;
			}
			set
			{
				Config.DefaultConfig.DynamicObjectType = value;
			}
		}

		public static IDomIndexProvider DomIndexProvider
		{
			get
			{
				return Config.DefaultConfig.DomIndexProvider;
			}
			set
			{
				Config.DefaultConfig.DomIndexProvider = value;
			}
		}

		static CsQueryConfig DefaultConfig;

		public static StartupOptions StartupOptions = StartupOptions.LookForExtensions;
	}
}
