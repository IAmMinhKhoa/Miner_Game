using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEditor;
using UnityEngine;
[CreateAssetMenu(fileName = "Banner Data", menuName = "ScriptableObjects/Baner")]
public class BannerSO : ScriptableObject
{
	public List<Color> dataColorText = new List<Color>();
	public List<TMP_FontAsset> dataFontText = new List<TMP_FontAsset>();
	public List<DesignTemplateInfo> dataSprite_1 = new();
	public List<Sprite> dataSprite_2 = new List<Sprite>();

	[SerializeField] Sprite _sprite;
	//readonly string folderPath = "Assets/Sprites/Banner/BannerData/Mẫu Trang Trí";
	private void OnValidate()
	{
		UpdateFileList("Assets/Sprites/Banner/BannerData/Mẫu Trang Trí", dataSprite_1);
	}
	private void UpdateFileList(string folderPath, List<DesignTemplateInfo> dataSprite)
	{
		dataSprite.Clear();
	
		//SpriteLoader.LoadSpriteFromPath("Assets/Sprites/Banner/BannerData/Mẫu Trang Trí/Vintage Châu Âu/Đỏ - Tím.png");
		if (Directory.Exists(folderPath))
		{
			string[] folders = Directory.GetDirectories(folderPath);
			int ID = 0;

			foreach (string folder in folders)
			{
				string folderName = Path.GetFileName(folder);
				List<string> sprites = new List<string>();
				
				foreach (string file in Directory.GetFiles(folder))
				{
					
					if (Path.GetExtension(file) == ".png" || Path.GetExtension(file) == ".jpg")
					{
						string assetPath = file.Replace("\\", "/"); 
						assetPath = assetPath.Replace(Application.dataPath, "Assets");

						Debug.Log(assetPath);
						sprites.Add(assetPath);
					}
				}
				
				if (sprites.Count > 0)
				{
					dataSprite.Add(new DesignTemplateInfo(ID++, sprites));
				}
			}
		}
		else
		{
			Debug.LogWarning($"Folder path '{folderPath}' does not exist.");
		}
	}
}
[System.Serializable]
public struct DesignTemplateInfo
{
	[ReadOnly]
	public int ID;
	[ReadOnly]
	public List<string> sprites;

	public DesignTemplateInfo(int ID, List<string> sprites)
	{
		this.ID = ID;
		this.sprites = sprites;
	}
}
public class SpriteLoader : MonoBehaviour
{
	public static Sprite LoadSpriteFromPath(string assetPath)
	{
		return AssetDatabase.LoadAssetAtPath<Sprite>(assetPath);
	}
}

