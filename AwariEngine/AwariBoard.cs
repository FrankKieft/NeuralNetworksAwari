using System;
using System.Collections.Generic;
using System.Linq;

namespace AwariEngine
{
    public class AwariBoard
    {
        private AwariPosition _position;
        private static readonly string[] _pits = new[] { "A", "B", "C", "D", "E", "F", "a", "b", "c", "d", "e", "f" };
        private Func<string, int> GetIndex = pit => Array.IndexOf(_pits, pit);

        public static AwariBoard GetInitialBoard()
        {
            return new AwariBoard(4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 0, 0);
        }

        public AwariBoard(
            int A, int B, int C, int D, int E, int F, 
            int a, int b, int c, int d, int e, int f, 
            int southAwari, int northAwari, 
            Player firstToMove = Player.South)
        {
            _position = new AwariPosition( A, B, C, D, E, F, a, b, c, d, e, f, southAwari, northAwari );

            if (TotalStones != 48)
            {
                throw new ArgumentException($"AwariBoard should always contain 48 stones while {TotalStones} stones are passed.");
            }
        }

        public int NorthAwari { get { return _position.Position[AwariPosition.NORTH_AWARI]; } }
        public int SouthAwari { get { return _position.Position[AwariPosition.SOUTH_AWARI]; } }
        public Player FirstToMove
        {
            get
            {
                return _position.History.Count % 2 == 0 || _position.InitialFirstMove == Player.North ? Player.South : Player.North;
            }
        }
        public int TotalStones { get { return _position.Position.Sum(); } }
        public Dictionary<string, int> Pits
        {
            get
            {
                return Enumerable.Range(0, 12).ToDictionary(x => _pits[x], x => _position.Position[x]);
            }
        }

        public bool CanSow(string pit)
        {
            var i = GetIndex(pit);

            return _position.CanSow(GetIndex(pit));
        }

        public void Sow(string pit)
        {
            _position.Sow(GetIndex(pit));
        }        
    }
}