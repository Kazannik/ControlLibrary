using System;
using System.Runtime.InteropServices;

namespace ControlLibrary.Utils
{
	public static class Shortcut
	{
		private static readonly Type m_type = Type.GetTypeFromProgID("WScript.Shell");
		private static readonly object m_shell = Activator.CreateInstance(m_type);

		[ComImport, TypeLibType(0x1040), Guid("F935DC23-1CF0-11D0-ADB9-00C04FD58A0B")]
		private interface IWshShortcut
		{
			[DispId(0)]
			string FullName { [return: MarshalAs(UnmanagedType.BStr)][DispId(0)] get; }
			
			[DispId(0x3e8)]
			string Arguments { [return: MarshalAs(UnmanagedType.BStr)][DispId(0x3e8)] get; [param: In, MarshalAs(UnmanagedType.BStr)][DispId(0x3e8)] set; }
			
			[DispId(0x3e9)]
			string Description { [return: MarshalAs(UnmanagedType.BStr)][DispId(0x3e9)] get; [param: In, MarshalAs(UnmanagedType.BStr)][DispId(0x3e9)] set; }
			
			[DispId(0x3ea)]
			string Hotkey { [return: MarshalAs(UnmanagedType.BStr)][DispId(0x3ea)] get; [param: In, MarshalAs(UnmanagedType.BStr)][DispId(0x3ea)] set; }
			
			[DispId(0x3eb)]
			string IconLocation { [return: MarshalAs(UnmanagedType.BStr)][DispId(0x3eb)] get; [param: In, MarshalAs(UnmanagedType.BStr)][DispId(0x3eb)] set; }
			
			[DispId(0x3ec)]
			string RelativePath { [param: In, MarshalAs(UnmanagedType.BStr)][DispId(0x3ec)] set; }
			
			[DispId(0x3ed)]
			string TargetPath { [return: MarshalAs(UnmanagedType.BStr)][DispId(0x3ed)] get; [param: In, MarshalAs(UnmanagedType.BStr)][DispId(0x3ed)] set; }
			
			[DispId(0x3ee)]
			int WindowStyle { [DispId(0x3ee)] get; [param: In][DispId(0x3ee)] set; }
			
			[DispId(0x3ef)]
			string WorkingDirectory { [return: MarshalAs(UnmanagedType.BStr)][DispId(0x3ef)] get; [param: In, MarshalAs(UnmanagedType.BStr)][DispId(0x3ef)] set; }
			
			[TypeLibFunc(0x40), DispId(0x7d0)]
			void Load([In, MarshalAs(UnmanagedType.BStr)] string PathLink);
			
			[DispId(0x7d1)]
			void Save();
		}

		/// <summary>
		/// Создать ярлык.
		/// </summary>
		/// <param name="name">Имя ярлыка.</param>
		/// <param name="path">Папка размещения ярлыка.</param>
		/// <param name="targetPath">Объект (путь к папке или запускаемому файлу).</param>
		/// <param name="windowsSyle">Стиль окна.</param>
		/// <param name="hotkey">Быстрый вызов. Например: "Ctrl+Alt+f</param>
		/// <param name="iconLocation">Значок. Например: "notepad.exe, 0"</param>
		/// <param name="description">Комментарий.</param>
		/// <param name="workingDirectory">Рабочая папка.</param>
		/// <param name="arguments">Аргументы коммандной строки.</param>
		public static void Create(string name, string path, string targetPath, WindowStyle windowsSyle, string hotkey, string iconLocation, string description, string workingDirectory, string arguments)
		{
			IWshShortcut shortcut = (IWshShortcut)m_type.InvokeMember("CreateShortcut", System.Reflection.BindingFlags.InvokeMethod, null, m_shell, new object[] { System.IO.Path.Combine(path, name + ".lnk") });
			shortcut.Description = description;
			shortcut.Hotkey = hotkey;
			shortcut.TargetPath = targetPath;
			shortcut.WindowStyle = (int)windowsSyle;
			shortcut.Hotkey = hotkey;
			shortcut.WorkingDirectory = workingDirectory;
			shortcut.Arguments = arguments;
			if (!string.IsNullOrEmpty(iconLocation))
			{
				shortcut.IconLocation = iconLocation;
			}
			shortcut.Save();
		}

		/// <summary>
		/// Создать ярлык.
		/// </summary>
		/// <param name="name">Имя ярлыка.</param>
		/// <param name="folder">Системная папка.</param>
		/// <param name="targetPath">Объект (путь к папке или запускаемому файлу).</param>
		/// <param name="windowsSyle">Стиль окна.</param>
		/// <param name="hotkey">Быстрый вызов. Например: "Ctrl+Alt+f</param>
		/// <param name="iconLocation">Значок. Например: "notepad.exe, 0"</param>
		/// <param name="description">Комментарий.</param>
		/// <param name="workingDirectory">Рабочая папка.</param>
		/// <param name="arguments">Аргументы коммандной строки.</param>
		public static void Create(string name, Environment.SpecialFolder folder, string targetPath, WindowStyle windowsSyle, string hotkey, string iconLocation, string description, string workingDirectory, string arguments)
		{
			Create(name: name, path: Environment.GetFolderPath(folder), targetPath: targetPath, windowsSyle: windowsSyle, hotkey: hotkey, iconLocation: iconLocation, description: description, workingDirectory: workingDirectory, arguments: arguments);
		}

		/// <summary>
		/// Определяет, существует ли заданный ярлык.
		/// </summary>
		/// <param name="name">Имя ярлыка.</param>
		/// <param name="path">Папка размещения ярлыка.</param>
		/// <param name="targetPath">Объект (путь к папке или запускаемому файлу).</param>
		public static bool Exists(string name, string path, string targetPath)
		{
			string fileName = System.IO.Path.Combine(path, name + ".lnk");
			if (!System.IO.File.Exists(fileName))
			{
				return false;
			}
			else
			{
				IWshShortcut shortcut = (IWshShortcut)m_type.InvokeMember("CreateShortcut", System.Reflection.BindingFlags.InvokeMethod, null, m_shell, new object[] { fileName });
				return targetPath.Equals(shortcut.TargetPath, StringComparison.CurrentCultureIgnoreCase);
			}
		}


		/// <summary>
		/// Определяет, существует ли заданный ярлык.
		/// </summary>
		/// <param name="name">Имя ярлыка.</param>
		/// <param name="folder">Системная папка.</param>
		/// <param name="targetPath">Объект (путь к папке или запускаемому файлу).</param>
		public static bool Exists(string name, Environment.SpecialFolder folder, string targetPath)
		{
			return Exists(name: name, path: Environment.GetFolderPath(folder), targetPath: targetPath);
		}

		/// <summary>
		/// Стиль окна.
		/// </summary>
		public enum WindowStyle : int
		{
			/// <summary>
			/// Обычный размерн окна.
			/// </summary>
			Original = 1,
			/// <summary>
			/// Окно, свернутое в значок.
			/// </summary>
			Maximized = 3,
			/// <summary>
			/// Окно, развернутое во весь экран.
			/// </summary>
			Minimizes = 7
		}
	}
}
