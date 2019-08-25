namespace KeywordQueryLanguageCompiler.Tests
{
    using System;

    using Xunit;

    public class CodeGeneratorTests
    {
        [Fact]
        public void NullParameter()
        {
            Assert.Throws<ArgumentNullException>(() => CodeGenerator.Generate(null));
        }

        [Fact]
        public void EmptyArrayParameter()
        {
            var input = new LexicalElement[0];
            Assert.Throws<ParsingException>(() => CodeGenerator.Generate(input));
        }

        [Fact]
        public void SimpleTerm()
        {
            var input = new[]
            {
                new LexicalElement(LexicalElementType.Term, "test", 1)
            };

            var expectedOutput = "(FORMSOF(INFLECTIONAL, \"test\") OR FORMSOF(THESAURUS, \"test\"))";

            var output = CodeGenerator.Generate(input, FormsOfType.Both);

            Assert.Equal(expectedOutput, output);
        }
        
        [Fact]
        public void SimpleTermWithSimpleQuote()
        {
            var input = new[]
            {
                new LexicalElement(LexicalElementType.Term, "l'espoir", 1)
            };

            var expectedOutput = "(FORMSOF(INFLECTIONAL, \"l'espoir\") OR FORMSOF(THESAURUS, \"l'espoir\"))";

            var output = CodeGenerator.Generate(input, FormsOfType.Both);

            Assert.Equal(expectedOutput, output);
        }

        [Fact]
        public void SimpleWildcardTerm()
        {
            var input = new[]
            {
                new LexicalElement(LexicalElementType.Term, "test*", 1)
            };

            var expectedOutput = "\"test*\"";

            var output = CodeGenerator.Generate(input, FormsOfType.Both);

            Assert.Equal(expectedOutput, output);
        }

        [Fact]
        public void SimpleAnd()
        {
            var input = new[]
            {
                new LexicalElement(LexicalElementType.And, "And", 1)
            };
            Assert.Throws<ParsingException>(() => CodeGenerator.Generate(input, FormsOfType.Both));
        }

        [Fact]
        public void SimpleNot()
        {
            var input = new[]
            {
                new LexicalElement(LexicalElementType.Not, "NOT", 1)
            };
            Assert.Throws<ParsingException>(() => CodeGenerator.Generate(input, FormsOfType.Both));
        }

        [Fact]
        public void SimpleAndNot()
        {
            var input = new[]
            {
                new LexicalElement(LexicalElementType.Not, "ANDNOT", 1)
            };
            Assert.Throws<ParsingException>(() => CodeGenerator.Generate(input, FormsOfType.Both));
        }

        [Fact]
        public void SimpleOr()
        {
            var input = new[]
            {
                new LexicalElement(LexicalElementType.Or, "OR", 1)
            };
            Assert.Throws<ParsingException>(() => CodeGenerator.Generate(input, FormsOfType.Both));
        }

        [Fact]
        public void SimpleNear()
        {
            var input = new[]
            {
                new LexicalElement(LexicalElementType.Near, "NEAR", 1)
            };
            Assert.Throws<ParsingException>(() => CodeGenerator.Generate(input, FormsOfType.Both));
        }

        [Fact]
        public void SimpleLeftParenthesis()
        {
            var input = new[]
            {
                new LexicalElement(LexicalElementType.LeftParenthesis, "(", 1)
            };
            Assert.Throws<ParsingException>(() => CodeGenerator.Generate(input, FormsOfType.Both));
        }

        [Fact]
        public void SimpleRightParenthesis()
        {
            var input = new[]
            {
                new LexicalElement(LexicalElementType.LeftParenthesis, ")", 1)
            };
            Assert.Throws<ParsingException>(() => CodeGenerator.Generate(input, FormsOfType.Both));
        }

        [Fact]
        public void DoubleTerm()
        {
            var input = new[]
            {
                new LexicalElement(LexicalElementType.Term, "test", 1),
                new LexicalElement(LexicalElementType.Term, "test", 2)
            };
            Assert.Throws<ParsingException>(() => CodeGenerator.Generate(input, FormsOfType.Both));
        }

        [Fact]
        public void DoubleAnd()
        {
            var input = new[]
            {
                new LexicalElement(LexicalElementType.And, "And", 1),
                new LexicalElement(LexicalElementType.And, "And", 2)
            };
            Assert.Throws<ParsingException>(() => CodeGenerator.Generate(input, FormsOfType.Both));
        }

        [Fact]
        public void ValidAnd()
        {
            var input = new[]
            {
                new LexicalElement(LexicalElementType.Term, "op1", 1),
                new LexicalElement(LexicalElementType.Term, "op2", 3),
                new LexicalElement(LexicalElementType.And, "AND", 2)
            };

            var expectedOutput = "((FORMSOF(INFLECTIONAL, \"op1\") OR FORMSOF(THESAURUS, \"op1\")) AND (FORMSOF(INFLECTIONAL, \"op2\") OR FORMSOF(THESAURUS, \"op2\")))";

            var output = CodeGenerator.Generate(input, FormsOfType.Both);

            Assert.Equal(expectedOutput, output);
        }

        [Fact]
        public void AndInflectionalFormsOfType()
        {
            var input = new[]
            {
                new LexicalElement(LexicalElementType.Term, "op1", 1),
                new LexicalElement(LexicalElementType.Term, "op2", 3),
                new LexicalElement(LexicalElementType.And, "AND", 2)
            };

            var expectedOutput = "(FORMSOF(INFLECTIONAL, \"op1\") AND FORMSOF(INFLECTIONAL, \"op2\"))";

            var output = CodeGenerator.Generate(input, FormsOfType.Inflectional);

            Assert.Equal(expectedOutput, output);
        }
        
        [Fact]
        public void AndThesaurusFormsOfType()
        {
            var input = new[]
            {
                new LexicalElement(LexicalElementType.Term, "op1", 1),
                new LexicalElement(LexicalElementType.Term, "op2", 3),
                new LexicalElement(LexicalElementType.And, "AND", 2)
            };

            var expectedOutput = "(FORMSOF(THESAURUS, \"op1\") AND FORMSOF(THESAURUS, \"op2\"))";

            var output = CodeGenerator.Generate(input, FormsOfType.Thesaurus);

            Assert.Equal(expectedOutput, output);
        }

        [Fact]
        public void AndNoneFormsOfType()
        {
            var input = new[]
            {
                new LexicalElement(LexicalElementType.Term, "op1", 1),
                new LexicalElement(LexicalElementType.Term, "op2", 3),
                new LexicalElement(LexicalElementType.And, "AND", 2)
            };

            var expectedOutput = "(\"op1\" AND \"op2\")";

            var output = CodeGenerator.Generate(input, FormsOfType.None);

            Assert.Equal(expectedOutput, output);
        }

        [Fact]
        public void AndInexistingFormsOfType()
        {
            var input = new[]
            {
                new LexicalElement(LexicalElementType.Term, "op1", 1),
                new LexicalElement(LexicalElementType.Term, "op2", 3),
                new LexicalElement(LexicalElementType.And, "AND", 2)
            };

            Assert.Throws<ParsingException>(() => CodeGenerator.Generate(input, (FormsOfType)9999999));
        }

        [Fact]
        public void ValidNot()
        {
            var input = new[]
            {
                new LexicalElement(LexicalElementType.Term, "op1", 1),
                new LexicalElement(LexicalElementType.Term, "op2", 3),
                new LexicalElement(LexicalElementType.Not, "NOT", 2)
            };

            var expectedOutput = "((FORMSOF(INFLECTIONAL, \"op1\") OR FORMSOF(THESAURUS, \"op1\")) AND NOT (FORMSOF(INFLECTIONAL, \"op2\") OR FORMSOF(THESAURUS, \"op2\")))";

            var output = CodeGenerator.Generate(input, FormsOfType.Both);

            Assert.Equal(expectedOutput, output);
        }

        [Fact]
        public void ValidAndNot()
        {
            var input = new[]
            {
                new LexicalElement(LexicalElementType.Term, "op1", 1),
                new LexicalElement(LexicalElementType.Term, "op2", 3),
                new LexicalElement(LexicalElementType.Not, "ANDNOT", 2)
            };

            var expectedOutput = "((FORMSOF(INFLECTIONAL, \"op1\") OR FORMSOF(THESAURUS, \"op1\")) AND NOT (FORMSOF(INFLECTIONAL, \"op2\") OR FORMSOF(THESAURUS, \"op2\")))";

            var output = CodeGenerator.Generate(input, FormsOfType.Both);

            Assert.Equal(expectedOutput, output);
        }

        [Fact]
        public void ValidOr()
        {
            var input = new[]
            {
                new LexicalElement(LexicalElementType.Term, "op1", 1),
                new LexicalElement(LexicalElementType.Term, "op2", 3),
                new LexicalElement(LexicalElementType.Or, "OR", 2)
            };

            var expectedOutput = "((FORMSOF(INFLECTIONAL, \"op1\") OR FORMSOF(THESAURUS, \"op1\")) OR (FORMSOF(INFLECTIONAL, \"op2\") OR FORMSOF(THESAURUS, \"op2\")))";

            var output = CodeGenerator.Generate(input, FormsOfType.Both);

            Assert.Equal(expectedOutput, output);
        }

        [Fact]
        public void ValidNear()
        {
            var input = new[]
            {
                new LexicalElement(LexicalElementType.Term, "op1", 1),
                new LexicalElement(LexicalElementType.Term, "op2", 3),
                new LexicalElement(LexicalElementType.Near, "NEAR", 2)
            };

            var expectedOutput = "(NEAR((\"op1\", \"op2\"), 2000, FALSE))";

            var output = CodeGenerator.Generate(input, FormsOfType.Both);

            Assert.Equal(expectedOutput, output);
        }

        [Fact]
        public void InfixOr()
        {
            var input = new[]
            {
                new LexicalElement(LexicalElementType.Term, "op1", 1),
                new LexicalElement(LexicalElementType.Or, "OR", 2),
                new LexicalElement(LexicalElementType.Term, "op2", 3)
            };
            Assert.Throws<ParsingException>(() => CodeGenerator.Generate(input, FormsOfType.Both));
        }

        [Fact]
        public void ComplexExpression1()
        {
            var input = new[]
            {
                new LexicalElement(LexicalElementType.Term, "a", 2),
                new LexicalElement(LexicalElementType.Term, "b", 4),
                new LexicalElement(LexicalElementType.And, "AND", 3),
                new LexicalElement(LexicalElementType.Term, "c", 7),
                new LexicalElement(LexicalElementType.Near, "NEAR", 6)
            };
            Assert.Throws<ParsingException>(() => CodeGenerator.Generate(input, FormsOfType.Both));
        }

        [Fact]
        public void ComplexExpression2()
        {
            var input = new[]
            {
                new LexicalElement(LexicalElementType.Term, "a", 2),
                new LexicalElement(LexicalElementType.Term, "b", 4),
                new LexicalElement(LexicalElementType.Term, "c", 6),
                new LexicalElement(LexicalElementType.And, "AND", 5),
                new LexicalElement(LexicalElementType.Near, "NEAR", 1)
            };
            Assert.Throws<ParsingException>(() => CodeGenerator.Generate(input, FormsOfType.Both));
        }

        [Fact]
        public void ComplexExpression3()
        {
            var input = new[]
            {
                new LexicalElement(LexicalElementType.Term, "op1", 1),
                new LexicalElement(LexicalElementType.Term, "op2", 2),
                new LexicalElement(LexicalElementType.Term, "op3", 3),
                new LexicalElement(LexicalElementType.Term, "op4", 4),
                new LexicalElement(LexicalElementType.Term, "op5", 5),
                new LexicalElement(LexicalElementType.Near, "Near", 6),
                new LexicalElement(LexicalElementType.And, "AND", 7),
                new LexicalElement(LexicalElementType.Not, "Not", 8),
                new LexicalElement(LexicalElementType.Or, "Or", 9),
            };

            var expectedOutput = "((FORMSOF(INFLECTIONAL, \"op1\") OR FORMSOF(THESAURUS, \"op1\")) OR ((FORMSOF(INFLECTIONAL, \"op2\") OR FORMSOF(THESAURUS, \"op2\")) AND NOT ((FORMSOF(INFLECTIONAL, \"op3\") OR FORMSOF(THESAURUS, \"op3\")) AND (NEAR((\"op4\", \"op5\"), 2000, FALSE)))))";

            var output = CodeGenerator.Generate(input, FormsOfType.Both);

            Assert.Equal(expectedOutput, output);
        }

        [Fact]
        public void ComplexExpression4()
        {
            var input = new[]
            {
                new LexicalElement(LexicalElementType.Term, "op1*", 1),
                new LexicalElement(LexicalElementType.Term, "op2*", 3),
                new LexicalElement(LexicalElementType.And, "AND", 2)
            };

            var expectedOutput = "(\"op1*\" AND \"op2*\")";

            var output = CodeGenerator.Generate(input, FormsOfType.Both);

            Assert.Equal(expectedOutput, output);
        }

        [Fact]
        public void ComplexExpression5()
        {
            var input = new[]
            {
                new LexicalElement(LexicalElementType.Term, "'op1'", 1),
                new LexicalElement(LexicalElementType.Term, "o'p'2", 3),
                new LexicalElement(LexicalElementType.And, "AND", 2)
            };

            var expectedOutput = "(FORMSOF(THESAURUS, \"'op1'\") AND FORMSOF(THESAURUS, \"o'p'2\"))";

            var output = CodeGenerator.Generate(input, FormsOfType.Thesaurus);

            Assert.Equal(expectedOutput, output);
        }
    }
}
