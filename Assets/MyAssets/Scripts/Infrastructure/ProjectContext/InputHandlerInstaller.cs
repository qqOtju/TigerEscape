using MyAssets.Scripts.Input;
using UnityEngine;
using Zenject;

namespace MyAssets.Scripts.Infrastructure.ProjectContext
{
    public class InputHandlerInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            var handler = new GameObject("InputHandler").AddComponent<InputHandler>();
            Container.Bind<ITapInput>().FromInstance(handler).AsSingle();
            Container.Bind<IPositionInput>().FromInstance(handler).AsSingle();
        }
    }
}
