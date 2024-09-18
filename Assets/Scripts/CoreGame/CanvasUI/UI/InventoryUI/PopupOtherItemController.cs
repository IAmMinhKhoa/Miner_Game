using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UI.Inventory
{
    public class PopupOtherItemController : MonoBehaviour
    {
		
        public void CloseUI()
		{
			gameObject.SetActive(false);
		}
    }
}
