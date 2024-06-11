using AutoFixture;
using AutoFixture.Xunit2;
using Test.Shared.SpecimenBuilder;

namespace Test.Shared.Data_Attribute
{
    public class FormFileDataAttribute : AutoDataAttribute
    {
        public FormFileDataAttribute() : base(() =>
        {
            var fixture = new Fixture();
            fixture.Customizations.Add(new IFormFileSpecimenBuilder());
            return fixture;
        })
        { }
    }
}
