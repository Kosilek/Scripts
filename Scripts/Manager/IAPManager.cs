using DredPack;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Kosilek.SaveAndLoad;
using Unity.Mathematics;
using Kosilek.Ad;

namespace Kosilek.Manager
{
    public class IAPManager : SimpleSingleton<IAPManager>
    {
        public void BuyNoAds(int day)
        {
            int daysFromToday = day;
            DateTime today = DateTime.Today;
            DateTime dateTime = today.AddDays(daysFromToday);
            string dayZ = "";
            string mouth = "";
            var a = dateTime.Day.ToString().Length == 1 ? dayZ = "0" + dateTime.Day.ToString() : dayZ = dateTime.Day.ToString();
            a = dateTime.Month.ToString().Length == 1 ? mouth = "0" + dateTime.Month.ToString() : mouth = dateTime.Month.ToString();

            string[] date = TimeManager.Instance.Load(Path.DefaultPath + Path.DateTime);
            Action<string[], string, string, string, int> action = date.Length > 0 ? ActionDateLoad : ActionDateNew;
            AdManager.Instance.isNoAd = true;
            action.Invoke(date, dayZ, mouth, dateTime.Year.ToString(), day);
            var count = SwitchCount(day);
            MoneyManager.Instance.SetMoney(count);
        }

        private int SwitchCount(int day)
        {
            switch(day)
            {
                case 30:
                    return 10000;
                case 90:
                    return 50000;
                case 182:
                    return 150000;
                case 365:
                    return 500000;
                default: return -1;
            }
        }

        private void ActionDateNew(string[] date, string dayZ, string mouth, string year, int day)
        {
            TimeManager.Instance.Save(true, Path.DateTime, dayZ + "." + mouth + "." + year);
        }

        private void ActionDateLoad(string[] date, string dayZ, string mouth, string year, int day)
        {
            var dataLoad = TimeManager.Instance.ConvertingAStringToADate(date[0]);
            DateTime newDataTime = dataLoad.AddDays(day);
            string dayX = "";
            string mouthX = "";
            var a = newDataTime.Day.ToString().Length == 1 ? dayX = "0" + newDataTime.Day.ToString() : dayX = newDataTime.Day.ToString();
            a = newDataTime.Month.ToString().Length == 1 ? mouthX = "0" + newDataTime.Month.ToString() : mouthX = newDataTime.Month.ToString();
            TimeManager.Instance.Save(true, Path.DateTime, dayX + "." + mouthX + "." + newDataTime.Year.ToString());
        }

        public void BuyMoney(int count)
        {
            MoneyManager.Instance.SetMoney(count);
        }
    }
}