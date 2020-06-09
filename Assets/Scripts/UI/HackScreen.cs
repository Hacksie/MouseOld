using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
namespace HackedDesign
{
    public class HackScreen : MonoBehaviour
    {
        [Header("Referenced Game Objects")]
        [SerializeField] private Text inputLine;
        [SerializeField] private Text outputLine;
        [Header("Settings")]
        [SerializeField] private string[] text;
        [SerializeField] private string successText = "Success!";
        [SerializeField] private float animTime = 1.0f;
        [SerializeField] private float successTime = 0.7f;
        [SerializeField] private float cursorBlinkTime = 0.5f;
        [SerializeField] private UnityEvent completeEvent;

        [Header("State")]
        [SerializeField] private HackState state;

        private float textTimer = 0;
        private int index = 0;
        private float cursorTimer = 0;
        private bool blink = false;

        private void Awake()
        {
            if (gameObject.activeInHierarchy)
            {
                gameObject.SetActive(false);
            }
        }

        private void Update()
        {
            switch (state)
            {
                case HackState.Pre:
                    if ((Time.time - cursorTimer) >= cursorBlinkTime)
                    {
                        blink = !blink;
                        cursorTimer = Time.time;
                    }

                    inputLine.text = blink ? ">_" : ">";
                    outputLine.text = "";

                    break;
                case HackState.Activated:
                    inputLine.text = text[index];
                    outputLine.text = "";

                    if ((Time.time - textTimer) >= successTime)
                    {
                        outputLine.text = successText;
                    }

                    if ((Time.time - textTimer) >= animTime)
                    {
                        index++;
                        textTimer = Time.time;
                        if (index >= text.Length)
                        {
                            state = HackState.Complete;
                            completeEvent.Invoke();
                        }
                    }
                    break;
                case HackState.Complete:
                    break;
            }

        }

        public void Activate()
        {
            textTimer = Time.time;
            state = HackState.Activated;
        }

        public enum HackState
        {
            Pre,
            Activated,
            Complete
        }


    }
}