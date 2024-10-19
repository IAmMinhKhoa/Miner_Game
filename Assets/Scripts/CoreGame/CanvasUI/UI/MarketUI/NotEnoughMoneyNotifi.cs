using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotEnoughMoneyNotifi : MonoBehaviour
{
   public void CloseUI()
	{
		gameObject.SetActive(false);
	}
}
