using System;
using System.Collections.Generic;
using System.Linq;

namespace AwariEngine
{
    public class AwariBoard
    {
        private AwariPosition _position;
        
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
            _position = new AwariPosition( 
                A, B, C, D, E, F, 
                a, b, c, d, e, f, 
                southAwari, 
                northAwari, 
                firstToMove==Player.South );

            if (TotalStones != 48)
                throw new ArgumentException($"AwariBoard should always contain 48 stones while {TotalStones} stones are passed.");
        }

        public int NorthAwari { get { return _position.Position[AwariPosition.NORTH_AWARI]; } }
        public int SouthAwari { get { return _position.Position[AwariPosition.SOUTH_AWARI]; } }
        public Player FirstToMove
        {
            get
            {
                return _position.SouthToMove ? Player.South : Player.North;
            }
        }
        public int TotalStones { get { return _position.Position.Sum(); } }
        public Dictionary<string, int> Pits
        {
            get
            {
                return Enumerable.Range(0, 12).ToDictionary(x => ((char)(x < 6 ? 'A' + x : 'a' - 6 + x)).ToString(), x => _position.Position[x]);
            }
        }

        public bool CanSow(string pit)
        {
            return PitIsOnSideToMove(pit) && _position.CanSow(GetPitIndex(pit));
        }

        public void Sow(string pit)
        {
            if (!PitIsOnSideToMove(pit))
            {
                if (_position.History.Count > 0)
                    throw new ArgumentException($"{(_position.SouthToMove ? Player.North : Player.South)} already moved, a player cannot make two moves in a row.");
                else
                    throw new ArgumentException($"Only south can make the first move (A-F).");
            }
            _position.Sow(GetPitIndex(pit));
        }        

        private int GetPitIndex(string pit)
        {
            var c = pit.ToCharArray()[0];
            var i = c <= 'F' ? c - 'A' : c - 'a';
            if (i<0 || i>5)
                throw new ArgumentException($"Invalid pit identifier '{pit}'");

            return i;
        }

        private bool PitIsOnSideToMove(string pit)
        {
            return _position.SouthToMove ^ pit.ToCharArray()[0] > 'F';
        }
    }
}