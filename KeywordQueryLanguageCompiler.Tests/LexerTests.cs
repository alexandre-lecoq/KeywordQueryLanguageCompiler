namespace KeywordQueryLanguageCompiler.Tests
{
    using System;

    using Xunit;

    public class LexerTests
    {
        [Fact]
        public void SimpleTokenization_Null()
        {
            Assert.Throws<ArgumentNullException>(() => Lexer.Tokenize(null));
        }

        [Fact]
        public void SimpleTokenization_EmptyString()
        {
            var input = string.Empty;

            var elements = Lexer.Tokenize(input);

            Assert.True(elements.Length == 0);
        }

        [Fact]
        public void SimpleTokenization_WhiteSpaces()
        {
            var input = " \t \t \t ";

            var elements = Lexer.Tokenize(input);

            Assert.True(elements.Length == 0);
        }

        [Fact]
        public void SimpleTokenization_OneCharacter()
        {
            var input = "A";

            var expectedElements = new[]
            {
                new LexicalElement(LexicalElementType.Term, "A", 1)
            };

            var elements = Lexer.Tokenize(input);

            Assert.True(TestsHelper.IsEqual(elements, expectedElements));
        }

        [Fact]
        public void SimpleTokenization_SimpleQuote()
        {
            var input = "'";

            var expectedElements = new[]
            {
                new LexicalElement(LexicalElementType.Term, "'", 1)
            };

            var elements = Lexer.Tokenize(input);

            Assert.True(TestsHelper.IsEqual(elements, expectedElements));
        }

        [Fact]
        public void SimpleTokenization_MultipleSimpleQuotes()
        {
            var input = "a'b'c";

            var expectedElements = new[]
            {
                new LexicalElement(LexicalElementType.Term, "a'b'c", 1)
            };

            var elements = Lexer.Tokenize(input);

            Assert.True(TestsHelper.IsEqual(elements, expectedElements));
        }

        [Fact]
        public void SimpleTokenization_Term()
        {
            var input = "word";

            var expectedElements = new[]
            {
                new LexicalElement(LexicalElementType.Term, "word", 1)
            };

            var elements = Lexer.Tokenize(input);

            Assert.True(TestsHelper.IsEqual(elements, expectedElements));
        }
        
        [Fact]
        public void SimpleTokenization_TermWithWhiteSpaces()
        {
            var input = "a b";

            var expectedElements = new[]
            {
                new LexicalElement(LexicalElementType.Term, "a b", 1)
            };

            var elements = Lexer.Tokenize(input);

            Assert.True(TestsHelper.IsEqual(elements, expectedElements));
        }

        [Fact]
        public void SimpleTokenization_TermWithWhiteSpacesAndKeyword()
        {
            var input = "a b AND c d";

            var expectedElements = new[]
            {
                new LexicalElement(LexicalElementType.Term, "a b", 1),
                new LexicalElement(LexicalElementType.And, "AND", 5),
                new LexicalElement(LexicalElementType.Term, "c d", 9)
            };

            var elements = Lexer.Tokenize(input);

            Assert.True(TestsHelper.IsEqual(elements, expectedElements));
        }


        [Fact]
        public void SimpleTokenization_ComplexSpaceSeparatedTerms1()
        {
            var input = "OR a b c AND  d  e  f  NOT";

            var expectedElements = new[]
            {
                new LexicalElement(LexicalElementType.Or, "OR", 1),
                new LexicalElement(LexicalElementType.Term, "a b c", 4),
                new LexicalElement(LexicalElementType.And, "AND", 10),
                new LexicalElement(LexicalElementType.Term, "d e f", 15),
                new LexicalElement(LexicalElementType.Not, "NOT", 24)
            };

            var elements = Lexer.Tokenize(input);

            Assert.True(TestsHelper.IsEqual(elements, expectedElements));
        }

        [Fact]
        public void SimpleTokenization_And()
        {
            var input = "aNd";

            var expectedElements = new[]
            {
                new LexicalElement(LexicalElementType.And, "aNd", 1)
            };

            var elements = Lexer.Tokenize(input);

            Assert.True(TestsHelper.IsEqual(elements, expectedElements));
        }

        [Fact]
        public void SimpleTokenization_Not()
        {
            var input = "nOt";

            var expectedElements = new[]
            {
                new LexicalElement(LexicalElementType.Not, "nOt", 1)
            };

            var elements = Lexer.Tokenize(input);

            Assert.True(TestsHelper.IsEqual(elements, expectedElements));
        }

        [Fact]
        public void SimpleTokenization_Or()
        {
            var input = "oR";

            var expectedElements = new[]
            {
                new LexicalElement(LexicalElementType.Or, "oR", 1)
            };

            var elements = Lexer.Tokenize(input);

            Assert.True(TestsHelper.IsEqual(elements, expectedElements));
        }
        
        [Fact]
        public void SimpleTokenization_Near()
        {
            var input = "nEaR";

            var expectedElements = new[]
            {
                new LexicalElement(LexicalElementType.Near, "nEaR", 1)
            };

            var elements = Lexer.Tokenize(input);

            Assert.True(TestsHelper.IsEqual(elements, expectedElements));
        }

        [Fact]
        public void SimpleTokenization_QuotedAnd()
        {
            var input = "\"AND\"";

            var expectedElements = new[]
            {
                new LexicalElement(LexicalElementType.Term, "AND", 2)
            };

            var elements = Lexer.Tokenize(input);

            Assert.True(TestsHelper.IsEqual(elements, expectedElements));
        }

        [Fact]
        public void SimpleTokenization_QuotedNot()
        {
            var input = "\"NOT\"";

            var expectedElements = new[]
            {
                new LexicalElement(LexicalElementType.Term, "NOT", 2)
            };

            var elements = Lexer.Tokenize(input);

            Assert.True(TestsHelper.IsEqual(elements, expectedElements));
        }

        [Fact]
        public void SimpleTokenization_QuotedOr()
        {
            var input = "\"OR\"";

            var expectedElements = new[]
            {
                new LexicalElement(LexicalElementType.Term, "OR", 2)
            };

            var elements = Lexer.Tokenize(input);

            Assert.True(TestsHelper.IsEqual(elements, expectedElements));
        }

        [Fact]
        public void SimpleTokenization_QuotedNear()
        {
            var input = "\"NEAR\"";

            var expectedElements = new[]
            {
                new LexicalElement(LexicalElementType.Term, "NEAR", 2)
            };

            var elements = Lexer.Tokenize(input);

            Assert.True(TestsHelper.IsEqual(elements, expectedElements));
        }

        [Fact]
        public void SimpleTokenization_BeginWithOr()
        {
            var input = "orxxx";

            var expectedElements = new[]
            {
                new LexicalElement(LexicalElementType.Term, "orxxx", 1)
            };

            var elements = Lexer.Tokenize(input);

            Assert.True(TestsHelper.IsEqual(elements, expectedElements));
        }

        [Fact]
        public void SimpleTokenization_EndWithOr()
        {
            var input = "xxxor";

            var expectedElements = new[]
            {
                new LexicalElement(LexicalElementType.Term, "xxxor", 1)
            };

            var elements = Lexer.Tokenize(input);

            Assert.True(TestsHelper.IsEqual(elements, expectedElements));
        }

        [Fact]
        public void SimpleTokenization_BeginWithNot()
        {
            var input = "notxxx";

            var expectedElements = new[]
            {
                new LexicalElement(LexicalElementType.Term, "notxxx", 1)
            };

            var elements = Lexer.Tokenize(input);

            Assert.True(TestsHelper.IsEqual(elements, expectedElements));
        }

        [Fact]
        public void SimpleTokenization_EndWithNot()
        {
            var input = "xxxnot";

            var expectedElements = new[]
            {
                new LexicalElement(LexicalElementType.Term, "xxxnot", 1)
            };

            var elements = Lexer.Tokenize(input);

            Assert.True(TestsHelper.IsEqual(elements, expectedElements));
        }

        [Fact]
        public void SimpleTokenization_BeginWithAnd()
        {
            var input = "andxxx";

            var expectedElements = new[]
            {
                new LexicalElement(LexicalElementType.Term, "andxxx", 1)
            };

            var elements = Lexer.Tokenize(input);

            Assert.True(TestsHelper.IsEqual(elements, expectedElements));
        }

        [Fact]
        public void SimpleTokenization_EndWithAnd()
        {
            var input = "xxxand";

            var expectedElements = new[]
            {
                new LexicalElement(LexicalElementType.Term, "xxxand", 1)
            };

            var elements = Lexer.Tokenize(input);

            Assert.True(TestsHelper.IsEqual(elements, expectedElements));
        }

        [Fact]
        public void SimpleTokenization_BeginWithNear()
        {
            var input = "nearxxx";

            var expectedElements = new[]
            {
                new LexicalElement(LexicalElementType.Term, "nearxxx", 1)
            };

            var elements = Lexer.Tokenize(input);

            Assert.True(TestsHelper.IsEqual(elements, expectedElements));
        }

        [Fact]
        public void SimpleTokenization_EndWithNear()
        {
            var input = "xxxnear";

            var expectedElements = new[]
            {
                new LexicalElement(LexicalElementType.Term, "xxxnear", 1)
            };

            var elements = Lexer.Tokenize(input);

            Assert.True(TestsHelper.IsEqual(elements, expectedElements));
        }

        [Fact]
        public void SimpleTokenization_LeftParenthesis()
        {
            var input = "(";

            var expectedElements = new[]
            {
                new LexicalElement(LexicalElementType.LeftParenthesis, "(", 1)
            };

            var elements = Lexer.Tokenize(input);

            Assert.True(TestsHelper.IsEqual(elements, expectedElements));
        }
        
        [Fact]
        public void SimpleTokenization_RightParenthesis()
        {
            var input = ")";

            var expectedElements = new[]
            {
                new LexicalElement(LexicalElementType.RightParenthesis, ")", 1)
            };

            var elements = Lexer.Tokenize(input);

            Assert.True(TestsHelper.IsEqual(elements, expectedElements));
        }

        [Fact]
        public void SimpleTokenization_Comma()
        {
            var input = ",";

            var expectedElements = new[]
            {
                new LexicalElement(LexicalElementType.And, ",", 1)
            };

            var elements = Lexer.Tokenize(input);

            Assert.True(TestsHelper.IsEqual(elements, expectedElements));
        }

        [Fact]
        public void SimpleTokenization_Asterisk()
        {
            var input = "*";

            var expectedElements = new[]
            {
                new LexicalElement(LexicalElementType.Term, "*", 1)
            };

            var elements = Lexer.Tokenize(input);

            Assert.True(TestsHelper.IsEqual(elements, expectedElements));
        }

        [Fact]
        public void SimpleTokenization_QuotedTerm()
        {
            var input = "\"a b\"";

            var expectedElements = new[]
            {
                new LexicalElement(LexicalElementType.Term, "a b", 2)
            };

            var elements = Lexer.Tokenize(input);

            Assert.True(TestsHelper.IsEqual(elements, expectedElements));
        }

        [Fact]
        public void SimpleTokenization_OneDoubleQuote()
        {
            var input = "\"";
            Assert.Throws<ParsingException>(() => Lexer.Tokenize(input));
        }
        
        [Fact]
        public void SimpleTokenization_TwoDoubleQuote()
        {
            var input = "\"\"";

            var elements = Lexer.Tokenize(input);

            Assert.True(elements.Length == 0);
        }

        [Fact]
        public void SimpleTokenization_ThreeDoubleQuote()
        {
            var input = "\"\"\"";
            Assert.Throws<ParsingException>(() => Lexer.Tokenize(input));
        }

        [Fact]
        public void ComplexTokenization_BeginningWithComma()
        {
            var input = ",a";

            var expectedElements = new[]
            {
                new LexicalElement(LexicalElementType.And, ",", 1),
                new LexicalElement(LexicalElementType.Term, "a", 2)
            };

            var elements = Lexer.Tokenize(input);

            Assert.True(TestsHelper.IsEqual(elements, expectedElements));
        }

        [Fact]
        public void ComplexTokenization_EndingWithComma()
        {
            var input = "a,";

            var expectedElements = new[]
            {
                new LexicalElement(LexicalElementType.Term, "a", 1),
                new LexicalElement(LexicalElementType.And, ",", 2)
            };

            var elements = Lexer.Tokenize(input);

            Assert.True(TestsHelper.IsEqual(elements, expectedElements));
        }

        [Fact]
        public void ComplexTokenization_CommaWithSpaces()
        {
            var input = "a , b";

            var expectedElements = new[]
            {
                new LexicalElement(LexicalElementType.Term, "a", 1),
                new LexicalElement(LexicalElementType.And, ",", 3),
                new LexicalElement(LexicalElementType.Term, "b", 5)
            };

            var elements = Lexer.Tokenize(input);

            Assert.True(TestsHelper.IsEqual(elements, expectedElements));
        }
        
        [Fact]
        public void ComplexTokenization_DoubleCommas()
        {
            var input = "a,,b";

            var expectedElements = new[]
            {
                new LexicalElement(LexicalElementType.Term, "a", 1),
                new LexicalElement(LexicalElementType.And, ",", 2),
                new LexicalElement(LexicalElementType.And, ",", 3),
                new LexicalElement(LexicalElementType.Term, "b", 4)
            };

            var elements = Lexer.Tokenize(input);

            Assert.True(TestsHelper.IsEqual(elements, expectedElements));
        }

        [Fact]
        public void ComplexTokenization_QuotedCommaWithSpaces()
        {
            var input = "\"a , b\"";

            var expectedElements = new[]
            {
                new LexicalElement(LexicalElementType.Term, "a , b", 2)
            };

            var elements = Lexer.Tokenize(input);

            Assert.True(TestsHelper.IsEqual(elements, expectedElements));
        }

        [Fact]
        public void ComplexTokenization_QuotedDoubleCommas()
        {
            var input = "\"a,,b\"";

            var expectedElements = new[]
            {
                new LexicalElement(LexicalElementType.Term, "a,,b", 2)
            };

            var elements = Lexer.Tokenize(input);

            Assert.True(TestsHelper.IsEqual(elements, expectedElements));
        }
        
        [Fact]
        public void ComplexTokenization_ParenthesizedCommaWithSpaces()
        {
            var input = "(a , b)";

            var expectedElements = new[]
            {
                new LexicalElement(LexicalElementType.LeftParenthesis, "(", 1),
                new LexicalElement(LexicalElementType.Term, "a", 2),
                new LexicalElement(LexicalElementType.And, ",", 4),
                new LexicalElement(LexicalElementType.Term, "b", 6),
                new LexicalElement(LexicalElementType.RightParenthesis, ")", 7)
            };

            var elements = Lexer.Tokenize(input);

            Assert.True(TestsHelper.IsEqual(elements, expectedElements));
        }

        [Fact]
        public void ComplexTokenization_ParenthesizedDoubleCommas()
        {
            var input = "(a,,b)";

            var expectedElements = new[]
            {
                new LexicalElement(LexicalElementType.LeftParenthesis, "(", 1),
                new LexicalElement(LexicalElementType.Term, "a", 2),
                new LexicalElement(LexicalElementType.And, ",", 3),
                new LexicalElement(LexicalElementType.And, ",", 4),
                new LexicalElement(LexicalElementType.Term, "b", 5),
                new LexicalElement(LexicalElementType.RightParenthesis, ")", 6)
            };

            var elements = Lexer.Tokenize(input);

            Assert.True(TestsHelper.IsEqual(elements, expectedElements));
        }

        [Fact]
        public void ComplexTokenization_AsteriskBeforeTermFails()
        {
            var input = "*ab";
            Assert.Throws<ParsingException>(() => Lexer.Tokenize(input));
        }

        [Fact]
        public void ComplexTokenization_AsteriskInsideTermFails()
        {
            var input = "a*b";
            Assert.Throws<ParsingException>(() => Lexer.Tokenize(input));
        }

        [Fact]
        public void ComplexTokenization_AsteriskAfterTermSucceeds()
        {
            var input = "ab*";

            var expectedElements = new[]
            {
                new LexicalElement(LexicalElementType.Term, "ab*", 1)
            };

            var elements = Lexer.Tokenize(input);

            Assert.True(TestsHelper.IsEqual(elements, expectedElements));
        }

        [Fact]
        public void ComplexTokenization_AllTokens()
        {
            var input = "reimbursement AnD NoT Or NeAr ( )";

            var expectedElements = new[]
            {
                new LexicalElement(LexicalElementType.Term, "reimbursement", 1),
                new LexicalElement(LexicalElementType.And, "AnD", 15),
                new LexicalElement(LexicalElementType.Not, "NoT", 19),
                new LexicalElement(LexicalElementType.Or, "Or", 23),
                new LexicalElement(LexicalElementType.Near, "NeAr", 26),
                new LexicalElement(LexicalElementType.LeftParenthesis, "(", 31),
                new LexicalElement(LexicalElementType.RightParenthesis, ")", 33)
            };

            var elements = Lexer.Tokenize(input);

            Assert.True(TestsHelper.IsEqual(elements, expectedElements));
        }

        [Fact]
        public void ComplexTokenization_TermCanContainTokens()
        {
            var input = "reimbursementAnDNoT()OrNeArdisapeard";

            var expectedElements = new[]
            {
                new LexicalElement(LexicalElementType.Term, "reimbursementAnDNoT", 1),
                new LexicalElement(LexicalElementType.LeftParenthesis, "(", 20),
                new LexicalElement(LexicalElementType.RightParenthesis, ")", 21),
                new LexicalElement(LexicalElementType.Term, "OrNeArdisapeard", 22)
            };

            var elements = Lexer.Tokenize(input);

            Assert.True(TestsHelper.IsEqual(elements, expectedElements));
        }

        [Fact]
        public void ComplexTokenization_ParenthesesAreValidSeparators()
        {
            var input = "and)not(or)near(word";

            var expectedElements = new[]
            {
                new LexicalElement(LexicalElementType.And, "and", 1),
                new LexicalElement(LexicalElementType.RightParenthesis, ")", 4),
                new LexicalElement(LexicalElementType.Not, "not", 5),
                new LexicalElement(LexicalElementType.LeftParenthesis, "(", 8),
                new LexicalElement(LexicalElementType.Or, "or", 9),
                new LexicalElement(LexicalElementType.RightParenthesis, ")", 11),
                new LexicalElement(LexicalElementType.Near, "near", 12),
                new LexicalElement(LexicalElementType.LeftParenthesis, "(", 16),
                new LexicalElement(LexicalElementType.Term, "word", 17)
            };

            var elements = Lexer.Tokenize(input);

            Assert.True(TestsHelper.IsEqual(elements, expectedElements));
        }
        
        [Fact]
        public void ComplexTokenization_CommasAreValidSeparators()
        {
            var input = "and,not,or,near,word";

            var expectedElements = new[]
            {
                new LexicalElement(LexicalElementType.And, "and", 1),
                new LexicalElement(LexicalElementType.And, ",", 4),
                new LexicalElement(LexicalElementType.Not, "not", 5),
                new LexicalElement(LexicalElementType.And, ",", 8),
                new LexicalElement(LexicalElementType.Or, "or", 9),
                new LexicalElement(LexicalElementType.And, ",", 11),
                new LexicalElement(LexicalElementType.Near, "near", 12),
                new LexicalElement(LexicalElementType.And, ",", 16),
                new LexicalElement(LexicalElementType.Term, "word", 17)
            };

            var elements = Lexer.Tokenize(input);

            Assert.True(TestsHelper.IsEqual(elements, expectedElements));
        }

        [Fact]
        public void ComplexTokenization_DoubleQuoteAndParenthesesMix1()
        {
            var input = "a\"b(c\"d)e\"f(g\"h(i";

            var expectedElements = new[]
            {
                new LexicalElement(LexicalElementType.Term, "a b(c d", 1),
                new LexicalElement(LexicalElementType.RightParenthesis, ")", 8),
                new LexicalElement(LexicalElementType.Term, "e f(g h", 9),
                new LexicalElement(LexicalElementType.LeftParenthesis, "(", 16),
                new LexicalElement(LexicalElementType.Term, "i", 17)
            };

            var elements = Lexer.Tokenize(input);

            Assert.True(TestsHelper.IsEqual(elements, expectedElements));
        }

        [Fact]
        public void ComplexTokenization_DoubleQuoteAndParenthesesMix2()
        {
            var input = "a(b\"c)d\"e(f\"g)h\"i";

            var expectedElements = new[]
            {
                new LexicalElement(LexicalElementType.Term, "a", 1),
                new LexicalElement(LexicalElementType.LeftParenthesis, "(", 2),
                new LexicalElement(LexicalElementType.Term, "b c)d e", 3),
                new LexicalElement(LexicalElementType.LeftParenthesis, "(", 10),
                new LexicalElement(LexicalElementType.Term, "f g)h i", 11)
            };

            var elements = Lexer.Tokenize(input);

            Assert.True(TestsHelper.IsEqual(elements, expectedElements));
        }

        [Fact]
        public void ComplexTokenization_LongValidQuery()
        {
            var input = "android , no AND (oracl* OR C++ OR C99) NOT iphone,less OR   \"  hey  baby  *\"   AND phone NEAR appl*, more";

            var expectedElements = new[]
            {
                new LexicalElement(LexicalElementType.Term, "android", 1),
                new LexicalElement(LexicalElementType.And, ",", 9),
                new LexicalElement(LexicalElementType.Term, "no", 11),
                new LexicalElement(LexicalElementType.And, "AND", 14),
                new LexicalElement(LexicalElementType.LeftParenthesis, "(", 18),
                new LexicalElement(LexicalElementType.Term, "oracl*", 19),
                new LexicalElement(LexicalElementType.Or, "OR", 26),
                new LexicalElement(LexicalElementType.Term, "C++", 29),
                new LexicalElement(LexicalElementType.Or, "OR", 33),
                new LexicalElement(LexicalElementType.Term, "C99", 36),
                new LexicalElement(LexicalElementType.RightParenthesis, ")", 39),
                new LexicalElement(LexicalElementType.Not, "NOT", 41),
                new LexicalElement(LexicalElementType.Term, "iphone", 45),
                new LexicalElement(LexicalElementType.And, ",", 51),
                new LexicalElement(LexicalElementType.Term, "less", 52),
                new LexicalElement(LexicalElementType.Or, "OR", 57),
                new LexicalElement(LexicalElementType.Term, "  hey  baby  *", 63),
                new LexicalElement(LexicalElementType.And, "AND", 81),
                new LexicalElement(LexicalElementType.Term, "phone", 85),
                new LexicalElement(LexicalElementType.Near, "NEAR", 91),
                new LexicalElement(LexicalElementType.Term, "appl*", 96),
                new LexicalElement(LexicalElementType.And, ",", 101),
                new LexicalElement(LexicalElementType.Term, "more", 103)
            };

            var elements = Lexer.Tokenize(input);

            Assert.True(TestsHelper.IsEqual(elements, expectedElements));
        }
    }
}
