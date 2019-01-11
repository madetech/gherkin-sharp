using FluentAssertions;
using NUnit.Framework;

namespace GherkinSharp.Test
{
    public class GherkinSharpTests
    {

        [Test]
        public void CanExecuteEmptyTestFile()
        {
            var testBuilder = new TestBuilder();
           
            var parser = new Parser(testBuilder);
            var lexer = new Lexer(parser);
            lexer.Lex("");

            var testRunner = new TestRunner();
            
            var response = testRunner.Execute(testBuilder.Build());

            response.TestsRun.Should().Be(0);
        }
        
        [Test]
        public void CanExecuteSingleStepTestFile()
        {
            var testBuilder = new TestBuilder();
           
            var parser = new Parser(testBuilder);
            var lexer = new Lexer(parser);
            lexer.Lex("Given Craig has a pie");

            var testRunner = new TestRunner();
            var called = false;
            testRunner.DefineGivenStep("Craig has a pie", () => { called = true; });
            
            var response = testRunner.Execute(testBuilder.Build());

            response.TestsRun.Should().Be(1);
            called.Should().BeTrue();
        }
    }
}