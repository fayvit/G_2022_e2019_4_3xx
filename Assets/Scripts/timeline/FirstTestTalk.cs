using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TalkSpace;
using FayvitCommandReader;
using FayvitBasicTools;
using UnityEngine.Playables;

public class FirstTestTalk : MonoBehaviour
{
    [SerializeField] private GameObject[] chars;

    private PlayableDirector P;
    private LocalState estado = LocalState.emEspera;
    private List<string> conversa = new List<string>() { 
        "Vejam...",
        "Parece que ele chegou"
    };

    private List<string> conversa_b = new List<string>()
    {
        "Então... é você que veio juntar-se a nós?",
                "Nós entendemos os seus motivos.",
                "Também sentimos que há algo errado com o rumo que o império está tomando",
                "Nós estamos tentando abrir a <color=yellow>Torre da Vida Eterna</color> ",
                "para encarar o imperador <color=orange>Logan</color> e mudar destino de <color=yellow>Orion</color>",
                "O caminho para isso é muito longo e muito difícil,",
                " você precisa de muita determinação para percorrer todo o caminho",
                "Nós estamos apenas no meio dele um tanto quanto sem rumo mas já temos algumas lições para tirar",
                "Todos os que querem fazer uma tarefa difícil tem um começo.",
                "E só quem já está no meio do caminho sabe como é dificil começar.",
                "Por isso vamos te ajudar",
                "Siga até a conexão <color=cyan>Armagedom</color> e você poderá pegar um criature de nossa reserva"
    };

    private enum LocalState
    {
        emEspera,
        conversaInicial,
        segundoTrack
    }

    // Start is called before the first frame update
    void Start()
    {
        P = GetComponent<PlayableDirector>();
    }

    private ICommandReader CurrentCommand => CommandReader.GetCR(AbstractGlobalController.Instance.Control);
    // Update is called once per frame
    void Update()
    {
        bool inputNext = CurrentCommand.GetButtonDown(CommandConverterInt.confirmButton);
        bool inputFinish = CurrentCommand.GetButtonDown(CommandConverterInt.returnButton);

        switch (estado)
        {
            case LocalState.conversaInicial:               

                if (DisplayTextManager.instance.DisplayText.UpdateTexts(inputNext, inputFinish, conversa.ToArray()))
                {
                    IniciarSegundoTrack();
                    estado = LocalState.emEspera;
                    
                }
                break;

            case LocalState.segundoTrack:

                P.initialTime = 13;
                P.Play();

                if (DisplayTextManager.instance.DisplayText.UpdateTexts(inputNext, inputFinish, conversa_b.ToArray()))
                {
                    
                    estado = LocalState.emEspera;
                    
                }
                break;
        }
    }

    void IniciarSegundoTrack()
    {
        P.Stop();
        P.initialTime = 7;
        P.Play();
    }

    public void StartFirstMessage()
    {
        
        DisplayTextManager.instance.DisplayText.StartTextDisplay();

        estado = LocalState.conversaInicial;
    }

    public void StartSecondMessage()
    {

        DisplayTextManager.instance.DisplayText.StartTextDisplay();

        estado = LocalState.segundoTrack;
    }

    public void RequestCharactersAnimations()
    {
        for (int i = 0; i < chars.Length; i++)
        {
            chars[i].GetComponent<Animator>().Play("SimpleIdle", 0, Random.Range(0, 1f));
        }

        P.Stop();
    }

    
}
