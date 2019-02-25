using System.Collections.Generic;

namespace VM.Platform.TestAutomationFramework.Core.Contracts
{
    using System;

    public interface IUiAdapter
    {
        string CurrentPageTitle { get; }
        void GoToUrl(Uri url);
        void CloseCurrentBrowser();
        void WaitForLoad(ControlDefinition controlDefinition);
        void Quit();
        void TakeScreenshot();
        void CloseAndReturn();
        void SetControlValue(ControlDefinition controlDefinition, string value);
        void SetControlValue(string key, string value);        
        void ClickElement(ControlDefinition controlDefinition);
        void DoubleClickElement(ControlDefinition controlDefinition);
        void HoverElement(ControlDefinition controlDefinition, string value);
        void SendKeyboadKeys(ControlDefinition controlDefinition, string value);
        string GetValue(string controlId, string property);
        void SetDropDownValue(ControlDefinition controlDefinition, 
            string value);
        void SetDropDownByIndex(ControlDefinition controlDefinition, int indexValue);
        void DriverWaitTime(long timeInseconds);
        void BackNavigation();
        string GetElementValue(ControlDefinition controlDefinition);
        string GetValueFromDB(string query);
        bool IsElementPresent(ControlDefinition controlDefinition);
        bool IsElementDisplay(ControlDefinition controlDefinition);
        bool IsSelectableInput(ControlDefinition controlDefinition);
        bool IsSelectedButtons(ControlDefinition controlDefinition);
        void WaitForElement(ControlDefinition controlDefinition);
        void WaitForAjaxCallComplete(ControlDefinition controlDefinition);
        void WaitForElementToBeClickable(ControlDefinition controlDefinition);
        void WaitForOptionToBeSelectable(ControlDefinition controlDefinition, string option);
        void WaitForElementToBeSelectable(ControlDefinition controlDefinition);
        void ClickUsingJavascript(ControlDefinition controlDefinition);
        void ClickElementAt(ControlDefinition controlDefinition, ClickPoint clickPoint);
        string GetRetentionKey(ControlDefinition controlDefinition);
        string GetDebugInfo(ControlDefinition controlDefinition);
        string GetAttributeValue(ControlDefinition controlDefinition, string attributeName);
        IEnumerable<string> GetDropDownOptions(ControlDefinition controlDefinition);
        string GetTableCellValue(ControlDefinition controlDefinition, int row, int column);
        //void ClickTableCell(ControlDefinition controlDefinition, int row, int column);
        void ClickTableCell(ControlDefinition controlDefinition, int row, int column, string objectName);
        bool FindElementEnableStatus(ControlDefinition controlDefinition);
        IEnumerable<string> GetTableColumn(ControlDefinition controlDefinition, int columnNumber);
        int GetTableRowCount(ControlDefinition controlDefinition);
        void CloseAlert(bool shouldAccept);
        int GetNumberOfOptions(ControlDefinition controlDefinition);
        void WaitExplicitly(TimeSpan duration);
        bool IsSelected(ControlDefinition controlDefinition);
        void SwitchToIFrame(ControlDefinition controlDefinition);
        void SwitchToIFrame(ControlDefinition parent, ControlDefinition child);
        void SwitchToDefaultContent();
        void SwitchToLastWindow();
        void SwitchToFirstWindow();
        void SwitchBrowser(string title);
        void SwitchToAlert();
        string GetTableValue(ControlDefinition ctrlDefinition);
       


    }
}
