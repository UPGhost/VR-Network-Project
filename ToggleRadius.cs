using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK.Controllables;
public class ToggleRadius : MonoBehaviour {

    public float maxRadius;
    private bool toggle;
    private float currentScale;
    

    GameObject GeologyObject;
    Presentation presentationScript;
    private int previousSceneNumber;
    public bool forcedBombDone;
    public bool forcedBombOpen;
    public bool forcedBombOpenSCENE4sequence1;
    public bool forcedBombDoneSCENE4sequence2;
    public bool forcedBombDoneSCENE5sequence0;
    public bool forcedBombDoneSCENE5sequence1;


    public VRTK_BaseControllable Button1, Button2, Button3, Button4;

    GameObject BombZoom0;
    GameObject BombZoom1;
    GameObject BombZoom2;

    float Value1; //50
    float Value2; //250
    float Value3;  //500

    float Value4; // toggle radius

    private bool button1pressed;
    private bool button2pressed;
    private bool button3pressed;
    private bool button4pressed;


    void Start () {
        transform.localScale = new Vector3(0, 0, 0);
        toggle = true;        

        GeologyObject = GameObject.Find("Geology");
        presentationScript = GeologyObject.GetComponent<Presentation>();

        BombZoom0 = GameObject.Find("BlastRadius (63)");
        BombZoom1 = GameObject.Find("BlastRadius (104)");
        BombZoom2 = GameObject.Find("BlastRadius CPTdrone");

        previousSceneNumber = presentationScript.currentSceneNumber;

        button1pressed = false;
        

}


    // close bomb radius
    IEnumerator AnimateRadiusClose()
    {

        for (float f = currentScale; f > 0; f -= 0.2f)
        {
            transform.localScale = new Vector3(f, f, f);
            currentScale = f;
            yield return null;
        }


    }
    // open bomb radius
    IEnumerator AnimateRadiusOpen()
    {

        for (float f = currentScale; f < maxRadius; f += 0.2f)
        {
            transform.localScale = new Vector3(f, f, f);
            currentScale = f;
            yield return null;
        }
    }

           

    void Update () {

        // value buttons return
        Value1 = Button1.GetNormalizedValue();
        Value2 = Button2.GetNormalizedValue();
        Value3 = Button3.GetNormalizedValue();

        Value4 = Button4.GetNormalizedValue();






        // change bomb radius with weight



        if (Value2 > 0)
        {
            button2pressed = true;
            maxRadius = 12f;
            RadiusClose();

            if (currentScale > 0)
            {
                RadiusClose();
            }


        }


        if (Value3 > 0)
        {
            button3pressed = true;
            maxRadius = 18f;
            RadiusClose();

            if (currentScale > 0)
            {
                RadiusClose();
            }

        }

        if (Value1 > 0)
        {
            button1pressed = true;
            maxRadius = 4f;

            if (currentScale > 0)
            {
                RadiusClose();
            }


        }
        
        if(Value4 > 0)
        {
            TriggerToggle();
            button4pressed = true;
        }
       
      

       

        if (button1pressed == true && currentScale < 0.1f)
        {
            RadiusExpand();
            button1pressed = false;
        }

        if (button2pressed == true && currentScale < 0.1f)
        {
            RadiusExpand();
            button2pressed = false;
        }
        if (button3pressed == true && currentScale < 0.1f)
        {
            RadiusExpand();
            button3pressed = false;
        }


        //presentation stuff

        if ((presentationScript.currentSceneNumber == 102 && previousSceneNumber != 102) || (presentationScript.currentSceneNumber == 106 && previousSceneNumber != 106) || (presentationScript.currentSceneNumber == 103 && previousSceneNumber != 103) && forcedBombDone == false)
        {
            RadiusExpand();
            //forcedBombDone = true;
            presentationScript.forceBombRadiusOpen = false;
        }

        if (presentationScript.forceBombRadiusClose == true && forcedBombDone == false)
        {
            RadiusClose();

            forcedBombDone = true;
            forcedBombOpen = false;
        }

        if (presentationScript.forceBombRadiusOpen == true && forcedBombOpen == false)
        {
            RadiusExpand();

            forcedBombOpen = true;
            forcedBombDone = false;
        }

        if (presentationScript.forceBombRadiusCloseSCENE5sequence0 == true && forcedBombDoneSCENE5sequence0 == false)
        {
            if (this.gameObject == BombZoom0)
            {
                RadiusExpand();
            }
            else
            {
                RadiusClose();
            }

            forcedBombDoneSCENE5sequence0 = true;
            forcedBombDoneSCENE5sequence1 = false;
        }

        if (presentationScript.forceBombRadiusCloseSCENE5sequence1 == true && forcedBombDoneSCENE5sequence1 == false)
        {
            if (this.gameObject == BombZoom1)
            {
                RadiusExpand();
            }
            else
            {
                RadiusClose();
            }

            forcedBombDoneSCENE5sequence1 = true;
            forcedBombDoneSCENE5sequence0 = false;
        }

        if (presentationScript.forceBombRadiusCloseSCENE4sequence2 == true && forcedBombDoneSCENE4sequence2 == false)
        {
            if (this.gameObject == BombZoom2)
            {
                RadiusClose();
            }
            else
            {
                RadiusExpand();
            }

            forcedBombDoneSCENE4sequence2 = true;
        }

        if (presentationScript.forceBombRadiusOpenSCENE4sequence1 == true && forcedBombOpenSCENE4sequence1 == false)
        {
            if (this.gameObject == BombZoom2)
            {
                RadiusExpand();
            }
            else
            {
                RadiusClose();
            }

            forcedBombOpenSCENE4sequence1 = true;
        }


        if (presentationScript.currentSceneNumber != 105)
        {
            forcedBombDoneSCENE5sequence0 = false;
            presentationScript.forceBombRadiusCloseSCENE5sequence0 = false;

            forcedBombDoneSCENE5sequence1 = false;
            presentationScript.forceBombRadiusCloseSCENE5sequence1 = false;
        }

        if (presentationScript.SequenceNumber == 0 || presentationScript.SequenceNumber == presentationScript.MaxScene4Sequences)
        {
            forcedBombOpenSCENE4sequence1 = false;
            presentationScript.forceBombRadiusOpenSCENE4sequence1 = false;
        }

        if (presentationScript.SequenceNumber != presentationScript.MaxScene4Sequences)
        {
            forcedBombDoneSCENE4sequence2 = false;
            presentationScript.forceBombRadiusCloseSCENE4sequence2 = false;
        }

        // button pressed
        if (Input.GetKeyDown(KeyCode.T))
        {
            TriggerToggle();
        }

        previousSceneNumber = presentationScript.currentSceneNumber;


       
       


    }

    public void TriggerToggle()
    {
        if (toggle)
        {
            RadiusExpand();
        }
        else
        {
            RadiusClose();
        }
    }

    void RadiusExpand()
    {
        toggle = !toggle;
        StopAllCoroutines();
        StartCoroutine("AnimateRadiusOpen");
    }

    void RadiusClose()
    {
        toggle = !toggle;
        StopAllCoroutines();
        StartCoroutine("AnimateRadiusClose");
    }
}

