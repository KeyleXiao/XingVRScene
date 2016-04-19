
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;

namespace UnityEngine.UI
{
	[RequireComponent(typeof(RectTransform))]
	public class ComboBox : MonoBehaviour
	{
        public Sprite UISprite;
        public Sprite Background;
        public Font font;

        [SerializeField]
        private bool _interactable = true;
        public bool Interactable
        {
        	get
        	{
        		return _interactable;
        	}
        	set
        	{
        		_interactable = value;
        		var btn = _rt.GetComponent<Button>();
        		btn.interactable = _interactable;
        	}
        }

		private bool _comboHasIcons;
		private bool ComboHasIcons
		{
			get
			{
				foreach (var item in _items)
					if (item.Icon != null)
						return true;
				return false;
			}
		}

		[SerializeField]
		private int _padding = 7;
		public int Padding
		{
			get
			{
				return _padding;
			}
		}

		[SerializeField]
		private int _elementsToDisplay = 4;
		public int ElementsToDisplay
		{
			get
			{
				return _elementsToDisplay;
			}
		}

		[SerializeField]
		private bool _hideFirst = true;
		public bool HideFirst
		{
			get
			{
				return _hideFirst;
			}
			set
			{
				_hideFirst = value;
				if (_hideFirst)
					FirstIsDefault = true;
			}
		}

		[SerializeField]
		private bool _firstIsDefault = true;
		public bool FirstIsDefault
		{
			get
			{
				return _firstIsDefault;
			}
			set
			{
				_firstIsDefault = value;
			}
		}

		[SerializeField]
		private bool _allowMultiselect = false;
		public bool AllowMultiselect
		{
			get
			{
				return _allowMultiselect;
			}
			set
			{
				_allowMultiselect = value;
				if (!_allowMultiselect)
				{
					var selectedIndexes = GetSelectedIndexes();
					if (selectedIndexes.Length > 1)
					{
						SetSelected(selectedIndexes[0]);
						Redraw();
					}
				}
			}
		}

		private int _selectedItem = 0;
		private int SelectedCount
		{
			get
			{
				var selectedCount = 0;
				for(var i = _items.Length - 1; i >= 0; i--)
					if (_items[i].IsSelected)
					{
						_selectedItem = i;
						selectedCount++;
					}
				return selectedCount;
			}
		}

		[SerializeField]
		private bool _dropDownIsShown = false;
		public bool DropDownIsShown
		{
			get
			{
				return _dropDownIsShown;
			}
			set
			{
				if (value)
					ShowDropDown();
				else
					HideDropDown();
				_dropDownIsShown = value;
			}
		}

		[SerializeField]
		private ComboBoxItem[] _items;
		public ComboBoxItem[] Items
		{
			get
			{
				_items = _items ?? new ComboBoxItem[0];
				return _items;
			}
			set
			{
				_items = value;
				Redraw();
			}
		}

        public ComboBoxItem this[int index]
        {
        	get
        	{
        		return _items[index];
        	}
        	set
        	{
        		_items[index] = value;
        	}
        }

        public event Action<ComboBox> OnSelect;
		public Func<ComboBoxItem[], string> MultiselectCaption;

		private Transform _canvas;
		private Transform _parent;
		private RectTransform _rt;
		private RectTransform _rtText;
		private RectTransform _rtIcon;

		private bool _changesDone = false;

		private List<ScrollRect> _scrollRects;
		private List<GraphicRaycaster> _graphicRaycasters;

		private int _scrollOffset;
		
		private void InitParts()
		{
			var tmp = transform;
			while (_canvas == null)
			{
				if (tmp.GetComponent<Canvas>() != null)
				{
					_canvas = tmp;
					break;
				}
				tmp = tmp.parent;
			}

			_rt = GetComponent<RectTransform>();

			_parent = _rt.parent;

			var img = _rt.GetComponent<Image>();
			if (img == null)
			{
				img = _rt.gameObject.AddComponent<Image>();
				img.type = Image.Type.Sliced;
				img.sprite = UISprite;
			}

			var btn = _rt.GetComponent<Button>();
			if (btn == null)
			{
				btn = _rt.gameObject.AddComponent<Button>();
				btn.targetGraphic = img;
			}
			btn.onClick.RemoveAllListeners();
			btn.onClick.AddListener(ShowDropDown);

			var height = _rt.rect.height;
			
			var rtArrow = (RectTransform)_rt.Find("Arrow");
			if (rtArrow == null)
			{
				var go = new GameObject("Arrow");
				rtArrow = go.AddComponent<RectTransform>();
				rtArrow.SetParent(_rt);
				rtArrow.pivot = new Vector2(1.0f, 0.5f);
				rtArrow.anchorMin = Vector2.right;
				rtArrow.anchorMax = Vector2.one;
				rtArrow.offsetMin = new Vector2(-height, 0.0f);
				rtArrow.offsetMax = Vector2.zero;
				rtArrow.localScale = new Vector3(1.0f, 0.5f, 1.0f);

				var txtArrow = go.AddComponent<Text>();
				txtArrow.text = "▼";
				txtArrow.font = font;
				txtArrow.color = new Color32(50, 50, 50, 255);
				txtArrow.alignment = TextAnchor.MiddleCenter;
			}

			_rtText = (RectTransform)_rt.Find("Text");
			if (_rtText == null)
			{
				var go = new GameObject("Text");
				_rtText = go.AddComponent<RectTransform>();
				_rtText.SetParent(_rt, false);
				_rtText.pivot = new Vector2(0.5f, 0.5f);
				_rtText.anchorMin = Vector2.zero;
				_rtText.anchorMax = Vector2.one;

				var txtText = _rtText.gameObject.AddComponent<Text>();
				txtText.font = font;
				txtText.color = new Color32(50, 50, 50, 255);
				txtText.alignment = TextAnchor.MiddleLeft;
			}

			_rtIcon = (RectTransform)_rt.Find("Icon");
			if (_rtIcon == null)
			{
				var go = new GameObject("Icon");
				_rtIcon = go.AddComponent<RectTransform>();
				_rtIcon.SetParent(_rt, false);
				_rtIcon.pivot = new Vector2(0.0f, 0.5f);
				_rtIcon.anchorMin = Vector2.zero;
				_rtIcon.anchorMax = Vector2.up;
				_rtIcon.offsetMin = new Vector2(Padding, Padding);
				_rtIcon.offsetMax = new Vector2(height - Padding, -Padding);

				var imgIcon = _rtIcon.gameObject.AddComponent<Image>();
				imgIcon.type = Image.Type.Sliced;
				imgIcon.color = Color.clear;
			}

			_scrollRects = _scrollRects ?? new List<ScrollRect>();
			_graphicRaycasters = _graphicRaycasters ?? new List<GraphicRaycaster>();
		}

		private void Redraw()
		{
			var selectedCount = SelectedCount;
			switch (selectedCount)
			{
				case 1:
					_rtText.GetComponent<Text>().text = _items[_selectedItem].Caption;
					var icon = _items.Length < 1 ? null : _items[_selectedItem].Icon;
					var img = _rtIcon.GetComponent<Image>();
					img.sprite = icon;
					img.color = icon == null ? Color.clear : Color.white;
					var textOffset = Padding + ((icon == null) ? 0.0f : _rt.rect.height);
					_rtText.offsetMin = new Vector2(textOffset, Padding);
					_rtText.offsetMax = new Vector2(-textOffset, -Padding);
					break;
				default:
					_rtText.GetComponent<Text>().text = _items.Length < 1 ? null : MultiselectCaption == null ? "Selected " + selectedCount + " items" : MultiselectCaption(GetSelectedItems());
					_rtText.offsetMin = new Vector2(Padding, Padding);
					_rtText.offsetMax = new Vector2(-Padding, -Padding);
					_rtIcon.GetComponent<Image>().color = Color.clear;
					break;
			}
		}

		public void Start()
		{
			if (_items == null)
				_items = new ComboBoxItem[0];
			if (_rt == null)
				InitParts();
			Redraw();
		}

		public void ShowDropDown()
		{
			if (_parent != _rt.parent)
				return;
			_dropDownIsShown = true;

			_parent = _rt.parent;

			var height = _rt.rect.height;
			var ddHeight = Mathf.Max(0.0f, Mathf.Min(ElementsToDisplay, _items.Length - (HideFirst ? 1 : 0)) * height);

			var go = new GameObject("Overlay");
			var rtOverlay = go.AddComponent<RectTransform>();
			rtOverlay.SetParent(_canvas, false);
			rtOverlay.pivot = Vector2.zero;
			rtOverlay.anchorMin = Vector2.zero;
			rtOverlay.anchorMax = Vector2.one;
			rtOverlay.offsetMin = Vector2.zero;
			rtOverlay.offsetMax = Vector2.zero;
			go.AddComponent<GraphicRaycaster>();
			go.GetComponent<Canvas>().overrideSorting = false;

			_rt.GetComponent<Button>().onClick.RemoveAllListeners();
			_rt.GetComponent<Button>().onClick.AddListener(HideDropDown);

			var img = go.AddComponent<Image>();
			img.color = Color.clear;

			var btn = go.AddComponent<Button>();
			btn.targetGraphic = img;
			btn.onClick.AddListener(HideDropDown);

			go = new GameObject("DropDown");
			var rtDD = go.AddComponent<RectTransform>();
			rtDD.SetParent(_rt, false);
			rtDD.pivot = new Vector2(0.5f, 1.0f);
			rtDD.anchorMin = Vector2.zero;
			rtDD.anchorMax = Vector2.right;
			rtDD.offsetMin = new Vector2(0.0f, -ddHeight);
			rtDD.offsetMax = Vector2.zero;

			var imgDD = go.AddComponent<Image>();
			imgDD.type = Image.Type.Sliced;
			imgDD.sprite = UISprite;

			_rt.SetParent(rtOverlay, false);
			rtDD.SetParent(rtOverlay, true);
			if (rtDD.offsetMax.y < ddHeight)
			{
				var offset = new Vector2(0.0f, ddHeight + height);
				rtDD.offsetMin += offset;
				rtDD.offsetMax += offset;
			}

			var tmp = rtOverlay.parent;
			ScrollRect scrollRect;
			GraphicRaycaster graphicRaycaster;
			while (tmp != null)
			{
				if ((scrollRect = tmp.GetComponent<ScrollRect>()) != null && scrollRect.IsActive())
				{
					scrollRect.enabled = false;
					_scrollRects.Add(scrollRect);
				}
				if ((graphicRaycaster = tmp.GetComponent<GraphicRaycaster>()) != null && graphicRaycaster.IsActive())
				{
					graphicRaycaster.enabled = false;
					_graphicRaycasters.Add(graphicRaycaster);
				}
				tmp = tmp.parent;
			}

			if (_items.Length - (HideFirst ? 1 : 0) > ElementsToDisplay)
			{
				var scrollWidth = height / 3.0f * 2.0f;

				scrollRect = go.AddComponent<ScrollRect>();
	            scrollRect.horizontal = false;
	            scrollRect.elasticity = 0.0f;
	            scrollRect.movementType = ScrollRect.MovementType.Clamped;
	            scrollRect.inertia = false;
	            scrollRect.scrollSensitivity = height;
	            go.AddComponent<Mask>();

	            var goItems = new GameObject("Items");
	            var rtItems = goItems.AddComponent<RectTransform>();
	            rtItems.SetParent(rtDD, false);
	            rtItems.anchorMin = Vector2.up;
	            rtItems.anchorMax = Vector2.one;
	            rtItems.pivot = Vector2.up;
				rtItems.offsetMin = new Vector2(0, -(_items.Length - (HideFirst ? 1 : 0)) * height);
				rtItems.offsetMax = new Vector2(-scrollWidth, 0.0f);
	            scrollRect.content = rtItems;

				var goScrollbar = new GameObject("Scrollbar");
				var rtScrollbar = goScrollbar.AddComponent<RectTransform>();
				rtScrollbar.SetParent(rtDD, false);
				rtScrollbar.anchorMin = Vector2.right;
				rtScrollbar.anchorMax = Vector2.one;
				rtScrollbar.pivot = new Vector2(1.0f, 0.5f);
				rtScrollbar.offsetMin = new Vector2(-20.0f, 0.0f);
				rtScrollbar.offsetMax = Vector2.zero;
				var imgScrollbar = goScrollbar.AddComponent<Image>();
				imgScrollbar.sprite = Background;
            	imgScrollbar.type = Image.Type.Sliced;
				var sbScrollbar = goScrollbar.AddComponent<Scrollbar>();
	            var sbColors = new ColorBlock();
	            sbColors.normalColor = new Color32(128, 128, 128, 128);
	            sbColors.highlightedColor = new Color32(128, 128, 128, 178);
	            sbColors.pressedColor = new Color32(88, 88, 88, 178);
	            sbColors.disabledColor = new Color32(64, 64, 64, 128);
	            sbColors.colorMultiplier = 2.0f;
	            sbColors.fadeDuration = 0.1f;
	            sbScrollbar.colors = sbColors;
	            sbScrollbar.direction = Scrollbar.Direction.BottomToTop;
	            scrollRect.verticalScrollbar = sbScrollbar;

	            var goSlidingArea = new GameObject("SlidingArea");
	            var rtSlidingArea = goSlidingArea.AddComponent<RectTransform>();
	            rtSlidingArea.SetParent(rtScrollbar, false);
	            rtSlidingArea.anchorMin = Vector2.zero;
	            rtSlidingArea.anchorMax = Vector2.one;
	            rtSlidingArea.offsetMin = new Vector2(scrollWidth / 2.0f, scrollWidth / 2.0f);
	            rtSlidingArea.offsetMax = new Vector2(-scrollWidth / 2.0f, -scrollWidth / 2.0f);

	            var goHandle = new GameObject("Handle");
	            var rtHandle = goHandle.AddComponent<RectTransform>();
	            rtHandle.SetParent(rtSlidingArea, false);
	            rtHandle.anchorMin = Vector2.zero;
	            rtHandle.anchorMax = Vector2.one;
	            rtHandle.offsetMin = new Vector2(-scrollWidth / 2.0f, -scrollWidth / 2.0f);
	            rtHandle.offsetMax = new Vector2(scrollWidth / 2.0f, scrollWidth / 2.0f);
	            var imgHandle = goHandle.AddComponent<Image>();
	            imgHandle.sprite = UISprite;
	            imgHandle.type = Image.Type.Sliced;
	            imgHandle.color = new Color32(255, 255, 255, 150);
	            sbScrollbar.targetGraphic = imgHandle;
	            sbScrollbar.handleRect = rtHandle;

	            if (SelectedCount > 0)
	            {
	            	var selectedIsShown = false;
	            	var elementsOffset = _scrollOffset + (HideFirst ? 1 : 0);
	            	for (var i = elementsOffset; i < elementsOffset + ElementsToDisplay; i++)
	            		if (_items[i].IsSelected)
	            		{
	            			selectedIsShown = true;
	            			break;
	            		}
	            	if (!selectedIsShown)
	            		_scrollOffset = _selectedItem < 1 ? _selectedItem : _selectedItem - (HideFirst ? 1 : 0);
	            	if (_scrollOffset + ElementsToDisplay > _items.Length - (HideFirst ? 1 : 0))
	            		_scrollOffset = _items.Length - ElementsToDisplay - (HideFirst ? 1 : 0);
	            }

				rtItems.anchoredPosition = new Vector2(0.0f, _scrollOffset * height);

	            rtDD = rtItems;
			}

			var textOffset = Padding + (ComboHasIcons ? (int)_rt.rect.height : 0);
			for (var i = HideFirst ? 1 : 0; i < _items.Length; i++)
			{
				var item = _items[i];
				var offset = ((HideFirst ? 0 : -1) - i) * height;
				var goItem = new GameObject("Item" + i);
				var rtItem = goItem.AddComponent<RectTransform>();
				rtItem.SetParent(rtDD, false);
				rtItem.anchorMin = Vector2.up;
				rtItem.anchorMax = Vector2.one;
				rtItem.offsetMin = new Vector2(0.0f, offset);
				rtItem.offsetMax = new Vector2(0.0f, offset + height);
				var imgItem = goItem.AddComponent<Image>();
				imgItem.type = Image.Type.Sliced;
				imgItem.sprite = UISprite;
				var btnItem = goItem.AddComponent<Button>();
				btnItem.targetGraphic = imgItem;
				if (item.IsSelected)
					imgItem.color = btnItem.colors.pressedColor;
				var index = i;
				if (item.IsDisabled)
					btnItem.interactable = true;
				else
					btnItem.onClick.AddListener(
						delegate()
						{
							_changesDone = true;
							if (!AllowMultiselect)
							{
								SetSelected(index);
								HideDropDown();
							}
							else
							{
								item.IsSelected = !item.IsSelected;
								imgItem.color = item.IsSelected ? btnItem.colors.pressedColor : btnItem.colors.normalColor;
								if (FirstIsDefault)
									_items[0].IsSelected = !_items[0].IsSelected && SelectedCount < 1;
							}
							Redraw();
		                    if (item.OnSelect != null)
		                        item.OnSelect();
						}
					);

				if (item.IsDisabled)
					imgItem.color = btnItem.colors.disabledColor;

				if (item.Caption != null)
				{
					var goText = new GameObject("Text");
					var rtText = goText.AddComponent<RectTransform>();
					rtText.SetParent(rtItem, false);
					rtText.pivot = new Vector2(0.5f, 0.5f);
					rtText.anchorMin = Vector2.zero;
					rtText.anchorMax = Vector2.one;
					rtText.offsetMin = new Vector2(textOffset, Padding);
					rtText.offsetMax = new Vector2(-textOffset, -Padding);

					var txtText = rtText.gameObject.AddComponent<Text>();
					txtText.font = font;
					txtText.color = (Color)(!item.IsSelected && item.IsDisabled ? new Color32(174, 174, 174, 255) : new Color32(50, 50, 50, 255));
					txtText.alignment = TextAnchor.MiddleLeft;
					txtText.text = item.Caption;
				}

				if (item.Icon != null)
				{
					var goIcon = new GameObject("Icon");
					var rtIcon = goIcon.AddComponent<RectTransform>();
					rtIcon.SetParent(rtItem, false);
					rtIcon.pivot = new Vector2(0.0f, 0.5f);
					rtIcon.anchorMin = Vector2.zero;
					rtIcon.anchorMax = Vector2.up;
					rtIcon.offsetMin = new Vector2(Padding, Padding);
					rtIcon.offsetMax = new Vector2(height - Padding, -Padding);

					var imgIcon = goIcon.AddComponent<Image>();
					imgIcon.type = Image.Type.Sliced;
					imgIcon.sprite = item.Icon;
					imgIcon.color = !item.IsSelected && item.IsDisabled ? (Color)new Color32(255, 255, 255, 147) : Color.white;
				}
			}
		}

		public void HideDropDown()
		{
			if (_parent == _rt.parent)
				return;
			_dropDownIsShown = false;

			if (_changesDone && OnSelect != null)
				OnSelect(this);
			_changesDone = false;

			var rtItems = _rt.parent.Find("DropDown/Items") as RectTransform;
			_scrollOffset = rtItems == null ? 0 : (int)Mathf.Round(rtItems.anchoredPosition.y / _rt.rect.height);

			foreach (var scrollRect in _scrollRects)
				scrollRect.enabled = true;
			_scrollRects.Clear();
			foreach (var graphicRaycaster in _graphicRaycasters)
				graphicRaycaster.enabled = true;
			_graphicRaycasters.Clear();
			var overlayGO = _rt.parent.gameObject;
			_rt.SetParent(_parent, false);
			DestroyImmediate(overlayGO);
			_rt.GetComponent<Button>().onClick.RemoveAllListeners();
			_rt.GetComponent<Button>().onClick.AddListener(ShowDropDown);
		}

		public int[] GetSelectedIndexes()
		{
			var selectedIndexes = new List<int>();
			for (var i = 0; i < Items.Length; i++)
				if (_items[i].IsSelected)
					selectedIndexes.Add(i);
			return selectedIndexes.ToArray();
		}

		public ComboBoxItem[] GetSelectedItems()
		{
			var selectedItems = new List<ComboBoxItem>();
			for (var i = 0; i < Items.Length; i++)
				if (_items[i].IsSelected)
					selectedItems.Add(_items[i]);
			return selectedItems.ToArray();
		}

		public void SetSelected(params int[] selected)
		{
			if (selected == null)
				return;
			for (var i = Items.Length - 1; i >= 0; i--)
			{
				_items[i].IsSelected = false;
				for (var j = selected.Length - 1; j >= 0; j--)
					if (selected[j] == i)
					{
						_items[i].IsSelected = true;
						break;
					}
			}
		}

        public void AddItems(params object[] list)
        {
            var cbItems = new List<ComboBoxItem>();
            foreach (var obj in list)
            {
                if (obj is ComboBoxItem)
                {
                    var item = (ComboBoxItem)obj;
                    cbItems.Add(item);
                    continue;
                }
                if (obj is string)
                {
                    var item = new ComboBoxItem((string)obj, null, false, null);
                    cbItems.Add(item);
                    continue;
                }
                if (obj is Sprite)
                {
                    var item = new ComboBoxItem(null, (Sprite)obj, false, null);
                    cbItems.Add(item);
                    continue;
                }
                throw new ArgumentException("Only ComboBoxItem, string and Sprite types are allowed", "list");
            }
            var newItems = new ComboBoxItem[Items.Length + cbItems.Count];
            _items.CopyTo(newItems, 0);
            cbItems.ToArray().CopyTo(newItems, _items.Length);
            var updateSelected = _items.Length < 1 && newItems.Length > 0 && FirstIsDefault;
            _items = newItems;
            if (updateSelected)
            	SetSelected(0);
			Redraw();
        }
	}
}