using UnityEngine;
using UnityEditor;
using UnityEditor.ShortcutManagement;
using static System.IO.Path;

namespace Iteria.EditorTooling
{
	public static class EditorTools
	{
		[MenuItem("File/Open Persistent Data Folder", priority = 225)]
		public static void OpenPersistentData()
		{
			var path = GetFullPath(Application.persistentDataPath);
			System.Diagnostics.Process.Start("explorer", @path);
		}

		[MenuItem("Tools/Editor/Rotate 90"), Shortcut("Iteria/Rotate 90", KeyCode.R, ShortcutModifiers.Shift)]
		public static void Rotate90()
		{
			if(EditorWindow.focusedWindow as SceneView == null)
				return;

			if(Selection.activeTransform != null)
			{
				var transforms = Selection.GetTransforms(SelectionMode.TopLevel);

				Vector3 center = Vector3.zero;
				foreach(var t in transforms)
					center += t.position;
				center /= transforms.Length;

				Undo.RecordObjects(transforms, "Rotate 90 Degrees");
				foreach(var t in transforms)
				{
					if(Tools.pivotMode == PivotMode.Pivot)
						t.RotateAround(Selection.activeTransform.position, Vector3.up, 90f);
					else
						t.RotateAround(center, Vector3.up, 90f);
				}
			}
		}
	}
}