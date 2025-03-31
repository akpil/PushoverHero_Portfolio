using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.IO;

namespace Utility
{
    public class LogCollector : MonoBehaviour
    {
        // Use this for initialization
        private List<Log> _logList;
        private Dictionary<LogType, List<Log>> _logDic;

        private class ExceptionDump
        {
            public Dictionary<string, string> SaveData;
            public List<Log> LogList;
        }

        private void Awake()
        {
            _logList = new List<Log>();
            _logDic = new Dictionary<LogType, List<Log>>();
            DontDestroyOnLoad(gameObject);

            Application.logMessageReceivedThreaded += OnLogCollected;
        }

        private void OnLogCollected(string condition, string stack, LogType logType)
        {
            var newLog = new Log(logType, condition, stack);
            _logList.Add(newLog);
            if (!_logDic.ContainsKey(logType))
            {
                _logDic[logType] = new List<Log>();
            }

            _logDic[logType].Add(newLog);


            if (logType == LogType.Exception)
            {
                // string[] stackArr = stack.Split('\n');
                // GameController.Instance.ValidSaveData = false;
                // ExceptionDump dump = new ExceptionDump();
                // dump.SaveData = GameController.Instance.GetExceptionSaveDataDump();
                // dump.LogList = mLogList;
                // string uid = MobileComponents.FBAuthHandler.Instance.UID;
                // MobileComponents.FBDatabaseHandler.Instance.UploadExceptionDump(uid, dump);
                // PopupMessageController.Instance.OpenMessagePopup($"치명적인 버그가 발생했습니다.\n\n이 화면을 캡쳐해\n디스코드 또는 갤러리에\n제보해주시면 빠르게\n대처해 해결하도록 하겠습니다.\n확인 버튼을 누르시면 게임이 종료됩니다.\n\n {condition} \n{stackArr[0]}",
                //                                                ePopupButtonType.Single,
                //                                                Application.Quit);
            }
        }
        
        private void OnApplicationQuit()
        {
    #if UNITY_EDITOR || UNITY_STANDALONE_WIN
            string location = string.Concat(Application.streamingAssetsPath, "/Play_LogAll.txt");
            string data = JsonConvert.SerializeObject(_logList, Formatting.Indented);
            StreamWriter streamWriter = new StreamWriter(location);
            streamWriter.Write(data);
            streamWriter.Close();

            location = string.Concat(Application.streamingAssetsPath, "/Play_LogDic.txt");
            data = JsonConvert.SerializeObject(_logDic, Formatting.Indented);
            streamWriter = new StreamWriter(location);
            streamWriter.Write(data);
            streamWriter.Close();
    #endif
        }

        [System.Serializable]
        public class Log
        {
            public LogType LogType;
            public string LogMessage;
            public string Stack;
            public Log(LogType logType, string message, string stack)
            {
                LogType = logType;
                LogMessage = message;
                Stack = stack;
            }
        }
    }
}
