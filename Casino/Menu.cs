using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Casino
{
    public static class Menu
    {
        private static float tiltSpeed = 2f;
        private static float tiltAmount = 10f;
        private static Vector2 iconSize = new Vector2(400, 400);
        private static Vector2 ruletaPos = new Vector2(SF.SW / 4, SF.SH / 2);
        private static Vector2 blackJackPos = new Vector2(SF.SW * 2 / 4, SF.SH / 2);
        private static Vector2 automatyPos = new Vector2(SF.SW * 3 / 4, SF.SH / 2);

        public static void Update()
        {
            
        }
        public static void Render()
        {
            Draw.RenderLabel("", new Vector2(SF.SW / 2, SF.SH / 2), new Vector2(SF.SW, SF.SH), 10, new Vector3(0, 0, 0), new Vector3(2, 114, 171));//pozadi

            float curentTilt = (float)Math.Sin(MainWindow.toralTime * tiltSpeed) * tiltAmount;
            Draw.RenderImage("Assets/Ruleta/ruleta.png", ruletaPos, iconSize, curentTilt);
            Draw.RenderImage("Assets/BlackJack/blackJack.png", blackJackPos, iconSize, curentTilt);
            Draw.RenderImage("Assets/Automaty/automaty.png", automatyPos, iconSize, curentTilt);
        }
        public static void LeftClick(Vector2 clickPos)
        {
            if(SF.DidIClick(clickPos, ruletaPos,iconSize)) MainWindow.selectedHra = "ruleta";
            if(SF.DidIClick(clickPos, blackJackPos, iconSize)) MainWindow.selectedHra = "blackJack";
            if(SF.DidIClick(clickPos, automatyPos, iconSize)) MainWindow.selectedHra = "automaty";
        }
    }
}
