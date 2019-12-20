using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoundingBoxRenderQueue : MonoBehaviour {

    public GameObject[] TransparentBoundingBoxGroup;
    private List<Material[]> BBmaterials = new List<Material[]>();

    private static int yesTransparent;
    private static int noTransparent;

    public int renderQueueTransparency;

    // Use this for initialization
    void Start () {

        yesTransparent = 2999;
        noTransparent = 3000;

        for (int BB = 0; BB < TransparentBoundingBoxGroup.Length; BB++)
        {
            BBmaterials.Add(TransparentBoundingBoxGroup[BB].GetComponent<Renderer>().materials);
        }

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void TransparentAtBoundingBox(int Transparent, int StartingFromLayer)
    {
        renderQueueTransparency = Transparent;

        if (renderQueueTransparency == yesTransparent || renderQueueTransparency == noTransparent)
        {

            for (int BB = StartingFromLayer; BB < BBmaterials[0].Length; BB++)
            {
                BBmaterials[0][BB].renderQueue = renderQueueTransparency;
            }

        }
    }



}
