using System;
using System.Collections.Generic;
using Telerik.Windows.Documents.Flow.FormatProviders.Rtf.Import.Model;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Rtf.Import.TagHandlers
{
	static class BorderHandlers
	{
		public static void InitializeBorderHandlers(Dictionary<string, ControlTagHandler> tagHandlers)
		{
			tagHandlers["brdrw"] = new ControlTagHandler(BorderHandlers.BorderSettingsHandler);
			tagHandlers["brdrs"] = new ControlTagHandler(BorderHandlers.BorderSettingsHandler);
			tagHandlers["brdrth"] = new ControlTagHandler(BorderHandlers.BorderSettingsHandler);
			tagHandlers["brdrcf"] = new ControlTagHandler(BorderHandlers.BorderSettingsHandler);
			tagHandlers["brdrsh"] = new ControlTagHandler(BorderHandlers.BorderSettingsHandler);
			tagHandlers["brdrdb"] = new ControlTagHandler(BorderHandlers.BorderSettingsHandler);
			tagHandlers["brdrdot"] = new ControlTagHandler(BorderHandlers.BorderSettingsHandler);
			tagHandlers["brdrdash"] = new ControlTagHandler(BorderHandlers.BorderSettingsHandler);
			tagHandlers["brdrhair"] = new ControlTagHandler(BorderHandlers.BorderSettingsHandler);
			tagHandlers["brdrdashsm"] = new ControlTagHandler(BorderHandlers.BorderSettingsHandler);
			tagHandlers["brdrdashd"] = new ControlTagHandler(BorderHandlers.BorderSettingsHandler);
			tagHandlers["brdrdashdd"] = new ControlTagHandler(BorderHandlers.BorderSettingsHandler);
			tagHandlers["brdrdashdot"] = new ControlTagHandler(BorderHandlers.BorderSettingsHandler);
			tagHandlers["brdrdashdotdot"] = new ControlTagHandler(BorderHandlers.BorderSettingsHandler);
			tagHandlers["brdrinset"] = new ControlTagHandler(BorderHandlers.BorderSettingsHandler);
			tagHandlers["brdroutset"] = new ControlTagHandler(BorderHandlers.BorderSettingsHandler);
			tagHandlers["brdrtriple"] = new ControlTagHandler(BorderHandlers.BorderSettingsHandler);
			tagHandlers["brdrtnthsg"] = new ControlTagHandler(BorderHandlers.BorderSettingsHandler);
			tagHandlers["brdrthtnsg"] = new ControlTagHandler(BorderHandlers.BorderSettingsHandler);
			tagHandlers["brdrtnthtnsg"] = new ControlTagHandler(BorderHandlers.BorderSettingsHandler);
			tagHandlers["brdrtnthmg"] = new ControlTagHandler(BorderHandlers.BorderSettingsHandler);
			tagHandlers["brdrthtnmg"] = new ControlTagHandler(BorderHandlers.BorderSettingsHandler);
			tagHandlers["brdrtnthtnmg"] = new ControlTagHandler(BorderHandlers.BorderSettingsHandler);
			tagHandlers["brdrtnthlg"] = new ControlTagHandler(BorderHandlers.BorderSettingsHandler);
			tagHandlers["brdrthtnlg"] = new ControlTagHandler(BorderHandlers.BorderSettingsHandler);
			tagHandlers["brdrtnthtnlg"] = new ControlTagHandler(BorderHandlers.BorderSettingsHandler);
			tagHandlers["brdrwavy"] = new ControlTagHandler(BorderHandlers.BorderSettingsHandler);
			tagHandlers["brdrwavydb"] = new ControlTagHandler(BorderHandlers.BorderSettingsHandler);
			tagHandlers["brdrdashdotstr"] = new ControlTagHandler(BorderHandlers.BorderSettingsHandler);
			tagHandlers["brdremboss"] = new ControlTagHandler(BorderHandlers.BorderSettingsHandler);
			tagHandlers["brdrengrave"] = new ControlTagHandler(BorderHandlers.BorderSettingsHandler);
			tagHandlers["brdrnone"] = new ControlTagHandler(BorderHandlers.BorderSettingsHandler);
		}

		static void BorderSettingsHandler(RtfTag tag, RtfImportContext context)
		{
			if (context.CurrentStyle.CurrentBorder != null)
			{
				context.CurrentStyle.CurrentBorder.HandleTag(tag, context);
			}
		}
	}
}
