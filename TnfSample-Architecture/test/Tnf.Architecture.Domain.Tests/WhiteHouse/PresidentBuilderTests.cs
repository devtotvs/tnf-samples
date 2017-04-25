﻿using Tnf.Architecture.Domain.WhiteHouse;
using Xunit;
using System.Linq;

namespace Tnf.Architecture.Domain.Tests.WhiteHouse
{
    public class PresidentBuilderTests
    {
        [Fact]
        public void Create_Valid_President()
        {
            // Arrange
            var builder = new PresidentBuilder()
                  .WithId("1")
                  .WithName("George Washington")
                  .WithZipCode("99.999-999");

            // Act
            var build = builder.Build();

            // Assert
            Assert.True(build.Success);
        }

        [Fact]
        public void Create_President_With_Invalid_Name()
        {
            // Arrange
            var builder = new PresidentBuilder()
                  .WithId("1")
                  .WithName(null)
                  .WithZipCode("99.999-999");

            // Act
            var build = builder.Build();

            // Assert
            Assert.False(build.Success);
            Assert.True(build.Notifications.Any(a => a.Message == President.Error.PresidentNameMustHaveValue.ToString()));
        }

        [Fact]
        public void Create_President_With_Invalid_ZipCode()
        {
            // Arrange
            var builder = new PresidentBuilder()
                  .WithId("1")
                  .WithName("George Washington")
                  .WithZipCode("99.999-999-23");

            // Act
            var build = builder.Build();

            // Assert
            Assert.False(build.Success);
            Assert.True(build.Notifications.Any(a => a.Message == President.Error.PresidentZipCodeMustHaveValue.ToString()));
        }
    }
}