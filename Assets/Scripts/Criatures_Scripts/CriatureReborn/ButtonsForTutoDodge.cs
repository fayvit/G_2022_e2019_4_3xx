using FayvitBasicTools;
using FayvitMessageAgregator;
using FayvitSounds;
using FayvitSupportSingleton;
using UnityEngine;

namespace Criatures2021
{
    public class ButtonsForTutoDodge:MonoBehaviour
    {
        [SerializeField] private Transform hierarchyVerifyParent;
        [SerializeField] private GameObject particulaDaValidacao;
        [SerializeField] private SoundEffectID somdaValidacao = SoundEffectID.XP_Earth02;
        [SerializeField] private SoundEffectID vinhetaDaValidacao = SoundEffectID.vinhetinhaMedia;
        [SerializeField] private string chaveDaqui;

        private bool foi;

        public bool CheckedID => AbstractGameController.Instance.MyKeys.VerificaAutoShift(chaveDaqui);

        private void Start()
        {
            if (StaticInstanceExistence<IGameController>.SchelduleExistence(Start, this, () =>
            {
                return AbstractGameController.Instance;
            }))
            {
                if (CheckedID)
                {
                    
                    FazerValidacao();
                }
            }
        }

        private void OnValidate()
        {
            BuscadorDeID.Validate(ref chaveDaqui, this, "chaveDaqui");
        }

        void FazerValidacao()
        {
            foi = true;
            var v = FindObjectsOfType<DarkenScreenDamageManager>();
            foreach (var vv in v)
            {
                if (HierarchyTools.EstaNaHierarquia(hierarchyVerifyParent, vv.transform)&&vv.gameObject.layer!=4)
                            vv.gameObject.SetActive(false);
            }
        }
        //private void OnCollisionEnter(Collision collision)
        //{
        //    Collider other = collision.collider;
        //    //}
            private void OnTriggerEnter(Collider other)
            {
                if ((other.CompareTag("Criature") || other.CompareTag("Player"))&&!foi)
            {
                particulaDaValidacao.SetActive(true);
                MessageAgregator<MsgRequestSfx>.Publish(new MsgRequestSfx()
                {
                    sfxId = somdaValidacao
                });
                SupportSingleton.Instance.InvokeInSeconds(() =>
                {
                    MessageAgregator<MsgRequestSfx>.Publish(new MsgRequestSfx()
                    {
                        sfxId = vinhetaDaValidacao
                    });
                }, 1);
                MessageAgregator<MsgChangeShiftKey>.Publish(new MsgChangeShiftKey()
                {
                    change = true,
                    sKey = chaveDaqui
                });
                FazerValidacao();   
            }
        }
    }
}
