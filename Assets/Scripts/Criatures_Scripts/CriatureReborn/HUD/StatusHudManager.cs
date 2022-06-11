using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using FayvitMessageAgregator;
using Criatures2021;

namespace Criatures2021Hud
{
    public class StatusHudManager : MonoBehaviour
    {
        [SerializeField] private GameObject container;
        [SerializeField] private Image imgStatus;
        [SerializeField] private Text txtStatus;
        [SerializeField] private ModoHudStatus modo = ModoHudStatus.player;

        private GameObject dono;
        private PetBase doStatus;
        private int indiceDoStatus=0;
        private float contadorDeTempo=0;
        private const float TEMPO_TOTAL_STATUS = 1;

        private enum ModoHudStatus
        {
            enemy,
            player
        }

        // Start is called before the first frame update
        void Start()
        {
            MessageAgregator<MsgSendUpdateStatus>.AddListener(OnUpdateStatus);
            MessageAgregator<MsgSendNewStatus>.AddListener(OnReceiveNewStatus);
            if (modo == ModoHudStatus.player)
            {
                MessageAgregator<MsgChangeToPet>.AddListener(OnChangeToPet);
            }
            else
            {
                MessageAgregator<MsgTargetEnemy>.AddListener(OnTargetEnemy);
            }
        }

        private void OnDestroy()
        {
            MessageAgregator<MsgSendUpdateStatus>.RemoveListener(OnUpdateStatus);
            MessageAgregator<MsgSendNewStatus>.RemoveListener(OnReceiveNewStatus);

            if (modo == ModoHudStatus.player)
            {
                MessageAgregator<MsgChangeToPet>.RemoveListener(OnChangeToPet);
            }
            else
            {
                MessageAgregator<MsgTargetEnemy>.RemoveListener(OnTargetEnemy);
            }
        }

        private void OnReceiveNewStatus(MsgSendNewStatus obj)
        {
            UpdateHudStatus(obj.receiver, obj.S.Dados.Tipo);
        }

        private void OnTargetEnemy(MsgTargetEnemy obj)
        {
            dono = obj.targetEnemy.gameObject;
        }

        private void OnChangeToPet(MsgChangeToPet obj)
        {
            Debug.Log("Change to pet status hud: " + obj.oCriature);
            dono = obj.oCriature;
            PetManager P = dono.GetComponent<PetManager>();
            List<DatesForTemporaryStatus> ld = P.MeuCriatureBase.StatusTemporarios;
            UpdateHudStatus(P,ld.Count>0 ? ld[0].Tipo : StatusType.nulo);
        }

        private void OnUpdateStatus(MsgSendUpdateStatus obj)
        {
            Debug.Log("On update Status: " + obj.receiver+" : "+dono);
            UpdateHudStatus(obj.receiver, obj.S.Dados.Tipo);
        }

        void UpdateHudStatus(PetManager receiver,StatusType tipo)
        {
            if (receiver != null)
                if (receiver.gameObject == dono)
                {
                    doStatus = receiver.MeuCriatureBase;
                    if (doStatus.StatusTemporarios.Count > 0)
                    {
                        container.SetActive(true);
                        imgStatus.sprite = ResourcesFolders.GetMiniStatus(tipo);
                        txtStatus.text = StatusTemporarioBase.NomeEmLinguas(tipo);
                    }
                    else
                        container.SetActive(false);
                }
        }

        // Update is called once per frame

        public void Update()
        {

            if (container.activeSelf)
            {
                if (doStatus.PetFeat.meusAtributos.PV.Corrente <= 0)
                    DesligarPainel();

                int numStatus = doStatus.StatusTemporarios.Count;
                if (numStatus > 1)
                {
                    contadorDeTempo += Time.fixedDeltaTime;
                    if (contadorDeTempo > TEMPO_TOTAL_STATUS / numStatus)
                    {
                        indiceDoStatus = indiceDoStatus + 1 < numStatus ? indiceDoStatus + 1 : 0;
                        contadorDeTempo = 0;
                        ModificaApresentacaoDoStatus();
                    }

                }
                else if (numStatus == 0)
                    DesligarPainel();
            }

        }

        private void ModificaApresentacaoDoStatus()
        {
            StatusType t = doStatus.StatusTemporarios[indiceDoStatus].Tipo;
            imgStatus.sprite = ResourcesFolders.GetMiniStatus(t);
            txtStatus.text = StatusTemporarioBase.NomeEmLinguas(t);
        }

        private void DesligarPainel()
        {
            container.SetActive(false);
        }
    }

    
}