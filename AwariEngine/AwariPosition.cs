using System;
using System.Collections.Generic;
using System.Linq;

namespace NeuralNetworksAwari.AwariEngine
{
    public class AwariPosition
    {
        public const int SOUTH_AWARI = 12;
        public const int NORTH_AWARI = 13;
        private Func<int, int> _nextPit = (x) => x += x < 11 ? 1 : -11;
        private readonly int[] _previous;
        
        public AwariPosition(int[] position)
        {
            Position = position;
            History = new List<int[]>();
        }

        public AwariPosition(AwariPosition position)
        {
            Position = position.Position;
            History = position.History;
        }

        public int[] Position { get; protected set; }
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

        public virtual AwariPosition Copy()
        {
            var p = new AwariPosition(Position.ToList().ToArray());
            History.ForEach(x => p.History.Add(x.ToList().ToArray()));
            return p;
        }

        private void ForEach(Func<object, object> p)
        {
            throw new NotImplementedException();
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

        public virtual void MoveBack()
        {
            Position = History[History.Count - 1];
            History.RemoveAt(History.Count - 1);
        }

        public virtual void Sow(int pit)
        {
            var previous = new int[14];
            Position.CopyTo(previous, 0);

            var stones = Position[pit];
            Position[pit] = 0;

            var p = pit;
            while (stones > 0)
            {
                p = _nextPit(p);
                if (p == pit) p = _nextPit(p);
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

            if (Position[0]==0 && Position[1] == 0 && Position[2] == 0 && Position[3] == 0 && Position[4] == 0 && Position[5] == 0)
            {
                for (var i = 6; i < 12; i++)
                {
                    Position[i] = 0;
                }
                Position[NORTH_AWARI] = 48 - Position[SOUTH_AWARI];
            }
        }
    }
}