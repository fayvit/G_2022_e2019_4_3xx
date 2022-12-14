using UnityEngine;
using FayvitMessageAgregator;
using System.Collections;

namespace FayvitMove
{
    [System.Serializable]
    public class StaminaManager
    {
        [SerializeField] private int staminaPoints = 100;
        [SerializeField] private int maxStaminaPoints = 100;
        [SerializeField] private float velToTotalRegen = 13;
        [SerializeField] private float zeroedVelToTotalRegen = 30;
        [SerializeField] private float runConsumeTax = .25f;
        [SerializeField] private float blockingSlowness = .25f;
        [SerializeField] private bool zeroedStamina;

        private int distanceToTotalRegen = 0;
        private int staminaStartCount = 0;
        private int runIntCount = 0;
        private float timeCount = 0;
        private bool wasRuning;
        private bool wasBlock;

        public int StaminaPoints { get => staminaPoints; }
        public int MaxStaminaPoints { get => maxStaminaPoints; }
        [System.NonSerialized] public System.Action OnChangeStaminaPoints;
        [System.NonSerialized] public System.Action OnZeroedStamina;
        [System.NonSerialized] public System.Action OnRegenZeroedStamina;

        public void ConsumeStamina(uint val, bool restarTime = true)
        {
            staminaPoints = Mathf.Max(0, staminaPoints - (int)val);

            if (staminaPoints <= 0)
            {
                OnZeroedStamina?.Invoke();
                zeroedStamina = true;
            }

            if (restarTime)
                RestartStaminaTimeCount();

            OnChangeStaminaPoints?.Invoke();
        }

        public void RestartStaminaTimeCount()
        {
            timeCount = 0;
            staminaStartCount = StaminaPoints;
            distanceToTotalRegen = maxStaminaPoints - staminaStartCount;
        }

        public void RestStamina()
        {
            RestartStaminaTimeCount();
            wasRuning = false;

        }

        public bool VerifyStaminaAction()
        {
            return !zeroedStamina;
        }

        public void StaminaRegen(bool run, bool block = false)
        {
            if (run)
            {
                if (!wasRuning)
                {
                    timeCount = 0;
                    runIntCount = 0;
                }

                wasRuning = true;

                timeCount += Time.deltaTime;

                if (timeCount - runConsumeTax * runIntCount > runConsumeTax)
                {
                    runIntCount++;
                    ConsumeStamina(1, false);
                }
            }
            else
            {
                if (wasRuning)
                    RestartStaminaTimeCount();

                wasRuning = false;
                timeCount += Time.deltaTime;
                float velToTotal = zeroedStamina ? zeroedVelToTotalRegen : velToTotalRegen;

                if (block && !wasBlock || !block && wasBlock)
                {
                    RestartStaminaTimeCount();
                }

                wasBlock = block;
                velToTotal *= block ? blockingSlowness : 1;

                bool full = StaminaPoints == MaxStaminaPoints;

                staminaPoints = distanceToTotalRegen == 0
                    ? maxStaminaPoints
                    : (int)Mathf.Lerp(staminaStartCount, maxStaminaPoints, velToTotal * timeCount / distanceToTotalRegen);

                if (!full)
                    OnChangeStaminaPoints?.Invoke();

                if (staminaPoints >= maxStaminaPoints)
                {
                    zeroedStamina = false;

                    OnRegenZeroedStamina?.Invoke();

                    timeCount = Mathf.Max(
                        blockingSlowness * zeroedVelToTotalRegen,
                        blockingSlowness * velToTotalRegen,
                        velToTotalRegen,
                        zeroedVelToTotalRegen);
                }
            }

        }
    }

    public struct MsgZeroedStamina : IMessageBase
    {
        public GameObject gameObject;
    }
    public struct MsgRegenZeroedStamina : IMessageBase
    {
        public GameObject gameObject;
    }
}