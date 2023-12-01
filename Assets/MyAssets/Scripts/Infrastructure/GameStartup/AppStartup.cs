using System.Collections.Generic;
using System.Threading.Tasks;
using MyAssets.Scripts.UI.Loading;
using MyAssets.Scripts.UI.Loading.Operations;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MyAssets.Scripts.Infrastructure.GameStartup
{
    public class AppStartup : MonoBehaviour
    {
        [Header("Menu Scene")] 
        [SerializeField] private string _menuScene;
        [Header("Loading Screen")]
        [SerializeField] private LoadingScreen _loadingScreen;
        
        private async void Awake()
        {
            var loadingOperations = new Queue<ILoadingOperation>();
            loadingOperations.Enqueue(new LoadingSceneOperation(_menuScene, LoadSceneMode.Additive));
            await Load(loadingOperations);
            Destroy(gameObject);
        }

        private async Task Load(Queue<ILoadingOperation> loadingOperations)
        {
            var loadingScreen = Instantiate(_loadingScreen);
            await loadingScreen.Load(loadingOperations);
            Destroy(loadingScreen.gameObject);
        }
    }
}