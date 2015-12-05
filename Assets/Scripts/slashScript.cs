using UnityEngine;
using System.Collections;

public class slashScript : MonoBehaviour
{
    [HideInInspector]
    public bool slashing;

    [HideInInspector]
    public int num;

    public bool visible = true; // Is the slash hitbox visible

    private BoxCollider2D _bCol;
    private SpriteRenderer _sprRenderer;

    // Use this for initialization
    void Start()
    {
        _sprRenderer = GetComponent<SpriteRenderer>();
        _sprRenderer.enabled = false;

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
                _sprRenderer.enabled = true;
            }
            else
            {
                _sprRenderer.enabled = false;
            }
        }
        else
        {
            _bCol.enabled = false;
            _sprRenderer.enabled = false;
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
