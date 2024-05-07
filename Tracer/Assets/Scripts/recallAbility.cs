using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class RecallAbility : MonoBehaviour
{
    //Storing how many positions that are saved
    [SerializeField] private int maxRecallData = 6;
    //how long you wait to receive new data position
    [SerializeField] private float secondsBetweenPosition = 0.5f;
    //how long it takes to get back to the old position
    [SerializeField] private float recallDuration = 1.25f;

    private CharacterController characterController;
    //collect recall positions while not recalling
    private bool collectRecalPosition = true;
    //how long during the current position
    private float currentPositionTimer = 0.5f;


    //So you can see it while testing it(Debugging)
    [System.Serializable]
    private class RecallData
    {
        public Vector3 tracerPosition;
        public Quaternion tracerRotation;
        public Quaternion cameraRotation;
    }


    //This will store where you were 1, 2, 3 seconds ago
    [SerializeField] private List<RecallData> recallData = new List<RecallData>();

    private void Start()
    {
        characterController = GetComponentInChildren<CharacterController>();
    }

    private void Update()
    {
        StoreRecallData();

        //will draw a line behind tracer so you can physically see the position and location
        for (int i = 0; i < recallData.Count - 1; i++)
        {
            Debug.DrawLine(recallData[i].tracerPosition, recallData[i + 1].tracerPosition);
        }

        RecallInput();
    }

    private void RecallInput()
    {
        if(Input.GetKeyDown(KeyCode.Mouse1))
        {
            StartCoroutine(Recall());
        }
    }
    private void StoreRecallData()
    {
        currentPositionTimer += Time.deltaTime;

        if(collectRecalPosition)
        {
            //This will store an array of tracers location
            if (currentPositionTimer >= secondsBetweenPosition)
            {
                if(recallData.Count >= maxRecallData)
                {
                    //As soon as reach the max recall limit (6) the array will start over at 0
                    recallData.RemoveAt(0);
                }

                recallData.Add(GetRecallData());

                //This will refresh the array
                currentPositionTimer = 0f;
            }
        }
    }

    private RecallData GetRecallData()
    {
        return new RecallData()
        {
            tracerPosition = transform.position,
            tracerRotation = transform.rotation,
            cameraRotation = characterController.transform.rotation
        };
    }

    private IEnumerator Recall()
    {

        //This will won't allow Tracer to collect position data while recalling
        collectRecalPosition = false;

        float secondsForEachPosition = recallDuration / recallData.Count;
        //This will get Tracer from new position to old position in a certain amount of time
        Vector3 currentDataPlayerStartPos = transform.position;
        Quaternion currentPositionPlayerStartRotation = transform.rotation;
        Quaternion currentPositionCameraStartRotation = characterController.transform.rotation;


        while (recallData.Count > 0)
        {
            //the time of the current position of the array
            float time = 0f;

            //this what is going to be changing which is position and rotation overtime
            while (time < secondsForEachPosition)
            {
                transform.position = Vector3.Lerp(currentDataPlayerStartPos, recallData[recallData.Count - 1].tracerPosition, time / secondsForEachPosition);

                transform.rotation = Quaternion.Lerp(currentPositionPlayerStartRotation, recallData[recallData.Count - 1].tracerRotation, time / secondsForEachPosition);

                characterController.transform.rotation = Quaternion.Lerp(currentPositionCameraStartRotation, recallData[recallData.Count - 1].cameraRotation, time / secondsForEachPosition);
                time += Time.deltaTime;

                yield return null;
            }

            //Restart
            currentDataPlayerStartPos = recallData[recallData.Count - 1].tracerPosition;
            currentPositionPlayerStartRotation = recallData[recallData.Count - 1].tracerRotation;
            currentPositionCameraStartRotation = recallData[recallData.Count - 1].cameraRotation;

            recallData.RemoveAt(recallData.Count - 1);
        }

        //Tracer can collect position data again
        collectRecalPosition = true;
    }
}
