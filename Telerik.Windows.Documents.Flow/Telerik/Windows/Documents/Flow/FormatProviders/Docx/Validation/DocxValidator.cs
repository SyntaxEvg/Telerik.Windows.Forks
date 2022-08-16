using System;
using System.Collections.Generic;
using System.Linq;
using Telerik.Windows.Documents.Flow.Model;
using Telerik.Windows.Documents.Flow.Model.Shapes;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model;
using Telerik.Windows.Documents.Model.Drawing.Shapes;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Docx.Validation
{
	public static class DocxValidator
	{
		static DocxValidator()
		{
			DocxValidator.RegisterRules<CommentRangeStart>(DocumentElementType.CommentRangeStart, (CommentRangeStart cr) => cr.Comment.CommentRangeEnd.Paragraph != null, delegate(CommentRangeStart cr)
			{
				Paragraph paragraph = cr.Comment.CommentRangeStart.Paragraph;
				int index = paragraph.Inlines.IndexOf(cr.Comment.CommentRangeStart);
				paragraph.Inlines[index] = cr.Comment.CommentRangeEnd;
			}, "CommentRangeStart element cannot exists without corresponding end.", true);
			DocxValidator.RegisterRules<Table>(DocumentElementType.Table, (Table t) => t.Children.Any<DocumentElementBase>(), delegate(Table t)
			{
				t.Rows.AddTableRow().Cells.AddTableCell().Blocks.AddParagraph();
			}, "Table element cannot be empty.", false);
			DocxValidator.RegisterRules<TableRow>(DocumentElementType.TableRow, (TableRow tr) => tr.Children.Any<DocumentElementBase>(), delegate(TableRow tr)
			{
				tr.Cells.AddTableCell().Blocks.AddParagraph();
			}, "Table row element cannot be empty.", false);
			DocxValidator.RegisterRules<TableCell>(DocumentElementType.TableCell, delegate(TableCell tc)
			{
				DocumentElementBase documentElementBase = tc.Children.LastOrDefault<DocumentElementBase>();
				return documentElementBase != null && documentElementBase.Type == DocumentElementType.Paragraph;
			}, delegate(TableCell tc)
			{
				tc.Blocks.AddParagraph();
			}, "Table cell element cannot be empty.", false);
			DocxValidator.RegisterRules<Section>(DocumentElementType.Section, (Section s) => s == s.Document.Sections.Last<Section>() || s.Children.Any<DocumentElementBase>(), delegate(Section s)
			{
				s.Blocks.AddParagraph();
			}, "Only last section can be empty.", false);
			DocxValidator.RegisterRules<Section>(DocumentElementType.Section, (Section s) => s == s.Document.Sections.Last<Section>() || (s.Blocks.Count != 0 && s.Blocks.Last<BlockBase>() is Paragraph), delegate(Section s)
			{
				s.Blocks.AddParagraph();
			}, "Last element in section should be paragraph to preserve the section properties.", false);
			DocxValidator.RegisterRules<ImageInline>(DocumentElementType.ImageInline, (ImageInline i) => i.Image.ImageSource == null || OpenXmlHelper.IsSupportedImageSourceExtension(i.Image.ImageSource.Extension), delegate(ImageInline i)
			{
				i.Image.ImageSource = null;
			}, "Image source format is not supported.", false);
			DocxValidator.RegisterRules<ImageInline>(DocumentElementType.ImageInline, (ImageInline i) => !i.Image.Size.IsEmpty, delegate(ImageInline i)
			{
				i.Image.Size = ShapeBase.DefaultSize;
			}, "Image size cannot be Size.Empty.", false);
			DocxValidator.RegisterRules<FloatingImage>(DocumentElementType.FloatingImage, (FloatingImage fi) => !fi.Image.Size.IsEmpty, delegate(FloatingImage fi)
			{
				fi.Image.Size = ShapeBase.DefaultSize;
			}, "Image size cannot be Size.Empty.", false);
		}

		public static ValidationResult Validate(RadFlowDocument document)
		{
			Guard.ThrowExceptionIfNull<RadFlowDocument>(document, "document");
			IEnumerable<ValidationError> validationErrors = DocxValidator.GetValidationErrors(document);
			if (validationErrors.Any<ValidationError>())
			{
				return new ValidationResult(validationErrors);
			}
			return new ValidationResult();
		}

		public static void Repair(RadFlowDocument document)
		{
			Guard.ThrowExceptionIfNull<RadFlowDocument>(document, "document");
			foreach (DocumentElementBase documentElement in DocxValidator.EnumerateDocumentElements(document))
			{
				DocxValidator.RepairDocumentElement(documentElement);
			}
			foreach (KeyValuePair<DocumentElementBase, IValidationRule> keyValuePair in DocxValidator.pendingRepairElements)
			{
				DocxValidator.TryRepairDocumentElement(keyValuePair.Key, keyValuePair.Value);
			}
		}

		static IEnumerable<ValidationError> GetValidationErrors(RadFlowDocument document)
		{
			Guard.ThrowExceptionIfNull<RadFlowDocument>(document, "document");
			foreach (DocumentElementBase child in DocxValidator.EnumerateDocumentElements(document))
			{
				foreach (ValidationError error in DocxValidator.GetValidationErrorsForDocumentElement(child))
				{
					yield return error;
				}
			}
			yield break;
		}

		static IEnumerable<ValidationError> GetValidationErrorsForDocumentElement(DocumentElementBase documentElement)
		{
			Guard.ThrowExceptionIfNull<DocumentElementBase>(documentElement, "documentElement");
			List<IValidationRule> rules;
			if (DocxValidator.validationRules.TryGetValue(documentElement.Type, out rules))
			{
				foreach (IValidationRule rule in rules)
				{
					if (!rule.IsCompliant(documentElement))
					{
						yield return new ValidationError(documentElement, rule.ErrorMessage);
					}
				}
			}
			yield break;
		}

		static void RepairDocumentElement(DocumentElementBase documentElement)
		{
			Guard.ThrowExceptionIfNull<DocumentElementBase>(documentElement, "documentElement");
			List<IValidationRule> list;
			if (DocxValidator.validationRules.TryGetValue(documentElement.Type, out list))
			{
				foreach (IValidationRule validationRule in list)
				{
					if (validationRule.IsDocumentChanging)
					{
						DocxValidator.pendingRepairElements.Add(new KeyValuePair<DocumentElementBase, IValidationRule>(documentElement, validationRule));
					}
					else
					{
						DocxValidator.TryRepairDocumentElement(documentElement, validationRule);
					}
				}
			}
		}

		static void TryRepairDocumentElement(DocumentElementBase documentElement, IValidationRule rule)
		{
			IRepairRule repairRule;
			if (!rule.IsCompliant(documentElement) && DocxValidator.repairRules.TryGetValue(rule, out repairRule))
			{
				repairRule.Repair(documentElement);
			}
		}

		static IEnumerable<DocumentElementBase> EnumerateDocumentElements(DocumentElementBase root)
		{
			Guard.ThrowExceptionIfNull<DocumentElementBase>(root, "root");
			yield return root;
			foreach (DocumentElementBase child in root.Children)
			{
				foreach (DocumentElementBase innerChild in DocxValidator.EnumerateDocumentElements(child))
				{
					yield return innerChild;
				}
			}
			yield break;
		}

		static void RegisterRules<T>(DocumentElementType elementType, Func<T, bool> predicate, Action<T> repairAction, string errorMessage, bool isDocumentChanging = false) where T : DocumentElementBase
		{
			Guard.ThrowExceptionIfNull<Func<T, bool>>(predicate, "predicate");
			Guard.ThrowExceptionIfNull<Action<T>>(repairAction, "repairAction");
			Guard.ThrowExceptionIfNullOrEmpty(errorMessage, "errorMessage");
			List<IValidationRule> list;
			if (!DocxValidator.validationRules.TryGetValue(elementType, out list))
			{
				list = new List<IValidationRule>();
				DocxValidator.validationRules[elementType] = list;
			}
			IValidationRule validationRule = new ValidationRule<T>(predicate, errorMessage, isDocumentChanging);
			list.Add(validationRule);
			DocxValidator.repairRules[validationRule] = new RepairRule<T>(repairAction);
		}

		static readonly Dictionary<DocumentElementType, List<IValidationRule>> validationRules = new Dictionary<DocumentElementType, List<IValidationRule>>();

		static readonly Dictionary<IValidationRule, IRepairRule> repairRules = new Dictionary<IValidationRule, IRepairRule>();

		static readonly List<KeyValuePair<DocumentElementBase, IValidationRule>> pendingRepairElements = new List<KeyValuePair<DocumentElementBase, IValidationRule>>();
	}
}
