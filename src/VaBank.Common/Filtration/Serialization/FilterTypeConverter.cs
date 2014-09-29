using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using VaBank.Common.Data.Filtering;

namespace VaBank.Common.Filtration.Serialization
{
    //Useful for parsing from query string for GET requests
    public class FilterTypeConverter : TypeConverter
    {
        private static readonly Dictionary<string, FilterOperator> OperatorMapping 
            = new Dictionary<string, FilterOperator>()
        {
            { "eq", FilterOperator.Equal },
            { "!eq", FilterOperator.NotEqual },
            { "gt", FilterOperator.GreaterThan },
            { "gte", FilterOperator.GreaterThanOrEqual },
            { "lt", FilterOperator.LessThan },
            { "lte", FilterOperator.LessThanOrEqual },
            { "in", FilterOperator.In },
            { "!in", FilterOperator.NotIn },
            { "contains", FilterOperator.Contains},
            { "!contains", FilterOperator.NotContains },
            { "startswith", FilterOperator.StartsWith },
            { "!startswith", FilterOperator.NotStartsWith },
            { "endswith", FilterOperator.EndsWith },
            { "!endswith", FilterOperator.NotEndsWith },
        };

        private static readonly Dictionary<FilterOperator, string> ReverseOperatorMapping =
            OperatorMapping.ToDictionary(x => x.Value, x => x.Key);

        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            return sourceType == typeof (string) || base.CanConvertFrom(context, sourceType);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            var stringValue = value as string;
            return string.IsNullOrEmpty(stringValue) 
                ? base.ConvertFrom(context, culture, value) 
                : Parse(stringValue);
        }

        private static Filter Parse(string filterQuery)
        {
            //TODO: exception handling
            return IsCombiner(filterQuery) 
                ? (Filter) ParseCombiner(filterQuery) 
                : ParseExpression(filterQuery);
        }

        private static bool IsCombiner(string filterQuery)
        {
            var operators = Enum.GetNames(typeof (FilterLogic))
                .Select(x => x.ToLower())
                .ToList();
            var regex = new Regex(string.Join("|", operators));
            return regex.IsMatch(filterQuery);
        }

        private static CombinerFilter ParseCombiner(string filterQuery)
        {
            var operators = Enum.GetNames(typeof(FilterLogic))
                .Select(x => x.ToLower())
                .ToList();
            var combiner = new CombinerFilter();
            var childrenQueries = GetTopLevelQueries(filterQuery);
            foreach (var childrenFilterQuery in childrenQueries)
            {
                combiner.Filters.Add(Parse(childrenFilterQuery));
            }
            return combiner;
        }

        private static ExpressionFilter ParseExpression(string filterQuery)
        {
            var firstSpaceIndex = filterQuery.IndexOf(' ');
            var secondSpaceIndex = filterQuery.IndexOf(' ', firstSpaceIndex + 1);
            var propertyName = filterQuery.Substring(0, firstSpaceIndex + 1).Trim();
            var @operator = filterQuery.Substring(firstSpaceIndex + 1, secondSpaceIndex - firstSpaceIndex).Trim();
            var value = filterQuery.Substring(secondSpaceIndex + 1);
            var expressionFilter = new ExpressionFilter
            {
                Property = propertyName,
                Operator = MatchOperatorEnum(@operator),
                Value = JsonConvert.DeserializeObject(value)
            };
            return expressionFilter;
        }

        private static FilterOperator MatchOperatorEnum(string @operator)
        {
            if (!OperatorMapping.ContainsKey(@operator))
            {
                var message = string.Format("Operator [{0}] is not supported.");
                throw new NotSupportedException(message);
            }
            return OperatorMapping[@operator];
        }

        private static string MatchOperatorString(FilterOperator filterOperator)
        {
            if (!ReverseOperatorMapping.ContainsKey(filterOperator))
            {
                var message = string.Format("Operator [{0}] is not supported.");
                throw new NotSupportedException(message);
            }
            return ReverseOperatorMapping[filterOperator];
        }

        private static IEnumerable<string> GetTopLevelQueries(string combinerString)
        {
            //note: looks like simple finite automata
            combinerString = combinerString.Trim();
            var stack = new Stack<StringBuilder>();

            var builder = new StringBuilder();
            const char LeftParenthesis = '(';
            const char RightParenthesis = ')';
            var builders = new Dictionary<int, List<StringBuilder>>();
            const char Quote = '"';
            var afterRightParenthesis = false;
            var inQuote = false;
            for (var i = 0; i < combinerString.Length; i++)
            {
                var ch = combinerString[i];
                if (ch == LeftParenthesis && !inQuote)
                {
                    afterRightParenthesis = false;
                    if (stack.Count < 1)
                    {
                        builder = new StringBuilder();
                        stack.Push(builder);
                        if (builders.ContainsKey(stack.Count))
                        {
                            builders[stack.Count].Add(builder);
                        }
                        else
                        {
                            builders[stack.Count] = new List<StringBuilder> { builder };
                        }
                    }
                    
                }
                else if (ch == RightParenthesis && !inQuote)
                {
                    if (stack.Count == 0)
                    {
                        throw new InvalidOperationException("Mismatched parenthesis");
                    }
                    afterRightParenthesis = true;
                    builder = stack.Pop();
                } 
                else if (ch == Quote && i > 0 && combinerString[i - 1] != '\\')
                {
                    inQuote = !inQuote;
                }
                if (ch == RightParenthesis || !afterRightParenthesis)
                {
                    builder.Append(ch);
                }
            }
            Debugger.Break();
            return Enumerable.Empty<string>();
        } 

    }
}
