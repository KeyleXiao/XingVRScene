using UnityEngine;
using UnityEngine.EventSystems;

namespace UnityEngine.UI
{
	public class SquareJoystick : Joystick
	{
		public override Vector2 Coordinates
		{
			get
			{
				var coordinates = handle.anchoredPosition;
				if ((coordinates - initialPosition).magnitude < deadZoneRadius)
					return Vector2.zero;
				return new Vector2(Mathf.Clamp(coordinates.x, -radius, radius), Mathf.Clamp(coordinates.y, -radius, radius)) / radius;
			}
		}

		protected override Vector2 GetJoystickOffset(PointerEventData eventData)
		{
			Vector3 globalHandle;
			if (RectTransformUtility.ScreenPointToWorldPointInRectangle(_canvas, eventData.position, eventData.pressEventCamera, out globalHandle))
				handle.position = globalHandle;
			var handleOffset = handle.anchoredPosition;
			handleOffset = new Vector2(Mathf.Clamp(handleOffset.x, -radius, radius), Mathf.Clamp(handleOffset.y, -radius, radius));
			if (deadZoneMagnet && (handleOffset - initialPosition).magnitude < deadZoneRadius)
			{
				handle.anchoredPosition = initialPosition;
				return initialPosition;
			}
			return handleOffset;
		}
	}
}