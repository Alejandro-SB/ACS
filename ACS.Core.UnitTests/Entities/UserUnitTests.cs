using ACS.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace ACS.Core.UnitTests.Entities
{
    public class UserUnitTests
    {
        private const string RefreshToken = "0000";
        [Fact]
        public void HasValidRefreshToken_GivenValidToken_ReturnsTrue()
        {
            var user = new ACSUser("", "", "", "");
            user.AddRefreshToken(RefreshToken, 0, "");

            Assert.True(user.HasValidRefreshToken(RefreshToken));
        }

        [Fact]
        public void HasValidRefreshToken_GivenExpiredToken_ReturnsFalse()
        {
            var user = new ACSUser("", "", "", "");
            user.AddRefreshToken(RefreshToken, 0, "", -1);

            Assert.False(user.HasValidRefreshToken(RefreshToken));
        }
    }
}