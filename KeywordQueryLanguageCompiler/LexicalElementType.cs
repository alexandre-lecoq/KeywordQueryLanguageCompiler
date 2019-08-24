namespace KeywordQueryLanguageCompiler
{
    public enum LexicalElementType
    {
        /// <summary>
        /// A search term.
        /// </summary>
        Term,

        /// <summary>
        /// The AND element.
        /// </summary>
        And,

        /// <summary>
        /// The NOT element.
        /// </summary>
        Not,

        /// <summary>
        /// The OR element.
        /// </summary>
        Or,

        /// <summary>
        /// The NEAR element.
        /// </summary>
        Near,

        /// <summary>
        /// The ( element.
        /// </summary>
        LeftParenthesis,

        /// <summary>
        /// The ) element.
        /// </summary>
        RightParenthesis,
    }
}
