using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FayvitBasicTools;

namespace Criatures2021
{
    public abstract class AnyRotateBarrier : BarrierBarEventBase
    {

        protected float velDeGiro = 0;
        protected float velAlvoDoGiro = -400;

        private EstadoDoCatavento estadoC = EstadoDoCatavento.neutro;

        private enum EstadoDoCatavento
        {
            neutro,
            acelerandoGiro,
            acelerado,
            desacelerandoGiro
        }

        protected abstract void EffectiveRotate(float vel);
        

        protected override void AtivacaoEspecifica()
        {

            velDeGiro = Mathf.Lerp(0, velAlvoDoGiro, 1.5f * TempoDecorrido / TempoDeEfetivaAcao);

            EffectiveRotate(velDeGiro);

            base.AtivacaoEspecifica();
        }

        protected override void BarraDescendo()
        {
            EffectiveRotate(velDeGiro);
            base.BarraDescendo();
        }

        protected override void ApresentacaoDeFinalizacaoEspecifica()
        {
            velDeGiro = Mathf.Lerp(velAlvoDoGiro, 0, 1.5f * TempoDecorrido / TempoDoFinalizaAcao);
            EffectiveRotate(velDeGiro);
            
            base.ApresentacaoDeFinalizacaoEspecifica();
        }

        protected override void CaseDoFeito()
        {
            switch (estadoC)
            {
                case EstadoDoCatavento.acelerandoGiro:
                    TempoDecorrido += Time.deltaTime;

                    velDeGiro = Mathf.Lerp(0, velAlvoDoGiro, 1.5f * TempoDecorrido / TempoDeEfetivaAcao);
                    EffectiveRotate(velDeGiro);

                    if (TempoDecorrido > TempoDeEfetivaAcao)
                    {
                        TempoDecorrido = 0;
                        estadoC = EstadoDoCatavento.desacelerandoGiro;
                    }
                    break;
                case EstadoDoCatavento.acelerado:
                    TempoDecorrido += Time.deltaTime;
                    EffectiveRotate(velDeGiro);
                    if (TempoDecorrido > TempoDeEfetivaAcao)
                    {
                        TempoDecorrido = 0;
                        estadoC = EstadoDoCatavento.desacelerandoGiro;
                    }
                    break;
                case EstadoDoCatavento.desacelerandoGiro:
                    velDeGiro = Mathf.Lerp(velAlvoDoGiro, 0, 1.5f * TempoDecorrido / TempoDoFinalizaAcao);
                    EffectiveRotate(velDeGiro);
                    TempoDecorrido += Time.deltaTime;
                    if (TempoDecorrido > TempoDoFinalizaAcao)
                    {
                        estadoC = EstadoDoCatavento.neutro;
                    }

                    break;
            }
        }

        public override void DisparaEvento(AttackNameId nomeDoGolpe)
        {
            if (AbstractGameController.Instance.MyKeys.VerificaAutoShift(Chave))
            {
                TempoDecorrido = 0;
                if (estadoC == EstadoDoCatavento.neutro)
                    estadoC = EstadoDoCatavento.acelerandoGiro;
                else
                    estadoC = EstadoDoCatavento.acelerado;
            }
            else
                base.DisparaEvento(nomeDoGolpe);
        }

    }
}