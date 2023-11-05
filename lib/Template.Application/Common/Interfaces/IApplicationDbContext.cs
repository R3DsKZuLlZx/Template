namespace Template.Application.Common.Interfaces;

public interface IApplicationDbContext
{
    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
