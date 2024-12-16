namespace Demo.ORM
{
    public class Data:IData
    {
        CustomerEfCoreContext _dbContext;
        public Data()
        {
            _dbContext = new CustomerEfCoreContext();
        }
        public List<Customer> GetCustomers()
        {
            return _dbContext.Customers.ToList();
        }
    }
}
