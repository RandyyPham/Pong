using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTwoPaddle : Paddle
{
    private Vector2 direction;
    private Color playerColor;

    private void Start()
    {
        if (ColorUtility.TryParseHtmlString(PlayerPrefs.GetString("Computer Color"), out playerColor))
        {
            _spriteRenderer.color = playerColor;
        }
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.UpArrow))
        {
            direction = Vector2.up;
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            direction = Vector2.down;
        }
        else
        {
            direction = Vector2.zero;
        }
    }

    private void FixedUpdate()
    {
        // checks if we are moving
        if (direction.sqrMagnitude != 0)
        {
            _rigidbody2D.AddForce(direction * this._speed);
        }
    }
}
