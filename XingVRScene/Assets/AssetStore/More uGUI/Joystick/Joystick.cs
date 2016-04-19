using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;

namespace UnityEngine.UI
{
	[RequireComponent(typeof(RectTransform))]
	public abstract class Joystick : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
	{
		public RectTransform handle;
		public Vector2 initialPosition = Vector2.zero;
		public Vector2 autoReturnSpeed = new Vector2(4.0f, 4.0f);
		public float radius = 40.0f;
		public bool deadZoneMagnet = true;
		public float deadZoneRadius = 10.0f;
	
		public event Action<Joystick, Vector2> OnStartJoystickMovement;
		public event Action<Joystick, Vector2> OnJoystickMovement;
		public event Action<Joystick> OnEndJoystickMovement;
	
		private bool _returnHandle;
		protected RectTransform _canvas;
	
		public abstract Vector2 Coordinates
		{
			get;
		}
	
		void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
		{
			_returnHandle = false;
			var handleOffset = GetJoystickOffset(eventData);
			handle.anchoredPosition = handleOffset;
			if (OnStartJoystickMovement != null)
				OnStartJoystickMovement(this, Coordinates);
		}
	
		void IDragHandler.OnDrag(PointerEventData eventData)
		{
			var handleOffset = GetJoystickOffset(eventData);
			handle.anchoredPosition = handleOffset;
			if (OnJoystickMovement != null)
				OnJoystickMovement(this, Coordinates);
		}
	
		void IPointerUpHandler.OnPointerUp(PointerEventData eventData)
		{
			_returnHandle = true;
			if (OnEndJoystickMovement != null)
				OnEndJoystickMovement(this);
		}
	
		protected abstract Vector2 GetJoystickOffset(PointerEventData eventData);
	
		private void Start()
		{
			_returnHandle = true;
			handle.transform.SetParent(transform, false);
			var curTransform = transform;
			do
			{
				if (curTransform.GetComponent<Canvas>() != null)
				{
					_canvas = curTransform.GetComponent<RectTransform>();
					break;
				}
				curTransform = transform.parent;
			}
			while (curTransform != null);
		}
	
		private void Update()
		{
			if (_returnHandle)
			{
				var offsetPosition = handle.anchoredPosition - initialPosition;
				if (offsetPosition.magnitude > Mathf.Epsilon)
				{
					offsetPosition -= Vector2.Scale(offsetPosition, autoReturnSpeed) * Time.deltaTime;
					handle.anchoredPosition = offsetPosition + initialPosition;
				}
				else
				{
					handle.anchoredPosition = initialPosition;
					_returnHandle = false;
				}
			}
		}
	}
}