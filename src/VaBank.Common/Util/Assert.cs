using System;
using System.Diagnostics;

namespace VaBank.Common.Util
{
    /// <summary>
    /// Provides access to assertion functions.
    /// </summary>
    [DebuggerStepThrough]
    public static class Assert
    {

        #region Methods
        /// <summary>
        /// Asserts that a condition is fase.
        /// </summary>
        /// <param name="paramName">The parameter name.</param>
        /// <param name="condition">The condition result.</param>
        /// <exception cref="System.ArgumentException">If <code>condition</code> is true.</exception>
        public static void IsFalse(string paramName, bool condition)
        {
            if (condition)
                throw new ArgumentException(paramName);
        }

        /// <summary>
        /// Asserts that a condition is true.
        /// </summary>
        /// <param name="paramName">The parameter name.</param>
        /// <param name="condition">The condition result.</param>
        /// <exception cref="System.ArgumentException">If <code>condition</code> is false.</exception>
        public static void IsTrue(string paramName, bool condition)
        {
            if (!condition)
                throw new ArgumentException(paramName);
        }

        /// <summary>
        /// Asserts that <code>value</code> is not null.
        /// </summary>
        /// <param name="paramName">The name of the parameter to assert.</param>
        /// <param name="value">The value to test.</param>
        /// <exception cref="System.ArgumentNullException">If <code>value</code> is null.</exception>
        public static void NotNull(string paramName, object value)
        {
            if (value == null)
                throw new ArgumentNullException(paramName);
        }

        /// <summary>
        /// Asserts that <code>value</code> is not null.
        /// </summary>
        /// <param name="value">The value to test.</param>
        /// <exception cref="System.ArgumentNullException">If <code>value</code> is null.</exception>
        public static void NotNull(object value)
        {
            if (value == null)
                throw new ArgumentNullException();
        }
        
        /// <summary>
        /// Asserts that <code>value</code> is not empty.
        /// </summary>
        /// <param name="paramName">The parameter name.</param>
        /// <param name="value">The value to test.</param>
        public static void NotEmpty(string paramName, string value)
        {
            NotEmpty(paramName, value, false);
        }

        /// <summary>
        /// Asserts that <code>value</code> is not empty.
        /// </summary>
        /// <param name="value">The value to test.</param>
        public static void NotEmpty(string value)
        {
            NotEmpty(null, value, false);
        }

        /// <summary>
        /// Asserts that <code>value</code> is not empty.
        /// </summary>
        /// <param name="value">The value to test.</param>
        /// <param name="trim">Whether to trim the string before checking it.</param>
        public static void NotEmpty(string value, bool trim)
        {
            NotEmpty(null, value, trim);
        }

        /// <summary>
        /// Asserts that <code>value</code> is not empty.
        /// </summary>
        /// <param name="paramName">The parameter name.</param>
        /// <param name="value">The value to test.</param>
        /// <param name="trim">Whether to trim the string before checking it.</param>
        public static void NotEmpty(string paramName, string value, bool trim)
        {
            if (value != null)
            {
                if (trim)
                    NotEmpty(paramName, value.Trim(), false);
                else if (value.Length == 0)
                    throw new ArgumentException(paramName);
            }
        }

        /// <summary>
        /// Asserts that <code>values</code> is not empty.
        /// </summary>
        /// <param name="values">The values to test.</param>
        public static void NotEmpty<T>(T[] values)
        {
            NotEmpty(values);
        }

        /// <summary>
        /// Asserts that <code>values</code> is not empty.
        /// </summary>
        /// <param name="paramName">The parameter name.</param>
        /// <param name="values">The values to test.</param>
        public static void NotEmpty<T>(string paramName, T[] values)
        {
            if ((values != null) && (values.Length == 0))
                throw new ArgumentException(paramName);
        }
        #endregion

    }
}