using System;
using System.Collections.Generic;
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
        
        [SerializeField] private List<GridCellView> _cells;
        //      Private
        private int _id;
        private Vector2 _originalPos;
        private RectTransform _rectTransform;
        private Action<int> _onSelect;
        private Action<int, PointerEventData> _onDrag;
        private Action<int> _onDeselect;
        
        //  METHODS
        public void Initialize(int id, float width, Vector2 originalPos, Action<int> onSelected, Action<int, PointerEventData> onDragged, Action<int> onDeselected)
        {
            _id = id;
            
            _originalPos = originalPos;
            _rectTransform = GetComponent<RectTransform>();
            _rectTransform.anchoredPosition = _originalPos;
                
            _onSelect   = onSelected;
            _onDrag     = onDragged;
            _onDeselect = onDeselected;
            
            _childRect.sizeDelta = new Vector2(width, width);
        }

        public void CancelDrag()
        {
            _rectTransform.anchoredPosition = _originalPos;
        }
        
        public void EndDragAndPlace(IGridCellView gridCell)
        {
            _rectTransform.SetParent(gridCell.transform, worldPositionStays: false);

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
            _rectTransform.anchoredPosition += eventData.delta;
            
            PointerEventData pointerData = new PointerEventData(EventSystem.current)
            {
                position = eventData.position
            };
            
            _onDrag(_id, pointerData);
        }
        
        public void UpdateDrag(Vector3 position)
        {
            transform.position = position;
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            _onDeselect(_id);
        }

        public IGridCellView GetCell(int cellIndex)
        {
            return _cells[cellIndex];
        }

        public List<GridCellView> GetCells()
        {
            return _cells;
        }
    }
}