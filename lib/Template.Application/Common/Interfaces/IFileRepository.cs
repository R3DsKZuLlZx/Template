using Foundatio.Storage;

namespace Template.Application.Common.Interfaces;

public interface IFileRepository
{
    public IFileStorage Storage { get; }
}
