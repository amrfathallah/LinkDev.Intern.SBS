using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SBS.Application.Interfaces.IServices;
using SBS.Application.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IResourceService = SBS.Application.Interfaces.IServices.IResourceService;

namespace SBS.Application
{
    public static class DependencyInjection
	{
		public static IServiceCollection AddApplicationDependencies(this IServiceCollection services, IConfiguration configuration)
		{
			services.AddAutoMapper(Mapper => Mapper.AddProfile(new MappingProfile()));
			
            return services;
		}
	}
}