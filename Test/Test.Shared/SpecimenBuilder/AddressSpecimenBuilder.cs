using AutoFixture.Kernel;
using Contact.Shared.Model;

namespace Test.Shared.SpecimenBuilder
{
    public class AddressSpecimenBuilder : ISpecimenBuilder
    {
        public object Create(object request, ISpecimenContext context)
        {
            if (request is Type type && type == typeof(Address))
            {
                return new Address
                {
                    Street = "Main St",
                    City = "Sample City",
                    PostalCode = "12345",
                    HouseNumber = "123",
                    ApartmentNumber = "1A",
                };
            }
            return new NoSpecimen();
        }
    }
}
