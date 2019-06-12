using System.Collections.Generic;

namespace NeuralNetworksAwari.AwariEngine
{
    public class AwariPositionWithRepetitionDetection: AwariPosition
    {
        private Stack<int> _lastCapture;

        public AwariPositionWithRepetitionDetection(AwariPosition awariPosition) : base(awariPosition)
        {
            _lastCapture = new Stack<int>();
            _lastCapture.Push(0);
        }

        public override AwariPosition Copy()
        {
            return new AwariPositionWithRepetitionDetection(base.Copy());
        }

        public override void MoveBack()
        {
            base.MoveBack();

            _lastCapture.Pop();
        }

        public override void Sow(int pit)
        {
            base.Sow(pit);

            _lastCapture.Push(Position[NORTH_AWARI] > History[History.Count - 1][SOUTH_AWARI] ? 0 : _lastCapture.Peek() + 1);

            if (_lastCapture.Peek() > 10)
            {
                EndGameIfPositionIsRepeated();
            }
        }

        private void EndGameIfPositionIsRepeated()
        {
            for (var i = History.Count - 2; i >= History.Count - _lastCapture.Peek(); i -= 2)
            {
                if (Position[0] == History[i][0] &&
                    Position[1] == History[i][1] &&
                    Position[2] == History[i][2] &&
                    Position[3] == History[i][3] &&
                    Position[4] == History[i][4] &&
                    Position[5] == History[i][5] &&
                    Position[6] == History[i][6] &&
                    Position[7] == History[i][7] &&
                    Position[8] == History[i][8] &&
                    Position[9] == History[i][9] &&
                    Position[10] == History[i][10] &&
                    Position[11] == History[i][11])
                {
                    for (var j = 0; j < 12; j++)
                    {
                        Position[j < 6 ? SOUTH_AWARI : NORTH_AWARI] += Position[j];
                        Position[j] = 0;
                    }
                }
            }
        }
    }
}