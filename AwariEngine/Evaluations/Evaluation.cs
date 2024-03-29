﻿namespace NeuralNetworksAwari.AwariEngine.Evaluations
{
    public struct Evaluation
    {
        public Evaluation(AwariPosition position, int firstMove = -1, int secondMove = -1, int value = 0)
        {
            Position = position;
            FirstMove = firstMove;
            SecondMove = secondMove;
        }

        public AwariPosition Position { get; }
        public int FirstMove { get; }
        public int SecondMove { get; }
    }
}