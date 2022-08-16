using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text;
using CsQuery.ExtensionMethods.Internal;
using CsQuery.StringScanner;

namespace CsQuery.Utility
{
	static class Support
	{
		public static string GetFile(string fileName)
		{
			string filePath = Support.GetFilePath(fileName);
			return File.ReadAllText(filePath);
		}

		public static FileStream GetFileStream(string fileName)
		{
			return new FileStream(fileName, FileMode.Open, FileAccess.Read);
		}

		public static bool TryGetFilePath(string partialPath, out string filePath)
		{
			if (Path.IsPathRooted(partialPath))
			{
				filePath = partialPath;
				return true;
			}
			string text = partialPath.Replace("/", "\\");
			if (text.StartsWith(".\\"))
			{
				text = text.Substring(1);
			}
			string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
			return Support.TryGetFilePath(text, baseDirectory, out filePath);
		}

		public static string GetFilePath(string partialPath)
		{
			string result;
			if (Support.TryGetFilePath(partialPath, out result))
			{
				return result;
			}
			return "";
		}

		public static string GetFilePath(string partialPath, string basePath)
		{
			string result;
			if (Support.TryGetFilePath(partialPath, basePath, out result))
			{
				return result;
			}
			throw new ArgumentException(string.Format("Unable to find path to \"{0}\" in base path \"{1}\" no matching parts.", partialPath, basePath));
		}

		public static bool TryGetFilePath(string partialPath, string basePath, out string outputPath)
		{
			List<string> list = new List<string>(basePath.ToLower().Split(new char[] { '\\' }, StringSplitOptions.RemoveEmptyEntries));
			List<string> list2 = new List<string>(partialPath.ToLower().Split(new char[] { '\\' }, StringSplitOptions.RemoveEmptyEntries));
			int num = list.IndexOf(list2[0]);
			if (num < 0)
			{
				outputPath = "";
				return false;
			}
			int num2 = 0;
			while (list[num++] == list2[num2++] && num2 < list2.Count && num < list.Count)
			{
			}
			string path = string.Join("\\", list.GetRange(0, num - 1)) + "\\" + string.Join("\\", list2.GetRange(num2 - 1, list2.Count - num2 + 1));
			outputPath = Support.CleanFilePath(path);
			return true;
		}

		public static Assembly GetFirstExternalAssembly()
		{
			Assembly executingAssembly = Assembly.GetExecutingAssembly();
			StackTrace stackTrace = new StackTrace(false);
			foreach (StackFrame stackFrame in stackTrace.GetFrames())
			{
				MethodBase method = stackFrame.GetMethod();
				if (method != null && method.DeclaringType != null && method.DeclaringType.Assembly != executingAssembly)
				{
					return method.DeclaringType.Assembly;
				}
			}
			throw new InvalidOperationException("Never found an external assembly.");
		}

		public static Stream GetResourceStream(string resourceName)
		{
			return Support.GetResourceStream(resourceName, Assembly.GetCallingAssembly());
		}

		public static Stream GetResourceStream(string resourceName, Assembly assembly)
		{
			return assembly.GetManifestResourceStream(resourceName);
		}

		public static Stream GetResourceStream(string resourceName, string assembly)
		{
			Assembly assembly2 = Assembly.Load(assembly);
			return Support.GetResourceStream(resourceName, assembly2);
		}

		public static string StreamToString(Stream stream)
		{
			byte[] array = new byte[stream.Length];
			stream.Position = 0L;
			stream.Read(array, 0, (int)stream.Length);
			return Encoding.UTF8.GetString(array);
		}

		public static string CleanFilePath(string path)
		{
			if (string.IsNullOrEmpty(path))
			{
				return "";
			}
			string text = path.Replace("/", "\\");
			while (text.IndexOf("\\\\") > 0)
			{
				text = text.Replace("\\\\", "\\");
			}
			int i = text.IndexOf("\\..\\");
			while (i > 0)
			{
				int num = text.Substring(0, i).LastIndexOf("\\");
				if (num > 0)
				{
					text = text.Substring(0, num) + text.Substring(i + 3);
					i = text.IndexOf("\\..\\");
				}
				else
				{
					i = -1;
				}
			}
			while (text.LastIndexOf("\\") == text.Length - 1)
			{
				text = text.Substring(0, text.Length - 1);
			}
			if (text.LastIndexOf(".") < text.LastIndexOf("\\"))
			{
				return text + "\\";
			}
			return text;
		}

		public static string CombinePaths(string path1, string path2)
		{
			return Support.RemoveRelativePath(path1) + Support.RemoveRelativePath(path2);
		}

		static string RemoveRelativePath(string path)
		{
			string text = path ?? "";
			if (text.StartsWith("~/"))
			{
				if (text.Length > 0)
				{
					text = text.Substring(2);
				}
				else
				{
					text = "";
				}
			}
			return text;
		}

		public static string MethodPath(MemberInfo mi)
		{
			return Support.TypePath(mi.ReflectedType) + "." + mi.Name;
		}

		public static string MethodPath(Type type, string memberName)
		{
			return Support.TypePath(type) + "." + memberName;
		}

		public static string TypePath(Type type)
		{
			return type.Namespace + "." + type.Name;
		}

		public static char[] StreamToCharArray(Stream stream)
		{
			StreamReader streamReader = new StreamReader(stream);
			long length = stream.Length;
			if (length > 0L && length < 2147483647L)
			{
				char[] array = new char[stream.Length];
				streamReader.Read(array, 0, Convert.ToInt32(length));
				return array;
			}
			return streamReader.ReadToEnd().ToCharArray();
		}

		public static void CopyFiles(DirectoryInfo source, DirectoryInfo destination, bool overwrite, params string[] patterns)
		{
			if (source == null)
			{
				throw new ArgumentException("No source directory specified.");
			}
			if (destination == null)
			{
				throw new ArgumentException("No destination directory specified.");
			}
			foreach (string searchPattern in patterns)
			{
				FileInfo[] files = source.GetFiles(searchPattern);
				foreach (FileInfo fileInfo in files)
				{
					fileInfo.CopyTo(destination.FullName + "\\" + fileInfo.Name, overwrite);
				}
			}
		}

		public static void CopyFiles(DirectoryInfo source, DirectoryInfo destination, params string[] patterns)
		{
			Support.CopyFiles(source, destination, true, patterns);
		}

		public static void DeleteFiles(DirectoryInfo directory, params string[] patterns)
		{
			if (directory == null)
			{
				throw new ArgumentException("No directory specified.");
			}
			foreach (string searchPattern in patterns)
			{
				FileInfo[] files = directory.GetFiles(searchPattern);
				foreach (FileInfo fileInfo in files)
				{
					fileInfo.Delete();
				}
			}
		}

		public static double DoubleOrZero(string value)
		{
			double result;
			if (double.TryParse(value, out result))
			{
				return result;
			}
			return 0.0;
		}

		public static int IntOrZero(string value)
		{
			int result;
			if (int.TryParse(value, out result))
			{
				return result;
			}
			return 0;
		}

		public static IConvertible NumberToDoubleOrInt(IConvertible value)
		{
			double num = (double)Convert.ChangeType(value, typeof(double));
			if (num == Math.Floor(num))
			{
				return (int)num;
			}
			return num;
		}

		public static string FromCamelCase(string name)
		{
			if (string.IsNullOrEmpty(name))
			{
				return "";
			}
			int i = 0;
			StringBuilder stringBuilder = new StringBuilder();
			while (i < name.Length)
			{
				char c = name[i];
				if (c >= 'A' && c <= 'Z')
				{
					if (i > 0 && name[i - 1] != '-')
					{
						stringBuilder.Append("-");
					}
					stringBuilder.Append(c.ToLower());
				}
				else
				{
					stringBuilder.Append(c);
				}
				i++;
			}
			return stringBuilder.ToString();
		}

		public static string ToCamelCase(string name, bool capFirst = false)
		{
			if (string.IsNullOrEmpty(name))
			{
				return "";
			}
			StringBuilder stringBuilder = new StringBuilder();
			int i = 0;
			bool flag = capFirst;
			while (i < name.Length)
			{
				char c = name[i];
				if (c == '-' && i > 0 && i < name.Length - 1 && CharacterData.IsType(name[i - 1], CharacterType.Alpha) && CharacterData.IsType(name[i + 1], CharacterType.Alpha))
				{
					c = name[++i];
					stringBuilder.Append(flag ? c : char.ToUpper(c));
					flag = false;
				}
				else
				{
					stringBuilder.Append(flag ? char.ToUpper(c) : c);
					if (CharacterData.IsType(c, CharacterType.Alpha))
					{
						flag = false;
					}
				}
				i++;
			}
			return stringBuilder.ToString();
		}

		public static TEnum AttributeToEnum<TEnum>(string value) where TEnum : struct
		{
			TEnum result;
			if (Enum.TryParse<TEnum>(Support.ToCamelCase(value, true), out result))
			{
				return result;
			}
			return (TEnum)((object)0);
		}

		public static string EnumToAttribute(Enum value)
		{
			return value.ToString().ToLower();
		}

		public static Stream GetEncodedStream(string html, Encoding encoding)
		{
			return new MemoryStream(encoding.GetBytes(html));
		}
	}
}
