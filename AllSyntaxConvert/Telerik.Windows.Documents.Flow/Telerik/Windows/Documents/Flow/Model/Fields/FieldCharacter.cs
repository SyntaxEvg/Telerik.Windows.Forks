using System;
using Telerik.Windows.Documents.Flow.Model.Cloning;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Flow.Model.Fields
{
	public sealed class FieldCharacter : AnnotationMarkerBase
	{
		internal FieldCharacter(RadFlowDocument document, FieldCharacterType type)
			: base(document)
		{
			this.fieldCharacterType = type;
		}

		public FieldCharacterType FieldCharacterType
		{
			get
			{
				return this.fieldCharacterType;
			}
		}

		public FieldInfo FieldInfo { get; internal set; }

		internal override DocumentElementType Type
		{
			get
			{
				return DocumentElementType.FieldCharacter;
			}
		}

		internal override DocumentElementBase CloneCore(CloneContext cloneContext)
		{
			Guard.ThrowExceptionIfNull<CloneContext>(cloneContext, "cloneContext");
			return new FieldCharacter(cloneContext.Document, this.FieldCharacterType);
		}

		internal override void OnAfterCloneCore(CloneContext cloneContext, DocumentElementBase clonedElement)
		{
			cloneContext.FieldContext.OnFieldCharacter((FieldCharacter)clonedElement);
			if (this.fieldCharacterType == FieldCharacterType.Start && this.FieldInfo != null)
			{
				cloneContext.FieldContext.SetIsDirty(this.FieldInfo.IsDirty);
				cloneContext.FieldContext.SetIsLocked(this.FieldInfo.IsLocked);
			}
		}

		internal override string GetPropertiesDebuggerDisplayText()
		{
			return string.Format("{0} FieldCharacterType=\"{1}\"", base.GetPropertiesDebuggerDisplayText(), this.FieldCharacterType);
		}

		readonly FieldCharacterType fieldCharacterType;
	}
}
