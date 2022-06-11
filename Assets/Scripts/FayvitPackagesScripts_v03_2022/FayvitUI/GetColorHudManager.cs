using FayvitEventAgregator;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace FayvitUI
{
    public class GetColorHudManager : MonoBehaviour, IDragHandler, IPointerUpHandler, IPointerDownHandler
    {
        [SerializeField] private Image target;
        [SerializeField] private RectTransform mark;
        [SerializeField] private Color showColor;
        [SerializeField] private Slider greySlider;

        private RectTransform r;
        private Image thisImage;
        private Color currentColor;
        private float greyScaleFactor = 1;


        public Color CurrentColor { get => currentColor; }
        //public Image Target { get => target; set => target = value; }

        void Start()
        {
            r = GetComponent<RectTransform>();
            thisImage = GetComponent<Image>();
        }

        // Update is called once per frame
        void Update() { }

        public Color[] RoletaDeCores()
        {
            Vector2[] V6 = new Vector2[12] {
            Vector3.up,
            new Vector2(.5f,.866f),
            new Vector2(.866f,.5f),
            Vector2.right,
            new Vector2(.866f,-.5f),
            new Vector2(.5f,-.866f),
            Vector3.down,
            new Vector2(-.5f,-.866f),
            new Vector2(-.866f,-.5f),
            Vector2.left,
            new Vector2(-.866f,.5f),
            new Vector2(-.5f,.866f),
        };

            List<Color> C = new List<Color>();
            Vector2 baseAnchoredPosition = mark.anchoredPosition;
            Vector2 V;

            for (int k = 0; k < 5; k++)
            {
                greyScaleFactor = 1 - (float)k / 5;
                C.Insert(0, new Color(greyScaleFactor, greyScaleFactor, greyScaleFactor, 1));
                //C.Add();
                for (int j = 0; j < 4; j++)
                    for (int i = 0; i < V6.Length; i++)
                    {

                        V = baseAnchoredPosition + (100 - 15 * j) * V6[i];
                        V = CalcularMarkPosition(V);
                        mark.anchoredPosition = V;
                        GetPixelColor(V);
                        C.Add(currentColor);
                    }
            }
            return C.ToArray();
        }

        public void MoveMark(Vector2 colorMove, float grayScaleMove)
        {
            //if (colorMove != Vector2.zero)
            {
                Vector2 V = mark.anchoredPosition;
                V += colorMove;
                V = CalcularMarkPosition(V);
                mark.anchoredPosition = V;
                GetPixelColor(V);
            }

            if (grayScaleMove != 0)
            {
                greyScaleFactor = Mathf.Clamp01(greyScaleFactor + grayScaleMove);
                //GreyScaleApplyFactor(greyScaleFactor);
                greySlider.value = 1 - greyScaleFactor;
            }
        }

        Vector2 CalcularMarkPosition(Vector2 localPoint)
        {
            float mag = Mathf.Min(0.95f * Mathf.Abs(r.rect.width / 2), Vector3.Magnitude(localPoint));
            return localPoint.normalized * mag;
        }

        public void OnDrag(PointerEventData ped)
        {
            Vector2 localPoint;

            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(r, ped.position, ped.pressEventCamera, out localPoint))
            {
                localPoint = CalcularMarkPosition(localPoint);
                mark.anchoredPosition = localPoint;
                GetPixelColor(localPoint);
            }
        }

        public void GetPixelColor(Vector2 myOut)
        {
            if (r == null)
                r = GetComponent<RectTransform>();


            Texture2D tx = GetComponent<Image>().sprite.texture as Texture2D;
            myOut = new Vector2((myOut.x - r.rect.x) / r.rect.width, (myOut.y - r.rect.y) / r.rect.height);
            myOut = new Vector2(myOut.x * tx.width, myOut.y * tx.height);

            Color color = tx.GetPixel((int)myOut.x, (int)myOut.y);
            color = new Color(color.r * greyScaleFactor, color.g * greyScaleFactor, color.b * greyScaleFactor, 1);
            //target.color = color;
            currentColor = color;

            EventAgregator.Publish(new GameEvent(EventKey.changeColorPicker, currentColor));

        }

        public void OnPointerUp(PointerEventData ped)
        {
            Vector2 localPoint;

            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(r, ped.position, ped.pressEventCamera, out localPoint))
            {
                localPoint = CalcularMarkPosition(localPoint);

                Debug.Log(localPoint + " : " + currentColor);
                mark.anchoredPosition = localPoint;
            }
        }

        public void OnPointerDown(PointerEventData ped)
        {
            OnDrag(ped);
        }

        public void GreyScaleSlider(Slider s)
        {
            GreyScaleApplyFactor(1 - s.value);
        }

        void GreyScaleApplyFactor(float f)
        {
            greyScaleFactor = f;
            GetPixelColor(mark.anchoredPosition);
            thisImage.color = new Color(greyScaleFactor, greyScaleFactor, greyScaleFactor, 1);
        }

        public void SetColor(Color C)
        {

            Vector2 V2 = C.r * Vector2.down + C.g * new Vector2(-.866f, .5f) + C.b * new Vector2(.866f, .5f);
            Vector2 v2n = V2.normalized;
            float greyScaleCalc = Mathf.Max(C.r, C.g, C.b);


            float n = Mathf.Min(1.05f * V2.magnitude / greyScaleCalc, 1);

            V2 = v2n * n;

            V2 = new Vector2(V2.x, V2.y);


            mark.anchoredPosition = 100 * V2;
            //GreyScaleApplyFactor(greyScaleCalc);

            greySlider.value = 1 - greyScaleCalc;
            #region tentativa2
            // Descobri que precisa de processamento para pegar a cor no for o que o torna inviável

            //Vector3 CV = new Vector3(C.r, C.g, C.b);
            //Vector3 guard = 100 * new Vector3(C.r, C.g, C.b);
            //Vector2Int V = Vector2Int.zero;
            //Texture2D tx = GetComponent<Image>().sprite.texture as Texture2D;


            //StartCoroutine(ShowPixelColor(tx));

            //for (int i = 0; i < tx.height; i++)
            //    for (int j = 0; j < tx.width; j++)
            //    {
            //        GetPixelColor(new Vector2(i, j));

            //        Vector3 curr = new Vector3(currentColor.r, currentColor.g, currentColor.b);
            //        float R = C.r != 0 ? curr.x / C.r : 0;
            //        float G = C.g != 0 ? curr.y / C.g : 0;
            //        float B = C.b != 0 ? curr.z / C.b : 0;

            //        R = GetDefinitiveFloat(new float[3] { R, G, B },1f);

            //        if (R != 0)
            //        {
            //            //Debug.Log(R);



            //            float f1 = Mathf.Abs(CV.x - curr.x) + Mathf.Abs(CV.y - curr.y) + Mathf.Abs(CV.z - curr.z);
            //            float f2 = Mathf.Abs(guard.x - curr.x) + Mathf.Abs(guard.y - curr.y) + Mathf.Abs(guard.z - curr.z);

            //            Debug.Log(i + " : " + j + " f1: " + f1 + " f2: " + f2 + " V1: " + Mathf.Abs(CV.x - curr.x) + " : " + Mathf.Abs(CV.y - curr.y) + " : " + Mathf.Abs(CV.z - curr.z)
            //                + " V2: " +
            //                Mathf.Abs(guard.x - curr.x) + " : " + Mathf.Abs(guard.y - curr.y) + " : " + Mathf.Abs(guard.z - curr.z)+currentColor);

            //            //Debug.Log("f1: " + f1 + " f2: " + f2);
            //            if (f1<f2)
            //                //Vector3.Distance(CV, curr) < Vector3.Distance(guard, curr))
            //            {
            //                Debug.Log(i + " : " + j +" f1: "+f1+" f2: "+f2+ " V1: " + Mathf.Abs(CV.x - curr.x) +" : "+ Mathf.Abs(CV.y - curr.y) +" : "+ Mathf.Abs(CV.z - curr.z)
            //                + " V2: " +
            //                Mathf.Abs(guard.x - curr.x) +" : "+ Mathf.Abs(guard.y - curr.y) +" : "+ Mathf.Abs(guard.z - curr.z));
            //                guard = curr;
            //                V = new Vector2Int(i, j);
            //                Debug.Log("corrente: "+curr + " C: " +  C+" : "+V+" CV: "+CV.x+" : "+CV.y+" : "+CV.z);
            //            }
            //        }
            //    }

            //Debug.Log(C + " : " + V + " : "+guard);
            #endregion

            #region tentativa_1
            //for (int j = 0; j < tx.width; j++)
            //    for (int i = 0; i < tx.height; i++)
            //    {
            //        GetPixelColor(new Vector2(i, j));

            //        float r = C.r != 0 ? currentColor.r / C.r : 0;
            //        float g = C.g != 0 ? currentColor.g / C.g : 0;
            //        float b = C.b != 0 ? currentColor.b / C.b : 0;

            //        Vector3 R = new Vector3(r * C.r, r * C.g, r * C.b);
            //        Vector3 G = new Vector3(g * C.r, g * C.g, g * C.b);
            //        Vector3 B = new Vector3(b * C.r, b * C.g, b * C.b);
            //        Vector3 curr = new Vector3(currentColor.r, currentColor.g, currentColor.b);


            //        if (
            //            Vector3.Distance(R, curr) < 1f ||
            //            Vector3.Distance(G, curr) < 1f ||
            //            Vector3.Distance(B, curr) < 1f
            //            )
            //        {
            //            if (Vector3.Distance(guard, curr) > Vector3.Distance(R, curr) ||
            //                Vector3.Distance(guard, curr) > Vector3.Distance(G, curr) ||
            //                Vector3.Distance(guard, curr) > Vector3.Distance(B, curr))
            //            {
            //                guard = curr;
            //                V = new Vector2Int(i, j);
            //            }

            //            Debug.Log(r + " : " + g + " : " + b + " : " + i + " : " + j);
            //        }
            //    }

            //mark.anchoredPosition = new Vector2(-380 * V.y / tx.height + 95f, -380 * V.x / tx.width + 95f);

            //Debug.Log(guard + " : " + V + " : " + mark.anchoredPosition);
            #endregion
        }

        //Tentativas removidas
        //float GetDefinitiveFloat(float[] quos,float admissive)
        //{
        //    float retorno = 0;
        //    List<float> quo = new List<float>();

        //    for (int i = 0; i < quos.Length; i++)
        //    {
        //        if (quos[i] != 0)
        //            quo.Add(quos[i]);
        //    }

        //    float menor = Mathf.NegativeInfinity;
        //    float maior = Mathf.Infinity;
        //    for (int i = 0; i < quo.Count; i++)
        //    {
        //        if (quo[i] > menor)
        //            menor = quo[i];
        //        if (quo[i] < maior)
        //            maior = quo[i];
        //    }

        //    if (maior - menor < admissive)
        //        retorno = quos[0];
        //    return retorno;
        //}

        //[SerializeField] Vector2 viewCoord;
        //IEnumerator ShowPixelColor(Texture2D tx)
        //{


        //    for (int i = 0; i < tx.height; i++)
        //        for (int j = 0; j < tx.width; j++)

        //        {

        //                yield return new WaitForSeconds(.025f);
        //                viewCoord = new Vector2(i, j);
        //                GetPixelColor(viewCoord);
        //                showColor = currentColor;

        //        }
        //}

        // Start is called before the first frame update

    }
}