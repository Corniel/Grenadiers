using FluentAssertions;
using Grenadiers;
using NUnit.Framework;
using Qowaiv.TestTools.IO;
using System;
using System.IO;

namespace Guard_specs;

public class Finite
{
    [TestCase(double.Epsilon)]
    [TestCase(1000)]
    [TestCase(0)]
    [TestCase(-6.9)]
    public void guards(double parameter)
        => Guard.Finite(parameter).Should().Be(parameter);

    [TestCase(null)]
    [TestCase(double.NegativeInfinity)]
    [TestCase(double.PositiveInfinity)]
    [TestCase(double.NaN)]
    public void blocks(double? parameter)
    {
        Func<double> guard = () => Guard.Finite(parameter);
        guard.Should().Throw<ArgumentException>();
    }
}

public class Positive
{
    [TestCase(double.PositiveInfinity)]
    [TestCase(double.Epsilon)]
    [TestCase(1000)]
    public void guards(double parameter)
        => Guard.Positive(parameter).Should().Be(parameter);

    [TestCase(double.NegativeInfinity)]
    [TestCase(0)]
    [TestCase(-double.Epsilon)]
    [TestCase(-6.9)]
    public void blocks(double parameter)
    {
        Func<double> guard = () => Guard.Positive(parameter);
        guard.Should().Throw<ArgumentOutOfRangeException>();
    }
}

public class Exists
{
    [Test]
    public void guard_existing_file()
    {
        using var dir = new TemporaryDirectory();
        
        var file = dir.CreateFile("empty.txt");
        using var stream = file.Create();
        stream.Flush();

        Guard.Exists(file).Should().Be(file);
    }

    [Test]
    public void blocks_non_existing_file()
    {
        var file = new FileInfo("C:/unknown.txt");

        file.Invoking(f => Guard.Exists(f))
            .Should().Throw<ArgumentException>()
            .WithMessage("Argument 'C:\\unknown.txt'does not exist. *");
    }
}
