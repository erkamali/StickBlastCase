using System.Collections.Generic;
using UnityEngine;

namespace StickBlastCase.Game
{
    [CreateAssetMenu]
    public class GameResources : ScriptableObject,
                                 IGameResources
    {
        //  MEMBERS
        //      Editor
        [SerializeField] private List<Color>    _colors;
        [Header("Shape Prefabs")]
        [SerializeField] private List<GameObject> _shapePrefabs;


        //  METHODS
        public Color        GetColor(int color)         {   return _colors[color];   }
        public GameObject   GetShapePrefab(int index)   {   return _shapePrefabs[index];   }
    }
}    
