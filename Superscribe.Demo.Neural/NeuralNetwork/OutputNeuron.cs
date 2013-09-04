namespace Superscribe.Demo.Neural.NeuralNetwork
{
    public class OutputNeuron : Neuron
    {
        public double Target { get; set; }

        public OutputNeuron(INeuralNode first, INeuralNode second)
            : base(first, second)
        {
        }

        public override double Error()
        {
            if (!this.ErrorCache.HasValue)
            {
                var output = this.Output();
                this.ErrorCache = (this.Target - output);
            }

            return this.ErrorCache.Value;
        }

        protected override double Activation(double x)
        {
            return x < 0.5 ? 0.0 : 1.0;
        }
    }
}