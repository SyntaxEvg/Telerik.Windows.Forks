using System;
using System.Collections.Generic;
using Telerik.Documents.SpreadsheetStreaming.ImportExport.Core;
using Telerik.Documents.SpreadsheetStreaming.Model.ContentTypes;

namespace Telerik.Documents.SpreadsheetStreaming
{
	class ContentTypesRepository
	{
		public ContentTypesRepository()
		{
			this.defaults = new Dictionary<string, DefaultContentType>();
			this.overrides = new HashSet<OverrideContentType>();
			this.RegisterDefaultContentType("xml");
		}

		public IEnumerable<DefaultContentType> Defaults
		{
			get
			{
				return this.defaults.Values;
			}
		}

		public IEnumerable<OverrideContentType> Overrides
		{
			get
			{
				return this.overrides;
			}
		}

		public void Register(PartBase part)
		{
			this.Register(part.GetPartFullPath(), part.ContentType);
		}

		public void Register(DefaultContentType value)
		{
			this.defaults[value.Extension] = value;
		}

		public void Register(OverrideContentType value)
		{
			if (!this.overrides.Contains(value))
			{
				this.overrides.Add(value);
			}
		}

		internal void Register(string path, string contentType)
		{
			path = '/' + path;
			this.RegisterOverrideContentType(path, contentType);
			this.RegisterDefaultContentType(path);
		}

		void RegisterOverrideContentType(string path, string contentType)
		{
			OverrideContentType item = new OverrideContentType(path, contentType);
			if (!this.overrides.Contains(item))
			{
				this.overrides.Add(item);
			}
		}

		void RegisterDefaultContentType(string path)
		{
			string extension = OpenXmlHelper.GetExtension(path);
			string contentType;
			if (OpenXmlHelper.TryGetContentTypeByExtension(extension, out contentType))
			{
				DefaultContentType value = new DefaultContentType(extension, contentType);
				if (!this.defaults.ContainsKey(extension))
				{
					this.defaults.Add(extension, value);
				}
			}
		}

		readonly Dictionary<string, DefaultContentType> defaults;

		readonly HashSet<OverrideContentType> overrides;
	}
}
