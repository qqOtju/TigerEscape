using System.Collections.Generic;
using System.Threading.Tasks;
using MyAssets.Scripts.Infrastructure.GameStartup;

namespace MyAssets.Scripts.UI.Loading
{
    public interface ILoadingScreen
    {
        public Task Load(Queue<ILoadingOperation> loadingOperations, bool withProgressBar = true);
    }
}