using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Pool;

public class SwitchController : MonoBehaviour
{
    public GameObject button_1;
    public GameObject button_2;
    public GameObject button_3;
    public GameObject button_4;

    [Space(10)]
    public GameObject spike_up;
    public GameObject spike_left;
    public GameObject spike_right;
    public GameObject spike_down;


    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other);

        EachSwitchMovement buttonController;

        if (other.gameObject == button_1)
        {
            buttonController = button_1.GetComponent<EachSwitchMovement>();

            if (buttonController.isActivated == true) buttonController.UnActivateSwitch();
            else buttonController.ActivateSwitch();
        }

        else if (other.gameObject == button_2)
        {
            buttonController = button_2.GetComponent<EachSwitchMovement>();

            if (buttonController.isActivated == true) buttonController.UnActivateSwitch();
            else buttonController.ActivateSwitch();
        }

        else if (other.gameObject == button_3)
        {
            buttonController = button_3.GetComponent<EachSwitchMovement>();

            if (buttonController.isActivated == true) buttonController.UnActivateSwitch();
            else buttonController.ActivateSwitch();
        }

        else if (other.gameObject == button_4)
        {
            buttonController = button_4.GetComponent<EachSwitchMovement>();

            if (buttonController.isActivated == true) buttonController.UnActivateSwitch();
            else buttonController.ActivateSwitch();
        }

        ChangeState();
    }

    // 매개변수 false : 올라감  / true : 내려감
    private void SpikeControl(bool up, bool left, bool right, bool down)
    {
        EachSpikeMovement upperSpike = spike_up.GetComponent<EachSpikeMovement>();
        EachSpikeMovement leftSpike = spike_left.GetComponent<EachSpikeMovement>();
        EachSpikeMovement rightSpike = spike_right.GetComponent<EachSpikeMovement>();
        EachSpikeMovement downSpike = spike_down.GetComponent<EachSpikeMovement>();

        if (up)
            upperSpike.OpenSpike();
        else
            upperSpike.CloseSpike();


        if (right)
            rightSpike.OpenSpike();
        else
            rightSpike.CloseSpike();


        if (left)
            leftSpike.OpenSpike();
        else
            leftSpike.CloseSpike();


        if (down)
            downSpike.OpenSpike();
        else
            downSpike.CloseSpike();

    }


    private void ChangeState()
    {
        EachSwitchMovement btn1 = button_1.GetComponent<EachSwitchMovement>();
        EachSwitchMovement btn2 = button_2.GetComponent<EachSwitchMovement>();
        EachSwitchMovement btn3 = button_3.GetComponent<EachSwitchMovement>();
        EachSwitchMovement btn4 = button_4.GetComponent<EachSwitchMovement>();


        if (btn1.isActivated == false && btn2.isActivated == false && btn3.isActivated == false && btn4.isActivated == false)
        {
            // all closed
            SpikeControl(false, false, false, false);
        }
        else if (btn1.isActivated == false && btn2.isActivated == true && btn3.isActivated == false && btn4.isActivated == false)
        {
            // upper opened
            SpikeControl(true, false, false, false);
        }
        else if (btn1.isActivated && btn2.isActivated && btn3.isActivated == false && btn4.isActivated == false)
        {
            // upper, right opened
            Debug.Log("3번째");
            SpikeControl(true, false, true, false);
        }
        else if (btn1.isActivated && btn2.isActivated == false && btn3.isActivated == false && btn4.isActivated == false)
        {
            // left, right opened
            SpikeControl(false, true, true, false);
        }
        else if (btn1.isActivated && btn2.isActivated == false && btn3.isActivated == false && btn4.isActivated)
        {
            // right open
            SpikeControl(false, false, true, false);
        }
        else if (btn1.isActivated && btn2.isActivated && btn3.isActivated == false && btn4.isActivated)
        {
            // right, down opened
            SpikeControl(false, false, true, true);
        }
        else if (btn1.isActivated && btn2.isActivated && btn3.isActivated && btn4.isActivated)
        {
            // left opened
            SpikeControl(false, true, false, false);
        }
        else if (btn1.isActivated == false && btn2.isActivated && btn3.isActivated && btn4.isActivated)
        {
            // left, right opened
            SpikeControl(false, true, true, false);
        }
        else if (btn1.isActivated == false && btn2.isActivated && btn3.isActivated == false && btn4.isActivated)
        {
            //upper, left, right opened
            SpikeControl(true, true, true, false);
        }
    }
}
