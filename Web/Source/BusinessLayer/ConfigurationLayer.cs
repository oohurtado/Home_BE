namespace Home.Source.BusinessLayer
{
    public class ConfigurationLayer
    {
        private readonly IConfiguration configuration;

        public ConfigurationLayer(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public List<string> GetRoles()
        {
            return configuration["Roles"]!.Split(",").ToList();
        }

        public List<string> GetAdmins()
        {
            return configuration["Admins"]!.Split(",").ToList();
        }

        public bool IsEmailAdmin(string email)
        {
            return GetAdmins().Any(p => p == email);
        }

        public string GetJWTKey()
        {
            return configuration["JWT:Key"]!;
        }
    }
}
