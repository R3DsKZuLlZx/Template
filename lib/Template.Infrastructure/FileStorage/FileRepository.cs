using Foundatio.Storage;
using Template.Application.Common.Interfaces;

namespace Template.Infrastructure.FileStorage;

public class FileRepository : IFileRepository
{
    public FileRepository(IFileStorage storage)
    {
        Storage = storage;
    }
    
    public IFileStorage Storage { get; }
}
