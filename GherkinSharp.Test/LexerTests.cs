using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;

namespace GherkinSharp.Test
{
    public class LexerTests : Lexer.ITokenReceiver
    {
        private List<string> tokensReceived;
        private Lexer _lexer;

        public void EndOfFile()
        {
            tokensReceived.Add("EOF");
        }

        public void Given(string text)
        {
            tokensReceived.Add($"Given[{text}]");
        }

        public void When(string text)
        {
            tokensReceived.Add($"When[{text}]");
        }

        public void Then(string text)
        {
            tokensReceived.Add($"Then[{text}]");
        }

        [SetUp]
        public void SetUp()
        {
            _lexer = new Lexer(this);
            tokensReceived = new List<string>();
        }

        private void ThenExpectToHaveLexedAs(params string[] expectedTokens)
        {
            var expectedTokensAsList = expectedTokens.ToList();
            tokensReceived.Should().Equal(expectedTokensAsList);
        }

        private void WhenLexing(string gherkin)
        {
            _lexer.Lex(gherkin);
        }

        [Test]
        public void CanLexEmptyString()
        {
            WhenLexing("");
            ThenExpectToHaveLexedAs("EOF");
        }

        [Test]
        [TestCase(
            "Given a bunch of things",
            "Given[a bunch of things]", "EOF"
        )]
        [TestCase(
            "Given one",
            "Given[one]", "EOF"
        )]
        [TestCase(
            "When I press the button",
            "When[I press the button]", "EOF"
        )]
        [TestCase(
            "Then I see magic",
            "Then[I see magic]", "EOF"
        )]
        public void CanLexStepOnce(string gherkin, params string[] expectedTokens)
        {
            WhenLexing(gherkin);
            ThenExpectToHaveLexedAs(expectedTokens);
        }

        [Test]
        [TestCase(
            @"
Given a thing
Given this other thing",
            "Given[a thing]", "Given[this other thing]", "EOF"
        )]
        [TestCase(
            @"
Given a thing
Given this other thing
Given this final thing",
            "Given[a thing]", "Given[this other thing]", "Given[this final thing]", "EOF"
        )]
        [TestCase(@"
When playing tennis
When spinning",
            "When[playing tennis]", "When[spinning]", "EOF"
        )]
        [TestCase(@"
Then blah
Then woah",
            "Then[blah]", "Then[woah]", "EOF"
        )]
        public void CanLexStepsManyTimes(string gherkin, params string[] expectedTokens)
        {
            WhenLexing(gherkin);
            ThenExpectToHaveLexedAs(expectedTokens);
        }

        [Test]
        [TestCase(
            @"
Given a ball
When I hit it",
            "Given[a ball]", "When[I hit it]", "EOF"
        )]
        [TestCase(
            @"
When I smash it
Given a brick",
            "When[I smash it]", "Given[a brick]", "EOF"
        )]
        public void CanLexInTheCorrectOrder(string gherkin, params string[] expectedTokens)
        {
            WhenLexing(gherkin);
            ThenExpectToHaveLexedAs(expectedTokens);
        }

        [Test]
        public void OnlyConsumesTokenisesTheFirstOccurrenceOfTheToken()
        {
            WhenLexing(@"
Given Craig has Given me a present
When Steve has When ed me a when?
Then we Then a then"
            );
            ThenExpectToHaveLexedAs(
                "Given[Craig has Given me a present]",
                "When[Steve has When ed me a when?]",
                "Then[we Then a then]",
                "EOF"
            );
        }

        [Test]
        public void CanHandleWindowsNewLines()
        {
            WhenLexing("Given woah\r\nWhen a");
            ThenExpectToHaveLexedAs(
                "Given[woah]",
                "When[a]",
                "EOF"
            );
        }
    }
}