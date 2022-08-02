using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testing : MonoBehaviour
{
    Renderer m_Renderer;
    void Start()
    {
        m_Renderer = GetComponent<Renderer>();
    }

    void Update()
    {
        //It means object is NOT visible in the scene if it is false is visible 
        if (!m_Renderer.isVisible)
        {
            Destroy(gameObject);
        }        
    }
}
