using System;
using UnityEngine;

namespace MyAssets.Scripts.MyInput
{
    public interface IPositionInput
    {
        public event Action<Vector2> OnPositionChange; 
    }
}