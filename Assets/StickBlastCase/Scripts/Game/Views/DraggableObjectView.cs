using UnityEngine;
using UnityEngine.UI;

namespace StickBlastCase.Game.Views
{
    public class DraggableObjectView : MonoBehaviour,
                                       IDraggableObjectView
    {
        //  MEMBERS
        [SerializeField] private RectTransform  _childRect;
        [SerializeField] private Image          _childImage;
        
        //  METHODS
        public void SetSize(float width, float height)
        {
            _childRect.sizeDelta = new Vector2(width, height);
        }
    }
}