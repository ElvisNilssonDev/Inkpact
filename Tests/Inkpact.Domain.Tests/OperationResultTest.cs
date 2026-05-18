using Domain.Common;
using FluentAssertions;
using Xunit;

namespace Inkpact.Domain.Tests.Common;

public class OperationResultTests
{
    [Fact]
    public void Success_WithData_ReturnsIsSuccessTrueAndData()
    {
        var guid = Guid.NewGuid();

        var result = OperationResult<Guid>.Success(guid);

        result.IsSuccess.Should().BeTrue();
        result.Data.Should().Be(guid);
        result.Status.Should().Be(OperationResultStatus.Ok);
    }

    [Fact]
    public void Failure_WithMessage_ReturnsIsSuccessFalseAndCorrectStatus()
    {
        var result = OperationResult<Guid>.Failure("Not found", OperationResultStatus.NotFound);

        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be("Not found");
        result.Status.Should().Be(OperationResultStatus.NotFound);
    }
}