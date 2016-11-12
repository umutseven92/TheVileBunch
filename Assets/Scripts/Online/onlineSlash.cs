using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class onlineSlash : NetworkBehaviour
{

	[HideInInspector]
	public uint ShooterId { get; set; }

	public bool visible = true; // Is the slash hitbox visible

	void Start()
	{
		var _sprRenderer = GetComponent<SpriteRenderer>();
		if (visible)
		{
			_sprRenderer.enabled = true;
		}
		else
		{
			_sprRenderer.enabled = false;
		}

		Destroy(gameObject, 0.4f);
	}
}
