using VaBank.Common.Validation;

namespace VaBank.Common.Util.Math
{
    public struct IntegerRounding
    {
        private readonly IntegerRoundingMode _mode;

        private readonly int _minimalAmount;

        public IntegerRounding(IntegerRoundingMode mode, int minimalAmount)
        {
            Argument.Satisfies(minimalAmount, x => x >= 1, "precision");
            _mode = mode;
            _minimalAmount = minimalAmount;
        }

        public IntegerRoundingMode Mode
        {
            get { return _mode; }
        }

        public int MinimalAmount
        {
            get { return _minimalAmount; }
        }
    }
}
