using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelScript : MonoBehaviour
{

    [SerializeField] int nextSceneIndex = -1;
    [SerializeField] float levelLoadDelay = 1f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        StartCoroutine(LoadNextSceneWithDelay());
    }
    
    IEnumerator LoadNextSceneWithDelay()
    {
        yield return new WaitForSecondsRealtime(levelLoadDelay);
        
        if (nextSceneIndex < 0)
        {
            LoadSuccess();
        }
        else
        {
            SceneManager.LoadScene(nextSceneIndex);
        }    
    }
    public void LoadSuccess()
    {
        if (FindObjectOfType<SceneStateDisplay>())
        {
            FindObjectOfType<SceneStateDisplay>().TriggerSuccessDisplay();
        }

    }
}
