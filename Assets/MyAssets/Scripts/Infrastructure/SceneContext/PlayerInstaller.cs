using MyAssets.Scripts.GameLogic;
using MyAssets.Scripts.GameLogic.Game;
using MyAssets.Scripts.GameLogic.Player;
using MyAssets.Scripts.Modules.Factories;
using UnityEngine;
using Zenject;

namespace MyAssets.Scripts.Infrastructure.SceneContext
{
    public class PlayerInstaller : MonoInstaller
    {
        [SerializeField] private PlayerController _playerController;
        [SerializeField] private Transform _container;
        [SerializeField] private Transform _spawnPos;
        [SerializeField] private Transform _left;
        [SerializeField] private Transform _right;

        private GameData _data;
        
        [Inject]
        public void Construct(GameData data)
        {
            _data = data;
        }
        
        public override void InstallBindings()
        {
            BindPlayer();
        }

        private void BindPlayer()
        {
            var factory = new PlayerFactory(_playerController,_data.GetCurrentSkin(), _data.Difficulty, _container, _spawnPos, _left, _right);
            var player = factory.CreatePlayer();
            Container.Bind<PlayerController>().FromInstance(player).AsSingle().NonLazy();
        }
    }
}