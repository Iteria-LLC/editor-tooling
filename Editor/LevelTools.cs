using UnityEngine;
using UnityEditor;
using UnityEditor.ShortcutManagement;
using static System.IO.Path;

public static class LevelTools
{
	[MenuItem("File/Run Existing Build", priority = 211)]
	public static void RunExistingBuild()
	{
		var path = EditorUserBuildSettings.GetBuildLocation(EditorUserBuildSettings.activeBuildTarget);
		if(System.IO.File.Exists(path))
			System.Diagnostics.Process.Start(path);
		else
			Debug.LogError($"Couldn't find {GetFileName(path)} in {GetDirectoryName(path)}. Aborting.");
	}

	[MenuItem("Tools/Editor/Rotate 90"), Shortcut("Rotate 90", KeyCode.R, ShortcutModifiers.Shift)]
	public static void Rotate90()
	{
		if(EditorApplication.isPlaying)
			return;

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
