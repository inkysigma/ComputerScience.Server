using System;
using System.Data.Common;
using ComputerScience.Server.Common;
using ComputerScience.Server.Web.Business.Solutions;
using ComputerScience.Server.Web.Data.SolutionCache;
using ComputerScience.Server.Web.Data.SolutionSet;
using ComputerScience.Server.Web.Models.Solutions;
using Microsoft.Extensions.DependencyInjection;

namespace ComputerScience.Server.Web.Extentions
{
    public static class SolutionSetExtensions
    {
        public static IServiceCollection AddSolutionServices(this IServiceCollection collection,
            Func<IServiceProvider, DbConnection> connectionFunction, 
            Func<IServiceProvider, ISolutionCache<Solution>> cacheFunction,
            SolutionServiceConfiguration serviceConfiguration = null,
            SolutionSetConfiguration configuration = null,
            SolutionValidator validator = null)
        {
            if (configuration == null)
                configuration = new SolutionSetConfiguration();
            if (validator == null)
                validator = new SolutionValidator();
            if (serviceConfiguration == null)
                serviceConfiguration = new SolutionServiceConfiguration();
            var connection = connectionFunction.Invoke(collection.BuildServiceProvider());
            var cache = cacheFunction.Invoke(collection.BuildServiceProvider());
            var set = new SolutionSet(configuration, connection);
            collection.AddTransient<ISolutionSet<Solution>>(provider => set);
            collection.AddTransient(
                provider => new SolutionService<Solution>(cache, set, validator, serviceConfiguration));
            return collection;
        }
    }
}
