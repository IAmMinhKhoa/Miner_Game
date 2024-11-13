using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
[CreateAssetMenu(fileName = "Banner Data", menuName = "ScriptableObjects/Baner")]
public class BannerSO : ScriptableObject
{
	public List<Color> dataColorText = new List<Color>();
	public List<TMP_FontAsset> dataFontText = new List<TMP_FontAsset>();
	[ReadOnly]
	public List<DesignTemplateInfo> dataSprite_1 = new();
	public List<Sprite> dataSprite_2 = new List<Sprite>();

	//readonly string folderPath = "Assets/Sprites/Banner/BannerData/Mẫu Trang Trí";
	private void OnValidate()
	{
		UpdateFileList("Assets/Sprites/Banner/BannerData/Mẫu Trang Trí", dataSprite_1);
	}
	private void UpdateFileList(string folderPath, List<DesignTemplateInfo> dataSprite)
	{
		dataSprite.Clear();

		if (Directory.Exists(folderPath))
		{
			string[] folder = Directory.GetDirectories(folderPath);
			int ID = 0;
			foreach (string file in folder)
			{
				string fileName = Path.GetFileName(file);
				
			}
		}
		else
		{
			Debug.LogWarning($"Folder path '{folderPath}' does not exist.");
		}
	}
}
public struct DesignTemplateInfo
{
	public int ID;
	public List<Sprite> sprites;

	public DesignTemplateInfo(int ID, List<Sprite> sprites)
	{
		this.ID = ID;
		this.sprites = sprites;
	}
}

