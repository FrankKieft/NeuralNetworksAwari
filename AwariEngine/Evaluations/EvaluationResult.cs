namespace NeuralNetworksAwari.AwariEngine.Evaluations
{
    public struct EvaluationResult
    {
        public EvaluationResult(int value, int firstMove = -1, int secondMove = -1)
        {
            FirstMove = firstMove;
            SecondMove = secondMove;
            Value = value;
        }

        public int FirstMove { get; }
        public int SecondMove { get; }
        public int Value { get; }
    }
}