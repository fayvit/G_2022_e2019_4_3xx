using UnityEngine;
[System.Serializable]
public struct Colisor
{
    public string osso;
    /*
	[NonSerializedAttribute]
	public Vector3 deslColisor;
	[NonSerializedAttribute]
	public Vector3 deslTrail;
*/

    private float dCx;
    private float dCy;
    private float dCz;

    private float dTx;
    private float dTy;
    private float dTz;

    private float colisorScaleX;
    private float colisorScaleY;
    private float colisorScaleZ;


    public Colisor(string _osso, float colisorScale)
    {
        this = new Colisor(_osso, Vector3.zero, Vector3.zero, colisorScale);
    }

    public Colisor(string _osso = "", Vector3 _deslColisor = default(Vector3), Vector3 _deslTrail = default(Vector3), float colisorScale = 1)
    {
        osso = _osso;
        dCx = _deslColisor.x;
        dCy = _deslColisor.y;
        dCz = _deslColisor.z;

        dTx = _deslTrail.x;
        dTy = _deslTrail.y;
        dTz = _deslTrail.z;

        colisorScaleX = colisorScale;
        colisorScaleY = colisorScale;
        colisorScaleZ = colisorScale;
    }

    public Vector3 ColisorScale
    {
        set {
            Vector3 v = value;
            colisorScaleX = v.x;
            colisorScaleY = v.y;
            colisorScaleZ = v.z;
        }
        get
        {
            if (colisorScaleX == 0 || colisorScaleY == 0 || colisorScaleZ == 0)
                ColisorScale = Vector3.one;

            return new Vector3(colisorScaleX,colisorScaleY,colisorScaleZ);
        }
    }

    public Vector3 deslColisor
    {
        get { return new Vector3(dCx, dCy, dCz); }
        set
        {
            Vector3 dC = value;
            dCx = dC.x;
            dCy = dC.y;
            dCz = dC.z;
        }
    }

    public Vector3 deslTrail
    {
        get { return new Vector3(dTx, dTy, dTz); }
        set
        {
            Vector3 dC = value;
            dTx = dC.x;
            dTy = dC.y;
            dTz = dC.z;
        }
    }
}