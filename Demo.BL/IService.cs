using Demo.Domain;

namespace Demo.BL
{
    public interface IService
    {
        public CustomerLight GetCustomer(int id);
    }
}
