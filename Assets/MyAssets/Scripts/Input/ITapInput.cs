using System;
using UnityEngine;

namespace MyAssets.Scripts.Input
{
    public interface ITapInput
    {
        public event Action<Vector2> OnTap; 
    }
}