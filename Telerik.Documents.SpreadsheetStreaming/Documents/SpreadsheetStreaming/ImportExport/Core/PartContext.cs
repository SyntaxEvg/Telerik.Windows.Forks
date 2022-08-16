using System;
using Telerik.Documents.SpreadsheetStreaming.Model.Themes;
using Telerik.Windows.Zip;

namespace Telerik.Documents.SpreadsheetStreaming.ImportExport.Core
{
	class PartContext
	{
		public PartContext(ZipArchive archive)
			: this(archive, null, null, null)
		{
		}

		public PartContext(ZipArchive archive, string name)
			: this(archive, null, name, null)
		{
		}

		public PartContext(ZipArchive archive, string name, string path)
			: this(archive, null, name, path)
		{
		}

		public PartContext(ZipArchive archive, SpreadTheme theme)
			: this(archive, theme, null)
		{
		}

		public PartContext(ZipArchive archive, SpreadTheme theme, string name)
			: this(archive, theme, name, null)
		{
		}

		public PartContext(ZipArchive archive, SpreadTheme theme, string name, string path)
		{
			this.Archive = archive;
			this.Theme = theme;
			if (name != null)
			{
				this.Name = name.ToLowerInvariant();
			}
			this.Path = path;
		}

		public ZipArchive Archive { get; set; }

		public SpreadTheme Theme { get; set; }

		public string Path { get; set; }

		public string Name { get; set; }
	}
}
