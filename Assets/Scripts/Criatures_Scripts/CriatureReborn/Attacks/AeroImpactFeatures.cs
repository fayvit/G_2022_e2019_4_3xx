using FayvitSounds;
using UnityEngine;
namespace Criatures2021
{

    [System.Serializable]
    public struct AeroImpactFeatures
    {
        public ImpactParticles noImpacto;
        public AttacksTrails trail;
        public ToutchingFloor toque;
        public PrepareJump prepara;
        public FinalAeroImpact final;
        public SoundEffectID onPrepareSound;
        public SoundEffectID onTouchGroundSound;
        public bool parentearNoOsso;

        public AeroImpactFeatures(
            ImpactParticles noImpacto,
            AttacksTrails trail,
            ToutchingFloor toque,
            PrepareJump prepara,
            FinalAeroImpact final,
            SoundEffectID onPrepareSound = SoundEffectID.rajadaDeAgua,
            SoundEffectID onTouchGroundSound = SoundEffectID.rajadaDeAgua,
            bool parentearNoOsso = true
            )
        {
            this.noImpacto = noImpacto;
            this.trail = trail;
            this.toque = toque;
            this.prepara = prepara;
            this.final = final;
            this.parentearNoOsso = parentearNoOsso;
            this.onPrepareSound = onPrepareSound;
            this.onTouchGroundSound = onTouchGroundSound;
        }

        public ImpactFeatures deImpacto
        {
            get
            {
                return new ImpactFeatures()
                {
                    noImpacto = this.noImpacto,
                    nomeTrail = this.trail,
                    parentearNoOsso = this.parentearNoOsso
                };
            }
        }
    }

    public enum ToutchingFloor
    {
        nulo,
        impactoAoChao,
        impactoDePedraAoChao,
        aguaAoChao,
        impactoDeFogo,
        eletricidadeAoChao,
        poeiraAoVento
    }

    public enum PrepareJump
    {
        nulo,
        preparaImpactoAoChao,
        impactoBaixo,
        impactoBaixoDeFolhas,
        preparaImpactoDeAguaAoChao,
        eletricidadeAoChao,
        preparaImpactoDeFogoAoChao,
        impulsoVenenoso
    }

    public enum FinalAeroImpact
    {
        AvanceEPareAbaixo,
        MaisAltoQueOAlvo
    }
}