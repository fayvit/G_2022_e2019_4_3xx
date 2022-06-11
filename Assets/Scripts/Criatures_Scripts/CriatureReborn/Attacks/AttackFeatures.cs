using UnityEngine;
using System.Collections;
using FayvitSounds;

namespace Criatures2021
{
    [System.Serializable]
    public class PetAttackFeatures 
    {
        public AttackNameId nome = AttackNameId.nulo;
        public PetTypeName tipo = PetTypeName.Normal;
        public AttackDiferentialId carac = AttackDiferentialId.colisao;
        public DamageBaseAtribute damageAtribute = DamageBaseAtribute.poder;
        public bool podeNoAr = false;

        public int potenciaMinima = 1;
        public int potenciaCorrente = 2;
        public int potenciaMaxima = 3;
        public int modCorrente = 0;
        public int custoPE = 0;

        //public float tempoDeReuso = 3.5f;
        //public float modTempoDeReuso = 0;
        public float custoDeStamina = -100;
        public float distanciaDeRepulsao = 55;
        public float velocidadeDeRepulsao = 27;
        public float TempoNoDano = 0.25f;

        public float velocidadeDeGolpe = 18f;
        public float tempoDeMoveMin = 0.25f;
        public float tempoDeMoveMax = 0.5f;
        public float tempoDeDestroy = 1;

        public float colisorScale = 1;

        public SoundEffectID somDoGolpe = SoundEffectID.XP_Swing04;
        public SoundEffectID somDoImpacto = SoundEffectID.XP_Knock04;

    }
}