using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombMovement : MonoBehaviour
{

    public GameObject GeologyObject;
    Presentation presentationScript;
    Presentation presentationFUNCTIONS;

    private Vector3 bombOriginalPosition;
    private Vector3 bombCurrentPosition;
    private Vector3 bombEndPosition;
    private float lerpSpeed;

    private bool wasScene102;
    private bool wasScene104;
    private bool checkAnimateBool;
    private bool GoUp;
    private bool GoDown;




    // Use this for initialization
    void Start()
    {

        presentationScript = GeologyObject.GetComponent<Presentation>();
        presentationFUNCTIONS = GetComponent<Presentation>();

        bombOriginalPosition = transform.localPosition;
        bombEndPosition = bombOriginalPosition;
        bombEndPosition.y = 4f;
        lerpSpeed = 5;
        wasScene102 = false;
        wasScene104 = false;
        checkAnimateBool = false;
        GoUp = false;
        GoDown = false;


    }

    // Update is called once per frame
    void Update()
    {
        bombCurrentPosition = transform.localPosition;

        if ((presentationScript.currentSceneNumber == 102) || (presentationScript.currentSceneNumber == 104 && presentationScript.SequenceNumber == presentationScript.MaxScene4Sequences))
        {

            wasScene102 = true;

            if (transform.localPosition == bombOriginalPosition || GoUp == true)
            {
                GoUp = true;

                LerpFullBombFunction(bombCurrentPosition, bombEndPosition, transform.localScale, transform.localScale, transform.localRotation, transform.localRotation, 1);

                if (transform.localPosition == bombEndPosition)
                {
                    GoUp = false;
                    checkAnimateBool = false;
                }
            }

        }

        if (presentationScript.currentSceneNumber != 102 && wasScene102 == true && presentationScript.SequenceNumber != presentationScript.MaxScene4Sequences)
        {

            LerpFullBombFunction(bombCurrentPosition, bombOriginalPosition, transform.localScale, transform.localScale, transform.localRotation, transform.localRotation, 1);

            if (transform.localPosition == bombOriginalPosition)
            {
                wasScene102 = false;
            }

        }
    }

    void LerpLocalTransform(Vector3 LerpPosition, Vector3 LerpScale, Quaternion LerpRotation)
    {
        transform.localPosition = LerpPosition;
        transform.localScale = LerpScale;
        transform.localRotation = LerpRotation;
    }

    void LerpAll(Vector3 OldPosition, Vector3 NewPosition, Vector3 OldScale, Vector3 NewScale, Quaternion OldRotation, Quaternion NewRotation, float lerpSpeedDivided)
    {
        Vector3 LerpPosition = Vector3.Lerp(OldPosition, NewPosition, Time.deltaTime * lerpSpeed / lerpSpeedDivided);
        Vector3 LerpScale = Vector3.Lerp(OldScale, NewScale, Time.deltaTime * lerpSpeed);
        Quaternion LerpRotation = Quaternion.Lerp(OldRotation, NewRotation, Time.deltaTime * lerpSpeed);

        LerpLocalTransform(LerpPosition, LerpScale, LerpRotation);
    }

    public void LerpFullBombFunction(Vector3 theOldPosition, Vector3 theNewPosition, Vector3 theOldScale, Vector3 theNewScale, Quaternion theOldRotation, Quaternion theNewRotation, float theLerpSpeedDivided)
    {
        Vector3 OldPosition = theOldPosition;
        Vector3 NewPosition = theNewPosition;

        Vector3 OldScale = theOldScale;
        Vector3 NewScale = theNewScale;

        Quaternion OldRotation = theOldRotation;
        Quaternion NewRotation = theNewRotation;

        LerpAll(OldPosition, NewPosition, OldScale, NewScale, OldRotation, NewRotation, theLerpSpeedDivided);

        //Set Postion at Max
        if (OldPosition.y > NewPosition.y - 0.05f && OldPosition.y < NewPosition.y + 0.05f)
        {
            transform.localPosition = NewPosition;
        }
    }
}
