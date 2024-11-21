using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TMPro;
using UnityEditor;
using UnityEngine;
[CreateAssetMenu(fileName = "Banner Data", menuName = "ScriptableObjects/Baner")]
public class BannerSO : ScriptableObject
{
	public List<Color> dataColorText = new List<Color>();
	public List<TMP_FontAsset> dataFontText = new List<TMP_FontAsset>();
	public List<DesignTemplateInfo> dataSprite_1 = new();
	public List<DesignTemplateInfo>  dataSprite_2 = new();

	private void OnValidate()
	{
		LoadSpirteData("Sprites/BannerData/MauTrangTri", dataSprite_1);
		LoadSpirteData("Sprites/BannerData/PhongNen", dataSprite_2);
	}
	void LoadSpirteData(string path, List<DesignTemplateInfo> list)
	{
		List<DesignTemplateInfo> tmpList = list.ToList();
		list.Clear();
		List<string> folderPaths = GetSubFolders(path);
		int ID = 0;
		foreach (var folderPath in folderPaths)
		{
			List<Sprite> sprites = GetAllSpritesInFolder(folderPath);
			List<TemplateDetail> templateDetails = new();
			for(int i = 0; i < sprites.Count; i++) 
			{
				var template = sprites[i];
			//	Color color = tmpList.Count > 0 ? tmpList[ID].templateDetails[i].color : Color.white;
				templateDetails.Add(new TemplateDetail(Color.white, template));
			}
			list.Add(new DesignTemplateInfo(ID++, templateDetails));
		}
	}
	public static List<string> GetSubFolders(string parentFolderPath)
	{
		List<string> folderPaths = new List<string>();

		string fullPath = Path.Combine("Assets/Resources", parentFolderPath);  

		if (Directory.Exists(fullPath))
		{
			
			string[] folders = Directory.GetDirectories(fullPath);

			foreach (string folder in folders)
			{
				
				folderPaths.Add(folder.Replace("\\", "/").Replace("Assets/Resources/", ""));
			}
		}
		else
		{
			Debug.LogError($"Thư mục {parentFolderPath} không tồn tại!");
		}

		return folderPaths;
	}
	
	public static List<Sprite> GetAllSpritesInFolder(string folderPath)
	{
		List<Sprite> sprites = new List<Sprite>();
		Sprite[] loadedSprites = Resources.LoadAll<Sprite>(folderPath);
		//Debug.Log(folderPath);
		foreach (Sprite sprite in loadedSprites)
		{
			if (sprite != null)
			{
				sprites.Add(sprite);
			}
		}

		return sprites;
	}

}
[System.Serializable]
public struct DesignTemplateInfo
{
	
	public int ID;
	public List<TemplateDetail> templateDetails;

	public DesignTemplateInfo(int ID, List<TemplateDetail> templateDetails)
	{
		this.ID = ID;
		this.templateDetails = templateDetails;
	}
}
[Serializable]
public struct TemplateDetail
{
	public Color color;
	
	public Sprite sprite;

	public TemplateDetail(Color color, Sprite sprite)
	{
		this.color = color;
		this.sprite = sprite;
	}

}
public class SpriteLoader : MonoBehaviour
{
	public static Sprite LoadSpriteFromPath(string assetPath)
	{
		return Resources.Load<Sprite>(assetPath);
	}
}

