using System.Diagnostics.CodeAnalysis;
using AutoFixture;
using AutoFixture.AutoNSubstitute;

namespace UnitTests;

[ExcludeFromCodeCoverage]
public class TestsBase
{
    public IFixture Fixture;

    public TestsBase()
    {
        Fixture = new Fixture();
        Fixture.Customize(new AutoNSubstituteCustomization());
        Fixture.Behaviors.Remove(new ThrowingRecursionBehavior());
        Fixture.Behaviors.Add(new OmitOnRecursionBehavior());
    }
}