﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayButtonScript : MonoBehaviour {

    private Button _button;
 
    // Use this for initialization
    void Start()
    {
        _button = GetComponent<Button>();
        _button.onClick.AddListener(() =>
        {

            Application.LoadLevel("StartPlaying");

        });
    }
}
