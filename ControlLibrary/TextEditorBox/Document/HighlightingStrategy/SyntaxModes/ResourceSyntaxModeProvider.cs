using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Xml;

namespace Controls.Document
{
	public class ResourceSyntaxModeProvider : ISyntaxModeFileProvider
	{
		List<SyntaxMode> syntaxModes = null;

		public ICollection<SyntaxMode> SyntaxModes
		{
			get
			{
				return syntaxModes;
			}
		}

		public ResourceSyntaxModeProvider()
		{
			Assembly assembly = typeof(SyntaxMode).Assembly;
			Stream syntaxModeStream = assembly.GetManifestResourceStream("TextEditor.Resources.SyntaxModes.xml");
			if (syntaxModeStream != null)
			{
				syntaxModes = SyntaxMode.GetSyntaxModes(syntaxModeStream);
			}
			else
			{
				syntaxModes = new List<SyntaxMode>();
			}
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
