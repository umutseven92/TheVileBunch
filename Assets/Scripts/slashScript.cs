using UnityEngine;
using System.Collections;

public class slashScript : MonoBehaviour
{
    public bool slashing;
    public int num;

    private BoxCollider2D _bCol;
    private SpriteRenderer sprRenderer;

    // Use this for initialization
    void Start()
    {
        sprRenderer = GetComponent<SpriteRenderer>();
        sprRenderer.enabled = false;

        _bCol = GetComponent<BoxCollider2D>();
        _bCol.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (slashing)
        {
            _bCol.enabled = true;
            sprRenderer.enabled = true;
        }
        else
        {
            _bCol.enabled = false;
            sprRenderer.enabled = false;
        }
    }

    void GetPlayerNum(int playerNum)
    {
        num = playerNum;
    }

    void GetCol(bool col)
    {
        slashing = col;
    }

}
