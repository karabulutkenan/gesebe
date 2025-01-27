using Microsoft.EntityFrameworkCore;

namespace GSBMaas.Context
{
    public class AppDbContext:DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //optionsBuilder.UseSqlServer("server=.;database=GSBMaas2;integrated security=true");
            //optionsBuilder.UseSqlServer("server=WIN-IB2CQ6NRK39;user id=Toleyis;password=ATzc71H8CYov62Q;database=GSBMaas2;integrated security=false;");
            //optionsBuilder.UseSqlServer("Data Source=.; TrustServerCertificate=True; MultiSubnetFailover=True; Initial Catalog=GSBMaas2; User ID='Toleyis'; Password='ATzc71H8CYov62Q';");
            //optionsBuilder.UseSqlServer("server=kenan\\sqlexpress;database=GSBMaas;integrated security=true");
            optionsBuilder.UseSqlServer("server=.;database=GSBMaas2;integrated security=true");
            //optionsBuilder.UseSqlServer("server=CI\\sqlexpress;database=GSBMaas;integrated security=true");
            //< add name = "TOLEYIS_Entities" connectionString = "metadata=res://*/Models.Model1.csdl|res://*/Models.Model1.ssdl|res://*/Models.Model1.msl;provider=System.Data.SqlClient;provider connection string=&quot;data source=.;initial catalog=TOLEYIS;integrated security=True;MultipleActiveResultSets=True;App=EntityFramework&quot;" providerName = "System.Data.EntityClient" />
        }
        public DbSet<Sabit> Sabits { get; set; }

    }
}
