using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class menuManager : MonoBehaviour
{
    public int sceneId;
    bool waitForReload;

    public Animator bottomStart;

    void Start()
    {
        sceneId = 1;
    }

    void Update()
    {

    }

    public void loadMission()
    {
        if (!waitForReload)
        {
            bottomStart.SetTrigger("startClick");
            AsyncOperation operation = SceneManager.LoadSceneAsync(sceneId);
            if (!operation.isDone) { waitForReload = true; }            
        }
    }
}
