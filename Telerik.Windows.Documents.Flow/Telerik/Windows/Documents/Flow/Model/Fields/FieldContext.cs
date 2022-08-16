using System;
using System.Collections.Generic;
using System.Linq;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Flow.Model.Fields
{
	class FieldContext
	{
		public FieldContext(bool isImporting)
		{
			this.isImporting = isImporting;
			this.currentFieldCharacters = new List<FieldContext.FieldCharacterInfo>();
		}

		public bool IsInInstruction
		{
			get
			{
				return this.currentFieldCharacters.Any<FieldContext.FieldCharacterInfo>() && this.currentFieldCharacters.Last<FieldContext.FieldCharacterInfo>().Character.FieldCharacterType == FieldCharacterType.Start;
			}
		}

		protected IEnumerable<FieldCharacter> CurrentFieldCharacters
		{
			get
			{
				return from info in this.currentFieldCharacters
					select info.Character;
			}
		}

		public void OnFieldCharacter(FieldCharacter fieldChar)
		{
			FieldContext.FieldCharacterInfo item = new FieldContext.FieldCharacterInfo(fieldChar);
			switch (fieldChar.FieldCharacterType)
			{
			case FieldCharacterType.Start:
				this.currentFieldCharacters.Add(item);
				return;
			case FieldCharacterType.End:
				if (this.currentFieldCharacters.Any<FieldContext.FieldCharacterInfo>())
				{
					this.OnFieldEnd(fieldChar);
					return;
				}
				this.OnInvalidEndFieldCharacter(fieldChar);
				return;
			case FieldCharacterType.Separator:
				if (this.IsInInstruction)
				{
					this.currentFieldCharacters.Add(item);
					return;
				}
				this.OnInvalidSeparatorFieldCharacter(fieldChar);
				return;
			default:
				return;
			}
		}

		public void SetIsLocked(bool isLocked)
		{
			FieldContext.FieldCharacterInfo currentFieldStartInfo = this.GetCurrentFieldStartInfo();
			if (currentFieldStartInfo == null)
			{
				return;
			}
			currentFieldStartInfo.IsLocked = isLocked;
		}

		public void SetIsDirty(bool isDirty)
		{
			FieldContext.FieldCharacterInfo currentFieldStartInfo = this.GetCurrentFieldStartInfo();
			if (currentFieldStartInfo == null)
			{
				return;
			}
			currentFieldStartInfo.IsDirty = isDirty;
		}

		protected virtual void OnInvalidSeparatorFieldCharacter(FieldCharacter fieldCharacter)
		{
		}

		protected virtual void OnInvalidEndFieldCharacter(FieldCharacter fieldCharacter)
		{
		}

		FieldContext.FieldCharacterInfo GetCurrentFieldStartInfo()
		{
			if (!this.currentFieldCharacters.Any<FieldContext.FieldCharacterInfo>())
			{
				return null;
			}
			FieldContext.FieldCharacterInfo fieldCharacterInfo = this.currentFieldCharacters.Last<FieldContext.FieldCharacterInfo>();
			if (fieldCharacterInfo.Character.FieldCharacterType != FieldCharacterType.Start)
			{
				return null;
			}
			return fieldCharacterInfo;
		}

		void OnFieldEnd(FieldCharacter end)
		{
			FieldContext.FieldCharacterInfo fieldCharacterInfo = this.Pop();
			FieldContext.FieldCharacterInfo fieldCharacterInfo2;
			if (fieldCharacterInfo.Character.FieldCharacterType == FieldCharacterType.Separator)
			{
				fieldCharacterInfo2 = this.Pop();
			}
			else
			{
				fieldCharacterInfo2 = fieldCharacterInfo;
				fieldCharacterInfo = null;
			}
			if (this.isImporting)
			{
				new FieldInfo(fieldCharacterInfo2.Character, (fieldCharacterInfo != null) ? fieldCharacterInfo.Character : null, end)
				{
					IsLocked = fieldCharacterInfo2.IsLocked,
					IsDirty = fieldCharacterInfo2.IsDirty
				}.LoadFieldFromCode();
			}
		}

		FieldContext.FieldCharacterInfo Pop()
		{
			FieldContext.FieldCharacterInfo result = this.currentFieldCharacters[this.currentFieldCharacters.Count - 1];
			this.currentFieldCharacters.RemoveAt(this.currentFieldCharacters.Count - 1);
			return result;
		}

		readonly List<FieldContext.FieldCharacterInfo> currentFieldCharacters;

		readonly bool isImporting;

		class FieldCharacterInfo
		{
			public FieldCharacterInfo(FieldCharacter character)
			{
				Guard.ThrowExceptionIfNull<FieldCharacter>(character, "character");
				this.Character = character;
			}

			public FieldCharacter Character { get; set; }

			public bool IsLocked { get; set; }

			public bool IsDirty { get; set; }
		}
	}
}
