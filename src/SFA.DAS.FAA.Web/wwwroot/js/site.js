const locationInputs = document.querySelectorAll(".faa-location-autocomplete");
const apiUrl = "/locations";

if (locationInputs.length > 0) {
  for (let i = 0; i < locationInputs.length; i++) {
    const input = locationInputs[i];
    const container = document.createElement("div");

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

const forms = document.querySelectorAll("main form");

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
    "#faa-extra-fildes-max-message"
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
  if (hiddenRowCount === 1) {
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
      const txtInput = document.querySelector(this.convertId(this.selectId));
      const searchString = opt || txtInput.value;
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

function FaaMap(mapId, link, container, data, centerLat, centerLng) {
  this.container = container;
  this.link = link;
  this.data = data;
  this.mapId = mapId;
  this.centerLat = centerLat;
  this.centerLng = centerLng;
}

FaaMap.prototype.init = function () {
  this.setUpEvents();
};

FaaMap.prototype.setUpEvents = function () {
  var that = this;
  this.link.addEventListener("click", this.showMap.bind(this));
  document.body.addEventListener("keydown", (e) => {
    if (e.code === "Escape") {
      that.hideMap();
    }
  });
};

FaaMap.prototype.showMap = function (e) {
  e.preventDefault();
  document.documentElement.classList.add("faa-map__body--open");
  document.body.classList.add("faa-map__body--open");
  if (!this.map) {
    this.loadMap();
  }
};

FaaMap.prototype.hideMap = function () {
  document.documentElement.classList.remove("faa-map__body--open");
  document.body.classList.remove("faa-map__body--open");
};

FaaMap.prototype.loadMap = async function () {
  const { Map, InfoWindow } = await google.maps.importLibrary("maps");
  const { AdvancedMarkerElement, PinElement } = await google.maps.importLibrary(
    "marker"
  );
  this.map = new google.maps.Map(this.container, {
    center: new google.maps.LatLng(this.centerLat, this.centerLng),
    zoom: 7,
    mapId: this.mapId,
    mapTypeControl: false,
    fullscreenControl: false,
    streetViewControl: false,
    zoomControl: true,
    zoomControlOptions: {
      position: google.maps.ControlPosition.LEFT_BOTTOM,
    },
  });
  const searchRadius = new google.maps.Circle({
    strokeColor: "#1D70B8",
    strokeOpacity: 0.8,
    strokeWeight: 2,
    fillColor: "#1D70B8",
    fillOpacity: 0.1,
    map: this.map,
    center: new google.maps.LatLng(52.400575, -1.507825),
    radius: 750,
  });

  const mapCloseButtonWrap = document.createElement("div");
  mapCloseButtonWrap.classList.add("faa-map__close");

  const mapCloseButton = document.createElement("a");
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

  const mapRoleDetailsWrap = document.createElement("div");
  mapRoleDetailsWrap.classList.add("faa-map__panel");
  mapRoleDetailsWrap.classList.add("faa-map__panel--role");
  mapRoleDetailsWrap.classList.add("faa-map__panel--hidden");

  this.map.markers = [];

  for (const role of this.data) {
    const Marker = new google.maps.marker.AdvancedMarkerElement({
      map: this.map,
      position: role.position,
      content: this.markerHtml(),
    });
    Marker.addListener("click", () => {
      this.toggleMarker(Marker, role, mapRoleDetailsWrap);
    });
    this.map.markers.push(Marker);
  }
  this.map.controls[google.maps.ControlPosition.BLOCK_START_INLINE_END].push(
    mapCloseButtonWrap
  );
  this.map.controls[google.maps.ControlPosition.INLINE_END_BLOCK_END].push(
    mapRoleDetailsWrap
  );
};

FaaMap.prototype.showRoleOverLay = function (role, panel) {
  panel.innerHTML = `
      <strong class="govuk-tag govuk-!-margin-bottom-2 ${
        role.job.status ? "" : "govuk-visually-hidden"
      } ${role.job.status === "New" ? "" : "govuk-tag--orange"}">${
    role.job.status
  }</strong>
      <h2 class="govuk-heading-m govuk-!-margin-bottom-2"><a href="#" class="govuk-link govuk-link--no-visited-state">${
        role.job.title
      }</a></h2>
      <p class="govuk-!-font-size-16 govuk-!-margin-bottom-1">${
        role.job.company
      }</p>
      <p class="govuk-!-font-size-16 govuk-hint">${role.job.addressLine1}, ${
    role.job.postcode
  }</p>
      <ul class="govuk-list govuk-!-font-size-16">
      <li><strong>Distance</strong> ${role.job.distance}</li>
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
  panel.classList.remove("faa-map__panel--hidden");
};

FaaMap.prototype.toggleMarker = function (markerElement, role, panel) {
  this.map.markers.forEach((mrk) => {
    mrk.content.classList.remove("highlight");
  });

  markerElement.content.classList.add("highlight");
  markerElement.zIndex = 1;
  this.showRoleOverLay(role, panel);
};

FaaMap.prototype.markerHtml = function () {
  const pinSvgString =
    '<svg viewBox="0 0 20 26" fill="none" xmlns="http://www.w3.org/2000/svg">' +
    '<path fill="#000" stroke="black" stroke-width="1.5" d="M19.25 9.75C19.25 10.7082 18.9155 11.923 18.3178 13.3034C17.7257 14.671 16.9022 16.1408 15.9852 17.591C14.152 20.4903 11.9832 23.2531 10.6551 24.8737C10.3145 25.2858 9.68536 25.2857 9.34482 24.8735C8.0167 23.2529 5.8479 20.4903 4.01478 17.591C3.09784 16.1408 2.27428 14.671 1.68215 13.3034C1.08446 11.923 0.75 10.7082 0.75 9.75C0.75 4.79919 4.87537 0.75 10 0.75C15.1246 0.75 19.25 4.79919 19.25 9.75ZM12.8806 6.9149C12.1135 6.16693 11.0769 5.75 10 5.75C8.92307 5.75 7.88655 6.16693 7.1194 6.9149C6.35161 7.6635 5.91667 8.68292 5.91667 9.75C5.91667 10.8171 6.35161 11.8365 7.1194 12.5851C7.88655 13.3331 8.92307 13.75 10 13.75C11.0769 13.75 12.1135 13.3331 12.8806 12.5851C13.6484 11.8365 14.0833 10.8171 14.0833 9.75C14.0833 8.68292 13.6484 7.6635 12.8806 6.9149Z" />' +
    "</svg>";
  const content = document.createElement("div");
  content.classList.add("faa-map__marker");
  content.innerHTML = pinSvgString;
  return content;
};
