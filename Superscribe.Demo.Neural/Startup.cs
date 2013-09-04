namespace Superscribe.Demo.Neural
{
    using Owin;

    using Superscribe.Demo.Neural.NeuralNetwork;
    using Superscribe.Models;

    using global::Owin;

    using RouteData = Superscribe.Models.RouteData;

    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ʃ.Route(root => root * this.ExplainPurpose);
            ʃ.Route((root, ʅ) =>
                root / "xor" / (ʃBool)"Param1" / (ʃBool)"Param2" / (
                    ʅ / "YouSaid" / (ʃBool)"Answer" / (
                          ʅ / "AndThatWasCorrect" * this.LearnThatItWasCorrect
                        | ʅ / "ButThatWasWrong" * this.LearnThatItWasWrong)
                  | ʅ - this.ComputeResult ^ (
                          ʅ - (o => o >= 0.5) ^ this.RespondTrue
                        | ʅ - (o => o < 0.5) ^ this.RespondFalse)));

            app.UseSuperscribe();
        }

        private double ComputeResult(RouteData o, string segment)
        {
            var param1 = (bool)o.Parameters["Param1"];
            var param2 = (bool)o.Parameters["Param2"];

            return Network.GetOutput(param1, param2);
        }

        private void ExplainPurpose(RouteData o)
        {
            o.Response = "<p>I'm trying to learn XOR, and I need your help!<p>" +
                "<p>Try me out...</p>" +
                "<ul><li><a href='/xor/false/false'>What is false XOR false?</a></li>" +
                "<li><a href='/xor/false/true'>What is false XOR true?</a></li>" +
                "<li><a href='/xor/true/false'>What is true XOR false?</a></li>" +
                "<li><a href='/xor/true/true'>What is true XOR true?</a></li></ul>";
        }

        private void RespondTrue(RouteData o)
        {
            this.Respond(o, true);
        }

        private void RespondFalse(RouteData o)
        {
            this.Respond(o, false);
        }

        private void Respond(RouteData o, bool value)
        {
            var param1 = (bool)o.Parameters["Param1"];
            var param2 = (bool)o.Parameters["Param2"];

            o.Response =
                string.Format(
                    "<p>Based on my training, I think {0} XOR {1} is <strong>{2}</strong>.<p>" + "<p>Was I right?</p>"
                    + "<ul><li><form method='POST' action='/xor/{0}/{1}/YouSaid/{2}/AndThatWasCorrect'><input type='submit' value='Yes!' /></form></li>"
                    + "<li><form method='POST' action='/xor/{0}/{1}/YouSaid/{2}/ButThatWasWrong'><input type='submit' value='No' /></form></li></ul>",
                    param1.ToString().ToLower(),
                    param2.ToString().ToLower(),
                    value.ToString().ToLower());
        }

        private void LearnThatItWasCorrect(RouteData o)
        {
            var param1 = (bool)o.Parameters["Param1"];
            var param2 = (bool)o.Parameters["Param2"];
            var answer = (bool)o.Parameters["Answer"];

            Network.Train(param1, param2, answer);

            o.Response = "<p>Great, thanks for reinforcing that for me!</p><a href='/'>Try again</a>";
        }

        private void LearnThatItWasWrong(RouteData o)
        {
            var param1 = (bool)o.Parameters["Param1"];
            var param2 = (bool)o.Parameters["Param2"];
            var answer = (bool)o.Parameters["Answer"];

            Network.Train(param1, param2, !answer);

            o.Response = string.Format("<p>I'll try to remember that you said {0} XOR {1} is <strong>{2}</strong> and factor that in next time.</p><a href='/'>Try again</a>"
                , param1, param2, !answer);
        }
    }
}