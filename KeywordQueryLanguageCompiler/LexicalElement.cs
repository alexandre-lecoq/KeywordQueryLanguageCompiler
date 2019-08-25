namespace KeywordQueryLanguageCompiler
{
    using System;

    /// <summary>
    /// Represents a lexical element.
    /// </summary>
    [Serializable]
    public sealed class LexicalElement : IEquatable<LexicalElement>
    {
        /// <summary>
        /// The lexical element type.
        /// </summary>
        public LexicalElementType Type { get; }

        /// <summary>
        /// The lexical element text.
        /// </summary>
        public string Text { get; }

        /// <summary>
        /// The lexical element start position.
        /// </summary>
        public int Position { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="LexicalElement"/> class.
        /// </summary>
        /// <param name="type">The lexical element type.</param>
        /// <param name="text">The lexical element text.</param>
        /// <param name="position">The lexical element start position.</param>
        public LexicalElement(LexicalElementType type, string text, int position)
        {
            Type = type;
            Text = text;
            Position = position;
        }

        /// <summary>
        /// Compares two lexical elements.
        /// </summary>
        /// <param name="other">The second lexical element.</param>
        /// <returns>true if they are identical; false otherwise.</returns>
        public bool Equals(LexicalElement other)
        {
            if (other == null)
            {
                return false;
            }

            if (other.Text != Text)
            {
                return false;
            }

            if (other.Type != Type)
            {
                return false;
            }

            if (other.Position != Position)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Compares two lexical elements.
        /// </summary>
        /// <param name="obj">The second lexical element.</param>
        /// <returns>true if they are identical; false otherwise.</returns>
        public override bool Equals(object obj)
        {
            var element = obj as LexicalElement;

            return Equals(element);
        }

        /// <summary>
        /// Compute the lexical element's hash code.
        /// </summary>
        /// <returns>The hash code.</returns>
        public override int GetHashCode()
        {
            var hashCode = Type.GetHashCode() ^ Position.GetHashCode();

            return Text == null ? hashCode : hashCode ^ Text.GetHashCode();
        }

        /// <summary>
        /// Computes a string representing the lexical element.
        /// </summary>
        /// <returns>The string representing the lexical element.</returns>
        public override string ToString()
        {
            return Text == null ? $"{Type}" : $"{Type} = \"{Text}\"";
        }
    }
}
