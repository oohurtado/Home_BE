using Home.Source.Data.Infrastructure;
using Home.Source.Data.Repositories;

namespace Home.Source.BusinessLayer
{
    public class SeedLayer
    {
        private readonly IAspNetRepository aspNetRepository;

        public SeedLayer(IAspNetRepository aspNetRepository)
        {
            this.aspNetRepository = aspNetRepository;
        }

        public async Task InitAsync()
        {
            await aspNetRepository.CreateRolesAsync();
            await aspNetRepository.DeleteRolesAsync();
        }
    }
}
