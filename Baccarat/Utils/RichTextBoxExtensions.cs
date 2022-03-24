using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Midas.Utils
{
    public static class RichTextBoxExtensions
    {
        public static void AppendText(this RichTextBox box, string text, Color color)
        {
            box.SelectionStart = box.TextLength;
            box.SelectionLength = 0;

            box.SelectionColor = color;
            box.AppendText(text);
            box.SelectionColor = box.ForeColor;
        }

        public static void InsertOnTop(this RichTextBox box, string text, Color color) 
        {
            box.Text = box.Text.Insert(0, text);
            box.SelectionStart = 0;
            box.SelectionLength = text.Length;
            box.SelectionColor = color;
        }       
    }

}
