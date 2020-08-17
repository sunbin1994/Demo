using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseGame : MonoBehaviour
{
    public GameObject GameObject;
    // Start is called before the first frame update
    void Start()
    {
        GameObject.transform.GetChild(0).gameObject.SetActive(true);
        GameObject.transform.GetChild(1).gameObject.SetActive(true);
        GameObject.transform.GetChild(2).gameObject.SetActive(false);
        GameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Time.timeScale = 0;
            GameObject.SetActive(true);
            GameObject.Find("JohnLemon (1)").GetComponent<AudioSource>().Stop();
        }
    }
}
