using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Chef.Shared
{
    public class StreamConverter
    {
        private string trashIcon = "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";

        public byte[] convertToByte(string fileName)
        {
            Stream stream = File.OpenRead(fileName);
            byte[] data = new byte[stream.Length];
            stream.Read(data, 0, data.Length);
            stream.Flush();
            stream.Close();
            return data;
        }

        public byte[] convertBase64ToByte(string base64)
        {
            return Convert.FromBase64String(base64);
        }

        public Image base64ToImage(byte[] binaryData)
        {
            BitmapImage imageData = new BitmapImage();
            imageData.BeginInit();
            imageData.StreamSource = new MemoryStream(binaryData);
            imageData.EndInit();

            BitmapImage trash = new BitmapImage();
            trash.BeginInit();
            trash.StreamSource = new MemoryStream(this.convertBase64ToByte(this.trashIcon));
            trash.EndInit();

            Image image = new Image();
            image.Source = imageData;
            image.Width = 40;
            image.Height = 40;
            image.Margin = new Thickness(12, 0, 0, 0);
            image.Stretch = Stretch.Fill;
            image.Cursor = Cursors.Hand;

            image.MouseEnter += (sender, args) => { image.Source = trash; };
            image.MouseLeave += (sender, args) => { image.Source = imageData; };

            return image;
        }
    }

}
