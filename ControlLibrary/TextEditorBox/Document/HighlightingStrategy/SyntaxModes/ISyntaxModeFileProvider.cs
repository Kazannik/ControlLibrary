using System.Collections.Generic;
using System.Xml;

namespace Controls.Document
{
	public interface ISyntaxModeFileProvider
	{
		ICollection<SyntaxMode> SyntaxModes
		{
			get;
		}

		XmlTextReader GetSyntaxModeFile(SyntaxMode syntaxMode);
		void UpdateSyntaxModeList();
	}
}
