using VaBank.Common.Validation;

namespace VaBank.Common.Util.Math
{
    public struct Rounding
    {
        private readonly IntegerRounding? _integerRounding;

        private readonly int _floatPrecision;

        public Rounding(int floatPrecision, IntegerRounding? integerRounding)
        {
            Argument.Satisfies(floatPrecision, x => x >= 0 && x <= 28, "floatPrecision");

            _floatPrecision = floatPrecision;
            _integerRounding = integerRounding;
        }

        public Rounding(int floatPrecision = 2)
            :this(floatPrecision, null)
        {            
        }

        public Rounding(IntegerRounding integerRounding)
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
