using Microsoft.AspNetCore.Mvc.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace App.Repositories.Intercepters;

public class AuditDbContextInterceptor : SaveChangesInterceptor
{
    public static readonly Dictionary<EntityState, Action<DbContext,IAuditEntity>> Behaviours = new()
    {
        { EntityState.Added, AddBehaviour },
        { EntityState.Modified, ModifyBehaviour }
    };
    private static void AddBehaviour(DbContext context, IAuditEntity auditEntity)
    {
        auditEntity.Created = DateTime.Now;
        context.Entry(auditEntity).Property(x => x.Updated).IsModified = false;
    }
    private static void ModifyBehaviour(DbContext context, IAuditEntity auditEntity)
    {
        context.Entry(auditEntity).Property(x => x.Created).IsModified = false;
        auditEntity.Updated = DateTime.Now;
    }
    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
    {

        foreach (var entityEntry in eventData.Context!.ChangeTracker.Entries().ToList())
        {
            if (entityEntry.Entity is not IAuditEntity auditEntity) continue;

            Behaviours[entityEntry.State](eventData.Context, auditEntity);

            #region 1.Yol
            //switch (entityEntry.State)
            //{
            //    case EntityState.Added:

            //        AddBehaviour(eventData.Context, auditEntity);


            //        break;

            //    case EntityState.Modified:


            //        ModifyBehaviour(eventData.Context, auditEntity);


            //        break;
            //}
            #endregion
        }


        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }
}
