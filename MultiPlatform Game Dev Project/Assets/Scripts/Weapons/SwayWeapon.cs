using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;

public class SwayWeapon : MonoBehaviour
{
    // Start is called before the first frame update

    #region Variables
    public float swayAmount;
    public float smoothFactor;

    private Quaternion origin;

    #endregion

    void Start()
    {
        origin = transform.localRotation;
    }

    // Update is called once per frame
    void Update()
    {
        // Every frame, get mouse X-axis and Y-axis movements
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        Quaternion rotX = Quaternion.AngleAxis(mouseX * swayAmount, Vector3.up);
        Quaternion rotY = Quaternion.AngleAxis(mouseY * swayAmount, Vector3.right);

        Quaternion finalRotation = origin * rotX * rotY;
        transform.localRotation = Quaternion.Lerp(transform.localRotation, finalRotation, smoothFactor * Time.deltaTime);
    }
}
