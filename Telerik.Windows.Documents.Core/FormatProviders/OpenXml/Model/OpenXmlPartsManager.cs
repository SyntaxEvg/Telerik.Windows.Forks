using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Telerik.Windows.Documents.Common.FormatProviders.OpenXml.Utilities;
using Telerik.Windows.Documents.Core.Data;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Elements;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Parts;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.FormatProviders.OpenXml.Model
{
	abstract class OpenXmlPartsManager
	{
		public OpenXmlPartsManager()
		{
			this.parts = new StackCollection<OpenXmlPartBase>();
			this.usedParts = new HashSet<string>();
		}

		public IEnumerable<OpenXmlPartBase> Parts
		{
			get
			{
				return this.parts;
			}
		}

		public bool ContainsPart(string partName)
		{
			return this.usedParts.Contains(partName);
		}

		public void AddPart(OpenXmlPartBase part)
		{
			Guard.ThrowExceptionIfNull<OpenXmlPartBase>(part, "part");
			this.parts.AddLast(part);
			this.MarkPartNameAsUsed(part.Name);
		}

		public void RegisterPart(OpenXmlPartBase part)
		{
			Guard.ThrowExceptionIfNull<OpenXmlPartBase>(part, "part");
			if (this.contentTypesPart == null)
			{
				this.contentTypesPart = this.GetRequiredElementByName<ContentTypesPart>("/[Content_Types].xml");
			}
			this.parts.AddLast(part);
			this.MarkPartNameAsUsed(part.Name);
			this.contentTypesPart.RegisterPart(part);
		}

		public void RegisterResource(string extension)
		{
			if (this.contentTypesPart == null)
			{
				this.contentTypesPart = this.GetRequiredElementByName<ContentTypesPart>("/[Content_Types].xml");
			}
			this.contentTypesPart.RegisterResource(extension);
		}

		public string CreateApplicationRelationship(string target, string type, string targetMode = null)
		{
			Guard.ThrowExceptionIfNullOrEmpty(target, "target");
			Guard.ThrowExceptionIfNullOrEmpty(type, "type");
			RelationshipsPart relationshipsPart = this.GetRelationshipsPart("/_rels/.rels");
			return relationshipsPart.CreateRelationship(target, type, targetMode);
		}

		public string CreateRelationship(string targetPartName, string target, string type, string targetMode = null)
		{
			Guard.ThrowExceptionIfNullOrEmpty(targetPartName, "targetPartName");
			Guard.ThrowExceptionIfNullOrEmpty(target, "target");
			Guard.ThrowExceptionIfNullOrEmpty(type, "type");
			RelationshipsPart relationshipsPartForPart = this.GetRelationshipsPartForPart(targetPartName);
			return relationshipsPartForPart.CreateRelationship(target, type, targetMode);
		}

		public static string GetRelationshipRelativeTarget(string relationshipPartName, string target)
		{
			Guard.ThrowExceptionIfNull<string>(relationshipPartName, "relationshipPartName");
			Guard.ThrowExceptionIfNullOrEmpty(target, "target");
			Guard.ThrowExceptionIfNullOrEmpty(target, "target");
			string directoryName = Path.GetDirectoryName(relationshipPartName);
			string text = directoryName.Substring(0, directoryName.Length - "_rels".Length);
			text = text.Replace('\\', '/');
			string text2 = target.Replace('\\', '/');
			string a = string.Empty;
			string b = string.Empty;
			while (a == b && !string.IsNullOrEmpty(text))
			{
				int directorySeparatorIndex = OpenXmlPartsManager.GetDirectorySeparatorIndex(text, 0);
				if (directorySeparatorIndex == -1 && text2.Length < directorySeparatorIndex + 1)
				{
					break;
				}
				a = text.Substring(0, directorySeparatorIndex + 1);
				b = text2.Substring(0, directorySeparatorIndex + 1);
				if (!(a == b))
				{
					break;
				}
				text2 = text2.Substring(directorySeparatorIndex + 1);
				text = text.Substring(directorySeparatorIndex + 1);
			}
			int num = (from ch in text
				where ch == '/'
				select ch).Count<char>();
			if (num > 0)
			{
				for (int i = 0; i < num; i++)
				{
					text2 = "../" + text2;
				}
			}
			return text2;
		}

		public string GetRelationshipTarget(string targetPartName, string relationshipId)
		{
			Guard.ThrowExceptionIfNullOrEmpty(targetPartName, "targetPartName");
			Guard.ThrowExceptionIfNullOrEmpty(relationshipId, "relationshipId");
			RelationshipsPart relationshipsPartForPart = this.GetRelationshipsPartForPart(targetPartName);
			return relationshipsPartForPart.GetRelationshipTarget(relationshipId);
		}

		public string GetRelationshipId(string targetPartName, string target)
		{
			Guard.ThrowExceptionIfNullOrEmpty(targetPartName, "targetPartName");
			Guard.ThrowExceptionIfNullOrEmpty(target, "target");
			RelationshipsPart relationshipsPartForPart = this.GetRelationshipsPartForPart(targetPartName);
			return relationshipsPartForPart.GetRelationshipId(target);
		}

		public abstract OpenXmlElementBase CreateElement(string elementName, OpenXmlPartBase part);

		public abstract void ReleaseElement(OpenXmlElementBase element);

		public virtual OpenXmlPartBase CreatePart(string partType, string partName)
		{
			Guard.ThrowExceptionIfNullOrEmpty(partType, "partType");
			Guard.ThrowExceptionIfNullOrEmpty(partName, "partName");
			if (OpenXmlPartsFactory.CanCreateInstance(partType))
			{
				return OpenXmlPartsFactory.CreateInstance(partType, this, partName);
			}
			return null;
		}

		public T GetPart<T>(string partName) where T : OpenXmlPartBase
		{
			return this.GetElementByName<T>(partName);
		}

		protected T GetElementByName<T>(string partName) where T : OpenXmlPartBase
		{
			return (T)((object)this.parts.GetElementByName(partName));
		}

		protected T GetRequiredElementByName<T>(string partName) where T : OpenXmlPartBase
		{
			T elementByName = this.GetElementByName<T>(partName);
			if (elementByName == null)
			{
				throw new InvalidOperationException();
			}
			return elementByName;
		}

		RelationshipsPart GetRelationshipsPartForPart(string partName)
		{
			Guard.ThrowExceptionIfNullOrEmpty(partName, "partName");
			string fileName = Path.GetFileName(partName);
			string directoryName = Path.GetDirectoryName(partName);
			string name = PathHelper.NormalizePath(Path.Combine(directoryName, "_rels", fileName + ".rels"));
			return this.GetRelationshipsPart(name);
		}

		RelationshipsPart GetRelationshipsPart(string name)
		{
			Guard.ThrowExceptionIfNullOrEmpty(name, "name");
			if (!this.ContainsPart(name))
			{
				this.RegisterPart(new RelationshipsPart(this, name));
			}
			return this.GetElementByName<RelationshipsPart>(name);
		}

		void MarkPartNameAsUsed(string partName)
		{
			this.usedParts.Add(partName);
		}

		static int GetDirectorySeparatorIndex(string pathToSubstract, int startIndex)
		{
			int num = pathToSubstract.IndexOf(Path.DirectorySeparatorChar, startIndex);
			if (num == -1)
			{
				num = pathToSubstract.IndexOf(Path.AltDirectorySeparatorChar, startIndex);
			}
			return num;
		}

		readonly HashSet<string> usedParts;

		readonly StackCollection<OpenXmlPartBase> parts;

		ContentTypesPart contentTypesPart;
	}
}
