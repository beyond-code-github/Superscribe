namespace Superscribe.Demo.Neural.NeuralNetwork
{
    using System;

    public static class RandomGenerator
    {
        public static Random Random = new Random();

        public static double Generate()
        {
            return Random.NextDouble();
        }
    }
}