using UnityEngine;
using System.Collections;

public class slashScript : MonoBehaviour
{
    [HideInInspector]
    public bool slashing;

    [HideInInspector]
    public int num;

    private BoxCollider2D _bCol;
    private SpriteRenderer sprRenderer;
    public bool visible = true;

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
            if (visible)
            {
                sprRenderer.enabled = true;
            }
            else
            {
                sprRenderer.enabled = false;
            }
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
