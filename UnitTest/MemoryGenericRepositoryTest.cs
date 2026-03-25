using CoreApp.Entities;
using CoreApp.Repositories;
using Infrastructure.Memory;
using Xunit;

namespace UnitTest;

public class MemoryGenericRepositoryTest
{
    private IGenericRepositoryAsync<Person> _repo = new MemoryGenericRepository<Person>();

    [Fact]
    public async Task AddPersonTestAsync()
    {
        // Arrange
        var expected = new Person()
        {
            FirstName = "Adam",
            LastName = "Kowalski"
        };

        // Act
        await _repo.AddAsync(expected);

        // Assert
        var actual = await _repo.FindByIdAsync(expected.Id);

        Assert.Equal(expected.Id, actual?.Id);
        Assert.Equal(expected.FirstName, actual?.FirstName);
    }
}