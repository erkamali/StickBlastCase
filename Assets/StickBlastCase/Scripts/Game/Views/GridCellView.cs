using StickBlastCase.Game.Constants;
using UnityEngine;
using UnityEngine.UI;

namespace StickBlastCase.Game.Views
{
    public class GridCellView : MonoBehaviour, 
                                IGridCellView
    {
        //  MEMBERS
        public int              Col     { get; private set; }
        public int              Row     { get; private set; }
        public GridCellShapes   Shape   { get { return _shape; } }
        //      Editor
        [SerializeField] private GridCellShapes _shape;
        [SerializeField] private Image background;
        //      Private
        
        //  METHODS
        public void Initialize(int column, int row, GridCellShapes shape)
        {
            _shape = shape;
        }
        
        public void SetColor(Color color)
        {
            background.color = color;
        }
        
        public void SetFilled(bool filled)
        {
            background.color = filled ? Color.green : Color.white;
        }
    }
}