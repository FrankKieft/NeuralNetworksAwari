using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;

namespace NeuralNetworksAwari.AwariEngine
{
    public class AwariBoard
    {
        private AwariPosition _position;
        private AwariEvaluator _evaluator;

        public static AwariBoard GetInitialBoard()
        {
            return new AwariBoard(4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4);
        }

        public AwariBoard( 
            int f=0, int e=0, int d=0, int c=0, int b=0, int a=0, 
            int A=0, int B=0, int C=0, int D=0, int E=0, int F=0,
            int southAwari=0, int northAwari=0, 
            Player firstToMove = Player.South)
        {
            _evaluator = new AwariEvaluator();
            FirstToMove = firstToMove;
            if (firstToMove == Player.South)
            {
                _position = new AwariPosition(new[]
                    { A, B, C, D, E, F,
                  a, b, c, d, e, f,
                  southAwari,
                  northAwari});
            }
            else
            {
                _position = new AwariPosition(new[]
                    { a, b, c, d, e, f,
                  A, B, C, D, E, F,
                  northAwari,
                  southAwari});
            }

            if (TotalStones != 48)
                throw new ArgumentException($"AwariBoard should always contain 48 stones while {TotalStones} stones are passed.");
        }

        public int Evaluate(int depth)
        {
            return _evaluator.Evaluate(_position, depth);
        }

        public Player FirstToMove { get; private set; }
        public int NorthAwari { get { return _position.Position[FirstToMove == Player.South ? AwariPosition.NORTH_AWARI: AwariPosition.SOUTH_AWARI]; } }
        public int SouthAwari { get { return _position.Position[FirstToMove == Player.South ? AwariPosition.SOUTH_AWARI : AwariPosition.NORTH_AWARI]; } }
        public int TotalStones { get { return _position.Position.Sum(); } }
        public bool GameHasEnded { get { return SouthAwari + NorthAwari == 48; } }

        public Dictionary<string, int> Pits
        {
            get
            {
                var position = FirstToMove == Player.South ? _position.Position : AwariPosition.FlipPosition(_position.Position);
                return Enumerable.Range(0, 12).ToDictionary(x => ((char)(x < 6 ? 'A' + x : 'a' + x - 6)).ToString(), x => position[x]);
            }
        }

        public bool CanSow(string pit)
        {
            return PitIsOnSideToMove(pit) && _position.CanSow().Contains(GetPitIndex(pit));
        }

        public AwariBoard Sow(string pit)
        {
            if (!PitIsOnSideToMove(pit))
            {
                if (_position.History.Count > 0)
                    throw new ArgumentException($"{(FirstToMove == Player.South ? Player.North : Player.South)} already moved, a player cannot make two moves in a row.");
                else
                    throw new ArgumentException($"Only South can make the first move (A-F).");
            }
            if (CanSow(pit))
            {
                _position.Sow(GetPitIndex(pit));
                FirstToMove = FirstToMove == Player.South ? Player.North : Player.South;
            }
            return this;
        }        

        private int GetPitIndex(string pit)
        {
            var c = pit.ToCharArray()[0];
            var i = c <= 'F' ? c - 'A' : c - 'a';
            if (i<0 || i>11)
                throw new ArgumentException($"Invalid pit identifier '{pit}'");

            return i;
        }

        private bool PitIsOnSideToMove(string pit)
        {
            return FirstToMove == Player.South ^ pit.ToCharArray()[0] > 'F';
        }
    }
}