using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Moq;
using Restaurants.Application.Users;
using Restaurants.Domain.Constants;
using System.Security.Claims;
using Xunit;

namespace Restaurants.Application.Tests.Users
{
    public class UserContextTests
    {
        [Fact()]
        public void GetCurrentUser_WithAuthanticatedUser_ShouldReturnCurrentUser()
        {

            // arrange

            var dateOfBirth = new DateOnly(1990, 1, 1);

            var httpContextAccessorMock = new Mock<IHttpContextAccessor>();
            var claims = new List<Claim>()
            {
                new (ClaimTypes.NameIdentifier, "1"),
                new (ClaimTypes.Email, "test@gmail.com"),
                new (ClaimTypes.Role, UserRoles.Admin),
                new (ClaimTypes.Role, UserRoles.Owner),
                new ("Nationality", "German"),
                new ("DateOfBirth", dateOfBirth.ToString("yyyy-MM-dd"))
            };

            var user = new ClaimsPrincipal(new ClaimsIdentity(claims, "Test"));

            httpContextAccessorMock.Setup(x => x.HttpContext).Returns(new DefaultHttpContext()
            {
                User = user
            });

            var userContext = new UserContext(httpContextAccessorMock.Object);
            // act

            var currentUser = userContext.GetCurrentUser();

            // asset

            currentUser.Should().NotBeNull();
            currentUser.Id.Should().Be("1");
            currentUser.Email.Should().Be("test@gmail.com");
            currentUser.Roles.Should().ContainInOrder(UserRoles.Admin, UserRoles.Owner);
            currentUser.Nationality.Should().Be("German");
            currentUser.DateOfBirth.Should().Be(dateOfBirth);
        }

        [Fact()]
        public void GetCurrentUser_WithUserContextNotPresent_ThrowInvalidOperationException()
        {
            // arrange 
            var httpContextAccessorMock = new Mock<IHttpContextAccessor>();
            httpContextAccessorMock.Setup(x => x.HttpContext).Returns((HttpContext)null);

            var userContext = new UserContext(httpContextAccessorMock.Object);

            //act 

            Action act = () => userContext.GetCurrentUser();

            // assert

            act.Should().Throw<InvalidOperationException>().WithMessage("User context is not present");
        }
    }
}