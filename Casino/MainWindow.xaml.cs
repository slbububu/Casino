using System.Diagnostics;
using System.Drawing;
using System.Reflection.Emit;
using System;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Numerics;
using System.Data;

namespace Casino
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static Canvas GameCanvas;
        public static string selectedHra = "menu";
        public MainWindow()
        {
            InitializeComponent();
            CompositionTarget.Rendering += GameLoop; // gameloop bude zavolan pred kazdym framem

            // Fullscreen
            WindowStyle = WindowStyle.None;
            WindowState = WindowState.Maximized;
            ResizeMode = ResizeMode.NoResize;

            //dela veci v MainWindow.xaml pristupne i od jinud nez od tud
            GameCanvas = canvas;
        }

        DateTime lastFrameTime = DateTime.Now;
        public static double deltaTime = 0;
        public static double toralTime = 0;
        bool gamePaused = false;
        bool gamePausedCahnged = false;
        public static int money = 10000;
        private void GameLoop(object sender, EventArgs e)
        {
            DeltaTime();
            Update();
            Render();
        }
        private void Update()
        {
            if(Keyboard.IsKeyDown(Key.Escape))
                selectedHra = "menu"; // escape to menu

            switch (selectedHra)
            {
                case "menu":
                    Menu.Update();
                    break;
                case "ruleta":
                    Ruleta.Update();
                    break;
                case "blackJack":
                    BlackJack.Update();
                    break;
                case "automaty":
                    Automaty.Update();
                    break;
            }
        }
        private void Render()
        {
            canvas.Children.Clear(); // background clear +-

            switch (selectedHra)
            {
                case "menu":
                    Menu.Render();
                    break;                    
                case "ruleta":
                    Ruleta.Render();
                    break;
                case "blackJack":
                    BlackJack.Render();
                    break;
                case "automaty":
                    Automaty.Render();
                    break;
            }

            Vector2 position = new Vector2(SF.SW-150, 100);
            Vector2 size = new Vector2(250, 100);
            float fontSize = 50;

            // RGB barvy jako Vector3
            Vector3 textColor = new Vector3(0, 0, 0);
            Vector3 backgroundColor = new Vector3(255, 255, 0);

            // Vytvoření labelu na obrazovce
            Draw.RenderLabel(money.ToString() + "$", position, size, fontSize, textColor, backgroundColor);
        }
        private void DeltaTime()
        {
            deltaTime = (DateTime.Now - lastFrameTime).TotalSeconds;
            lastFrameTime = DateTime.Now;

            //testing (debug mod je nepouzitelny bez tohoto)
            //deltaTime = 0.1;

            toralTime += deltaTime;
        }

        private void canvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            System.Windows.Point clickPosPOINT = e.GetPosition(GameCanvas);
            Vector2 clickPos = new Vector2((float)clickPosPOINT.X, (float)clickPosPOINT.Y);

            switch (selectedHra)
            {
                case "menu":
                    Menu.LeftClick(clickPos);
                    break;                    
                case "ruleta":
                    Ruleta.LeftClick(clickPos);
                    break;
                case "blackJack":
                    BlackJack.LeftClick(clickPos);
                    break;
                case "automaty":
                    Automaty.LeftClick(clickPos);
                    break;
            }
        }
    }
}