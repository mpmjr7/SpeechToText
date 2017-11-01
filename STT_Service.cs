using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

using IBM.WatsonDeveloperCloud.SpeechToText.v1;
using IBM.WatsonDeveloperCloud.SpeechToText.v1.Model;
using IBM.WatsonDeveloperCloud.SpeechToText.v1.Util;

namespace SpeechToText
{
    public class STT_Service
    {
        private SpeechToTextService _speechToText = new SpeechToTextService();
        private string _audioFilePath { get; set; }
        private string _outPath { get; set; }
        private string _audioFileContentType = "audio/mp3";
        private string _modelToGet = "en-US_BroadbandModel";
        private string _sessionID;
        AutoResetEvent autoEvent = new AutoResetEvent(false);

        public STT_Service(string username, string password, string audioPath, string outputPath)
        {
            _speechToText.SetCredential(username, password);
            _audioFilePath = audioPath;
            _outPath = outputPath;

            CreateSession();
            RecognizeSessionBody();
            DeleteSession();

            //Console.WriteLine("\nSTT_Test2 complete.");
        }

        #region Create Session
        private void CreateSession()
        {
            //Console.WriteLine(string.Format("\nCalling CreateSession({0})...", _modelToGet));
            var result = _speechToText.CreateSession(_modelToGet);

            if (result != null)
            {
                // Console.WriteLine("Session received...");
                // Console.WriteLine("SessionId: {0}\nNewSessionUri: {1}\nRecognize: {2}\nRecognizeWS: {3}\nObserveResult: {4}",
                //    result.SessionId,
                //    result.NewSessionUri,
                //    result.Recognize,
                //    result.RecognizeWS,
                //    result.ObserveResult);

                _sessionID = result.SessionId;
            }
            else
            {
                //Console.WriteLine("Session is null.");
            }
        }
        #endregion

        #region Recognize Session Body
        private void RecognizeSessionBody()
        {
            using (FileStream fs = File.OpenRead(_audioFilePath))
            {
                using (StreamWriter sw = new StreamWriter(_outPath))
                {
                    //Console.WriteLine("\nCalling RecognizeSessionBody()...");
                    var speechEvent = _speechToText.RecognizeWithSession(_sessionID, _audioFileContentType, fs);

                    if (speechEvent != null)
                    {
                        if (speechEvent.Results != null || speechEvent.Results.Count > 0)
                        {
                            foreach (SpeechRecognitionResult result in speechEvent.Results)
                            {
                                if (result.Alternatives != null && result.Alternatives.Count > 0)
                                {
                                    foreach (SpeechRecognitionAlternative alternative in result.Alternatives)
                                    {
                                        //Console.WriteLine(string.Format("{0}, {1}", alternative.Transcript, alternative.Confidence));
                                        sw.WriteLine(string.Format("{0}, {1}", alternative.Transcript, alternative.Confidence));
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        //Console.WriteLine("Result is null");
                    }
                }
            }
        }
        #endregion

        #region Delete Session
        private void DeleteSession()
        {
            Console.WriteLine(string.Format("\nCalling DeleteSession({0})...", _sessionID));
            var result = _speechToText.DeleteSession(_sessionID);

            if (result != null)
            {
                Console.WriteLine(string.Format("Session {0} deleted.", _sessionID));
            }
            else
            {
                Console.WriteLine("Result is null");
            }

        }
        #endregion

    }
}
