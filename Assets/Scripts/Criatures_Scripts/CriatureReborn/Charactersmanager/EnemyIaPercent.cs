using FayvitMessageAgregator;
using FayvitMove;
using FayvitSupportSingleton;
using UnityEngine;

namespace Criatures2021
{
    [System.Serializable]
    public class EnemyIaPercent : EnemyIaBase
    {
        [SerializeField] private float distanciaTesteparaDesviarDeProjetil = 12;
        [SerializeField] private float tempoBaseDoPuloFuga = .5f;
        [SerializeField, ArrayElementTitle("atkDifId")] private PercentManagerDodge[] percentManagerDodge;

        private float f;
        private Vector3 fDir;
        private Vector3 ddir;
        private PetAttackBase esseAtk;
        private GameObject atacante;
        private Vector3 guardDir;
        private ExtendState extendState = ExtendState.neutral;

        [System.Serializable]
        private struct PercentManagerDodge
        {
            public AttackDiferentialId atkDifId;
            public int addOnHit;
            public int subtractOnAnotherHit;            
            public int dodgeChance;
            public int onDodgeThisSubtract;
            public int onDodgeAllSubtract;
        }

        private enum ExtendState
        { 
            neutral,
            emPulo,
            emMove,
            emMoveDoColisaoComPow
        }

        private bool iniciado;

        public override void Start(Transform T, PetBase P, ControlledMoveForCharacter controll)
        {
            if (!iniciado)
            {
                MessageAgregator<MsgInvokeStartAtk>.AddListener(OnAnyStartAtk);
                MessageAgregator<MsgCriatureDefeated>.AddListener(OnCriatureDefeated);
                MessageAgregator<MsgEnterInDamageState>.AddListener(OnEnterInDamageState);
                iniciado = true;
            }
            base.Start(T, P, controll);
        }

        void OnDestroy()
        {
            MessageAgregator<MsgInvokeStartAtk>.RemoveListener(OnAnyStartAtk);
            MessageAgregator<MsgCriatureDefeated>.RemoveListener(OnCriatureDefeated);
            MessageAgregator<MsgEnterInDamageState>.RemoveListener(OnEnterInDamageState);
        }

        private void OnEnterInDamageState(MsgEnterInDamageState obj)
        {
            if (obj.oAtacado == MeuTransform.gameObject)
            {
                UpdatePercentmanagerDodge(obj.golpe.Caracteristica);
            }
        }

        private void OnCriatureDefeated(MsgCriatureDefeated obj)
        {
            if (obj.doDerrotado == myPet)
            {
                SupportSingleton.Instance.InvokeOnEndFrame(() =>
                {
                    MessageAgregator<MsgInvokeStartAtk>.RemoveListener(OnAnyStartAtk);
                    MessageAgregator<MsgCriatureDefeated>.RemoveListener(OnCriatureDefeated);
                    MessageAgregator<MsgEnterInDamageState>.RemoveListener(OnEnterInDamageState);
                });
            }
        }

        private void OnAnyStartAtk(MsgInvokeStartAtk obj)
        {
            if (obj.atacado == MeuTransform.gameObject && DodgeThisAttack(obj.atk.Caracteristica))
            {

                esseAtk = obj.atk;
                atacante = obj.atacante;

                fDir = (atacante.transform.position - MeuTransform.position).normalized;
                ddir = Vector3.Cross(fDir, Vector3.up);
                f = Vector3.Distance(MeuTransform.position, atacante.transform.position);

                switch (esseAtk.Caracteristica)
                {
                    case AttackDiferentialId.projetil:
                        RespostaAoProjetil();
                    break;
                    case AttackDiferentialId.colisao:
                        RespostAaColisao();
                    break;
                    case AttackDiferentialId.colisaoComPow:
                        RespostaColisaoComPow();
                    break;
                    case AttackDiferentialId.hitNoChao:
                        RespostaAoHitNoChao();
                    break;
                }
            }
        }

        private bool DodgeThisAttack(AttackDiferentialId id)
        {
            float dodgeChance = DodgeChanceForAttackType(id);
            bool retorno = false;
            if (dodgeChance > 0)
            {
                int x = Random.Range(0, 100);
                if (x < dodgeChance)
                    retorno = true;

                VerifyDodgeFactos(id);
                
                Debug.Log("dodgeChange: " + dodgeChance + " sorteado: " + x + " resultado: " + retorno);
            }

            return retorno;
        }

        private void VerifyDodgeFactos(AttackDiferentialId id)
        {
            for (int i = 0; i < percentManagerDodge.Length; i++)
            {
                if (percentManagerDodge[i].atkDifId == id)
                {
                    percentManagerDodge[i].dodgeChance 
                        = Mathf.Max(percentManagerDodge[i].dodgeChance - percentManagerDodge[i].onDodgeThisSubtract, 0);
                }
                else
                    percentManagerDodge[i].dodgeChance 
                        = Mathf.Max(percentManagerDodge[i].dodgeChance - percentManagerDodge[i].onDodgeAllSubtract, 0);
            }
        }

        private float DodgeChanceForAttackType(AttackDiferentialId id)
        {
            PercentManagerDodge per = new PercentManagerDodge();

            foreach (var v in percentManagerDodge)
            {
                if (v.atkDifId == id)
                    per = v;
            }

            return per.dodgeChance;
        }

        private void RespostaColisaoComPow()
        {
            controll.ModificarOndeChegar(MeuTransform.position - 9 * ddir - 15 * fDir);
            extendState = ExtendState.emMoveDoColisaoComPow;
            EnterInAtkResponse();

            SupportSingleton.Instance.InvokeInSeconds(() => { ExitOutAtkResponse(); }, 
                esseAtk.Nome==AttackNameId.sobreVoo?
                1.1f*esseAtk.TempoDeDestroy:
                1.1f*esseAtk.TempoDeMoveMax);
        }

        private void RespostaAoHitNoChao()
        {   
            controll.ModificarOndeChegar(MeuTransform.position + 9 * ddir + 15 * fDir);
            extendState = ExtendState.emMove;
            EnterInAtkResponse();
            SupportSingleton.Instance.InvokeInSeconds(() => { RequisitarPulo(); }, esseAtk.TempoDeMoveMin * .65f);
        }

        private void RequisitarPulo()
        {
            MessageAgregator<MsgRequestAtkResponse>.Publish(new MsgRequestAtkResponse()
            {
                acao = () =>
                {
                    guardDir = ddir;
                    controll.Mov.MoveApplicator(ddir, startJump: true, pressJump: true);
                    extendState = ExtendState.emPulo;
                    EnterInAtkResponse();
                },
                sender = MeuTransform.gameObject
            });
        }

        private void RespostAaColisao()
        {   
            float v = esseAtk.VelocidadeDeGolpe;
            float t = f / v;
            float dmax = 1.075f*(esseAtk.TempoDeMoveMax - esseAtk.TempoDeMoveMin) * v;

            SupportSingleton.Instance.InvokeInSeconds(() =>
            {
                if (f < dmax && t > tempoBaseDoPuloFuga)
                {
                    RequisitarPulo();
                }
                else if(f<dmax)
                {
                    MessageAgregator<MsgRequestCpuRoll>.Publish(new MsgRequestCpuRoll()
                    {
                        dir = (ddir + fDir).normalized,
                        sender = MeuTransform.gameObject
                    });
                }
            }, esseAtk.TempoDeMoveMin);
        }

        void RespostaAoProjetil()
        {
            if (f < distanciaTesteparaDesviarDeProjetil)
            {
                MessageAgregator<MsgRequestCpuRoll>.Publish(new MsgRequestCpuRoll()
                {
                    dir = (ddir + fDir).normalized,
                    sender = MeuTransform.gameObject
                });
            }
            else
            {
                controll.ModificarOndeChegar(MeuTransform.position + 3 * ddir + 5 * fDir);
                extendState = ExtendState.emMove;
                EnterInAtkResponse();
            }

        }

        protected override void UpdateAtkResponse()
        {
            switch (extendState)
            {
                case ExtendState.emMove:
                    if (controll.UpdatePosition())
                    {
                        ExitOutAtkResponse();
                    }
                break;
                case ExtendState.emPulo:
                    controll.Mov.MoveApplicator(guardDir, pressJump: true);
                    if (controll.Mov.IsGrounded && !controll.Mov._JumpM.isJumping)
                    {
                        ExitOutAtkResponse();
                        extendState = ExtendState.neutral;
                    }
                break;
                case ExtendState.emMoveDoColisaoComPow:
                    controll.UpdatePosition();
                    float ff = Vector3.Distance(MeuTransform.position, atacante.transform.position);
                    
                    if (ff< 4.75f)
                    {
                        MessageAgregator<MsgRequestCpuRoll>.Publish(new MsgRequestCpuRoll()
                        {
                            dir = (ddir + fDir).normalized,
                            sender = MeuTransform.gameObject
                        });
                    }
                break;
            }
            
        }


        private void UpdatePercentmanagerDodge(AttackDiferentialId caracteristica)
        {
            for (int i = 0; i < percentManagerDodge.Length; i++)
            {
                if (percentManagerDodge[i].atkDifId == caracteristica)
                {
                    percentManagerDodge[i].dodgeChance 
                        = Mathf.Min(percentManagerDodge[i].dodgeChance+ percentManagerDodge[i].addOnHit,100);
                }
                else
                    percentManagerDodge[i].dodgeChance 
                        = Mathf.Max(percentManagerDodge[i].dodgeChance- percentManagerDodge[i].subtractOnAnotherHit,0);
            }
        }
    }

    public struct MsgRequestCpuRoll : IMessageBase
    {
        public GameObject sender;
        public Vector3 dir;
    }

    public struct MsgRequestAtkResponse : IMessageBase
    {
        public GameObject sender;
        public System.Action acao;
    }
}