using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEditor;
using System.Collections;

namespace UnityEditor.UI
{
	[CustomEditor(typeof(ComboBox))]
	public class ComboBoxEditor : Editor 
	{
		public override void OnInspectorGUI()
		{
			var comboBox = target as ComboBox;
			EditorGUI.BeginChangeCheck();
			DrawDefaultInspector();
			if (EditorGUI.EndChangeCheck())
			{
				comboBox.Start();
				comboBox.FirstIsDefault = comboBox.FirstIsDefault || comboBox.HideFirst;
				comboBox.Interactable = comboBox.Interactable;
				comboBox.DropDownIsShown = EditorApplication.isPlaying && comboBox.DropDownIsShown;
				comboBox.AllowMultiselect = comboBox.AllowMultiselect;
			}
		}
	}

	public class ComboBoxMenuItem
	{
		[MenuItem("GameObject/UI/ComboBox")]
		public static void CreateComboBox()
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
				Undo.RegisterCreatedObjectUndo(goEventSystem, "Created event system");
				eventSystem = goEventSystem.AddComponent<EventSystem>();
				goEventSystem.AddComponent<StandaloneInputModule>();
				goEventSystem.AddComponent<TouchInputModule>();
			}
			var goComboBox = new GameObject("ComboBox");
				Undo.RegisterCreatedObjectUndo(goComboBox, "Created comboBox");
			goComboBox.transform.SetParent(goCanvas.transform, false);
			var rtComboBox = goComboBox.AddComponent<RectTransform>();
			rtComboBox.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 160);
			rtComboBox.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 30);
			for (var i = 0; i < Selection.objects.Length; i++)
			{
				var selected = Selection.objects[i] as GameObject;
				var hierarchyItem = selected.transform;
				canvas = null;
				while (hierarchyItem != null && (canvas = hierarchyItem.GetComponent<Canvas>()) == null)
					hierarchyItem = hierarchyItem.parent;
				if (canvas != null)
				{
					goComboBox.transform.SetParent(selected.transform, false);
					break;
				}
			}
			rtComboBox.anchoredPosition = Vector2.zero;
			var comboBox = goComboBox.AddComponent<ComboBox>();
			LoadAssets();
			comboBox.UISprite = UISprite;
			comboBox.Background = Background;
			comboBox.font = Font_Arial;
			comboBox.Start();
			Selection.activeGameObject = goComboBox;
		}

		private static Sprite UISprite;
		private static Sprite Background;
		private static Font Font_Arial;
		public static void LoadAssets()
		{
			UISprite = UISprite ?? AssetDatabase.GetBuiltinExtraResource<Sprite>("UI/Skin/UISprite.psd");
			Background = Background ?? AssetDatabase.GetBuiltinExtraResource<Sprite>("UI/Skin/Background.psd");
			Font_Arial = Font_Arial ?? Resources.GetBuiltinResource(typeof(Font), "Arial.ttf") as Font;
		}
	}
}