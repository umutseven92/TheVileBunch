using System.Collections.Generic;
using System.Linq;
using log4net;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class onlinePlayer : playerControl
{
	[SyncVar(hook = "OnChangeHealth")]
	private int _health;

	protected override void Update()
	{
		if(!isLocalPlayer)
		{
			return;
		}	

		base.Update();
	}

	protected override void FixedUpdate()
	{
		if (_paused || !isLocalPlayer)
		{
			return;
		}

		base.FixedUpdate();
	}


	public override int Health
	{
		get { return _health; }
		set { _health = value; }
	}

	protected override void Awake()
	{
		base.Awake();

		if (string.IsNullOrEmpty(Control) && Debug.isDebugBuild)
		{
			Control = "j1";
			this.Ammo = this.MaxAmmo = 99;
		}
	}

	protected override void LowerHealth(int damage)
	{
		if (isServer)
		{
			return;
		}
		base.LowerHealth(damage);
	}

	protected override void GiveHealth(int health)
	{
		if (isServer)
		{
			return;
		}
		base.GiveHealth(health);
	}

	private void OnChangeHealth(int health)
	{
		base.UpdateHealthSlider(health);
	}


	[Command]
	protected override void CmdShoot()
	{
		base.CmdShoot();

	}
}
