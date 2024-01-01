using UnityEngine.UIElements;
using UnityEditor;
using UnityEditor.Overlays;
using UnityEditor.Toolbars;
using System.Collections.Generic;

namespace Iteria.EditorTooling
{
	[Overlay(typeof(SceneView), "Pivot & Space Selection", defaultDisplay = true, defaultDockPosition = DockPosition.Top, defaultDockZone = DockZone.TopToolbar, defaultLayout = Layout.HorizontalToolbar)]
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
		PivotRotation expectedRotation = PivotRotation.Global;

		public GlobalLocalToggle()
		{
			offIcon = EditorGUIUtility.FindTexture("d_ToolHandleGlobal");
			onIcon = EditorGUIUtility.FindTexture("d_ToolHandleLocal");
			this.RegisterValueChangedCallback(OnChange);
			EditorApplication.update += CheckChange;

			text = Tools.pivotRotation == PivotRotation.Local ? local : global;
		}

		void OnChange(ChangeEvent<bool> e)
		{
			text = e.newValue ? local : global;
			Tools.pivotRotation = e.newValue ? PivotRotation.Local : PivotRotation.Global;
			expectedRotation = Tools.pivotRotation;
		}

		void CheckChange()
		{
			if(Tools.pivotRotation != expectedRotation)
				ToggleValue();
		}
	}

	[EditorToolbarElement("Toggle Tool Handle Position", typeof(SceneView))]
	class PivotCenterToggle : EditorToolbarToggle
	{
		const string pivot = " Pivot";
		const string center = " Center";
		PivotMode expectedMode = PivotMode.Center;

		public PivotCenterToggle()
		{
			offIcon = EditorGUIUtility.FindTexture("d_ToolHandleCenter");
			onIcon = EditorGUIUtility.FindTexture("d_ToolHandlePivot");
			this.RegisterValueChangedCallback(OnChange);
			EditorApplication.update += CheckChange;

			text = Tools.pivotMode == PivotMode.Pivot ? pivot : center;
		}

		void OnChange(ChangeEvent<bool> e)
		{
			text = e.newValue ? pivot : center;
			Tools.pivotMode = e.newValue ? PivotMode.Pivot : PivotMode.Center;
			expectedMode = Tools.pivotMode;
		}

		void CheckChange()
		{
			if(Tools.pivotMode != expectedMode)
				ToggleValue();
		}
	}
}
