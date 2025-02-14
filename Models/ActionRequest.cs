﻿using System.Text.Json.Nodes;
using FlaUI.Core.WindowsAPI;
using WindowsAutomationPlugin.Models.Enums;

namespace WindowsAutomationPlugin.Models
{
    public class ActionRequest
    {
        public Actions Action;
        public string ActionValue;
        public By By;
        public string LocatorValue;
        public VirtualKeyShort[] Keys;
        public ActionRequest() { }
        public ActionRequest(JsonObject json)
        {
            ActionRequest req = Newtonsoft.Json.JsonConvert.DeserializeObject<ActionRequest>(json.ToJsonString());
            this.Action = req.Action;
            this.ActionValue = req.ActionValue;
            this.By = req.By;
            this.LocatorValue = req.LocatorValue;
            this.Keys = req.Keys;
        }
        public string ToString()
        {
            return string.Format("Action: {0}.\nActionValue: {1}.\nBy: {2}.\nLocatorValue: {3}", Action, ActionValue, By, LocatorValue);
        }
    }
}
