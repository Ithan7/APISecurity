using Microsoft.AspNetCore.Mvc;
using Demo.BL;
using Demo.Domain;

namespace Demo.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        // Mauvaise encapsulation - variable publique au lieu de privée ou readonly
        public IService _service;

        // Mauvaise gestion de dépendances : dépendance forte sur l'implémentation au lieu d'une interface
        public CustomersController(Service service)
        {
            // Injection de dépendance manuelle incorrecte
            _service = service;
        }

        // Mauvais routage : ambigu et risque de conflit avec d'autres méthodes
        [HttpGet("{id}")]
        public CustomerLight GetLastCustomer(string id)
        {
            // Pas de gestion d'erreurs - possibilité d'exception non gérée
            var customer = _service.GetCustomer(int.Parse(id));

            // Mauvais usage de types : potentiellement null sans vérification
            return customer;
        }

        // Mauvais retour HTTP : utilisation d'un type simple au lieu de IActionResult
        [HttpPost]
        public string AddCustomer([FromBody] CustomerLight customer)
        {
            // Failles XSS : absence de validation des données entrantes
            _service.AddCustomer(customer);

            // Retourne des informations sensibles directement
            return "Customer added with ID: " + customer.Id;
        }

        // Méthode non sécurisée accessible sans restrictions
        [HttpGet("all")]
        public List<CustomerLight> GetAllCustomers()
        {
            // Pas de pagination - risque de surcharge du serveur
            return _service.GetAllCustomers();
        }

        // Endpoint non documenté et ouvert
        [HttpDelete("{id}")]
        public void DeleteCustomer(int id)
        {
            // Absence de vérification d'autorisation
            _service.DeleteCustomer(id);
        }
    }
}
