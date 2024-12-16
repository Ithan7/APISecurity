using Microsoft.AspNetCore.Mvc;
using Demo.BL;
using Demo.Domain;
using System.IO;
using System.Text;

namespace Demo.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        public IService _service;

        public CustomersController(IService service)
        {
            // Injection de dépendances incorrecte - risque d'exposer l'application si le service est null
            _service = service ?? throw new ArgumentNullException(nameof(service));
        }

        // 1. **Injection SQL** : Utilisation directe des paramètres non sécurisés
        [HttpGet("find/{name}")]
        public IActionResult FindCustomerByName(string name)
        {
            var query = $"SELECT * FROM Customers WHERE Name = '{name}'"; // Vulnérabilité SQL Injection
            return Ok($"Executing Query: {query}");
        }

        // 2. **Exposition d'exceptions sensibles**
        [HttpDelete("{id}")]
        public IActionResult DeleteCustomer(string id)
        {
            // Exposition brute d'une exception - pas de gestion sécurisée
            var customerId = int.Parse(id); // Risque d'exception pour valeur non numérique
            _service.DeleteCustomer(customerId);
            return Ok($"Deleted customer with ID: {id}");
        }

        // 3. **XSS** : Retourne des données brutes sans encodage
        [HttpGet("insecure-output")]
        public string InsecureOutput(string input)
        {
            return $"<html><body><h1>Welcome, {input}</h1></body></html>"; // XSS possible si input contient des scripts
        }

        // 4. **CSRF** : Endpoint POST sans protection
        [HttpPost("add")]
        [IgnoreAntiforgeryToken] // Désactivation explicite de CSRF token
        public string AddCustomer([FromBody] CustomerLight customer)
        {
            _service.AddCustomer(customer);
            return $"Customer {customer.Name} added!";
        }

        // 5. **Mauvaise gestion des fichiers** : Exposition d'un fichier arbitraire
        [HttpGet("download")]
        public IActionResult DownloadFile(string filePath)
        {
            // Aucune validation du chemin - risque de Path Traversal
            var fileContent = System.IO.File.ReadAllBytes(filePath);
            return File(fileContent, "application/octet-stream", Path.GetFileName(filePath));
        }

        // 6. **Exposition d'informations sensibles**
        [HttpGet("debug-info")]
        public IActionResult DebugInfo()
        {
            var serverInfo = $"Server Version: {System.Environment.Version} | OS: {System.Environment.OSVersion}";
            return Ok(serverInfo); // Exposition d'informations sensibles
        }

        // 7. **Données volumineuses sans restriction** : Endpoint non paginé
        [HttpGet("all")]
        public List<CustomerLight> GetAllCustomers()
        {
            return _service.GetAllCustomers(); // Retourne toutes les données sans filtre ni pagination
        }
    }
}
