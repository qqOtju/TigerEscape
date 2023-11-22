using System;
using Task = System.Threading.Tasks.Task;

namespace MyAssets.Scripts.Infrastructure.GameStartup
{
    public interface ILoadingOperation
    {
        public event Action<string> OnDescriptionChange;
        public string Description { get; }

        public Task Load(Action<float> onProgress);
    }
}