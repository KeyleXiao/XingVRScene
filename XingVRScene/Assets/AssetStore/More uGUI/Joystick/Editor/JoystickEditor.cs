using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEditor;
using System.Collections;

namespace UnityEditor.UI
{
	public class JoystickMenuItem
	{
		private enum JoystickType {
			Round,
			Square,
			Horizontal,
			Vertical
		}

		[MenuItem("GameObject/UI/Joystick/Round")]
		private static void CreateRoundJoystick()
		{
			CreateJoystick(JoystickType.Round);
		}

		[MenuItem("GameObject/UI/Joystick/Square")]
		private static void CreateSquareJoystick()
		{
			CreateJoystick(JoystickType.Square);
		}

		[MenuItem("GameObject/UI/Joystick/Horizontal")]
		private static void CreateHorizontalJoystick()
		{
			CreateJoystick(JoystickType.Horizontal);
		}

		[MenuItem("GameObject/UI/Joystick/Vertical")]
		private static void CreateVerticalJoystick()
		{
			CreateJoystick(JoystickType.Vertical);
		}

		private static void CreateJoystick(JoystickType type)
		{
			var canvas = Object.FindObjectOfType<Canvas>();
			var goCanvas = canvas == null ? null : canvas.gameObject;
			if (goCanvas == null)
			{
				goCanvas = new GameObject("Canvas");
				Undo.RegisterCreatedObjectUndo(goCanvas, "Created canvas");
				canvas = goCanvas.AddComponent<Canvas>();
				canvas.renderMode = RenderMode.ScreenSpaceOverlay;
				goCanvas.AddComponent<CanvasScaler>();
				goCanvas.AddComponent<GraphicRaycaster>();
			}
			var eventSystem = Object.FindObjectOfType<EventSystem>();
			var goEventSystem = eventSystem == null ? null : eventSystem.gameObject;
			if (goEventSystem == null)
			{
				goEventSystem = new GameObject("EventSystem");
				eventSystem = goEventSystem.AddComponent<EventSystem>();
				goEventSystem.AddComponent<StandaloneInputModule>();
				goEventSystem.AddComponent<TouchInputModule>();
				Undo.RegisterCreatedObjectUndo(goEventSystem, "Created event system");
			}
			var goJoystick = new GameObject("Joystick");
			Undo.RegisterCreatedObjectUndo(goJoystick, "Created joystick");
			goJoystick.transform.SetParent(goCanvas.transform, false);
			var rtJoystick = goJoystick.AddComponent<RectTransform>();
			Joystick joystick = null;
			switch (type)
			{
				case JoystickType.Round:
					joystick = goJoystick.AddComponent<RoundJoystick>();
					rtJoystick.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 160);
					rtJoystick.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 160);
					break;
				case JoystickType.Square:
					joystick = goJoystick.AddComponent<SquareJoystick>();
					rtJoystick.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 160);
					rtJoystick.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 160);
					break;
				case JoystickType.Horizontal:
					joystick = goJoystick.AddComponent<HorizontalJoystick>();
					rtJoystick.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 160);
					rtJoystick.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 30);
					break;
				case JoystickType.Vertical:
					joystick = goJoystick.AddComponent<VerticalJoystick>();
					rtJoystick.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 30);
					rtJoystick.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 160);
					break;
			}
			for (var i = 0; i < Selection.objects.Length; i++)
			{
				var selected = Selection.objects[i] as GameObject;
				var hierarchyItem = selected.transform;
				canvas = null;
				while (hierarchyItem != null && (canvas = hierarchyItem.GetComponent<Canvas>()) == null)
					hierarchyItem = hierarchyItem.parent;
				if (canvas != null)
				{
					rtJoystick.SetParent(selected.transform, false);
					break;
				}
			}
			rtJoystick.anchoredPosition = Vector2.zero;
			joystick.radius = 65;
			LoadAssets();
			var imgJoystick = goJoystick.AddComponent<Image>();
			imgJoystick.sprite = type == JoystickType.Round ? Knob : UISprite;
			var goHandle = new GameObject("Handle");
			var rtHandle = goHandle.AddComponent<RectTransform>();
			rtHandle.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 30);
			rtHandle.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 30);
			rtHandle.SetParent(rtJoystick, false);
			rtHandle.anchoredPosition = Vector2.zero;
			var imgHandle = goHandle.AddComponent<Image>();
			imgHandle.sprite = type == JoystickType.Round ? Knob : UISprite;
			if (type != JoystickType.Round)
			{
				imgJoystick.type = Image.Type.Sliced;
				imgHandle.type = Image.Type.Sliced;
			}
			joystick.handle = rtHandle;
			Selection.activeGameObject = goJoystick;
		}

		private static Sprite Knob;
		private static Sprite UISprite;
		public static void LoadAssets()
		{
			Knob = Knob ?? AssetDatabase.GetBuiltinExtraResource<Sprite>("UI/Skin/Knob.psd");
			UISprite = UISprite ?? AssetDatabase.GetBuiltinExtraResource<Sprite>("UI/Skin/UISprite.psd");
		}
	}
}