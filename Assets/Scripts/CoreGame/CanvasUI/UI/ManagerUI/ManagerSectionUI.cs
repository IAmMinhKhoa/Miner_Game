using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using NOOD;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Localization;
public class ManagerSectionUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _sectionText;
    [SerializeField] private ManagerGridUI _managerGridUI;
    private RectTransform _rectTransform;
	private Dictionary<string, string> englishToVietnamese;
	private Dictionary<string, string> vietnameseToEnglish;
	public static bool isEnglish = true;
	void Awake()
    {
		
		_rectTransform = this.GetComponent<RectTransform>();
		englishToVietnamese = new Dictionary<string, string>
		{
			{ "Hổ", "Tiger" },
			{ "Gấu", "Bear" },
			{ "Chó", "Dog" }
		};
		vietnameseToEnglish = new Dictionary<string, string>
		{
			{ "Tiger", "Hổ" },
			{ "Bear", "Gấu" },
			{ "Dog", "Chó" }
		};
	}

    public async UniTask SetData(string sectionName, List<Manager> managerDatas)
    {
		//Debug.LogError("SetData:"+isEnglish);
		if (isEnglish)
		{
			if (englishToVietnamese.ContainsKey(sectionName))
			{
				_sectionText.text = englishToVietnamese[sectionName];
			}
			else
			{
				_sectionText.text = sectionName; 
			}
		}
		else
		{
			if (vietnameseToEnglish.ContainsKey(sectionName))
			{
				_sectionText.text = vietnameseToEnglish[sectionName];
			}
			else
			{
				_sectionText.text = sectionName; 
			}
		}
		await _managerGridUI.ShowMangers(managerDatas);
        await UniTask.WaitForEndOfFrame(this);
        LayoutRebuilder.ForceRebuildLayoutImmediate(_rectTransform);
    }
}
