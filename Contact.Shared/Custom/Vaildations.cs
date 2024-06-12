using System.Globalization;
using System.Text.RegularExpressions;

namespace Contact.Shared.Custom
{
    public static class Vaildations
    {
        private static List<string> _allowedDomains = new List<string>();

        public static void SetAllowedDomains(List<string> allowedDomains)
        {
            _allowedDomains = allowedDomains;
        }
        public static bool IsEmailValid(string email)
        {
            var emailRegex = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.IgnoreCase);
            if (emailRegex.IsMatch(email))
            {
                return true;
            }
            throw new Exception("Invalid email");
        }

        public static bool IsValidPersonalCode(string personalCode)
        {
            // Check if the personal code has the correct format
            if (!Regex.IsMatch(personalCode, @"^\d{11}$"))
            {
                return false;
            }

            // Extract the birth date from the personal code
            string birthDatePart = personalCode.Substring(1, 6);
            if (!DateTime.TryParseExact(birthDatePart, "yyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None, out _))
            {
                return false;
            }

            // Calculate the checksum digit
            int[] weights = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 1 };
            int[] weights2 = { 3, 4, 5, 6, 7, 8, 9, 1, 2, 3 };

            int checksum = 0;
            for (int i = 0; i < 10; i++)
            {
                checksum += (personalCode[i] - '0') * weights[i];
            }
            checksum %= 11;

            // If the checksum is 10, calculate using the second set of weights
            if (checksum == 10)
            {
                checksum = 0;
                for (int i = 0; i < 10; i++)
                {
                    checksum += (personalCode[i] - '0') * weights2[i];
                }
                checksum %= 11;
                if (checksum == 10)
                {
                    checksum = 0;
                }
            }

            return (personalCode[10] - '0') == checksum;
        }

        public static bool IsAllowedDomain(string email)
        {
            var domain = email.Split('@').Last();
            if (!_allowedDomains.Contains(domain, StringComparer.OrdinalIgnoreCase))
            {
                throw new Exception("Email domain is not allowed");
            }
            return true;
        }

        public static bool ValidateEmail(string email)
        {
            return IsEmailValid(email) && IsAllowedDomain(email);
        }

        public static bool ValidateName(string name)
        {
            var nameRegex = new Regex(@"^[a-zA-Z]+$", RegexOptions.IgnoreCase);
            if (name.Length < 2 || name.Length > 50)
            {
                throw new Exception("Must be between 2 and 50 characters");
            }
            else if (!nameRegex.IsMatch(name))
            {
                throw new Exception("Must not contain numbers");
            }
            return true;

        }

        public static bool ValidateNoSpecialCharAndNumber(string input)
        {
            var regex = new Regex(@"^[a-zA-Z]+$", RegexOptions.IgnoreCase);
            return regex.IsMatch(input);
        }

        public static bool ValidateAtleastOneSpace(string input)
        {
            var regex = new Regex(@"\s");
            return regex.IsMatch(input);
        }

        public static bool ValidateIsAlphaNumeric(string input)
        {
            var regex = new Regex(@"^[0-9]+[A-Za-z]?$", RegexOptions.IgnoreCase);
            return regex.IsMatch(input);
        }

        public static bool ValidateApartmentNumber(string input)
        {
            var regex = new Regex(@"^[0-9]+[A-Za-z]?$", RegexOptions.IgnoreCase);
            return regex.IsMatch(input);
        }

        public static bool ValidateUsername(string username)
        {
            var nameRegex = new Regex(@"^[a-zA-Z]+$", RegexOptions.IgnoreCase);
            if (username.Length < 8 || username.Length > 20)
            {
                return false;
            }
            else if (!nameRegex.IsMatch(username))
            {
                return false;
            }
            return true;
        }

        public static bool ValidatePassword(string password)
        {
            if (password.Length < 12)
            {
                throw new Exception("Password must be at least 12 characters long");
            }

            if (password.Count(char.IsUpper) < 2)
            {
                throw new Exception("Password must contain at least 2 uppercase letters");
            }

            if (password.Count(char.IsLower) < 2)
            {
                throw new Exception("Password must contain at least 2 lowercase letters");
            }

            if (password.Count(char.IsDigit) < 2)
            {
                throw new Exception("Password must contain at least 2 digits");
            }

            var specialCharactersCount = password.Count(c => !char.IsLetterOrDigit(c));
            if (specialCharactersCount < 2)
            {
                throw new Exception("Password must contain at least 2 special characters");
            }

            if (password.Contains(' '))
            {
                throw new Exception("Password must not contain spaces");
            }

            if (Regex.IsMatch(password, @"(\w)\1{2,}"))
            {
                throw new Exception("Password must not contain more than 2 consecutive identical characters");
            }

            return true;

        }
        public static int GetAge(DateTime dateOfBirth)
        {

            var today = DateTime.Today;
            var age = today.Year - dateOfBirth.Year;
            if (dateOfBirth.Date > today.AddYears(-age))
            {
                age--;
            }
            return age;
        }

        public static bool IsDateOfBirthValid(DateTime dateOfBirth)
        {
            int age = GetAge(dateOfBirth);
            if (age < 0 || age > 150)
            {
                return false;
            }
            return true;
        }

        public static bool ValidatePersonalCode(string personalCode)
        {
            if (!IsValidPersonalCode(personalCode))
            {
                throw new Exception("Invalid personal code");
            }
            return true;
        }
    }
}
