using System.Drawing;
using System.Text;

namespace ControlLibrary.Controls.TextControl.TextEditor.Document
{
	public enum BracketMatchingStyle
	{
		Before,
		After
	}

	public class DefaultTextEditorProperties : ITextEditorProperties
	{
		private int tabIndent = 4;
		private int indentationSize = 4;
		private IndentStyle indentStyle = IndentStyle.Smart;
		private DocumentSelectionMode documentSelectionMode = DocumentSelectionMode.Normal;
		private Encoding encoding = Encoding.UTF8;
		private BracketMatchingStyle bracketMatchingStyle = BracketMatchingStyle.After;
		private static Font DefaultFont;

		public DefaultTextEditorProperties()
		{
			if (DefaultFont == null)
			{
				DefaultFont = new Font("Courier New", 14);
			}
			FontContainer = new FontContainer(DefaultFont);
		}

		private bool allowCaretBeyondEOL = false;
		private bool caretLine = false;
		private bool showMatchingBracket = true;
		private bool showLineNumbers = true;
		private bool showSpaces = false;
		private bool showTabs = false;
		private bool showEOLMarker = false;
		private bool showInvalidLines = false;
		private bool isIconBarVisible = false;
		private bool enableFolding = true;
		private bool showHorizontalRuler = false;
		private bool showVerticalRuler = true;
		private bool convertTabsToSpaces = false;
		private System.Drawing.Text.TextRenderingHint textRenderingHint = System.Drawing.Text.TextRenderingHint.SystemDefault;
		private bool mouseWheelScrollDown = true;
		private bool mouseWheelTextZoom = true;
		private bool hideMouseCursor = false;
		private bool cutCopyWholeLine = true;
		private int verticalRulerRow = 80;
		private LineViewerStyle lineViewerStyle = LineViewerStyle.None;
		private string lineTerminator = "\r\n";
		private bool autoInsertCurlyBracket = true;
		private bool supportReadOnlySegments = false;

		public int TabIndent
		{
			get => tabIndent;
			set => tabIndent = value;
		}

		public int IndentationSize
		{
			get => indentationSize;
			set => indentationSize = value;
		}

		public IndentStyle IndentStyle
		{
			get => indentStyle;
			set => indentStyle = value;
		}

		public bool CaretLine
		{
			get => caretLine;
			set => caretLine = value;
		}

		public DocumentSelectionMode DocumentSelectionMode
		{
			get => documentSelectionMode;
			set => documentSelectionMode = value;
		}
		public bool AllowCaretBeyondEOL
		{
			get => allowCaretBeyondEOL;
			set => allowCaretBeyondEOL = value;
		}
		public bool ShowMatchingBracket
		{
			get => showMatchingBracket;
			set => showMatchingBracket = value;
		}
		public bool ShowLineNumbers
		{
			get => showLineNumbers;
			set => showLineNumbers = value;
		}
		public bool ShowSpaces
		{
			get => showSpaces;
			set => showSpaces = value;
		}
		public bool ShowTabs
		{
			get => showTabs;
			set => showTabs = value;
		}
		public bool ShowEOLMarker
		{
			get => showEOLMarker;
			set => showEOLMarker = value;
		}
		public bool ShowInvalidLines
		{
			get => showInvalidLines;
			set => showInvalidLines = value;
		}
		public bool IsIconBarVisible
		{
			get => isIconBarVisible;
			set => isIconBarVisible = value;
		}
		public bool EnableFolding
		{
			get => enableFolding;
			set => enableFolding = value;
		}
		public bool ShowHorizontalRuler
		{
			get => showHorizontalRuler;
			set => showHorizontalRuler = value;
		}
		public bool ShowVerticalRuler
		{
			get => showVerticalRuler;
			set => showVerticalRuler = value;
		}
		public bool ConvertTabsToSpaces
		{
			get => convertTabsToSpaces;
			set => convertTabsToSpaces = value;
		}
		public System.Drawing.Text.TextRenderingHint TextRenderingHint
		{
			get => textRenderingHint;
			set => textRenderingHint = value;
		}

		public bool MouseWheelScrollDown
		{
			get => mouseWheelScrollDown;
			set => mouseWheelScrollDown = value;
		}
		public bool MouseWheelTextZoom
		{
			get => mouseWheelTextZoom;
			set => mouseWheelTextZoom = value;
		}

		public bool HideMouseCursor
		{
			get => hideMouseCursor;
			set => hideMouseCursor = value;
		}

		public bool CutCopyWholeLine
		{
			get => cutCopyWholeLine;
			set => cutCopyWholeLine = value;
		}

		public Encoding Encoding
		{
			get => encoding;
			set => encoding = value;
		}
		public int VerticalRulerRow
		{
			get => verticalRulerRow;
			set => verticalRulerRow = value;
		}
		public LineViewerStyle LineViewerStyle
		{
			get => lineViewerStyle;
			set => lineViewerStyle = value;
		}
		public string LineTerminator
		{
			get => lineTerminator;
			set => lineTerminator = value;
		}
		public bool AutoInsertCurlyBracket
		{
			get => autoInsertCurlyBracket;
			set => autoInsertCurlyBracket = value;
		}

		public Font Font
		{
			get => FontContainer.DefaultFont;
			set => FontContainer.DefaultFont = value;
		}

		public FontContainer FontContainer { get; }

		public BracketMatchingStyle BracketMatchingStyle
		{
			get => bracketMatchingStyle;
			set => bracketMatchingStyle = value;
		}

		public bool SupportReadOnlySegments
		{
			get => supportReadOnlySegments;
			set => supportReadOnlySegments = value;
		}
	}
}
