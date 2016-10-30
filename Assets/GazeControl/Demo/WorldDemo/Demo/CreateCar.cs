using UnityEngine;
using System.Collections;

public class CreateCar : MonoBehaviour {
    public GameObject carPrefab;

	public void Createcar()
    {
        Instantiate(carPrefab);
    }
}
