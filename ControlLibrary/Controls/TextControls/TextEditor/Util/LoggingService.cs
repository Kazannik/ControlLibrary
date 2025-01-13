using System;

namespace ControlLibrary.Controls.TextControl.TextEditor.Util
{
	/// <summary>
	/// Central location for logging calls in the text editor.
	/// </summary>
	internal static class LoggingService
	{
		public static void Debug(string text)
		{
#if DEBUG
			Console.WriteLine(text);
#endif
		}
	}
}
