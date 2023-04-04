using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MapleScreenScraper
{
    internal static class RegionSelection
    {
        public static Rectangle GetSelection()
        {
            using (var form = new Form())
            {
                form.WindowState = FormWindowState.Maximized;
                form.FormBorderStyle = FormBorderStyle.None;
                form.Opacity = 0.5;
                form.BackColor = Color.Black;
                form.ShowInTaskbar = false;
                form.TopMost = true;

                Rectangle selection = Rectangle.Empty;
                bool dragging = false;
                form.MouseDown += (s, e) =>
                        {
                            selection.Location = e.Location;
                            dragging = true;
                        };
                form.MouseMove += (s, e) =>
                {
                    if (dragging)
                    {
                        selection.Width = e.X - selection.X;
                        selection.Height = e.Y - selection.Y;
                        form.Invalidate();
                    }
                };
                form.Paint += (s, e) =>
                {
                    using (var pen = new Pen(Color.Red, 2))
                    {
                        e.Graphics.DrawRectangle(pen, selection);
                    }
                };
                form.KeyDown += (s, e) =>
                {
                    if (e.KeyCode == Keys.Enter)
                    {
                        form.DialogResult = DialogResult.OK;
                    }
                };

                form.ShowDialog();

                return selection;
            }
        }
    }
}
