using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CameraAngles : MonoBehaviour {

    public Scrollbar ScrollX;
   public Scrollbar ScrollY;
    public Transform center;
    Vector3 startRot;
    Transform mainCamera;
 

    public void Start()
    {
        startRot = transform.rotation.eulerAngles;
       
        transform.rotation = Quaternion.Euler(startRot.x, startRot.y - 15, startRot.z);

     
    }

    public void ChangeX()
    {
        float angle = ScrollX.value*2f ;
        if(angle>.4f)
        center.transform.localScale = new Vector3(angle, angle, angle);
       
    }
    public void ChangeY()
    {
        float angle = ScrollY.value * 100;
      center.rotation = Quaternion.Euler(0, 0 - angle, 0);
    }
}
