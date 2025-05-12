using Casino;
using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Numerics;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Navigation;

namespace EpicGame
{
    static public class Pich
    {
        public static bool isBallBehindFence = false;

        static public Vector3 StartPos = new Vector3(150, -20, 0);
        static Vector3 EndPos = new Vector3(50, -20, 0);
        static Vector2 BatSize = new Vector2(250, 900);
        static float VM = 0.4f * EndPos.X; //Vyska Moznosti
        static float SM = 0.8f * EndPos.X;  //Sirka Moznosti
        static public int ballSize = 6000;
        static float CHKMO = 0.3f; //CastHoduKdyMuzesOdrazit v sekundach
    
        public static string ballState = "neaktivni";
        static double batSwingState = -1; //(v sekundach)
        static double maxBatSwingState = 0.5; //(v sekundach)
        static float ttt;  //time to travel (v sekundach)
        static float tct;  //time curently travel (v sekundach)
        static Vector2 diff;
        public static Vector3 pos = new Vector3(0, -9999, 0);
        static Vector3 posLastFrame = new Vector3(0, -9999, 0);

        static Vector3 velocity; //toto je pouzito po odpalu a misu
        static EpicGame.Oponent oponent;

        /// <summary>
        /// pro TRUE -> exponentalni funguji hodnoty od 2 vyse ||| 
        /// pro FALSE -> hump funguji dobre -2 az 2, negativni cisla jsou podobna exponentalnim fcim|||
        /// ttt je delka hodu v sekundach
        /// </summary>
        static public void Throw(float ttt, EpicGame.Oponent oponent)
        {
            long ticks = DateTime.Now.Ticks;
            uint seed = (uint)(ticks & 0xFFFFFFFF); // just grabs the lower 32 
            MyRandom RNG = new MyRandom(seed);

            Pich.oponent = oponent;
            ballState = "beingthrown";
            pos = StartPos;
            Pich.ttt = ttt;
            tct = 0;
            diff = new Vector2(RNG.NewNumber % SM - SM / 2, RNG.NewNumber % VM - VM / 2);
            //pokud se RNG rozhodne dat male diff tha ho zvetsim
            if (Math.Abs(diff.X) < SM / 4) diff.X *= 2;
            if (Math.Abs(diff.Y) < VM / 4) diff.Y *= 2;

            //favourite mista na hazeni
            diff.X *= oponent.OblibeneMult.X;
            diff.Y *= oponent.OblibeneMult.Y;
            diff.X += oponent.OblibeneOftset.X * SM;
            diff.Y += oponent.OblibeneOftset.Y * VM;

            velocity = Vector3.Zero;
        }
        static public void Swing(Vector2 mouse)
        {
            if (batSwingState > 0) return;
            batSwingState = maxBatSwingState;

            float timeToEnd = ttt - tct;
            if (IsMouseOnBall(mouse) && timeToEnd < CHKMO && ballState == "beingthrown")
            {
                ballState = "odpalen";
                velocity = BallVelocity(mouse);
            }
        }
        static public void Update()
        {
            //MainWindow.debugLabel.Content = "" + ballState + "\n" + (int)pos.X + "\n" + (int)pos.Y + "\n" + (int)pos.Z;

            tct += (float)MainWindow.deltaTime;
            batSwingState -= MainWindow.deltaTime;

            if (ballState == "odpalen" || ballState == "misnuty" || ballState == "nazemi" || ballState == "win")
            {
                Vector3 dt = new Vector3((float)MainWindow.deltaTime, (float)MainWindow.deltaTime, (float)MainWindow.deltaTime);
                pos += velocity * dt;
                Vector3 gravity = new Vector3(0, PlayerStats.gravity, 0);
                velocity += gravity * dt;

                if (pos.Y > 0)
                {
                    if(ballState != "win") ballState = "nazemi";
                    velocity.Y = -velocity.Y;
                    pos.Y = -0.00001f;
                    velocity /= 2;
                }
            }
            if(ballState == "odpalen")
            {
                if (IsHomeRun())
                {
                    Draw.Zajasat(1);
                    ballState = "win";
                }
            }
            if (ballState == "beingthrown")
            {
                float interpStart = (1 - (tct / ttt));
                float interpEnd = tct / ttt;

                //uprimne doufam ze se tohoto nebudu muset dotykat
                pos = new Vector3(StartPos.X * interpStart + EndPos.X * interpEnd, StartPos.Y * interpStart + EndPos.Y* interpEnd, StartPos.Z * interpStart + EndPos.Z * interpEnd);
                if (oponent.exponentalY) pos = new Vector3(pos.X, pos.Y + diff.X * (float)Math.Pow(interpEnd, oponent.expDiff.Y), pos.Z);
                else pos = new Vector3(pos.X, pos.Y + diff.X * Hump(interpStart, oponent.expDiff.Y), pos.Z);
                if (oponent.exponentalX) pos = new Vector3(pos.X, pos.Y, pos.Z + diff.Y * (float)Math.Pow(interpEnd, oponent.expDiff.X));
                else pos = new Vector3(pos.X, pos.Y, pos.Z + diff.Y * Hump(interpStart, oponent.expDiff.X));

                //testing (simple throws)
                //pos = new Vector3(StartPos.X * interpStart + EndPos.X * interpEnd, StartPos.Y * interpStart + EndPos.Y * interpEnd, StartPos.Z * interpStart + EndPos.Z * interpEnd);

                if (pos.X < EndPos.X) ballState = "misnuty";

                pos = Rotate(pos, 225);

                Vector3 dt = new Vector3((float)MainWindow.deltaTime, (float)MainWindow.deltaTime, (float)MainWindow.deltaTime);
                velocity = (pos - posLastFrame) / dt;
                posLastFrame = pos;
            }

            isBallBehindFence = IsHomeRun(true);
        }
        static public void RenderAimRectangle()
        {
            Vector3 k = new Vector3(0, SM / 2, VM / 2) + EndPos;
            Vector3 l = new Vector3(0, -SM / 2, VM / 2) + EndPos;
            Vector3 m = new Vector3(0, -SM / 2, -VM / 2) + EndPos;
            Vector3 n = new Vector3(0, SM / 2, -VM / 2) + EndPos;


            k = Rotate(k, 225);
            l = Rotate(l, 225);
            m = Rotate(m, 225);
            n = Rotate(n, 225);

            int thickness = 1000;

            Draw.Line(k, l, thickness, Brushes.Red);
            Draw.Line(l, m, thickness, Brushes.Red);
            Draw.Line(m, n, thickness, Brushes.Red);
            Draw.Line(n, k, thickness, Brushes.Red);

            Draw.Circle(k, thickness, Brushes.Red);
            Draw.Circle(l, thickness, Brushes.Red);
            Draw.Circle(m, thickness, Brushes.Red);
            Draw.Circle(n, thickness, Brushes.Red);
        }
        static public void RenderBall()
        {
            if (ballState == "neaktivni") return;
            Brush myBrush = new SolidColorBrush(Color.FromRgb(75, 75, 75));
            if (ballState == "beingthrown")
            {
                float timeToEnd = ttt - tct;
                if (timeToEnd < CHKMO) myBrush = new SolidColorBrush(Color.FromRgb(0, 255, 0));
                else myBrush = new SolidColorBrush(Color.FromRgb(255, 0, 0));
            }
            if (ballState == "odpalen") myBrush = new SolidColorBrush(Color.FromRgb(25, 25, 25));

            if (ballState == "beingthrown" || ballState == "odpalen") Draw.Circle(pos, ballSize * 1.05, Brushes.Black);
            Draw.Circle(pos, ballSize, myBrush);
        }
        static public void Renderbat()
        {
            if (batSwingState < 0) //redy to swing
            {
                Vector2 batPos = new Vector2(1600, 100);
                Draw.FlatRectangle(batPos, batPos + BatSize, new SolidColorBrush(Color.FromRgb(128, 64, 0)));
            }
            else //swinging
            {
                Vector2 batPos = new Vector2(800, 700 - (int)(batSwingState * (100 / maxBatSwingState)));

                Draw.FlatRectangle(batPos, new Vector2(batPos.X + BatSize.Y, batPos.Y + BatSize.X), new SolidColorBrush(Color.FromRgb(128, 64, 0)));
            }
        }
        public static Vector3 Rotate(Vector3 pos, float angleDegrees)
        {
            //otoci Vector3 okolo osy Y

            float angleRadians = MathF.PI * angleDegrees / 180f;
            float cos = MathF.Cos(angleRadians);
            float sin = MathF.Sin(angleRadians);
            float x = pos.X * cos + pos.Z * sin;
            float z = -pos.X * sin + pos.Z * cos;
            return new Vector3(x, pos.Y, z);
        }
        static float Hump(float t, float peak)
        {
            return (float)(4 * peak * t * (1 - t) + (1 - t) * 3/4);
        }
        static bool IsMouseOnBall(Vector2 mouse)
        {
            Vector2 ballpos = Cam.convertToScreenXY(pos);
            double alowedDist = (Cam.ScreenSizeMult(pos) * ballSize );
            alowedDist /= 2; //prumner -> polomner
            double dist = Math.Sqrt(Math.Pow(ballpos.X - mouse.X, 2) + Math.Pow(ballpos.Y - mouse.Y, 2));

            return dist < alowedDist;
        }
        static Vector3 BallVelocity(Vector2 mouse)
        {
            float zvetseniVyskoveSily = 2f; // timto chci fixnout problem toho ze nickje piche byly mnohel tezsi na odpaleni prez plot

            Vector2 ballpos = Cam.convertToScreenXY(pos);
            Vector2 diff = mouse - ballpos;
            double alowedDist = (Cam.ScreenSizeMult(pos) * ballSize);
            alowedDist /= 2; //prumner -> polomner
            double dist = Math.Sqrt(Math.Pow(diff.X, 2) + Math.Pow(diff.Y, 2));


            diff /= (float)alowedDist;
            //testing
            //diff = new Vector2(1,0);
            diff *= (float)Math.PI/2;
            Vector3 vel = new Vector3(0.8f, (float)Math.Sin(-diff.Y) * zvetseniVyskoveSily, -(float)Math.Sin(diff.X));

            float jakMocStredFloat = (float)Math.Cos((dist / alowedDist) * Math.PI / 2); 
            Vector3 jakMocStred = new Vector3(jakMocStredFloat, jakMocStredFloat, jakMocStredFloat);

            vel *= PlayerStats.power * jakMocStred;

            vel = Rotate(vel,225);

            return vel;
        }
        static bool IsHomeRun()
        {
            if (oponent == null) return false;

            return ((-pos.Y > Draw.vyskaFence) && (Math.Abs(pos.X) > oponent.fenceDistance || Math.Abs(pos.Z) > oponent.fenceDistance));
        } 
        static bool IsHomeRun(bool ignoreHeight)
        {
            if (oponent == null) return false;

            if(ignoreHeight) return (Math.Abs(pos.X) > oponent.fenceDistance || Math.Abs(pos.Z) > oponent.fenceDistance);
            else return ((-pos.Y > Draw.vyskaFence) && (Math.Abs(pos.X) > oponent.fenceDistance || Math.Abs(pos.Z) > oponent.fenceDistance));
        }
        static public bool IsBallFartherThat(float distance)
        {
            Vector3 diff = pos - Cam.CamPos;
            float ballDist = (float)Math.Sqrt(Math.Pow(diff.X, 2) + Math.Pow(diff.Y, 2));
            return distance > ballDist;
        }
    }
}