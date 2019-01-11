using System;
using System.Text.RegularExpressions;

namespace GherkinSharp
{
    public class Lexer
    {
        private readonly ITokenReceiver _tokenReceiver;

        public interface ITokenReceiver
        {
            void EndOfFile();
            void Given(string text);
            void When(string text);
            void Then(string text);
        }

        public Lexer(ITokenReceiver tokenReceiver)
        {
            _tokenReceiver = tokenReceiver;
        }

        public void Lex(string gherkin)
        {
            foreach (var gherkinLine in GetGherkinLines(gherkin))
            {
                MatchAndCallback("^Given (.*)", _tokenReceiver.Given, gherkinLine);
                MatchAndCallback("^When (.*)", _tokenReceiver.When, gherkinLine);
                MatchAndCallback("^Then (.*)", _tokenReceiver.Then, gherkinLine);
            }

            _tokenReceiver.EndOfFile();
        }

        private static void MatchAndCallback(string pattern, Action<string> callback, string gherkinLine)
        {
            var m = Regex.Match(gherkinLine, pattern);
            
            while (m.Success)
            {
                var theText = m.Groups[1].Value;
                callback(theText);
                
                m = m.NextMatch();
            }
        }

        private static string[] GetGherkinLines(string gherkin)
        {
            return gherkin.Split(new[] { "\n", "\r\n" }, StringSplitOptions.None);
        }
    }
}