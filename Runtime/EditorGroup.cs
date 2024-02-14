#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using UnityEditor.ShortcutManagement;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine.SceneManagement;

namespace Iteria.EditorTooling
{
	[SelectionBase]
	public class EditorGroup : MonoBehaviour
	{
		public delegate void PostGroupCreated();
		public static PostGroupCreated postGroupCreated;

		public delegate void SetGroupPosition();
		public static SetGroupPosition setGroupPosition;

		[Shortcut("Create/Dissolve Editor Group", KeyCode.G, ShortcutModifiers.Control)]
		public static void GroupOrDissolve() => GroupSelection(false);

		[MenuItem("Tools/Group Selection", priority = 500000)]
		public static void MenuGroup() => GroupSelection(true);

		public static void GroupSelection(bool disallowDissolve)
		{
			if(Selection.objects.Length == 0)
			{
				SceneView.lastActiveSceneView.ShowNotification(new GUIContent("Can't create group. Nothing is selected."));
				return;
			}
			if(Selection.objects.Length == 1)
			{
				if(Selection.activeGameObject.TryGetComponent<EditorGroup>(out var group))
				{
					if(disallowDissolve)
						SceneView.lastActiveSceneView.ShowNotification(new GUIContent("An editor group is already selected."));
					else
						group.DissolveGroup();

					return;
				}
			}

			Undo.IncrementCurrentGroup();
			int id = Undo.GetCurrentGroup();

			Vector3 pos = Vector3.zero;
			for(int i = 0; i < Selection.transforms.Length; i++)
				pos += Selection.transforms[i].position;
			pos /= Selection.transforms.Length;

			setGroupPosition();

			var g = new GameObject("Group", typeof(EditorGroup));
			g.transform.position = pos;
			Undo.RegisterCreatedObjectUndo(g, "Created editor group");

			for(int i = 0; i < Selection.transforms.Length; i++)
			{
				var oldParent = Selection.transforms[i].parent;
				Undo.SetTransformParent(Selection.transforms[i], g.transform, true, "Added to group");

				if(oldParent != null && oldParent.TryGetComponent<EditorGroup>(out var oldGroup))
					oldGroup.CheckEmpty();
			}

			Undo.CollapseUndoOperations(id);
			Selection.activeGameObject = g;

			postGroupCreated();
		}

		public void DissolveGroup()
		{
			Undo.IncrementCurrentGroup();
			int id = Undo.GetCurrentGroup();

			GameObject[] children = new GameObject[transform.childCount];
			for(int i = 0; i < transform.childCount; i++)
				children[i] = transform.GetChild(i).gameObject;

			for(int i = 0; i < children.Length; i++)
				Undo.SetTransformParent(children[i].transform, null, true, "Dissolved Group");

			Selection.objects = children;

			Undo.DestroyObjectImmediate(gameObject);

			Undo.CollapseUndoOperations(id);
		}

		public void DissolveGroupRaw()
		{
			GameObject[] children = new GameObject[transform.childCount];
			for(int i = 0; i < transform.childCount; i++)
				children[i] = transform.GetChild(i).gameObject;

			for(int i = 0; i < children.Length; i++)
				children[i].transform.SetParent(null, true);

			DestroyImmediate(gameObject);
		}

		void CheckEmpty()
		{
			if(transform.childCount > 0)
				return;

			var components = gameObject.GetComponents<Component>();
			if(components.Length > 2)
				return;

			DestroyImmediate(gameObject);
		}
	}

	class DestroyGroupsOnBuild : IProcessSceneWithReport
	{
		public int callbackOrder { get; }

		public void OnProcessScene(Scene scene, BuildReport report)
		{
			if(EditorApplication.isPlayingOrWillChangePlaymode)
				return;

			foreach(var group in Object.FindObjectsByType<EditorGroup>(FindObjectsInactive.Include, FindObjectsSortMode.None))
				group.DissolveGroupRaw();
		}
	}
}
#endif
