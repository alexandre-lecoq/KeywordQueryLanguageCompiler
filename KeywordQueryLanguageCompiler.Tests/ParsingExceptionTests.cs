namespace KeywordQueryLanguageCompiler.Tests
{
    using System;
    using System.IO;
    using System.Runtime.Serialization;
    using System.Runtime.Serialization.Formatters.Binary;

    using Xunit;

    public class ParsingExceptionTests
    {
        [Fact]
        public void BasicConstructor()
        {
            var ex = new ParsingException();
            Assert.Null(ex.LexicalElement);
        }

        [Fact]
        public void MessageConstructor()
        {
            var ex = new ParsingException("test");
            Assert.Null(ex.LexicalElement);
            Assert.Equal("test", ex.Message);
            Assert.Equal(ErrorKind.UnknownError, ex.ErrorKind);
        }

        [Fact]
        public void MessageErrorKindConstructor()
        {
            var ex = new ParsingException("test", ErrorKind.MissingOperand);
            Assert.Null(ex.LexicalElement);
            Assert.Equal("test", ex.Message);
            Assert.Equal(ErrorKind.MissingOperand, ex.ErrorKind);
        }

        [Fact]
        public void MessageInnerExceptionConstructor()
        {
            var innerException = new Exception();
            var ex = new ParsingException("test", innerException);
            Assert.Null(ex.LexicalElement);
            Assert.Equal("test", ex.Message);
            Assert.Equal(innerException, ex.InnerException);
        }

        [Fact]
        public void LexicalElementConstructor()
        {
            var element = new LexicalElement(LexicalElementType.And, "test", 15);
            var ex = new ParsingException(element);
            Assert.Equal(element, ex.LexicalElement);
        }

        [Fact]
        public void MessageLexicalElementConstructor()
        {
            var element = new LexicalElement(LexicalElementType.And, "test", 15);
            var ex = new ParsingException("test2", element);
            Assert.Equal("test2", ex.Message);
            Assert.Equal(element, ex.LexicalElement);
        }

        [Fact]
        public void MessageLexicalElementErrorKindConstructor()
        {
            var element = new LexicalElement(LexicalElementType.And, "test", 15);
            var ex = new ParsingException("test2", element, ErrorKind.MissingLeftParenthesis);
            Assert.Equal("test2", ex.Message);
            Assert.Equal(element, ex.LexicalElement);
            Assert.Equal(ErrorKind.MissingLeftParenthesis, ex.ErrorKind);
        }

        [Fact]
        public void MessageInnerExceptionLexicalElementConstructor()
        {
            var innerException = new Exception();
            var element = new LexicalElement(LexicalElementType.And, "test", 15);
            var ex = new ParsingException("test2", innerException, element);
            Assert.Equal("test2", ex.Message);
            Assert.Equal(element, ex.LexicalElement);
            Assert.Equal(innerException, ex.InnerException);
        }

        [Fact]
        public void SerializationConstructor()
        {
            var info = new SerializationInfo(typeof(ParsingException), new FormatterConverter());
            info.AddValue("ClassName", string.Empty);
            info.AddValue("Message", string.Empty);
            info.AddValue("InnerException", new ArgumentException());
            info.AddValue("HelpURL", string.Empty);
            info.AddValue("StackTraceString", string.Empty);
            info.AddValue("RemoteStackTraceString", string.Empty);
            info.AddValue("RemoteStackIndex", 0);
            info.AddValue("ExceptionMethod", string.Empty);
            info.AddValue("HResult", 1);
            info.AddValue("Source", string.Empty);
            info.AddValue("LexicalElement", new LexicalElement(LexicalElementType.Term, "test", 1));
            info.AddValue("ErrorKind", ErrorKind.UnknownError);
            var ex = new ParsingException(info, new StreamingContext());
            Assert.NotNull(ex);
        }

        [Fact]
        public void SerializationRoundtripSuccessful()
        {
            var sourceLexicalElement = new LexicalElement(LexicalElementType.Term, "term", 1);
            var sourceException = new ParsingException("test", sourceLexicalElement);

            var binaryFormatter = new BinaryFormatter();

            using (var memoryStream = new MemoryStream())
            {
                binaryFormatter.Serialize(memoryStream, sourceException);
                memoryStream.Seek(0, SeekOrigin.Begin);
                var obj = binaryFormatter.Deserialize(memoryStream);
                var destinationException = obj as ParsingException;
                Assert.NotNull(destinationException);
                Assert.Equal("test", destinationException.Message);
                Assert.Equal(sourceLexicalElement, destinationException.LexicalElement);
            }
        }
    }
}
