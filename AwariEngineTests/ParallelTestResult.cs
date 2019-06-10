namespace NeuralNetworksAwari.AwariEngineTests
{
    public partial class ParallelEvaluationTests
    {
        public struct ParallelTestResult
        {
            public int Index { get; set; }
            public string FirstMove { get; set; }
            public string SecondMove { get; set; }
            public int Evaluation { get; set; }
        }
    }
}
