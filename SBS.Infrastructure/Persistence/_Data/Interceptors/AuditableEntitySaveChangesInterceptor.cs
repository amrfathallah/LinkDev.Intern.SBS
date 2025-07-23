using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using SBS.Application.Interfaces.Common;
using SBS.Domain._Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SBS.Infrastructure.Persistence._Data.Interceptors
{
	public class AuditableEntitySaveChangesInterceptor : SaveChangesInterceptor
	{
		private readonly ICurrentUserService _currentUserService;

		public AuditableEntitySaveChangesInterceptor(ICurrentUserService currentUserService)
		{
			_currentUserService = currentUserService;
		}

		public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
		{
			UpdateAuditableEntities(eventData.Context);
			return base.SavingChanges(eventData, result);
		}

		public override ValueTask<int> SavedChangesAsync(SaveChangesCompletedEventData eventData, int result, CancellationToken cancellationToken = default)
		{
			UpdateAuditableEntities(eventData.Context);
			return base.SavedChangesAsync(eventData, result, cancellationToken);
		}


		private void UpdateAuditableEntities(DbContext? dbContext)
		{

			if (dbContext is null) return;

			var entries = dbContext.ChangeTracker.Entries<IBaseAuditableEntity>()
						  .Where(entity => entity.State is EntityState.Added or EntityState.Modified);

			var userId = _currentUserService.UserId ?? "System";
			var utcNow = DateTime.UtcNow;

			foreach (var entry in entries)
			{

				if (entry.State is EntityState.Added)
				{
					entry.Entity.CreatedBy = userId;
					entry.Entity.CreatedAt = utcNow;
				}
				entry.Entity.LastModifiedBy = userId;
				entry.Entity.LastModifiedAt = utcNow;

			}
		}
	}
}
