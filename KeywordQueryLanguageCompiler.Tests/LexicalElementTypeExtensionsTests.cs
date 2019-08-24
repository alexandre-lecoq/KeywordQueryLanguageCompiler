namespace KeywordQueryLanguageCompiler.Tests
{
    using System;

    using Xunit;

    public class LexicalElementTypeExtensionsTests
    {
        [Fact]
        public void AndTypeAssociativityIsKnown()
        {
            var type = LexicalElementType.And;
            type.GetAssociativity();
        }

        [Fact]
        public void LeftParenthesisTypeAssociativityIsKnown()
        {
            var type = LexicalElementType.LeftParenthesis;
            type.GetAssociativity();
        }

        [Fact]
        public void NearTypeAssociativityIsKnown()
        {
            var type = LexicalElementType.Near;
            type.GetAssociativity();
        }

        [Fact]
        public void NotTypeAssociativityIsKnown()
        {
            var type = LexicalElementType.Not;
            type.GetAssociativity();
        }

        [Fact]
        public void OrTypeAssociativityIsKnown()
        {
            var type = LexicalElementType.Or;
            type.GetAssociativity();
        }

        [Fact]
        public void RightParenthesisTypeAssociativityIsKnown()
        {
            var type = LexicalElementType.RightParenthesis;
            type.GetAssociativity();
        }

        [Fact]
        public void TermTypeAssociativityIsKnown()
        {
            var type = LexicalElementType.Term;
            type.GetAssociativity();
        }

        [Fact]
        public void UnknownTypeAssociativityIsUnknown()
        {
            var type = (LexicalElementType)999999;
            Assert.Throws<NotImplementedException>(() => type.GetAssociativity());
        }

        [Fact]
        public void AndTypePrecedenceIsKnown()
        {
            var type = LexicalElementType.And;
            type.GetPrecedence();
        }

        [Fact]
        public void LeftParenthesisTypePrecedenceIsKnown()
        {
            var type = LexicalElementType.LeftParenthesis;
            type.GetPrecedence();
        }

        [Fact]
        public void NearTypePrecedenceIsKnown()
        {
            var type = LexicalElementType.Near;
            type.GetPrecedence();
        }

        [Fact]
        public void NotTypePrecedenceIsKnown()
        {
            var type = LexicalElementType.Not;
            type.GetPrecedence();
        }

        [Fact]
        public void OrTypePrecedenceIsKnown()
        {
            var type = LexicalElementType.Or;
            type.GetPrecedence();
        }

        [Fact]
        public void RightParenthesisTypePrecedenceIsKnown()
        {
            var type = LexicalElementType.RightParenthesis;
            type.GetPrecedence();
        }

        [Fact]
        public void TermTypePrecedenceIsKnown()
        {
            var type = LexicalElementType.Term;
            type.GetPrecedence();
        }

        [Fact]
        public void UnknownTypePrecedenceIsUnknown()
        {
            var type = (LexicalElementType)999999;
            Assert.Throws<NotImplementedException>(() => type.GetPrecedence());
        }
        
        [Fact]
        public void AndTypeIsOperatorIsKnown()
        {
            var type = LexicalElementType.And;
            Assert.True(type.IsOperator());
        }

        [Fact]
        public void LeftParenthesisTypeIsOperatorIsKnown()
        {
            var type = LexicalElementType.LeftParenthesis;
            Assert.False(type.IsOperator());
        }

        [Fact]
        public void NearTypeIsOperatorIsKnown()
        {
            var type = LexicalElementType.Near;
            Assert.True(type.IsOperator());
        }

        [Fact]
        public void NotTypeIsOperatorIsKnown()
        {
            var type = LexicalElementType.Not;
            Assert.True(type.IsOperator());
        }

        [Fact]
        public void OrTypeIsOperatorIsKnown()
        {
            var type = LexicalElementType.Or;
            Assert.True(type.IsOperator());
        }

        [Fact]
        public void RightParenthesisTypeIsOperatorIsKnown()
        {
            var type = LexicalElementType.RightParenthesis;
            Assert.False(type.IsOperator());
        }

        [Fact]
        public void TermTypeIsOperatorIsKnown()
        {
            var type = LexicalElementType.Term;
            Assert.False(type.IsOperator());
        }

        [Fact]
        public void UnknownTypeIsOperatorIsUnknown()
        {
            var type = (LexicalElementType)999999;
            Assert.Throws<NotImplementedException>(() => type.IsOperator());
        }
        
        [Fact]
        public void AndTypeOperandCountIsKnown()
        {
            var type = LexicalElementType.And;
            Assert.Equal(2, type.GetOperandCount());
        }

        [Fact]
        public void LeftParenthesisTypeOperandCountIsKnown()
        {
            var type = LexicalElementType.LeftParenthesis;
            Assert.Throws<NotSupportedException>(() => type.GetOperandCount());
        }

        [Fact]
        public void NearTypeOperandCountIsKnown()
        {
            var type = LexicalElementType.Near;
            Assert.Equal(2, type.GetOperandCount());
        }

        [Fact]
        public void NotTypeOperandCountIsKnown()
        {
            var type = LexicalElementType.Not;
            Assert.Equal(2, type.GetOperandCount());
        }

        [Fact]
        public void OrTypeOperandCountIsKnown()
        {
            var type = LexicalElementType.Or;
            Assert.Equal(2, type.GetOperandCount());
        }

        [Fact]
        public void RightParenthesisTypeOperandCountIsKnown()
        {
            var type = LexicalElementType.RightParenthesis;
            Assert.Throws<NotSupportedException>(() => type.GetOperandCount());
        }

        [Fact]
        public void TermTypeOperandCountIsKnown()
        {
            var type = LexicalElementType.Term;
            Assert.Throws<NotSupportedException>(() => type.GetOperandCount());
        }

        [Fact]
        public void UnknownTypeOperandCountIsUnknown()
        {
            var type = (LexicalElementType)999999;
            Assert.Throws<NotImplementedException>(() => type.GetOperandCount());
        }
    }
}
