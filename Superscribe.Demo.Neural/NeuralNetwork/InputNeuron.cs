namespace Superscribe.Demo.Neural.NeuralNetwork
{
    public class InputNeuron : NeuronBase, INeuralNode
    {
        public bool Value { get; set; }

        public override double Output()
        {
            return this.Value ? 1 : 0;
        }
    }
}