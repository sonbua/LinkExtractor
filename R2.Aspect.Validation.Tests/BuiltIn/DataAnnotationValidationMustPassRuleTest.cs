using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using R2.Aspect.Validation.BuiltIn;
using Xunit;

namespace R2.Aspect.Validation.Tests.BuiltIn
{
    public class DataAnnotationValidationMustPassRuleTest
    {
        [Fact]
        public async Task Test()
        {
            // arrange
            var rule = new DataAnnotationValidationMustPassRule<HasNestedObjectToValidateCommand>();

            var command = new HasNestedObjectToValidateCommand
            {
                NestedObject = new NestedType()
            };

            // act
            var testDelegate = new Func<Task>(async () => await rule.TestAsync(command));

            // assert
            await Assert.ThrowsAsync<CompositeValidationException>(testDelegate);
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