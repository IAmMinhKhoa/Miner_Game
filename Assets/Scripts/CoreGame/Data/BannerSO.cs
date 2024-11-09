using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
[CreateAssetMenu(fileName = "Banner Data", menuName = "ScriptableObjects/Baner")]
public class BannerSO : ScriptableObject
{
	public List<Color> dataColorText = new List<Color>();
	public List<TMP_FontAsset> dataFontText = new List<TMP_FontAsset>();
	public List<Sprite> dataSprite_1 = new List<Sprite>();
	public List<Sprite> dataSprite_2 = new List<Sprite>();
}
