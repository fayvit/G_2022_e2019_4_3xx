using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FayvitSupportSingleton;
using FayvitUI;
using FayvitCommandReader;
using Unity.Mathematics;
using FayvitSave;

public class ColorDbManager
{
    public string fileName = "DateColors/skinColors.crs";
    private GetColorHudManager myGetColor;
    private ColorGridMenu cMenu;
    private EstadoLocal estado = EstadoLocal.mainState;

    private List<Color> listaDeCores=new List<Color>();
    private int guardado;
    private enum EstadoLocal
    { 
        mainState,
        ordenacao
    }

    public GameObject PaiDoGrid { get => cMenu.GetTransformContainer.gameObject; }
    public Color CurrentGridColor { get => listaDeCores[cMenu.SelectedOption]; }

    // Start is called before the first frame update
    public ColorDbManager(ColorGridMenu mySuggestionColors,GetColorHudManager myGetColor)
    {
        this.myGetColor = myGetColor;
        InteractiveUiReference iRef = SupportCreationUi.Instance.gameObject.AddComponent<InteractiveUiReference>();
        iRef.bMenu = mySuggestionColors;
        iRef = SupportCreationUi.Instance.CreateInteractiveUi(iRef, 0, .5f, .5f, 1);

        cMenu = iRef.bMenu;

        SupportSingleton.Instance.InvokeOnEndFrame(() =>
        {
            cMenu.StartHud(new Color[0] { });
        });

        myGetColor.transform.parent.gameObject.SetActive(true);
    }

    // Update is called once per frame
    public void Update()
    {
        switch (estado)
        {
            case EstadoLocal.mainState:
                MainState();
            break;
            case EstadoLocal.ordenacao:
                OrdenationState();
            break;
        }

        VerifySave();
        VerifyLoad();
    }

    void OrdenationState()
    {
        int x = CommandReader.GetIntTriggerDown("horizontal", Controlador.teclado);
        //float x = CommandReader.GetAxis("horizontal", Controlador.teclado);
        int y = CommandReader.GetIntTriggerDown("vertical", Controlador.teclado);
        //float y = CommandReader.GetAxis("vertical", Controlador.teclado);
        cMenu.ChangeOption(y, x);
        if (CommandReader.GetButtonDown(7, Controlador.teclado))
        {
            if (guardado == -1)
                guardado = cMenu.SelectedOption;
            else
            {
                Color guard = listaDeCores[guardado];
                listaDeCores[guardado] = listaDeCores[cMenu.SelectedOption];
                listaDeCores[cMenu.SelectedOption] = guard;
                guardado = -1;

                cMenu.FinishHud();
                cMenu.StartHud(listaDeCores.ToArray());
            }
        }

        if (CommandReader.GetButtonDown(0, Controlador.teclado))
        {
            listaDeCores.RemoveAt(cMenu.SelectedOption);
            cMenu.FinishHud();
            cMenu.StartHud(listaDeCores.ToArray());

        }

        if (CommandReader.GetButtonDown(9, Controlador.teclado))
        {
            estado = EstadoLocal.mainState;
        }

       
    }

    void VerifySave()
    {
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            SaveColors(listaDeCores, fileName);
        }
    }

    public static void SaveColors(List<Color> listaDeCores,string fileName)
    {
        List<float4> f4 = new List<float4>();
        Color[] C = listaDeCores.ToArray();
        for (int i = 0; i < C.Length; i++)
        {
            f4.Add(new float4(C[i].r, C[i].g, C[i].b, C[i].a));
        }

        RawLoadAndSave.SalvarArquivo(fileName, f4, Application.dataPath);
    }

    public static List<Color> LoadColors(string fileName)
    {
        List<float4> f4 = RawLoadAndSave.CarregarArquivo<List<float4>>(fileName, Application.dataPath);
        List<Color> listaDeCores = new List<Color>();
        foreach (var f in f4)
        {
            listaDeCores.Add(new Color(f.x, f.y, f.z, f.w));
        }

        return listaDeCores;
    }

    void VerifyLoad()
    {
        if (Input.GetKeyDown(KeyCode.Alpha9))
        {

            listaDeCores = LoadColors(fileName);
            cMenu.FinishHud();
            cMenu.StartHud(listaDeCores.ToArray());
        }
    }

    

    void MainState()
    {
        
        //int x = CommandReader.GetIntTriggerDown("horizontal", Controlador.teclado);
        float x = CommandReader.GetAxis("horizontal", Controlador.teclado);
        //int y = CommandReader.GetIntTriggerDown("vertical", Controlador.teclado);
        float y = CommandReader.GetAxis("vertical", Controlador.teclado);
        //int z = CommandReader.GetIntTriggerDown("HDpad", Controlador.teclado);
        float z = CommandReader.GetAxis("HDpad", Controlador.teclado);
        myGetColor.MoveMark(new Vector2(x, y)*5,z*3*Time.deltaTime);

        if (CommandReader.GetButtonDown(7, Controlador.teclado))
        {
            listaDeCores.Add(myGetColor.CurrentColor);
            cMenu.FinishHud();
            SupportSingleton.Instance.InvokeOnEndFrame(() =>
            {
                cMenu.StartHud(listaDeCores.ToArray());
            });
        }

        if (CommandReader.GetButtonDown(9, Controlador.teclado))
        {
            estado = EstadoLocal.ordenacao;
        }
    }
}
