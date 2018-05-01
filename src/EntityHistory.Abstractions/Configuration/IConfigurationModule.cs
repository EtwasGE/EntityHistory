namespace EntityHistory.Abstractions.Configuration
{
    public interface IConfigurationModule<TEntity>
    {
        void Configure(IEntityConfiguration<TEntity> config);
    }
}
