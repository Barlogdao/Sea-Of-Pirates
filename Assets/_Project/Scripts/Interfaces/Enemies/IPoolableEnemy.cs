using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Project.Interfaces.Enemies
{
    public interface IPoolableEnemy : IEnemy
    {
       Transform Transform { get; }
       void Respawn(Vector3 atPosition);
       UniTask SinkAsync(); 
    }
}