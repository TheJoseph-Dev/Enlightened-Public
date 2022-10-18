using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideCursor : MonoBehaviour
{

    public bool activate = false;

    void Update()
    {
	    Cursor.visible = activate;
    }
}
