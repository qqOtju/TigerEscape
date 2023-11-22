using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace MyAssets.Scripts.UI.GameScene
{
    [RequireComponent(typeof(Canvas))]
    public class ResultsUI: MonoBehaviour
    {
        [SerializeField] private Button _restartButton;
        [SerializeField] private Button _mainMenuButton;
        [SerializeField] private string _gameSceneName;
        [SerializeField] private string _mainMenuSceneName;
        
        private void Awake()
        {
            _restartButton.onClick.AddListener(OnRestartClicked);
            _mainMenuButton.onClick.AddListener(OnMainMenuClicked);
        }

        private void OnMainMenuClicked() =>
            SceneManager.LoadScene(_mainMenuSceneName, LoadSceneMode.Single);

        private void OnRestartClicked() =>
            SceneManager.LoadScene(_gameSceneName, LoadSceneMode.Single);
    }
}