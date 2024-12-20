// Tayde M. Cruz (tayde@algostudio.mx)
// ALGO STUDIO
// 11-08-2023

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallButton : KeyboardEvents
{
    public char key;

    public void KeyDown()
    {
        base.KeyDown(key);
        //Debug.Log(key);
    }
}
