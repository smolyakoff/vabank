using Newtonsoft.Json;
using VaBank.Common.Validation;

namespace VaBank.Common.Util.Math
{
    public struct IntegerRounding
    {
        public IntegerRounding(IntegerRoundingMode mode, int minimalAmount) 
            : this()
        {
            Argument.Satisfies(minimalAmount, x => x >= 1, "precision");
            Mode = mode;
            MinimalAmount = minimalAmount;
        }

        [JsonProperty]
        public IntegerRoundingMode Mode { get; private set; }

        [JsonProperty]
        public int MinimalAmount { get; private set; }
    }
}
