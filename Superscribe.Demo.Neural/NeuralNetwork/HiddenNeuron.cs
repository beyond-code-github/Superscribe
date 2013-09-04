namespace Superscribe.Demo.Neural.NeuralNetwork
{
    using System;

    public class HiddenNeuron : Neuron
    {
        public HiddenNeuron(INeuralNode first, INeuralNode second)
            : base(first, second)
        {
        }

        protected override double Activation(double x)
        {
            if (x < -45.0) return 0.0;
            if (x > 45.0) return 1.0;
            return 1.0 / (1.0 + Math.Exp(-x));
        }
    }
}