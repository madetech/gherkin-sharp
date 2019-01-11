using System;
using System.Collections.Generic;
using System.Linq;

namespace GherkinSharp
{
    public class TestRunner
    {
        private readonly Dictionary<string, Action> _givenSteps;

        public TestRunner()
        {
            _givenSteps = new Dictionary<string, Action>();
        }

        public TestResults Execute(Tests tests)
        {
            var numberOfSteps = tests.Givens?.Length ?? 0;
            
            if(numberOfSteps > 0)
            {
                _givenSteps.FirstOrDefault().Value?.Invoke();
            }
            
            return new TestResults { TestsRun = numberOfSteps};
        }

        public void DefineGivenStep(string craigHasAPie, Action action)
        {
            _givenSteps.Add(craigHasAPie, action);
        }
    }
}