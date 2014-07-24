using System.Data.Entity.Migrations;
using System.Web.Http;
using londoncoffeeService.Migrations;
using Microsoft.WindowsAzure.Mobile.Service;

namespace londoncoffeeService
{
    public static class WebApiConfig
    {
        public static void Register()
        {
            // Use this class to set configuration options for your mobile service
            var options = new ConfigOptions();

            // Use this class to set WebAPI configuration options
            HttpConfiguration config = ServiceConfig.Initialize(new ConfigBuilder(options));

            // To display errors in the browser during development, uncomment the following
            // line. Comment it out again when you deploy your service for production use.
            config.IncludeErrorDetailPolicy = IncludeErrorDetailPolicy.Always;
            
            //Database.SetInitializer(new DataInitializer());
            var configuration = new Configuration();
            var migrator = new DbMigrator(configuration);
            migrator.Update();
        }
    }
}

