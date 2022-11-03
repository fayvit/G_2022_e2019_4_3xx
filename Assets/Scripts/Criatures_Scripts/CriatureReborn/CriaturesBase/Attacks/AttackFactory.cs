using UnityEngine;
using System.Collections;

namespace Criatures2021
{
    public class AttackFactory
    {
        public static PetAttackBase GetAttack(AttackNameId nome)
        {
            PetAttackBase retorno;
            switch (nome)
            {
                case AttackNameId.rajadaDeAgua:
                    retorno = new RajadaDeAgua();
                break;

                case AttackNameId.turboDeAgua:
                    retorno = new TurboDeAgua();
                break;
                case AttackNameId.impulsoAquatico:
                    retorno = new ImpulsoAquativo();
                break;
                case AttackNameId.tapa:
                    retorno = new Tapa();
                break;

                case AttackNameId.laminaDeFolha:
                    retorno = new LaminaDeFolhas();
                break;

                case AttackNameId.garra:
                    retorno = new Garra();
                break;

                case AttackNameId.furacaoDeFolhas:
                    retorno = new FuracaoDeFolhas();
                break;

                case AttackNameId.bolaDeFogo:
                    retorno = new BolaDeFogo();
                break;

                case AttackNameId.rajadaDeFogo:
                    retorno = new RajadaDeFogo();
                break;
                case AttackNameId.impulsoFlamejante:
                    retorno = new ImpulsoFlamejante();
                break;
                case AttackNameId.ventania:
                    retorno = new Ventania();
                break;
                case AttackNameId.bico:
                    retorno = new Bico();
                break;
                case AttackNameId.ventosCortantes:
                    retorno = new VentosCortantes();
                break;
                case AttackNameId.chicoteDeCalda:
                    retorno = new ChicoteDeCalda();
                break;
                case AttackNameId.gosmaDeInseto:
                    retorno = new GosmaDeInseto();
                break;
                case AttackNameId.gosmaAcida:
                    retorno = new GosmaAcida();
                break;
                case AttackNameId.deslizamentoNaGosma:
                    retorno = new DeslizamentoNaGosma();
                break;
                case AttackNameId.psicose:
                    retorno = new Psicose();
                break;
                case AttackNameId.bolaPsiquica:
                    retorno = new BolaPsiquica();
                break;
                case AttackNameId.flashPsiquico:
                    retorno = new FlashPsiquico();
                break;
                case AttackNameId.chicoteDeMao:
                    retorno = new ChicoteDeMao();
                break;
                case AttackNameId.dentada:
                    retorno = new Dentada();
                break;
                case AttackNameId.sobreSalto:
                    retorno = new SobreSalto();
                    break;
                case AttackNameId.agulhaVenenosa:
                    retorno = new AgulhaVenenosa();
                break;
                case AttackNameId.ondaVenenosa:
                    retorno = new OndaVenenosa();
                break;
                case AttackNameId.bastao:
                    retorno = new Bastao();
                break;
                case AttackNameId.pedregulho:
                    retorno = new Pedregulho();
                    break;
                case AttackNameId.cascalho:
                    retorno = new Cascalho();
                    break;
                case AttackNameId.pedraPartida:
                    retorno = new PedraPartida();
                break;
                case AttackNameId.cabecada:
                    retorno = new Cabecada();
                break;
                case AttackNameId.chute:
                    retorno = new Chute();
                break;
                case AttackNameId.espada:
                    retorno = new Espada();
                break;
                case AttackNameId.chifre:
                    retorno = new Chifre();
                break;
                case AttackNameId.tempestadeDeFolhas:
                    retorno = new TempestadeDeFolhas();
                break;

                case AttackNameId.eletricidade:
                    retorno = new Eletricidade();
                break;
                case AttackNameId.eletricidadeConcentrada:
                    retorno = new EletricidadeConcentrada();
                break;
                case AttackNameId.impulsoEletrico:
                    retorno = new ImpulsoEletrico();
                break;
                case AttackNameId.tempestadeEletrica:
                    retorno = new TempestadeEletrica();
                break;

                case AttackNameId.hidroBomba:
                    retorno = new HidroBomba();
                break;

                case AttackNameId.tosteAtaque:
                    retorno = new TosteAtaque();
                break;

                case AttackNameId.chuvaVenenosa:
                    retorno = new ChuvaVenenosa();
                break;
                case AttackNameId.rajadaDeTerra:
                    retorno = new RajadaDeTerra();
                    break;
                case AttackNameId.vingancaDaTerra:
                    retorno = new VingancaDaTerra();
                break;
                case AttackNameId.cortinaDeTerra:
                    retorno = new CortinaDeTerra();
                break;
                case AttackNameId.rajadaDeGas:
                    retorno = new RajadaDeGas();
                    break;
                case AttackNameId.bombaDeGas:
                    retorno = new BombaDeGas();
                break;
                case AttackNameId.cortinaDeFumaca:
                    retorno = new CortinaDeFumaca();
                break;
                case AttackNameId.anelDoOlhar:
                    retorno = new AnelDoOlhar();
                break;
                case AttackNameId.teletransporte:
                    retorno = new Teletransporte();
                break;
                case AttackNameId.avalanche:
                    retorno = new Avalanche();
                break;
                case AttackNameId.multiplicar:
                    retorno = new Multiplicar();
                break;
                case AttackNameId.sobreVoo:
                    retorno = new SobreVoo();
                break;
                case AttackNameId.tesoura:
                    retorno = new Tesoura();
                break;
                //case nomesGolpes.sabreDeAsa:
                //case nomesGolpes.sabreDeBastao:
                //case nomesGolpes.sabreDeEspada:
                //case nomesGolpes.sabreDeNadadeira:
                //    retorno = new Sabre(nome);
                //    break;
                case AttackNameId.olharMal:
                    retorno = new OlharMal();
                break;
                case AttackNameId.olharEnfraquecedor:
                    retorno = new OlharEnfraquecedor();
                break;
                default:
                    retorno = new NullPetAttack(new PetAttackFeatures());
                break;
            }
            return retorno;
        }
    }
}