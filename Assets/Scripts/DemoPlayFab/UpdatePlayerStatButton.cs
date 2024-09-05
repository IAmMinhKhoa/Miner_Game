using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace PlayFabDemo
{
	public class UpdatePlayerStatButton : MonoBehaviour
	{


		public TMP_InputField playerName;
		public TMP_InputField heath;
		public TMP_InputField level;
		public TextMeshProUGUI playerStat;

		public PlayerStat ReturnClass()
		{
			return new PlayerStat(playerName.text, int.Parse(heath.text), int.Parse(level.text));
		}
		public void SetUI(PlayerStat player)
		{
			string tmp = $"Player Name: {player.name}\nHealth: {player.heath}\nLevel: {player.level}";
			playerStat.text = tmp;
		}
		public class PlayerStat
		{
			public string name;
			public int heath;
			public int level;
			public PlayerStat(string name, int heath, int level)
			{
				this.name = name;
				this.heath = heath;
				this.level = level;
			}
		}
	}
}
