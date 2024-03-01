using System.Diagnostics.CodeAnalysis;
using Application.Features.CreateAuction;
using AutoFixture;
using FluentAssertions;

namespace UnitTests.Application.Features.CreateAuction;

[ExcludeFromCodeCoverage]
public class CreateAuctionCommandValidatorTests : TestsBase
{
    private readonly CreateAuctionCommandValidator _validator;

    public CreateAuctionCommandValidatorTests()
    {
        _validator = new CreateAuctionCommandValidator();
    }

    [Fact]
    public void Validate_ValidCommand_IsValid()
    {
        //Arrange
        var command = Fixture.Create<CreateAuctionCommand>();

        //Act
        var validationResult = _validator.Validate(command);

        //Assert
        validationResult.IsValid.Should().BeTrue();
    }

    [Fact]
    public void Validate_InvalidCommand_IsInvalid()
    {
        //Arrange
        var command = new CreateAuctionCommand(Guid.Empty);
        //Act
        var validationResult = _validator.Validate(command);

        //Assert
        validationResult.IsValid.Should().BeFalse();
        validationResult.Errors.Any(x => x.ErrorMessage == "Invalid Vehicle Unique Identifier")
            .Should().BeTrue();
    }
}