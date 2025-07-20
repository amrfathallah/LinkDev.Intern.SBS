using SBS.Application.Interfaces.IRepositories;
using SBS.Domain.Entities;
using SBS.Infrastructure.Persistence._Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SBS.Infrastructure.Repositories
{
	internal class ResourceRepository : GenericRepository<Resource>, IResourceRepository
	{
		private readonly AppDbContext _appDbContext;

		public ResourceRepository(AppDbContext appDbContext) : base(appDbContext)
		{
			_appDbContext = appDbContext;
		}
	}
}
