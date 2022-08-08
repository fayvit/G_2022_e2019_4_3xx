using UnityEngine;
using System.Collections;
using Criatures2021;
using FayvitSounds;
using System;

public class ResourcesFolders {

    public static AudioClip GetClip(NameMusic n)
    {
        return Resources.Load<AudioClip>(n.ToString());
    }

    public static Sprite GetMiniStatus(StatusType P)
    {
        return Resources.Load<Sprite>("miniStatus/" + P.ToString());
    }

    public static Sprite GetMiniPet(PetName P)
    {
        return Resources.Load<Sprite>("miniCriatures/" + P.ToString());
    }

    public static Sprite GetMiniAttack(AttackNameId atk)
    {
        return Resources.Load<Sprite>("miniGolpes/" + atk.ToString());
    }

    public static Sprite GetMiniItem(NameIdItem nameItem)
    {
        return Resources.Load<Sprite>("miniItens/" + nameItem.ToString());
    }

    public static GameObject GetGeneralElements(GeneralElements G)
    {
        return Resources.Load<GameObject>("GeneralElements/"+G.ToString());
    }

    public static GameObject GetGeneralElements(GeneralParticles G)
    {
        return Resources.Load<GameObject>("particles/" + G.ToString());
    }

    internal static Sprite GetMiniInfo(Criatures2021Hud.InfoMessageType info)
    {
        return Resources.Load<Sprite>("miniInfos/" + info.ToString());
    }

    public static GameObject GetPet(PetName NomeID)
    {
        return Resources.Load<GameObject>("Criatures/" + NomeID.ToString());
    }
}

public enum GeneralElements
{ 
    passouDeNivel,
    cilindroEncontro,
    keydjeyParticle
}
