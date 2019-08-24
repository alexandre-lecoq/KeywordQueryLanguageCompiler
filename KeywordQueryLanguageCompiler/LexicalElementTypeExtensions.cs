namespace KeywordQueryLanguageCompiler
{
    using System;

    /// <summary>
    /// A class containing LexicalElementType's extension methods.
    /// </summary>
    public static class LexicalElementTypeExtensions
    {
        /// <summary>
        /// Get the lexical element type's associativity.
        /// </summary>
        /// <param name="type">The lexical element type.</param>
        /// <returns>The type's associativity.</returns>
        public static OperatorAssociativity GetAssociativity(this LexicalElementType type)
        {
            switch (type)
            {
                case LexicalElementType.Term:
                case LexicalElementType.And:
                case LexicalElementType.Not:
                case LexicalElementType.Or:
                case LexicalElementType.Near:
                case LexicalElementType.LeftParenthesis:
                case LexicalElementType.RightParenthesis:
                    return OperatorAssociativity.Left;

                default:
                    throw new NotImplementedException("Unknown lexical element type.");
            }
        }

        /// <summary>
        /// Gets the al element type's precedence.
        /// </summary>
        /// <param name="type">The lexical element type.</param>
        /// <returns>The type's precedence.</returns>
        /// <remarks>
        /// The given precedence are of arbitrary values.
        /// The important point is what types have the same precedences,
        /// and what types have lower or higher precedence than others.
        /// </remarks>
        public static int GetPrecedence(this LexicalElementType type)
        {
            switch (type)
            {
                case LexicalElementType.Term:
                    return 0;

                case LexicalElementType.Or:
                    return 2;

                case LexicalElementType.And:
                case LexicalElementType.Not:
                    return 3;

                case LexicalElementType.Near:
                    return 5;

                case LexicalElementType.LeftParenthesis:
                case LexicalElementType.RightParenthesis:
                    return 13;

                default:
                    throw new NotImplementedException("Unknown lexical element type.");
            }
        }

        /// <summary>
        /// Indicates whether the lexical element type is an operator.
        /// </summary>
        /// <param name="type">The lexical element type.</param>
        /// <returns>true if the type is an operator; false otherwise.</returns>
        public static bool IsOperator(this LexicalElementType type)
        {
            switch (type)
            {
                case LexicalElementType.And:
                case LexicalElementType.Not:
                case LexicalElementType.Or:
                case LexicalElementType.Near:
                    return true;

                case LexicalElementType.Term:
                case LexicalElementType.LeftParenthesis:
                case LexicalElementType.RightParenthesis:
                    return false;

                default:
                    throw new NotImplementedException("Unknown lexical element type.");
            }
        }

        /// <summary>
        /// Gets the operator's operand count.
        /// </summary>
        /// <param name="type">The lexical element type.</param>
        /// <returns>The operand count needed by the operator.</returns>
        public static int GetOperandCount(this LexicalElementType type)
        {
            switch (type)
            {
                case LexicalElementType.And:
                case LexicalElementType.Not:
                case LexicalElementType.Or:
                case LexicalElementType.Near:
                    return 2;

                case LexicalElementType.Term:
                case LexicalElementType.LeftParenthesis:
                case LexicalElementType.RightParenthesis:
                    throw new NotSupportedException($"The lexical element type {type} is not an operator type.");

                default:
                    throw new NotImplementedException("Unknown lexical element type.");
            }
        }
    }
}
