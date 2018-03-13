using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
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
            var command = new HasNestedObjectToValidateCommand
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
    }

    public class HasNestedObjectToValidateCommand
    {
        [ValidateObject]
        public NestedType NestedObject { get; set; }
    }

    public class NestedType
    {
        [Required]
        public string RequiredProp { get; set; }
    }
}