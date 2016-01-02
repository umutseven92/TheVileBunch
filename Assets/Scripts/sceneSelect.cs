using UnityEngine;
using UnityEngine.UI;

public class sceneSelect : Photon.PunBehaviour
{
    public Button PlayButton;
    public Button BackButton;

    // Use this for initialization
    protected virtual void Start()
    {
        PlayButton.Select();
    }

    // Update is called once per frame
    protected virtual void Update()
    {
    }
}
