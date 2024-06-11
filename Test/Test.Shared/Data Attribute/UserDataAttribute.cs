using AutoFixture;
using AutoFixture.Xunit2;
using Test.Shared.SpecimenBuilder;

namespace Contact.Domain.Test.Data_Attribute
{
    public class UserDataAttribute : AutoDataAttribute
    {
        public UserDataAttribute() : base(() =>
        {
            var fixture = new Fixture();
            fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
                .ForEach(b => fixture.Behaviors.Remove(b));
            fixture.Behaviors.Add(new OmitOnRecursionBehavior());

            fixture.Customizations.Add(new UserSpecimenBuilder());
            return fixture;
        })
        { }
    }
}
