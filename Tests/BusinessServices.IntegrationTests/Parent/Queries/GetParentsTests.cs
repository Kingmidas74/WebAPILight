using System.Linq;
using System;
namespace BusinessServices.IntegrationTests.Parents.Queries
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using BusinessServices.Modules.ParentModule;
    using Domain;
    using FluentAssertions;
    using NUnit.Framework;
    using static Testing;

    public class GetParentsTests : TestBase
    {
        [Test]
        public async Task ShouldReturnNoParents()
        {
            var query = new GetAllParentsQuery();

            var result = await SendAsync(query);

            result.Should().BeEmpty();
        }

        [Test]
        public async Task ShouldReturnAllParentsAndChildred()
        {
            await AddAsync(new Parent {
                BirthDay = DateTime.UtcNow.AddDays(-3),
                Id = Guid.NewGuid(),
                LastName = nameof(ShouldReturnAllParentsAndChildred),
                FirstName = nameof(ShouldReturnAllParentsAndChildred),
                SecondName = nameof(ShouldReturnAllParentsAndChildred),
                Children = new List<Child> {
                    new Child {
                        BirthDay = DateTime.UtcNow,
                        LastName = nameof(ShouldReturnAllParentsAndChildred),
                        FirstName = nameof(ShouldReturnAllParentsAndChildred),
                        SecondName = nameof(ShouldReturnAllParentsAndChildred),
                        Id = Guid.NewGuid()
                    }
                }
            });

            var query = new GetAllParentsQuery();

            var result = await SendAsync(query);

            result.Should().HaveCount(1);
            result.First().CreatedDate.Should().BeCloseTo(DateTime.UtcNow,10000);
            result.First().Children.Should().HaveCount(1);
        }
    }
}