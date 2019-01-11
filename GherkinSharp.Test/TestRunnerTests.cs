using FluentAssertions;
using NUnit.Framework;

namespace GherkinSharp.Test
{
    public class TestRunnerTests
    {
        [Test]
        public void CanRunNoTests()
        {
            var testRunner = new TestRunner();
            var response = testRunner.Execute(new Tests());
            response.TestsRun.Should().Be(0);
        }

        [Test]
        public void CanRunOneTest()
        {
            var testRunner = new TestRunner();

            var spy = false;
            testRunner.DefineGivenStep("steve", () => { spy = true; });

            var tests = new Tests(new Given[]{ new Given("steve") });
          
            var response = testRunner.Execute(tests);

            spy.Should().BeTrue();
            response.TestsRun.Should().Be(1);
        }
        
        
        [Test]
        public void CanRunOneTest2()
        {
            var testRunner = new TestRunner();

            var spy = false;
            testRunner.DefineGivenStep("stevil", () => { spy = true; });

            var tests = new Tests(new Given[]{ new Given("stevil") });
          
            var response = testRunner.Execute(tests);

            spy.Should().BeTrue();
            response.TestsRun.Should().Be(1);
        }
        
        
        [Test]
        public void GivenOneStepDefinition_WhenRunningEmptyTests_ThenRunsNoTest()
        {
            var testRunner = new TestRunner();

            var spy = false;
            testRunner.DefineGivenStep("stevil", () => { spy = true; });

            var tests = new Tests(new Given[]{});
          
            var response = testRunner.Execute(tests);

            spy.Should().BeFalse();
            response.TestsRun.Should().Be(0);
        }
    }
}