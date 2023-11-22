using MyAssets.Scripts.GameLogic.Game;
using MyAssets.Scripts.GameLogic.Player;
using MyAssets.Scripts.GameLogic.Weathers;
using UnityEngine;
using Object = UnityEngine.Object;

namespace MyAssets.Scripts.Modules.Factories
{
    public class PlayerFactory
    {
        private readonly PlayerController _playerController;
        private readonly DifficultySO _difficulty;
        private readonly Transform _container;
        private readonly Transform _spawnPos;
        private readonly Transform _right;
        private readonly Transform _left;
        private readonly Sprite _skin;

        public PlayerFactory(PlayerController playerController, Sprite skin, DifficultySO difficulty, Transform container, Transform spawnPos, Transform left, Transform right)
        {
            _playerController = playerController;
            _skin = skin;
            _difficulty = difficulty;
            _container = container;
            _spawnPos = spawnPos;
            _left = left;
            _right = right;
        }

        public PlayerController CreatePlayer()
        {
            var player = Object.Instantiate(_playerController, _container);
            player.transform.position = _spawnPos.position;
            player.Init(_skin, _difficulty.PlayerMoveSpeed, _difficulty.PlayerRandomRange, _left, _right, WeatherType.Rain);
            return player;
        }
    }
}