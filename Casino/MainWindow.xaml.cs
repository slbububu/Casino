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

namespace Casino
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            CompositionTarget.Rendering += GameLoop; // gameloop bude zavolan pred kazdym framem

            // Fullscreen
            WindowStyle = WindowStyle.None;
            WindowState = WindowState.Maximized;
            ResizeMode = ResizeMode.NoResize;
        }

        DateTime lastFrameTime = DateTime.Now;
        public static double deltaTime = 0;
        public static double toralTime = 0;
        bool gamePaused = false;
        bool gamePausedCahnged = false;
        private void GameLoop(object sender, EventArgs e)
        {
            DeltaTime();
            Update();
            Render();
        }
        private void Update()
        {

        }
        private void Render()
        {
            canvas.Children.Clear(); // background clear +-

            Draw.RenderImage("Assets/ball.png", new Vector2(100, 200), new Vector2(64, 64), 45f);

        }
        private void DeltaTime()
        {
            deltaTime = (DateTime.Now - lastFrameTime).TotalSeconds;
            lastFrameTime = DateTime.Now;

            //testing (debug mod je nepouzitelny bez tohoto)
            //deltaTime = 0.1;

            toralTime += deltaTime;
        }
    }
}