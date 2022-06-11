using System.Collections.Generic;
using UnityEngine;

namespace Criatures2021
{
    class VerifyApplyPoisonStatus
    {
        public static void Verify(PetBase atacante, PetManager cDoAtacado, PetAttackBase golpe, int dano)
        {
            PetBase atacado = cDoAtacado.MeuCriatureBase;
            if (atacado.PetFeat.contraTipos.Length > 0)// os multiplicados fazem esse condicional mas não tem o contratipos
            {
                if (VaiColocarStatus(
                    golpe,
                    atacante.PetFeat.meusAtributos,
                    atacado.PetFeat.meusAtributos,
                    atacado.PetFeat.contraTipos[(int)PetTypeName.Veneno].Mod
                    ))
                {

                    Debug.Log("Aplicou Envenenamento");

                    InsereStatus(cDoAtacado,
                        new DatesForTemporaryStatus()
                        {
                            Quantificador = dano,
                            TempoSignificativo = 50,
                            Tipo = StatusType.envenenado
                        }
                        );
                }
            }
        }
        public static bool VaiColocarStatus(PetAttackBase ativa, PetAtributes bateu, PetAtributes levou, float contraTipoVeneno)
        {
            bool retorno = false;
            switch (ativa.Nome)
            {
                case AttackNameId.agulhaVenenosa:
                case AttackNameId.ondaVenenosa:
                case AttackNameId.chuvaVenenosa:

                    if (contraTipoVeneno > 0)
                    {
                        
                        float ff = ativa.PotenciaCorrente *
                            Mathf.Max(1,
                                      Random.Range(0.75f, 1f) * bateu.Poder.Corrente -
                                      Random.Range(0, 0.75f) * levou.Defesa.Corrente);
                        int range = Random.Range(0, 500);
                        float supraCont = contraTipoVeneno * ff + range;
                        Debug.Log("contA DO VENENO=>POTENCIA ATIVA-> "+ativa.PotenciaCorrente 
                            + " : pelo aleatorio-> " + ff + " : verdadeira conta->  " 
                            + supraCont + " ; aleatorio da verdadeira-> " + range);

                        if (supraCont > 400)
                            retorno = true;

                        
                    }

                    break;
            }

            return retorno;
        }

        public static void InsereStatus(PetManager levou, DatesForTemporaryStatus dadosDoStatus)
        {
            InsereStatus(levou, levou.MeuCriatureBase, dadosDoStatus);
        }

        /*
        public static void InserindoNovoStatus(CreatureManager levou, CriatureBase C, DatesForTemporaryStatus dadosDoStatus,bool eLoad = false)
        {
            C.StatusTemporarios.Add(dadosDoStatus);



            if (levou != null)
            {

                if (levou.name == "CriatureAtivo")
                {
                    GameController.g.ContStatus.AdicionaStatusAoHeroi(S);
                }
                else
                {
                    GameController.g.ContStatus.AdicionaStatusAoInimigo(S);

                }
            }
            else
                GameController.g.ContStatus.AdicionaStatusAoHeroi(S);
        }*/

        public static void InsereStatus(PetManager levou, PetBase C, DatesForTemporaryStatus dadosDoStatus,bool deMenu=false)
        {
            int numStatus = StatusTemporarioBase.ContemStatus(StatusType.envenenado, C);

            if (numStatus == -1)
            {
                StatusTemporarioBase S = new Envenenado()
                {
                    Dados = dadosDoStatus,
                    CDoAfetado = levou,
                    OAfetado = C
                };

                AddSimpleStatus.InserindoNovoStatus(levou, C, S,deMenu);
                //InserindoNovoStatus(levou, C, dadosDoStatus);
            }
            else
            {
                DatesForTemporaryStatus d = C.StatusTemporarios[numStatus];
                d.Quantificador = Mathf.Max(dadosDoStatus.Quantificador, d.Quantificador + 1);
                d.TempoSignificativo *= (14f / 15f);
            }
        }
    }

}
