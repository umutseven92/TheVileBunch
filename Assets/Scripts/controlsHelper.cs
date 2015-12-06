using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class controlsHelper : MonoBehaviour
{
	public InputField NameInput;
	public Button NameButton;

	// Update is called once per frame
	void Update () {
		if (NameInput.isFocused && Input.GetButtonDown("Submit"))
		{
			EventSystem.current.SetSelectedGameObject(NameButton.gameObject);
		}
	}
}
