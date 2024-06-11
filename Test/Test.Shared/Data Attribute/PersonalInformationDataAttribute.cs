using AutoFixture;
using AutoFixture.AutoMoq;
using AutoFixture.Xunit2;
using AutoMapper;
using Contact.Infrastructure.Mapper;
using Test.Shared.SpecimenBuilder;


namespace Contact.Domain.Test.Data_Attribute
{
    public class PersonalInformationDataAttribute : AutoDataAttribute
    {
        public PersonalInformationDataAttribute() : base(() =>
        {
            var fixture = new Fixture();
            fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
                .ForEach(b => fixture.Behaviors.Remove(b));
            fixture.Behaviors.Add(new OmitOnRecursionBehavior());


            fixture.Customizations.Add(new PersonalInformationSpecimenBuilder());
            fixture.Customizations.Add(new IFormFileSpecimenBuilder());
            fixture.Customizations.Add(new AddressSpecimenBuilder());
            fixture.Customizations.Add(new UserSpecimenBuilder());
            fixture.Customize(new AutoMoqCustomization());

            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<UserMapper>();
            });
            var mapper = config.CreateMapper();

            fixture.Register<IMapper>(() => mapper);

            return fixture;
        })
        { }
    }
}
