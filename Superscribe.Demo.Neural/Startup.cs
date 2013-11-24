namespace Superscribe.Demo.Neural
{
    using Newtonsoft.Json;

    using global::Owin;

    using Superscribe.Demo.Neural.NeuralNetwork;
    using Superscribe.Owin;
    using Superscribe.Owin.Extensions;

    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var config = new SuperscribeOwinConfig();
            config.MediaTypeHandlers.Add(
                "application/json",
                new MediaTypeHandler { Write = (env, o) => env.WriteResponse(JsonConvert.SerializeObject(o)) });
            
            app.UseSuperscribe(config);
        }

        private double ComputeResult(dynamic o, string segment)
        {
            var param1 = (bool)o.Parameters["Param1"];
            var param2 = (bool)o.Parameters["Param2"];

            return Network.GetOutput(param1, param2);
        }

        private object ExplainPurpose(dynamic o)
        {
            return "<p>I'm trying to learn XOR, and I need your help!<p>" + "<p>Try me out...</p>"
                         + "<ul><li><a href='/xor/false/false'>What is false XOR false?</a></li>"
                         + "<li><a href='/xor/false/true'>What is false XOR true?</a></li>"
                         + "<li><a href='/xor/true/false'>What is true XOR false?</a></li>"
                         + "<li><a href='/xor/true/true'>What is true XOR true?</a></li></ul>";
        }

        private object RespondTrue(dynamic o)
        {
            return this.Respond(o, true);
        }

        private object RespondFalse(dynamic o)
        {
            return this.Respond(o, false);
        }

        private object Respond(dynamic o, bool value)
        {
            var param1 = (bool)o.Parameters["Param1"];
            var param2 = (bool)o.Parameters["Param2"];

            return
                string.Format(
                    "<p>Based on my training, I think {0} XOR {1} is <strong>{2}</strong>.<p>" + "<p>Was I right?</p>"
                    + "<ul><li><form method='POST' action='/xor/{0}/{1}/YouSaid/{2}/AndThatWasCorrect'><input type='submit' value='Yes!' /></form></li>"
                    + "<li><form method='POST' action='/xor/{0}/{1}/YouSaid/{2}/ButThatWasWrong'><input type='submit' value='No' /></form></li></ul>",
                    param1.ToString().ToLower(),
                    param2.ToString().ToLower(),
                    value.ToString().ToLower());
        }

        private object LearnThatItWasCorrect(dynamic o)
        {
            var param1 = (bool)o.Parameters["Param1"];
            var param2 = (bool)o.Parameters["Param2"];
            var answer = (bool)o.Parameters["Answer"];

            Network.Train(param1, param2, answer);

            return "<p>Great, thanks for reinforcing that for me!</p><a href='/'>Try again</a>";
        }

        private object LearnThatItWasWrong(dynamic o)
        {
            var param1 = (bool)o.Parameters["Param1"];
            var param2 = (bool)o.Parameters["Param2"];
            var answer = (bool)o.Parameters["Answer"];

            Network.Train(param1, param2, !answer);

            return string.Format(
                    "<p>I'll try to remember that you said {0} XOR {1} is <strong>{2}</strong> and factor that in next time.</p><a href='/'>Try again</a>",
                    param1,
                    param2,
                    !answer);
        }
    }
}