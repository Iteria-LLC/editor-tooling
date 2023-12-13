using System.Reflection;
using static System.Reflection.BindingFlags;
using UnityEditor;
using UnityEditor.ShortcutManagement;

namespace Iteria.EditorTooling
{
	//https://github.com/Unity-Technologies/UnityCsReference/blob/e740821767d2290238ea7954457333f06e952bad/Editor/Mono/Annotation/AnnotationUtility.bindings.cs
	public static class AnnotationUtilityEx
	{
		const string showSelectionOutline = "showSelectionOutline";
		const string showSelectionWire = "showSelectionWire";
		const string drawGizmos = "drawGizmos";

		[Shortcut("Toggle Gizmos")]
		public static void ToggleGizmos()
		{
			SceneView.lastActiveSceneView.drawGizmos = !SceneView.lastActiveSceneView.drawGizmos;
		}

		[MenuItem("Tools/Editor/Toggle Gizmos + Selection Highlight"), Shortcut("Toggle Gizmos & Selection Highlight", UnityEngine.KeyCode.Space, ShortcutModifiers.Control)]
		public static void ToggleSelectionOutlinesAndGizmos()
		{
			if(ShowSelectionOutline || ShowSelectionWire)
			{
				EditorPrefs.SetBool(showSelectionOutline, ShowSelectionOutline);
				EditorPrefs.SetBool(showSelectionWire, ShowSelectionWire);
				EditorPrefs.SetBool(drawGizmos, SceneView.lastActiveSceneView.drawGizmos);

				SceneView.lastActiveSceneView.drawGizmos = false;
				ShowSelectionOutline = false;
				ShowSelectionWire = false;
			}
			else
			{
				SceneView.lastActiveSceneView.drawGizmos = EditorPrefs.GetBool(drawGizmos);
				ShowSelectionOutline = EditorPrefs.GetBool(showSelectionOutline);
				ShowSelectionWire = EditorPrefs.GetBool(showSelectionWire);
			}
		}

		public static bool ShowSelectionOutline
		{
			get
			{
				if(_showSelectionOutline == null)
					_showSelectionOutline = AnnotationUtility.GetProperty(showSelectionOutline, Static | NonPublic);
				return (bool) _showSelectionOutline.GetValue(null);
			}
			set
			{
				if(_showSelectionOutline == null)
					_showSelectionOutline = AnnotationUtility.GetProperty(showSelectionOutline, Static | NonPublic);
				_showSelectionOutline.SetValue(null, value);
			}
		}
		static PropertyInfo _showSelectionOutline;

		public static bool ShowSelectionWire
		{
			get
			{
				if(_showSelectionWire == null)
					_showSelectionWire = AnnotationUtility.GetProperty(showSelectionWire, Static | NonPublic);
				return (bool) _showSelectionWire.GetValue(null);
			}
			set
			{
				if(_showSelectionWire == null)
					_showSelectionWire = AnnotationUtility.GetProperty(showSelectionWire, Static | NonPublic);
				_showSelectionWire.SetValue(null, value);
			}
		}
		static PropertyInfo _showSelectionWire;

		static System.Type AnnotationUtility
		{
			get
			{
				if(_annotationUtility == null)
				{
					var assembly = typeof(SceneViewCameraWindow).Assembly;
					_annotationUtility = assembly.GetType("UnityEditor.AnnotationUtility");
				}
				return _annotationUtility;
			}
		}
		static System.Type _annotationUtility;
	}
}
