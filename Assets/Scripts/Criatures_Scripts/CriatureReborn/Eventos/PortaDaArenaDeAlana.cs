using UnityEngine;
using FayvitMessageAgregator;
using FayvitSupportSingleton;
using FayvitBasicTools;
using Criatures2021;
using FayvitCam;

namespace Assets.Scripts.Criatures_Scripts.CriatureReborn.Eventos
{
    public class PortaDaArenaDeAlana : MonoBehaviour
    {
        [SerializeField] private MonoBehaviour observedID;
        [SerializeField] private Transform posCameraDaPorta;
        [SerializeField] private GameObject emissorDeParticulas;
        [SerializeField] private float tempoAbrindoPorta = 2;

        private LocalState state = LocalState.emEspera;
        private Quaternion startQuatern;
        private Quaternion endQuatern;
        private float tempoDecorrido = 0;

        private enum LocalState
        { 
            emEspera,
            mostrandoPortaAbrindo
        }

        // Use this for initialization
        void Start()
        {
            MessageAgregator<MsgDragPuzzleComplete>.AddListener(OnDragPuzzleComplete);
            MessageAgregator<MsgDragPuzzleStartComplete>.AddListener(OnDragPuzzleStartComplete);
        }

        private void OnDestroy()
        {
            MessageAgregator<MsgDragPuzzleComplete>.RemoveListener(OnDragPuzzleComplete);
            MessageAgregator<MsgDragPuzzleStartComplete>.RemoveListener(OnDragPuzzleStartComplete);
        }

        private void OnDragPuzzleStartComplete(MsgDragPuzzleStartComplete obj)
        {
            if (((IIdentifiable)observedID).PublicID == obj.ID)
            { 
                transform.rotation*= Quaternion.Euler(0, 0, -90); 
            }
        }

        private void OnDragPuzzleComplete(MsgDragPuzzleComplete obj)
        {
            if (((IIdentifiable)observedID).PublicID == obj.ID)
            {
                MessageAgregator<MsgRequestSfx>.Publish(new MsgRequestSfx() { sfxId = FayvitSounds.SoundEffectID.paraBau });
                state = LocalState.mostrandoPortaAbrindo;
                tempoDecorrido = 0;
                startQuatern = transform.rotation;
                endQuatern = startQuatern * Quaternion.Euler(0, 0, -90);
                CameraApplicator.cam.OffCamera();
                CameraApplicator.cam.transform.SetPositionAndRotation(posCameraDaPorta.position, posCameraDaPorta.rotation);
                emissorDeParticulas.SetActive(true);
            }
        }

        // Update is called once per frame
        void Update()
        {
            switch (state)
            {
                case LocalState.mostrandoPortaAbrindo:
                    tempoDecorrido += Time.deltaTime;
                    transform.rotation = Quaternion.Lerp(startQuatern, endQuatern, tempoDecorrido / tempoAbrindoPorta);

                    if (tempoDecorrido > tempoAbrindoPorta)
                    {
                        MessageAgregator<MsgRequestSfx>.Publish(new MsgRequestSfx() { sfxId = FayvitSounds.SoundEffectID.ItemImportante });

                        emissorDeParticulas.SetActive(false);
                        state = LocalState.emEspera;
                        SupportSingleton.Instance.InvokeInSeconds(() =>
                        {
                            MessageAgregator<MsgChangeToHero>.Publish(new MsgChangeToHero()
                            {
                                myHero = MyGlobalController.MainCharTransform.gameObject
                            });
                            MessageAgregator<MsgRequestSfx>.Publish(new MsgRequestSfx() { sfxId = FayvitSounds.SoundEffectID.Book1 });
                        }, 2);
                        Destroy(this);
                    }
                break;
            }
        }
    }
}