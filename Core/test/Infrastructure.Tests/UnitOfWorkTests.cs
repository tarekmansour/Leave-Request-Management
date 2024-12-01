using FluentAssertions;
using NSubstitute;

namespace Infrastructure.Tests;
public class UnitOfWorkTests
{
    private readonly ApplicationDbContext _dbContext;
    private readonly UnitOfWork _unitOfWork;

    public UnitOfWorkTests()
    {
        _dbContext = Substitute.For<ApplicationDbContext>();
        _unitOfWork = new UnitOfWork(_dbContext);
    }

    [Fact(DisplayName = "New should throw ArgumentNullException when dbContext is null")]
    public void Constructor_ShouldThrowArgumentNullException_WhenDbContextIsNull()
    {
        // Arrange & Act
        Action act = () => new UnitOfWork(null!);

        // Assert
        act.Should().Throw<ArgumentNullException>()
            .WithMessage("*dbContext*");
    }

    [Fact(DisplayName = "PersistChangesAsync should call SaveChangesAsync on the dbContext")]
    public async Task PersistChangesAsync_ShouldCallSaveChangesAsync_OnDbContext()
    {
        // Arrange
        _dbContext.SaveChangesAsync().Returns(Task.FromResult(1));

        // Act
        await _unitOfWork.PersistChangesAsync();

        // Assert
        await _dbContext.Received(1).SaveChangesAsync();
    }

    [Fact(DisplayName = "PersistChangesAsync should propagate exceptions from dbContext")]
    public async Task PersistChangesAsync_ShouldPropagateExceptions_FromDbContext()
    {
        // Arrange
        _dbContext.SaveChangesAsync().Returns(Task.FromException<int>(new InvalidOperationException("Database error")));

        // Act
        Func<Task> act = async () => await _unitOfWork.PersistChangesAsync();

        // Assert
        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("Database error");
    }
}
