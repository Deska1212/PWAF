using System.Collections;
using System.Collections.Generic;
using Core;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PersistantsScene : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(gameObject);
        SceneLoader.Instance.LoadScene(SCENE.MENU);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
