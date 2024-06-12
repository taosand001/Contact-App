using Contact.Shared.Enum;

namespace Contact.Infrastructure.Helpers
{
    public static class PersonalCodeExtractor
    {
        public static (DateTime birthDate, GenderType gender) ExtractBirthDateFromPersonalCode(string personalCode)
        {
            if (string.IsNullOrWhiteSpace(personalCode) || personalCode.Length != 11)
            {
                throw new ArgumentException("Invalid personal code.");
            }

            // Extracting the birth date parts from the personal code
            int centuryIndicator = int.Parse(personalCode.Substring(0, 1));
            int year = int.Parse(personalCode.Substring(1, 2));
            int month = int.Parse(personalCode.Substring(3, 2));
            int day = int.Parse(personalCode.Substring(5, 2));

            // Determine the century based on the century indicator
            int century;
            GenderType gender;
            switch (centuryIndicator)
            {
                case 1:
                    century = 1800;
                    gender = GenderType.Male;
                    break;
                case 2:
                    century = 1800;
                    gender = GenderType.Female;
                    break;
                case 3:
                    century = 1900;
                    gender = GenderType.Male;
                    break;
                case 4:
                    century = 1900;
                    gender = GenderType.Female;
                    break;
                case 5:
                    century = 2000;
                    gender = GenderType.Male;
                    break;
                case 6:
                    century = 2000;
                    gender = GenderType.Female;
                    break;
                default:
                    throw new ArgumentException("Invalid century indicator in personal code.");
            }

            year += century;

            return (new DateTime(year, month, day), gender);
        }
    }
}
