using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FayvitBasicTools;
using FayvitMessageAgregator;
using FayvitSounds;

namespace Criatures2021
{
    public abstract class BarrierEventBase : SimpleBarrierBase//EventoComGolpe
    {
        [SerializeField] private bool usarForwardDoObjeto = false;
        [SerializeField] private float tempoDoFinalizaAcao = 1.75f;
        [SerializeField] private SoundEffectID somDaEfetivacao; 
        
        
        private int numJaRepetidos = 0;
        private int numRepeticoesDoSom = 8;

        protected bool UsarForwardDoObjeto
        {
            get { return usarForwardDoObjeto; }
        }        

        public float TempoDoFinalizaAcao
        {
            get { return tempoDoFinalizaAcao; }
        }

        public int NumJaRepetidos
        {
            get { return numJaRepetidos; }
            set { numJaRepetidos = value; }
        }

        protected void VeririqueSom(float tempoDeEfetivaAcao, SoundEffectID som = default)
        {
            if (som == default)
                som = somDaEfetivacao;

            if (TempoDecorrido > NumJaRepetidos * (tempoDeEfetivaAcao / numRepeticoesDoSom))
            {
                MessageAgregator<MsgRequestSfx>.Publish(new MsgRequestSfx()
                {
                    sfxId = som
                });
                
                NumJaRepetidos++;
            }
        }
    }
}