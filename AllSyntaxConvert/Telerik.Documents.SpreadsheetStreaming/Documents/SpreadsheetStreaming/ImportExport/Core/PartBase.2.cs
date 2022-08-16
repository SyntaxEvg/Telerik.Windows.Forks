using System;
using System.IO;
using System.Linq;
using Telerik.Windows.Zip;

namespace Telerik.Documents.SpreadsheetStreaming.ImportExport.Core
{
	abstract class PartBase<T> : PartBase where T : ElementBase, new()
	{
		internal PartBase(PartContext context)
			: base(context.Name)
		{
			string path = context.Path;
			if (string.IsNullOrEmpty(path))
			{
				path = base.GetPartFullPath();
			}
			ZipArchive archive = context.Archive;
			this.entry = (from p in archive.Entries
				where p.FullName == path
				select p).FirstOrDefault<ZipArchiveEntry>();
			if (this.entry != null)
			{
				this.stream = this.entry.Open();
				this.reader = new OpenXmlReader(this.stream);
			}
			else
			{
				this.entry = context.Archive.CreateEntry(path);
				this.stream = this.entry.Open();
				this.writer = new OpenXmlWriter(this.stream);
			}
			ElementContext context2 = new ElementContext(this.writer, this.reader, context.Theme);
			this.RootElement = Activator.CreateInstance<T>();
			T rootElement = this.RootElement;
			rootElement.SetContext(context2);
		}

		public T RootElement { get; set; }

		public OpenXmlReader Reader
		{
			get
			{
				return this.reader;
			}
		}

		protected OpenXmlWriter Writer
		{
			get
			{
				return this.writer;
			}
		}

		internal override void CompleteWriteOverride()
		{
			if (this.Writer != null)
			{
				this.Writer.Flush();
			}
			this.entry.Dispose();
			this.stream.Dispose();
		}

		readonly ZipArchiveEntry entry;

		readonly Stream stream;

		readonly OpenXmlWriter writer;

		readonly OpenXmlReader reader;
	}
}
