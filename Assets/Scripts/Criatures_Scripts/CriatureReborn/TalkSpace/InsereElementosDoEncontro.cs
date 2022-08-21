using FayvitMessageAgregator;
using UnityEngine;

namespace Criatures2021
{
    public class InsereElementosDoEncontro
    {
        static void ColocaTreinadorEmPosicao(CharacterManager manager, Transform trainer,Vector3 basePosition=default)
        {
            
            Debug.Log("quem é o trainer?: " + trainer);
            CharacterController controle = trainer.GetComponent<CharacterController>();
            controle.enabled = false;
            trainer.position = Vector3.up + MelhoraInstancia3D.PosParaDeslocamento(basePosition + 40 * manager.transform.forward, trainer.transform.position);
            Transform aux = manager.ActivePet.transform;

            //FayvitSupportSingleton.SupportSingleton.Instance.InvokeOnCountFrame(() =>
            //{
            controle.enabled = true;
            //},100);

            Vector3 V = DirectionOnThePlane.InTheUp(trainer.position, aux.position + 3 * aux.forward);
            trainer.rotation = Quaternion.LookRotation(V);
        }

        public static void EncontroDeTreinador(CharacterManager manager, Transform trainer,Vector3 basePosition=default)
        {
            if (basePosition == default)
                basePosition = manager.transform.position;

            manager.ContraTreinador = true;

            ColocaTreinadorEmPosicao(manager, trainer,basePosition);
            
            AnimacaoDeEncontro(manager.transform.position);
            AdicionaCilindroEncontro(basePosition);
            //AlternanciaParaCriature(manager);
            ImpedeMovimentoDoCriature(manager.ActivePet);
            AlteraPosDoCriature(manager,basePosition);
            ColocaOHeroiNaPOsicaoDeEncontro(manager,basePosition);

           
        }

        protected static void ColocaOHeroiNaPOsicaoDeEncontro(CharacterManager manager,Vector3 basePosition)
        {
            CharacterController controle = manager.GetComponent<CharacterController>();
            controle.enabled = false;
                
            manager.transform.position = 2*Vector3.up+MelhoraInstancia3D.ProcuraPosNoMapa(
                basePosition - 40f * manager.transform.forward
                );//40f * tHeroi.forward;


            //manager.gameObject.AddComponent<gravidadeGambiarra>();
            controle.enabled = true;
        }

        protected static void AlteraPosDoCriature(CharacterManager manager,Vector3 basePosition)
        {
            Transform X = manager.ActivePet.transform;
            X.GetComponent<CharacterController>().enabled = false;

            X.position = basePosition;//new melhoraPos().novaPos(posHeroi,X.transform.lossyScale.y);
            X.rotation = manager.transform.rotation;

            X.GetComponent<CharacterController>().enabled = true;
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