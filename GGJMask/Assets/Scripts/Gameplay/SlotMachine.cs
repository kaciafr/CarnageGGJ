using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using DG.Tweening;
using Masque;
using Random = UnityEngine.Random;

namespace Gameplay
{
    public class SlotMachine : MonoBehaviour
    {
        
        public InventorySysteme inventory;
        public GameObject[] wheels; 
        public GameObject[] Lightsphere;

        public Material materialOn;  
        public Material materialOff; 

        public float blinkSpeed = 0.5f;
        public float pullAngle = -70f;
        public float restAngle = 0f;
        private float returnDuration = 0.5f;
        private bool isAnimating = false;
        
        private SlotSymbol[] wheelResults;
        [SerializeField] private ScriptEntermachine scriptEntermachine;
        [SerializeField] private AudioSource audioSource;
        [SerializeField] private AudioSource VictoryaudioSource;
        public enum SlotSymbol
        {
            Cherry,
            Lemon,
            Skull
        }
            
        [SerializeField] private List<MaskData> maskData = new List<MaskData>();
        
        private Tweener[] wheelTweens;
        
        public void Start()
        {
            wheelResults = new SlotSymbol[wheels.Length];
            wheelTweens = new Tweener[wheels.Length];
            
            foreach (GameObject light in Lightsphere)
            {
                light.GetComponent<MeshRenderer>().material = materialOff;
            }
        }
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space) && !isAnimating && scriptEntermachine.enter)
            {
               AnimateSlotMachine();
                audioSource.Play();
            }
        }
        public async void AnimateSlotMachine()
        {
            if (isAnimating) return;

            isAnimating = true;

            Sequence sequence = DOTween.Sequence();
            sequence.Append(transform.DOLocalRotate(new Vector3( 0f,pullAngle, 0f), returnDuration).SetEase(Ease.OutQuad));
            sequence.Append(transform.DOLocalRotate(new Vector3( 0f,restAngle, 0f), returnDuration).SetEase(Ease.OutBounce));

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
        
                wheelTweens[i] = wheels[i].transform.DOLocalRotate(new Vector3( 0,360, 0), randomSpeed, RotateMode.FastBeyond360)
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
                wheelResults[i] = GetRandomSymbol();
                Debug.Log($"Roue {i} : {wheelResults[i]}");
            }

            CheckWin();
            return;
        }

        private void CheckWin()
        {
            SlotSymbol firstSymbol = wheelResults[0];
            for (int i = 1; i < wheels.Length; i++)
            {
                if (wheelResults[i] != firstSymbol)
                {
                    Debug.Log($"Perdu gros Looser");
                    return;
                }

            }
                OnWin(firstSymbol);
        }

        private void OnWin( SlotSymbol symbol)
        {
            VictoryaudioSource.Play();
            Debug.Log($" GAGNÉ : {symbol} !");
            RandomWin();

            int reward = GetReward(symbol);
            Debug.Log($"Gain : {reward}");
        }
        
        private int GetReward(SlotSymbol symbol)
        {
            return symbol switch
            {
                SlotSymbol.Cherry => 10,
                SlotSymbol.Lemon => 20,
                SlotSymbol.Skull => 50,
                _ => 0
            };
        }


        private SlotSymbol GetRandomSymbol()
        {
            int count = System.Enum.GetValues(typeof(SlotSymbol)).Length;
            return (SlotSymbol)Random.Range(0, count);
        }

        private void RandomWin()
        {
            if (inventory == null)
            {
                Debug.LogError("InventorySysteme n'est pas assigné !");
                isAnimating = false;
                return;
            }

            MaskData maskWin = maskData[Random.Range(0, maskData.Count)];
            inventory.AddMAsk(maskWin);
            Debug.Log("Masque gagné : " + maskWin.Name);
            isAnimating = false;
        }

    }
}
