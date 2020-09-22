using Business.Core;
using Business.Definition.Contract;
using Business.Definition.Infra.Repository;
using Business.Definition.Model;
using Infra.Repository.ZipFile.GitHub;
using Microsoft.Extensions.DependencyInjection;
using Presentation;
using Presentation.Definition.Contract;

namespace IoC
{
    public static class DependencyContainer
    {
        public static void RegisterDependencies(this IServiceCollection service)
        {
            //Repositories
            service.AddScoped(typeof(IRepositoryStatsRepository), typeof(RepositoryStatsRepository));

            //Business Core
            service.AddScoped(typeof(IRepositoryStatsCore), typeof(RepositoryStatsCore));

            //Presentation
            service.AddScoped(typeof(IReposytoryStatsPresentation), typeof(RepositoryStatsPresentation));
        }
    }
}