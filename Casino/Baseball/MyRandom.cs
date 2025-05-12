using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EpicGame
{
    internal class MyRandom
    {
        public MyRandom(uint seed) { newNumber = seed; }
        private uint newNumber;
        public uint NewNumber
        {
            get
            {
                newNumber = newNumber * newNumber;
                newNumber += newNumber % 6729599;
                newNumber += 679389209;
                return newNumber;
            }
            set => newNumber = newNumber; //nejde nastavit jinde nez 
        }
    }
}
