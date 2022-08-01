using FayvitMessageAgregator;
using UnityEngine;

namespace Criatures2021
{
    public class InsereElementosDoEncontro
    {
        static void ColocaTreinadorEmPosicao(CharacterManager manager, Transform trainer)
        {
            Debug.Log("quem é o trainer?: " + trainer);
            CharacterController controle = trainer.GetComponent<CharacterController>();
            controle.enabled = false;
            trainer.position = Vector3.up + MelhoraInstancia3D.PosParaDeslocamento(trainer.transform.position + 40 * manager.transform.forward, trainer.transform.position);
            Transform aux = manager.ActivePet.transform;

            //FayvitSupportSingleton.SupportSingleton.Instance.InvokeOnCountFrame(() =>
            //{
            controle.enabled = true;
            //},100);

            Vector3 V = DirectionOnThePlane.InTheUp(trainer.position, aux.position + 3 * aux.forward);
            trainer.rotation = Quaternion.LookRotation(V);
        }

        public static void EncontroDeTreinador(CharacterManager manager, Transform trainer)
        {

            manager.ContraTreinador = true;

            ColocaTreinadorEmPosicao(manager, trainer);
            
            AnimacaoDeEncontro(manager.transform.position);
            AdicionaCilindroEncontro(manager.transform.position);
            //AlternanciaParaCriature(manager);
            ImpedeMovimentoDoCriature(manager.ActivePet);
            AlteraPosDoCriature(manager);
            ColocaOHeroiNaPOsicaoDeEncontro(manager);

           
        }

        protected static void ColocaOHeroiNaPOsicaoDeEncontro(CharacterManager manager)
        {
            CharacterController controle = manager.GetComponent<CharacterController>();
            controle.enabled = false;
                
            manager.transform.position = 2*Vector3.up+MelhoraInstancia3D.ProcuraPosNoMapa(
                manager.transform.position - 40f * manager.transform.forward
                );//40f * tHeroi.forward;


            //manager.gameObject.AddComponent<gravidadeGambiarra>();
            controle.enabled = true;
        }

        protected static void AlteraPosDoCriature(CharacterManager manager)
        {
            Transform X = manager.ActivePet.transform;

            X.position = manager.transform.position;//new melhoraPos().novaPos(posHeroi,X.transform.lossyScale.y);
            X.rotation = manager.transform.rotation;
        }

        protected static void AnimacaoDeEncontro(Vector3 posHeroi)
        {
            //heroi.emLuta = true;
            GameObject anima = ResourcesFolders.GetGeneralElements(GeneralParticles.encontro);

            MonoBehaviour.Destroy(MonoBehaviour.Instantiate(anima, posHeroi, Quaternion.identity), 2);
            MessageAgregator<FayvitBasicTools.MsgRequestSfx>.Publish(new FayvitBasicTools.MsgRequestSfx()
            {
                sfxId = FayvitSounds.SoundEffectID.encontro
            });
        }
        protected static void AdicionaCilindroEncontro(Vector3 posHeroi)
        {
            GameObject cilindro = ResourcesFolders.GetGeneralElements(GeneralElements.cilindroEncontro);
            Object cilindro2 = MonoBehaviour.Instantiate(cilindro, posHeroi, Quaternion.identity);
            cilindro2.name = "cilindroEncontro";
        }

        protected static void AlternanciaParaCriature(CharacterManager manager)
        {
            MessageAgregator<MsgRequestChangeToPetByReplace>.Publish(new MsgRequestChangeToPetByReplace()
            {
                dono = manager.gameObject,
                fluxo = FluxoDeRetorno.criature
            });
        }

        protected static void ImpedeMovimentoDoCriature(PetManager C)
        {

            C.PararCriatureNoLocal();
        }
    }
}