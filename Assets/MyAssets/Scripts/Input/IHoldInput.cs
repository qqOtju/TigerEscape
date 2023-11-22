using System;

namespace MyAssets.Scripts.Input
{
    public interface IHoldInput
    {
        public event Action OnHoldStart;
        public event Action OnHoldEnd;
    }
}