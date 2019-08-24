namespace KeywordQueryLanguageCompiler.Tests
{
    using System;

    using Xunit;

    public class ParserTests
    {
        [Fact]
        public void NullParameter()
        {
            Assert.Throws<ArgumentNullException>(() => Parser.GetPostfixNotation(null));
        }

        [Fact]
        public void EmptyArrayParameter()
        {
            var input = new LexicalElement[0];
            var output = Parser.GetPostfixNotation(input);
            Assert.True(output.Length == 0);
        }

        [Fact]
        public void TermElementParameter()
        {
            var input = new[]
            {
                new LexicalElement(LexicalElementType.Term, "test", 1)
            };

            var output = Parser.GetPostfixNotation(input);

            Assert.True(TestsHelper.IsEqual(input, output));
        }

        [Fact]
        public void OrphanAndElementParameter()
        {
            var input = new[]
            {
                new LexicalElement(LexicalElementType.And, "AND", 1)
            };

            var output = Parser.GetPostfixNotation(input);

            Assert.True(TestsHelper.IsEqual(input, output));
        }

        [Fact]
        public void OrphanNotElementParameter()
        {
            var input = new[]
            {
                new LexicalElement(LexicalElementType.Not, "NOT", 1)
            };

            var output = Parser.GetPostfixNotation(input);

            Assert.True(TestsHelper.IsEqual(input, output));
        }

        [Fact]
        public void OrphanOrElementParameter()
        {
            var input = new[]
            {
                new LexicalElement(LexicalElementType.Or, "OR", 1)
            };

            var output = Parser.GetPostfixNotation(input);

            Assert.True(TestsHelper.IsEqual(input, output));
        }

        [Fact]
        public void OrphanNearElementParameter()
        {
            var input = new[]
            {
                new LexicalElement(LexicalElementType.Near, "NEAR", 1)
            };

            var output = Parser.GetPostfixNotation(input);

            Assert.True(TestsHelper.IsEqual(input, output));
        }

        [Fact]
        public void LeftParenthesisElementParameter()
        {
            var input = new[]
            {
                new LexicalElement(LexicalElementType.LeftParenthesis, "(", 1)
            };
            Assert.Throws<ParsingException>(() => Parser.GetPostfixNotation(input));
        }

        [Fact]
        public void RightParenthesisElementParameter()
        {
            var input = new[]
            {
                new LexicalElement(LexicalElementType.RightParenthesis, ")", 1)
            };
            Assert.Throws<ParsingException>(() => Parser.GetPostfixNotation(input));
        }

        [Fact]
        public void AndElementParameter()
        {
            var input = new[]
            {
                new LexicalElement(LexicalElementType.Term, "a", 1),
                new LexicalElement(LexicalElementType.And, "AND", 3),
                new LexicalElement(LexicalElementType.Term, "b", 7)
            };

            var expectedOutput = new[]
            {
                new LexicalElement(LexicalElementType.Term, "a", 1),
                new LexicalElement(LexicalElementType.Term, "b", 7),
                new LexicalElement(LexicalElementType.And, "AND", 3)
            };

            var output = Parser.GetPostfixNotation(input);

            Assert.True(TestsHelper.IsEqual(output, expectedOutput));
        }

        [Fact]
        public void NotElementParameter()
        {
            var input = new[]
            {
                new LexicalElement(LexicalElementType.Term, "a", 1),
                new LexicalElement(LexicalElementType.Not, "NOT", 3),
                new LexicalElement(LexicalElementType.Term, "b", 7)
            };

            var expectedOutput = new[]
            {
                new LexicalElement(LexicalElementType.Term, "a", 1),
                new LexicalElement(LexicalElementType.Term, "b", 7),
                new LexicalElement(LexicalElementType.Not, "NOT", 3)
            };

            var output = Parser.GetPostfixNotation(input);

            Assert.True(TestsHelper.IsEqual(output, expectedOutput));
        }
        
        [Fact]
        public void OrElementParameter()
        {
            var input = new[]
            {
                new LexicalElement(LexicalElementType.Term, "a", 1),
                new LexicalElement(LexicalElementType.Or, "OR", 3),
                new LexicalElement(LexicalElementType.Term, "b", 7)
            };

            var expectedOutput = new[]
            {
                new LexicalElement(LexicalElementType.Term, "a", 1),
                new LexicalElement(LexicalElementType.Term, "b", 7),
                new LexicalElement(LexicalElementType.Or, "OR", 3)
            };

            var output = Parser.GetPostfixNotation(input);

            Assert.True(TestsHelper.IsEqual(output, expectedOutput));
        }

        [Fact]
        public void NearElementParameter()
        {
            var input = new[]
            {
                new LexicalElement(LexicalElementType.Term, "a", 1),
                new LexicalElement(LexicalElementType.Near, "NEAR", 3),
                new LexicalElement(LexicalElementType.Term, "b", 7)
            };

            var expectedOutput = new[]
            {
                new LexicalElement(LexicalElementType.Term, "a", 1),
                new LexicalElement(LexicalElementType.Term, "b", 7),
                new LexicalElement(LexicalElementType.Near, "NEAR", 3)
            };

            var output = Parser.GetPostfixNotation(input);

            Assert.True(TestsHelper.IsEqual(output, expectedOutput));
        }

        [Fact]
        public void EmptyParentheses()
        {
            var input = new[]
            {
                new LexicalElement(LexicalElementType.LeftParenthesis, "(", 1),
                new LexicalElement(LexicalElementType.RightParenthesis, ")", 2)
            };
            Assert.Throws<ParsingException>(() => Parser.GetPostfixNotation(input));
        }

        [Fact]
        public void ValidSimpleParentheses()
        {
            var input = new[]
            {
                new LexicalElement(LexicalElementType.LeftParenthesis, "(", 1),
                new LexicalElement(LexicalElementType.Term, "a", 2),
                new LexicalElement(LexicalElementType.RightParenthesis, ")", 3)
            };

            var expectedOutput = new[]
            {
                new LexicalElement(LexicalElementType.Term, "a", 2)
            };

            var output = Parser.GetPostfixNotation(input);

            Assert.True(TestsHelper.IsEqual(output, expectedOutput));
        }

        [Fact]
        public void ValidDoubleParentheses()
        {
            var input = new[]
            {
                new LexicalElement(LexicalElementType.LeftParenthesis, "(", 1),
                new LexicalElement(LexicalElementType.LeftParenthesis, "(", 2),
                new LexicalElement(LexicalElementType.Term, "a", 3),
                new LexicalElement(LexicalElementType.RightParenthesis, ")", 4),
                new LexicalElement(LexicalElementType.RightParenthesis, ")", 5)
            };

            var expectedOutput = new[]
            {
                new LexicalElement(LexicalElementType.Term, "a", 3)
            };

            var output = Parser.GetPostfixNotation(input);

            Assert.True(TestsHelper.IsEqual(output, expectedOutput));
        }
        
        [Fact]
        public void ValidTripleParentheses()
        {
            var input = new[]
            {
                new LexicalElement(LexicalElementType.LeftParenthesis, "(", 1),
                new LexicalElement(LexicalElementType.LeftParenthesis, "(", 2),
                new LexicalElement(LexicalElementType.LeftParenthesis, "(", 3),
                new LexicalElement(LexicalElementType.Term, "a", 4),
                new LexicalElement(LexicalElementType.RightParenthesis, ")", 5),
                new LexicalElement(LexicalElementType.RightParenthesis, ")", 6),
                new LexicalElement(LexicalElementType.RightParenthesis, ")", 7)
            };

            var expectedOutput = new[]
            {
                new LexicalElement(LexicalElementType.Term, "a", 4)
            };

            var output = Parser.GetPostfixNotation(input);

            Assert.True(TestsHelper.IsEqual(output, expectedOutput));
        }

        [Fact]
        public void EmptyInvertedParentheses()
        {
            var input = new[]
            {
                new LexicalElement(LexicalElementType.RightParenthesis, ")", 1),
                new LexicalElement(LexicalElementType.LeftParenthesis, "(", 2)
            };
            Assert.Throws<ParsingException>(() => Parser.GetPostfixNotation(input));
        }

        [Fact]
        public void InvertedParentheses()
        {
            var input = new[]
            {
                new LexicalElement(LexicalElementType.RightParenthesis, ")", 1),
                new LexicalElement(LexicalElementType.Term, "a", 2),
                new LexicalElement(LexicalElementType.LeftParenthesis, "(", 3)
            };
            Assert.Throws<ParsingException>(() => Parser.GetPostfixNotation(input));
        }

        [Fact]
        public void MissingLeftParentheses()
        {
            var input = new[]
            {
                new LexicalElement(LexicalElementType.Term, "a", 2),
                new LexicalElement(LexicalElementType.RightParenthesis, ")", 3)
            };
            Assert.Throws<ParsingException>(() => Parser.GetPostfixNotation(input));
        }

        [Fact]
        public void TooManyLeftParentheses()
        {
            var input = new[]
            {
                new LexicalElement(LexicalElementType.LeftParenthesis, "(", 1),
                new LexicalElement(LexicalElementType.LeftParenthesis, "(", 2),
                new LexicalElement(LexicalElementType.Term, "a", 3),
                new LexicalElement(LexicalElementType.RightParenthesis, ")", 4)
            };
            Assert.Throws<ParsingException>(() => Parser.GetPostfixNotation(input));
        }

        [Fact]
        public void MissingRightParentheses()
        {
            var input = new[]
            {
                new LexicalElement(LexicalElementType.LeftParenthesis, "(", 1),
                new LexicalElement(LexicalElementType.Term, "a", 2)
            };
            Assert.Throws<ParsingException>(() => Parser.GetPostfixNotation(input));
        }

        [Fact]
        public void TooManyRightParentheses()
        {
            var input = new[]
            {
                new LexicalElement(LexicalElementType.LeftParenthesis, "(", 1),
                new LexicalElement(LexicalElementType.Term, "a", 2),
                new LexicalElement(LexicalElementType.RightParenthesis, ")", 3),
                new LexicalElement(LexicalElementType.RightParenthesis, ")", 4)
            };
            Assert.Throws<ParsingException>(() => Parser.GetPostfixNotation(input));
        }
        
        [Fact]
        public void ParenthesesOrder1()
        {
            var input = new[]
            {
                new LexicalElement(LexicalElementType.LeftParenthesis, "(", 1),
                new LexicalElement(LexicalElementType.LeftParenthesis, "(", 2),
                new LexicalElement(LexicalElementType.Term, "a", 3),
                new LexicalElement(LexicalElementType.And, "AND", 4),
                new LexicalElement(LexicalElementType.Term, "b", 5),
                new LexicalElement(LexicalElementType.RightParenthesis, ")", 6),
                new LexicalElement(LexicalElementType.And, "AND", 7),
                new LexicalElement(LexicalElementType.Term, "c", 8),
                new LexicalElement(LexicalElementType.RightParenthesis, ")", 9)
            };

            var expectedOutput = new[]
            {
                new LexicalElement(LexicalElementType.Term, "a", 3),
                new LexicalElement(LexicalElementType.Term, "b", 5),
                new LexicalElement(LexicalElementType.And, "AND", 4),
                new LexicalElement(LexicalElementType.Term, "c", 8),
                new LexicalElement(LexicalElementType.And, "AND", 7),
            };

            var output = Parser.GetPostfixNotation(input);

            Assert.True(TestsHelper.IsEqual(output, expectedOutput));
        }

        [Fact]
        public void ParenthesesOrder2()
        {
            var input = new[]
            {
                new LexicalElement(LexicalElementType.LeftParenthesis, "(", 1),
                new LexicalElement(LexicalElementType.Term, "a", 2),
                new LexicalElement(LexicalElementType.And, "AND", 3),
                new LexicalElement(LexicalElementType.LeftParenthesis, "(", 4),
                new LexicalElement(LexicalElementType.Term, "b", 5),
                new LexicalElement(LexicalElementType.And, "AND", 6),
                new LexicalElement(LexicalElementType.Term, "c", 7),
                new LexicalElement(LexicalElementType.RightParenthesis, ")", 8),
                new LexicalElement(LexicalElementType.RightParenthesis, ")", 9)
            };

            var expectedOutput = new[]
            {
                new LexicalElement(LexicalElementType.Term, "a", 2),
                new LexicalElement(LexicalElementType.Term, "b", 5),
                new LexicalElement(LexicalElementType.Term, "c", 7),
                new LexicalElement(LexicalElementType.And, "AND", 6),
                new LexicalElement(LexicalElementType.And, "AND", 3),
            };

            var output = Parser.GetPostfixNotation(input);

            Assert.True(TestsHelper.IsEqual(output, expectedOutput));
        }

        [Fact]
        public void ComplexInput1()
        {
            var input = new[]
            {
                new LexicalElement(LexicalElementType.Term, "a", 1),
                new LexicalElement(LexicalElementType.And, "AND", 2),
                new LexicalElement(LexicalElementType.LeftParenthesis, "(", 3),
                new LexicalElement(LexicalElementType.Term, "b", 4),
                new LexicalElement(LexicalElementType.Not, "NOT", 5),
                new LexicalElement(LexicalElementType.Term, "c", 6),
                new LexicalElement(LexicalElementType.Or, "OR", 7),
                new LexicalElement(LexicalElementType.Term, "d", 8),
                new LexicalElement(LexicalElementType.RightParenthesis, ")", 9),
                new LexicalElement(LexicalElementType.Near, "NEAR", 10),
                new LexicalElement(LexicalElementType.Term, "e", 11)
            };

            var expectedOutput = new[]
            {
                new LexicalElement(LexicalElementType.Term, "a", 1),
                new LexicalElement(LexicalElementType.Term, "b", 4),
                new LexicalElement(LexicalElementType.Term, "c", 6),
                new LexicalElement(LexicalElementType.Not, "NOT", 5),
                new LexicalElement(LexicalElementType.Term, "d", 8),
                new LexicalElement(LexicalElementType.Or, "OR", 7),
                new LexicalElement(LexicalElementType.Term, "e", 11),
                new LexicalElement(LexicalElementType.Near, "NEAR", 10),
                new LexicalElement(LexicalElementType.And, "AND", 2)
            };

            var output = Parser.GetPostfixNotation(input);

            Assert.True(TestsHelper.IsEqual(output, expectedOutput));
        }

        [Fact]
        public void ComplexInput2()
        {
            var input = new[]
            {
                new LexicalElement(LexicalElementType.LeftParenthesis, "(", 1),
                new LexicalElement(LexicalElementType.Term, "a", 2),
                new LexicalElement(LexicalElementType.And, "AND", 3),
                new LexicalElement(LexicalElementType.LeftParenthesis, "(", 4),
                new LexicalElement(LexicalElementType.Term, "b", 5),
                new LexicalElement(LexicalElementType.Not, "NOT", 6),
                new LexicalElement(LexicalElementType.LeftParenthesis, "(", 7),
                new LexicalElement(LexicalElementType.Term, "c", 8),
                new LexicalElement(LexicalElementType.Or, "OR", 9),
                new LexicalElement(LexicalElementType.LeftParenthesis, "(", 10),
                new LexicalElement(LexicalElementType.Term, "d", 11),
                new LexicalElement(LexicalElementType.Near, "NEAR", 12),
                new LexicalElement(LexicalElementType.Term, "e", 13),
                new LexicalElement(LexicalElementType.RightParenthesis, ")", 14),
                new LexicalElement(LexicalElementType.RightParenthesis, ")", 15),
                new LexicalElement(LexicalElementType.RightParenthesis, ")", 16),
                new LexicalElement(LexicalElementType.RightParenthesis, ")", 17)
            };

            var expectedOutput = new[]
            {
                new LexicalElement(LexicalElementType.Term, "a", 2),
                new LexicalElement(LexicalElementType.Term, "b", 5),
                new LexicalElement(LexicalElementType.Term, "c", 8),
                new LexicalElement(LexicalElementType.Term, "d", 11),
                new LexicalElement(LexicalElementType.Term, "e", 13),
                new LexicalElement(LexicalElementType.Near, "NEAR", 12),
                new LexicalElement(LexicalElementType.Or, "OR", 9),
                new LexicalElement(LexicalElementType.Not, "NOT", 6),
                new LexicalElement(LexicalElementType.And, "AND", 3),
            };

            var output = Parser.GetPostfixNotation(input);

            Assert.True(TestsHelper.IsEqual(output, expectedOutput));
        }

        [Fact]
        public void NearExpression1()
        {
            var input = new[]
            {
                new LexicalElement(LexicalElementType.LeftParenthesis, "(", 1),
                new LexicalElement(LexicalElementType.Term, "a", 2),
                new LexicalElement(LexicalElementType.And, "AND", 3),
                new LexicalElement(LexicalElementType.Term, "b", 4),
                new LexicalElement(LexicalElementType.RightParenthesis, ")", 5),
                new LexicalElement(LexicalElementType.Near, "NEAR", 6),
                new LexicalElement(LexicalElementType.Term, "c", 7)
            };

            var expectedOutput = new[]
            {
                new LexicalElement(LexicalElementType.Term, "a", 2),
                new LexicalElement(LexicalElementType.Term, "b", 4),
                new LexicalElement(LexicalElementType.And, "AND", 3),
                new LexicalElement(LexicalElementType.Term, "c", 7),
                new LexicalElement(LexicalElementType.Near, "NEAR", 6)
            };

            var output = Parser.GetPostfixNotation(input);

            Assert.True(TestsHelper.IsEqual(output, expectedOutput));
        }

        [Fact]
        public void NearExpression2()
        {
            var input = new[]
            {
                new LexicalElement(LexicalElementType.Near, "NEAR", 1),
                new LexicalElement(LexicalElementType.Term, "a", 2),
                new LexicalElement(LexicalElementType.LeftParenthesis, "(", 3),
                new LexicalElement(LexicalElementType.Term, "b", 4),
                new LexicalElement(LexicalElementType.And, "AND", 5),
                new LexicalElement(LexicalElementType.Term, "c", 6),
                new LexicalElement(LexicalElementType.RightParenthesis, ")", 7)
            };

            var expectedOutput = new[]
            {
                new LexicalElement(LexicalElementType.Term, "a", 2),
                new LexicalElement(LexicalElementType.Term, "b", 4),
                new LexicalElement(LexicalElementType.Term, "c", 6),
                new LexicalElement(LexicalElementType.And, "AND", 5),
                new LexicalElement(LexicalElementType.Near, "NEAR", 1)
            };

            var output = Parser.GetPostfixNotation(input);

            Assert.True(TestsHelper.IsEqual(output, expectedOutput));
        }
    }
}
