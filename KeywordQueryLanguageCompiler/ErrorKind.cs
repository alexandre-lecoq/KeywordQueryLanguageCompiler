namespace KeywordQueryLanguageCompiler
{
    /// <summary>
    /// Describes the kind of parsing error.
    /// </summary>
    public enum ErrorKind
    {
        UnknownError,

        InvalidEmptyExpression,

        MissingOperand,

        UnexpectedTokenType,

        StackHasMoreOrLessThanOneItem,

        NearKeywordOperandError,

        UnknownFormsOfType,

        UnmatchedDoubleQuote,

        MisplacedAsteriskInTerm,

        MissingLeftParenthesis,

        UnmatchedLeftParenthesis,

        IncorrectEmptyParentheses
    }
}
