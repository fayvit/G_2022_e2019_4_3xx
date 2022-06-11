using FayvitCommandReader;

namespace FayvitBasicTools
{
    public interface IPlayersInGameDb
    {
        
        int NetID { get; set; }
        string GameName { get; set; }
        PlayerDbState DbState { get; set; }
        Controlador Control { get; set; }
        ICharacterManager Manager{get;set;}
    }

    public enum PlayerDbState
    {
        abertoParaLocal,
        abertoParaRede,
        ocupadoLocal,
        ocupadoRede,
        fechado,
        desconexaoAgendada
    }

    public class PlayersInGameDb : PlayersInGameDbBase, IPlayersInGameDb
    {
        public int NetID { get; set; }
        public string GameName { get; set; }
        public PlayerDbState DbState { get; set; }
        public Controlador Control { get; set; }
        public ICharacterManager Manager { get; set; }
    }

    public class PlayersInGameDbBase
    {
        public static int FirstOfState(IPlayersInGameDb[] s,PlayerDbState findingState)
        {
            for (int i = 0; i < s.Length; i++)
            {
                if (s[i].DbState == findingState)
                    return i;
            }

            return -1;
        }

        /// <summary>
        /// Retorna true se o controlador não controle ninguém e falso caso contrario
        /// </summary>
        /// <param name="s">Banco de dados de jogadores</param>
        /// <param name="controlador">id do controlador a ser avaliado</param>
        /// <returns></returns>
        public static bool IDontControlAnyone(IPlayersInGameDb[] s, int controlador)
        {
            for (int i = 0; i < s.Length; i++)
            {
                if (s[i].DbState == PlayerDbState.ocupadoLocal && (int)s[i].Control == controlador)
                    return false;
            }

            return true;
        }
    }
}