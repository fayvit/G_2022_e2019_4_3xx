using UnityEngine;
using System.Collections;
using FayvitMessageAgregator;
using FayvitBasicTools;

namespace Criatures2021
{
    public class DamageColliderBase : MonoBehaviour
    {

        public float velocidadeProjetil = 6f;
        public GameObject dono;
        public ImpactParticles noImpacto;
        public PetAttackBase esseGolpe;

        protected Quaternion Qparticles;

        protected void FuncaoTrigger(Collider emQ)
        {

            if (emQ.gameObject != dono
               &&
               emQ.tag != "cenario" && emQ.tag != "gatilhoDePuzzle" && (emQ.gameObject.layer != 2||esseGolpe.IgnoreDodge) /*|| velocidadeProjetil > 0*/
               &&
               emQ.tag != "desvieCamera")
            {
                FacaImpacto(emQ.gameObject);

            }


        }

        protected void QuaternionDeImpacto()
        {

            switch (noImpacto)
            {
                case ImpactParticles.impactoComum:
                    Qparticles = Quaternion.LookRotation(dono.transform.forward);
                break;
                default:
                    //Debug.LogError("fazer get prefab de impacto");
                    GameObject impacto = Resources.Load<GameObject>("particles/"+noImpacto.ToString());
                    Qparticles = impacto.transform.rotation;
                break;
            }


        }

        protected void FacaImpacto(GameObject emQ, bool destroiAqui = true, bool noTransform = false)
        {

            if(noTransform)
                InsertImpactView.Insert(noImpacto,transform.position,Qparticles);
            else
                InsertImpactView.Insert(noImpacto, emQ.transform.position, Qparticles);

            //DamageManager.VerificaDano(emQ, dono, esseGolpe);
            DamageManager.VerificaDano(emQ, dono, esseGolpe);

            //Debug.Log("antes de spawnar o somdoimpacto: "+emQ + " : " + esseGolpe.GetNomeEmLinguas);

            MessageAgregator<MsgRequest3dSound>.Publish(new MsgRequest3dSound()
            {
                sender = emQ.transform,
                sfxId = esseGolpe.SomDoImpacto
            });

            if (destroiAqui)
                Destroy(gameObject);

            #region suprimido

            //Debug.LogError("fazer get prefab de impacto");
            //GameObject impacto=null;
            //GameObject impacto = GameController.g.El.retorna(noImpacto);


            //if (!noTransform)
            //    impacto = Instantiate(impacto, transform.position, Qparticles);



            //if (noTransform)
            //    impacto = Instantiate(impacto, emQ.transform.position, Qparticles);


            //if (impacto)
            //    Destroy(impacto, 1.5f);
            #endregion

        }
    }
}