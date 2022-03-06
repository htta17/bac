using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Midas.Utils
{
    public static class PhotoService
    {
        const string IMAGE_FORMAT = FOLDER_FORMAT + "\\Image_{0:HHmmss}.png";
        const string FOLDER_FORMAT = "Logs\\{0:yyyy-MM-dd}";

        public static void TakeScreenshot(bool showMessage, int width = 1920, int height = 1080)
        {
            var dateTimeNow = DateTime.Now;
            if (!Directory.Exists(string.Format(FOLDER_FORMAT, dateTimeNow)))
            {
                Directory.CreateDirectory(string.Format(FOLDER_FORMAT, dateTimeNow));
            }
            try
            {
                Rectangle bounds = Screen.PrimaryScreen.Bounds;                
                using (Bitmap bitmap = new Bitmap(width, height))
                {
                    using (Graphics g = Graphics.FromImage(bitmap))
                    {
                        g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

                        g.CopyFromScreen(new Point(0, 0), Point.Empty, new Size(width, height));
                    }
                    bitmap.Save(string.Format(IMAGE_FORMAT, dateTimeNow), ImageFormat.Jpeg);
                }
            }
            catch (Exception e)
            {
                if (showMessage)
                {
                    MessageBox.Show("Error" + e.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}
