using FayvitMessageAgregator;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Criatures2021
{
    public class MultiplyedBehaviour : DamageColliderBase
    {

        public Transform alvo;
        public Vector3 direcaoMovimento;
        public float tempoDestroy = 10;

        private bool alvoFoiNaoNulo = false;
        private CharacterController controle;
        private Animator animator;

        private float tempoAcumulado = 0;

        public PetManager Pet { get; set; }


        // Use this for initialization
        void Start()
        {
            controle = GetComponent<CharacterController>();
            animator = GetComponent<Animator>();

            //if (dono.name == "CriatureAtivo")
            //{
            //    C = GameController.g.Manager.CriatureAtivo;

            //}
            //else
            //{
            //    C = GameController.g.InimigoAtivo;

            //}
            noImpacto = ImpactParticles.impactoDeGosma;
            //MessageAgregator<MsgStartWaitToPetChange>.AddListener(OnWaitPetChange);
            //MessageAgregator<MsgEndWaitToPetChange>.AddListener(OnEndWaitPetChange);
        }

        //private void OnDestroy()
        //{
        //    MessageAgregator<MsgStartWaitToPetChange>.RemoveListener(OnWaitPetChange);
        //    MessageAgregator<MsgEndWaitToPetChange>.RemoveListener(OnEndWaitPetChange);
        //}

        //private void OnEndWaitPetChange(MsgEndWaitToPetChange obj)
        //{
        //    if (obj.waiter == Pet.gameObject)
        //    {
        //        externalPause = false;
        //    }
        //}

        //private void OnWaitPetChange(MsgStartWaitToPetChange obj)
        //{
        //    if (obj.waiter == Pet.gameObject)
        //    {
        //        externalPause = true;
        //    }
        //}

        bool PodeAtualizar()
        {
            return
                Pet.State != PetManager.LocalState.defeated &&
                Pet.State != PetManager.LocalState.stopped
            ;
        }

        // Update is called once per frame
        void Update()
        {

           
            if (PodeAtualizar())
            {

                tempoAcumulado += Time.deltaTime;

                if (!alvo)
                    direcaoMovimento = transform.forward;
                else
                {
                    alvoFoiNaoNulo = true;
                    if (Vector3.Distance(transform.position, alvo.position) > 2.5f)
                    {
                        direcaoMovimento = Vector3.Slerp(direcaoMovimento, alvo.position - transform.position, 0.9f * Time.deltaTime);
                        direcaoMovimento.Normalize();
                    }
                    else
                        direcaoMovimento = (alvo.position - transform.position).normalized;
                }

                direcaoMovimento = Vector3.ProjectOnPlane(direcaoMovimento, Vector3.up).normalized;

                controle.Move((direcaoMovimento * velocidadeProjetil+12*Vector3.down) * Time.deltaTime);

                animator.SetFloat("velocidade", controle.velocity.magnitude);

                transform.rotation = Quaternion.Lerp(transform.rotation,
                                                     Quaternion.LookRotation(direcaoMovimento),
                                                     Time.deltaTime * 5);
            }
            if (Pet.MeuCriatureBase.PetFeat.meusAtributos.PV.Corrente <= 0 || tempoAcumulado > tempoDestroy
                || (alvo == null && alvoFoiNaoNulo))
                meDestrua();
        }

        void meDestrua()
        {            
           InsertImpactView.Insert(noImpacto, transform.position + Vector3.up, Quaternion.LookRotation(transform.forward));               

            Destroy(gameObject, 2.1f);
            gameObject.SetActive(false);
        }

        void OnTriggerEnter(Collider hit)
        {

            if (hit.gameObject.tag != "cenario" && hit.gameObject.layer != 10 && hit.gameObject != dono)
            {
                //print(hit.gameObject.name+" hit do multiplicado");
                Qparticles = Quaternion.LookRotation(transform.forward);
                FuncaoTrigger(hit);
                meDestrua();
            }

        }

    }

}