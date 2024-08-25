namespace MovSharkApi.Services
{

    public interface IDbConfigurationService
    {
        string GetConnectionString();
    }

    public class DbConfigurationService: IDbConfigurationService
    {
        private readonly IConfiguration _configuration;

        public DbConfigurationService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GetConnectionString()
        {
            return _configuration.GetConnectionString("DefaultConnection");
        }
    }
}
