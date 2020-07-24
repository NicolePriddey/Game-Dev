using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CameraPosition { CLOSE, NORMAL, FAR }

public class TempCamera : MonoBehaviour
{
    private GameObject hero;

    private Dictionary<string, float>[] camPositions = new Dictionary<string, float>[3];
    private Dictionary<string, float> cam;

    // Camera angles. Pitch = up-down, Yaw = left-right
    private float pitch = 0;
    private float yaw = 0;
    private float sensitivity = 4;

    void Start()
    {
        float distance = 0;
        float height = 1;
        for (int i = 0; i < camPositions.Length; i++)
        {
            Dictionary<string, float> camPos = new Dictionary<string, float>();
            camPos.Add("distance", distance);
            camPos.Add("height", height);
            camPositions[i] = camPos;

            distance += 5;
            height += 2;
        }
        hero = GameObject.FindGameObjectWithTag("Hero");


        setCam(CameraPosition.NORMAL);
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.Mouse1))
        {
            pitch -= sensitivity * Input.GetAxis("Mouse Y");
            yaw += sensitivity * Input.GetAxis("Mouse X");

            transform.localEulerAngles = new Vector3(pitch, yaw);
        }

        if (Input.GetKeyDown("1")) setCam(CameraPosition.CLOSE);
        if (Input.GetKeyDown("2")) setCam(CameraPosition.NORMAL);
        if (Input.GetKeyDown("3")) setCam(CameraPosition.FAR);

        transform.position = hero.transform.position - (cam["distance"] * transform.forward) + (Vector3.up * cam["height"]);
    }

    /**
     * Sets the camera to the zoom ratio as specified by the parameter.
     */
    private void setCam(CameraPosition position)
    {
        cam = camPositions[(int)position];
    }
}