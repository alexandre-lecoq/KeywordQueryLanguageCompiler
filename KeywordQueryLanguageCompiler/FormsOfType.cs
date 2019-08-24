namespace KeywordQueryLanguageCompiler
{
    using System;

    /// <summary>
    /// Type of FORMSOF predicate to use.
    /// </summary>
    [Flags]
    public enum FormsOfType
    {
        /// <summary>
        /// No fuzzy matching.
        /// </summary>
        None = 0x0,

        /// <summary>
        /// Specifies that a stemmer is used.
        /// </summary>
        Inflectional = 0x1,

        /// <summary>
        /// Specifies that a thesaurus is used.
        /// </summary>
        Thesaurus = 0x2,

        /// <summary>
        /// Specifies that both a stemmer and thesaurus are used.
        /// </summary>
        Both = 0x3
    }
}
