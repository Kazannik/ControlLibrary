using System;
using System.Drawing;
using System.IO;

namespace ControlLibrary.Utils
{
	public static class Directories
	{
		private const string ICON_FILE_NAME = "FolderIcon.ico";
		private const string INI_FILE_NAME = "desktop.ini";

		/// <summary>
		/// Создание папки с определенным значком
		/// </summary>
		/// <param name="path">Путь папки</param>
		/// <param name="icon">Значек для папки</param>
		public static void CreateFolder(string path, Icon icon)
		{
			if (icon == null) throw new ArgumentNullException("icon", "Параметр не может быть null.");
			if (Directory.Exists(path)) 
			{
				Directory.CreateDirectory(path);
			}
			
			string iconFileName = Path.Combine(path, ICON_FILE_NAME);
			if (!File.Exists(iconFileName))
			{
				FileStream fileStream = new FileStream(iconFileName, FileMode.Create);
				icon.Save(fileStream);
				fileStream.Close();
				_ = new FileInfo(iconFileName)
				{
					Attributes = FileAttributes.Hidden
				};
			}

			string iniFileName = Path.Combine(path, INI_FILE_NAME);
			if (!File.Exists(iniFileName)) 
			{
				string iniContent =
					string.Format(
						"[.ShellClassInfo]{0}"
						+ "IconFile = {1}{0}"
						+ "IconIndex = 0{0}"
						+ "ConfirmFileOp = 0{0}"
						+ "OriginalIcon = %{0}{0}"
						+ "[.ShellClassInfo.A]{0}"
						+ "IconFile = {1}{0}"
						+ "[.ShellClassInfo.W]{0}"
						+ "IconFile = {1}{0}"
						+ "[.ShellClassInfo]{0}"
						+ "IconFile = {1}{0}"
						+ "IconIndex = 0", Environment.NewLine, ICON_FILE_NAME);
				File.WriteAllText(iniFileName, iniContent);
				_ = new FileInfo(iniFileName)
				{
					Attributes = FileAttributes.Hidden | FileAttributes.Archive | FileAttributes.System
				};
			}

			_ = new DirectoryInfo(path)
			{
				Attributes = FileAttributes.Directory | FileAttributes.ReadOnly
			};
		}
	}
}
