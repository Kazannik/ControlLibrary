using System;
using System.Xml;

namespace ControlLibrary.Controls.TextControl.TextEditor.Document
{
	public sealed class Span
	{
		private bool stopEOL;
		private HighlightColor color;
		private HighlightColor beginColor;
		private HighlightColor endColor;
		private char[] begin;
		private char[] end;
		private string name;
		private string rule;
		private HighlightRuleSet ruleSet;
		private char escapeCharacter;
		private bool ignoreCase;
		private bool isBeginSingleWord;
		private bool? isBeginStartOfLine;
		private bool isEndSingleWord;

		internal HighlightRuleSet RuleSet
		{
			get => ruleSet;
			set => ruleSet = value;
		}

		public bool IgnoreCase
		{
			get => ignoreCase;
			set => ignoreCase = value;
		}

		public bool StopEOL => stopEOL;

		public bool? IsBeginStartOfLine => isBeginStartOfLine;

		public bool IsBeginSingleWord => isBeginSingleWord;

		public bool IsEndSingleWord => isEndSingleWord;

		public HighlightColor Color => color;

		public HighlightColor BeginColor => beginColor ?? color;

		public HighlightColor EndColor => endColor ?? color;

		public char[] Begin => begin;

		public char[] End => end;

		public string Name => name;

		public string Rule => rule;

		/// <summary>
		/// Gets the escape character of the span. The escape character is a character that can be used in front
		/// of the span end to make it not end the span. The escape character followed by another escape character
		/// means the escape character was escaped like in @"a "" b" literals in C#.
		/// The default value '\0' means no escape character is allowed.
		/// </summary>
		public char EscapeCharacter => escapeCharacter;

		public Span(XmlElement span)
		{
			color = new HighlightColor(span);

			if (span.HasAttribute("rule"))
			{
				rule = span.GetAttribute("rule");
			}

			if (span.HasAttribute("escapecharacter"))
			{
				escapeCharacter = span.GetAttribute("escapecharacter")[0];
			}

			name = span.GetAttribute("name");
			if (span.HasAttribute("stopateol"))
			{
				stopEOL = Boolean.Parse(span.GetAttribute("stopateol"));
			}

			begin = span["Begin"].InnerText.ToCharArray();
			beginColor = new HighlightColor(span["Begin"], color);

			if (span["Begin"].HasAttribute("singleword"))
			{
				this.isBeginSingleWord = Boolean.Parse(span["Begin"].GetAttribute("singleword"));
			}
			if (span["Begin"].HasAttribute("startofline"))
			{
				this.isBeginStartOfLine = Boolean.Parse(span["Begin"].GetAttribute("startofline"));
			}

			if (span["End"] != null)
			{
				end = span["End"].InnerText.ToCharArray();
				endColor = new HighlightColor(span["End"], color);
				if (span["End"].HasAttribute("singleword"))
				{
					this.isEndSingleWord = Boolean.Parse(span["End"].GetAttribute("singleword"));
				}

			}
		}
	}
}
