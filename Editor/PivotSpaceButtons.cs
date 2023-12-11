using UnityEngine.UIElements;
using UnityEditor;
using UnityEditor.Overlays;
using UnityEditor.Toolbars;
using System.Collections.Generic;

[Overlay(typeof(SceneView), "Pivot & Space Selection", defaultDisplay = true)]
public class PivotSpaceButtons : Overlay, ICreateToolbar
{
	IEnumerable<string> ICreateToolbar.toolbarElements => k_ToolbarItems;
	static readonly string[] k_ToolbarItems = new[]
	{
		"Toggle Tool Handle Rotation",
		"Toggle Tool Handle Position"
	};

	public override VisualElement CreatePanelContent() => new Label() { text = "Please dock me to a toolbar! I'm your pivot and space controls!" };
}

[EditorToolbarElement("Toggle Tool Handle Rotation", typeof(SceneView))]
class GlobalLocalToggle : EditorToolbarToggle
{
	const string global = " Global";
	const string local = " Local";

	public GlobalLocalToggle()
	{
		offIcon = EditorGUIUtility.FindTexture("d_ToolHandleGlobal");
		onIcon = EditorGUIUtility.FindTexture("d_ToolHandleLocal");
		this.RegisterValueChangedCallback(OnChange);

		if(Tools.pivotRotation == PivotRotation.Local)
			ToggleValue();

		text = Tools.pivotRotation == PivotRotation.Local ? local : global;
	}

	void OnChange(ChangeEvent<bool> e)
	{
		text = e.newValue ? local : global;
		Tools.pivotRotation = e.newValue ? PivotRotation.Local : PivotRotation.Global;
	}
}

[EditorToolbarElement("Toggle Tool Handle Position", typeof(SceneView))]
class PivotCenterToggle : EditorToolbarToggle
{
	const string pivot = " Pivot";
	const string center = " Center";

	public PivotCenterToggle()
	{
		offIcon = EditorGUIUtility.FindTexture("d_ToolHandleCenter");
		onIcon = EditorGUIUtility.FindTexture("d_ToolHandlePivot");
		this.RegisterValueChangedCallback(OnChange);

		if(Tools.pivotMode == PivotMode.Pivot)
			ToggleValue();

		text = Tools.pivotMode == PivotMode.Pivot ? pivot : center;
	}

	void OnChange(ChangeEvent<bool> e)
	{
		text = e.newValue ? pivot : center;
		Tools.pivotMode = e.newValue ? PivotMode.Pivot : PivotMode.Center;
	}
}
