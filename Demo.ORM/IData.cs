namespace Demo.ORM
{
    public interface IData
    {
        // Récupérer tous les clients
        List<Customer> GetCustomers();

        // Ajouter un nouveau client
        void AddCustomer(Customer customer);

        // Supprimer un client
        void DeleteCustomer(int id);
    }
}
