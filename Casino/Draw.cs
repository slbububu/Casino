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
using System.Windows.Shapes;
using EpicGame;
using System.ComponentModel;

namespace Casino
{
    public static class Draw
    {
        /// <summary>
        /// pokud ti tato funkce nefunguje na tvuj novy obrazek tak to spravis takto. 
        /// Klikni v "solutin exploreru" na nefungujici obrazek a v "properties" nastav "Build Action" na "Resource"/"Zdroj"
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
        public static void Circle(Vector3 pos, double size, Brush color)
        {
            double WidthX = size * Cam.ScreenSizeMult(pos);
            double HeightX = size * Cam.ScreenSizeMult(pos);

            Ellipse crcl = new Ellipse
            {
                Width = WidthX,
                Height = HeightX,
                Fill = color,
                IsHitTestVisible = false
            };

            Canvas.SetLeft(crcl, Cam.convertToScreenXY(pos).X - WidthX / 2);
            Canvas.SetTop(crcl, Cam.convertToScreenXY(pos).Y - HeightX / 2);
            MainWindow.GameCanvas.Children.Add(crcl);
        }
        public static void Line(Vector3 pos1, Vector3 pos2, double size, Brush color)
        {
            size *= (Cam.ScreenSizeMult(pos1) + Cam.ScreenSizeMult(pos2)) / 2;

            Vector2 a = Cam.convertToScreenXY(pos1);
            Vector2 b = Cam.convertToScreenXY(pos2);

            Line l = new Line
            {
                X1 = Cam.convertToScreenXY(pos1).X,
                Y1 = Cam.convertToScreenXY(pos1).Y,
                X2 = Cam.convertToScreenXY(pos2).X,
                Y2 = Cam.convertToScreenXY(pos2).Y,
                StrokeThickness = size,
                Stroke = color,
                IsHitTestVisible = false
            };
            MainWindow.GameCanvas.Children.Add(l);
        }
        public static void FlatRectangle(Vector2 pos1, Vector2 pos2, Brush color) // davas souradky obrazovky
        {
            double x = Math.Min(pos1.X, pos2.X);
            double y = Math.Min(pos1.Y, pos2.Y);
            double width = Math.Abs(pos2.X - pos1.X);
            double height = Math.Abs(pos2.Y - pos1.Y);

            System.Windows.Shapes.Rectangle rect = new System.Windows.Shapes.Rectangle
            {
                Width = width,
                Height = height,
                Fill = color,
                IsHitTestVisible = false
            };

            Canvas.SetLeft(rect, x);
            Canvas.SetTop(rect, y);
            MainWindow.GameCanvas.Children.Add(rect);
        }
        public static void Ground(Brush color)
        {
            double size = 50000;

            Ellipse crcl = new Ellipse
            {
                Width = size,
                Height = size,
                Fill = color,
                IsHitTestVisible = false
            };

            Canvas.SetLeft(crcl, (SF.SW - size) / 2);
            Canvas.SetBottom(crcl, Cam.rotation.Y * 12 * (90 / Cam.fov) - size + 550);//je tu spoustu magickych cisel, ale +- to funguje, takze na to nemusime sahat :)
            MainWindow.GameCanvas.Children.Add(crcl);
        }
        public static void Background(Brush color)
        {
            System.Windows.Shapes.Rectangle rt = new System.Windows.Shapes.Rectangle
            {
                Width = SF.SW,
                Height = SF.SH,
                Fill = color,
                IsHitTestVisible = false
            };

            Canvas.SetLeft(rt, 0);
            Canvas.SetTop(rt, 0);
            MainWindow.GameCanvas.Children.Add(rt);
        }
        public static int vyskaFence = 0;
        public static void Fence(float distance, Brush color)
        {
            float size = 2000;
            int rozdilVysek = 10;

            Vector3 a = Vector3.Zero;
            Vector3 b = Vector3.Zero;
            Vector3 c = Vector3.Zero;
            Vector3 d = Vector3.Zero;
            Vector3 hv = Vector3.Zero;

            int roz = 5;
            int heights = 4;

            for (int r = 0; r < roz; r++) //rozdeleni
            {
                for (int h = 0; h < heights; h++)     //heights
                {
                    a = new Vector3(-distance, 0, distance * r / roz);
                    b = new Vector3(-distance, 0, distance * (r + 1) / roz);
                    c = new Vector3(-distance * r / roz, 0, distance);
                    d = new Vector3(-distance * (r + 1) / roz, 0, distance);

                    hv = new Vector3(0, -h * rozdilVysek, 0);

                    Line(a + hv, b + hv, size, color);
                    Line(c + hv, d + hv, size, color);
                }
                a = new Vector3(-distance, 0, distance * r / roz);
                b = new Vector3(-distance, 0, distance * r / roz);
                c = new Vector3(-distance * r / roz, 0, distance);
                d = new Vector3(-distance * r / roz, 0, distance);

                Line(a, b + hv, size, color);
                Line(c, d + hv, size, color);

                Circle(a, size, color);
                Circle(b + hv, size, color);
                Circle(c, size, color);
                Circle(d + hv, size, color);
            }

            a = new Vector3(-distance, 0, distance);
            Line(a, a + hv, size, color);

            vyskaFence = rozdilVysek * heights;
        }
        public static void Divaci(float distance, uint kolik)
        {
            MyRandom RNG = new MyRandom(kolik);

            int vyskaOdstup = 7;
            int vyskaOfset = -20;
            int sirkaOdstup = 1;
            int velikostDivaku = 5000;
            uint pocetmist = 120;
            bool[] obsazeno = new bool[pocetmist];
            for (int i = 0; i < pocetmist; i++) { obsazeno[i] = false; }
            if (kolik > pocetmist) kolik = pocetmist;

            for (int i = 0; i < kolik; i++)
            {
                uint value = RNG.NewNumber % pocetmist;

                if (obsazeno[value]) { i--; continue; }
                ; // , aby se nekreslili divaci prez sebe
                obsazeno[value] = true;

                if (value < pocetmist / 2)
                {
                    Vector3 pos = new Vector3(-distance, vyskaOfset - (value / 20) * vyskaOdstup, distance * (value % 20) / 20 * sirkaOdstup);
                    pos -= new Vector3(0, vyskaDivaku * vyskaOdstup, 0);
                    Circle(pos, velikostDivaku * 1.1, Brushes.Black);
                    Circle(pos, velikostDivaku, Brushes.Orange);
                }
                else
                {
                    value -= pocetmist / 2;

                    Vector3 pos = new Vector3(-distance * (value % 20) / 20 * sirkaOdstup, vyskaOfset - (value / 20) * vyskaOdstup, distance);
                    pos -= new Vector3(0, vyskaDivaku * vyskaOdstup, 0);
                    Circle(pos, velikostDivaku * 1.1, Brushes.Black);
                    Circle(pos, velikostDivaku, Brushes.Orange);
                }
            }

            if (vyskaDivaku > 0) vyskaDivaku -= (float)MainWindow.deltaTime;
            else vyskaDivaku = 0;
        }
        static float vyskaDivaku = 0;
        public static void Zajasat(float jakDlouho) //jak dlouho v sekundach
        {
            vyskaDivaku = jakDlouho;
        }
        public static void Enemy()
        {
            Vector3 enemyPos = Pich.Rotate(Pich.StartPos, 225);

            Vector2 enemyPos2D = new Vector2(Cam.convertToScreenXY(enemyPos).X, SF.SH-Cam.convertToScreenXY(enemyPos).Y);
            Vector2 enemySize = new Vector2 (350,350);

            Draw.RenderImage("Assets/Enemies/" + GameManager.GetOponent().fileName + ".png", enemyPos2D, enemySize);
        }

    }
}