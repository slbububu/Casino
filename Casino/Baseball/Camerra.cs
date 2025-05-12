using Casino;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace EpicGame
{
    public static class Cam    // Camerra
    {
        public static Vector3 defaultRotation = new Vector3(135, 0, 0);
        static Vector3 desiredRotation = defaultRotation;
        public static Vector3 rotation = defaultRotation;
        public static Vector3 CamPos = new Vector3(0, -20, 0);
        public static float fov = 90;
        private static float movementSpeed = 50;
        public static Vector2 convertToScreenXY(Vector3 INpos)
        {
            Vector3 rPos = INpos - CamPos; //relative position

            Vector2 OUTpos = new Vector2((float)Math.Atan2(rPos.Z, rPos.X),0); //nastavin X
            OUTpos = new Vector2(OUTpos.X, (float)Math.Atan2(rPos.Y, Math.Sqrt(Math.Pow(rPos.X,2) + Math.Pow(rPos.Z, 2))));   //nastavim Y
            
            OUTpos *= (float)((float)180 / Math.PI);    //rad -> degrees
            OUTpos -= new Vector2(rotation.X,rotation.Y);   //aply camera rotations
            OUTpos = new Vector2((OUTpos.X - 180) % 360 + 180, (OUTpos.Y - 180) % 360 + 180);   //aply camera rotations
            OUTpos /= fov;   //degrees -> (-0.5 , 0.5)
            OUTpos = new Vector2(OUTpos.X + 0.5f, OUTpos.Y + 0.5f); //(-0.5 , 0.5) -> (0, 1)
            OUTpos = new Vector2(OUTpos.X * (float)SF.SW, OUTpos.Y * (float)SF.SH);    //(0 , 1) -> screen XY

            return OUTpos;
        } 
        public static double ScreenSizeMult(Vector3 INpos)  //vynasob toto s velikosti objektu
        {
            Vector3 rPos = INpos - CamPos; //relative position
            double distance = Math.Sqrt(Math.Pow(rPos.X, 2) + Math.Pow(rPos.Y, 2) + Math.Pow(rPos.Z, 2));
            double returnVal = 1.0 / (distance * (fov/90));   //nemaz ".0" protoze se jinak vse posere
            return returnVal;  
        }
        public static void Rotate(System.Windows.Point mousePos) //tato je jen pro mys
        {
            rotation = new Vector3((float)mousePos.X / SF.SW * 360, (float)mousePos.Y / SF.SH * 360 - 180, 0);
        }
        public static void Rotate(Vector3 rot)
        {
            rotation = rot;
        }
        public static void LookatRotation(Vector3 desiredRotation)
        {
            Cam.desiredRotation = desiredRotation;
        }
        public static void Lookat(Vector3 desiredPosition)
        {
            Vector3 diff = desiredPosition - CamPos;

            float yaw = (float)Math.Atan2(diff.Z, diff.X);
            yaw = ToDegrees(yaw);

            float distanceXZ = (float)Math.Sqrt(diff.X * diff.X + diff.Z * diff.Z);
            float pitch = (float)Math.Atan2(diff.Y, distanceXZ);
            pitch = ToDegrees(pitch);

            desiredRotation = new Vector3(yaw, pitch, 0);

            //MainWindow.debugLabel.Content = "" + desiredPosition + "\n" + desiredRotation; //debug
        }
        public static void Update()
        {
            if(false) MoveUpdate(); //testing movement
            else rotation = Vector3.Lerp(rotation, desiredRotation, (float)MainWindow.deltaTime * 10);
        }
        static float ToDegrees(float radians)
        {
            return radians * (180f / (float)Math.PI);
        }
        static void MoveUpdate()
        {
            Vector3 input = Vector3.Zero;

            if (Keyboard.IsKeyDown(Key.W)) input += new Vector3(0, 0, 1);
            if (Keyboard.IsKeyDown(Key.S)) input += new Vector3(0, 0, -1);
            if (Keyboard.IsKeyDown(Key.A)) input += new Vector3(1, 0, 0);
            if (Keyboard.IsKeyDown(Key.D)) input += new Vector3(-1, 0, 0);
            if (Keyboard.IsKeyDown(Key.Space)) input += new Vector3(0, -1, 0);
            if (Keyboard.IsKeyDown(Key.LeftShift)) input += new Vector3(0, 1, 0);

            input *= movementSpeed * new Vector3((float)MainWindow.deltaTime, (float)MainWindow.deltaTime, (float)MainWindow.deltaTime);
            Cam.CamPos += Pich.Rotate(input, -rotation.X + 90);
        }
    }
}
