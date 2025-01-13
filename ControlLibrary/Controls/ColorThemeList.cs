using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;

namespace ControlLibrary.Controls
{
	public partial class ColorThemeList : Component
	{
		private readonly List<ColorTheme> themes;

		public ColorThemeList()
		{
			themes = new List<ColorTheme>();
			Default = new ColorThemeOffice2003(this);
			themes.Add(Default);
			InitializeComponent();
		}

		public ColorTheme this[int index] => themes[index];

		public ColorTheme Default { get; }

		#region InitializeControl

		/// <summary>
		/// Обязательная переменная конструктора.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		public ColorThemeList(IContainer container)
		{
			container.Add(this);
			themes = new List<ColorTheme>();
			InitializeComponent();
		}

		/// <summary>
		/// Освободить все используемые ресурсы.
		/// </summary>
		/// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Код, автоматически созданный конструктором компонентов

		/// <summary>
		/// Требуемый метод для поддержки конструктора — не изменяйте 
		/// содержимое этого метода с помощью редактора кода.
		/// </summary>
		private void InitializeComponent()
		{
			components = new System.ComponentModel.Container();
		}

		#endregion

		#endregion

	}

	public enum ThemeType
	{
		Office2003,
		Office2007
	}

	public class ColorTheme
	{
		private readonly ColorThemeList control;
		public ColorTheme(ColorThemeList control)
		{
			this.control = control;
		}

		public Color ForeColor { get; set; }
		public Color BackColor { get; set; }
		public Color ForeColorSelected { get; set; }
		public Color BackColorSelected { get; set; }
		public Color ColorHoveringTop { get; set; }
		public Color ColorHoveringBottom { get; set; }
		public Color ColorSelectedTop { get; set; }
		public Color ColorSelectedBottom { get; set; }
		public Color ColorSelectedAndHoveringTop { get; set; }
		public Color ColorSelectedAndHoveringBottom { get; set; }
		public Color ColorPassiveTop { get; set; }
		public Color ColorPassiveBottom { get; set; }
	}

	public class ColorThemeOffice2003 : ColorTheme
	{
		public ColorThemeOffice2003(ColorThemeList control) : base(control)
		{
			ColorSelectedAndHoveringTop = Color.FromArgb(232, 127, 8);
			ColorSelectedAndHoveringBottom = Color.FromArgb(247, 218, 124);
			ColorHoveringTop = Color.FromArgb(255, 255, 220);
			ColorHoveringBottom = Color.FromArgb(247, 192, 91);
			ColorSelectedTop = Color.FromArgb(247, 218, 124);
			ColorSelectedBottom = Color.FromArgb(232, 127, 8);
			ColorPassiveTop = Color.FromArgb(203, 225, 252);
			ColorPassiveBottom = Color.FromArgb(125, 166, 223);
		}

		public new Color ForeColor { get; }
		public new Color BackColor { get; }
		public new Color ForeColorSelected { get; }
		public new Color BackColorSelected { get; }
		public new Color ColorHoveringTop { get; }
		public new Color ColorHoveringBottom { get; }
		public new Color ColorSelectedTop { get; }
		public new Color ColorSelectedBottom { get; }
		public new Color ColorSelectedAndHoveringTop { get; }
		public new Color ColorSelectedAndHoveringBottom { get; }
		public new Color ColorPassiveTop { get; }
		public new Color ColorPassiveBottom { get; }
	}
}
