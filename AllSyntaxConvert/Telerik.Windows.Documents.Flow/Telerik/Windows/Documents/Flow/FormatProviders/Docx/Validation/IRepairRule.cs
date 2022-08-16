using System;
using Telerik.Windows.Documents.Flow.Model;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Docx.Validation
{
	interface IRepairRule
	{
		void Repair(DocumentElementBase documentElement);
	}
}
