using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunBullets : MonoBehaviour
{
    public float Speed;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.Rotate(Vector3.up * Speed);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            if (other.GetComponent<PlayerMovement>().IsArmed)
            {
                GameObject.Find("Pickup").GetComponent<AudioSource>().Play();
                other.GetComponent<PlayerMovement>().bulletNumAll += 10;
                Destroy(gameObject);
            }
            
        }
    }
}
