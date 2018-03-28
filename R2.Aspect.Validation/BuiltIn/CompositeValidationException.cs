using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace R2.Aspect.Validation.BuiltIn
{
    [Serializable]
    public class CompositeValidationException : ValidationException
    {
        public CompositeValidationException(List<ValidationResult> validationResults)
            : base(validationResults[0], validatingAttribute: null, value: null)
        {
            ValidationResults = validationResults;
        }

        public CompositeValidationException()
        {
        }

        public CompositeValidationException(string message)
            : base(message)
        {
        }

        public CompositeValidationException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        protected CompositeValidationException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            ValidationResults =
                (List<ValidationResult>) info.GetValue(
                    nameof(ValidationResults),
                    typeof(List<ValidationResult>)
                );
        }

        public List<ValidationResult> ValidationResults { get; }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            if (info == null)
            {
                throw new ArgumentNullException(nameof(info));
            }

            info.AddValue(nameof(ValidationResults), ValidationResults, typeof(List<ValidationResult>));

            base.GetObjectData(info, context);
        }
    }
}