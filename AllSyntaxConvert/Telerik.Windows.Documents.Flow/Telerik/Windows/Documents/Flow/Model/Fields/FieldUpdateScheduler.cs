using System;
using System.Collections.Generic;
using System.Linq;

namespace Telerik.Windows.Documents.Flow.Model.Fields
{
	static class FieldUpdateScheduler
	{
		internal static List<FieldCharacter> GetOrderedListOfFieldToUpdate(IEnumerable<FieldCharacter> fields)
		{
			List<FieldCharacter> list = new List<FieldCharacter>();
			Stack<FieldCharacter> stack = new Stack<FieldCharacter>();
			foreach (FieldCharacter fieldCharacter in fields)
			{
				switch (fieldCharacter.FieldCharacterType)
				{
				case FieldCharacterType.Start:
					if (FieldUpdateScheduler.ShouldAddToUpdateStack(stack))
					{
						list.Add(fieldCharacter);
					}
					stack.Push(fieldCharacter);
					break;
				case FieldCharacterType.End:
				{
					int count = stack.Count;
					FieldUpdateScheduler.OnFieldEnd(stack);
					break;
				}
				case FieldCharacterType.Separator:
					if (stack.Count != 0)
					{
						FieldCharacterType fieldCharacterType = stack.Peek().FieldCharacterType;
					}
					stack.Push(fieldCharacter);
					break;
				}
			}
			list.Reverse();
			return list;
		}

		static void OnFieldEnd(Stack<FieldCharacter> stcurrentFieldsStackack)
		{
			FieldCharacter fieldCharacter = stcurrentFieldsStackack.Pop();
			if (fieldCharacter.FieldCharacterType == FieldCharacterType.Separator)
			{
				stcurrentFieldsStackack.Pop();
				return;
			}
		}

		static bool ShouldAddToUpdateStack(Stack<FieldCharacter> currentFieldsStack)
		{
			return !currentFieldsStack.Any((FieldCharacter fc) => fc.FieldCharacterType == FieldCharacterType.Separator);
		}
	}
}
