using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour
{
    public bool ifActice;
    private GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        ifActice = false;
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(new Vector3(15, 30, 45) * Time.deltaTime);
        if (ifActice)
        {
            transform.LookAt(player.transform);
        }
    }
}
