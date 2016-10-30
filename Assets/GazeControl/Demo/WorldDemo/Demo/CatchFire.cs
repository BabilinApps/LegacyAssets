using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using GazeInput;

[RequireComponent(typeof(BoxCollider))]
[RequireComponent(typeof(VRInteractiveItem))]

public class CatchFire : MonoBehaviour, IPointerClickHandler
{
    public GameObject fire;
    public GameObject destroy;
    public void OnPointerClick(PointerEventData eventData)
    {

    Instantiate(fire, transform.position, transform.rotation);
     
        Invoke("Destroy", 5);
    }
    void Destroy()
    {
        Instantiate(destroy, transform.position, transform.rotation);
        Destroy(gameObject);
    }
}
