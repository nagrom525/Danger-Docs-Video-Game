using UnityEngine;
using System.Collections;
using InControl;
using UnityEngine.SceneManagement;
public class MainMenuController : MonoBehaviour
{

    public int playerNum;

    // Use this for initialization
    void Start()
    {

    }

    void Update()
    {
        InputDevice inputDevice = (InputManager.Devices.Count > playerNum) ? InputManager.Devices[playerNum] : null;
        if (inputDevice == null)
        {
            // If no controller exists for this doctor make it translucent.
            //playerRenderer.material.color = new Color(1.0f, 1.0f, 1.0f, 0.2f);
        }
        else
        {
            UpdateWithInputDevice(inputDevice);
        }
    }

    void UpdateWithInputDevice(InputDevice inputDevice)
    {
        if (inputDevice.Action1.WasPressed)
        {
            //start game
            SceneManager.LoadScene("Tutorial");
   
        }
        else
        if (inputDevice.Action2.WasPressed)
        {


        }
        else
        if (inputDevice.Action3.WasPressed)
        {

        }
        else
        if (inputDevice.Action4)
        {
        
        }



    }
}

