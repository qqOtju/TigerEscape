using System;
using UnityEngine;

namespace MyAssets.Scripts.Input
{
    public interface IPositionInput
    {
        public event Action<Vector2> OnPositionChange; 
    }
}