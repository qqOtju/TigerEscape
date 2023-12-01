using System;

namespace MyAssets.Scripts.MyInput
{
    public interface IHoldInput
    {
        public event Action OnHoldStart;
        public event Action OnHoldEnd;
    }
}