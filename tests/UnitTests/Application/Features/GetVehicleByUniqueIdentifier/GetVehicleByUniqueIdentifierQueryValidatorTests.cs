using System.Diagnostics.CodeAnalysis;
using Application.Features.GetVehicleByUniqueIdentifier;
using AutoFixture;
using FluentAssertions;

namespace UnitTests.Application.Features.GetVehicleByUniqueIdentifier;

[ExcludeFromCodeCoverage]
public class GetVehicleByUniqueIdentifierQueryValidatorTests : TestsBase
{
    private readonly GetVehicleByUniqueIdentifierQueryValidator _validator;

    public GetVehicleByUniqueIdentifierQueryValidatorTests()
    {
        _validator = new GetVehicleByUniqueIdentifierQueryValidator();
    }

    [Fact]
    public void Validate_EmtpyGuid_ReturnsFail()
    {
        //Arrange
        var uniqueIdentifier = Guid.Empty;
        var query = new GetVehicleByUniqueIdentifierQuery(uniqueIdentifier);
        
        //Act
        var validationResult = _validator.Validate(query);
        
        //Assert
        validationResult.IsValid.Should().BeFalse();
        validationResult.Errors.Count.Should().Be(1);
        validationResult.Errors.Any(x => x.ErrorMessage == "Invalid Vehicle Unique Identifier");
    }

    [Fact]
    public void Validate_CorrectGuid_ReturnsFail()
    {
        //Arrange
        var uniqueIdentifier = Fixture.Create<Guid>();
        var query = new GetVehicleByUniqueIdentifierQuery(uniqueIdentifier);
        
        //Act
        var validationResult = _validator.Validate(query);
        
        //Assert
        validationResult.IsValid.Should().BeTrue();
    }
}