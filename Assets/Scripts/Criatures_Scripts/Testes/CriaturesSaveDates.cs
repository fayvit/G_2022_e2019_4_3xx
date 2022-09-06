using FayvitBasicTools;
using System.Collections;
using UnityEngine;
using Unity.Mathematics;
using CustomizationSpace;

namespace Criatures2021
{
    [System.Serializable]
    public class CriaturesSaveDates : FayvitSave.SaveDates
    {
        private float3 pos;
        private float3 forward;
        
        public override Vector3 Posicao =>pos;

        public override Quaternion Rotacao => Quaternion.LookRotation(forward);

        public CustomizationSpace.CustomizationContainerDates Ccd { get; private set; }

        public CriaturesSaveDates(bool setar=true)
        {
            Debug.Log("passei pela chamada de setar savedates");
            if(setar)
                SetarSaveDates();
        }

        protected override void SetarSaveDates()
        {
            VariaveisChave = AbstractGameController.Instance.MyKeys;
            CharacterManager manager = MyGlobalController.MainPlayer;
            Dados = manager.Dados;
            VariaveisChave.SetarCenasAtivas();

            //Debug.Log(Dados + " : " + Dados.DinheiroCaido);

            Vector3 X = manager.transform.position;
            Vector3 R = manager.transform.forward;
            Ccd = manager.Ccd;

            pos = new float3( X.x, X.y, X.z );
            forward = new float3( R.x, R.y, R.z );

            //Debug.Log(X +" : "+ posicao[0]+" : "+posicao[1]+" : "+posicao[2]);
        }
    }
}