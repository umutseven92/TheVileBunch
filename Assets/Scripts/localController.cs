using UnityEngine;
using System.Collections;

public class localController : MonoBehaviour
{
    public Transform player;

    // Use this for initialization
    void Start()
    {
        CreatePlayers(loadOnClick.playerCount);
    }

    void CreatePlayers(int playerCount)
    {
        switch (playerCount)
        {
            case 0:
                // For debug purposes only!
                Instantiate(player, new Vector3(-4.65f, 3.22f, 0), Quaternion.identity);
                Instantiate(player, new Vector3(4.15f, 3.45f, 0), Quaternion.identity);
                break;
            case 2:
                Instantiate(player, new Vector3(-4.65f, 3.22f, 0), Quaternion.identity);
                Instantiate(player, new Vector3(4.15f, 3.45f, 0), Quaternion.identity);

                break;
            case 3:
                Instantiate(player, new Vector3(-4.65f, 3.22f, 0), Quaternion.identity);
                Instantiate(player, new Vector3(4.15f, 3.45f, 0), Quaternion.identity);
                Instantiate(player, new Vector3(-6.36f, -2.07f, 0), Quaternion.identity);
                break;
            case 4:
                Instantiate(player, new Vector3(-4.65f, 3.22f, 0), Quaternion.identity);
                Instantiate(player, new Vector3(4.15f, 3.45f, 0), Quaternion.identity);
                Instantiate(player, new Vector3(-6.36f, -2.07f, 0), Quaternion.identity);
                Instantiate(player, new Vector3(6.00f, -2.07f, 0), Quaternion.identity);
                break;
            default:
                Debug.LogError("Player count not between 1 and 4!");
                break;
        }

    }
}
