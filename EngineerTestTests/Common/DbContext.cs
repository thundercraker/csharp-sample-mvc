using System;
using EngineerTest.Data;
using Microsoft.EntityFrameworkCore;

namespace EngineerTestTests.Common
{
    public static class DbContext 
    {
        public static ApplicationDbContext GetTestContext(
            string dbName = null)
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: dbName ?? Guid.NewGuid().ToString())
                .Options;
            return new ApplicationDbContext(options);
        }
    }
}