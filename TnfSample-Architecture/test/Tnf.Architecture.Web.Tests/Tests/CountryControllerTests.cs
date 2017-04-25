﻿using Tnf.Architecture.Web.Tests.App.Controllers;
using Xunit;
using Shouldly;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using Tnf.Web.Models;
using Tnf.Application.Services.Dto;
using Tnf.Architecture.Dto;
using System.Net;

namespace Tnf.Architecture.Web.Tests.Tests
{
    public class CountryControllerTests : AppTestBase
    {
        [Fact]
        public void Should_Resolve_Controller()
        {
            ServiceProvider.GetService<CountryController>().ShouldNotBeNull();
        }

        [Fact]
        public async Task GetAll_Countries_With_Success()
        {
            // Act
            var response = await GetResponseAsObjectAsync<AjaxResponse<PagedResultDto<CountryDto>>>(
                "/api/countries?skipCount=0",
                HttpStatusCode.OK
            );

            // Assert
            Assert.True(response.Success);
            Assert.Equal(response.Result.TotalCount, 5);
        }

        [Fact]
        public async Task GetAll_Countries_With_Default_Parameter()
        {
            // Act
            var response = await GetResponseAsObjectAsync<AjaxResponse<PagedResultDto<CountryDto>>>(
                "/api/countries?",
                HttpStatusCode.OK
            );

            // Assert
            Assert.True(response.Success);
            Assert.Equal(response.Result.TotalCount, 5);
        }

        [Fact]
        public async Task Get_Country_With_Success()
        {
            // Act
            var response = await GetResponseAsObjectAsync<AjaxResponse<CountryDto>>(
                "/api/countries/1",
                HttpStatusCode.OK
            );

            // Assert
            Assert.True(response.Success);
            Assert.Equal(response.Result.Id, 1);
            Assert.Equal(response.Result.Name, "Brasil");
        }

        [Fact]
        public async Task Get_Country_With_Invalid_Parameter_Return_Bad_Request()
        {
            // Act
            var response = await GetResponseAsObjectAsync<AjaxResponse>(
                "/api/countries/0",
                HttpStatusCode.BadRequest
            );

            // Assert
            Assert.True(response.Success);
            Assert.Equal(response.Result, "Invalid parameter: id");
        }

        [Fact]
        public async Task Get_Country_When_Not_Exits_Return_Not_Found_Request()
        {
            // Act
            var response = await GetResponseAsObjectAsync<AjaxResponse>(
                "/api/countries/99",
                HttpStatusCode.NotFound
            );

            // Assert
            Assert.True(response.Success);
            Assert.Equal(response.Result, "Country not found");
        }

        [Fact]
        public async Task Post_Country_With_Success()
        {
            var countryDto = new CountryDto()
            {
                Id = 6,
                Name = "Canada"
            };

            // Act
            var response = await PostResponseAsObjectAsync<CountryDto, AjaxResponse<CountryDto>>(
                "/api/countries",
                countryDto,
                HttpStatusCode.OK
            );

            // Assert
            Assert.True(response.Success);
            Assert.Equal(response.Result.Id, 6);
            Assert.Equal(response.Result.Name, "Canada");
        }

        [Fact]
        public async Task Post_Country_With_Invalid_Parameter_Return_Bad_Request()
        {
            // Act
            var response = await PostResponseAsObjectAsync<CountryDto, AjaxResponse>(
                "/api/countries",
                null,
                HttpStatusCode.BadRequest
            );

            // Assert
            Assert.True(response.Success);
            response.Result.ShouldBe("Invalid parameter: dto");
        }

        [Fact]
        public async Task Put_Country_With_Success()
        {
            var countryDto = new CountryDto()
            {
                Id = 4,
                Name = "Canada"
            };

            // Act
            var response = await PutResponseAsObjectAsync<CountryDto, AjaxResponse<CountryDto>>(
                "/api/countries",
                countryDto,
                HttpStatusCode.OK
            );

            // Assert
            Assert.True(response.Success);
            Assert.Equal(response.Result.Id, 4);
            Assert.Equal(response.Result.Name, "Canada");
        }

        [Fact]
        public async Task Put_Country_With_Invalid_Parameter_Return_Bad_Request()
        {
            // Act
            var response = await PutResponseAsObjectAsync<CountryDto, AjaxResponse>(
                "/api/countries",
                null,
                HttpStatusCode.BadRequest
            );

            // Assert
            Assert.True(response.Success);
            response.Result.ShouldBe("Invalid parameter: dto");
        }

        [Fact]
        public async Task Delete_Country_With_Success()
        {
            // Act
            await DeleteResponseAsObjectAsync<AjaxResponse>(
                "/api/countries/1",
                HttpStatusCode.OK
            );
        }

        [Fact]
        public async Task Delete_Country_With_Invalid_Parameter_Return_Bad_Request()
        {
            // Act
            var response = await DeleteResponseAsObjectAsync<AjaxResponse>(
                "/api/countries/0",
                HttpStatusCode.BadRequest
            );

            // Assert
            response.Success.ShouldBeTrue();
            response.Result.ShouldBe("Invalid parameter: id");
        }
    }
}