using Fiap.Banco.API.Data;
using Microsoft.EntityFrameworkCore;

namespace Fiap.Banco.API.Tests.TestHelpers;

public static class TestDbContextFactory
{
    public static AppDbContext Create()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new AppDbContext(options);
    }
}
