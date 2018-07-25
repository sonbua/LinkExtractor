using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using R2.Aspect.Validation.BuiltIn;
using Xunit;

namespace R2.Aspect.Validation.Tests.BuiltIn
{
    public class ValidateCollectionAttributeTest
    {
        [Fact]
        public void PropertyIsNotOfTypeIEnumerable_ThrowsValidationException()
        {
            // arrange
            var command = new ValidateCollectionThatIsNotOfTypeIEnumerableCommand
            {
                NonIEnumerableProp = 0
            };

            var context = new ValidationContext(command);
            var results = new List<ValidationResult>();

            // act
            var isValid = Validator.TryValidateObject(command, context, results, validateAllProperties: true);

            // assert
            Assert.False(isValid);
            Assert.Single(results);
        }

        [Fact]
        public void CollectionsThatHaveInvalidElements_ReturnsInvalidWithCorrectCount()
        {
            // arrange
            var command = new ValidateNestedObjectsThatImplementIEnumerableCommand
            {
                NestedListObject = new List<NestedType> {new NestedType()},
                NestedIListObject = new List<NestedType> {new NestedType()},
                NestedICollectionObject = new List<NestedType> {new NestedType()},
                NestedIEnumerableObject = new List<NestedType> {new NestedType()},
            };

            var context = new ValidationContext(command);
            var results = new List<ValidationResult>();

            // act
            var isValid = Validator.TryValidateObject(command, context, results, validateAllProperties: true);

            // assert
            Assert.False(isValid);
            Assert.Equal(4, results.Count);
        }

        private class ValidateCollectionThatIsNotOfTypeIEnumerableCommand
        {
            [ValidateCollection]
            public int NonIEnumerableProp { get; set; }
        }

        private class ValidateNestedObjectsThatImplementIEnumerableCommand
        {
            [ValidateCollection]
            public List<NestedType> NestedListObject { get; set; }

            [ValidateCollection]
            public IList<NestedType> NestedIListObject { get; set; }

            [ValidateCollection]
            public ICollection<NestedType> NestedICollectionObject { get; set; }

            [ValidateCollection]
            public IEnumerable<NestedType> NestedIEnumerableObject { get; set; }
        }

        private class NestedType
        {
            [Required]
            public string RequiredProp { get; set; }
        }
    }
}