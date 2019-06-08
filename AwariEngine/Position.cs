﻿using System.Collections.Generic;

namespace AwariEngine
{
    public class AwariPosition
    {
        public const int SOUTH_AWARI = 12;
        public const int NORTH_AWARI = 13;
        private static readonly string[] _pits = new[] { "A", "B", "C", "D", "E", "F", "a", "b", "c", "d", "e", "f" };
        
        public AwariPosition(
            int A, int B, int C, int D, int E, int F, 
            int a, int b, int c, int d, int e, int f, 
            int southAwari, int northAwari, 
            Player firstToMove = Player.South)
        {
            Position = new[] { A, B, C, D, E, F, a, b, c, d, e, f, southAwari, northAwari };
            History = new List<int[]>();
        }

        public int[] Position { get; }
        public List<int[]> History { get; }
        public Player InitialFirstMove { get; }

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
            while (stones>0)
            {
                pit = pit < 11 ? pit + 1 : 0;
                Position[pit]++;
                stones--;
            }

            History.Add(previous);
        }
    }
}