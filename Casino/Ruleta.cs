using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Casino
{
    public static class Ruleta
    {
        public static void Update()
        {
            
        }
        public static void Render()
        {
            Draw.RenderLabel("", new Vector2(SF.SW / 2, SF.SH / 2), new Vector2(SF.SW, SF.SH), 10, new Vector3(0, 0, 0), new Vector3(2, 114, 171));//pozadi
        }
        public static void LeftClick(Vector2 clickPos)
        {

        }
    }
}
