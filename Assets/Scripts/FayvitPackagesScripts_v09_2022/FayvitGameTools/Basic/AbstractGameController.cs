using UnityEngine;
using System.Collections;
using FayvitMessageAgregator;

namespace FayvitBasicTools
{
    public abstract class AbstractGameController : MonoBehaviour,IGameController
    {
        private static AbstractGameController instance;

        public static IGameController Instance => instance;

        public KeyVar MyKeys { get; protected set; } = new KeyVar();

        public GameObject ThisGameObject => gameObject;

        protected virtual void Awake()
        {
            AbstractGameController[] gg = FindObjectsOfType<AbstractGameController>();

            if (gg.Length > 1)
            {
                Destroy(gameObject);
                return;
            }

            transform.parent = null;

            DontDestroyOnLoad(gameObject);

            instance = this;
        }

        // Use this for initialization
        protected virtual void Start()
        {
            MessageAgregator<MsgDestroyWithCheckId>.AddListener(OnRequestDestroyCheckId);
            MessageAgregator<MsgEnemyDefeatedCheck>.AddListener(OnRequestEnemyDestroyCheck);
            MessageAgregator<MsgChangeToTrueEnemyKey>.AddListener(OnRequestChangeToTrueEnemyKey);
            MessageAgregator<MsgDestroyWithShiftCheck>.AddListener(OnRequestDestroyCheckShift);
            MessageAgregator<MsgChangeShiftKey>.AddListener(OnRequestChangeShiftKey);
            MessageAgregator<MsgSumContShift>.AddListener(OnRequestSumContShift);
        }

        protected virtual void OnDestroy()
        {
            MessageAgregator<MsgDestroyWithCheckId>.RemoveListener(OnRequestDestroyCheckId);
            MessageAgregator<MsgEnemyDefeatedCheck>.RemoveListener(OnRequestEnemyDestroyCheck);
            MessageAgregator<MsgChangeToTrueEnemyKey>.RemoveListener(OnRequestChangeToTrueEnemyKey);
            MessageAgregator<MsgDestroyWithShiftCheck>.RemoveListener(OnRequestDestroyCheckShift);
            MessageAgregator<MsgChangeShiftKey>.RemoveListener(OnRequestChangeShiftKey);
            MessageAgregator<MsgSumContShift>.RemoveListener(OnRequestSumContShift);
        }

        private void OnRequestSumContShift(MsgSumContShift obj)
        {
            if (!string.IsNullOrEmpty(obj.sKey))
                MyKeys.SomaAutoCont(obj.sKey, obj.val);
            else if (obj.keyCont != KeyCont.indiceZero)
                MyKeys.SomaCont(obj.keyCont, obj.val);
        }

        private void OnRequestChangeShiftKey(MsgChangeShiftKey obj)
        {
            if (!string.IsNullOrEmpty(obj.sKey))
                MyKeys.MudaAutoShift(obj.sKey, obj.change);
            else if (obj.key != KeyShift.sempreFalso)
                MyKeys.MudaShift(obj.key, obj.change);
        }

        private void OnRequestDestroyCheckShift(MsgDestroyWithShiftCheck obj)
        {
            if (MyKeys.VerificaAutoShift(obj.sKey))
                Destroy(obj.destructible);
        }

        private void OnRequestChangeToTrueEnemyKey(MsgChangeToTrueEnemyKey obj)
        {
            MyKeys.MudaEnemyShift(obj.sKey, true);
        }

        private void OnRequestEnemyDestroyCheck(MsgEnemyDefeatedCheck obj)
        {
            if (MyKeys.VerificaEnemyShift(obj.sKey))
                Destroy(obj.destructible);
        }

        private void OnRequestDestroyCheckId(MsgDestroyWithCheckId obj)
        {
            if (MyKeys.VerificaAutoShift(obj.key))
                Destroy(obj.destructible);
        }

        // Update is called once per frame
        protected virtual void Update()
        {

        }
    }

    public struct MsgDestroyWithCheckId : IMessageBase
    {
        public KeyShift key;
        public GameObject destructible;
    }
    public struct MsgEnemyDefeatedCheck : IMessageBase {
        public string sKey;
        public GameObject destructible;
    }
    public struct MsgChangeToTrueEnemyKey : IMessageBase
    {
        public string sKey;
    }
    public struct MsgDestroyWithShiftCheck : IMessageBase
    {
        public string sKey;
        public GameObject destructible;
    }
    public struct MsgChangeShiftKey : IMessageBase
    {
        public KeyShift key;
        public string sKey;        
        public bool change;
    }
    public struct MsgSumContShift : IMessageBase
    {
        public string sKey;
        public KeyCont keyCont;
        public int val;
    }
}
