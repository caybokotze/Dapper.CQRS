using NExpect;
using NUnit.Framework;
using static NExpect.Expectations;

namespace Dapper.CQRS.Tests
{
    [TestFixture]
    public class CaseHelperTests
    {
        [TestCase("TheQuickBrownFoxJumps")]
        public void PascalCase_DoesConvert_ToKebabCase(string text)
        {
            Expect(text.ConvertCase(Case.KebabCase)).To.Equal("the-quick-brown-fox-jumps");
        }

        [TestCase("TheQuickBrownFoxJumps")]
        public void PascalCase_DoesConvert_ToCamelCase(string text)
        {
            Expect(text.ConvertCase(Case.CamelCase)).To.Equal("theQuickBrownFoxJumps");
        }

        [TestCase("the_quick_brown_fox")]
        public void SnakeCase_DoesConvert_ToCamelCase(string text)
        {
            Expect(text.ConvertCase(Case.CamelCase)).To.Equal("theQuickBrownFox");
        }

        [TestCase("TheQuickBrownFoxJumps")]
        public void SnakeCase_DoesConvert_ToKebabCase(string text)
        {
            Expect(text.ConvertCase(Case.KebabCase)).To.Equal("the-quick-brown-fox-jumps");
        }

        [TestCase("the-quick-brown-fox-jumps-over")]
        public void KebabCase_DoesConvert_ToPascalCase(string text)
        {
            Expect(text.ConvertCase(Case.PascalCase)).To.Equal("TheQuickBrownFoxJumpsOver");
        }
    }
}