using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace StickBlastCase.Game.Views
{
    public class DraggableObjectView : MonoBehaviour,
                                       IDraggableObjectView,
                                       IPointerDownHandler, 
                                       IDragHandler, 
                                       IPointerUpHandler
    {
        //  MEMBERS
        [SerializeField] private RectTransform  _childRect;
        [SerializeField] private Image          _childImage;
        //      Private
        private static readonly Vector2 DragOffset = new Vector2(0, 10f);
        private int _id;
        private Vector2 _originalPos;
        private RectTransform _rectTransform;
        private Action<int> _onSelect;
        private Action<int> _onDeselect;
        
        //  METHODS
        public void Initialize(int id, float width, float height, Vector2 originalPos, Action<int> onSelected, Action<int> onDeselected)
        {
            _id = id;
            
            _originalPos = originalPos;
            _rectTransform = GetComponent<RectTransform>();
            _rectTransform.anchoredPosition = _originalPos;
                
            _onSelect = onSelected;
            _onDeselect = onDeselected;
            
            _childRect.sizeDelta = new Vector2(width, height);
        }

        public void CancelDrag()
        {
            _rectTransform.anchoredPosition = _originalPos;
        }
        
        public void EndDragAndPlace(IGridGapView gap)
        {
            RectTransform rt = GetComponent<RectTransform>();
            rt.SetParent(gap.Transform, worldPositionStays: false);

            // Animate snapping to center of the gap
            //rt.DOAnchorPos(Vector2.zero, 0.1f).SetEase(Ease.OutQuad)
            //    .OnComplete(() => _onPlaceCompleted?.Invoke(/* optionally gap.Col, gap.Row */));
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            _onSelect(_id);
        }

        public void StartDrag()
        {
            UnityEngine.Debug.Log("hop");
        }

        public void OnDrag(PointerEventData eventData)
        {
            /*
            Canvas canvas = GetComponentInParent<Canvas>();
            // Convert screen position to canvas local position
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                canvas.transform as RectTransform,
                eventData.position,
                eventData.pressEventCamera,
                out Vector2 localPoint
            );

            // Add slight offset (optional)
            Vector3 position = localPoint + Vector2.up * 10f;

            UpdateDrag(position);
            */
            
            RectTransform rt = GetComponent<RectTransform>();
            rt.anchoredPosition += eventData.delta;
        }
        
        public void UpdateDrag(Vector3 position)
        {
            transform.position = position;
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            _onDeselect(_id);
        }
    }
}