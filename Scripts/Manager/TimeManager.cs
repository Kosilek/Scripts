using DredPack;
using Kosilek.Ad;
using Kosilek.SaveAndLoad;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;


public class TimeManager : SimpleSingleton<TimeManager>, ISaveble
{
    public bool isNetwork = false;

    private DateTime nowDateTime;
        
    #region InitializeName
    public void InitializeLoadData()
    {
        nowDateTime = DateTime.Now;
        string[] dateTime = Load(Path.DefaultPath + Path.DateTime);
        InitializeDateTime(dateTime);
    }

    private void InitializeDateTime(string[] dateTime)
    {
        try
        {
            var hasNameData = dateTime.Length > 0;
            Action<string[]> action = hasNameData ? InitializeDateTimeLoad : InitializeDateTimeFirst;
            action.Invoke(dateTime);
        }
        catch
        {
            Debug.LogError("Error: Data could not be watered, InitializeDateTime");
        }
    }

    private void InitializeDateTimeFirst(string[] dateTime)
    {
        AdManager.Instance.isNoAd = false;
    }

    private void InitializeDateTimeLoad(string[] dateTime)
    {
        var dateTimeLoad = ConvertingAStringToADate(dateTime[0]);

        if (nowDateTime > dateTimeLoad)
        {
            AdManager.Instance.isNoAd = false;
            Save(true, Path.DateTime, null);
        }
        else
        {
            AdManager.Instance.isNoAd = true;
        }
    }

    public DateTime ConvertingAStringToADate(string date)
    {
        DateTime dateTime = ConvertDateStringToDateTime(date);
        return dateTime;
    }

    private DateTime ConvertDateStringToDateTime(string dateString)
    {
        string format = "dd.MM.yyyy";
        return DateTime.ParseExact(dateString, format, CultureInfo.InvariantCulture);
    }

    #endregion endInitializeName  

    #region Save
    #region Save
    public void Save(string path)
    {
        throw new System.NotImplementedException();
    }

    public void Save(bool isNewData, string fileName, string fileData)
    {
        SaveManager.Instance.multiThread.SaveData(SaveManager.Instance.saveSystem.WritingDataToADocument, SaveTaskType.OtherData, isNewData, fileName, fileData);
    }

    public string[] Load(string path)
    {
        try
        {
            return SaveManager.Instance.loadSystem.LoadData(path);
        }
        catch
        {
            Debug.LogError($"Error: Data could not be downloaded from the file. File path: {path}");
            return null;
        }
    }
    #endregion Save
    #endregion endSave
}
