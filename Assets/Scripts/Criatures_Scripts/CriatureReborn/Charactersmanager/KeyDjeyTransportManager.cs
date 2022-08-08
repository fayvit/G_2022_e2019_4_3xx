using FayvitBasicTools;
using FayvitCam;
using FayvitCommandReader;
using FayvitMessageAgregator;
using FayvitMove;
using FayvitSupportSingleton;
using UnityEngine;

namespace Criatures2021
{
    public class KeyDjeyTransportManager : MonoBehaviour
    {
        [SerializeField] private BasicMove move;
        
        private DamageState damageState;
        private Transform usuario;
        private LocalState state = LocalState.onFree;

        private enum LocalState
        { 
            onFree,
            inDamage
        }

        public static void StartKeyDjeyTransport(Transform usuario,bool masculino)
        {
            GameObject G = ResourcesFolders.GetPet(PetName.KeyDjey);
            G = Instantiate(G,MelhoraInstancia3D.ProcuraPosNoMapa(usuario.position),usuario.rotation);
            ParticulasComSom(usuario);
            //Destroy(G.GetComponent<PetManager>());
            KeyDjeyTransportManager k = G.AddComponent<KeyDjeyTransportManager>();
            k.usuario = usuario;
            usuario.SetParent(G.transform.Find("Armature/Bone"));

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
            damageState = new DamageState(transform);
            move = new BasicMove(new MoveFeatures()
            {
                jumpFeat = new JumpFeatures()
                {
                    fallSpeed = 18,
                    inJumpSpeed = 9,
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

        public ICommandReader GetCommander => CommandReader.GetCR(Controlador.teclado);

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

                    if (GetCommander.GetButtonDown(CommandConverterInt.keyDjeyAction))
                    {
                        ParticulasComSom(usuario);
                        MessageAgregator<MsgExitKeyDjey>.Publish(new MsgExitKeyDjey()
                        {
                            usuario = usuario.gameObject,
                            returnState = CharacterState.onFree
                        });

                        usuario.parent = null;
                        Destroy(gameObject);
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
    }

    public struct MsgExitKeyDjey : IMessageBase
    {
        public GameObject usuario;
        public FayvitBasicTools.CharacterState returnState;
    }
}