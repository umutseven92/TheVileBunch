using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Networking;

public class localPlayer : playerControl
{
	private List<playerSelect.Player> _localPlayers;

	protected override void Awake()
	{
		base.Awake();

		_localPlayers = playerSelect.PlayerList;
	}

	protected override void FixedUpdate()
	{
		if (_paused)
		{
			return;
		}

		base.FixedUpdate();
	}


	void Pause()
	{
		_paused = true;
	}

	void UnPause()
	{
		_paused = false;
	}
	void PlayerNumber(int num)
	{
		playerNum = num;
		Control = _localPlayers[num - 1].Control;

		_playerClass = _localPlayers[num - 1].Class;

		_slashCol.SendMessage("GetPlayerNum", playerNum);
	}

}
