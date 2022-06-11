using System.Collections;
using UnityEngine;
using FayvitBasicTools;

namespace TalkSpace
{
    public class IniciarComTrueSeNaoConversado : MonoBehaviour
    {
        [SerializeField] private string ID;
        [SerializeField] private string sKeyChange;
        [SerializeField] private KeyShift shiftKeyChange = KeyShift.sempretrue;

        // Use this for initialization
        void Start()
        {
            if (StaticInstanceExistence<IGameController>.SchelduleExistence(Start, this, () => {
                return AbstractGameController.Instance;
            }))
            {
                KeyVar myKeys= AbstractGameController.Instance.MyKeys;
                if (!myKeys.VerificaAutoShift(ID))
                {
                    myKeys.MudaAutoShift(sKeyChange, true);
                    myKeys.MudaShift(shiftKeyChange, true);
                }

            }
        }

        private void OnValidate()
        {
            BuscadorDeID.Validate(ref ID, this);
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}