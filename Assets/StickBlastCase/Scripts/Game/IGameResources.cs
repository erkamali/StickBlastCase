using UnityEngine;

namespace StickBlastCase.Game
{
    public interface IGameResources
    {
        //  METHODS
        Color       GetColor(int color);
        GameObject  GetShapePrefab(int index);
    }
}