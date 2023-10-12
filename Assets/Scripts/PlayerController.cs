using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour
{
    public Vector2 moveValue;
    public float speed;
    private int count;
    private int numPickups = 5; // Put here the number of pickups you have .
    public TextMeshProUGUI winText;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI PlayerText;
    public TextMeshProUGUI DistanceText;
    public static GameObject[] pickupList;
    public static float minDistance;
    public int lastMostClose;
    public static int UiMode;
    private Vector3 lastLocation;
    private Vector3 speedV3;

    private float mode3MinDistance;
    void OnMove(InputValue value)
    {
        moveValue = value.Get<Vector2>();
    }

    void FixedUpdate()
    {
        Vector3 movement = new Vector3(moveValue.x, 0.0f, moveValue.y);
        GetComponent<Rigidbody>().AddForce(movement * speed * Time.fixedDeltaTime);
        if (UiMode == 1)
        {
            FindMinDistance();
            PlayerText.text = "Player Position: X: " + this.transform.position.x.ToString("0.00") + ", Z: " + this.transform.position.z.ToString("0.00") + "\n"
            + "Player Velocity: " + ((Vector3.Distance(this.transform.position, lastLocation)) / Time.fixedDeltaTime).ToString("0.00");
        }
        else if (UiMode == 2)
        {
            Vector3 nowPos = this.transform.position;
            speedV3 = (nowPos - lastLocation) / Time.fixedDeltaTime;
            GameController.endPosition = nowPos + speedV3;          
            FindMostLikely();
        }
        storeLastLocation();
    }
    // Start is called before the first frame update
    void Start()
    {
        count = 0;
        winText.text = "";
        SetCountText();
        FindPickUps(); 
        minDistance = 0;
        mode3MinDistance = 0;
        lastLocation = transform.position;
        lastMostClose = -1;
        UiMode = 0;
        GameController.lineRenderer.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (UiMode == 0)
            {
                GameController.lineRenderer.enabled = true;
                UiMode = 1;
            }
            else if(UiMode == 1)
            {
                GameController.SetBack(lastMostClose);
                PlayerText.text = "";
                DistanceText.text = "";
                lastMostClose = -1;
                minDistance = 0;
                UiMode = 2;
            }
            else if(UiMode ==2)
            {
                GameController.lineRenderer.enabled = false;
                GameController.SetBack(lastMostClose);
                UiMode = 0;
            }
        }
    }

   
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "PickUp") {
            other.gameObject.SetActive(false);
            count += 1;
            SetCountText();
            FindPickUps();
        }
    }

    private void SetCountText()
    {
        scoreText.text = " Score: " + count.ToString();
        if (count >= numPickups)
        {
            winText.text = " You win!";
        }
    }

    private void FindPickUps()
    {
        pickupList = GameObject.FindGameObjectsWithTag("PickUp");
        lastMostClose = -1;
        GameController.pickup = pickupList;
    }

    private void FindMinDistance()
    {
        for (int i = 0; i < pickupList.Length; i++)
        {
            float nowDistance = Vector3.Distance(this.transform.position, pickupList[i].transform.position);
            if (minDistance == 0 || nowDistance < minDistance)
            {
                minDistance = nowDistance;
                if (UiMode == 1)
                {
                    GameController.endPosition = pickupList[i].transform.position;
                    if (lastMostClose != -1)
                    {
                        GameController.SetBack(lastMostClose);                     
                    }
                    GameController.HighLight(i);
                    lastMostClose = i;
                }
            }
        }
        ShowDistance();
    }

    private void ShowDistance(){
        DistanceText.text = "Distance to the closest pickup: " + minDistance.ToString("0.00");
        minDistance = 0;
    }

    private void storeLastLocation()
    {
        lastLocation = this.transform.position;
    }

    private void FindMostLikely()
    {
        for (int i = 0; i < pickupList.Length; i++)
        {
            Vector3 playerToPickUp = pickupList[i].transform.position - transform.position;
            float cos = Vector3.Dot(playerToPickUp.normalized, speedV3.normalized);
            if (cos > 0){
                float nowDistance = playerToPickUp.magnitude * Mathf.Sqrt(1 - cos * cos);
                if(nowDistance < minDistance || minDistance == 0)
                {
                    minDistance = nowDistance;
                    Debug.Log(minDistance);
                    if (lastMostClose != -1)
                    {
                        GameController.SetBack(lastMostClose);
                    }
                    GameController.HighLightGreen(i);
                    lastMostClose = i;
                }              
            }
        }
        minDistance = 0;
    }
}

