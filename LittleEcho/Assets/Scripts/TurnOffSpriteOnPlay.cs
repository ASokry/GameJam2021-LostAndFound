using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnOffSpriteOnPlay : MonoBehaviour
{
    private void Start()
    {
        var spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer) spriteRenderer.enabled = false;
    }
}
