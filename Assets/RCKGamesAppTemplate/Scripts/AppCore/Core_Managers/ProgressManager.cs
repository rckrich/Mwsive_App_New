using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgressManager : Manager
{
    private static ProgressManager self;

    public static ProgressManager instance
    {
        get
        {
            if (self == null)
            {
                self = (ProgressManager)GameObject.FindObjectOfType(typeof(ProgressManager));
            }
            return self;
        }
    }

    public static ProgressManager dontDestroyProgressM;

    public AppProgress progress;

    public void save()
    {
        FileManager.saveProgress(this.progress);
    }

    public void DeleteCache()
    {
        AppProgress freshAppProgress = new AppProgress();
        progress = freshAppProgress;
        save();
    }

    // Use this for initialization
    void Awake()
    {
        if (dontDestroyProgressM == null)
        {
            DontDestroyOnLoad(gameObject);
            dontDestroyProgressM = this;
        }
        else if (dontDestroyProgressM != this)
        {
            Destroy(gameObject);
        }

        initApp();
    }

    // Update is called once per frame
    private void initApp()
    {
        AppProgress loadedProgress = FileManager.loadProgress();
        if (loadedProgress == null)
        {
            Debug.Log("No se ha encontrado los datos de la app, inicializando datos de la app...");
            FileManager.saveProgress(this.progress);
#if UNITY_IOS
            FileManager.setNoBackUpIOS();
#endif
        }
        else
        {
            this.progress = loadedProgress;
        }

    }

}
