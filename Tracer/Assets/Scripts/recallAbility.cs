using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class recallAbility : MonoBehaviour
{
    //how long the recall ability lasts
    public float maxDuration = 3;
    //how long the current movement is saved (position and location)
    public float saveInterval = 0.5f;
    //how fast is the recall speed
    public float recallSpeed = 18;
    //this will turn the canvas UI like a camera effect
    public CanvasGroup cameraFX;


    public List<Vector3> positions;

    private bool recalling;
    private float savePositionTimer;
    private float maxPositionStored;
  
    void Start()
    {
        maxPositionStored = maxDuration / saveInterval;
    }

    // Update is called once per frame
    void Update()
    {
        if (!recalling)
        {
            if(Input.GetKeyDown(KeyCode.Z) && positions.Count > 0)
            {
                recalling = true;
            }

            if (savePositionTimer > 0)
            {
                savePositionTimer -= Time.deltaTime;
            }
            else
            {
                SavePosition();
            }

            cameraFX.alpha = Mathf.Lerp(cameraFX.alpha, 0, recallSpeed * Time.deltaTime);
        }
        else
        {
            if(positions.Count > 0)
            {
                transform.position = Vector3.Lerp(transform.position, positions[0], recallSpeed * Time.deltaTime);

                float distance = Vector3.Distance(transform.position, positions[0]);
                if (distance < 0.25f)
                {
                    SetPostion();
                }
            }
            else
            {
                recalling = false;
            }

            cameraFX.alpha = Mathf.Lerp(cameraFX.alpha, 0, recallSpeed * Time.deltaTime);
        }
    }


    void SavePosition()
    {
        savePositionTimer = saveInterval;

        positions.Insert(0, transform.position);

        if(positions.Count > maxPositionStored)
        {
            positions.RemoveAt(positions.Count - 1);
        }
    }

    void SetPostion()
    {
        transform.position = positions[0];

        positions.RemoveAt(0);

    }
}
