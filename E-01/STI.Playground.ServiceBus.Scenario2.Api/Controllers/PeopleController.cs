using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using STI.Playground.ServiceBus.Scenario2.Domain.People;
using STI.Playground.ServiceBus.Scenario2.Infra.AzureBus;
using STI.Playground.ServiceBus.Scenario2.Domain.People.Commands;
using System.Diagnostics;

namespace STI.Playground.ServiceBus.Scenario2.Api.Controllers
{
    [Route("api/[controller]")]
    public class PeopleController : Controller
    {
        private readonly IBus _bus;
        private readonly IPeopleRepository _peopleRepository;

        public PeopleController(
            IPeopleRepository peopleRepository,
            IBus bus)
        {
            _peopleRepository = peopleRepository;
            _bus = bus;
        }

        [HttpGet, Route("{id}")]
        public IActionResult Get(Guid id)
            => Ok(_peopleRepository.Get(id));

        [HttpPost, Route("")]
        public IActionResult SaveNew([FromBody]NewPersonInputModel newPerson)
        {
            var person = Person.New(
                newPerson.PublicId,
                FullName.New(newPerson.Name.FirstName, newPerson.Name.LastName),
                Address.New(newPerson.HomeAddress.Street1, newPerson.HomeAddress.Street2,
                    newPerson.HomeAddress.City, newPerson.HomeAddress.State,
                    newPerson.HomeAddress.ZipCode
                ));
            _peopleRepository.Save(person);
            return Ok();
        }

        [HttpPut, Route("{personId}")]
        public IActionResult SaveAddress(
            Guid personId,
            [FromBody]NewPersonInputModel.AddressInput newHomeAddress)
        {
            _bus.SendCommand<ChangeHomeAddressCommand>(
            ChangeHomeAddressCommand.New(
                personId,
                Address.New(newHomeAddress.Street1, newHomeAddress.Street2,
                newHomeAddress.City, newHomeAddress.State,
                newHomeAddress.ZipCode
            )));
            return Ok( "Command sent");
        }
    }

    public sealed class NewPersonInputModel
    {
        public string PublicId { get; set; }
        public FullNameInput Name { get; set; }
        public AddressInput HomeAddress { get; set; }

        public sealed class FullNameInput
        {
            public string FirstName { get; set; }
            public string LastName { get; set; }
        }

        public sealed class AddressInput
        {
            public string Street1 { get; set; }
            public string Street2 { get; set; }
            public string City { get; set; }
            public string State { get; set; }
            public string ZipCode { get; set; }
        }
    }
}
