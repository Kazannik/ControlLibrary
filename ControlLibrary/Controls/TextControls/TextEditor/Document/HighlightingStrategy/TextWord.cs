using System;
using System.Diagnostics;
using System.Drawing;

namespace ControlLibrary.Controls.TextControl.TextEditor.Document
{
	public enum TextWordType
	{
		Word,
		Space,
		Tab
	}

	/// <summary>
	/// This class represents single words with color information, two special versions of a word are
	/// spaces and tabs.
	/// </summary>
	public class TextWord
	{
		private HighlightColor color;
		private LineSegment line;
		private IDocument document;
		private int offset;
		private int length;

		public sealed class SpaceTextWord : TextWord
		{
			public SpaceTextWord()
			{
				length = 1;
			}

			public SpaceTextWord(HighlightColor color)
			{
				length = 1;
				base.SyntaxColor = color;
			}

			public override Font GetFont(FontContainer fontContainer)
			{
				return null;
			}

			public override TextWordType Type => TextWordType.Space;
			public override bool IsWhiteSpace => true;
		}

		public sealed class TabTextWord : TextWord
		{
			public TabTextWord()
			{
				length = 1;
			}
			public TabTextWord(HighlightColor color)
			{
				length = 1;
				base.SyntaxColor = color;
			}

			public override Font GetFont(FontContainer fontContainer)
			{
				return null;
			}

			public override TextWordType Type => TextWordType.Tab;
			public override bool IsWhiteSpace => true;
		}

		private static TextWord spaceWord = new SpaceTextWord();
		private static TextWord tabWord = new TabTextWord();
		private bool hasDefaultColor;

		public static TextWord Space => spaceWord;

		public static TextWord Tab => tabWord;

		public int Offset => offset;

		public int Length => length;

		/// <summary>
		/// Splits the <paramref name="word"/> into two parts: the part before <paramref name="pos"/> is assigned to
		/// the reference parameter <paramref name="word"/>, the part after <paramref name="pos"/> is returned.
		/// </summary>
		public static TextWord Split(ref TextWord word, int pos)
		{
#if DEBUG
			if (word.Type != TextWordType.Word)
				throw new ArgumentException("word.Type must be Word");
			if (pos <= 0)
				throw new ArgumentOutOfRangeException("pos", pos, "pos must be > 0");
			if (pos >= word.Length)
				throw new ArgumentOutOfRangeException("pos", pos, "pos must be < word.Length");
#endif
			TextWord after = new TextWord(word.document, word.line, word.offset + pos, word.length - pos, word.color, word.hasDefaultColor);
			word = new TextWord(word.document, word.line, word.offset, pos, word.color, word.hasDefaultColor);
			return after;
		}

		public bool HasDefaultColor => hasDefaultColor;

		public virtual TextWordType Type => TextWordType.Word;

		public string Word => document == null ? string.Empty : document.GetText(line.Offset + offset, length);

		public virtual Font GetFont(FontContainer fontContainer)
		{
			return color.GetFont(fontContainer);
		}

		public Color Color => color == null ? Color.Black : color.Color;

		public bool Bold => color == null ? false : color.Bold;

		public bool Italic => color == null ? false : color.Italic;

		public HighlightColor SyntaxColor
		{
			get => color;
			set
			{
				Debug.Assert(value != null);
				color = value;
			}
		}

		public virtual bool IsWhiteSpace => false;

		protected TextWord()
		{
		}

		// TAB
		public TextWord(IDocument document, LineSegment line, int offset, int length, HighlightColor color, bool hasDefaultColor)
		{
			Debug.Assert(document != null);
			Debug.Assert(line != null);
			Debug.Assert(color != null);

			this.document = document;
			this.line = line;
			this.offset = offset;
			this.length = length;
			this.color = color;
			this.hasDefaultColor = hasDefaultColor;
		}

		/// <summary>
		/// Converts a <see cref="TextWord"/> instance to string (for debug purposes)
		/// </summary>
		public override string ToString()
		{
			return "[TextWord: Word = " + Word + ", Color = " + Color + "]";
		}
	}
}
