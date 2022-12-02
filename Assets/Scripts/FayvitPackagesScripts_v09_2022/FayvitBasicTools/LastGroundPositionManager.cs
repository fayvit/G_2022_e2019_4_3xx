using UnityEngine;

namespace FayvitBasicTools
{
    [System.Serializable]
    public class LastGroundPositionManager
    {
        [SerializeField] private bool debug;
        [SerializeField] private float intervalToUpdate = 3;
        private Vector3 lastPosition;
        private float tempoDecorrido = 0;

        public float IntervalToUpdate => intervalToUpdate;
        public Vector3 Get { get { return lastPosition; } }
        public LastGroundPositionManager(Vector3 startPosition)
        {
            lastPosition = startPosition;
        }

        public void SetPosition(Vector3 pos)
        {
            lastPosition = pos;
            tempoDecorrido = 0;
        }

        public void NoTimedUpdate(bool podeAtualizar, Transform transform)
        {
            if (debug)
                Debug.Log("tentou: ");
            RaycastHit[] hit = new RaycastHit[5];
            if (podeAtualizar &&
            Physics.Raycast(transform.position, Vector3.down, out hit[0])
            )
            {
                Vector3[] dirs = new Vector3[5]
                {
                    Vector3.zero,
                    Vector3.forward,
                    Vector3.back,
                    Vector3.right,
                    Vector3.left
                };

                bool vai = true;

                for (int i = 0; i < 5; i++)
                    if (Physics.Raycast(transform.position + dirs[i], Vector3.down, out hit[i], 2))
                    {
                        float angle = Vector3.Angle(hit[i].normal, Vector3.up);
                        if (debug)
                        {
                            Debug.Log("Entrou no debug");
                            GameObject G = GameObject.CreatePrimitive(PrimitiveType.Cube);
                            G.transform.position = hit[i].point;
                            G.transform.localScale = new Vector3(.1f, .1f, 1);
                            MonoBehaviour.Destroy(G.GetComponent<Collider>());
                            G.transform.rotation = Quaternion.LookRotation(hit[i].normal);
                            G.GetComponent<Renderer>().material.color = (angle < 65 && !hit[i].collider.isTrigger) ? Color.blue : Color.red;
                        }

                        if (hit[i].collider.isTrigger)
                            vai = false;

                        if (angle < 65)
                            vai &= true;
                        else
                            vai = false;
                    }
                    else vai = false;

                lastPosition = vai ? transform.position : lastPosition;
            }
        }

        public void Update(bool podeAtualizar, Transform transform)
        {
            tempoDecorrido += Time.deltaTime;
            if (tempoDecorrido > intervalToUpdate)
            {
                tempoDecorrido = 0;
                NoTimedUpdate(podeAtualizar, transform);
                
            }



        }


    }
}
