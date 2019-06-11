namespace NeuralNetworksAwari.AwariEngine.Evaluations
{
    public struct Evaluation
    {
        public AwariPosition Position {get;set;}
        public int FirstMove { get; set; }
        public int SecondMove { get; set; }
        public int Value { get; set; }
    }
}