using MyAssets.Scripts.GameLogic.Abilities;
using MyAssets.Scripts.GameLogic.Game;
using UnityEngine;
using Zenject;

namespace MyAssets.Scripts.Infrastructure.ProjectContext
{
    public class GameDataInstaller : MonoInstaller
    {
        [SerializeField] private Sprite[] _skinsSprites;
        [SerializeField] private ShieldAbilitySO _shieldAbility;
        [SerializeField] private ScaleAbilitySO _scaleAbility;
        
        public override void InstallBindings()
        {
            BindGameSettings();
        }

        private void BindGameSettings() =>
            Container.Bind<GameData>().AsSingle().
                WithArguments(_skinsSprites, _scaleAbility, _shieldAbility);
    }
}