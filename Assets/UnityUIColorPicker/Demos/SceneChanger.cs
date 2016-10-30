using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
public class SceneChanger : MonoBehaviour {

   public void LoadNewLevel(string name) {
        SceneManager.LoadSceneAsync(name);
    }
}
