using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.Windows;
using System.Net.NetworkInformation;

namespace Casino
{
    public static class Draw
    {
        /*
        public static void Clear()
        {
            canvas.Children.Clear(); // background clear +-
        }*/

        /// <summary>
        /// pokud ti tato funkce nefunguje na tvuj novy obrazek tak to spravis takto. 
        /// Klikni v "solutin exploreru" na nefungujici obrazek a v "properties" nastav "Build Action" na "Resource"
        /// </summary>
        public static void RenderImage(string filePath, Vector2 position, Vector2 size, float rotation = 0f)
        {
            // Load the image
            BitmapImage bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.UriSource = new Uri(filePath, UriKind.RelativeOrAbsolute);
            bitmap.EndInit();

            //image 
            Image img = new Image
            {
                Source = bitmap,
                Width = size.X,
                Height = size.Y,
                RenderTransformOrigin = new System.Windows.Point(0.5, 0.5),
                IsHitTestVisible = false //aby to to nezablokovalo kliknuti
            };

            // rotation
            if (rotation != 0f) img.RenderTransform = new RotateTransform(rotation);

            // Position v canvas
            Canvas.SetLeft(img, position.X - size.X / 2);
            Canvas.SetBottom(img, position.Y - size.Y / 2);

            // srci to na canvas
            MainWindow.GameCanvas.Children.Add(img);
        }
        public static void RenderLabel(string text, Vector2 position, Vector2 size, float fontSize = 14f, Vector3 textColor = default, Vector3 backgroundColor = default)
        {
            // Vytvoření labelu
            Label label = new Label
            {
                Content = text,
                Width = size.X,
                Height = size.Y,
                FontSize = fontSize,
                Foreground = new SolidColorBrush(Color.FromRgb((byte)textColor.X, (byte)textColor.Y, (byte)textColor.Z)),
                Background = new SolidColorBrush(Color.FromRgb((byte)backgroundColor.X, (byte)backgroundColor.Y, (byte)backgroundColor.Z)),
                HorizontalContentAlignment = HorizontalAlignment.Center,
                VerticalContentAlignment = VerticalAlignment.Center,
                IsHitTestVisible = false // Aby label nezabíral interaktivní plochu
            };

            // Nastavení pozice na canvasu
            Canvas.SetLeft(label, position.X - size.X / 2);
            Canvas.SetBottom(label, position.Y - size.Y / 2);

            // Přidání labelu na canvas
            MainWindow.GameCanvas.Children.Add(label);
        }
    }
}