using System;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Encryption
{
	enum PermissionFlag
	{
		Print = 3,
		ModifyContent,
		ExtractTextAndGraphics,
		ModifyAnnotationsAndForms,
		FillForms = 9,
		ExtractTextAndGraphicsForAccessibilityToDisabledUsers,
		AssemblePagesBookmarksAndThumbnails,
		PrintWithHighQuality
	}
}
