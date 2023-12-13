using UnityEngine;
using UnityEditor;

namespace Iteria.EditorTooling
{
	[CustomEditor(typeof(EditorGroup))]
	public class EditorGroupInspector : Editor
	{
		public override void OnInspectorGUI()
		{
			var editorGroup = target as EditorGroup;

			if(editorGroup.transform.childCount < 1)
				EditorGUILayout.LabelField("Group has no children.");

			var components = editorGroup.gameObject.GetComponents<Component>();
			if(components.Length > 2)
				EditorGUILayout.LabelField("Dissolving this group will destroy all components!");

			if(GUILayout.Button("Dissolve Group"))
				editorGroup.DissolveGroup();
		}
	}
}
