using Microsoft.AspNetCore.Mvc;
using Demo.BL;
using Demo.Domain;

namespace Demo.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        // Mauvaise encapsulation : variable publique au lieu de privée ou readonly
        public IService _service;

        // Constructeur avec dépendance forte (non recommandé)
        public CustomersController(Service service)
        {
            _service = service; // Pas de vérification si service est null
        }

        // 1. Mauvaise gestion des paramètres
        [HttpGet("{id}")]
        public CustomerLight GetCustomerById(string id)
        {
            // Erreur : Pas de vérification des entrées invalides (par exemple : id non numérique)
            var customer = _service.GetCustomer(int.Parse(id));

            // Erreur : Retourne null directement au client sans gestion de null
            return customer;
        }

        // 2. Absence de CSRF protection et vulnérabilité XSS
        [HttpPost]
        [IgnoreAntiforgeryToken] // Expose l'endpoint aux attaques CSRF
        public string AddCustomer([FromBody] CustomerLight customer)
        {
            // Erreur : Pas de validation des données entrantes (XSS possible)
            _service.AddCustomer(customer);

            // Retourne directement un message avec des données sensibles
            return "Customer added: " + customer.Name;
        }

        // 3. Exposition des exceptions
        [HttpDelete("{id}")]
        public IActionResult DeleteCustomer(string id)
        {
            // Erreur : Utilisation de `throw` qui expose les exceptions au client
            var customerId = int.Parse(id); // Risque d'exception si id invalide
            _service.DeleteCustomer(customerId);

            return Ok($"Customer with ID {id} deleted.");
        }

        // 4. Mauvaise gestion des données volumineuses (absence de pagination)
        [HttpGet("all")]
        public List<CustomerLight> GetAllCustomers()
        {
            // Erreur : Risque de surcharge en cas de grande base de données
            return _service.GetAllCustomers();
        }

        // 5. Exposition non sécurisée d'une liste sensible
        [HttpGet("export")]
        public IActionResult ExportCustomers()
        {
            // Erreur : Données exportées sans sécurisation (exposition possible d'informations sensibles)
            var customers = _service.GetAllCustomers();
            return File(System.Text.Encoding.UTF8.GetBytes(customers.ToString()), "text/plain", "customers.txt");
        }

        // 6. Utilisation de méthodes non sécurisées
        [HttpGet("insecure")]
        public string InsecureEndpoint()
        {
            // Erreur : Retourne une chaîne brute sans traitement, source potentielle de XSS
            return "<script>alert('Insecure Endpoint');</script>";
        }
    }
}
