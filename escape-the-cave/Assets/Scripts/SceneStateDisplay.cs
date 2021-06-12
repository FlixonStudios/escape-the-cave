using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneStateDisplay : MonoBehaviour
{
    [SerializeField] GameObject successText;
    [SerializeField] GameObject playAgainButton;
    void Start()
    {
        successText.SetActive(false);
        playAgainButton.SetActive(false);
    }

    public void TriggerSuccessDisplay()
    {
        successText.SetActive(true);
        playAgainButton.SetActive(true);
    }
}
