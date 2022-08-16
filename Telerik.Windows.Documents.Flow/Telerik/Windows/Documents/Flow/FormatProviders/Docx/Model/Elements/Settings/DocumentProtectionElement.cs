using System;
using Telerik.Windows.Documents.Flow.FormatProviders.Docx.Model.Elements.Common;
using Telerik.Windows.Documents.Flow.FormatProviders.Docx.Model.Types;
using Telerik.Windows.Documents.Flow.Model.Protection;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Core;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Docx.Model.Elements.Settings
{
	class DocumentProtectionElement : DocxElementBase
	{
		public DocumentProtectionElement(DocxPartsManager partsManager)
			: base(partsManager)
		{
			this.enforcementAttribute = base.RegisterAttribute<BoolOpenXmlAttribute>(new BoolOpenXmlAttribute("enforcement", OpenXmlNamespaces.WordprocessingMLNamespace));
			this.hashValueAttribute = base.RegisterAttribute<OpenXmlAttribute<string>>(new OpenXmlAttribute<string>("hash", OpenXmlNamespaces.WordprocessingMLNamespace, false));
			this.saltValueAttribute = base.RegisterAttribute<OpenXmlAttribute<string>>(new OpenXmlAttribute<string>("salt", OpenXmlNamespaces.WordprocessingMLNamespace, false));
			this.spinCountAttribute = base.RegisterAttribute<IntOpenXmlAttribute>(new IntOpenXmlAttribute("cryptSpinCount", OpenXmlNamespaces.WordprocessingMLNamespace, false));
			this.protectionModeAttribute = base.RegisterAttribute<MappedOpenXmlAttribute<ProtectionMode>>(new MappedOpenXmlAttribute<ProtectionMode>("edit", OpenXmlNamespaces.WordprocessingMLNamespace, TypeMappers.ProtectionModeTypeMapper, false));
			this.algorithmTypeAttribute = base.RegisterAttribute<MappedOpenXmlAttribute<string>>(new MappedOpenXmlAttribute<string>("cryptAlgorithmSid", OpenXmlNamespaces.WordprocessingMLNamespace, TypeMappers.ProtectionAlgorithmTypeMapper, false));
		}

		public override string ElementName
		{
			get
			{
				return "documentProtection";
			}
		}

		internal ProtectionMode ProtectionMode
		{
			get
			{
				return this.protectionModeAttribute.Value;
			}
			set
			{
				this.protectionModeAttribute.Value = value;
			}
		}

		internal bool Enforcement
		{
			get
			{
				return this.enforcementAttribute.Value;
			}
			set
			{
				this.enforcementAttribute.Value = value;
			}
		}

		internal string AlgorithmName
		{
			get
			{
				return this.algorithmTypeAttribute.Value;
			}
			set
			{
				this.algorithmTypeAttribute.Value = value;
			}
		}

		internal string Salt
		{
			get
			{
				return this.saltValueAttribute.Value;
			}
			set
			{
				this.saltValueAttribute.Value = value;
			}
		}

		internal string Hash
		{
			get
			{
				return this.hashValueAttribute.Value;
			}
			set
			{
				this.hashValueAttribute.Value = value;
			}
		}

		internal int SpinCount
		{
			get
			{
				return this.spinCountAttribute.Value;
			}
			set
			{
				this.spinCountAttribute.Value = value;
			}
		}

		internal void CopyPropertiesTo(ProtectionSettings protectionSettings)
		{
			protectionSettings.Enforced = this.Enforcement;
			protectionSettings.ProtectionMode = this.ProtectionMode;
			protectionSettings.AlgorithmName = this.AlgorithmName;
			protectionSettings.Salt = this.Salt;
			protectionSettings.Hash = this.Hash;
			protectionSettings.SpinCount = this.SpinCount;
		}

		internal void CopyPropertiesFrom(ProtectionSettings protectionSettings)
		{
			this.Enforcement = protectionSettings.Enforced;
			this.ProtectionMode = protectionSettings.ProtectionMode;
			this.AlgorithmName = protectionSettings.AlgorithmName;
			this.Salt = protectionSettings.Salt;
			this.Hash = protectionSettings.Hash;
			this.SpinCount = protectionSettings.SpinCount;
		}

		readonly MappedOpenXmlAttribute<ProtectionMode> protectionModeAttribute;

		readonly BoolOpenXmlAttribute enforcementAttribute;

		readonly OpenXmlAttribute<string> hashValueAttribute;

		readonly OpenXmlAttribute<string> saltValueAttribute;

		readonly IntOpenXmlAttribute spinCountAttribute;

		readonly MappedOpenXmlAttribute<string> algorithmTypeAttribute;
	}
}
