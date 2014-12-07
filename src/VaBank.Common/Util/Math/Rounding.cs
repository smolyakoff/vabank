using Newtonsoft.Json;
using VaBank.Common.Validation;

namespace VaBank.Common.Util.Math
{
    public struct Rounding
    {
        public Rounding(int floatPrecision, IntegerRounding? integerRounding) 
            : this()
        {
            Argument.Satisfies(floatPrecision, x => x >= 0 && x <= 28, "floatPrecision");
            FloatPrecision = floatPrecision;
            IntegerRounding = integerRounding;
        }

        public Rounding(int floatPrecision = 2)
            :this(floatPrecision, null)
        {            
        }

        public Rounding(IntegerRounding integerRounding)
            : this(0, integerRounding)
        {
        }

        [JsonProperty]
        public int FloatPrecision { get; private set; }

        [JsonProperty]
        public IntegerRounding? IntegerRounding { get; private set; }
    }
}
