using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Telerik.Windows.Documents.Common.FormatProviders.OpenXml.Utilities
{
	static class PathHelper
	{
		public static string NormalizePath(string path)
		{
			path = path.Replace("\\", "/");
			path = path.Replace("//", "/");
			return path;
		}

		public static string NormalizePathForExport(string path)
		{
			path = path.Replace("\\", "/");
			path = path.Replace("//", "/");
			if (path.StartsWith("/"))
			{
				path = path.Substring(1);
			}
			return path;
		}

		public static string NormalizePathFromImport(string path)
		{
			path = path.Replace("\\", "/");
			path = path.Replace("//", "/");
			if (!path.StartsWith("/"))
			{
				path = "/" + path;
			}
			return path;
		}

		public static string Combine(string root, string path)
		{
			path = PathHelper.NormalizePath(path);
			if (path.StartsWith("/"))
			{
				return path;
			}
			root = PathHelper.NormalizePath(Path.GetDirectoryName(root));
			Stack<string> stack = new Stack<string>(root.Split(new string[] { "/" }, StringSplitOptions.RemoveEmptyEntries));
			string[] array = path.Split(new string[] { "/" }, StringSplitOptions.RemoveEmptyEntries);
			foreach (string text in array)
			{
				if (text == "..")
				{
					stack.Pop();
				}
				else
				{
					stack.Push(text);
				}
			}
			StringBuilder stringBuilder = new StringBuilder();
			while (stack.Count > 0)
			{
				stringBuilder.Insert(0, "/" + stack.Pop());
			}
			return stringBuilder.ToString();
		}

		public const string RelationDirectory = "_rels";

		const string DirectoryMarker = "/";

		const string ParentDirectoryMarker = "..";
	}
}
