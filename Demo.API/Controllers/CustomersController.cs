using Microsoft.AspNetCore.Mvc;
using Demo.BL;
using Demo.Domain;

namespace Demo.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        public readonly IService _service;
        public CustomersController(IService service)
        {
            _service = service;
        }

        [HttpGet]
        public CustomerLight GetLastCustomer(int id)
        {
            var customer = _service.GetCustomer(id);
            return customer;
        }
    }
}
