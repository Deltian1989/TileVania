using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelExit : MonoBehaviour
{
    [SerializeField] float timeScale = 0.2f;
    [SerializeField] float levelLoadDelay = 2;

    void OnTriggerEnter2D(Collider2D other)
    {
        StartCoroutine(WaitUntilNextSceneLoaded());
    }

    IEnumerator WaitUntilNextSceneLoaded()
    {
        Time.timeScale = timeScale;
        yield return new WaitForSecondsRealtime(levelLoadDelay);

        var nextSceneIndex = SceneManager.GetActiveScene().buildIndex;

        SceneManager.LoadScene(++nextSceneIndex);
        Time.timeScale = 1;
    }
}
