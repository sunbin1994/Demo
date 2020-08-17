using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReloadCursor : MonoBehaviour
{
    public Texture2D[] texture2Ds = new Texture2D[30];
    public PlayerMovement playerMovement;
    float timer = 0;
    int index = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (playerMovement.IsReloading)
        {
            timer += Time.deltaTime;
            if (timer >= 1 / 33f)
            {
                if (index >= 29)
                {
                    index = 0;
                }

                timer = 0;
                Cursor.SetCursor(texture2Ds[index++], Vector2.zero, CursorMode.Auto);

            }
        }
    }
}
