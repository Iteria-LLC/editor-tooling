using UnityEditor.Build.Reporting;
using UnityEditor.Build;
using UnityEditor;
using UnityEngine;

namespace Iteria.EditorTooling
{
	public class Build : IPostprocessBuildWithReport
	{
		const string lastBuildLocation = "Iteria_last_build_location";
		static string prefsLastBuildKey => $"{lastBuildLocation}_{Application.productName}";
		public int callbackOrder => 0;

		public void OnPostprocessBuild(BuildReport report)
		{
            EditorPrefs.SetString(prefsLastBuildKey, report.summary.outputPath);
		}

		[MenuItem("File/Run Existing Build", priority = 211)]
		public static void RunExistingBuild()
		{
			if(!EditorPrefs.HasKey(prefsLastBuildKey))
			{
                Debug.LogError("No previous build found. Try making a new build with this package installed.");
				return;
			}

			var path = EditorPrefs.GetString(prefsLastBuildKey);
			if(System.IO.File.Exists(path))
				System.Diagnostics.Process.Start(path);
			else
				Debug.LogError($"Couldn't find {System.IO.Path.GetFileName(path)} in {System.IO.Path.GetDirectoryName(path)}. Aborting.");
		}
	}
}
