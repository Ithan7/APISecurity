using Demo.ORM;
using Demo.Domain;

namespace Demo.BL
{
    public class Service : IService
    {
        private readonly IData _data;

        public Service(IData data)
        {
            _data = data;
        }

        // Méthode pour récupérer un client par ID
        public CustomerLight GetCustomer(int id)
        {
            var customer = _data.GetCustomers().FirstOrDefault(c => c.Id == id);
            if (customer == null)
            {
                throw new Exception($"Customer with ID {id} not found.");
            }

            return new CustomerLight
            {
                Id = customer.Id,
                Name = customer.Name,
                LastOrderDate = customer.LastOrderDate
            };
        }

        // Méthode pour ajouter un client
        public void AddCustomer(CustomerLight customerLight)
        {
            var customer = new Customer
            {
                Id = customerLight.Id,
                Name = customerLight.Name,
                LastOrderDate = customerLight.LastOrderDate
            };

            _data.AddCustomer(customer);
        }

        // Méthode pour récupérer tous les clients
        public List<CustomerLight> GetAllCustomers()
        {
            return _data.GetCustomers()
                .Select(c => new CustomerLight
                {
                    Id = c.Id,
                    Name = c.Name,
                    LastOrderDate = c.LastOrderDate
                })
                .ToList();
        }

        // Méthode pour supprimer un client par ID
        public void DeleteCustomer(int id)
        {
            var customer = _data.GetCustomers().FirstOrDefault(c => c.Id == id);
            if (customer == null)
            {
                throw new Exception($"Customer with ID {id} not found.");
            }

            _data.DeleteCustomer(id);
        }
    }
}
