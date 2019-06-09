﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace AwariEngine
{
    public class AwariPosition
    {
        public const int SOUTH_AWARI = 12;
        public const int NORTH_AWARI = 13;
        private Func<int, int> NextPit = (x) => x += x < 11 ? 1 : -11;

        public AwariPosition(int[] position)
        {
            Position = position;
            History = new List<int[]>();
        }

        public int[] Position { get; private set; }
        public List<int[]> History { get; }

        public static int[] FlipPosition(int[] position)
        {
            int[] newPosition = new int[14];
            Array.Copy(position, 6, newPosition, 0, 6);
            Array.Copy(position, 0, newPosition, 6, 6);
            newPosition[NORTH_AWARI] = position[SOUTH_AWARI];
            newPosition[SOUTH_AWARI] = position[NORTH_AWARI];
            return newPosition;
        }

        public List<int> CanSow()
        {
            var pits = new List<int>();
            var noMoveLeftPits = new List<int>();
            for (var i = 0; i < 6; i++)
            {
                if (Position[i] > 0)
                {
                    Sow(i);
                    if (Position[0] + Position[1] + Position[2] + Position[3] + Position[4] + Position[5] == 0)
                    {
                        noMoveLeftPits.Add(i);
                    }
                    else
                    {
                        pits.Add(i);
                    }
                    MoveBack();
                }
            }
            return pits.Count == 0 ? noMoveLeftPits : pits;
        }

        private void MoveBack()
        {
            Position = History.Last();
            History.Remove(Position);
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

            while (p > 5 && Position[p] > 1 && Position[p] < 4)
            {
                Position[SOUTH_AWARI] += Position[p];
                Position[p] = 0;
                p--;
            }

            Position = FlipPosition(Position);
            History.Add(previous);
        }
    }
}