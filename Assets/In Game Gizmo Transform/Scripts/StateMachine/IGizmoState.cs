using UnityEngine;
using System.Collections;

public interface IGizmoState

{
    void Awake(GameObject gameObject);
    void OnRenderObject();

}
