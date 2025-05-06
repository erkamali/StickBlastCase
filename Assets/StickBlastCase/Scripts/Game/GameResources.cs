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


        //  METHODS
        public Color    GetColor(int color) {   return _colors[color];   }
    }
}    
