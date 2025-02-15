﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIParallax : MonoBehaviour
{
    public GameObject[] layers;
    public static bool active;

    float dragSpeed = 0.01f; //0.01f ANDROID, 0.001f PC
    float mousePosYStart = 0f;
    float minDragY = -15f;
    float maxDragY = 15f;
    float deltaY = 0f;

    private void Start()
    {
        active = true;
    }

    public void setActive(bool flag)
    {
        active = flag;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
            mousePosYStart = Input.mousePosition.y;

        //uncomment to use on PC
        if (((Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Moved /*|| Input.GetMouseButton(0)*/) && MenuManager.cameraStatus != MenuManager.Status.freeOnPlanet) && active == true)
        {
            Vector3 newPosition;

            if (Input.touchCount == 1)
                deltaY = Input.GetTouch(0).deltaPosition.y;
            else
                deltaY = Input.mousePosition.y - mousePosYStart;

            for (int i = 0; i < layers.Length; ++i)
            {
                float newY = Mathf.Clamp(layers[i].transform.position.y + deltaY * dragSpeed * (i + 1) / 10.0f, minDragY, maxDragY);

                if (i == layers.Length - 1)
                {
                    newPosition = new Vector3(layers[i].transform.position.x, newY, layers[i].transform.position.z);

                    layers[i].transform.position = newPosition;

                }
                else if (layers[layers.Length - 1].transform.position.y < maxDragY && layers[layers.Length - 1].transform.position.y > minDragY)
                {

                    newPosition = new Vector3(layers[i].transform.position.x, newY, layers[i].transform.position.z);

                    layers[i].transform.position = newPosition;
                }

            }
        }
    }
}
