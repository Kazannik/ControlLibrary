using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Xml;

namespace ControlLibrary.Controls.TextControl.TextEditor.Document
{
	public class ResourceSyntaxModeProvider : ISyntaxModeFileProvider
	{
		private List<SyntaxMode> syntaxModes = null;

		public ICollection<SyntaxMode> SyntaxModes => syntaxModes;

		public ResourceSyntaxModeProvider()
		{
			Assembly assembly = typeof(SyntaxMode).Assembly;
			Stream syntaxModeStream = assembly.GetManifestResourceStream("TextEditor.Resources.SyntaxModes.xml");
			syntaxModes = syntaxModeStream != null ? SyntaxMode.GetSyntaxModes(syntaxModeStream) : new List<SyntaxMode>();
		}

		public XmlTextReader GetSyntaxModeFile(SyntaxMode syntaxMode)
		{
			Assembly assembly = typeof(SyntaxMode).Assembly;
			return new XmlTextReader(assembly.GetManifestResourceStream("TextEditor.Resources." + syntaxMode.FileName));
		}

		public void UpdateSyntaxModeList()
		{
			// resources don't change during runtime
		}
	}
}
