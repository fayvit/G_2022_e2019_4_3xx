using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Criatures2021Hud;
using TextBankSpace;
using FayvitBasicTools;
using FayvitMessageAgregator;

namespace Criatures2021
{
    public class PushBlockPuzzle : ButtonActivate
    {
        [SerializeField] private Light myLight;
        [SerializeField, ColorUsageAttribute(true, true)] private Color colorBase = new Color(.23f,.71f,.75f);
        [SerializeField,ColorUsageAttribute(true, true)] private Color colorCheck = new Color(.75f, .14f, 0);
        [SerializeField] private float tempoTotalparaMovimentoInicial=.25f;
        [SerializeField] private float tempoTotalEmpurrando = .75f;
        [SerializeField] private float varDir = 1;
        [SerializeField] private float dotNotDir = .25f;

        private float tempoDecorrido = 0;
        private Vector3 startPlayerPosition;
        private Vector3 startPlayerForward;
        private Transform player;
        private Vector3 dir;
        private PushState state = PushState.emEspera;
        private Transform ikLeftHand;
        private Transform ikRightHand;
        private Vector3 startBlockPosition;

        private enum PushState
        {
            emEspera,
            iniciandoEmpurrar,
            empurrando
        }

        public override void SomDoIniciar()
        {
            MessageAgregator<MsgRequestSfx>.Publish(new MsgRequestSfx()
            {
                sfxId = FayvitSounds.SoundEffectID.Book1
            });
        }

        public override void FuncaoDoBotao()
        {
            SomDoIniciar();
            player = MyGlobalController.MainCharTransform;
            Vector3[] direcoes = new Vector3[4] {
            Vector3.forward,
            Vector3.right,
            Vector3.left,
            Vector3.back
            };

            dir = direcoes[0];
            for (int i = 0; i < 4; i++)
                if (Vector3.Dot(dir, player.forward) < Vector3.Dot(direcoes[i], player.forward))
                { 
                    dir = direcoes[i];
                }
            RaycastHit hit;
            Vector3 refPosition = transform.position + 0.1f * Vector3.up;
            if (Physics.Raycast(refPosition, dir, out hit, .7f)
                || Physics.Raycast(refPosition, -dir, out hit, .7f)
                || Vector3.Dot((transform.position - player.position).normalized, dir) < dotNotDir)
            {
                //if (hit.collider)
                //    Debug.Log(hit.collider.gameObject + ": " + dir + Vector3.Dot((transform.position - player.position).normalized, dir));
                //else
                //    Debug.Log(Vector3.Dot((transform.position - player.position).normalized, dir) + " : " + dir);

                bool foi = false;
                if (hit.collider)
                    if (hit.collider.transform == player)
                    {
                        foi = true;
                        StartPush();
                    }

                if (!foi)
                    MessageAgregator<MsgRequestRapidInfo>.Publish(new MsgRequestRapidInfo()
                    {
                        message = TextBank.RetornaFraseDoIdioma(TextKey.blocoBarreiraBloqueado)
                    });
            }
            else
            {
                StartPush();
            }
        }

        void StartPush()
        {
            RaycastHit hit;
            Vector3 refPosition = transform.position + 0.2f * Vector3.up;
            if (Physics.Raycast(refPosition, Vector3.down, out hit, .7f))
            {
                if (hit.collider.GetComponent<CheckableBlock>())
                {
                    MessageAgregator<MsgChangeCheck>.Publish(new MsgChangeCheck()
                    {
                        check = false,
                        checkable = hit.collider.gameObject
                    });
                }
                myLight.color = colorBase;
                GetComponentInChildren<MeshRenderer>().material.SetColor("EmiterColor", colorBase);
            }
            tempoDecorrido = 0;
            startPlayerForward = player.forward;
            startPlayerPosition = player.position;
            MessageAgregator<MsgRequestExternalMoviment>.Publish(new MsgRequestExternalMoviment()
            {
                oMovimentado = player.gameObject
            });
            state = PushState.iniciandoEmpurrar;
        }

        // Start is called before the first frame update
        void Start()
        {
            textoDoBotao = TextBank.RetornaListaDeTextoDoIdioma(TextKey.textoBaseDeAcao)[2];
            SempreEstaNoTrigger();
            startBlockPosition = transform.position;

            MessageAgregator<MsgPushBlockReturnToStartPosition>.AddListener(OnRequestStartPosition);

            myLight.color = colorBase;
            
        }

        private void OnDestroy()
        {
            MessageAgregator<MsgPushBlockReturnToStartPosition>.RemoveListener(OnRequestStartPosition);
        }

        private void OnRequestStartPosition(MsgPushBlockReturnToStartPosition obj)
        {
            if (HierarchyTools.EstaNaHierarquia(obj.sender.transform, transform))
            {
                transform.position = startBlockPosition;

                myLight.color = colorBase;
                GetComponentInChildren<MeshRenderer>().material.SetColor("EmiterColor", colorBase);
            }
        }

        // Update is called once per frame
        new void Update()
        {
            base.Update();
            switch (state)
            {
                case PushState.iniciandoEmpurrar:
                    tempoDecorrido += Time.deltaTime;
                    player.GetComponent<CharacterController>().enabled = false;
                    player.transform.position 
                        = Vector3.Lerp(startPlayerPosition,MelhoraInstancia3D.ProcuraPosNoMapa(transform.position - varDir * dir), 
                        tempoDecorrido / tempoTotalparaMovimentoInicial);
                    player.transform.rotation = Quaternion.Lerp(
                        Quaternion.LookRotation(startPlayerForward),
                        Quaternion.LookRotation(dir),
                        tempoDecorrido / tempoTotalparaMovimentoInicial);

                    if (tempoDecorrido > tempoTotalparaMovimentoInicial)
                    {
                        startPlayerPosition = player.position;
                        startPlayerForward = transform.position;
                        state = PushState.empurrando;
                        
                        tempoDecorrido = 0;

                        if (ikLeftHand == null)
                        {
                            var LeftHand = new GameObject().transform;
                            var RightHand = new GameObject().transform;
                            LeftHand.name = "ikLefthand";
                            RightHand.name = "ikRighthand";
                            ikLeftHand = LeftHand.transform;
                            ikRightHand = RightHand.transform;
                            ikLeftHand.parent = transform;
                            ikRightHand.parent = transform;
                        }

                        ikRightHand.position = transform.position + 0.99f * Vector3.up - .56f * dir-.2f*Vector3.Cross(dir,Vector3.up);
                        ikLeftHand.position = transform.position + 0.99f * Vector3.up - .56f * dir + .2f * Vector3.Cross(dir, Vector3.up);

                        MessageAgregator<MsgStartPushElement>.Publish(new MsgStartPushElement()
                        {
                            quemEstaEmpurrando = player.gameObject,
                            ikLeftHand=ikLeftHand,
                            ikRightHand=ikRightHand
                        });
                        //state = PushState.emEspera;
                        //MessageAgregator<MsgChangeToHero>.Publish(new MsgChangeToHero()
                        //{
                        //    myHero = player.gameObject
                        //});
                        //Debug.Log("esse é o final");
                        //player.GetComponent<CharacterController>().enabled = true;
                    }
                break;
                case PushState.empurrando:
                    tempoDecorrido += Time.deltaTime;
                    player.position = Vector3.Lerp(startPlayerPosition, startPlayerPosition + dir, tempoDecorrido / tempoTotalEmpurrando);
                    transform.position = Vector3.Lerp(startPlayerForward, startPlayerForward + dir, tempoDecorrido / tempoTotalEmpurrando);
                    if (tempoDecorrido > tempoTotalEmpurrando)
                    {
                        state = PushState.emEspera;
                        MessageAgregator<MsgChangeToHero>.Publish(new MsgChangeToHero()
                        {
                            myHero = player.gameObject,
                            blockReturnCam=true
                        });

                        RaycastHit hit;
                        Vector3 refPosition = transform.position + 0.1f * Vector3.up;
                        if (Physics.Raycast(refPosition, Vector3.down, out hit, .7f))
                        {
                            if (hit.collider.GetComponent<CheckableBlock>())
                            {
                                MessageAgregator<MsgRequest3dSound>.Publish(new MsgRequest3dSound()
                                {
                                    sfxId = FayvitSounds.SoundEffectID.painelAbrindo,
                                    sender = transform
                                });
                                MessageAgregator<MsgChangeCheck>.Publish(new MsgChangeCheck()
                                {
                                    check = true,
                                    checkable = hit.collider.gameObject
                                });

                                myLight.color = colorCheck;
                                GetComponentInChildren<MeshRenderer>().material.SetColor("EmiterColor", colorCheck);

                                Debug.Log("Esse é um bloco de check;");
                            }

                            
                        }

                        player.GetComponent<CharacterController>().enabled = true;
                    }
                    break;
            }
        }
    }

    public struct MsgStartPushElement : IMessageBase
    {
        public GameObject quemEstaEmpurrando;
        public Transform ikLeftHand;
        public Transform ikRightHand;
    }
}