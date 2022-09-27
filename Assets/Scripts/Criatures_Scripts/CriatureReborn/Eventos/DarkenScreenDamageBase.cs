using UnityEngine;
using Criatures2021;
using FayvitMessageAgregator;
using FayvitBasicTools;
using FayvitSupportSingleton;

public class DarkenScreenDamageBase : MonoBehaviour
{
    [SerializeField] private ButtonsForTutoDodge monitoredID;
    [SerializeField] private ImpactParticles impactParticle = ImpactParticles.impactoEletrico;
    [SerializeField] private float darkenTime = 1.25f;
    [SerializeField] private float lightenTime = .75f;
    [SerializeField] private float timeInBlack = .5f;
    [SerializeField] private DarkenEndEvent checkableEndEvent = DarkenEndEvent.voltaAoFree;
    [SerializeField] private DarkenEndEvent uncheckableEndEvent = DarkenEndEvent.voltaAoFree;
    [SerializeField] private Vector3 dirRefRepulsao = Vector3.forward;

    [SerializeField]
    private PetAttackBase petAttack = new PetAttackBase(new PetAttackFeatures()
    {
        potenciaCorrente = 0,
        potenciaMaxima = 0,
        potenciaMinima = 0,
        TempoNoDano = 0.5f,
        distanciaDeRepulsao = 65f,
        velocidadeDeRepulsao = 33,
        tempoDeMoveMin = 0.45f,//74
        tempoDeMoveMax = 0.85f,
        tempoDeDestroy = 1.1f,
        velocidadeDeGolpe = 28,
    });

    protected Transform Moved { get; private set; }

    // Start is called before the first frame update
    void Start()
    {

    }

    private void OnDestroy()
    {

    }

    protected virtual void Posicione()
    { }

    private void OnFadeOutComplete(FadeOutComplete obj)
    {
        Posicione();
        SupportSingleton.Instance.InvokeInSeconds(() =>
        {
            Posicione();
            MessageAgregator<MsgRequestFadeIn>.Publish(new MsgRequestFadeIn()
            {
                lightenTime = lightenTime
            });
            MessageAgregator<FadeInComplete>.AddListener(OnFadeInComplete);
        }, timeInBlack);
        SupportSingleton.Instance.InvokeOnEndFrame(() =>
        {
            Posicione();
            MessageAgregator<FadeOutComplete>.RemoveListener(OnFadeOutComplete);
        });
    }

    private void OnFadeInComplete(FadeInComplete obj)
    {
        iniciou = false;
        DarkenEndEvent d = uncheckableEndEvent;
        if (!monitoredID || monitoredID.CheckedID)
            d = checkableEndEvent;

        DarkenEndEventFactory.Publish(d, Moved.gameObject);
         
        SupportSingleton.Instance.InvokeOnEndFrame(() =>
        {
            MessageAgregator<FadeInComplete>.RemoveListener(OnFadeInComplete);
        });
    }

    // Update is called once per frame
    void Update()
    {

    }

    void ComunsDePersonagem(Transform T)
    {

        InsertImpactView.Insert(impactParticle, T.position, Quaternion.identity);
        MessageAgregator<MsgRequest3dSound>.Publish(new MsgRequest3dSound()
        {
            sfxId = petAttack.SomDoImpacto,
            sender = transform
        });
        MessageAgregator<MsgRequestFadeOut>.Publish(new MsgRequestFadeOut()
        {
            darkenTime = darkenTime
        });
        petAttack.DirDeREpulsao = Vector3.Dot(T.forward, dirRefRepulsao.normalized) > 0 ? -dirRefRepulsao : dirRefRepulsao;
        MessageAgregator<FadeOutComplete>.AddListener(OnFadeOutComplete);
    }

    bool iniciou;

    private void OnTriggerStay(Collider other)
    {
        if (!iniciou)
            OnTriggerEnter(other);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !MyGlobalController.MainPlayer.ContraTreinador)
        {
            iniciou = true;
            Moved = other.transform;
            ComunsDePersonagem(other.transform);

            MessageAgregator<MsgRequestHumanDamage>.Publish(new MsgRequestHumanDamage()
            {
                esseGolpe = petAttack,
                gameObject = other.gameObject
            });
        }
        else if (other.CompareTag("Criature") && other.gameObject.layer != 2)
        {
            PetManagerCharacter P = other.GetComponent<PetManagerCharacter>();
            if (P)
            {
                iniciou = true;
                Moved = P.T_Dono.transform;
                ComunsDePersonagem(other.transform);

                MessageAgregator<MsgRequestNonReturnableDamage>.Publish(new MsgRequestNonReturnableDamage()
                {
                    gameObject = other.gameObject,
                    petAttack = petAttack
                });
            }
            else {
                KeyDjeyTransportManager k = other.GetComponent<KeyDjeyTransportManager>();
                if (k)
                {
                    k.SairDoKeyDjey(false,false);

                    iniciou = true;
                    Moved = k.GetUser;
                    ComunsDePersonagem(k.GetUser);

                    MessageAgregator<MsgRequestHumanDamage>.Publish(new MsgRequestHumanDamage()
                    {
                        esseGolpe = petAttack,
                        gameObject = k.GetUser.gameObject
                    });

                    SetHeroCamera.Set(k.GetUser);
                }
                
            }
        }
    }
}


    public enum DarkenEndEvent
    {
        explicandoDodgeTuto,
        voltaAoFree
    }

    public static class DarkenEndEventFactory
    {
        public static void Publish(DarkenEndEvent eventType, GameObject heroGameObject)
        {
            switch (eventType)
            {
                case DarkenEndEvent.explicandoDodgeTuto:
                    Debug.Log("publicou o evento dodge info");
                    MessageAgregator<MsgRequestDodgeInfoEvent>.Publish(new MsgRequestDodgeInfoEvent() { heroGameObject = heroGameObject });
                    break;
                case DarkenEndEvent.voltaAoFree:
                    MessageAgregator<MsgChangeToHero>.Publish(new MsgChangeToHero()
                    {
                        myHero = heroGameObject
                    });
                    break;
            }
        }
    }

    public struct MsgRequestDodgeInfoEvent : IMessageBase { public GameObject heroGameObject; }



