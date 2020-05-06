using System;
using System.Threading.Tasks;
using BusinessServices.Exceptions;
using BusinessServices.Modules.ParentModule;
using FluentAssertions;
using NUnit.Framework;
using static Testing;

namespace BusinessServices.IntegrationTests.Parent.Commands
{
    public class CreateParentsTests:TestBase
    {
        [Test]
        public async Task ShouldCreateValidParent()
        {
            var parentId = Guid.NewGuid();
            var command = new CreateParentCommand() {
                BirthDay = DateTime.UtcNow.AddDays(-3),
                Id = parentId,
                LastName = nameof(ShouldCreateValidParent),
                FirstName = nameof(ShouldCreateValidParent),
                SecondName = nameof(ShouldCreateValidParent),
            };

            var result = await SendAsync(command);

            result.Id.Should().Be(parentId);
            result.CreatedDate.Should().BeCloseTo(DateTime.UtcNow, 10000);
        }

        [Test]
        public void ShouldThrowValidationException()
        {
            var command = new CreateParentCommand();
            FluentActions.Invoking(()=> SendAsync(command)).Should().Throw<BusinessValidationException>();
        }
    }
}