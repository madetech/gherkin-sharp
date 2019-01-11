using GherkinSharp.Test;

namespace GherkinSharp
{
    public class Tests
    {
        private readonly Given[] _givenSteps;
        public Tests(Given[] givens = null)
        {
            _givenSteps = givens;
        }

        public Given[] Givens => _givenSteps;
    }
}