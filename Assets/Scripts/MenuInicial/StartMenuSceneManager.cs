using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using FayvitBasicTools;
using FayvitMessageAgregator;
using FayvitSounds;
using FayvitUI;
using FayvitLoadScene;
using TextBankSpace;
using FayvitSave;
using FayvitCommandReader;
using Criatures2021;

public class StartMenuSceneManager : MonoBehaviour
{
    [SerializeField] private BasicMenu menu;
    [SerializeField] private LoadMenuManager containerDeLoads;
    [SerializeField] private NameMusicaComVolumeConfig menuMusic = new NameMusicaComVolumeConfig() {
        Musica = NameMusic.Brasileirinho,
        Volume = 1
    };

    private LocalState state = LocalState.menuAtivo;

    private enum LocalState
    { 
        menuAtivo,
        externalAction,
        saveLoadsAberto,
        oneMessageOpened,
        confirmationOpened
    }

    private ICommandReader Commander => CommandReader.GetCR(AbstractGlobalController.Instance.Control);

    // Start is called before the first frame update
    void Start()
    {
        //PlayerPrefs.DeleteAll();
        GlobalControllerDependecies();
        MenuPrincipal();
        Criatures2021.ProvisionalEnemyInstantiate c = gameObject.AddComponent<Criatures2021.ProvisionalEnemyInstantiate>();
        c.xVertePosition = new Vector2(465,497);
        c.zVertePosition = new Vector2(411, 443);
        c.maxCriatures = 12;

        SaveDatesManager.Load();
        //SaveDatesManager.RemoverSavesNulosDaLista();

        for (int i = 0; i < SaveDatesManager.s.SavedGames.Count; i++)
            Debug.Log("Save dates: " + i + " é nulo?: " + (SaveDatesManager.s.SavedGames[i] == null));
        for (int i = 0; i < SaveDatesManager.s.SaveProps.Count; i++)
        {
            Debug.Log("save props: "+SaveDatesManager.s.SaveProps[i] + " : " + SaveDatesManager.s.SaveProps[i].indiceDoSave);
        }
    }

    void MenuPrincipal()
    {
        state = LocalState.menuAtivo;
        menu.StartHud(EscolhaDeMenu, TextBank.RetornaListaDeTextoDoIdioma(TextKey.menuInicial).ToArray());
    }

    private void GlobalControllerDependecies()
    {
        if (StaticInstanceExistence<IGlobalController>.SchelduleExistence(GlobalControllerDependecies, this, () =>
        {
            return AbstractGlobalController.Instance;
        }))
        {
            MessageAgregator<MsgStartMusic>.Publish(new MsgStartMusic()
            {
                nmcvc = menuMusic
            });
        }
    }

    void EscolhaDeMenu(int x)
    {
        state = LocalState.externalAction;
        switch (x)
        {
            case 0:
                IniciarJogo();
            break;
            case 1:
                MenuDeCarregarJogo();
            break;
        }

        menu.FinishHud();
        
    }

    void MenuDeCarregarJogo()
    {
        if (SaveDatesManager.s.SaveProps.Count > 0)
        {
            containerDeLoads.StartHud(EscolhiQualCarregar, EscolhiDelete, SaveDatesManager.s.SaveProps.ToArray());
            state = LocalState.saveLoadsAberto;
        }
        else {
            state = LocalState.oneMessageOpened;
            AbstractGlobalController.Instance.OneMessage.StartMessagePanel(() =>
            {
                MenuPrincipal();
            }, "Não há nenhum arquivo de save");
        }
    }

    private void EscolhiDelete(int obj)
    {
        List<PropriedadesDeSave> lp= SaveDatesManager.s.SaveProps;
        containerDeLoads.ChangeInteractiveButtons(false);
        AbstractGlobalController.Instance.Confirmation.StartConfirmationPanel(()=> {
            containerDeLoads.FinishHud();
            PropriedadesDeSave p = lp[obj];
            if (SaveDatesManager.s.SavedGames.Count > p.indiceDoSave)
                SaveDatesManager.s.SavedGames[p.indiceDoSave] = null;

            lp.Remove(p);
            lp.Sort();

            SaveDatesManager.Save();

            

            if (lp.Count > 0)
                MenuDeCarregarJogo();
            else
            {
                MenuPrincipal();
            }

        }, DeletarNao, "Deseja deletar o arquivo de save "+ lp[obj].nome+"?");
        state = LocalState.confirmationOpened;
    }

    [SerializeField] private string DebugAutoKeyval;

    private void EscolhiQualCarregar(int indice)
    {
        state = LocalState.externalAction;

        PropriedadesDeSave p = SaveDatesManager.s.SaveProps[indice];

        SaveDatesManager.s.SaveProps[indice] = new PropriedadesDeSave()
        {
            ultimaJogada = System.DateTime.Now,
            nome = p.nome,
            indiceDoSave = p.indiceDoSave
        };

        SaveDates S = null;
        if (SaveDatesManager.s.SavedGames.Count>p.indiceDoSave)
            S = SaveDatesManager.s.SavedGames[p.indiceDoSave];

        SaveDatesManager.s.SaveProps.Sort();

        SaveDatesManager.Save();

        containerDeLoads.FinishHud();

        AbstractGlobalController.Instance.FadeV.StartFadeOutWithAction(()=> {
            SceneLoader.IniciarCarregamento(p.indiceDoSave);
        });
        

        
    }

    private void IniciarJogo()
    {

        PropriedadesDeSave prop = new PropriedadesDeSave() { nome = "Jogo Criado: " + System.DateTime.Now, ultimaJogada = System.DateTime.Now };

        List<PropriedadesDeSave> lista = SaveDatesManager.s.SaveProps;

        if (lista != null)
        {
            //int maior = -1;
            //for (int i = 0; i < lista.Count; i++)
            //{
            //    if (lista[i].indiceDoSave > maior)
            //        maior = lista[i].indiceDoSave;
            //}

            prop.indiceDoSave = PropriedadesDeSave.PrimeiroIndiceLivre(lista.ToArray());
            lista.Add(prop);
        }
        else
            lista = new List<PropriedadesDeSave>() { prop };

        if (SaveDatesManager.s.SavedGames.Count > prop.indiceDoSave)
            SaveDatesManager.s.SavedGames[prop.indiceDoSave] = new Criatures2021.CriaturesSaveDates(false);
        else
        {
            Criatures2021.CriaturesSaveDates cs = new Criatures2021.CriaturesSaveDates(false);
            SaveDatesManager.s.SavedGames.Add(cs);
            Debug.Log("O indice pedido para criar foi: "
                + prop.indiceDoSave + " e a criação me deu o indice" + SaveDatesManager.s.SavedGames.IndexOf(cs));
        }




        SaveDatesManager.s.SaveProps = lista;
        SaveDatesManager.Save();

        state = LocalState.externalAction;
        AbstractGlobalController.Instance.FadeV.StartFadeOutWithAction(() => {
            SceneLoader.IniciarCarregamento(prop.indiceDoSave);
            //SceneManager.sceneLoaded += SeraHoraDoFadeIn;
        });
    }

    private void SeraHoraDoFadeIn(Scene arg0, LoadSceneMode arg1)
    {
        if (arg0 == SceneManager.GetSceneByName(NomesCenasEspeciais.CenaDeCarregamento.ToString()))
        {
            SceneManager.sceneLoaded -= SeraHoraDoFadeIn;
            AbstractGlobalController.Instance.FadeV.StartFadeIn();
        }
    }

    // Update is called once per frame
    void Update()
    {
        int changeV = Commander.GetIntTriggerDown(CommandConverterString.menuChangeV);
        int changeH = Commander.GetIntTriggerDown(CommandConverterString.menuChangeH);
        bool select = Commander.GetButtonDown(CommandConverterInt.starterMenuConfirm);
        bool cancel = Commander.GetButtonDown(CommandConverterInt.starterMenuReturn);
        bool deletar = Commander.GetButtonDown(CommandConverterInt.deletarSave);

        switch (state)
        {
            case LocalState.menuAtivo:
                
                if (changeV != 0)
                    menu.ChangeOption(-changeV);
                else if (select)
                    EscolhaDeMenu(menu.SelectedOption);
                
            break;
            case LocalState.saveLoadsAberto:
                if (changeH != 0)
                    containerDeLoads.ChangeOption(changeH);
                else if (cancel)
                {
                    containerDeLoads.FinishHud();
                    MenuPrincipal();
                }
                else if (deletar)
                {
                    EscolhiDelete(containerDeLoads.SelectedOption);
                }
                else if (select)
                {
                    EscolhiQualCarregar(containerDeLoads.SelectedOption);
                }
            break;
            case LocalState.oneMessageOpened:
                AbstractGlobalController.Instance.OneMessage.ThisUpdate(select || cancel);
            break;
            case LocalState.confirmationOpened:
                AbstractGlobalController.Instance.Confirmation.ThisUpdate(changeH!=0, select, cancel);
            break;
        }
        
    }

    private void DeletarNao()
    {
        containerDeLoads.ChangeInteractiveButtons(true);
        state = LocalState.saveLoadsAberto;
    }
}
