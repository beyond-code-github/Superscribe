namespace Superscribe.Demo.Neural.NeuralNetwork
{
    using System.Collections.Generic;

    public abstract class NeuronBase
    {
        public readonly List<LinkWeight> Outputs = new List<LinkWeight>();

        public void AddOutput(LinkWeight neuron)
        {
            this.Outputs.Add(neuron);
        }

        public abstract double Output();
    }
}