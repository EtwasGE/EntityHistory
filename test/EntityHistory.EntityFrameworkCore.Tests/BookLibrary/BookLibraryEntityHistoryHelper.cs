using System.Collections.Generic;
using EntityHistory.Abstractions.Configuration;
using EntityHistory.Core.Extensions;
using EntityHistory.EntityFrameworkCore.Common;
using EntityHistory.EntityFrameworkCore.Tests.BookLibrary.Domain;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace EntityHistory.EntityFrameworkCore.Tests.BookLibrary
{
    public class BookLibraryEntityHistoryHelper : EntityHistoryHelper<CustomEntityChangeSet, long>
    {
        public BookLibraryEntityHistoryHelper(IEntityHistoryConfiguration configuration)
            : base(configuration)
        {
        }

        public override CustomEntityChangeSet GetEntityChangeSet(ICollection<EntityEntry> entityEntries)
        {
            if (!IsEntityHistoryEnabled)
            {
                return null;
            }

            var changeSet = new CustomEntityChangeSet
            {
                BrowserInfo = ClientInfoProvider.BrowserInfo.TruncateWithPostfix(CustomEntityChangeSet.MaxBrowserInfoLength),
                ClientIpAddress = ClientInfoProvider.ClientIpAddress.TruncateWithPostfix(CustomEntityChangeSet.MaxClientIpAddressLength),
                ClientName = ClientInfoProvider.ComputerName.TruncateWithPostfix(CustomEntityChangeSet.MaxClientNameLength),
                UserId = Session.UserId,

                CustomProperty = "Custom Property", //new
            }; 

            foreach (var entry in entityEntries)
            {
                if (!ShouldSaveEntityHistory(entry))
                {
                    continue;
                }

                var entityChange = GetEntityChange(entry);
                if (entityChange != null)
                {
                    changeSet.EntityChanges.Add(entityChange);
                }
            }

            return changeSet;
        }
    }
}
