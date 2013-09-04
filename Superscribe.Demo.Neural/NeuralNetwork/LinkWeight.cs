namespace Superscribe.Demo.Neural.NeuralNetwork
{
    public class LinkWeight
    {
        public LinkWeight(INeuralNode from, Neuron to)
        {
            this.Weight = RandomGenerator.Generate();
            this.From = @from;
            this.To = to;

            this.From.AddOutput(this);

            this.PreviousDelta = 0;
        }

        public double Weight { get; set; }

        public INeuralNode From { get; set; }

        public Neuron To { get; set; }

        public double PreviousDelta { get; set; }
    }
}