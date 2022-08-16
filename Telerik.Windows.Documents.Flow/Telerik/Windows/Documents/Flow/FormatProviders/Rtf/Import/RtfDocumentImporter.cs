using System;
using System.Collections.Generic;
using System.IO;
using Telerik.Windows.Documents.Flow.FormatProviders.Rtf.Import.Model;
using Telerik.Windows.Documents.Flow.FormatProviders.Rtf.Import.Parser;
using Telerik.Windows.Documents.Flow.FormatProviders.Rtf.Import.TagHandlers;
using Telerik.Windows.Documents.Flow.FormatProviders.Rtf.Utilities;
using Telerik.Windows.Documents.Flow.Model;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Rtf.Import
{
	class RtfDocumentImporter : RtfElementIteratorBase
	{
		static RtfDocumentImporter()
		{
			RtfTagHandlers.InitializeAllTagHandlers(RtfDocumentImporter.TagHandlers);
			RtfDocumentImporter.GroupHandlers = new Dictionary<string, ControlGroupHandler>();
			GroupTagHandlers.InitializeGroupHandlers(RtfDocumentImporter.GroupHandlers);
		}

		public RtfDocumentImporter()
		{
		}

		public RtfDocumentImporter(RtfImportContext context)
		{
			this.context = context;
		}

		public RtfImportContext Context
		{
			get
			{
				return this.context;
			}
		}

		public RadFlowDocument Import(Stream input)
		{
			RtfSource rtfTextSource = new RtfSource(input);
			RtfParserListenerStructureBuilder rtfParserListenerStructureBuilder = new RtfParserListenerStructureBuilder();
			RadFlowDocument document;
			using (RtfParser rtfParser = new RtfParser(new RtfParserListenerBase[] { rtfParserListenerStructureBuilder }))
			{
				rtfParser.Parse(rtfTextSource);
				this.context = new RtfImportContext();
				Util.EnsureGroupDestination(rtfParserListenerStructureBuilder.StructureRoot, "rtf");
				this.ImportRoot(rtfParserListenerStructureBuilder.StructureRoot);
				this.context.ClearUnclosedBookmarkRanges();
				document = this.context.Document;
			}
			return document;
		}

		public void ImportRoot(RtfGroup rtfGroup)
		{
			base.VisitGroupChildren(rtfGroup, false);
			this.context.FlushSection();
			RedundantPropertiesProcessor.ClearRedundantLocalProperties(this.context.Document);
		}

		protected override void DoVisitGroup(RtfGroup group)
		{
			ControlGroupHandler controlGroupHandler = null;
			if (group.Destination != null)
			{
				RtfDocumentImporter.GroupHandlers.TryGetValue(group.Destination, out controlGroupHandler);
			}
			if (controlGroupHandler != null)
			{
				this.context.EnterGroup(group);
				bool flag = controlGroupHandler(group, this.context);
				if (flag)
				{
					base.VisitGroupChildren(group, false);
				}
				this.context.LeaveGroup(group);
				return;
			}
			if (!group.IsExtensionDestination)
			{
				this.context.EnterGroup(group);
				base.VisitGroupChildren(group, false);
				this.context.LeaveGroup(group);
			}
		}

		protected override void DoVisitTag(RtfTag tag)
		{
			ControlTagHandler controlTagHandler;
			RtfDocumentImporter.TagHandlers.TryGetValue(tag.Name, out controlTagHandler);
			if (controlTagHandler != null)
			{
				controlTagHandler(tag, this.context);
			}
		}

		protected override void DoVisitText(RtfText text)
		{
			this.context.AddText(text.Text);
		}

		static readonly Dictionary<string, ControlTagHandler> TagHandlers = new Dictionary<string, ControlTagHandler>();

		static readonly Dictionary<string, ControlGroupHandler> GroupHandlers;

		RtfImportContext context;
	}
}
