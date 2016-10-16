using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class graveyardScript : MonoBehaviour
{
	public Text TimerText;
	public Text WinnerText;
	public Text WinnerTextShadow;

	public Text One;
	public Text[] Two;
	public Text[] Three;

	public GameObject OneGrave;
	public GameObject[] TwoGrave;
	public GameObject[] ThreeGrave;

	public Canvas OneCanvas;
	public Canvas[] TwoCanvas;
	public Canvas[] ThreeCanvas;

	public int MaxTimerCount;

	protected bool done = false;
	private float _timerCounter;

	// Use this for initialization
	protected virtual void Start ()
	{
		Time.timeScale = 1;
		TimerText.text = MaxTimerCount.ToString();
		_timerCounter = MaxTimerCount;

		OneGrave.GetComponent<SpriteRenderer>().enabled = false;

		foreach (var o in TwoGrave)
		{
			o.GetComponent<SpriteRenderer>().enabled = false;
		}

		foreach (var o in ThreeGrave)
		{
			o.GetComponent<SpriteRenderer>().enabled = false;
		}

		OneCanvas.GetComponent<Canvas>().enabled = false;

		foreach (var c in TwoCanvas)
		{
			c.GetComponent<Canvas>().enabled = false;
		}

		foreach (var c in ThreeCanvas)
		{
			c.GetComponent<Canvas>().enabled = false;
		}

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
