using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Casino
{
    public static class Automaty
    {
        public static void Update()
        {
            
        }
        public static void Render()
        {
            Draw.RenderImage("Assets/automaty.png", new Vector2(SF.SW / 2, SF.SH / 2), new Vector2(SF.SW, SF.SH));
        }
        public static void LeftClick(Vector2 clickPos)
        {

        }
    }
}
