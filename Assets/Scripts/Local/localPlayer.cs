using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Networking;

public class localPlayer : playerControl
{
	private List<playerSelect.Player> _localPlayers;

	public override int Health { get; set; }

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

	protected override void LowerHealth(int damage)
	{
		base.LowerHealth(damage);
		base.UpdateHealthSlider(Health);
	}

	protected override void GiveHealth(int health)
	{
		base.GiveHealth(health);
		base.UpdateHealthSlider(Health);
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
