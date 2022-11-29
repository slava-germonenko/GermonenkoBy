using AutoMapper;
using GermonenkoBy.Contacts.Api.Mapping.Profiles;

namespace GermonenkoBy.Contacts.Tests.Models;

[TestClass]
public class MappingProfileConfigurationTests
{
    [TestMethod]
    public void AssertContactEmailResponseProfileIsValid_ShouldNot_ThrowException()
    {
        var configuration = new MapperConfiguration(options =>
        {
            options.AddProfile<ContactEmailResponseProfile>();
        });

        configuration.AssertConfigurationIsValid();
    }
}