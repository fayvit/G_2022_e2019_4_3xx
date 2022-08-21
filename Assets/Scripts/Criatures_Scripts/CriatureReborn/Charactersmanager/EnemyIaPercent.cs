using FayvitMessageAgregator;
using FayvitMove;
using UnityEngine;

namespace Criatures2021
{
    [System.Serializable]
    public class EnemyIaPercent : EnemyIaBase
    {
        [SerializeField] private float distanciaTesteparaDesviarDeProjetil = 12;

        private PetAttackBase esseAtk;
        private GameObject atacante;
        public override void Start(Transform T, PetBase P, ControlledMoveForCharacter controll)
        {
            MessageAgregator<MsgInvokeStartAtk>.AddListener(OnAnyStartAtk);
            base.Start(T, P, controll);
        }

        private void OnAnyStartAtk(MsgInvokeStartAtk obj)
        {
            if (obj.atacado == MeuTransform.gameObject)
            {
                esseAtk = obj.atk;
                atacante = obj.atacante;

                switch (esseAtk.Caracteristica)
                {
                    case AttackDiferentialId.projetil:
                        RespostaAoProjetil();
                    break;
                    case AttackDiferentialId.colisao:
                        RespostAaColisao();
                    break;
                }
            }
        }

        private void RespostAaColisao()
        {
            
        }

        void RespostaAoProjetil()
        {
            float f = Vector3.Distance(MeuTransform.position,atacante.transform.position);
            Vector3 fDir = (atacante.transform.position - MeuTransform.position).normalized;
            Vector3 ddir = Vector3.Cross(fDir, Vector3.up);
            if (f < distanciaTesteparaDesviarDeProjetil)
            {
                
                MessageAgregator<MsgRequestCpuRoll>.Publish(new MsgRequestCpuRoll()
                {
                    dir = (ddir+fDir).normalized,
                    sender = MeuTransform.gameObject
                });
            }
            else
            {
                EnterInAtkResponse();
                controll.ModificarOndeChegar(MeuTransform.position + 3 * ddir+5*fDir);
                
                
            }
        }

        protected override void UpdateAtkResponse()
        {
            if (controll.UpdatePosition())
            {
                
                ExitOutAtkResponse();
            }
        }
    }

    public struct MsgRequestCpuRoll : IMessageBase
    {
        public GameObject sender;
        public Vector3 dir;
    }
}