/*
    ------------------- Code Monkey -------------------

    Thank you for downloading this package
    I hope you find it useful in your projects
    If you have any questions let me know
    Cheers!

               unitycodemonkey.com
    --------------------------------------------------
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Speedometer : MonoBehaviour {

    private const float MAX_SPEED_ANGLE = 0;
    private const float ZERO_SPEED_ANGLE = 180;

    private Transform needleTranform;
    private Transform speedLabelTemplateTransform;

    public float maxValue;
    public float currentValue;

    private int labelAmount = 4;
    public float targetValue = 0;
    private void Awake() {
        needleTranform = transform.Find("needle");
        speedLabelTemplateTransform = transform.Find("speedLabelTemplate");
        speedLabelTemplateTransform.gameObject.SetActive(false);

        currentValue = -1f;
        maxValue = 1f;
        targetValue = 0f;
        CreateSpeedLabels();
    }

    private void Update() {
        updateArrow();
        needleTranform.eulerAngles = new Vector3(0, 0, GetSpeedRotation());
    }

    private void updateArrow() {
        if (currentValue < targetValue) {
            currentValue += 1f * Time.deltaTime;
        }
        if (currentValue > targetValue) {
            currentValue -= 1f * Time.deltaTime;
        }

        currentValue = Mathf.Clamp(currentValue, -1f, 1);
    }

    private void CreateSpeedLabels() {

        float totalAngleSize = ZERO_SPEED_ANGLE - MAX_SPEED_ANGLE;

        for (int i = 0; i <= labelAmount; i++) {
            Transform speedLabelTransform = Instantiate(speedLabelTemplateTransform, transform);
            float labelSpeedNormalized = (float)i / labelAmount ;
            float speedLabelAngle = ZERO_SPEED_ANGLE - labelSpeedNormalized * totalAngleSize;
            speedLabelTransform.eulerAngles = new Vector3(0, 0, speedLabelAngle);
            speedLabelTransform.Find("speedText").GetComponent<Text>().text = (-1+ labelSpeedNormalized* (maxValue - currentValue)).ToString();
            speedLabelTransform.Find("speedText").eulerAngles = Vector3.zero;
            speedLabelTransform.gameObject.SetActive(true);
        }

        needleTranform.SetAsLastSibling();
    }

    private float GetSpeedRotation() {
        return 90 - currentValue * 90;
    }
}
