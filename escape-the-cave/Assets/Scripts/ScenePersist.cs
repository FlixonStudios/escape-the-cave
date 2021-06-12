using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScenePersist : MonoBehaviour
{
    int originalBuildIndex = -1;
    private void Awake()
    {
        ScenePersist[] scenePersists = FindObjectsOfType<ScenePersist>();
        int numScenePersists = scenePersists.Length;                   
        int currentBuildIndex = SceneManager.GetActiveScene().buildIndex;        

        //Singleton pattern for destroying the new GO created when reloading the scene
        if (numScenePersists > 1)
        {
            foreach (ScenePersist scenePersist in scenePersists)
            {           
                //restrict check to old scene
                if (scenePersist != this)
                {
                    // the old scene's buildindex (range 0 to 2) if == to new scene buildIndex -> destroy new scene as it is a respawn
                    if (scenePersist.originalBuildIndex == currentBuildIndex)
                    {                        
                        Destroy(gameObject);
                    }
                    // if the old scene's buildindex is > new scene (i.e. new scene is lower buildindex) destroy old scene as it is a restart
                    // if the old scene's buildindex is < new scene (i.e. new scene is higher buildindex), this is progression dont destroy also
                    else
                    {
                        Destroy(scenePersist.gameObject);
                    }
                }                
            }            
        }
        else
        {
            DontDestroyOnLoad(gameObject);
            originalBuildIndex = SceneManager.GetActiveScene().buildIndex;
        }
    }
}
