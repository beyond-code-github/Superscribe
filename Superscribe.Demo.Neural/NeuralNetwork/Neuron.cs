namespace Superscribe.Demo.Neural.NeuralNetwork
{
    using System.Collections.Generic;
    using System.Linq;

    public abstract class Neuron : NeuronBase, INeuralNode
    {
        public readonly List<LinkWeight> Inputs = new List<LinkWeight>();

        public double? ErrorCache { get; set; }

        public double? OutputCache { get; set; }

        protected Neuron(params INeuralNode[] parameters)
        {
            foreach (var param in parameters)
            {
                this.Inputs.Add(new LinkWeight(param, this));
            }
        }

        public void Train()
        {
            foreach (var input in this.Inputs)
            {
                var delta = (Network.LearnRate * this.Error() * input.From.Output());
                input.Weight = input.Weight + delta + (Network.Momentum * input.PreviousDelta);
                input.PreviousDelta = delta;
            }
        }

        public virtual double Error()
        {
            if (!this.ErrorCache.HasValue)
            {
                var output = this.Output();
                this.ErrorCache = output * (1 - output) * (this.Outputs.Sum(o => o.Weight * o.To.Error()));
            }

            return this.ErrorCache.Value;
        }

        public override double Output()
        {
            if (!this.OutputCache.HasValue)
            {
                var input = this.Inputs.Sum(o => o.Weight * o.From.Output());
                this.OutputCache = this.Activation(input);
            }

            return this.OutputCache.Value;
        }

        protected abstract double Activation(double x);
    }
}