using UnityEngine;
using FayvitMessageAgregator;
//using Criatures2021Hud;
using TextBankSpace;
using FayvitSupportSingleton;

namespace Criatures2021
{
    class Envenenado: StatusTemporarioSimplesBase
    {
        private float tempoAcumulado = 0;
        private EstadoDaqui estado = EstadoDaqui.tempoCorrente;
        
        //private ApresentaDerrota apresentaDerrota;

        private enum EstadoDaqui
        {
            tempoCorrente,
            derrotadoAtivo,
            derrotadoInterno,
            morreuEnvenenadoAtivo,
            emEspera
        }

        // Use this for initialization
        public override void Start()
        {
            if (CDoAfetado != null)
            {
                ColocaAParticulaEAdicionaEsseStatus(StatusParticles.particulasEnvenenado.ToString(), CDoAfetado.transform);
            }

        }

        public override void RecolocaParticula()
        {    
            ColocaAParticulaEAdicionaEsseStatus(StatusParticles.particulasEnvenenado.ToString(), CDoAfetado.transform);
        }

        // Update is called once per frame
        public override void Update()
        {
            switch (estado)
            {
                case EstadoDaqui.tempoCorrente:

                    if (PodeContarTempo())
                        tempoAcumulado += Time.deltaTime;

                    if (tempoAcumulado >= Dados.TempoSignificativo && OAfetado.PetFeat.meusAtributos.PV.Corrente > 0)
                    {
                        Debug.Log(CDoAfetado);

                        DamageManager.AplicaCalculoDoDano(OAfetado.PetFeat.meusAtributos, (int)Dados.Quantificador);

                        MessageAgregator<MsgVerifyPoisonDamageMessage>.Publish(new MsgVerifyPoisonDamageMessage()
                        {
                            afetado = CDoAfetado!=null? CDoAfetado.gameObject:null,
                            pDoAfetado = OAfetado,
                            quantificador=Dados.Quantificador
                        });

                        if (CDoAfetado != null)
                        {

                            //DamageManager.EmEstadoDeDano(A, CDoAfetado);
                            DamageManager.InsereEstouEmDano(CDoAfetado.gameObject, new PetAttackBase(new PetAttackFeatures() { }), null);
                            DamageManager.AplicaVisaoDeDano(CDoAfetado, (int)Dados.Quantificador,
                                OAfetado.PetFeat.contraTipos[(int)PetTypeName.Veneno].Mod
                                );
                            DamageManager.VerificaVida(null, CDoAfetado);
                        }


                        


                        VerificaVida();
                        tempoAcumulado = 0;
                    }
                    else if (OAfetado.PetFeat.meusAtributos.PV.Corrente <= 0)
                    {
                        RetiraComponenteStatus();

                        //if (CDoAfetado != null)
                        //    MudaParaEstadoMorto();
                    }
                    break;
                case EstadoDaqui.derrotadoAtivo:
                    //tempoAcumulado += Time.deltaTime;
                    //if (tempoAcumulado > 2 || GameController.g.CommandR.DisparaAcao())
                    //{
                    //    apresentaDerrota = new ApresentaDerrota(GameController.g.Manager, CDoAfetado);
                    //    estado = EstadoDaqui.morreuEnvenenadoAtivo;
                    //}

                    break;
                case EstadoDaqui.morreuEnvenenadoAtivo:
                    //ApresentaDerrota.RetornoDaDerrota R = apresentaDerrota.Update();
                    //if (R != ApresentaDerrota.RetornoDaDerrota.atualizando)
                    //{
                    //    if (R == ApresentaDerrota.RetornoDaDerrota.voltarParaPasseio)
                    //    {
                    //        GameController.g.Manager.AoHeroi();
                    //        RetiraComponenteStatus();
                    //        estado = EstadoDaqui.emEspera;
                    //    }
                    //    else
                    //    if (R == ApresentaDerrota.RetornoDaDerrota.deVoltaAoArmagedom)
                    //    {

                    //    }



                    //}

                break;
            }
        }

        bool PodeContarTempo()
        {
            bool retorno = true;
            if (CDoAfetado != null)
            {
                if (CDoAfetado.State == PetManager.LocalState.stopped
                    ||
                    CDoAfetado.State == PetManager.LocalState.inDamage
                    ||
                    CDoAfetado.State == PetManager.LocalState.defeated
                    ||
                    CDoAfetado.State == PetManager.LocalState.atk
                    )
                    retorno = false;
            }

            return retorno;
        }

        void VerificaVida()
        {

            if (OAfetado.PetFeat.meusAtributos.PV.Corrente <= 0)
            {
                if (CDoAfetado == null)
                {
                    MessageAgregator<MsgRequestRapidInfo>.Publish(new MsgRequestRapidInfo()
                    {
                        message = string.Format(TextBank.RetornaListaDeTextoDoIdioma(TextKey.status)[0],
                                        OAfetado.GetNomeEmLinguas)
                    });
                    estado = EstadoDaqui.derrotadoInterno;
                }
                else
                {
                    MessageAgregator<MsgWhoIsTheLoserPet>.AddListener(OnDecidWhoLoser);
                    MessageAgregator<MsgVerifyPoisonDefeatedPet>.Publish(new MsgVerifyPoisonDefeatedPet()
                    {
                        afetado = CDoAfetado,
                        pDoAfetado = OAfetado
                    });

                }

                

                //GameController.g.HudM.Painel.AtivarNovaMens(
                //                string.Format(BancoDeTextos.RetornaListaDeTextoDoIdioma(ChaveDeTexto.status)[0],
                //                OAfetado.NomeEmLinguas), 20, 5
                //                );

            }
            
        }

        private void OnDecidWhoLoser(MsgWhoIsTheLoserPet obj)
        {
            if (obj.player)
            {
                estado = EstadoDaqui.derrotadoAtivo;
            }
            else
            {
                estado = EstadoDaqui.emEspera;
            }

            SupportSingleton.Instance.InvokeOnEndFrame(() =>
            {
                MessageAgregator<MsgWhoIsTheLoserPet>.RemoveListener(OnDecidWhoLoser);
            });
        }

        //void MudaParaEstadoMorto()
        //{
        //    CDoAfetado.GetComponent<Animator>().SetBool("cair", true);
        //    CDoAfetado.State = PetManager.LocalState.defeated;
        //}
    }

}
