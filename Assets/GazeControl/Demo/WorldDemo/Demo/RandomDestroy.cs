using UnityEngine;
using System.Collections;

public class RandomDestroy : MonoBehaviour {

    public static bool canDestroy;

    public void Change(bool on)
    {
        canDestroy = on;
    }
}
