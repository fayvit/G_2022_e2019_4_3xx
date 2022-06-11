using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public struct CharacterIkSupport
{
    [Range(0, 10), SerializeField] private float distanceRay;// = 3;
    [Range(0, 1), SerializeField] private float maxDeslBone;// = .8f;
    [Range(0, 2), SerializeField] private float varOriginRay;// = 1;
    [Range(0, 3), SerializeField] private float varDirRay;// = 1.15f;
    [Range(0, 1), SerializeField] private float varFootGround;// = 0.2f;
    [SerializeField] private float varTransformPosition;// = 0;
    [SerializeField] private float minRadiusToIk;// = 0.22f;
    [SerializeField] private LayerMask lMask;
    [SerializeField] private ListAnimationNames[] layer_animationName;

    private Animator A;

    [System.Serializable]
    public struct ListAnimationNames
    {
        public string[] animacoes;
    }

    public void SetAnimator(Animator A)
    {
        this.A = A;
        VerifyStandardValues();
    }

    void VerifyStandardValues()
    {
        if (distanceRay == default
            && maxDeslBone == default
            && varOriginRay == default
            && varDirRay == default
            && varFootGround == default
            && varTransformPosition == default
            && minRadiusToIk == default
            )
        {
            distanceRay = 3;
            maxDeslBone = .8f;
            varOriginRay = .1f;
            varDirRay = 1.15f;
            varFootGround = .12f;
            varTransformPosition = 0;
            minRadiusToIk = .22f;
        }
    }

    public bool VerifyIkAnimationApplyable()
    {
        for (int i = 0; i < layer_animationName.Length; i++)
        {
            for (int j = 0; j < layer_animationName[i].animacoes.Length; j++)
            {
                if (A.GetCurrentAnimatorStateInfo(i).IsName(layer_animationName[i].animacoes[j]))
                    return true;
            }
        }

        return false;
    }

    public void SetFootIk(AvatarIKGoal foot)
    {
        if (!Physics.Raycast(A.GetIKPosition(foot), Vector3.down, minRadiusToIk))

        {
            RaycastHit hit;
            Vector3 startPos = A.GetIKPosition(foot) + varOriginRay * Vector3.up;
            Vector3 varDir = A.transform.position + varTransformPosition * Vector3.up - startPos;
            varDir = varDir.normalized;
            Ray r = new Ray(startPos, Vector3.down + varDirRay * varDir);

            if (Physics.Raycast(r, out hit, distanceRay, lMask))
            {
                if (hit.transform.tag == "cenario")
                {

                    Vector3 pos = hit.point;
                    pos.y += varFootGround;

                    if (Vector3.Distance(pos, A.GetIKPosition(foot)) < maxDeslBone)
                    {
                        A.SetIKPosition(foot, pos);

                        A.SetIKRotation(foot, Quaternion.LookRotation(A.GetIKRotation(foot) * Vector3.forward, hit.normal));
                    }

                }
            }
        }
    }
}