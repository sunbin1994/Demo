using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    AudioSource audioSource;
    //float timer = 0;
    //bool continue_game = false;
    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }
    public void PlayGame()
    {
        audioSource.Play();
        SceneManager.LoadScene(1);
    }
    public void QuitGame()
    {
        audioSource.Play();
        Application.Quit();
    }
    public void ContinueGame()
    {
        audioSource.Play();
        Time.timeScale = 1;
        GameObject.Find("Pause").SetActive(false);

    }
    public void HelpGame()
    {
        audioSource.Play();
        gameObject.transform.GetChild(1).gameObject.SetActive(false);
        gameObject.transform.GetChild(2).gameObject.SetActive(true);
    }
    public void ReturnMenu()
    {
        audioSource.Play();
        gameObject.transform.GetChild(2).gameObject.SetActive(false);
        gameObject.transform.GetChild(1).gameObject.SetActive(true);
    }
    
}
