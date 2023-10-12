using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameObject[] pickup;
    public static LineRenderer lineRenderer;
    public static Vector3 endPosition;
    // Start is called before the first frame update

    private void Awake()
    {
        GameController.lineRenderer = gameObject.AddComponent<LineRenderer>();
    }
    void Start()
    {
        
        pickup = PlayerController.pickupList;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        if(PlayerController.UiMode != 0)
        {
            ShowLine();
        }

    }

    public static void HighLight(int No)
    {
        pickup[No].GetComponent<Renderer>().material.color = Color.blue;
    }

    public static void HighLightGreen(int No)
    {
        pickup[No].GetComponent<Renderer>().material.color = Color.green;
    }

    public static void SetBack(int No)
    {
        pickup[No].GetComponent<Renderer>().material.color = Color.white;
    }

    private void ShowLine()
    {
        lineRenderer.SetPosition(0, this.transform.position);
        lineRenderer.SetPosition(1, endPosition);
        lineRenderer.startWidth = 0.1f;
        lineRenderer.endWidth = 0.1f;
    }

    
}
