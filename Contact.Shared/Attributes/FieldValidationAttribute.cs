using Contact.Shared.Custom;
using PhoneNumbers;
using System.ComponentModel.DataAnnotations;

namespace Contact.Shared.Attributes
{
    public class FieldValidationAttribute : ValidationAttribute
    {
        private readonly string _fieldName;
        public FieldValidationAttribute(string fieldName)
        {
            _fieldName = fieldName;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            try
            {
                if (value is string input)
                {
                    switch (_fieldName)
                    {
                        case "Email":
                            if (!Vaildations.ValidateEmail(input))
                            {
                                return new ValidationResult(GetErrorMessage($"Invalid {_fieldName}"));
                            }
                            break;
                        case "PhoneNumber":
                            var phoneUtil = PhoneNumberUtil.GetInstance();
                            try
                            {
                                var parsedNumber = phoneUtil.Parse(input, "LT");
                                bool isValid = phoneUtil.IsValidNumber(parsedNumber);
                                var numberType = phoneUtil.GetRegionCodeForNumber(parsedNumber);
                                if (!isValid || numberType != "LT")
                                {
                                    return new ValidationResult(GetErrorMessage($"Invalid {_fieldName}. {_fieldName} must be a valid Lithuanian number"));
                                }
                            }
                            catch (NumberParseException)
                            {
                                return new ValidationResult(GetErrorMessage($"Invalid {_fieldName}. {_fieldName} must be a valid Lithuanian number"));
                            }
                            break;
                        case "FirstName":
                            if (!Vaildations.ValidateName(input))
                            {
                                return new ValidationResult(GetErrorMessage($"Invalid {_fieldName}"));
                            }
                            break;
                        case "LastName":
                            if (!Vaildations.ValidateName(input))
                            {
                                return new ValidationResult(GetErrorMessage($"Invalid {_fieldName}"));
                            }
                            break;
                        case "PersonalCode":
                            if (!Vaildations.ValidatePersonalCode(input))
                            {
                                return new ValidationResult(GetErrorMessage($"Invalid {_fieldName}"));
                            }
                            break;
                        case "City":
                            if (!Vaildations.ValidateNoSpecialCharAndNumber(input))
                            {
                                return new ValidationResult(GetErrorMessage($"Invalid {_fieldName}"));
                            }
                            break;
                        case "Street":
                            if (!Vaildations.ValidateAtleastOneSpace(input))
                            {
                                return new ValidationResult(GetErrorMessage($"Invalid {_fieldName}"));
                            }
                            break;
                        case "HouseNumber":
                            if (!Vaildations.ValidateIsAlphaNumeric(input))
                            {
                                return new ValidationResult(GetErrorMessage($"Invalid {_fieldName}"));
                            }
                            break;
                        case "PostalCode":
                            if (!Vaildations.ValidateIsAlphaNumeric(input))
                            {
                                return new ValidationResult(GetErrorMessage($"Invalid {_fieldName}"));
                            }
                            break;
                        case "ApartmentNumber":
                            if (input.Length > 0 && !Vaildations.ValidateApartmentNumber(input))
                            {
                                return new ValidationResult(GetErrorMessage($"Invalid {_fieldName}"));
                            }
                            break;
                        case "Username":
                            if (!Vaildations.ValidateUsername(input))
                            {
                                return new ValidationResult(GetErrorMessage("Username must be between 8 and 20 chars, only Alphabets"));
                            }
                            break;
                        case "Password":
                            if (!Vaildations.ValidatePassword(input))
                            {
                                return new ValidationResult(GetErrorMessage("Invalid password entered"));
                            }
                            break;
                    }
                }
                else if (value is DateTime dateOfBirth)
                {
                    if (_fieldName == "DateofBirth" && !Vaildations.IsDateOfBirthValid(dateOfBirth))
                    {
                        return new ValidationResult(GetErrorMessage("Invalid date of birth"));
                    }
                }
                else if (_fieldName != "ApartmentNumber" && value is null)
                {
                    return new ValidationResult(GetErrorMessage($"Invalid {_fieldName}"));
                }
                return ValidationResult.Success!;
            }
            catch (Exception ex)
            {

                return new ValidationResult(ex.Message);
            }
        }

        public string GetErrorMessage(string message)
        {
            return $"{message}";
        }
    }
}
