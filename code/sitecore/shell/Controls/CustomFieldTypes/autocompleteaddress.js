"use strict"

var autocomplete = null;
var txtAutocomplete = null;
var txtAddressLine1 = null;
var txtAddressLine2 = null;
var txtLatitude = null;
var txtLongitude = null;	

window.autoCompleteAddress = {
    
    // Initialize autocomplete
    initAutocomplete: function (txtAutoCompleteId, txtAddressLine1Id, txtAddressLine2Id, txtLatitudeId, txtLongitudeId) {
        txtAutocomplete = document.getElementById(txtAutoCompleteId);
        txtAddressLine1 = document.getElementById(txtAddressLine1Id);
        txtAddressLine2 = document.getElementById(txtAddressLine2Id);
        txtAutocomplete = document.getElementById(txtAutoCompleteId);
        txtLatitude = document.getElementById(txtLatitudeId);
        txtLongitude = document.getElementById(txtLongitudeId);

        if (txtAutocomplete) {
            var options = {
                componentRestrictions: {
                    country: 'au',
                }
            };

            autocomplete = new google.maps.places.Autocomplete(txtAutocomplete, options);

            google.maps.event.clearInstanceListeners(txtAutoCompleteId);

            autocomplete.addListener('place_changed', function () {

                var place = autocomplete.getPlace();

                if (place.formatted_address != undefined) {
                    autoCompleteAddress.setAddressLines(place.formatted_address);
                }
                if (place.geometry != undefined) {
                    autoCompleteAddress.setLatitudeLongitude(autoCompleteAddress.setFixedLength(place.geometry.location.lat()), autoCompleteAddress.setFixedLength(place.geometry.location.lng()));
                }
            });

            txtAutocomplete.addEventListener('change', function () {
                autoCompleteAddress.setAddressLines('');
                autoCompleteAddress.setLatitudeLongitude('', '');
            });
        }
    },

    // Set address lines
    setAddressLines: function (formattedAddress) {
        try {
            const adjustedAddress = formattedAddress.replace(', Australia', '');
            const indexOfComma = adjustedAddress.indexOf(',');
            const isMultiLine = indexOfComma >= 0;
            txtAddressLine1.value = isMultiLine ? adjustedAddress.substring(0, indexOfComma) : adjustedAddress;
            txtAddressLine2.value = isMultiLine ? adjustedAddress.substring(indexOfComma + 1, adjustedAddress.length).trim() : '';
        }
        catch (e) {
        }
    },

    // Set latitude and longitude
    setLatitudeLongitude: function (latitude, longitude) {
        try {
            txtLatitude.value = latitude;
            txtLongitude.value = longitude;
        }
        catch (e) {
        }
    },

    // Set coordinate length to 6
    setFixedLength: function (coordinate) {
        try {
            var decimalplaces = coordinate.toString().split(".");
            if (decimalplaces != null && decimalplaces.length > 1 && decimalplaces[1].length > 5) {
                coordinate = coordinate.toFixed(6);
            }
        }
        catch (e) {
        }
        return coordinate;
    }
}