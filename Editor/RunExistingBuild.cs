using UnityEditor.Build.Reporting;
using UnityEditor.Build;
using UnityEditor;
using static System.IO.Path;

namespace Iteria.EditorTooling
{
	public class Build : IPostprocessBuildWithReport
	{
		const string prefsLastBuildLocation = "Iteria_last_build_location";
		public int callbackOrder => 0;

		public void OnPostprocessBuild(BuildReport report)
		{
			EditorPrefs.SetString(prefsLastBuildLocation, report.summary.outputPath);
		}

		[MenuItem("File/Run Existing Build", priority = 211)]
		public static void RunExistingBuild()
		{
			if(!EditorPrefs.HasKey(prefsLastBuildLocation))
			{
				UnityEngine.Debug.LogError("No previous build found. Try making a new build with this package installed.");
				return;
			}

			var path = EditorPrefs.GetString(prefsLastBuildLocation);
			if(System.IO.File.Exists(path))
				System.Diagnostics.Process.Start(path);
			else
				UnityEngine.Debug.LogError($"Couldn't find {GetFileName(path)} in {GetDirectoryName(path)}. Aborting.");
		}
	}
}
