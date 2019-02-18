using Sitecore.Web.UI.HtmlControls;
using System;
using System.Collections.Specialized;
using System.Web;
using System.Web.UI.HtmlControls;

namespace Sitecore.Foundation.CustomFields.Fields
{
    /// <summary>
    /// Copied from https://www.sitecoregabe.com/2018/07/custom-sitecore-field-google-maps.html
    /// and then tweaked.
    /// </summary>
    public class GoogleMapsAutocompleteAddress : Control
    {
        private enum ControlsField
        {
            Address,
            Latitude,
            Longitude,
            AddressLine1,
            AddressLine2
        }

        /// <summary>
        /// To get ID of auto complete address control
        /// </summary>
        public string TextAutocompleteId => GetID("textautocomplete");

        /// <summary>
        /// To get ID of latitude control
        /// </summary>
        public string TextLatitudeId => GetID("textlatitude");

        /// <summary>
        /// To get ID of longitude control
        /// </summary>
        public string TextLongitudeId => GetID("textlongitude");

        public string TextAddressLine1Id => GetID("textaddressline1");

        public string TextAddressLine2Id => GetID("textaddressline2");

        public const string AddressLine1 = "Address Line 1";
        public const string AddressLine2 = "Address Line 2";
        public const string Latitude = "Latitude";
        public const string Longitude = "Longitude";
        public const string TagnameDiv = "div";
        public const string TagnameInput = "input";
        public const string CssDiv = "scEditorFieldLabel";
        public const string CssInput = "scContentControl";

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
        }

        protected override void OnLoad(EventArgs e)
        {
            if (!Sitecore.Context.ClientPage.IsEvent)
            { CreateControls(); }
            else { SetValues(); }

            base.OnLoad(e);
        }

        /// <summary>
        /// This method is used to create Control
        /// </summary>
        protected void CreateControls()
        {
            var divAutoCompleteAddress = GetHtmlGenericControl(TagnameDiv, string.Empty, "divAutoCompleteAddress", true);
            var textAutocomplete = GetHtmlGenericControl(TagnameInput, string.Empty, TextAutocompleteId);
            var divAddresLine1 = GetHtmlGenericControl(TagnameDiv, AddressLine1, "divAddressLine1", true);
            var textAddressLine1 = GetHtmlGenericControl(TagnameInput, string.Empty, TextAddressLine1Id);
            var divAddresLine2 = GetHtmlGenericControl(TagnameDiv, AddressLine2, "divAddressLine2", true);
            var textAddressLine2 = GetHtmlGenericControl(TagnameInput, string.Empty, TextAddressLine2Id);
            var divLatitude = GetHtmlGenericControl(TagnameDiv, Latitude, "divLatitude", true);
            var textLatitude = GetHtmlGenericControl(TagnameInput, string.Empty, TextLatitudeId);
            var divLongitude = GetHtmlGenericControl(TagnameDiv, Longitude, "divLongitude", true);
            var textLongitude = GetHtmlGenericControl(TagnameInput, string.Empty, TextLongitudeId);

            // Add attribute to invoke JS function
            textAutocomplete.Attributes.Add("onfocus", "javascript:autoCompleteAddress.initAutocomplete('" + TextAutocompleteId + "','" + TextAddressLine1Id + "','" + TextAddressLine2Id + "','" + TextLatitudeId + "','" + TextLongitudeId + "');");
            textAutocomplete.Attributes.Add("placeholder", "Enter a location");

            // Add CSS
            divAddresLine1.Attributes["class"] = divAddresLine2.Attributes["class"] = divLatitude.Attributes["class"] = divLongitude.Attributes["class"] = CssDiv;
            divAddresLine1.Attributes["style"] = divAddresLine2.Attributes["style"] = divLatitude.Attributes["style"] = divLongitude.Attributes["style"] = "padding-top: 15px;";
            textAddressLine1.Attributes["class"] = textAddressLine2.Attributes["class"] = textLongitude.Attributes["class"] = textLatitude.Attributes["class"] = textAutocomplete.Attributes["class"] = CssInput;

            // Add Value
            if (!string.IsNullOrEmpty(Value))
            {
                var data = HttpUtility.ParseQueryString(Value);
                // ReSharper disable once ConditionIsAlwaysTrueOrFalse
                if (data != null)
                {
                    //textAutocomplete.Attributes["value"] = data[ControlsField.Address.ToString()];
                    textAddressLine1.Attributes["value"] = data[ControlsField.AddressLine1.ToString()];
                    textAddressLine2.Attributes["value"] = data[ControlsField.AddressLine2.ToString()];
                    textLatitude.Attributes["value"] = data[ControlsField.Latitude.ToString()];
                    textLongitude.Attributes["value"] = data[ControlsField.Longitude.ToString()];
                }
            }

            // Add Controls
            divAutoCompleteAddress.Controls.Add(textAutocomplete);
            divAutoCompleteAddress.Controls.Add(divAddresLine1);
            divAutoCompleteAddress.Controls.Add(textAddressLine1);
            divAutoCompleteAddress.Controls.Add(divAddresLine2);
            divAutoCompleteAddress.Controls.Add(textAddressLine2);
            divAutoCompleteAddress.Controls.Add(divLatitude);
            divAutoCompleteAddress.Controls.Add(textLatitude);
            divAutoCompleteAddress.Controls.Add(divLongitude);
            divAutoCompleteAddress.Controls.Add(textLongitude);

            Controls.Add(divAutoCompleteAddress);
        }

        /// <summary>
        /// This method is used to set values to Value Property
        /// </summary>
        public void SetValues()
        {
            //var autocompleteAddress = Context.Request.Form[TextAutocompleteId] ?? string.Empty;
            var addressLine1 = Context.Request.Form[TextAddressLine1Id] ?? string.Empty;
            var addressLine2 = Context.Request.Form[TextAddressLine2Id] ?? string.Empty;
            var latitude = Context.Request.Form[TextLatitudeId] ?? string.Empty;
            var longitude = Context.Request.Form[TextLongitudeId] ?? string.Empty;

            var allValues = new NameValueCollection
            {
                //{ ControlsField.Address.ToString(), autocompleteAddress },
                { ControlsField.AddressLine1.ToString(), HttpUtility.UrlEncode(addressLine1) },
                { ControlsField.AddressLine2.ToString(), HttpUtility.UrlEncode(addressLine2) },
                { ControlsField.Latitude.ToString(), latitude },
                { ControlsField.Longitude.ToString(), longitude }
            };

            var combinedValue = StringUtil.NameValuesToString(allValues, "&");

            Sitecore.Context.ClientPage.Modified = (Value != combinedValue);
            Value = combinedValue;
        }

        /// <summary>
        /// This method is used to get a new HtmlGenericControl
        /// </summary>
        /// <param name="tagName">It is a string type of parameter that holds Tag Name</param>
        /// <param name="innerText">It is a string type of parameter that holds Inner Text</param>
        /// <param name="controlId">It is a string type of parameter that holds Control ID</param>
        /// <param name="getControlId">It is a bool type of parameter that True when control ID is to be Get</param>
        /// <returns>It returns HtmlGenericControl</returns>
        public HtmlGenericControl GetHtmlGenericControl(string tagName, string innerText, string controlId, bool getControlId = false)
        {
            return new HtmlGenericControl
            {
                TagName = tagName,
                InnerText = innerText,
                ID = getControlId ? GetID(controlId) : controlId
            };
        }
    }
}