using DG.Tweening;
using NOOD.Sound;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FruitCombiner : MonoBehaviour
{
    public GameObject combineFX;
    public ListFruit FruitList;
	public TMP_Text combo_lb;
	
	private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<FruitInfo>() != null)
        {
            int otherIndex = collision.gameObject.GetComponent<FruitInfo>().index;
            if (GetComponent<FruitInfo>().index == otherIndex && otherIndex != FruitList.list.Count)
            {
                if(gameObject.GetInstanceID() < collision.gameObject.GetInstanceID())
                {
                    Destroy(gameObject);
                    Destroy(collision.gameObject);


					SoundManager.PlaySound(SoundEnum.popMerge);

					Vector3 hitpoint = (transform.position + collision.gameObject.transform.position) / 2;
					GameObject newFruit = Instantiate(FruitList.list[otherIndex], hitpoint, Quaternion.identity, gameObject.transform.parent);
					newFruit.transform.DOScale(0.001f, 0f);
					newFruit.transform.DOScale(0.0624f, 0.5f);
					Color newColor = GetComponent<FruitInfo>().mergeColor; // Lấy màu từ mergeColor
					if (newColor != null) // Kiểm tra màu sắc có hợp lệ không
					{
						GameObject FX = Instantiate(combineFX, hitpoint, Quaternion.identity);
						FX.GetComponent<ParticleSystem>().startColor = newColor;
						ParticleSystem[] particleSystems = FX.GetComponentsInChildren<ParticleSystem>();
						foreach (var ps in particleSystems)
						{
							var main = ps.main;
							main.startColor = newColor; // Áp dụng màu cho tất cả các particle system
						}
						Destroy(FX, 3f); // Hủy đối tượng FX sau 3 giây
					}
					ComboMergeManager.Instance.comboCount++;
					ComboMergeManager.Instance.comboTime = ComboMergeManager.Instance.maxComboTime;
					//
					TMP_Text newFruitComboLb = newFruit.GetComponent<FruitCombiner>().combo_lb;
					ComboUI.Instance.transform.position = newFruitComboLb.transform.position;
					if (ComboMergeManager.Instance.comboCount > 0 && ComboMergeManager.Instance.comboCount <= 5)
					{
						ComboUI.Instance.combo_lb.text = "<color=#0092ff>" + "Combo x" + ComboMergeManager.Instance.comboCount.ToString() + "</color>";
					}
					else if (ComboMergeManager.Instance.comboCount > 5 && ComboMergeManager.Instance.comboCount <= 10)
					{
						ComboUI.Instance.combo_lb.text = "<color=#00ff3c>" + "Combo x" + ComboMergeManager.Instance.comboCount.ToString() + "</color>";
					}
					else if (ComboMergeManager.Instance.comboCount > 10 && ComboMergeManager.Instance.comboCount <= 15)
					{
						ComboUI.Instance.combo_lb.text = "<color=#ff003d>" + "Combo x" + ComboMergeManager.Instance.comboCount.ToString() + "</color>";
					}
					else if (ComboMergeManager.Instance.comboCount > 15 && ComboMergeManager.Instance.comboCount <= 20)
					{
						ComboUI.Instance.combo_lb.text = "<color=#bd4dff>" + "Combo x" + ComboMergeManager.Instance.comboCount.ToString() + "</color>";
					}
					else
					{
						ComboUI.Instance.combo_lb.text = "<color=#ff9700>" + "Combo x" + ComboMergeManager.Instance.comboCount.ToString() + "</color>";
					}	
					ComboUI.Instance.ShowComboUI();
					ComboUI.Instance.Hide(ComboUI.Instance.gameObject, 1f);

				}
				GameObject.FindWithTag("manager").GetComponent<FruitGameManager>().UpdateScore(otherIndex);
			}
        }


    }
	
}
