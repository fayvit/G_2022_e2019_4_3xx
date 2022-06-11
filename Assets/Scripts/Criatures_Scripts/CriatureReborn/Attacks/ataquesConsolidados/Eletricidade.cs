using UnityEngine;
using System.Collections.Generic;
using FayvitMessageAgregator;
using FayvitBasicTools;

namespace Criatures2021
{
    [System.Serializable]
    public class Eletricidade : EletricProjectleBase
    {
        [System.NonSerialized] private int raios = 0;

        public Eletricidade() : base(new PetAttackFeatures()
        {
            nome = AttackNameId.eletricidade,
            tipo = PetTypeName.Eletrico,
            carac = AttackDiferentialId.projetil,
            damageAtribute = DamageBaseAtribute.poder,
            custoPE = 1,
            potenciaCorrente = 15,
            potenciaMaxima = 19,
            potenciaMinima = 11,
            //tempoDeReuso = 5,
            tempoDeMoveMax = 1,
            tempoDeMoveMin = 0,
            tempoDeDestroy = 2,
            TempoNoDano = 1.05f,
            velocidadeDeGolpe = 30,
            custoDeStamina = 40,
            somDoGolpe = FayvitSounds.SoundEffectID.Thunder12

        }
            )
        {

        }

        public override void IniciaGolpe(GameObject G)
        {
            base.IniciaGolpe(G);
            raios = 0;

            
        }

        public override void UpdateGolpe(GameObject G,GameObject focado=null)
        {

            tempoDecorrido += Time.deltaTime;
            if (raios < 5 && tempoDecorrido > TempoDeMoveMin + raios * (TempoDeMoveMax - TempoDeMoveMin) / 35)
            {

                float tempinho = TempoDeMoveMax - tempoDecorrido;
                switch (raios)
                {
                    case 0:
                        carac.posInicial += Vector3.up * 0.25f;
                        InstanciaEletricidade(G, G.transform.forward, tempinho);
                        break;
                    case 1:
                        carac.posInicial = G.transform.position + G.transform.right + 5 * Vector3.up;
                        InstanciaEletricidade(G, G.transform.right + Vector3.up + G.transform.forward, tempinho);
                        break;
                    case 2:
                        carac.posInicial = G.transform.position + G.transform.right + Vector3.up;
                        InstanciaEletricidade(G, G.transform.right + Vector3.up + G.transform.forward, tempinho);
                        break;
                    case 3:
                        carac.posInicial = G.transform.position - G.transform.right + 5 * Vector3.up;
                        InstanciaEletricidade(G, -G.transform.right + Vector3.up + G.transform.forward, tempinho);
                        break;
                    case 4:
                        carac.posInicial = G.transform.position - G.transform.right + Vector3.up;
                        InstanciaEletricidade(G, -G.transform.right + Vector3.up + G.transform.forward, tempinho);
                        break;
                }

                raios++;
            }
        }
    }
}