using UnityEngine;
using System.Collections;

namespace Criatures2021
{
    public class DamageState {

        private float tempoDeDano = 0;
        private float alturaAtual;
        private float alturaDoDano;
        private Vector3 direcao = Vector3.zero;
        private Vector3 vMove = Vector3.zero;
        private Vector3 posInicial;


        private CharacterController controle;

        public PetAttackBase esseGolpe;

        public DamageState(Transform T)
        {
            controle = T.GetComponent<CharacterController>();
        }

        public void StartDamageState(PetAttackBase golpe)
        {
            esseGolpe = golpe;
            posInicial = controle.transform.position;
            alturaDoDano = controle.transform.position.y;
            tempoDeDano = 0;
        }

        public bool Update()
        {
            tempoDeDano += Time.deltaTime;

            alturaAtual = controle.transform.position.y;
            direcao = Vector3.zero;
            if (alturaAtual < alturaDoDano + 0.5f)
            {
                direcao += 12 * Vector3.up;
            }
            if ((controle.transform.position - posInicial).sqrMagnitude < esseGolpe.DistanciaDeRepulsao)
                direcao += esseGolpe.VelocidadeDeRepulsao * esseGolpe.DirDeREpulsao;//direcaoDoDano;

            vMove = Vector3.Lerp(vMove, direcao, 10 * Time.deltaTime);
            controle.Move(vMove * Time.deltaTime);

            if (tempoDeDano > esseGolpe.TempoNoDano)
            {
                return true;
                //gerente.LiberaMovimento(CreatureManager.CreatureState.emDano);
                //animator.SetBool("dano1", false);
                //Destroy(this);
            }

            return false;
        }
    }
}