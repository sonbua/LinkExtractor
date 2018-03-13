using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using R2.Aspect.Validation.BuiltIn;
using Xunit;

namespace R2.Aspect.Validation.Tests.BuiltIn
{
    public class ValidateObjectAttributeTest
    {
        [Fact]
        public void RequiredPropertyThatIsNull_ReturnsInvalid()
        {
            // arrange
            var command = new ValidateNestedObjectCommand
            {
                NestedObject = new NestedType()
            };

            var context = new ValidationContext(command);
            var results = new List<ValidationResult>();

            // act
            var isValid = Validator.TryValidateObject(command, context, results, validateAllProperties: true);

            // assert
            Assert.False(isValid);
        }

        private class ValidateNestedObjectCommand
        {
            [ValidateObject]
            public NestedType NestedObject { get; set; }
        }

        private class NestedType
        {
            [Required]
            public string RequiredProp { get; set; }
        }
    }
}