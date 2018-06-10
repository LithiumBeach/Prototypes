using patterns;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections.Generic;
using System;

namespace pe
{

    public class PlayerConsoleUI : SingletonBehavior<PlayerConsoleUI>
    {
        public InputField console;
        public RectTransform logRoot;
        public RectTransform logRect;
        public ScrollRect m_LogScrollRect;

        public Stack<LogData> m_LogStack;

        //history
        private List<LogData> m_LogHistory;
        private int m_LogHistoryIndex=0;
        private bool m_LogHistoryFirstKeyDown = true;

        public Text _PlayerLogText;
        public Text _AILogText;

        private List<BaseCommand> m_Commands;

        public RectTransform m_VisualRoot;

        protected override void OnAwake()
        {
            base.OnAwake();
            InputManager.Instance.OnConsoleKeyDown += OnConsoleKeyDown;
            InputManager.Instance.OnSubmitText += OnSubmitText;
            InputManager.Instance.OnConsoleHistoryDown += OnConsoleHistoryDown;

            m_VisualRoot.gameObject.SetActive(true);
            logRect.gameObject.SetActive(true);
            m_LogStack = new Stack<LogData>();
            m_LogHistory = new List<LogData>();

            InitializeCommands();

            SetContextInputField(console);
        }

        private void InitializeCommands()
        {
            m_Commands = new List<BaseCommand>();
            AddCommand(new CmdClearLog());
            AddCommand(new CmdList());
            AddCommand(new CmdExplode());
            AddCommand(new CmdFocusAndMoveTo());
        }

        public void AddCommand(BaseCommand _cmd)
        {
            if (!m_Commands.Contains(_cmd))
            {
                m_Commands.Add(_cmd);
                _cmd.Initialize();
            }
        }

        private void OnConsoleKeyDown(bool ctrl, bool alt, bool shift)
        {
            ToggleConsole();
        }

        public void ToggleConsole()
        {
            m_VisualRoot.gameObject.SetActive(!m_VisualRoot.gameObject.activeSelf);
            if (console.gameObject.activeSelf)
            {
                SetContextInputField(console);
            }

            logRect.gameObject.SetActive(!logRect.gameObject.activeSelf);
            if (logRect.gameObject.activeSelf)
            {

            }
        }

        public void PushToLog(LogData _newLog)
        {
            m_LogStack.Push(_newLog);

            Text newLogText = null;
            switch (_newLog.m_Alignment)
            {
                case TextAnchor.MiddleLeft:
                    newLogText = Instantiate(_PlayerLogText, logRoot.transform);
                    m_LogHistory.Add(_newLog);
                    m_LogHistoryIndex++;
                    break;
                case TextAnchor.MiddleCenter:
                    break;
                case TextAnchor.MiddleRight:
                    newLogText = Instantiate(_AILogText, logRoot.transform);
                    break;
                default:
                    break;
            }
            newLogText.text = _newLog.m_Text;
            newLogText.alignment = _newLog.m_Alignment;
            _newLog.m_GO = newLogText.gameObject;
            Invoke("ScrollToLogBottom", Time.deltaTime * 4f);
        }

        public void ScrollToLogBottom()
        {
            m_LogScrollRect.verticalNormalizedPosition = 0;
        }

        //single character 
        private int charCount = 0;
        public void OnValueChanged(string _c)
        {
            if (charCount == 0)
            {
                console.placeholder.gameObject.SetActive(false);
            }
            charCount++;
        }

        private void OnSubmitText()
        {
            OnEndEdit(console.text);
            m_LogHistoryIndex = m_LogHistory.Count - 1;
            m_LogHistoryFirstKeyDown = true;
        }

        private bool m_SkipLog = false;
        //command entered
        public void OnEndEdit(string cmd)
        {
            //try command
            bool success = TryCommand(cmd);

            //reset console text input field.
            console.text = "";
            console.textComponent.text = "";

            //push to backlog
            if (!m_SkipLog)
            {
                PushToLog(new LogData(cmd, TextAnchor.MiddleLeft));
            }
            else
            {
                m_SkipLog = false;
            }

            //virtual click on input field.
            SetContextInputField(console);
        }

        private void OnConsoleHistoryDown(bool ctrl, bool alt, bool shift, float axisValue)
        {
            //make sure the console is open.
            if (!console.gameObject.activeInHierarchy || m_LogHistory.Count < 1)
            {
                return;
            }

            if (!m_LogHistoryFirstKeyDown)
            {
                //down (go forward in history)
                if (axisValue < 0)
                {
                    m_LogHistoryIndex++;
                }
                //up (go back in history)
                else if (axisValue > 0)
                {
                    m_LogHistoryIndex--;
                }
                else
                {
                    return;
                } 
            }

            m_LogHistoryFirstKeyDown = false;

            m_LogHistoryIndex = Mathf.Clamp(m_LogHistoryIndex, 0, m_LogHistory.Count - 1);
            console.text = m_LogHistory[m_LogHistoryIndex].m_Text;
            console.caretPosition = console.text.Length;
        }

        /// <summary>
        /// attempt to find the command the user is attempting to invoke.
        /// always the first chunk of submitted string
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        private bool TryCommand(string s)
        {
            //convert all to upper case
            s = s.ToUpper();

            //split assuming space delimiter char
            string[] cmdFull = s.Split(' ');

            //make base command (first entry)
            string cmd = cmdFull[0];

            //make array of parameters
            string[] parameters = new string[cmdFull.Length - 1];
            for (int i = 1; i < cmdFull.Length; i++)
            {
                parameters[i - 1] = cmdFull[i];
            }

            for (int i = 0; i < m_Commands.Count; i++)
            {
                if (m_Commands[i].CheckCommandString(cmd))
                {
                    m_Commands[i].Evaluate(parameters);

                    //special case: don't log screen clears. This should be the only cmd we don't log
                    if (m_Commands[i] as CmdClearLog != null)
                    {
                        m_SkipLog = true;
                    }

                    return true;
                }
            }
            Debug.Log("BaseCommand::CheckCommandString: '" + cmd + "' is not a valid command.");

            return false;
        }

        //set event system's selected gameobject, no clicking required
        public void SetContextInputField(InputField _obj)
        {
            EventSystem.current.SetSelectedGameObject(_obj.gameObject, null);
            _obj.OnPointerClick(new PointerEventData(EventSystem.current));
        }

        public void ClearLog()
        {
            while(m_LogStack.Count > 0)
            {
                Destroy(m_LogStack.Peek().m_GO);
                m_LogStack.Pop();
            }
            m_LogStack.Clear();
        }
    }
    public class LogData
    {
        public LogData(string _text, TextAnchor _alignment = TextAnchor.MiddleCenter)
        {
            m_Text = _text;
            m_Alignment = _alignment;
        }

        public string m_Text;
        //only use middle alignment
        //left aligned = player text
        //right aligned = AI text
        //center aligned = help, nonspecific dialogue
        public TextAnchor m_Alignment;

        //don't worry about setting this, it'll be set in PushLog()
        public GameObject m_GO;
    }
}