namespace KeywordQueryLanguageCompiler
{
    using System;
    using System.Runtime.Serialization;
    using System.Security.Permissions;

    /// <summary>
    /// Exception thrown when parsing.
    /// </summary>
    [Serializable]
    public class ParsingException : Exception
    {
        /// <summary>
        /// The lexical element where the problem was detected. 
        /// </summary>
        public LexicalElement LexicalElement { get; }

        public ErrorKind ErrorKind { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ParsingException"/> class.
        /// </summary>
        public ParsingException()
        {
            ErrorKind = ErrorKind.UnknownError;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ParsingException"/> class.
        /// </summary>
        /// <param name="message">The message that explains the reason for this exception.</param>
        public ParsingException(string message)
            : base(message)
        {
            ErrorKind = ErrorKind.UnknownError;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ParsingException"/> class.
        /// </summary>
        /// <param name="message">The message that explains the reason for this exception.</param>
        /// <param name="errorKind">The error kind.</param>
        public ParsingException(string message, ErrorKind errorKind)
            : base(message)
        {
            ErrorKind = errorKind;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ParsingException"/> class.
        /// </summary>
        /// <param name="message">The message that explains the reason for this exception.</param>
        /// <param name="innerException">The exception that is the cause of the current exception, or a null reference if no inner exception is specified.</param>
        public ParsingException(string message, Exception innerException)
            : base(message, innerException)
        {
            ErrorKind = ErrorKind.UnknownError;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ParsingException"/> class.
        /// </summary>
        /// <param name="lexicalElement">The lexical element where the problem was detected.</param>
        public ParsingException(LexicalElement lexicalElement)
        {
            LexicalElement = lexicalElement;
            ErrorKind = ErrorKind.UnknownError;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ParsingException"/> class.
        /// </summary>
        /// <param name="message">The message that explains the reason for this exception.</param>
        /// <param name="lexicalElement">The lexical element where the problem was detected.</param>
        public ParsingException(string message, LexicalElement lexicalElement)
            : base(message)
        {
            LexicalElement = lexicalElement;
            ErrorKind = ErrorKind.UnknownError;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ParsingException"/> class.
        /// </summary>
        /// <param name="message">The message that explains the reason for this exception.</param>
        /// <param name="lexicalElement">The lexical element where the problem was detected.</param>
        /// <param name="errorKind">The error kind.</param>
        public ParsingException(string message, LexicalElement lexicalElement, ErrorKind errorKind)
            : base(message)
        {
            LexicalElement = lexicalElement;
            ErrorKind = errorKind;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ParsingException"/> class.
        /// </summary>
        /// <param name="message">The message that explains the reason for this exception.</param>
        /// <param name="innerException">The exception that is the cause of the current exception, or a null reference if no inner exception is specified.</param>
        /// <param name="lexicalElement">The lexical element where the problem was detected.</param>
        public ParsingException(string message, Exception innerException, LexicalElement lexicalElement)
            : base(message, innerException)
        {
            LexicalElement = lexicalElement;
            ErrorKind = ErrorKind.UnknownError;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ParsingException"/> class with serialized data.
        /// </summary>
        /// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo"/> that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The <see cref="T:System.Runtime.Serialization.StreamingContext"/> that contains contextual information about the source or destination.</param>
        /// <exception cref="T:System.ArgumentNullException">The <paramref name="info"/> parameter is null.</exception>
        /// <exception cref="T:System.Runtime.Serialization.SerializationException">The class name is null or <see cref="P:System.Exception.HResult"/> is zero (0).</exception>
        [SecurityPermissionAttribute(SecurityAction.Demand, SerializationFormatter = true)]
        public ParsingException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            LexicalElement = (LexicalElement)info.GetValue("LexicalElement", typeof(LexicalElement));
            ErrorKind = (ErrorKind)info.GetValue("ErrorKind", typeof(ErrorKind));
        }

        [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);

            info.AddValue("LexicalElement", LexicalElement);
            info.AddValue("ErrorKind", ErrorKind);
        }
    }
}
