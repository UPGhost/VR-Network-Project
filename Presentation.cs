using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoudiniEngineUnity;
using VRTK.Controllables;
using VRTK;



public class Presentation : MonoBehaviour
{

    public int currentSceneNumber;
    private int previousSceneNumber;
    private int[] sceneNumberArray = new int[] { 100, 101, 102, 103, 104, 105, 106 };
    public int SequenceNumber;
    private float lerpSpeed;
    private float lerpSpeedFast;
    private float lerpAnimationSpeed;
    private float lerpBombAnimationSpeed;
    private float rotY;
    private float rotationRate;
    private float lerpingSpeed;
    public Vector3 manipulateMapPosition;
    private Vector3 startPosition;
    private int previousSequenceNumber;
    private bool Interactable;
    private bool checkAnimateBool;
    public bool forceBombRadiusClose;
    public bool forceBombRadiusOpen;
    public bool forceBombRadiusOpenSCENE4sequence1;
    public bool forceBombRadiusCloseSCENE4sequence2;
    public bool forceBombRadiusCloseSCENE5sequence0;
    public bool forceBombRadiusCloseSCENE5sequence1;
    private bool wasSequence2;
    private bool BoundingBoxIsTransparent;
    public int MaxScene4Sequences;
    public int MaxScene5Sequences;
    private Vector3 AwayFromScene;
    private bool justEnteredScene4;
    private bool justEnteredScene5;

    public GameObject CPTObject;
    private Vector3 CPTStartPosition;
    private Vector3 CPTCurrentPosition;
    private Vector3 CPTNewPosition;
    private float CPTyDropPosition;

    public GameObject bombObject;
    private Vector3 bombStartPosition;

    private bool isTransparent;

    public GameObject[] TransparentGroup;
    private List<Material> Tmaterial = new List<Material>();
    private List<Color> TmaterialColor = new List<Color>();
    public float Ttransparency;
    public float previousTranparency;
    private float MinTransparency;
    private float MaxTransparency;
    private bool TisTransparent;

    public GameObject RiverObject;
    private Material Rmaterial;
    private Color RmaterialColor;
    private float Rtransparency;
    private float RminTransparency;
    private float RmaxTransparency;
    private bool RisTransparent;

    private float TransparencyChangeSpeed;

    public GameObject[] TransparentBoundingBoxGroup;
    private List<Material[]> BBmaterials = new List<Material[]>();

    private int yesTransparent;
    private int noTransparent;

    public int renderQueueTransparency;

    public GameObject TunnelStartObject;
    private Material TunnelStartMaterial;
    private Color TunnelStartMaterialColor;
    private float TunnelStartTransparency;
    private bool TunnelStartIsTransparent;

    public GameObject TunnelEndObject; 
    private Material TunnelEndMaterial;
    private Color TunnelEndMaterialColor;
    private float TunnelEndTransparency;
    private bool TunnelEndIsTransparent;

    public clientReceiver clientScript;

    public VRTK_BaseControllable TransparencySlider;


    // Use this for initialization
    void Start()
    {
        currentSceneNumber = 100;
        previousSceneNumber = currentSceneNumber;
        SequenceNumber = 0;
        lerpAnimationSpeed = 1.2f;
        lerpBombAnimationSpeed = lerpAnimationSpeed * 3;
        lerpSpeed = 3;
        lerpSpeedFast = 5;
        rotationRate = 8;
        Interactable = false;
        checkAnimateBool = false;
        BoundingBoxIsTransparent = false;

        MinTransparency = 0;
        MaxTransparency = 1;
        RminTransparency = 0;
        RmaxTransparency = 0.3f;

        TransparencyChangeSpeed = 50;

        Ttransparency = MaxTransparency;
        Rtransparency = Ttransparency / 3;

        TisTransparent = false;
        RisTransparent = false;

        yesTransparent = 2999;
        noTransparent = 3000;       

        manipulateMapPosition = new Vector3(0, 1.75f, 0);

        startPosition = new Vector3(0, 1, 0);
        transform.position = startPosition;

        CPTStartPosition = CPTObject.transform.localPosition;
        CPTyDropPosition = -0.02923f;

        bombStartPosition = bombObject.transform.localPosition;

        MaxScene4Sequences = 8;
        MaxScene5Sequences = 1;

        AwayFromScene = new Vector3(999, 999, 999);

        TransparencySlider.ValueChanged += transparencySlider_ValueChanged;

        //Transparent

        for (int T = 0; T < TransparentGroup.Length; T++)
        {
            Tmaterial.Add(TransparentGroup[T].GetComponent<Renderer>().material);
            TmaterialColor.Add(Tmaterial[T].color);
        }

        //River

        Rmaterial = RiverObject.GetComponent<Renderer>().material;
        RmaterialColor = Rmaterial.color;


        //LOOK AT TRANSPARENT SCRIPTS, REWRITE THEM IN HERE

        //BB Transparent

        for (int BB = 0; BB < TransparentBoundingBoxGroup.Length; BB++)
        {
            BBmaterials.Add(TransparentBoundingBoxGroup[BB].GetComponent<Renderer>().materials);
        }

        //Tunnel

        TunnelStartMaterial = TunnelStartObject.GetComponent<Renderer>().material;
        TunnelEndMaterial = TunnelEndObject.GetComponent<Renderer>().material;
        TunnelStartMaterialColor = TunnelStartMaterial.color;
        TunnelEndMaterialColor = TunnelEndMaterial.color;

    }

    // Update is called once per frame
    void Update()
    {
        sceneSwitch();
        ScenePlay();
        previousSceneNumber = currentSceneNumber;

    }



    void ScenePlay()
    {
        //Initialise

        TransparentBoundingBox();

        if (currentSceneNumber < sceneNumberArray[4])
        {
            TunnelIsTransparent();
        }

        if (currentSceneNumber > sceneNumberArray[4])
        {
            TunnelIsNotTransparent();
            
        }

        // If NOT Scene
        
        if ((currentSceneNumber != sceneNumberArray[3] && previousSceneNumber == sceneNumberArray[3]) || (currentSceneNumber != sceneNumberArray[5] && previousSceneNumber == sceneNumberArray[5]))
        {
            TransparentGroup[6].GetComponent<MeshRenderer>().enabled = true;
        }
        

        if (currentSceneNumber != sceneNumberArray[4])
        {
            CPTObject.transform.localPosition = CPTStartPosition;


            for (int AT = 2; AT < TransparentGroup.Length; AT++)
            {
                TransparentGroup[AT].GetComponent<MeshRenderer>().enabled = true;
            }

            
        }

        if (currentSceneNumber != sceneNumberArray[4] && currentSceneNumber != sceneNumberArray[5])
        {
            RiverObject.GetComponent<MeshRenderer>().enabled = true;
        }

        if (currentSceneNumber != sceneNumberArray[4])
        {
            justEnteredScene4 = true;
        }

        if (currentSceneNumber != sceneNumberArray[5])
        {
            justEnteredScene5 = true;
        }

        if (currentSceneNumber != sceneNumberArray[4] && currentSceneNumber != sceneNumberArray[5])
        {
            SequenceNumber = 0;
        }

        if (currentSceneNumber != sceneNumberArray[6])
        {
            Interactable = false;
            isTransparent = false;
        }

        if (currentSceneNumber > sceneNumberArray[4])
        {
            CPTObject.transform.localPosition = AwayFromScene;
            bombObject.transform.localPosition = AwayFromScene;
        }

        //Scene 0 - Under Table

        if (currentSceneNumber == sceneNumberArray[0])
        {
            NothingTransparent(false);
            LerpQuickFunction(transform.position, startPosition, transform.localScale, new Vector3(1, 1, 1), transform.localRotation, Quaternion.identity, 4);
        }

        //Scene 1 - Over Table, Original

        if (currentSceneNumber == sceneNumberArray[1])
        {
            NothingTransparent(false);
            LerpFullFunction(transform.position, new Vector3(0, startPosition.y + 0.55f, 0), transform.localScale, new Vector3(1, 1, 1), transform.localRotation, Quaternion.identity, 1);
        }

        //Scene 2 - Rotated, 2D Map

        if (currentSceneNumber == sceneNumberArray[2])
        {
            NothingTransparent(false);
            LerpFullFunction(transform.position, new Vector3(0, startPosition.y + 1.25f, 0), transform.localScale, new Vector3(1.15f, 1.15f, 1.15f), transform.localRotation, Quaternion.Euler(0, 0, 90), 1);
        }

        //Scene 3 - Transparent Layers

        if (currentSceneNumber == sceneNumberArray[3])
        {
            //TransparentAtBoundingBox(noTransparent, 0);
            TransparentAtBoundingBox(yesTransparent, 0);
            bombOpen();
            EverythingTransparent(true);
            TransparentGroup[6].GetComponent<MeshRenderer>().enabled = false;

            float NewScale3float = 2.0f;

            Vector3 OldPosition = transform.position;
            Vector3 NewPosition = new Vector3(0, startPosition.y + 0.6f, 0);


            Vector3 OldScale = transform.localScale;
            Vector3 NewScale = new Vector3(NewScale3float, NewScale3float, NewScale3float);


            Quaternion OldRotation = transform.localRotation;
            Quaternion NewRotation = Quaternion.identity;

            //Speed Up Final Part of Transition
            if (OldPosition.y > NewPosition.y - 0.05f && OldPosition.y < NewPosition.y + 0.05f || (OldRotation.z > -0.05f && OldRotation.z < 0.05f))
            {
                lerpingSpeed = lerpSpeedFast;
            }
            else
            {
                lerpingSpeed = lerpSpeed;
            }

            Vector3 LerpPosition = Vector3.Lerp(OldPosition, NewPosition, Time.deltaTime * lerpingSpeed);
            Vector3 LerpScale = Vector3.Lerp(OldScale, NewScale, Time.deltaTime * lerpSpeed);
            Quaternion LerpRotation = Quaternion.Lerp(OldRotation, NewRotation, Time.deltaTime * lerpSpeed);


            //Spin Around
            if ((OldPosition.y > NewPosition.y - 0.01f && OldPosition.y < NewPosition.y + 0.01f) && (OldScale.x > NewScale.x - 0.01f && OldScale.x < NewScale.x + 0.01f) &&
                (OldRotation.z > -0.001 && OldRotation.z < 0.001f))
            {
                transform.position = OldPosition;
                transform.localScale = NewScale;

                rotY += Time.deltaTime * rotationRate;

                NewRotation = Quaternion.Euler(0, rotY, 0);
                transform.localRotation = NewRotation;
            }

            //Transition Normal
            else
            {
                LerpTransform(LerpPosition, LerpScale, LerpRotation);
                rotY = 0;
            }

        }

        //Scene 4 - Zoom on Tunnel

        if (currentSceneNumber == sceneNumberArray[4])
        {

            if (justEnteredScene4 == true)
            {
                SequenceNumber = 0;
                justEnteredScene4 = false;

            }


            if (SequenceNumber < 3 || SequenceNumber == MaxScene4Sequences)
            {
                CheckChangePosition();
            }

            TransparentGroupTrue();
            FinalTransparency(false);

            
            for (int AT = 2; AT < 7; AT++)
            {
                TransparentGroup[AT].GetComponent<MeshRenderer>().enabled = false;
            }
            

            RiverObject.GetComponent<MeshRenderer>().enabled = false;

            Vector3 OldPosition = transform.position;
            Vector3 NewPosition;


            Vector3 OldScale = transform.localScale;
            Vector3 NewScale;
            float Scale;

            Quaternion OldRotation = transform.localRotation;
            Quaternion NewRotation = Quaternion.Euler(0, 34.2f, 0);

            TransparentAtBoundingBox(noTransparent, 0);
            TransparentAtBoundingBox(yesTransparent, 4);

            if (SequenceNumber == 0)
            {
                TunnelIsTransparent();
                bombOpen();
                NewPosition = new Vector3(0.63f, startPosition.y + 0.6f, 1.458f);
                Scale = 5;
                TunnelAppear(false, false);

            }
            else if (SequenceNumber == 1)
            {
                forceBombRadiusOpenSCENE4sequence1 = true;
                NewPosition = new Vector3(0.63f, startPosition.y + 1.0f, 2f);
                Scale = 7;
            }
            else if (SequenceNumber == 2)
            {
                NewPosition = new Vector3(0.63f, startPosition.y + 1.0f, 2f);
                Scale = 7;
                forceBombRadiusCloseSCENE4sequence2 = true;
                TunnelAppear(true, false);

                if (TunnelStartTransparency < 0.99f)
                {
                    TunnelStartTransparency = Mathf.Lerp(TunnelStartTransparency, MaxTransparency, Time.deltaTime);
                }
                else
                {
                    TunnelStartTransparency = MaxTransparency;
                }
            }
            else if (SequenceNumber == MaxScene4Sequences)
            {
                //TunnelStartTransparency = MaxTransparency;
                
                NewPosition = new Vector3(0, startPosition.y + 1.15f, 0);
                Scale = 1.15f;
                NewRotation = Quaternion.Euler(0, 0, 90);
                bombObject.transform.localPosition = AwayFromScene;
                CPTObject.transform.localPosition = AwayFromScene;
                TunnelAppear(true, true);
            }
            else
            {
                TunnelStartTransparency = MaxTransparency;
                NewPosition = new Vector3(1.314f, startPosition.y + 1.3f, 1.454f);
                Scale = 13;
            }


            NewScale = new Vector3(Scale, Scale, Scale);

            //Speed Up Final Part of Transition
            if (OldPosition.y > NewPosition.y - 0.05f && OldPosition.y < NewPosition.y + 0.05f)
            {
                lerpingSpeed = lerpSpeedFast;
            }
            else
            {
                lerpingSpeed = lerpSpeed;
            }

            //Animation

            if (((OldPosition.y > NewPosition.y - 0.01f && OldPosition.y < NewPosition.y + 0.01f) && (OldScale.x > Scale - 0.01f && OldScale.x < Scale + 0.01f) &&
                (OldRotation.z > -0.001 && OldRotation.z < 0.001f)) && SequenceNumber >= 3)
            {
                SequenceAnimation(CPTyDropPosition);
            }
            else
            {
                bombObject.transform.localPosition = bombStartPosition;
                CPTObject.transform.localPosition = CPTStartPosition;
            }

            LerpAll(OldPosition, NewPosition, OldScale, NewScale, OldRotation, NewRotation, 1);

            previousSequenceNumber = SequenceNumber;
        }

        //Scene 5 - Zoom on Bombs

        if (currentSceneNumber == sceneNumberArray[5])
        {
           TransparentAtBoundingBox(yesTransparent, 0);
           //TransparentAtBoundingBox(yesTransparent, 4);
           TransparentGroup[6].GetComponent<MeshRenderer>().enabled = false;
            RiverObject.GetComponent<MeshRenderer>().enabled = false;

            if (justEnteredScene5 == true)
            {
                SequenceNumber = 0;
                justEnteredScene5 = false;
                EverythingTransparent(false);
                

            }

            CheckChangePosition();


            if (SequenceNumber == 0)
            {
                LerpFullFunction(transform.position, new Vector3(1.65f, startPosition.y + 1.2f, -0.9f), transform.localScale, new Vector3(10, 10, 10), transform.localRotation, Quaternion.Euler(0, 90, 0), 2f);
                forceBombRadiusCloseSCENE5sequence0 = true;
                forceBombRadiusCloseSCENE5sequence1 = false;
            }

            if (SequenceNumber == 1)
            {
                LerpFullFunction(transform.position, new Vector3(-3.376f, startPosition.y + 1.2f, 4f), transform.localScale, new Vector3(10, 10, 10), transform.localRotation, Quaternion.identity, 1.2f);
                forceBombRadiusCloseSCENE5sequence1 = true;
                forceBombRadiusCloseSCENE5sequence0 = false;
            }

        }

        //Scene 6 - Full Interaction

        if (currentSceneNumber == sceneNumberArray[6])
        {
            InteractableTransparencyFunction();
            FinalTransparency(true);
            //bombOpen();
            

            if (Interactable == false)
            {
                ResetPosition();                
                NothingTransparent(true);
            }

            Interactable = true;

            /*
            if (Interactable == false)
            {

                float NewPosition5y = startPosition.y + 1;
                float NewScale5float = 1;

                Vector3 OldPosition = transform.position;
                Vector3 NewPosition = new Vector3(0, startPosition.y + 0.75f, 0);

                Vector3 OldScale = transform.localScale;
                Vector3 NewScale = new Vector3(NewScale5float, NewScale5float, NewScale5float);

                Quaternion OldRotation = transform.localRotation;
                Quaternion NewRotation = Quaternion.identity;

                //Speed Up Final Part of Transition (If close to final position, speed up Lerp)
                if (OldPosition.y > NewPosition5y - 0.05f && OldPosition.y < NewPosition5y + 0.05f)
                {
                    lerpingSpeed = lerpSpeedFast;
                }
                else
                {
                    lerpingSpeed = lerpSpeed;
                }

                //Stop Moving when Close (Allows for Map Manipulation)

                if ((OldPosition.y > NewPosition5y - 0.01f || OldPosition.y < NewPosition5y + 0.01f) && (OldScale.x > NewScale5float - 0.005f || OldScale.x < NewScale5float + 0.005f))
                {
                    LerpAll(OldPosition, NewPosition, OldScale, NewScale, OldRotation, NewRotation, 1);
                }
                else
                {
                    Interactable = true;
                }
            }
            //GetComponent<ManipulateMap>().startPos = transform.position;
            */

        }

        previousTranparency = Ttransparency;
        //previousNewTransparency = NewTransparency;

    }

    void bombOpen()
    {
        forceBombRadiusOpen = true;
        forceBombRadiusClose = false;
    }

    void bombClose()
    {
        forceBombRadiusOpen = false;
        forceBombRadiusClose = true;
    }

    void TunnelIsTransparent()
    {
        TunnelStartTransparency = 0;
        TunnelEndTransparency = 0;

        TunnelStartIsTransparent = true;
        TunnelEndIsTransparent = true;

        TunnelAppear(false, false);
    }
    void TunnelIsNotTransparent()
    {
        TunnelStartTransparency = 1;
        TunnelEndTransparency = 1;

        TunnelStartIsTransparent = false;
        TunnelEndIsTransparent = false;

        TunnelAppear(true, true);
    }

    void TunnelAppear(bool Start, bool End)
    {
        TunnelStartObject.GetComponent<MeshRenderer>().enabled = Start;
        TunnelEndObject.GetComponent<MeshRenderer>().enabled = End;
    }

    void InteractableTransparencyFunction()
    {
        isTransparent = true;
        TransparentBoundingBoxInteractable();

        if (isTransparent == true)
        {
            if (Input.GetKey("j"))
            {
                TransparencyDown();
            }

            if (Input.GetKey("k"))
            {
                TransparencyUp();
            }
        }
    }

    public void TransparencyDown()
    {
        if (Ttransparency > 0)
        {
            Ttransparency -= MaxTransparency / TransparencyChangeSpeed;
            //ATtransparency -= MaxTransparency / TransparencyChangeSpeed;
            //Rtransparency -= RmaxTransparency / TransparencyChangeSpeed;
            Rtransparency = Ttransparency / 3;
        }
    }

    public void TransparencyUp()
    {
        if (Ttransparency < 1)
        {
            Ttransparency += MaxTransparency / TransparencyChangeSpeed;
            //ATtransparency += MaxTransparency / TransparencyChangeSpeed;
            //Rtransparency += RmaxTransparency / TransparencyChangeSpeed;
            Rtransparency = Ttransparency / 3;
        }
    }

    void EverythingTransparent(bool IncludingTopLayers)
    {
        TransparentGroupTrue();
        //AnimTransparentGroupTrue();
        RiverTransparentTrue();
        FinalTransparency(IncludingTopLayers);
    }

    void NothingTransparent(bool IncludingTopLayers)
    {
        TransparentGroupFalse();
        //AnimTransparentGroupFalse();
        RiverTransparentFalse();
        FinalTransparency(IncludingTopLayers);
    }

    void TransparentGroupTrue()
    {
        Ttransparency = MinTransparency;
        TisTransparent = true;
    }

    void TransparentGroupFalse()
    {
        Ttransparency = MaxTransparency;
        TisTransparent = false;
    }
    /*
        void AnimTransparentGroupTrue()
        {
            ATtransparency = 0;
            ATisTransparent = true;
        }

        void AnimTransparentGroupFalse()
        {
            ATtransparency = 1;
            ATisTransparent = false;
        }
    */
    void RiverTransparentTrue()
    {
        Rtransparency = RminTransparency;
        RisTransparent = true;
    }

    void RiverTransparentFalse()
    {
        Rtransparency = RmaxTransparency;
        RisTransparent = false;
    }

    void FinalTransparency(bool IncludingTopLayers)
    {
        /*
        if(NewTransparency != previousNewTransparency)
        {
            Ttransparency = Newtranparency;
            Rtransparency = Ttransparency/3;
        }
        */

      

        if (IncludingTopLayers == true)
        {
            for (int T = 0; T < TransparentGroup.Length; T++)
            {
                Color TColor = TmaterialColor[T];
                TColor.a = Ttransparency;
                Tmaterial[T].color = TColor;
            }
        }
        else
        {
            for (int T = 0; T < 7; T++)
            {
                Color TColor = TmaterialColor[T];
                TColor.a = Ttransparency;
                Tmaterial[T].color = TColor;
            }

            for (int T = 7; T < TransparentGroup.Length; T++)
            {
                Color TColor = TmaterialColor[T];
                TColor.a = 1;
                Tmaterial[T].color = TColor;
            }
        }

        RmaterialColor.a = Rtransparency;
        Rmaterial.color = RmaterialColor;

        TunnelStartMaterialColor.a = TunnelStartTransparency;
        TunnelStartMaterial.color = TunnelStartMaterialColor;

        TunnelEndMaterialColor.a = TunnelEndTransparency;
        TunnelEndMaterial.color = TunnelEndMaterialColor;
    }

    void TransparentAtBoundingBox(int Transparent, int StartingFromLayer)
    {
        renderQueueTransparency = Transparent;

        if (renderQueueTransparency == yesTransparent || renderQueueTransparency == noTransparent)
        {

            for (int BB = StartingFromLayer; BB < BBmaterials[0].Length; BB++)
            {
                BBmaterials[0][BB].renderQueue = renderQueueTransparency;
            }


            if (renderQueueTransparency == yesTransparent)
            {
                BoundingBoxIsTransparent = true;
            }

            else if (renderQueueTransparency == noTransparent)
            {
                BoundingBoxIsTransparent = false;
            }
        }
    }
    void TransparentBoundingBoxInteractable()
    {
        if (Ttransparency > 0.9f)
        {
            TransparentAtBoundingBox(noTransparent, 0);
        }
        else
        {
            TransparentAtBoundingBox(yesTransparent, 0);
        }
    }
    void TransparentBoundingBox()
    {

        if (/*currentSceneNumber == sceneNumberArray[6] ||*/ currentSceneNumber == sceneNumberArray[3] || currentSceneNumber == sceneNumberArray[5])
        {
            TransparentAtBoundingBox(yesTransparent, 0);
        }
        else if (currentSceneNumber != sceneNumberArray[3] && currentSceneNumber != sceneNumberArray[4] && currentSceneNumber != sceneNumberArray[5] && currentSceneNumber != sceneNumberArray[6])
        {
            TransparentAtBoundingBox(noTransparent, 0);
        }

    }

    void LerpTransform(Vector3 LerpPosition, Vector3 LerpScale, Quaternion LerpRotation)
    {
        transform.position = LerpPosition;
        transform.localScale = LerpScale;
        transform.localRotation = LerpRotation;
    }

    void LerpAll(Vector3 OldPosition, Vector3 NewPosition, Vector3 OldScale, Vector3 NewScale, Quaternion OldRotation, Quaternion NewRotation, float lerpSpeedDivided)
    {
        Vector3 LerpPosition = Vector3.Lerp(OldPosition, NewPosition, Time.deltaTime * lerpSpeed / lerpSpeedDivided);
        Vector3 LerpScale = Vector3.Lerp(OldScale, NewScale, Time.deltaTime * lerpSpeed);
        Quaternion LerpRotation = Quaternion.Lerp(OldRotation, NewRotation, Time.deltaTime * lerpSpeed);

        LerpTransform(LerpPosition, LerpScale, LerpRotation);
    }


    public void LerpQuickFunction(Vector3 theOldPosition, Vector3 theNewPosition, Vector3 theOldScale, Vector3 theNewScale, Quaternion theOldRotation, Quaternion theNewRotation, float theLerpSpeedDivided)
    {
        Vector3 OldPosition = theOldPosition;
        Vector3 NewPosition = theNewPosition;

        Vector3 OldScale = theOldScale;
        Vector3 NewScale = theNewScale;

        Quaternion OldRotation = theOldRotation;
        Quaternion NewRotation = theNewRotation;

        LerpAll(OldPosition, NewPosition, OldScale, NewScale, OldRotation, NewRotation, theLerpSpeedDivided);
    }

    public void LerpFullFunction(Vector3 theOldPosition, Vector3 theNewPosition, Vector3 theOldScale, Vector3 theNewScale, Quaternion theOldRotation, Quaternion theNewRotation, float theLerpSpeedDivided)
    {
        Vector3 OldPosition = theOldPosition;
        Vector3 NewPosition = theNewPosition;

        Vector3 OldScale = theOldScale;
        Vector3 NewScale = theNewScale;

        Quaternion OldRotation = theOldRotation;
        Quaternion NewRotation = theNewRotation;

        //Speed Up Final Part of Transition
        if (OldPosition.y > NewPosition.y - 0.05f && OldPosition.y < NewPosition.y + 0.05f)
        {
            lerpingSpeed = lerpSpeedFast;
            lerpingSpeed = lerpSpeedFast;
        }
        else
        {
            lerpingSpeed = lerpSpeed;
        }

        LerpAll(OldPosition, NewPosition, OldScale, NewScale, OldRotation, NewRotation, theLerpSpeedDivided);
    }

    void CheckChangePosition()
    {
        if (Input.GetKeyDown("c"))
        {
            SequenceChange();
        }
    }

    public void SequenceChange()
    {
        SequenceNumber += 1;

        if ((SequenceNumber > MaxScene4Sequences && currentSceneNumber == sceneNumberArray[4]) || (SequenceNumber > MaxScene5Sequences && currentSceneNumber == sceneNumberArray[5]))
        {
            SequenceNumber = 0;
        }
    }


    public void ResetPosition()
    {
        transform.position = new Vector3(0, startPosition.y + 0.75f, 0);
        transform.localScale = new Vector3(1, 1, 1);
        transform.rotation = Quaternion.identity;
    }


    public void SequenceAnimation(float yDropPosition)
    {
        bombClose();

        CheckChangePosition();

        if (SequenceNumber >= 3 && SequenceNumber < 6)
        {
            bombObject.transform.localPosition = bombStartPosition;
        }

        if (SequenceNumber == 4)
        {

            bombStartPosition = bombObject.transform.localPosition;

            if (wasSequence2 == true)
            {
                resetCPTposition();
            }

            CPTdropAnimation(yDropPosition);

        }

        if (((CPTCurrentPosition.y > CPTNewPosition.y - 0.0003f && CPTCurrentPosition.y < CPTNewPosition.y + 0.0003f) && CPTObject.transform.localPosition != CPTNewPosition) && SequenceNumber == 4 || SequenceNumber == 5)
        {
            sequenceCheck(4, 5);
            checkAnimateBool = false;
            CPTObject.transform.localPosition = CPTNewPosition;

        }

        if (SequenceNumber >= 6)
        {
            CPTObject.transform.localPosition = AwayFromScene;
            bombObject.transform.localPosition = AwayFromScene;
            checkAnimateBool = false;
            wasSequence2 = true;
        }

        if (SequenceNumber == 7)
        {
            TunnelAppear(true, true);
            if (TunnelEndTransparency < 0.99f)
            {
                TunnelEndTransparency = Mathf.Lerp(TunnelEndTransparency, 1, Time.deltaTime);
            }
            else
            {
                TunnelEndTransparency = 1;
            }
        }

    }


    void resetCPTposition()
    {
        CPTObject.transform.localPosition = CPTStartPosition;
        wasSequence2 = false;
    }

    void CPTdropAnimation(float yDropPosition)
    {
        CPTCurrentPosition = CPTObject.transform.localPosition;
        CPTNewPosition = new Vector3(CPTCurrentPosition.x, yDropPosition, CPTCurrentPosition.z);

        Vector3 LerpPosition = Vector3.Lerp(CPTCurrentPosition, CPTNewPosition, Time.deltaTime * lerpAnimationSpeed);
        CPTObject.transform.localPosition = LerpPosition;
    }

    void sequenceCheck(int SequenceNumber1, int SequenceNumber2)
    {
        if (SequenceNumber == SequenceNumber1)
        {
            SequenceNumber = SequenceNumber2;
        }
    }

    void sceneSwitch()
    {
        /*
        if (Input.GetKeyDown("0") && currentSceneNumber != sceneNumberArray[0])
        {
            currentSceneNumber = sceneNumberArray[0];
        }

        if (Input.GetKeyDown("1") && currentSceneNumber != sceneNumberArray[1])
        {
            currentSceneNumber = sceneNumberArray[1];
        }

        if (Input.GetKeyDown("2") && currentSceneNumber != sceneNumberArray[2])
        {
            currentSceneNumber = sceneNumberArray[2];
        }

        if (Input.GetKeyDown("3") && currentSceneNumber != sceneNumberArray[3])
        {
            currentSceneNumber = sceneNumberArray[3];
        }

        if (Input.GetKeyDown("4") && currentSceneNumber != sceneNumberArray[4])
        {
            currentSceneNumber = sceneNumberArray[4];
        }

        if (Input.GetKeyDown("5") && currentSceneNumber != sceneNumberArray[5])
        {
            currentSceneNumber = sceneNumberArray[5];
        }

        if (Input.GetKeyDown("6") && currentSceneNumber != sceneNumberArray[6])
        {
            currentSceneNumber = sceneNumberArray[6];
        }
        */

        if (Input.GetKeyDown(KeyCode.LeftArrow) && currentSceneNumber != sceneNumberArray[0])
        {
            currentSceneNumber -= 1;
        }

        if (Input.GetKeyDown(KeyCode.RightArrow) && currentSceneNumber != sceneNumberArray[6])
        {
            currentSceneNumber += 1;
        }

    }

    public virtual void transparencySlider_ValueChanged(object sender, ControllableEventArgs e)
    {
        Ttransparency = e.value;
    }


}


