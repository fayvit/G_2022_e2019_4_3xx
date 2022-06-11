using UnityEngine;
using FayvitMove;
using System.Collections;

namespace FayvitLikeDarkSouls
{
    [System.Serializable]
    public class DadosDePersonagem
    {
        [SerializeField] private int lifePoints = 500;
        [SerializeField] private int maxLifePoints = 500;
        [SerializeField] private int magicPoints = 100;
        [SerializeField] private int maxMagicPoint = 100;
        [SerializeField] private StaminaManager stManager;

        public DadosDePersonagem()
        {
            stManager = new StaminaManager();
        }

        public int LifePoints { get => lifePoints; }
        public int MaxLifePoints { get => maxLifePoints; }
        public int MagicPoints { get => magicPoints; }
        public int MaxMagicPoints { get => maxMagicPoint; }
        public StaminaManager StManager { get => stManager; }


        public void ApplyDamage(uint val)
        {
            lifePoints = Mathf.Max(lifePoints - (int)val, 0);
        }

        public void RestoreLifePoints(uint val)
        {
            lifePoints = Mathf.Min(lifePoints + (int)val, maxLifePoints);
        }
    }
}