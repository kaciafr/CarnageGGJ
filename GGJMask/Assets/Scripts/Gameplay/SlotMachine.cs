using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using DG.Tweening;

namespace Gameplay
{
    public class SlotMachine : MonoBehaviour
    {
        public GameObject[] wheels; 

        public GameObject[] Lightsphere;

        public Material materialOn;  
        public Material materialOff; 

        public float blinkSpeed = 0.5f;

        public float pullAngle = -70f;

        public float restAngle = 0f;

        private float returnDuration = 0.5f;

        private bool isAnimating = false;

        public float[] speedRotations = new float[] { 0.3f, 0.5f, 0.7f }; // Vitesse pour chaque roue

        private Tweener[] wheelTweens;

        public void Start()
        {
            wheelTweens = new Tweener[wheels.Length];
            
            foreach (GameObject light in Lightsphere)
            {
                light.GetComponent<MeshRenderer>().material = materialOff;
            }
        }

        private void Update()
        {
           // if (Input.GetKeyDown(KeyCode.Space) && !isAnimating)
           // {
                //AnimateSlotMachine();
           // }        
        }

        public async void AnimateSlotMachine()
        {
            if (isAnimating) return;

            isAnimating = true;

            Sequence sequence = DOTween.Sequence();
            sequence.Append(transform.DOLocalRotate(new Vector3(pullAngle, 0f, 0f), returnDuration).SetEase(Ease.OutQuad));
            sequence.Append(transform.DOLocalRotate(new Vector3(restAngle, 0f, 0f), returnDuration).SetEase(Ease.OutBounce));

            OnLeverPull();
            
            TurnWheels();
            
            await Task.Delay(1000);
            StartCoroutine(BlinkLights());
            
            await Task.Delay(3000);
            StopWheels();
            
            isAnimating = false;
        }

        private void OnLeverPull()
        {
            Debug.Log("Levier actionné");
        }

        IEnumerator BlinkLights()
        {
            for (int i = 0; i < 3; i++) 
            {
                foreach (GameObject light in Lightsphere)
                {
                    light.GetComponent<MeshRenderer>().material = materialOn;
                    yield return new WaitForSeconds(blinkSpeed);

                    light.GetComponent<MeshRenderer>().material = materialOff;
                    yield return new WaitForSeconds(blinkSpeed * 0.5f);
                }
            }
        }

        public float minSpeed = 0.3f;
        public float maxSpeed = 0.8f;

        public void TurnWheels()
        {
            for (int i = 0; i < wheels.Length; i++)
            {
                float randomSpeed = Random.Range(minSpeed, maxSpeed);
        
                wheelTweens[i] = wheels[i].transform.DOLocalRotate(new Vector3(360, 0, 0), randomSpeed, RotateMode.FastBeyond360)
                    .SetLoops(-1, LoopType.Restart)
                    .SetEase(Ease.Linear)
                    .SetRelative(true);
            }
        }


        public void StopWheels()
        {
            for (int i = 0; i < wheelTweens.Length; i++)
            {
                wheelTweens[i]?.Kill();
            }
        }
    }
}
