using System;

namespace ControlLibrary.Controls.TextControl.TextEditor.Document
{
	/// <summary>
	/// This interface is used to describe a span inside a text sequence
	/// </summary>
	public class AbstractSegment : ISegment
	{
		//[CLSCompliant(false)]
		protected int offset = -1;
		//[CLSCompliant(false)]
		protected int length = -1;

		#region TextEditor.Document.ISegment interface implementation
		public virtual int Offset
		{
			get => offset;
			set => offset = value;
		}

		public virtual int Length
		{
			get => length;
			set => length = value;
		}

		#endregion

		public override string ToString()
		{
			return String.Format("[AbstractSegment: Offset = {0}, Length = {1}]",
								 Offset,
								 Length);
		}


	}
}
