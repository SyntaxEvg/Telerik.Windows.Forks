using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using CsQuery.Engine;
using CsQuery.Output;
using HttpWebAdapters;

namespace CsQuery
{
	class CsQueryConfig
	{
		public CsQueryConfig()
		{
			this.DynamicObjectType = typeof(JsObject);
			this.DomRenderingOptions = DomRenderingOptions.QuoteAllAttributes;
			this.HtmlParsingOptions = HtmlParsingOptions.None;
			this.HtmlEncoder = HtmlEncoders.Basic;
			this.DocType = DocType.HTML5;
			this.GetOutputFormatter = new Func<IOutputFormatter>(this.GetDefaultOutputFormatter);
			this.DomIndexProvider = DomIndexProviders.Ranged;
		}

		IOutputFormatter GetDefaultOutputFormatter()
		{
			return OutputFormatters.Create(this.DomRenderingOptions, this.HtmlEncoder);
		}

		IDomIndex GetDomIndex
		{
			get
			{
				return this.DomIndexProvider.GetDomIndex();
			}
		}

		public IDomIndexProvider DomIndexProvider { get; set; }

		public DomRenderingOptions DomRenderingOptions
		{
			get
			{
				return this._DomRenderingOptions;
			}
			set
			{
				if (value.HasFlag(DomRenderingOptions.Default))
				{
					throw new InvalidOperationException("The default DomRenderingOptions cannot contain DomRenderingOptions.Default");
				}
				this._DomRenderingOptions = value;
			}
		}

		public HtmlParsingOptions HtmlParsingOptions
		{
			get
			{
				return this._HtmlParsingOptions;
			}
			set
			{
				if (value.HasFlag(HtmlParsingOptions.Default))
				{
					throw new InvalidOperationException("The default HtmlParsingOptions cannot contain HtmlParsingOptions.Default");
				}
				this._HtmlParsingOptions = value;
			}
		}

		public IHtmlEncoder HtmlEncoder { get; set; }

		public IOutputFormatter OutputFormatter
		{
			get
			{
				if (this.GetOutputFormatter != null)
				{
					return this.GetOutputFormatter();
				}
				return this._OutputFormatter;
			}
			set
			{
				this._OutputFormatter = value;
				this._GetOutputFormatter = null;
			}
		}

		public Func<IOutputFormatter> GetOutputFormatter
		{
			get
			{
				return this._GetOutputFormatter;
			}
			set
			{
				this._GetOutputFormatter = value;
				this._OutputFormatter = null;
			}
		}

		public IHttpWebRequestFactory WebRequestFactory
		{
			get
			{
				if (this._WebRequestFactory == null)
				{
					this._WebRequestFactory = new HttpWebRequestFactory();
				}
				return this._WebRequestFactory;
			}
		}

		public DocType DocType
		{
			get
			{
				return this._DocType;
			}
			set
			{
				if (value == DocType.Default)
				{
					throw new InvalidOperationException("The default DocType cannot be DocType.Default");
				}
				this._DocType = value;
			}
		}

		public Type DynamicObjectType
		{
			get
			{
				return this._DynamicObjectType;
			}
			set
			{
				if ((from item in value.GetInterfaces()
					where item == typeof(IDynamicMetaObjectProvider) || item == typeof(IDictionary<string, object>)
					select item).Count<Type>() == 2)
				{
					this._DynamicObjectType = value;
					return;
				}
				throw new ArgumentException("The DynamicObjectType must inherit IDynamicMetaObjectProvider and IDictionary<string,object>. Example: ExpandoObject, or the built-in JsObject.");
			}
		}

		DocType _DocType;

		DomRenderingOptions _DomRenderingOptions;

		HtmlParsingOptions _HtmlParsingOptions;

		IOutputFormatter _OutputFormatter;

		Func<IOutputFormatter> _GetOutputFormatter;

		Type _DynamicObjectType;

		IHttpWebRequestFactory _WebRequestFactory;
	}
}
