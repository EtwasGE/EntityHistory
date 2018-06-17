﻿using Autofac;
using EntityHistory.Configuration;
using EntityHistory.EntityFrameworkCore.Identity.Tests.BookLibrary.Domain;

namespace EntityHistory.EntityFrameworkCore.Identity.Tests.BookLibrary
{
    public class BookLibraryTestModule : EntityFrameworkCoreTestModule<CustomUser>
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            var options = GetDbContextOptions<BookLibraryDbContext>(builder);
            var isCreated = new BookLibraryDbContext(options).Database.EnsureCreated();

            builder.RegisterType<BookLibraryDbContext>()
                .PropertiesAutowired();
            
            InitialConfiguration();
        }

        private void InitialConfiguration()
        {
            HistoryConfiguration.IsEnabledForAnonymousUsers = true;
        }
    }
}
