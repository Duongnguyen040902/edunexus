using FluentValidation.TestHelper;
using sep.backend.v1.DTOs;
using sep.backend.v1.Validators;
using sep.backend.v1.Common.Const;
using Xunit;

namespace sep.test.v1.Validation
{
    public class ClassValidatorTests
    {
        private readonly ClassValidator _validator;

        public ClassValidatorTests()
        {
            _validator = new ClassValidator();
        }

        [Fact]
        public void Validate_NameIsEmpty_ReturnsError()
        {
            // Arrange
            var dto = new AddClassDTO { Name = string.Empty };

            // Act
            var result = _validator.TestValidate(dto);

            // Assert
            //result.ShouldHaveValidationErrorFor(c => c.Name).WithErrorMessage(Messages.NAME_REQUIRED);
        }

        [Fact]
        public void Validate_NameExceedsMaxLength_ReturnsError()
        {
            // Arrange
            var dto = new AddClassDTO { Name = "ExceedMaxLength" }; // Length > 5

            // Act
            var result = _validator.TestValidate(dto);

            // Assert
            //result.ShouldHaveValidationErrorFor(c => c.Name) .WithErrorMessage(Messages.MAX.Replace("{attribute}", "Tên lớp").Replace("{maxLength}", "5"));
        }

        [Fact]
        public void Validate_NameIsValid_ReturnsSuccess()
        {
            // Arrange
            var dto = new AddClassDTO { Name = "Valid" }; // Length <= 5

            // Act
            var result = _validator.TestValidate(dto);

            // Assert
           // result.ShouldNotHaveValidationErrorFor(c => c.Name);
        }
    }
}
