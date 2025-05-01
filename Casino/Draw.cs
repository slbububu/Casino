using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Media;

namespace Casino
{
    public static class Draw
    {
        /*
        public static void Clear()
        {
            canvas.Children.Clear(); // background clear +-
        }*/
        public static void RenderImage(string filePath, Vector2 position, Vector2 size, float rotation = 0f)
        {
            // Load the image
            BitmapImage bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.UriSource = new Uri(filePath, UriKind.RelativeOrAbsolute);
            bitmap.EndInit();

            // Create the image control
            Image img = new Image
            {
                Source = bitmap,
                Width = size.X,
                Height = size.Y,
                RenderTransformOrigin = new Point(0.5, 0.5)
            };

            // Apply rotation
            if (rotation != 0f)
            {
                img.RenderTransform = new RotateTransform(rotation);
            }

            // Position on canvas
            Canvas.SetLeft(img, position.X);
            Canvas.SetTop(img, position.Y);

            // Add to canvas
            MainWindow.GameCanvas.Children.Add(img);
        }

    }
}
