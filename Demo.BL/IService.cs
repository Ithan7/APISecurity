using Demo.Domain;

namespace Demo.BL
{
    public interface IService
    {
        CustomerLight GetCustomer(int id);
        void AddCustomer(CustomerLight customer);
        List<CustomerLight> GetAllCustomers();
        void DeleteCustomer(int id);
    }
}
