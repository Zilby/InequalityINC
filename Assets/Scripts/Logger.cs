using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public static class Logger {
    private static StreamWriter logStream;

    /// <summary>
    /// The default filepath for logging.
    /// </summary>
    public const string DEFAULT_FILENAME = "logfile";

    /// <summary>
    /// Is logging active? Initial value is the default. 
    /// </summary>
    private static bool logActive = false;
    public static bool LogActive
    {
        get { return logActive; }
        set { logActive = value; }
    }
      
    /// <summary>
    /// Creates a new log file, if one doesn't already exist.
    /// </summary>
    public static void CreateLog(string logFile)
    {
        if (logStream != null)
            Debug.Log ("Log file already created: doing nothing.");
        else {
            // creates Logs folder if it doesn't exist
            if (!Directory.Exists(Application.persistentDataPath + "/logs"))
                Directory.CreateDirectory(Application.persistentDataPath + "/logs");
            
            // Finds an unused filename and creates the log there.
            string fileName = logFile;
            int redundancy = 1;
            while (File.Exists (Application.persistentDataPath + "/logs/" + fileName + ".txt"))
                fileName = logFile + ((++redundancy).ToString ());
            logStream = File.CreateText (Application.persistentDataPath + "/logs/" + fileName + ".txt");

            // Logs time of creation
            Debug.Log("Playtest Log Created at: " + Application.persistentDataPath + "/logs/" + fileName + ".txt");
            logStream.AutoFlush = true;
            Log("Playtest Log Created");
        }
    }

    /// <summary>
    /// Log the specified message, with a timestamp.
    /// </summary>
    /// <param name="msg">the message to log.</param>
    public static void Log(string msg) {
        if (logActive) {
            if (logStream == null)
                CreateLog (DEFAULT_FILENAME);
            logStream.WriteLine ("[" + DateTime.Now.ToString () + "] " + msg);
        } else
            Debug.Log ("Log is inactive. Message: "+ msg);
    }
}
