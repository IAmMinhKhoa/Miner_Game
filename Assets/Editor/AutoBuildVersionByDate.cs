using UnityEditor;
using UnityEditor.Callbacks;
public class AutoBuildVersionByDate
{
	[PostProcessBuild]
	public static void OnPostprocessBuild(BuildTarget target, string path)
	{
		string dateVersion = System.DateTime.Now.ToString("yyyy.MM.dd.HH.mm");
		PlayerSettings.bundleVersion = dateVersion;

	}
}
