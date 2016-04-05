using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class graveyardScript : Photon.PunBehaviour 
{
	public Text TimerText;
	public Text WinnerText;
	public GameObject Graveyard;

	public GameObject One;
	public GameObject[] Two;
	public GameObject[] Three;

	public int MaxTimerCount;

	protected bool done = false;
	private float _timerCounter;

	// Use this for initialization
	protected virtual void Start ()
	{
		Time.timeScale = 1;
		TimerText.text = MaxTimerCount.ToString();
		_timerCounter = MaxTimerCount;

	}
	
	// Update is called once per frame
	protected virtual void Update () {
		CheckTimer();
		TimerText.text = ((int)_timerCounter).ToString();
	}

	private void CheckTimer()
	{
		if (!done)
		{
			_timerCounter -= 1 * UnityEngine.Time.deltaTime;
			if (_timerCounter <= 5)
			{
				TimerText.color = Color.red;
			}

			if (_timerCounter <= 0)
			{
				done = true;
			}
		}
	}
}
