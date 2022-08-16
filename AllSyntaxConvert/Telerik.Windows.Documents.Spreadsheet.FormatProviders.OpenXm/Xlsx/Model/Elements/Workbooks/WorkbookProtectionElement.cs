using System;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Core;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders.Contexts;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Export;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Import;
using Telerik.Windows.Documents.Spreadsheet.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Model.Elements.Workbooks
{
	class WorkbookProtectionElement : WorkbookElementBase
	{
		public WorkbookProtectionElement(XlsxPartsManager partsManager)
			: base(partsManager)
		{
			this.workbookAlgorithmName = base.RegisterAttribute<string>("workbookAlgorithmName", string.Empty, false);
			this.workbookHashValue = base.RegisterAttribute<string>("workbookHashValue", string.Empty, false);
			this.workbookSaltValue = base.RegisterAttribute<string>("workbookSaltValue", string.Empty, false);
			this.workbookSpinCount = base.RegisterAttribute<IntOpenXmlAttribute>(new IntOpenXmlAttribute("workbookSpinCount", false));
			this.password = base.RegisterAttribute<string>("workbookPassword", string.Empty, false);
			this.lockStructure = base.RegisterAttribute<BoolOpenXmlAttribute>(new BoolOpenXmlAttribute("lockStructure"));
			this.lockWindows = base.RegisterAttribute<BoolOpenXmlAttribute>(new BoolOpenXmlAttribute("lockWindows"));
		}

		public override string ElementName
		{
			get
			{
				return "workbookProtection";
			}
		}

		public bool LockStructure
		{
			get
			{
				return this.lockStructure.Value;
			}
			set
			{
				this.lockStructure.Value = value;
			}
		}

		public bool LockWindows
		{
			get
			{
				return this.lockWindows.Value;
			}
			set
			{
				this.lockWindows.Value = value;
			}
		}

		public string WorkbookAlgorithmName
		{
			get
			{
				return this.workbookAlgorithmName.Value;
			}
			set
			{
				this.workbookAlgorithmName.Value = value;
			}
		}

		public string WorkbookHashValue
		{
			get
			{
				return this.workbookHashValue.Value;
			}
			set
			{
				this.workbookHashValue.Value = value;
			}
		}

		public string WorkbookSaltValue
		{
			get
			{
				return this.workbookSaltValue.Value;
			}
			set
			{
				this.workbookSaltValue.Value = value;
			}
		}

		public int WorkbookSpinCount
		{
			get
			{
				return this.workbookSpinCount.Value;
			}
			set
			{
				this.workbookSpinCount.Value = value;
			}
		}

		public string Password
		{
			get
			{
				return this.password.Value;
			}
			set
			{
				this.password.Value = value;
			}
		}

		protected override bool ShouldExport(IXlsxWorkbookExportContext context)
		{
			Guard.ThrowExceptionIfNull<IXlsxWorkbookExportContext>(context, "context");
			return context.GetWorkbookProtectionInfo().Enforced;
		}

		protected override void OnBeforeWrite(IXlsxWorkbookExportContext context)
		{
			Guard.ThrowExceptionIfNull<IXlsxWorkbookExportContext>(context, "context");
			WorkbookProtectionInfo workbookProtectionInfo = context.GetWorkbookProtectionInfo();
			if (!string.IsNullOrEmpty(workbookProtectionInfo.Password))
			{
				this.Password = workbookProtectionInfo.Password;
			}
			else if (!string.IsNullOrEmpty(workbookProtectionInfo.HashValue))
			{
				this.WorkbookAlgorithmName = workbookProtectionInfo.AlgorithmName;
				this.WorkbookHashValue = workbookProtectionInfo.HashValue;
				this.WorkbookSpinCount = workbookProtectionInfo.SpinCount;
				if (!string.IsNullOrEmpty(workbookProtectionInfo.SaltValue))
				{
					this.WorkbookSaltValue = workbookProtectionInfo.SaltValue;
				}
			}
			if (this.lockStructure.DefaultValue != workbookProtectionInfo.LockStructure)
			{
				this.LockStructure = workbookProtectionInfo.LockStructure;
			}
			if (this.lockWindows.DefaultValue != workbookProtectionInfo.LockWindows)
			{
				this.LockWindows = workbookProtectionInfo.LockWindows;
			}
		}

		protected override void OnAfterRead(IXlsxWorkbookImportContext context)
		{
			Guard.ThrowExceptionIfNull<IXlsxWorkbookImportContext>(context, "context");
			WorkbookProtectionInfo workbookProtectionInfo = new WorkbookProtectionInfo();
			if (string.IsNullOrEmpty(this.Password))
			{
				workbookProtectionInfo.AlgorithmName = this.WorkbookAlgorithmName;
				workbookProtectionInfo.HashValue = this.WorkbookHashValue;
				workbookProtectionInfo.SaltValue = this.WorkbookSaltValue;
				workbookProtectionInfo.SpinCount = this.WorkbookSpinCount;
			}
			else
			{
				workbookProtectionInfo.Password = this.Password;
			}
			workbookProtectionInfo.LockStructure = this.LockStructure;
			workbookProtectionInfo.LockWindows = this.LockWindows;
			workbookProtectionInfo.Enforced = workbookProtectionInfo.LockStructure || workbookProtectionInfo.LockWindows;
			context.ApplyWorkbookProtectionInfo(workbookProtectionInfo);
		}

		readonly OpenXmlAttribute<string> workbookAlgorithmName;

		readonly OpenXmlAttribute<string> workbookHashValue;

		readonly OpenXmlAttribute<string> workbookSaltValue;

		readonly IntOpenXmlAttribute workbookSpinCount;

		readonly OpenXmlAttribute<string> password;

		readonly BoolOpenXmlAttribute lockStructure;

		readonly BoolOpenXmlAttribute lockWindows;
	}
}
