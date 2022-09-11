using FluentAssertions;
using Grenadiers;
using NUnit.Framework;
using System;

namespace Guard_specs;

public class Finite
{
    [TestCase(double.Epsilon)]
    [TestCase(1000)]
    [TestCase(0)]
    [TestCase(-6.9)]
    public void guards(double parameter)
        => Guard.Finite(parameter, nameof(parameter)).Should().Be(parameter);

    [TestCase(null)]
    [TestCase(double.NegativeInfinity)]
    [TestCase(double.PositiveInfinity)]
    [TestCase(double.NaN)]
    public void blocks(double? parameter)
    {
        Func<double> guard = () => Guard.Finite(parameter, nameof(parameter));
        guard.Should().Throw<ArgumentException>();
    }
}

public class Positive
{
    [TestCase(double.PositiveInfinity)]
    [TestCase(double.Epsilon)]
    [TestCase(1000)]
    public void guards(double parameter)
        => Guard.Positive(parameter, nameof(parameter)).Should().Be(parameter);

    [TestCase(double.NegativeInfinity)]
    [TestCase(0)]
    [TestCase(-double.Epsilon)]
    [TestCase(-6.9)]
    public void blocks(double parameter)
    {
        Func<double> guard = () => Guard.Positive(parameter, nameof(parameter));
        guard.Should().Throw<ArgumentOutOfRangeException>();
    }
}
