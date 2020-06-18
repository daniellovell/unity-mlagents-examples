using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RTSGridViewer : MonoBehaviour
{

    public Material gridMat;
    public RTSGridObserver gridObs;

    private Texture2D texMex;


    void Update()
    {
        texMex = new Texture2D(gridObs.numGridsPerSide, gridObs.numGridsPerSide, TextureFormat.ARGB32, false);

        for (int r = 0; r < gridObs.numGridsPerSide; r++)
        {
            for (int c = 0; c < gridObs.numGridsPerSide; c++)
            {
                if(gridObs.megaMatrix[(int) ObservationType.Danger][r][c] == 0)
                    texMex.SetPixel(r, c, Color.white);
                else if (gridObs.megaMatrix[(int)ObservationType.Danger][r][c] > 0)
                    texMex.SetPixel(r, c, Color.red);
                else
                    texMex.SetPixel(r, c, Color.green);
            }
        }
        texMex.Apply();
        // connect texture to material of GameObject this script is attached to
        gridMat.mainTexture = texMex;
    }

}
