using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using FayvitUI;
using FayvitCommandReader;
using FayvitMessageAgregator;
using FayvitBasicTools;
using TextBankSpace;

[System.Serializable]
public class CharDbManager
{
    [SerializeField] private BasicMenu charDbMenu;
    [SerializeField] private ConfirmationPanel confirmation;
    [SerializeField] private InputTextManager inputTextManager;

    private int guard;
    private List<CustomizationContainerDates> currentList;
    private LocalState estado = LocalState.emEspera;

    private enum LocalState
    {
        emEspera,
        confirmationOpened,
        listSelectOpened,
        baseMenuOpened,
        inputTextOpened,
        moveCharToList,
        singleMessageOpened
    }

    private ICommandReader CurrentCommander => CommandReader.GetCR(AbstractGlobalController.Instance.Control);
    bool Jolt
    {
        get
        {
            
            if (GameJolt.API.GameJoltAPI.Instance != null)
                if (GameJolt.API.GameJoltAPI.Instance.CurrentUser != null)
                    return true;
            

            return false;
        }
    }

    public void StartFields(ConfirmationPanel c, InputTextManager i)
    {
        confirmation = c;
        inputTextManager = i;
    }

    public void UpdateDb()
    {
        switch (estado)
        {
            case LocalState.confirmationOpened: ConfirmationOpened(); break;
            case LocalState.singleMessageOpened: SingleMessageOpened(); break;
            case LocalState.listSelectOpened: ListSelectOpened(); break;
            case LocalState.baseMenuOpened: CharacterSaveChangesState(); break;
            case LocalState.inputTextOpened: InputTextOpened(); break;
            case LocalState.moveCharToList: MoveCharToListState(); break;
        }
    }

    void MoveCharToListState()
    {
        int change = -CurrentCommander.GetIntTriggerDown(CommandConverterString.menuChangeV);
        if (change != 0)
        {
            charDbMenu.ChangeOption(change);
        }
        else if (CurrentCommander.GetButtonDown(CommandConverterInt.starterMenuConfirm))
        {
            if (Jolt)
                MyJoltSpace.SaveAndLoadInJolt.SaveList(charDbMenu.LabelOfSelectedOption, currentList[guard]);
            else
                ListToSaveCustomizationContainer.Instance.Save(charDbMenu.LabelOfSelectedOption, currentList[guard]);

            charDbMenu.FinishHud();

            StartCharStandardVector(currentList, guard);
        }
    }


    private bool InputTextOpened()
    {
        if (CurrentCommander.GetButtonDown(CommandConverterInt.starterMenuConfirm))
        {
            inputTextManager.ConfirmationAction();
        }
        else if (CurrentCommander.GetButtonDown(CommandConverterInt.starterMenuReturn))
        {
            inputTextManager.BackAction();
        }

        return false;
    }

    private void CharacterSaveChangesState()
    {
        int change = -CurrentCommander.GetIntTriggerDown(CommandConverterString.menuChangeV);
        if (change != 0)
        {
            charDbMenu.ChangeOption(change);
            List<CustomizationContainerDates> lccd = currentList;

            MessageAgregator<MsgRequestUpdateViewChar>.Publish(new MsgRequestUpdateViewChar()
            {
                ccd = lccd[charDbMenu.SelectedOption]
            });

        }
        else if (CurrentCommander.GetButtonDown(CommandConverterInt.starterMenuConfirm))
        {
            MessageAgregator<MsgFinishCharDbManager>.Publish();
            charDbMenu.FinishHud();

        }
        else if (CurrentCommander.GetButtonDown(CommandConverterInt.starterMenuReturn))
        {
            estado = LocalState.confirmationOpened;
            confirmation.StartConfirmationPanel(
                () =>
                {
                    currentList.RemoveAt(charDbMenu.SelectedOption);
                    if (Jolt)
                        MyJoltSpace.SaveAndLoadInJolt.Save();
                    else
                    {
                        ToSaveCustomizationContainer.Instance.SaveLoaded();
                        ListToSaveCustomizationContainer.Instance.SaveLoaded();
                    }

                    charDbMenu.FinishHud();

                    StartCharStandardVector(currentList);
                    //StartCharactersSavedMenu();

                    MessageAgregator<MsgRequestUpdateViewChar>.Publish(new MsgRequestUpdateViewChar()
                    {
                        ccd = currentList[0]
                    });

                    estado = LocalState.baseMenuOpened;
                },
                () => { estado = LocalState.baseMenuOpened; },
                /*"Gostaria de deletar esse personagem do vetor?"*/
                TextBank.RetornaListaDeTextoDoIdioma(TextKey.frasesDoCharDbMenu)[0]
                ,
                hideSelections: true);
        }
        else if (CurrentCommander.GetButtonDown(CommandConverterInt.starterMenuOption1))
        {
            //charDbMenu.FinishHud();
            estado = LocalState.inputTextOpened;

            inputTextManager.StartHud(() =>
            {
                CustomizationContainerDates ccd = currentList[charDbMenu.SelectedOption];
                //ToSaveCustomizationContainer.Instance.ccds[charDbMenu.SelectedOption];

                ccd.Sid = inputTextManager.TextContent;
                if (Jolt)
                {
                    MyJoltSpace.SaveAndLoadInJolt.Save();
                    MyJoltSpace.SaveAndLoadInJolt.SavedaGalera(ccd);
                }
                else
                    ToSaveCustomizationContainer.Instance.SaveLoaded();

                int guard = charDbMenu.SelectedOption;
                charDbMenu.FinishHud();
                StartCharStandardVector(currentList, guard);
                inputTextManager.FinishHud();

                estado = LocalState.baseMenuOpened;

            }, () =>
            {
                //inputTextManager.FinishHud();
                //charDbMenu.FinishHud();
                //StartCharactersSavedMenu();
                estado = LocalState.baseMenuOpened;
                inputTextManager.FinishHud();
            },
                /*"Escolha um nome, que será identificador  ID, para esse personagem"*/
            TextBank.RetornaListaDeTextoDoIdioma(TextKey.frasesDoCharDbMenu)[1]    
            );
        }
        else if (CurrentCommander.GetButtonDown(CommandConverterInt.starterMenuOption2))
        {
            estado = LocalState.confirmationOpened;
            confirmation.StartConfirmationPanel(() =>
            {
                estado = LocalState.inputTextOpened;

                inputTextManager.StartHud(() =>
                {
                    if (Jolt)
                    {
                        MyJoltSpace.SaveAndLoadInJolt.CreateList(inputTextManager.TextContent);
                    }
                    else
                        ListToSaveCustomizationContainer.Instance.CreateList(inputTextManager.TextContent);

                    inputTextManager.FinishHud();

                    estado = LocalState.baseMenuOpened;
                },
                    () => {
                        inputTextManager.FinishHud();
                        estado = LocalState.baseMenuOpened;
                    },
                    TextBank.RetornaListaDeTextoDoIdioma(TextKey.frasesDoCharDbMenu)[2]
                    /*"Escolha um nome para a lista de Personagens"*/);
            }, () => {
                estado = LocalState.baseMenuOpened; },
                TextBank.RetornaListaDeTextoDoIdioma(TextKey.frasesDoCharDbMenu)[3]
            /*"Deseja criar uma lista de personagens salvos?"*/, hideSelections: true);
        }
        else if (CurrentCommander.GetButtonDown(CommandConverterInt.menuOpenList))
        {
            guard = charDbMenu.SelectedOption;
            charDbMenu.FinishHud();

            if (StartListSavedMenu())
            {
                estado = LocalState.listSelectOpened;
            }
            else
            {
                StartCharStandardVector(currentList, guard);
            }
        }
        else if (CurrentCommander.GetButtonDown(CommandConverterInt.menuMoveToList))
        {
            estado = LocalState.confirmationOpened;
            confirmation.StartConfirmationPanel(() => {
                guard = charDbMenu.SelectedOption;
                estado = LocalState.moveCharToList;
                charDbMenu.FinishHud();
                StartListSavedMenu();
            }, () => {
                estado = LocalState.baseMenuOpened;
            },
            TextBank.RetornaListaDeTextoDoIdioma(TextKey.frasesDoCharDbMenu)[4]
            /*"Deseja copiar esse personagem para uma lista de personagens?"*/, hideSelections: true);
        }
    }

    private void SingleMessageOpened()
    {
        AbstractGlobalController.Instance.OneMessage.ThisUpdate(
            CurrentCommander.GetButtonDown(CommandConverterInt.starterMenuConfirm) ||
            CurrentCommander.GetButtonDown(CommandConverterInt.starterMenuReturn)
            );
    }

    private void ConfirmationOpened()
    {
        if (CurrentCommander.GetButtonDown(CommandConverterInt.starterMenuReturn))
        {
            confirmation.BtnNo();
        }
        else if (CurrentCommander.GetButtonDown(CommandConverterInt.starterMenuConfirm))
        {
            confirmation.BtnYes();
        }
    }

    public void StartCharStandardVector(List<CustomizationContainerDates> lccd, int indice = 0)
    {
        
        currentList = lccd;
        string[] ss = new string[lccd.Count];
        for (int i = 0; i < ss.Length; i++)
        {
            ss[i] = lccd[i].Sid;
        }

        charDbMenu.StartHud((int x) => {
            Debug.Log(charDbMenu.SelectedOption + " chardb option: " + x);
            if (charDbMenu.SelectedOption != x)
            {
                MessageAgregator<MsgRequestUpdateViewChar>.Publish(new MsgRequestUpdateViewChar()
                {
                    ccd = lccd[x]
                });
            }

        }, ss, selectIndex: indice);
        estado = LocalState.baseMenuOpened;
    }

    public bool StartListSavedMenu(int indice = 0)
    {
        Dictionary<string, List<CustomizationContainerDates>> dccd = Jolt ?
            MyJoltSpace.SaveDatesForJolt.instance.lccds.dccd :
            ListToSaveCustomizationContainer.Instance.dccd;
        if (dccd.Count > 0)
        {
            string[] ss = dccd.Keys.ToArray();
            charDbMenu.StartHud(ConfirmSelectList, ss, selectIndex: indice);
            return true;
        }
        else return false;


    }

    void ConfirmSelectList(int x=0)
    {
        List<CustomizationContainerDates> lccd = !Jolt ?
                ListToSaveCustomizationContainer.Instance.dccd[charDbMenu.LabelOfSelectedOption] :
                MyJoltSpace.SaveDatesForJolt.instance.lccds.dccd[charDbMenu.LabelOfSelectedOption];

        if (lccd.Count > 0)
        {
            currentList = lccd;
            charDbMenu.FinishHud();
            estado = LocalState.baseMenuOpened;
            StartCharStandardVector(currentList);
            MessageAgregator<MsgRequestUpdateViewChar>.Publish(new MsgRequestUpdateViewChar()
            {
                ccd = currentList[0]
            });
        }
        else
        {
            estado = LocalState.confirmationOpened;
            confirmation.StartConfirmationPanel(() => {
                charDbMenu.FinishHud();
                estado = LocalState.baseMenuOpened;
                StartCharStandardVector(currentList);
            }, () => {
                estado = LocalState.listSelectOpened;
            },
            TextBank.RetornaListaDeTextoDoIdioma(TextKey.frasesDoCharDbMenu)[5]
                /*"Essa lista não tem personagens salvos. Deseja escolher outra lista ou voltar para lista padrão?"*/);
            confirmation.ChangeBtnYesText( TextBank.RetornaListaDeTextoDoIdioma(TextKey.frasesDoCharDbMenu)[6]/*"Voltar para padrão"*/);
            confirmation.ChangeBtnNoText(TextBank.RetornaListaDeTextoDoIdioma(TextKey.frasesDoCharDbMenu)[7]/*"Escolher outra Lista"*/);
        }
    }

    void ListSelectOpened()
    {
        int change = -CurrentCommander.GetIntTriggerDown(CommandConverterString.menuChangeV);
        if (change != 0)
        {
            charDbMenu.ChangeOption(change);
        }
        else if (CurrentCommander.GetButtonDown(CommandConverterInt.starterMenuConfirm))
        {
            

            ConfirmSelectList();
            //cMenu.StartHud(secManager, MainAction, ChangeAction, EscapeAction, activeEditables);
            //charDbMenu.FinishHud();
            //estado = EstadoDoMenu.main;

        }
        else if (CurrentCommander.GetButtonDown(CommandConverterInt.starterMenuReturn))
        {
            estado = LocalState.confirmationOpened;
            confirmation.StartConfirmationPanel(
                () =>
                {
                    if (Jolt)
                    {
                        MyJoltSpace.SaveDatesForJolt.instance.lccds.dccd.Remove(charDbMenu.LabelOfSelectedOption);
                        MyJoltSpace.SaveAndLoadInJolt.Save();
                    }
                    else
                    {
                        ListToSaveCustomizationContainer.Instance.dccd.Remove(charDbMenu.LabelOfSelectedOption);
                        ListToSaveCustomizationContainer.Instance.SaveLoaded();
                    }

                    charDbMenu.FinishHud();

                    StartListSavedMenu();

                    estado = LocalState.listSelectOpened;
                },
                () => { estado = LocalState.listSelectOpened; },
                string.Format(TextBank.RetornaListaDeTextoDoIdioma(TextKey.frasesDoCharDbMenu)[8], charDbMenu.LabelOfSelectedOption)
                /*"Gostaria de deletar a lista <color=yellow>" + charDbMenu.LabelOfSelectedOption + "</color> de personagem?"*/,
                hideSelections: true);
        }
    }

    public void DoubleClickInMenu()
    {
        if (estado == LocalState.baseMenuOpened)
        {
            MessageAgregator<MsgFinishCharDbManager>.Publish();
            charDbMenu.FinishHud();
        }
    }

}

public struct MsgRequestUpdateViewChar : IMessageBase
{
    public CustomizationContainerDates ccd;
}

public struct MsgFinishCharDbManager : IMessageBase { }
