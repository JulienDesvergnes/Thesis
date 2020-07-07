using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Globalization;


public class MyLogger : MonoBehaviour
{

    public Text TextRewardvalue;
    public Text TextRCvalue;
    public Text ACtionvalue;

    private CultureInfo en_US = new CultureInfo("en-US");

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetRewardValue(float v) {
        TextRewardvalue.text = v.ToString();
        if (v < 0.0f) {
            TextRewardvalue.color = Color.red;
        } else if (v > 0.0f) {
            TextRewardvalue.color = Color.green;
        } else {
            TextRewardvalue.color = Color.white;
        }
    }

    /*public void AddRewardValue(float v) {
        float oldVAlue = float.Parse(TextRewardvalue.text);
        TextRewardvalue.text = (oldVAlue + v).ToString();
        if (v < 0.0f) {
            TextRewardvalue.color = Color.red;
        } else if (v > 0.0f) {
            TextRewardvalue.color = Color.green;
        } else {
            TextRewardvalue.color = Color.white;
        }
    }*/

    public void AddRewardCumulatedValue(float v) {
        float oldVAlue = float.Parse(TextRCvalue.text);
        TextRCvalue.text = (oldVAlue + v).ToString();
        float newValue = float.Parse(TextRCvalue.text);
        if (newValue < 0.0f) {
            TextRCvalue.color = Color.red;
        } else if (newValue > 0.0f) {
            TextRCvalue.color = Color.green;
        } else {
            TextRCvalue.color = Color.white;
        }
    }

    public void reset() {
        TextRewardvalue.color = Color.white;
        TextRCvalue.color = Color.white;
        ACtionvalue.color = Color.white;

        TextRewardvalue.text = (0.0f).ToString();
        TextRCvalue.text = (0.0f).ToString();
        ACtionvalue.text = "No Action";
    }

    public void resetRewardValue() {
        TextRewardvalue.color = Color.white;
        TextRewardvalue.text = (0.0f).ToString();
    }

    public void setAction(string s) {
        ACtionvalue.text = s;
    }
}
