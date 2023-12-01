using System;
using UnityEngine;

namespace MyAssets.Scripts.MyInput
{
    public interface ITapInput
    {
        public event Action<Vector2> OnTap; 
    }
}