using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Text.RegularExpressions;

namespace Fonlow.CommandLineGui.Gui
{

    internal class OutputWindow : IDisposable
    {
        public OutputWindow()
        {
            Window = new Form();
            textBox = new TextBox();
            textBox.Dock = DockStyle.Fill;
            textBox.Multiline = true;
            textBox.WordWrap = true;
            textBox.ScrollBars = ScrollBars.Vertical;
            textBox.Font = new Font("Courier New", 9, FontStyle.Regular);

            Window.Text = "Output";
            Window.ShowInTaskbar = false;
            Window.Controls.Add(textBox);
        }

        public Form Window { get; private set; }
        public TextBox TextBox { get { return textBox; } }

        TextBox textBox;

        #region IDisposable Members
        bool disposed;

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2213:DisposableFieldsShouldBeDisposed", MessageId = "textBox")]
        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    Window.Dispose();
                }
                disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}
