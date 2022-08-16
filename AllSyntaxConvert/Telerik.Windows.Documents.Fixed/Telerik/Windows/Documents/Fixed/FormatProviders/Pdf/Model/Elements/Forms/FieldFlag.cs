using System;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.Forms
{
	enum FieldFlag
	{
		ReadOnly = 1,
		Required,
		NoExport,
		NoToggleToOff = 15,
		Radio,
		PushButton,
		RadiosInUnison = 26,
		Multiline = 13,
		Password,
		FileSelect = 21,
		DoNotScroll = 24,
		Comb,
		RichText,
		Combo = 18,
		Edit,
		Sort,
		MultiSelect = 22,
		CommitOnSelChange = 27,
		DoNotSpellCheck = 23
	}
}
