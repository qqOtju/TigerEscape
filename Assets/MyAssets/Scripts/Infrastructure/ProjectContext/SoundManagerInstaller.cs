using MyAssets.Scripts.GameLogic.Audio;
using UnityEngine;
using Zenject;

namespace MyAssets.Scripts.Infrastructure.ProjectContext
{
    public class SoundManagerInstaller: MonoInstaller
    {
        [SerializeField] private SoundManager _soundManager;

        public override void InstallBindings()
        {
            BindSounManager();
        }

        private void BindSounManager() =>
            Container.Bind<SoundManager>().
                FromInstance(Instantiate(_soundManager)).AsSingle();
    }
}