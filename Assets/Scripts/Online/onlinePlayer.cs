using System.Collections.Generic;
using System.Linq;
using log4net;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class onlinePlayer : playerControl
{
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

	protected override void Awake()
	{
		base.Awake();

		if (string.IsNullOrEmpty(Control) && Debug.isDebugBuild)
		{
			Control = "j1";
			this.Ammo = this.MaxAmmo = 99;
		}
	}


	[Command]
	protected override void CmdShoot()
	{
		base.CmdShoot();

	}
}
