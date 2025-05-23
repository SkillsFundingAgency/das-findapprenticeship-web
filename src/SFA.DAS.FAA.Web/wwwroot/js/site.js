﻿const locationInputs = document.querySelectorAll(".faa-location-autocomplete");
const apiUrl = "/locations";

if (locationInputs.length > 0) {
  for (let i = 0; i < locationInputs.length; i++) {
    const input = locationInputs[i];
    const container = document.createElement("div");
    const withinSelect = document.getElementById("within");

    container.className = "das-autocomplete-wrap";
    container.dataset.trackUserSelected = input.dataset.trackUserSelected;
    input.parentNode.replaceChild(container, input);

    const getSuggestions = async (query, updateResults) => {
      const results = [];
      var xhr = new XMLHttpRequest();
      xhr.onreadystatechange = function () {
        if (xhr.readyState === 4) {
          let results = JSON.parse(xhr.responseText);
          results = results.locations.locations.map(function (r) {
            return r.name;
          });
          updateResults(results);
        }
      };
      xhr.open("GET", `${apiUrl}?searchTerm=${query}`, true);
      xhr.send();
    };

    accessibleAutocomplete({
      element: container,
      id: input.id,
      name: input.name,
      defaultValue: input.value,
      displayMenu: "overlay",
      showNoOptionsFound: false,
      minLength: 2,
      source: getSuggestions,
      placeholder: "",
      confirmOnBlur: false,
      autoselect: true,
      onConfirm: () => {
        const trackSelection = input.dataset.trackUserSelected;
        if (trackSelection) {
          const hiddenField = document.getElementById(trackSelection);
          if (hiddenField) {
            hiddenField.value = "true";
          }
        }
        if (withinSelect) {
          if (withinSelect.value === "all") {
            withinSelect.value = "10";
          }
        }
      },
    });
  }

  const autocompleteInputs = document.querySelectorAll(".autocomplete__input");
  if (autocompleteInputs.length > 0) {
    for (let i = 0; i < autocompleteInputs.length; i++) {
      const autocompleteInput = autocompleteInputs[i];
      autocompleteInput.setAttribute("autocomplete", "new-password");
    }
  }
}

const sortSelect = document.getElementById("sort-results");

if (sortSelect) {
  sortSelect.addEventListener("change", (e) => {
    const sortValue = e.target.value;
    const queryString = window.location.search;
    const params = new URLSearchParams(queryString);
    params.delete("sort");
    params.append("sort", sortValue);
    document.location.href = "?" + params.toString();
  });
}

const printLinks = document.querySelectorAll(
  ".faa-vacancy-actions__link--print"
);

if (printLinks.length > 0) {
  for (let i = 0; i < printLinks.length; i++) {
    printLinks[i].addEventListener("click", (e) => {
      e.preventDefault();
      window.print();
    });
  }
}

const forms = document.querySelectorAll("main form:not(.faa-do-not-disable)");

if (forms.length > 0) {
  for (let i = 0; i < forms.length; i++) {
    const form = forms[i];
    const button = form.querySelector(".govuk-button");

    form.addEventListener("submit", () => {
      if (button) {
        button.setAttribute("disabled", "disabled");
        setTimeout(function () {
          button.removeAttribute("disabled");
        }, 5000);
      }
    });
  }
}

const jsBackLink = document.querySelector(".faa-js-back-link");

if (jsBackLink) {
  const backLink = document.createElement("a");
  const backLinkText = document.createTextNode("Back");
  backLink.appendChild(backLinkText);
  backLink.className = "govuk-back-link";
  backLink.href = "#";
  backLink.addEventListener("click", (e) => {
    e.preventDefault();
    window.history.back();
  });
  jsBackLink.parentNode.replaceChild(backLink, jsBackLink);
}

const jsBackLinkHistory = document.querySelector(".faa-js-back-link-history");

if (jsBackLinkHistory) {
  const referrer = document.referrer;
  const backLink = document.createElement("a");
  const backLinkText = document.createTextNode("Back");
  backLink.appendChild(backLinkText);
  backLink.className = "govuk-back-link";
  backLink.href = "#";
  backLink.addEventListener("click", (e) => {
    e.preventDefault();
    window.history.back();
  });

  if (referrer && referrer !== document.location.href) {
    jsBackLinkHistory.parentNode.replaceChild(backLink, jsBackLinkHistory);
  }
}

const jsSelectChangeSubmitForm = document.querySelector(
  ".faa-js-select-change-submit-form"
);

if (jsSelectChangeSubmitForm) {
  jsSelectChangeSubmitForm.addEventListener("change", () => {
    jsSelectChangeSubmitForm.closest("form").submit();
  });
}

// Save to favourites

function Favourites(container) {
  this.container = container;
  this.addLink = this.container.querySelector("[data-add-favourite]");
  this.deleteLink = this.container.querySelector("[data-delete-favourite]");
}

Favourites.prototype.init = function () {
  if (!this.addLink || !this.deleteLink) {
    return;
  }
  this.addLink.addEventListener("click", async (e) => {
    e.preventDefault();
    await this.submit(this.addLink, "add");
    this.deleteLink.focus();
  });
  this.deleteLink.addEventListener("click", async (e) => {
    e.preventDefault();
    await this.submit(this.deleteLink, "delete");
    this.addLink.focus();
  });
};

Favourites.prototype.submit = async function (link, action) {
  const url = link.parentElement.action + "?redirect=false";
  const headers = new Headers();
  headers.append("Content-Type", "application/json");
  headers.append("X-Requested-With", "XMLHttpRequest");
  headers.append("RequestVerificationToken", link.nextElementSibling.value);
  await fetch(url, {
    method: "POST",
    headers: headers,
    body: JSON.stringify({
      __RequestVerificationToken: link.nextElementSibling.value,
    }),
  })
    .then((response) => {
      if (response.ok) {
        return response.json();
      }
      throw new Error("Something went wrong");
    })
    .then((data) => {
      this.updateUI(action);
    })
    .catch((error) => {
      console.error("Error: ", error);
    });
};

Favourites.prototype.updateUI = function (action) {
  if (action === "add") {
    this.addLink.ariaHidden = true;
    this.deleteLink.ariaHidden = false;
    this.container.classList.add("faa-save-vacancy--saved");
  } else {
    this.addLink.ariaHidden = false;
    this.deleteLink.ariaHidden = true;
    this.container.classList.remove("faa-save-vacancy--saved");
  }
};

const addToFavourites = document.querySelectorAll("[data-favourite]");

if (addToFavourites) {
  addToFavourites.forEach(function (container) {
    new Favourites(container).init();
  });
}

// Save search alert

function Alerts(container) {
  this.container = container;
  this.createForm = this.container.querySelector("[data-alert-create]");
  this.confirmation = this.container.querySelector("[data-alert-confirmation]");
  this.noResults = !!document.getElementById("faa-no-results-alert");
}

Alerts.prototype.init = function () {
  if (!this.createForm || !this.confirmation) {
    return;
  }
  this.createForm.addEventListener("submit", async (e) => {
    e.preventDefault();
    await this.submitForm();
  });
  if (this.noResults) this.extraEvents();
};

Alerts.prototype.extraEvents = function () {
  this.nrContainer = document.getElementById("faa-no-results-alert");
  this.nrCreateForm = document.getElementById("faa-no-results-alert--create");
  this.nrConfirmation = document.getElementById(
    "faa-no-results-alert--created"
  );
  this.nrCreateForm.addEventListener("submit", async (e) => {
    e.preventDefault();
    await this.submitForm();
  });
};

Alerts.prototype.submitForm = async function (button) {
  const requestValidationToken = this.createForm.querySelector(
    "input[name=__RequestVerificationToken]"
  ).value;
  const url = this.createForm.action + "?redirect=false";
  const headers = new Headers();
  headers.append("X-Requested-With", "XMLHttpRequest");
  headers.append("RequestVerificationToken", requestValidationToken);

  const formData = new FormData();
  formData.append("__RequestVerificationToken", requestValidationToken);
  formData.append(
    "Data",
    this.createForm.querySelector("input[name=Data]").value
  );

  await fetch(url, {
    method: "POST",
    headers: headers,
    body: formData,
  })
    .then((response) => {
      if (response.ok) {
        return response.json();
      }
      throw new Error("Something went wrong");
    })
    .then((data) => {
      this.updateUI();
    })
    .catch((error) => {
      console.error("Error: ", error);
    });
};

Alerts.prototype.updateUI = function () {
  this.container.classList.add("faa-filter-alert--saved");
  this.createForm.ariaHidden = true;
  this.confirmation.ariaHidden = false;
  if (this.noResults) {
    this.nrContainer.classList.add("faa-filter-alert--saved");
    this.nrCreateForm.ariaHidden = true;
    this.nrConfirmation.ariaHidden = false;
  }
};

const createAlert = document.querySelectorAll("[data-alert]");

if (createAlert) {
  createAlert.forEach(function (container) {
    new Alerts(container).init();
  });
}

// Show/Hide Extra Form Fields

function ExtraFieldRows(container) {
  this.container = container;
  this.fieldset = this.container.querySelector(".faa-extra-fields__fieldset");
  this.fieldsetHiddenClass = "faa-extra-fields__fieldset--all-hidden";
  this.extraFieldRows = this.fieldset.querySelectorAll(
    ".faa-extra-fields__form-group"
  );
  this.hiddenClass = "faa-extra-field__form-group--hidden";
  this.addButtonText = this.container.dataset.addButtonText || "Add another";
  this.fieldset.classList.add("faa-extra-fields__form-group--loaded");
  this.maxMessage = this.container.querySelector(
    "#faa-extra-fields-max-message"
  );
}

ExtraFieldRows.prototype.init = function () {
  this.insertAddLink();
  // Append the remove links
  for (let f = 0; f < this.extraFieldRows.length; f++) {
    const extraFieldRow = this.extraFieldRows[f];
    this.appendRemoveLink(extraFieldRow, f);
  }

  // If all rows are hidden, show the first row
  const hiddenRowCount = this.showHideEmptyRows();
  if (hiddenRowCount === this.extraFieldRows.length) {
    this.showRow(this.extraFieldRows[0]);
  }
  if (hiddenRowCount === 0) {
    this.addLink.classList.add(this.hiddenClass);
    this.maxMessage.classList.remove(this.hiddenClass);
  }
};

ExtraFieldRows.prototype.showHideEmptyRows = function () {
  let hiddenRowCount = 0;
  for (let f = 0; f < this.extraFieldRows.length; f++) {
    const extraFieldRow = this.extraFieldRows[f];
    const inputs = extraFieldRow.querySelectorAll("input:not([type=hidden])");
    const errorMessages = extraFieldRow.querySelectorAll(
      ".govuk-form-group--error"
    );
    let areAllFieldsEmpty = true;
    inputs.forEach((input) => {
      if (input.type === "text" && input.value.length > 0) {
        areAllFieldsEmpty = false;
      }
      if (input.type === "checkbox" && input.checked) {
        areAllFieldsEmpty = false;
      }
    });
    if (errorMessages.length > 0) {
      areAllFieldsEmpty = false;
    }
    if (areAllFieldsEmpty) {
      this.hideRow(extraFieldRow);
      hiddenRowCount++;
    } else {
      this.showRow(extraFieldRow, false);
    }
  }
  return hiddenRowCount;
};

ExtraFieldRows.prototype.insertAddLink = function () {
  const addRowLink = document.createElement("a");
  addRowLink.innerHTML = this.addButtonText;
  addRowLink.className =
    "govuk-button govuk-button--secondary faa-extra-field__form-group-link--add";
  addRowLink.href = "#";
  addRowLink.addEventListener("click", this.showFirstAvailableRow.bind(this));
  this.addLink = addRowLink;
  this.fieldset.parentNode.insertBefore(
    this.addLink,
    this.fieldset.nextSibling
  );
};

ExtraFieldRows.prototype.appendRemoveLink = function (row, index) {
  const that = this;
  const removeLinkWrap = document.createElement("span");
  removeLinkWrap.className = "faa-extra-field__form-group-link--remove-wrap";
  const removeLink = document.createElement("a");
  removeLink.innerHTML = "Remove";
  removeLink.className = `govuk-button govuk-button--secondary faa-extra-field__form-group-link--remove ${
    index === 0 ? "faa-visibly-hidden" : ""
  }`;
  removeLink.href = "#";
  removeLink.addEventListener("click", function (e) {
    e.preventDefault();
    that.addLink.classList.remove(that.hiddenClass);
    that.maxMessage.classList.add(that.hiddenClass);
    that.hideRow(row);
    that.updateRowOrder();
  });

  removeLinkWrap.append(removeLink);
  row.append(removeLinkWrap);
};

ExtraFieldRows.prototype.updateRowOrder = function () {};

ExtraFieldRows.prototype.showFirstAvailableRow = function (e) {
  let hiddenRowCount = 0;
  let rowToShow;
  for (let f = 0; f < this.extraFieldRows.length; f++) {
    const extraFieldRow = this.extraFieldRows[f];
    if (extraFieldRow.classList.contains(this.hiddenClass)) {
      if (hiddenRowCount === 0) {
        rowToShow = extraFieldRow;
      }
      hiddenRowCount++;
    }
  }
  if (hiddenRowCount < 2) {
    this.addLink.classList.add(this.hiddenClass);
    this.maxMessage.classList.remove(this.hiddenClass);
  }
  this.showRow(rowToShow, true);
  e.preventDefault();
};

ExtraFieldRows.prototype.hideRow = function (row) {
  let deleteField;
  const inputs = row.querySelectorAll("input");
  inputs.forEach((input) => {
    if (
      input.type === "hidden" &&
      (input.value === "" || input.value === "false")
    ) {
      deleteField = input;
    }
    if (input.type === "text") {
      input.value = "";
    }
    if (input.type === "checkbox") {
      input.checked = false;
    }
  });
  if (deleteField) {
    deleteField.value = "true";
  }
  row.classList.add(this.hiddenClass);
};

ExtraFieldRows.prototype.showRow = function (row, focus = false) {
  if (row === undefined) {
    return;
  }
  const textInput = row.querySelector("input");
  const hiddenInput = row.querySelector("[data-type-remove]");
  hiddenInput.value = "false";
  this.fieldset.classList.remove(this.fieldsetHiddenClass);
  row.classList.remove(this.hiddenClass);
  if (focus) {
    textInput.focus();
  }
};

ExtraFieldRows.prototype.areAllRowsHidden = function () {
  let hiddenRowCount = 0;
  for (let f = 0; f < this.extraFieldRows.length; f++) {
    const extraFieldRow = this.extraFieldRows[f];
    if (extraFieldRow.classList.contains(this.hiddenClass)) {
      hiddenRowCount++;
    }
  }
  return hiddenRowCount === this.extraFieldRows.length;
};

function Autocomplete(select) {
  this.select = select;
  this.selectId = this.select.id;
}

Autocomplete.prototype.convertId = function (id) {
  const replaceSqL = id.replace(/[[]/g, "\\[");
  return `#${replaceSqL.replace(/]/g, "\\]")}`;
};

Autocomplete.prototype.init = function () {
  accessibleAutocomplete.enhanceSelectElement({
    selectElement: this.select,
    minLength: 1,
    defaultValue: "",
    displayMenu: "overlay",
    placeholder: "",
    onConfirm: (opt) => {
      const txtInput = document.getElementById(this.selectId);
      const searchString = opt || txtInput?.value || "";
      const requestedOption = [].filter.call(
        this.select.options,
        function (option) {
          return (option.textContent || option.innerText) === searchString;
        }
      )[0];
      if (requestedOption) {
        requestedOption.selected = true;
      } else {
        this.select.selectedIndex = 0;
      }
    },
  });
};

// Extra field row

const extraFieldRows = document.querySelectorAll("[data-extra-field-rows]");

if (extraFieldRows) {
  extraFieldRows.forEach(function (extraFieldRows) {
    new ExtraFieldRows(extraFieldRows).init();
  });
}

// Autocomplete
const autocompleteSelects = document.querySelectorAll("[data-autocomplete]");

if (autocompleteSelects) {
  autocompleteSelects.forEach(function (select) {
    new Autocomplete(select).init();
  });
}

// Maps

function FaaMapDirections(mapId, lng, lat, ptcd, mapLocations, form, container) {
  this.mapId = mapId;
  this.container = container;
  this.form = form;
  this.multiLocations = mapLocations;
  this.location = { lat, lng }
  this.ptcd = ptcd;
}

FaaMapDirections.prototype.init = async function () {
  const { Map, InfoWindow } = await google.maps.importLibrary("maps");
  const { AdvancedMarkerElement, PinElement } = await google.maps.importLibrary(
    "marker"
  );
  const directionsService = new google.maps.DirectionsService();
  const directionsRenderer = new google.maps.DirectionsRenderer();

  console.log(this.location)

  if (this.multiLocations.length > 0) {
    this.multiLocationsDropdown();
  }

  if ("geolocation" in navigator) {
    const useLocationLink = document.createElement("a");
    useLocationLink.classList.add("govuk-link");
    useLocationLink.classList.add("govuk-link--no-visited-state");
    useLocationLink.innerHTML = "use current location";
    useLocationLink.setAttribute("href", "#");
    useLocationLink.addEventListener("click", (e) => {
      e.preventDefault();
      this.getCurrentPostcode();
    });
    document.getElementById("faa-navigator-link").append(useLocationLink);
  }

  if (this.centerLat === 0 && this.centerLng === 0) {
    await this.updateLonLatFromPostcode();
  }

  this.destination = new google.maps.LatLng(this.location);

  this.map = new google.maps.Map(this.container, {
    zoom: 10,
    center: this.location,
    mapId: this.mapId,
  });

  this.map.markers = [];

  if (this.multiLocations.length === 0) {
    const vacancyLocationPin = new google.maps.marker.AdvancedMarkerElement({
      map: this.map,
      position: this.location,
      content: this.markerHtml(),
    });
    this.map.markers.push(vacancyLocationPin);
  } else {
    const bounds = new google.maps.LatLngBounds();
    for (const location of this.multiLocations) {
      const vacancyLocationPin = new google.maps.marker.AdvancedMarkerElement({
        map: this.map,
        position: {lat: location.lat, lng: location.lon},
        content: this.markerHtml(),
      });
      this.map.markers.push(vacancyLocationPin);
      bounds.extend(vacancyLocationPin.position);
    }
    this.map.fitBounds(bounds);
  }

  directionsRenderer.setMap(this.map);
  this.calculateAndDisplayRoute(directionsRenderer, directionsService);

  this.form.addEventListener("submit", (e) => {
    e.preventDefault();
    this.map.markers.forEach((marker) => {
        marker.setMap(null);
    });
    this.origin = document.getElementById("directions-postcode").value;
    this.travelMode = document.getElementById("directions-travelMode").value;
    if (this.isPostcodeValid()) {
      this.calculateAndDisplayRoute(directionsRenderer, directionsService);
    }
  });
};

FaaMapDirections.prototype.multiLocationsDropdown = function () {
  const formRow = document.getElementById("directions-locations");
  formRow.classList.remove("govuk-visually-hidden");
  formRow.ariaHidden = false;
  const select = document.getElementById("directions-location");
  for (const location of this.multiLocations) {
    const option = document.createElement("option");
    option.text = location.address;
    select.add(option);
  }

  this.location = { lat: this.multiLocations[0].lat, lng: this.multiLocations[0].lon };
  this.destination = new google.maps.LatLng(this.location);
  
  select.addEventListener("change", (e) => {
    const selectedLocation = this.multiLocations.find(
      (location) => location.address === e.target.value
    );
    this.location = { lat: selectedLocation.lat, lng: selectedLocation.lon };
    this.destination = new google.maps.LatLng(this.location);
  })

}

FaaMapDirections.prototype.markerHtml = function () {
  const pinSvgString =
    '<svg viewBox="0 0 20 26" fill="none" xmlns="http://www.w3.org/2000/svg">' +
    '<path fill="#000" stroke="black" stroke-width="1.5" d="M19.25 9.75C19.25 10.7082 18.9155 11.923 18.3178 13.3034C17.7257 14.671 16.9022 16.1408 15.9852 17.591C14.152 20.4903 11.9832 23.2531 10.6551 24.8737C10.3145 25.2858 9.68536 25.2857 9.34482 24.8735C8.0167 23.2529 5.8479 20.4903 4.01478 17.591C3.09784 16.1408 2.27428 14.671 1.68215 13.3034C1.08446 11.923 0.75 10.7082 0.75 9.75C0.75 4.79919 4.87537 0.75 10 0.75C15.1246 0.75 19.25 4.79919 19.25 9.75ZM12.8806 6.9149C12.1135 6.16693 11.0769 5.75 10 5.75C8.92307 5.75 7.88655 6.16693 7.1194 6.9149C6.35161 7.6635 5.91667 8.68292 5.91667 9.75C5.91667 10.8171 6.35161 11.8365 7.1194 12.5851C7.88655 13.3331 8.92307 13.75 10 13.75C11.0769 13.75 12.1135 13.3331 12.8806 12.5851C13.6484 11.8365 14.0833 10.8171 14.0833 9.75C14.0833 8.68292 13.6484 7.6635 12.8806 6.9149Z" />' +
    "</svg>";
  const content = document.createElement("div");
  content.classList.add("faa-map__marker");
  content.innerHTML = pinSvgString;
  return content;
};

FaaMapDirections.prototype.updateLonLatFromPostcode = async function () {
  const url = `https://api.postcodes.io/postcodes/${this.ptcd}`;
  try {
    await fetch(url)
      .then((response) => {
        return response.json();
      })
      .then((data) => {
        this.location = { lat: data.result.latitude, lng: data.result.longitude };
      });
  } catch (error) {
    console.error(error);
  }
  return;
};

FaaMapDirections.prototype.isPostcodeValid = function () {
  const regex = new RegExp(
    "^[A-Za-z]{1,2}\\d{1,2}[A-Za-z]?\\s?\\d[A-Za-z]{2}$"
  );
  const result = !regex.test(this.origin);
  if (result) {
    this.updatePostcodeRow("Enter a valid postcode");
  } else {
    this.updatePostcodeRow();
  }
  return !result;
};

FaaMapDirections.prototype.updatePostcodeRow = function (message) {
  const postcodeRow = document.getElementById(
    "directions-postcode"
  ).parentElement;
  const errorMessage = postcodeRow.querySelector(".govuk-error-message");
  if (message !== undefined) {
    postcodeRow.classList.add("govuk-form-group--error");
    errorMessage.innerHTML = message;
  } else {
    postcodeRow.classList.remove("govuk-form-group--error");
    errorMessage.innerHTML = "";
  }
};

FaaMapDirections.prototype.getCurrentPostcode = function () {
  const postcodeField = document.getElementById("directions-postcode");
  const options = {
    maximumAge: 600000,
  };

  function success(position) {
    const url = `https://api.postcodes.io/postcodes?lon=${position.coords.longitude}&lat=${position.coords.latitude}&wideSearch=true`;
    try {
      fetch(url)
        .then((response) => {
          return response.json();
        })
        .then((data) => {
          postcodeField.value = data.result[0].postcode;
          postcodeField.placeholder = "";
        });
    } catch (error) {
      this.updatePostcodeRow("Location could not be found");
      postcodeField.placeholder = "";
    }
  }

  function error() {
    this.updatePostcodeRow("Location could not be found");
    postcodeField.placeholder = "";
  }
  this.updatePostcodeRow();
  postcodeField.placeholder = "Locating...";
  navigator.geolocation.getCurrentPosition(success, error, options);
};

FaaMapDirections.prototype.calculateAndDisplayRoute = function (
  directionsRenderer,
  directionsService
) {
  if (this.origin === undefined || this.travelMode === undefined) {
    return;
  }
  const journeyInfo = document.getElementById("faa-journey-info");

  directionsService
    .route({
      origin: {
        query: this.origin,
      },
      destination: this.destination,
      travelMode: google.maps.TravelMode[this.travelMode],
      unitSystem: google.maps.UnitSystem.IMPERIAL,
    })
    .then((response) => {
      journeyInfo.innerHTML = `<b>Distance</b> ${response.routes[0].legs[0].distance.text.replace(
        "mi",
        "miles"
      )}<br /> <b>Duration</b> ${response.routes[0].legs[0].duration.text.replace(
        "mins",
        "minutes"
      )}`;
      directionsRenderer.setDirections(response);
    })
    .catch((e) => {
      console.error(e);
      this.updatePostcodeRow("Location could not be found");
      journeyInfo.innerHTML = "";
    });
};

function FaaMap(mapId, link, linkLoading, container, radius) {
  this.container = container;
  this.radius = radius;
  this.link = link;
  this.linkLoading = linkLoading;
  this.mapId = mapId;
  this.centerLat;
  this.centerLng;
  this.mapData = [];
}

FaaMap.prototype.init = function () {
  this.setUpEvents();
};

FaaMap.prototype.setUpEvents = async function () {
  const hash = window.location.hash;
  if (hash === "#showMap") {
    await this.checkIfMapIsCachedOrGetData();
  } else {
    this.hideMap();
  }
  var that = this;
  document.body.addEventListener("keydown", (e) => {
    if (e.code === "Escape") {
      that.hideMap();
    }
  });
  window.addEventListener(
    "hashchange",
    async () => {
      const hash = window.location.hash;
      if (hash === "#showMap") {
        await this.checkIfMapIsCachedOrGetData();
      } else {
        this.hideMap();
      }
    },
    false
  );
};

FaaMap.prototype.checkIfMapIsCachedOrGetData = async function () {
  if (this.mapData.length > 0) {
    this.showMap();
  } else {
    this.link.classList.add("govuk-visually-hidden");
    this.linkLoading.classList.remove("govuk-visually-hidden");
    await this.getMapData();
    this.link.classList.remove("govuk-visually-hidden");
    this.linkLoading.classList.add("govuk-visually-hidden");
  }
};

FaaMap.prototype.getMapData = async function () {
  let params = "";
  const urlParams = new URLSearchParams(window.location.search);
  urlParams.append("excludeNational", "true");
  if (urlParams.size > 0) {
    params = `?${urlParams.toString()}`;
  }
  const url = `/map-search-results${params}`;
  await fetch(url, {
    method: "GET",
    headers: {
      "Content-Type": "application/json",
    },
  })
    .then((response) => {
      return response.json();
    })
    .then((data) => {
      this.mapData = data.apprenticeshipMapData;
      this.showMap();
      this.centerLat = data.searchedLocation.lat;
      this.centerLng = data.searchedLocation.lon;

      if (this.centerLat === 0 && this.centerLng === 0) {
        this.centerLat = 52.4379;
        this.centerLng = -1.6496;
      }
    });
};

FaaMap.prototype.showMap = function () {
  document.documentElement.classList.add("faa-map__body--open");
  document.body.classList.add("faa-map__body--open");
  window.location.hash = "#showMap";
  if (!this.map) {
    this.loadMap();
  }
};

FaaMap.prototype.hideMap = function () {
  document.documentElement.classList.remove("faa-map__body--open");
  document.body.classList.remove("faa-map__body--open");
  window.location.hash = "";
  localStorage.removeItem("faaMapActiveRole");
};

FaaMap.prototype.loadMap = async function () {
  const { Map, InfoWindow } = await google.maps.importLibrary("maps");
  const { AdvancedMarkerElement, PinElement } = await google.maps.importLibrary(
    "marker"
  );
  const mapCloseButtonWrap = document.createElement("div");
  const mapCloseButton = document.createElement("a");
  const mapRoleDetailsWrap = document.createElement("div");
  const bounds = new google.maps.LatLngBounds();

  this.map = new google.maps.Map(this.container, {
    center: new google.maps.LatLng(this.centerLat, this.centerLng),
    zoom: 10,
    mapId: this.mapId,
    mapTypeControl: false,
    fullscreenControl: false,
    streetViewControl: false,
    zoomControl: true,
    zoomControlOptions: {
      position: google.maps.ControlPosition.LEFT_BOTTOM,
    },
  });

  if (this.radius > 0) {
    const searchRadius = new google.maps.Circle({
      strokeColor: "#1D70B8",
      strokeOpacity: 0.8,
      strokeWeight: 2,
      fillColor: "#1D70B8",
      fillOpacity: 0.1,
      map: this.map,
      center: new google.maps.LatLng(this.centerLat, this.centerLng),
      radius: this.radius * 1609.34,
    });
  }

  mapCloseButtonWrap.classList.add("faa-map__close");

  mapCloseButton.classList.add("govuk-link");
  mapCloseButton.classList.add("govuk-link--no-visited-state");
  mapCloseButton.innerHTML =
    '<span class="faa-map__close-label">Close map</span><span class="faa-map__close-icon" />';
  mapCloseButton.setAttribute("href", "#");

  mapCloseButton.addEventListener("click", (e) => {
    e.preventDefault();
    this.hideMap();
  });

  mapCloseButtonWrap.append(mapCloseButton);

  mapRoleDetailsWrap.classList.add("faa-map__panel");
  mapRoleDetailsWrap.classList.add("faa-map__panel--role");
  mapRoleDetailsWrap.classList.add("faa-map__panel--hidden");

  this.map.markers = [];

  const activeMapMarker = localStorage.getItem("faaMapActiveRole");

  const duplicates = this.mapData.reduce((acc, current, index, array) => {
    if (
      array.findIndex(
        (item) =>
          item.position.lat === current.position.lat &&
          item.position.lng === current.position.lng
      ) !== index &&
      !acc.find(
        (item) =>
          item.position.lat === current.position.lat &&
          item.position.lng === current.position.lng
      )
    ) {
      acc.push(current);
    }
    return acc;
  }, []);

  const duplicatedGroups = [];
  duplicates.forEach((duplicatedGroup) => {
    const array = this.mapData.filter(
      (item) =>
        item.position.lat === duplicatedGroup.position.lat &&
        item.position.lng === duplicatedGroup.position.lng
    );
    duplicatedGroups.push(array);
  });

  const duplicatesRemoved = this.mapData.filter((item) => {
    return !duplicates.some(
      (duplicatedGroup) =>
        duplicatedGroup.position.lat === item.position.lat &&
        duplicatedGroup.position.lng === item.position.lng
    );
  });

  for (const role of duplicatesRemoved) {
    const Marker = new google.maps.marker.AdvancedMarkerElement({
      map: this.map,
      position: role.position,
      content: this.markerHtml(),
    });
    Marker.addListener("click", () => {
      this.toggleMarker(Marker, role, mapRoleDetailsWrap);
      this.map.panTo(role.position);
    });
    this.map.markers.push(Marker);

    if (activeMapMarker === role.job.id.toString()) {
      this.toggleMarker(Marker, role, mapRoleDetailsWrap);
      this.map.panTo(role.position);
    }

    bounds.extend(role.position);
  }

  for (const group of duplicatedGroups) {
    const Marker = new google.maps.marker.AdvancedMarkerElement({
      map: this.map,
      position: group[0].position,
      content: this.plusMarkerHtml(),
    });
    Marker.addListener("click", () => {
      this.handlePlusMarkerClick(Marker, group, mapRoleDetailsWrap);
      this.map.panTo(group[0].position);
    });
    this.map.markers.push(Marker);

    if (activeMapMarker === group[0].job.id.toString()) {
      this.toggleMarker(Marker, group, mapRoleDetailsWrap);
      this.map.panTo(group[0].position);
    }

    bounds.extend(group[0].position);
  }

  this.map.fitBounds(bounds);

  this.map.controls[google.maps.ControlPosition.BLOCK_START_INLINE_END].push(
    mapCloseButtonWrap
  );
  this.map.controls[google.maps.ControlPosition.INLINE_END_BLOCK_END].push(
    mapRoleDetailsWrap
  );
};

FaaMap.prototype.handlePlusMarkerClick = function (
  markerElement,
  group,
  mapRoleDetailsWrap
) {
  if (markerElement.content.classList.contains("expanded")) {
    group.forEach((role) => {
      this.removeMarkersWithTitle(`VAC${role.job.id}`);
    });
    markerElement.content.classList.remove("expanded");
    return;
  }

  markerElement.content.classList.add("expanded");
  markerElement.zIndex = 1;

  const angle = 360 / group.length;
  group.forEach((role, index) => {
    const a = Math.round(angle * index);
    const x = Math.round(100 * Math.cos(a * (Math.PI / 180)));
    const y = Math.round(100 * Math.sin(a * (Math.PI / 180)));
    const Marker = new google.maps.marker.AdvancedMarkerElement({
      map: this.map,
      title: `VAC${role.job.id}`,
      position: group[0].position,
      content: this.markerHtml(x, y, a),
    });
    Marker.addListener("click", () => {
      this.toggleMarker(Marker, role, mapRoleDetailsWrap);
      this.map.panTo(role.position);
    });
    this.map.markers.push(Marker);
  });
};

FaaMap.prototype.removeMarkersWithTitle = function (title) {
  for (let i = 0; i < this.map.markers.length; i++) {
    if (this.map.markers[i].title === title) {
      this.map.markers[i].setMap(null);
    }
  }
};

FaaMap.prototype.markerHtml = function (
  marginLeft = 0,
  marginTop = 0,
  angle = 0
) {
  const pinSvgString =
    '<svg viewBox="0 0 20 26" fill="none" xmlns="http://www.w3.org/2000/svg">' +
    '<path fill="#000" stroke="black" stroke-width="1.5" d="M19.25 9.75C19.25 10.7082 18.9155 11.923 18.3178 13.3034C17.7257 14.671 16.9022 16.1408 15.9852 17.591C14.152 20.4903 11.9832 23.2531 10.6551 24.8737C10.3145 25.2858 9.68536 25.2857 9.34482 24.8735C8.0167 23.2529 5.8479 20.4903 4.01478 17.591C3.09784 16.1408 2.27428 14.671 1.68215 13.3034C1.08446 11.923 0.75 10.7082 0.75 9.75C0.75 4.79919 4.87537 0.75 10 0.75C15.1246 0.75 19.25 4.79919 19.25 9.75ZM12.8806 6.9149C12.1135 6.16693 11.0769 5.75 10 5.75C8.92307 5.75 7.88655 6.16693 7.1194 6.9149C6.35161 7.6635 5.91667 8.68292 5.91667 9.75C5.91667 10.8171 6.35161 11.8365 7.1194 12.5851C7.88655 13.3331 8.92307 13.75 10 13.75C11.0769 13.75 12.1135 13.3331 12.8806 12.5851C13.6484 11.8365 14.0833 10.8171 14.0833 9.75C14.0833 8.68292 13.6484 7.6635 12.8806 6.9149Z" />' +
    "</svg>";
  const content = document.createElement("div");
  content.classList.add("faa-map__marker");
  content.innerHTML = pinSvgString;

  if (marginLeft !== 0 || marginTop !== 0 || angle !== 0) {
    content.style.transform = `translate(${marginLeft}px, ${marginTop}px)`;
    const xOffset = (marginLeft * -1 + 100) / 2 - 100;
    const tail = document.createElement("span");
    tail.classList.add("faa-map__marker-tail");
    tail.style.transform =
      `rotate(${angle}deg)` + `translate(${xOffset}px, ${marginTop / 2}px)`;
    content.appendChild(tail);
  }

  return content;
};

FaaMap.prototype.showRoleOverLay = function (role, panel) {
  function statusTag(vacancy) {
    if (vacancy.isClosingSoon) {
      return `<strong class="govuk-tag govuk-tag--orange govuk-!-margin-bottom-2">
                Closing soon
            </strong>`;
    }
    if (vacancy.isNew) {
      return `<strong class="govuk-tag govuk-!-margin-bottom-2">
                New
            </strong>`;
    }
    if (vacancy.applicationStatus != null) {
      return `<strong class="govuk-tag govuk-!-margin-bottom-2">
                ${vacancy.applicationStatus}
            </strong>`;
    }
    return "";
  }

  function closePanelButton(t) {
    const that = t;
    const closeButton = document.createElement("button");
    closeButton.classList.add("faa-map__panel-close");
    closeButton.innerHTML = `<span class="faa-map__close-icon"></span>`;
    closeButton.addEventListener("click", (e) => {
      e.preventDefault();
      that.hideRoleOverLay(panel);
    });
    return closeButton;
  }

  function showDistance(distance) {
    if (distance > 0) {
      return `<li><strong>Distance</strong> ${distance} miles</li>`;
    }
    return "";
  }

  panel.innerHTML = `
      ${statusTag(role.job)}
      <h2 class="govuk-heading-m govuk-!-margin-bottom-2"><a href="/apprenticeship/VAC${
        role.job.id
      }" class="govuk-link govuk-link--no-visited-state das-breakable faa-role-panel-heading">${
    role.job.title
  }</a></h2>
      <p class="govuk-!-font-size-16 govuk-!-margin-bottom-1">${
        role.job.company
      }</p>
      <p class="govuk-!-font-size-16 govuk-hint">${role.job.vacancyLocation}</p>
      <ul class="govuk-list govuk-!-font-size-16">
      ${showDistance(role.job.distance)}
      <li><strong>Training course</strong> ${role.job.apprenticeship}</li>
      <li><strong>Annual wage</strong>  ${role.job.wage}</li>
      </ul>
      <p class="govuk-!-font-size-16 govuk-!-margin-bottom-1">${
        role.job.closingDate
      }</p>
      <p class="govuk-!-font-size-16 govuk-!-margin-bottom-0 govuk-hint">${
        role.job.postedDate
      }</p>
  `;
  panel.append(closePanelButton(this));
  panel.classList.remove("faa-map__panel--hidden");
};

FaaMap.prototype.hideRoleOverLay = function (panel) {
  panel.classList.add("faa-map__panel--hidden");
  this.map.markers.forEach((mrk) => {
    mrk.content.classList.remove("highlight");
  });
};

FaaMap.prototype.toggleMarker = function (markerElement, role, panel) {
  this.map.markers.forEach((mrk) => {
    mrk.content.classList.remove("highlight");
  });

  markerElement.content.classList.add("highlight");
  markerElement.zIndex = 1;
  localStorage.setItem("faaMapActiveRole", role.job.id);
  this.showRoleOverLay(role, panel);
};

FaaMap.prototype.plusMarkerHtml = function () {
  const pinSvgString =
    '<svg viewBox="0 0 44 57" fill="none" xmlns="http://www.w3.org/2000/svg">' +
    "<g>" +
    '<path fill="#000" d="M22,0C9.9,0,0,9.6,0,21.4s13.4,27.1,19.3,34.3c1.4,1.7,4,1.7,5.4,0,5.9-7.2,19.3-24.5,19.3-34.3S34.1,0,22,0Z"/>' +
    '<polygon fill="#fff" points="34 19 25 19 25 10 19 10 19 19 10 19 10 25 19 25 19 34 25 34 25 25 34 25 34 19"/>' +
    "</g>" +
    "</svg>";
  const content = document.createElement("div");
  content.classList.add("faa-map__marker");
  content.innerHTML = pinSvgString;
  return content;
};

// cookies
function saveCookieSettings() {
  let consentAnalyticsCookieRadioValue = document.querySelector(
    "input[name=ConsentAnalyticsCookie]:checked"
  ).value;
  let consentFunctionalCookieRadioValue = document.querySelector(
    "input[name=ConsentFunctionalCookie]:checked"
  ).value;

  createCookie("AnalyticsConsent", consentAnalyticsCookieRadioValue);
  createCookie("FunctionalConsent", consentFunctionalCookieRadioValue);

  document.getElementById("confirmation-banner").removeAttribute("hidden");
  window.scrollTo({ top: 0, behavior: "instant" });
}
function acceptCookies(args) {
  createCookie("DASSeenCookieMessage", true);
  document.getElementById("cookieConsent").style.display = "none";
  if (args === true) {
    createCookie("AnalyticsConsent", true);
    createCookie("FunctionalConsent", true);
    document.getElementById("cookieAccept").removeAttribute("hidden");
  } else {
    createCookie("AnalyticsConsent", false);
    createCookie("FunctionalConsent", false);
    document.getElementById("cookieReject").removeAttribute("hidden");
  }
}
function createCookie(cookiname, cookivalue) {
  let date = new Date();
  date.setFullYear(date.getFullYear() + 1);
  let expires = "expires=" + date.toGMTString();
  document.cookie =
    cookiname + "=" + cookivalue + ";" + expires + ";path=/;Secure";
}
function hideAcceptBanner() {
  document.getElementById("cookieAccept").setAttribute("hidden", "");
}
function hideRejectBanner() {
  document.getElementById("cookieReject").setAttribute("hidden", "");
}
