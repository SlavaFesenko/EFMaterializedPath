﻿using System.Linq;
using System.Threading.Tasks;
using EFMaterializedPath.Test.Mocks;
using FluentAssertions;
using Xunit;

namespace EFMaterializedPath.Test
{
    // ReSharper disable once InconsistentNaming
    public class TestTreeRepository_GetParent
    {
        private readonly TestDbContext dbContext;
        private readonly TreeRepository<TestDbContext, Category> repository;

        public TestTreeRepository_GetParent()
        {
            dbContext = TestHelpers.CreateTestDb();
            repository = new TreeRepository<TestDbContext, Category>(dbContext);
            
            TestHelpers.CreateTestCategoryTree(dbContext, repository);

            //         ┌───────1───────┐   
            //         │       │       │ 
            //     ┌───2───┐   3       4
            //     │       │           │
            //     5       6           8
            //     │       │ 
            //     9       10
            //     │
            //     7
        }

        [Fact]
        public async Task GetParentOnRoot()
        {
            var root = await dbContext.Categories.FindAsync(1);
            (await repository.GetParentAsync(root)).Should().BeNull();
        }
        
        [Fact]
        public async Task GetParentIntermediateNode()
        {
            var five = await dbContext.Categories.FindAsync(5);
            (await repository.GetParentAsync(five))!.Id.Should().Be(2);
        }
    }
}