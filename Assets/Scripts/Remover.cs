﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Remover : MonoBehaviour
{
    public GameObject splash;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CamerFollow>().enabled = false;
            //Camera.main.GetComponent<>;
            if (GameObject.Find("UI_HealthDisplay").activeSelf)
            {
               GameObject.Find("UI_HealthDisplay").SetActive(false);
            }
        }
        Instantiate(splash, collision.transform.position, collision.transform.rotation);
        Destroy(collision.gameObject);
    }
}
