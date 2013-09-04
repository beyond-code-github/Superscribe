namespace Superscribe.Demo.Neural.NeuralNetwork
{
    public static class Network
    {
        public static InputNeuron First, Second;

        public static HiddenNeuron A, B;

        public static OutputNeuron Output;

        static Network()
        {
            Reset();
        }

        public static double LearnRate = 3;

        public static double Momentum = 0;

        public static void Reset()
        {
            First = new InputNeuron();
            Second = new InputNeuron();

            A = new HiddenNeuron(First, Second);
            B = new HiddenNeuron(First, Second);

            Output = new OutputNeuron(A, B);
        }

        public static double GetOutput(bool first, bool second)
        {
            First.Value = first;
            Second.Value = second;

            A.OutputCache = null;
            B.OutputCache = null;
            Output.OutputCache = null;

            return Output.Output();
        }

        public static void Train(bool first, bool second, bool correctAnswer)
        {
            First.Value = first;
            Second.Value = second;
            Output.Target = correctAnswer ? 1 : 0;

            Output.ErrorCache = null;
            A.ErrorCache = null;
            B.ErrorCache = null;

            Output.Error();
            A.Error();
            B.Error();

            Output.Train();
            A.Train();
            B.Train();
        }
    }
}
