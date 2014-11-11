using VaBank.Common.Validation;

namespace VaBank.Core.Processing
{
    public struct MoneyRounding
    {
        private readonly IntegerRounding? _integerRounding;

        private readonly int _floatPrecision;

        public MoneyRounding(int floatPrecision, IntegerRounding? integerRounding)
        {
            Argument.Satisfies(floatPrecision, x => x >= 0 && x <= 28, "floatPrecision");

            _floatPrecision = floatPrecision;
            _integerRounding = integerRounding;
        }

        public MoneyRounding(int floatPrecision = 2)
            :this(floatPrecision, null)
        {            
        }

        public MoneyRounding(IntegerRounding integerRounding)
            : this(0, integerRounding)
        {
        }

        public int FloatPrecision
        {
            get { return _floatPrecision; }
        }

        public IntegerRounding? IntegerRounding
        {
            get { return _integerRounding; }
        }
    }
}
