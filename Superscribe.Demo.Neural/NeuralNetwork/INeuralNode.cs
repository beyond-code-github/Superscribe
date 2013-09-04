namespace Superscribe.Demo.Neural.NeuralNetwork
{
    public interface INeuralNode
    {
        double Output();

        void AddOutput(LinkWeight link);
    }
}