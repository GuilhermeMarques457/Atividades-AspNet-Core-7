﻿using AutoFixture;
using Moq;
using ServiceContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentAssertions;
using ServiceContracts.DTO;
using System.Threading.Tasks;
using ServiceContracts.Enums;
using Microsoft.AspNetCore.Mvc;
using Entities;
using Microsoft.Extensions.Logging;
using Services;
using SampleApplicationCRUD.Controllers;

namespace Tests
{
    public class PersonsControllerTest
    {
        private readonly IPersonService _personsService;
        private readonly ICountriesService _countriesService;

        private readonly Mock<IPersonService> _personsServiceMock;
        private readonly Mock<ICountriesService> _countriesServiceMock;
        private readonly Fixture _fixture;
        private readonly Mock<ILogger<PersonsController>> _loggerMock;

        public PersonsControllerTest()
        {
            _fixture = new Fixture();
            _loggerMock = new Mock<ILogger<PersonsController>>();
            _countriesServiceMock = new Mock<ICountriesService>();
            _personsServiceMock = new Mock<IPersonService>();

            _countriesService = _countriesServiceMock.Object;
            _personsService = _personsServiceMock.Object;
        }

        #region Index

        [Fact]
        public async Task Index_ShouldReturnIndexViewWithPersonsList()
        {
            //Arrange
            List<PersonResponse> personResponsesList =
                _fixture.Create<List<PersonResponse>>();

            PersonsController peopleController = new PersonsController(_personsService, _countriesService, _loggerMock.Object);

            //Mocking the usings methods in the action
            _personsServiceMock.Setup(temp => temp.GetFilterdPeople(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(personResponsesList);

            _personsServiceMock.Setup(temp => temp.GetSortedPeople
                (It.IsAny<List<PersonResponse>>(),
                 It.IsAny<string>(),
                 It.IsAny<SortOrderOptions>()))
                .Returns(personResponsesList);

            //Act
            //Creating a result based on the parameters of the controller
            IActionResult result = await peopleController.Index(
                _fixture.Create<string>(),
                _fixture.Create<string>(),
                _fixture.Create<string>(),
                _fixture.Create<SortOrderOptions>()
            );

            //Assert
            //Seeing if the return of the view is type of ViewResult
            ViewResult viewResult = Assert.IsType<ViewResult>(result);

            //Acessing the model object passed into the view
            viewResult.ViewData.Model.Should().BeAssignableTo<List<PersonResponse>>();
            viewResult.ViewData.Model.Should().BeEquivalentTo(personResponsesList);

        }

        #endregion

        #region AddPerson

        [Fact]
        public async Task AddPerson_IfModelErros_ToReturnCreateView()
        {
            //Arrange
            PersonAddRequest personAddRequest =
                _fixture.Create<PersonAddRequest>();

            PersonResponse personResponse =
                _fixture.Create<PersonResponse>();

            List<CountryResponse> countries =
                _fixture.Create<List<CountryResponse>>();

            PersonsController peopleController = new PersonsController(_personsService, _countriesService, _loggerMock.Object);

            _countriesServiceMock.Setup(temp => temp.GetAllCountries())
                .ReturnsAsync(countries);            

            //Act
            peopleController.ModelState.AddModelError(
                "PersonName", "Person Name can't be blank"    
            );

            List<CountryResponse> countryResponsesActual = await _countriesService.GetAllCountries();

            IActionResult result = 
                await peopleController.Create(personAddRequest);

            //Assert
            ViewResult viewResult = Assert.IsType<ViewResult>(result);

            viewResult.ViewData.Model.Should().BeAssignableTo<PersonAddRequest>();
            viewResult.ViewData.Model.Should().Be(personAddRequest);
            countryResponsesActual.Should().BeEquivalentTo(countries);

        }

        [Fact]
        public async Task AddPerson_IfNoModelErros_ToReturnRedirectToIndexView()
        {
            //Arrange
            PersonAddRequest personAddRequest =
                _fixture.Create<PersonAddRequest>();

            PersonResponse personResponseExpected =
                _fixture.Create<PersonResponse>();

            PersonsController peopleController = new PersonsController(_personsService, _countriesService, _loggerMock.Object);

            _personsServiceMock.Setup(temp => temp.AddPerson(It.IsAny<PersonAddRequest>()))
                .ReturnsAsync(personResponseExpected);

            //Act

            IActionResult result =
                await peopleController.Create(personAddRequest);

            PersonResponse personResponseActual = await _personsService.AddPerson(personAddRequest);

            //Assert
            RedirectToActionResult redirectResult = Assert.IsType<RedirectToActionResult>(result);
            redirectResult.ActionName.Should().Be("Index");

            personResponseActual.Should().BeEquivalentTo(personResponseExpected);

        }
        #endregion

        #region EditPerson

        [Fact]
        public async Task EditPerson_ValidGuid_ToReturnEditView()
        {
            //Arrange
            

            PersonResponse personResponse = _fixture.Build<PersonResponse>()
                .With(temp => temp.PersonGender, _fixture.Create<GenderOptions>().ToString())
                .Create();

            PersonUpdateRequest personUpdateRequest = personResponse.ToPersonUpdateRequest();

            List <CountryResponse> countries =
                _fixture.Create<List<CountryResponse>>();

            PersonsController peopleController = new PersonsController(_personsService, _countriesService, _loggerMock.Object);

            _countriesServiceMock.Setup(temp => temp.GetAllCountries())
                .ReturnsAsync(countries);

            _personsServiceMock.Setup(temp => temp.GetPersonById(It.IsAny<Guid>()))
                .ReturnsAsync(personResponse);

            //Act

            List<CountryResponse> countryResponsesActual = await _countriesService.GetAllCountries();
            PersonResponse? personResponseActual = await _personsService.GetPersonById(personUpdateRequest.PersonId);

            IActionResult result =
                await peopleController.Edit(personUpdateRequest.PersonId);

            //Assert
            ViewResult viewResult = Assert.IsType<ViewResult>(result);

            viewResult.ViewData.Model.Should().BeAssignableTo<PersonUpdateRequest>();
            viewResult.ViewData.Model.Should().BeEquivalentTo(personResponseActual?.ToPersonUpdateRequest());

            personResponseActual.Should().BeEquivalentTo(personResponse);
            countryResponsesActual.Should().BeEquivalentTo(countries);

        }

        #endregion
    }
}
