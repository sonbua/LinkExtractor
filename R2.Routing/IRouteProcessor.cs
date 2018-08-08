using System.Collections.Generic;
using System.Threading.Tasks;

namespace R2.Routing
{
    public interface IRouteProcessor
    {
        Task ProcessCommandAsync(string commandName, string commandObjectString);

        Task<object> ProcessQueryAsync(string queryName, string queryObjectString);

        Task<object> ProcessUploadAsync(string uploadName, IList<IFile> files);
    }
}