using UnityEngine;
using UnityEditor;
using FayvitMessageAgregator;
using FayvitBasicTools;
using FayvitSupportSingleton;

public class TuinManager
{
    public static void RequestDoubleDown()
    {
        MessageAgregator<MsgRequestSfx>.Publish(new MsgRequestSfx()
        {
            sfxId = FayvitSounds.SoundEffectID.tuin_1ponto3
        });

        SupportSingleton.Instance.InvokeInSeconds(() =>
        {
            MessageAgregator<MsgRequestSfx>.Publish(new MsgRequestSfx()
            {
                sfxId = FayvitSounds.SoundEffectID.tuin_1ponto3
            });
        }, .15f);
    }
    public static void RequestDecreaseTuin()
    {
        MessageAgregator<MsgRequestSfx>.Publish(new MsgRequestSfx()
        {
            sfxId = FayvitSounds.SoundEffectID.tuimParaNivel
        });

        SupportSingleton.Instance.InvokeInSeconds(() =>
        {
            MessageAgregator<MsgRequestSfx>.Publish(new MsgRequestSfx()
            {
                sfxId = FayvitSounds.SoundEffectID.tuin_1ponto3
            });
        },.15f);
    }

    public static void RequestTripleTuin()
    {
        MessageAgregator<MsgRequestSfx>.Publish(new MsgRequestSfx()
        {
            sfxId = FayvitSounds.SoundEffectID.tuin_1ponto3
        });

        SupportSingleton.Instance.InvokeInSeconds(() => {
            MessageAgregator<MsgRequestSfx>.Publish(new MsgRequestSfx()
            {
                sfxId = FayvitSounds.SoundEffectID.tuin_1ponto3
            });

            SupportSingleton.Instance.InvokeInSeconds(() => {
                MessageAgregator<MsgRequestSfx>.Publish(new MsgRequestSfx()
                {
                    sfxId = FayvitSounds.SoundEffectID.tuimParaNivel
                });
            }, 0.15f);
        }, 0.15f);
    }
}