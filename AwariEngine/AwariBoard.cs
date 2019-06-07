using System;
using System.Collections.Generic;
using System.Linq;

namespace AwariEngine
{
    public class AwariBoard
    {
        private const int SOUTH_AWARI = 12;
        private const int NORTH_AWARI = 13;
        private readonly string[] _pits = new[] { "A", "B", "C", "D", "E", "F", "a", "b", "c", "d", "e", "f" };
        private int[] _position;
        private List<int[]> _history;
        private readonly Player _initialFirstMove;

        public AwariBoard(
            int A, int B, int C, int D, int E, int F, 
            int a, int b, int c, int d, int e, int f, 
            int southAwari, int northAwari, 
            Player firstToMove = Player.South)
        {
            _position = new[] { A, B, C, D, E, F, a, b, c, d, e, f, southAwari, northAwari };

            if (_position.Sum() != 48)
            {
                throw new ArgumentException("AwariBoard should always contain 48 stones.");
            }

            _history = new List<int[]>();
        }

        public int NorthAwari { get { return _position[NORTH_AWARI]; } }
        public int SouthAwari { get { return _position[SOUTH_AWARI]; } }
        public Player FirstToMove
        {
            get
            {
                return _history.Count % 2 == 0 || _initialFirstMove == Player.North ? Player.South : Player.North;
            }
        }
        public int TotalStones { get { return _position.Sum(); } }
        public Dictionary<string, int> Pits
        {
            get
            {
                return Enumerable.Range(0, 12).ToDictionary(x => _pits[x], x => _position[x]);
            }
        }

        public void Move(int pit)
        {
            int[] previous = new int[14];
            _position.CopyTo(previous, 0);

            _history.Add(previous);
        }
        
        public static AwariBoard GetInitialBoard()
        {
            return new AwariBoard(4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 0, 0);
        }
    }
}