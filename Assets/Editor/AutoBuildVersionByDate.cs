using System;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEditor.Callbacks;
public class AutoBuildVersionByDate : IPreprocessBuildWithReport
{
	public int callbackOrder => 0;
	private const string initVersion = "0.0";
	public void OnPreprocessBuild(BuildReport report)
	{
		string currentVersion = FindCurrentVersion();
		UpdateVersion(currentVersion);
	}
	private string FindCurrentVersion()
	{
		string[] currentVersion = PlayerSettings.bundleVersion.Split('[', ']');
		return currentVersion.Length == 1 ? initVersion : currentVersion[1];
	}
	private void UpdateVersion(string version)
	{
		if(float.TryParse(version, out float currentVersion))
		{
			float newVersion = currentVersion + 0.01f;
			string date = DateTime.Now.ToString("d");

			PlayerSettings.bundleVersion =string.Format("version [{0}] - {1}",newVersion,date);

		}
	}
}
