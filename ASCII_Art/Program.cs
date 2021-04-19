using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ASCII_Art
{
    class Program
    {
        private const double WIDTH_OFFSET = 2.5;
        private const int maxWidth = 350;

        [STAThread]
        static void Main(string[] args)
        {
            Console.BackgroundColor = ConsoleColor.White;
            Console.ForegroundColor = ConsoleColor.Black;


            var openFileDialog = new OpenFileDialog
            {
                Filter = "Images | *.bmp; *.png; *.jpg; *.JPEG"
            };

            Console.WriteLine("Press enter to start...\n");

            while(true)
            {
                Console.ReadLine();

                if(openFileDialog.ShowDialog() != DialogResult.OK)
                {
                    continue;
                }

                Console.Clear();

                var bitmap = new Bitmap(openFileDialog.FileName);
                bitmap = ResizeBitmap(bitmap);
                bitmap.ToGrayscala();

                var converter = new BitmapToASCIIConverter(bitmap);
                var rows = converter.Convert();

                
                foreach(var row in rows)
                {
                    Console.WriteLine(row);
                }

                var rowNegative = converter.ConvertAsNegative();

                File.WriteAllLines("images.txt", rows.Select(r => new string(r)));

                Console.SetCursorPosition(0, 0);

            }

        }

        private static Bitmap ResizeBitmap(Bitmap bitmap)
        {
            
            var newHeight = bitmap.Height / WIDTH_OFFSET * maxWidth / bitmap.Width;
            if(bitmap.Width > maxWidth || bitmap.Height > newHeight)
            {
                bitmap = new Bitmap(bitmap, new Size(maxWidth, (int)newHeight));
            }
            return bitmap;
        }


    }
}
