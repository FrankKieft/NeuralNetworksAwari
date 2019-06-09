using System;
using System.Collections.Generic;

namespace AwariEngine
{
    public class AwariPosition
    {
        public const int SOUTH_AWARI = 12;
        public const int NORTH_AWARI = 13;
        private static readonly string[] _pits = new[] { "A", "B", "C", "D", "E", "F", "a", "b", "c", "d", "e", "f" };
        private Func<int, int> NextPit = (x) => x += x < 11 ? 1 : -11;
        
        public AwariPosition(
            int A, int B, int C, int D, int E, int F, 
            int a, int b, int c, int d, int e, int f, 
            int southAwari, int northAwari, 
            bool southToMove = true)
        {
            Position = new[] { A, B, C, D, E, F, a, b, c, d, e, f, southAwari, northAwari };
            History = new List<int[]>();
            SouthToMove = southToMove;
        }

        public int[] Position { get; }
        public List<int[]> History { get; }
        public bool SouthToMove { get; private set; }

        public bool CanSow(int pit)
        {
            return Position[pit] > 0;
        }

        public void Sow(int pit)
        {
            int[] previous = new int[14];
            Position.CopyTo(previous, 0);

            var stones = Position[pit];
            Position[pit] = 0;

            var p = pit;
            while (stones>0)
            {
                p = NextPit(p);
                if (p == pit) p=NextPit(p);
                Position[p]++;
                stones--;
            }

            while (p>=0 && (SouthToMove ^ p < 6) && Position[p] > 1 && Position[p] < 4)
            {
                Position[SouthToMove ? SOUTH_AWARI : NORTH_AWARI] += Position[p];
                Position[p] = 0;
                p--;
            }
            
            History.Add(previous);
            SouthToMove = !SouthToMove;
        }
    }
}