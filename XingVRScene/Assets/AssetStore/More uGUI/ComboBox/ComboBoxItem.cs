using UnityEngine;
using UnityEngine.UI;
using System;

namespace UnityEngine.UI
{
	[Serializable]
	public class ComboBoxItem
	{
		[SerializeField]
		private string _caption;
		public string Caption
		{
			get
			{
				return _caption;
			}
			set
			{
				_caption = value;
			}
		}

		[SerializeField]
		private Sprite _icon;
		public Sprite Icon
		{
			get
			{
				return _icon;
			}
			set
			{
				_icon = value;
			}
		}

		[SerializeField]
		private bool _isSelected;
		public bool IsSelected
		{
			get
			{
				return _isSelected;
			}
			set
			{
				_isSelected = value;
			}
		}

		[SerializeField]
		private bool _isDisabled;
		public bool IsDisabled
		{
			get
			{
				return _isDisabled;
			}
			set
			{
				_isDisabled = value;
			}
		}

		public Action OnSelect;

		public ComboBoxItem(string caption, Sprite icon, bool disabled, Action onSelect)
		{
			_caption = caption;
			_icon = icon;
			_isDisabled = disabled;
			OnSelect = onSelect;
		}

		public ComboBoxItem(string caption, Sprite icon, bool disabled)
			: this(caption, icon, disabled, null)
		{
		}

		public ComboBoxItem(string caption, Sprite icon, Action onSelect)
			: this(caption, icon, false, onSelect)
		{
		}

		public ComboBoxItem(string caption, Sprite icon)
			: this(caption, icon, false, null)
		{
		}

		public ComboBoxItem(string caption, bool disabled)
			: this(caption, null, disabled, null)
		{
		}

		public ComboBoxItem(Sprite icon, bool disabled)
			: this(null, icon, disabled, null)
		{
		}

		public ComboBoxItem(string caption, Action onSelect)
			: this(caption, null, false, onSelect)
		{
		}

		public ComboBoxItem(Sprite icon, Action onSelect)
			: this(null, icon, false, onSelect)
		{
		}

		public ComboBoxItem(string caption)
			: this(caption, null, false, null)
		{
		}

		public ComboBoxItem(Sprite icon)
			: this(null, icon, false, null)
		{
		}
	}
}