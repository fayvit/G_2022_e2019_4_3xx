using FayvitBasicTools;
using FayvitCam;
using FayvitCommandReader;
using FayvitLoadScene;
using FayvitMessageAgregator;
using FayvitMove;
using FayvitSupportSingleton;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Criatures2021
{
    public class KeyDjeyTransportManager : MonoBehaviour
    {
        [SerializeField] private BasicMove move;

        private DamageState damageState;
        private Transform usuario;
        private LocalState state = LocalState.onFree;
        private bool podeVoltarProTtreinador;

        private const float TEMPO_PODE_MUDAR = 1.25f;

        private enum LocalState
        {
            onFree,
            inDamage
        }

        public ICommandReader GetCommander => CommandReader.GetCR(AbstractGlobalController.Instance.Control);
        public Transform GetUser => usuario;

        public static void StartKeyDjeyTransport(Transform usuario, bool masculino)
        {
            GameObject G = ResourcesFolders.GetPet(PetName.KeyDjey);
            G = Instantiate(G, MelhoraInstancia3D.ProcuraPosNoMapa(usuario.position), usuario.rotation);
            G.layer = 0;
            if(SceneManager.GetSceneByName(NomesCenasEspeciais.ComunsDeFase.ToString()).isLoaded)
            SceneManager.MoveGameObjectToScene(G,
             SceneManager.GetSceneByName(
                NomesCenasEspeciais.ComunsDeFase.ToString())
             );

            ParticulasComSom(usuario);

            KeyDjeyTransportManager k = G.AddComponent<KeyDjeyTransportManager>();
            k.usuario = usuario;
            usuario.SetParent(G.transform.Find("Armature/Bone"));
            CameraApplicator.cam.FocusForDirectionalCam(usuario, .4f, 5, 1f);
            SupportSingleton.Instance.InvokeOnEndFrame(() =>
            {
                if (masculino)
                    usuario.localPosition = new Vector3(0, .54f, .85f);
                else
                    usuario.localPosition = new Vector3(0, 0.61f, 0.797f);
            });

        }

        public static void ParticulasComSom(Transform usuario)
        {
            GameObject G = ResourcesFolders.GetGeneralElements(GeneralElements.keydjeyParticle);
            G = Instantiate(G, MelhoraInstancia3D.ProcuraPosNoMapa(usuario.position), usuario.rotation);
            Destroy(G, 5);
            MessageAgregator<MsgRequestSfx>.Publish(new MsgRequestSfx()
            {
                sfxId = FayvitSounds.SoundEffectID.keyDjeySound
            });
        }

        // Start is called before the first frame update
        void Start()
        {
            SupportSingleton.Instance.InvokeInSeconds(() =>
            {
                podeVoltarProTtreinador = true;
            }, TEMPO_PODE_MUDAR);
            damageState = new DamageState(transform);
            move = new BasicMove(new MoveFeatures()
            {
                jumpFeat = new JumpFeatures()
                {
                    fallSpeed = 18,
                    inJumpSpeed = 9,
                    risingSpeed = 7,
                    jumpHeight = 3
                },
                runSpeed = 18,
                walkSpeed = 9
            }, 9);
            move.StartFields(transform);

            MessageAgregator<MsgSlopeSlip>.AddListener(OnSlopeSlip);

        }

        private void OnDestroy()
        {
            MessageAgregator<MsgSlopeSlip>.RemoveListener(OnSlopeSlip);

        }

        private void OnEnterInDamageState(MsgEnterInDamageState obj)
        {
            if (obj.oAtacado == gameObject)
            {
                damageState.StartDamageState(obj.golpe);
                state = LocalState.inDamage;
            }
        }

        private void OnSlopeSlip(MsgSlopeSlip obj)
        {
            if (obj.slipped == gameObject)
            {
                PetAttackBase petAttack = SlopeSlipManager.Slip(gameObject, obj.hit);

                OnEnterInDamageState(new MsgEnterInDamageState()
                {
                    oAtacado = gameObject,
                    golpe = petAttack,
                    atacante = null
                });
            }
        }

        // Update is called once per frame
        void Update()
        {
            switch (state)
            {
                case LocalState.onFree:


                    CameraComands();

                    float h = GetCommander.GetAxis(CommandConverterString.moveH);
                    float v = GetCommander.GetAxis(CommandConverterString.moveV);
                    Vector3 dir = CameraApplicator.cam.SmoothCamDirectionalVector(new Vector3(h, 0, v));
                    move.MoveApplicator(dir, GetCommander.GetButton(CommandConverterInt.run),
                        GetCommander.GetButtonDown(CommandConverterInt.jump),
                        GetCommander.GetButton(CommandConverterInt.jump)
                        );

                    if (GetCommander.GetButtonDown(CommandConverterInt.keyDjeyAction) && podeVoltarProTtreinador)
                    {
                        SairDoKeyDjey();
                    }

                    break;
                case LocalState.inDamage:
                    if (damageState.Update())
                    {
                        state = LocalState.onFree;
                    }
                    break;
            }


        }

        void CameraComands()
        {
            Vector2 V = new Vector3(
                                GetCommander.GetAxis(CommandConverterString.camX),
                                GetCommander.GetAxis(CommandConverterString.camY)
                                );

            bool focar = GetCommander.GetButtonDown(CommandConverterInt.camFocus);
            CameraApplicator.cam.ValoresDeCamera(V.x, V.y, focar, move.Controller.velocity.sqrMagnitude > .1f);

        }

        public void SairDoKeyDjey(
            bool melhorarPosicao = true,
            bool melhorarPosicaoDoPet = true,
            CharacterState returnState = CharacterState.onFree)
        {
            move.MoveApplicator(Vector3.zero);
            CameraApplicator.cam.ValoresDeCamera(0, 0, false, false);

            ParticulasComSom(usuario);

            usuario.parent = null;
            usuario.rotation = DirectionOnThePlane.Rotation(usuario.forward);
            if (melhorarPosicao)
                usuario.position = MelhoraInstancia3D.ProcuraPosNoMapa(transform.position) + Vector3.up;

            if (melhorarPosicaoDoPet)
                MessageAgregator<MsgBlockPetAdvanceInTrigger>.Publish(new MsgBlockPetAdvanceInTrigger()
                {
                    pet = usuario.GetComponent<CharacterManager>().ActivePet.gameObject
                });

            MessageAgregator<MsgExitKeyDjey>.Publish(new MsgExitKeyDjey()
            {
                usuario = usuario.gameObject,
                returnState = returnState
            });


            Destroy(gameObject);

        }

    }

    public struct MsgExitKeyDjey : IMessageBase
    {
        public GameObject usuario;
        public CharacterState returnState;
    }
}