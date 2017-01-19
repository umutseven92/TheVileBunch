using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class onlineProjectile : NetworkBehaviour {

	[HideInInspector]
	public uint ShooterId { get; set; }

	// Use this for initialization
	protected virtual void Start()
	{
		Destroy(gameObject, 3);
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		var colName = other.name;

		if (!colName.StartsWith("Online") && !colName.StartsWith("Pickup"))
		{
			Destroy(gameObject);
		}
	}
}
