using System;

namespace VaBank.Core.Processing
{
    internal static class MoneyMath
    {
        public static decimal Round(decimal amount, MoneyRounding rounding)
        {
            var floatRounded = Math.Round(amount, rounding.FloatPrecision, MidpointRounding.AwayFromZero);
            return rounding.IntegerRounding != null 
                ? IntegerRound(amount, rounding.IntegerRounding.Value) 
                : floatRounded;
        }

        public static decimal IntegerRound(decimal amount, IntegerRounding rounding)
        {
            var rounded = Math.Round(amount);
            if (rounding.MinimalAmount == 1)
            {
                return rounded;
            }
            Func<decimal, decimal> roundingMethod;
            switch (rounding.Mode)
            {
                case IntegerRoundingMode.Ceiling:
                    roundingMethod = Math.Ceiling;
                    break;
                case IntegerRoundingMode.Floor:
                    roundingMethod = Math.Floor;
                    break;
                case IntegerRoundingMode.Round:
                    roundingMethod = Math.Round;
                    break;
                default:
                    roundingMethod = Math.Round;
                    break;
            }
            return roundingMethod(rounded / rounding.MinimalAmount) * rounding.MinimalAmount;
        }
    }
}
