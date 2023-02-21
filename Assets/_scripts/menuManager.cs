using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class menuManager : MonoBehaviour
{
    public int sceneId;

    void Start()
    {
        sceneId = 1;
    }

    void Update()
    {
        
    }

    public void loadMission()
    {
        SceneManager.LoadScene(sceneId);
    }
}
