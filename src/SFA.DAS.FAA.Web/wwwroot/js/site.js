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
}

ExtraFieldRows.prototype.init = function () {
  this.insertAddLink();
  // Append the remove links
  for (let f = 0; f < this.extraFieldRows.length; f++) {
    const extraFieldRow = this.extraFieldRows[f];
    this.appendRemoveLink(extraFieldRow);
  }

  // If all rows are hidden, add class to hide the fieldset
  const hiddenRowCount = this.showHideEmptyRows();
  if (hiddenRowCount === this.extraFieldRows.length) {
    this.fieldset.classList.add(this.fieldsetHiddenClass);
  }
};

ExtraFieldRows.prototype.showHideEmptyRows = function () {
  let hiddenRowCount = 0;
  for (let f = 0; f < this.extraFieldRows.length; f++) {
    const extraFieldRow = this.extraFieldRows[f];
    const textInput = extraFieldRow.querySelector("input");
    if (textInput.value === "") {
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

ExtraFieldRows.prototype.appendRemoveLink = function (row) {
  const that = this;
  const removeLink = document.createElement("a");
  removeLink.innerHTML = "Remove";
  removeLink.className =
    "govuk-link govuk-link--no-visited-state faa-extra-field__form-group-link--remove";
  removeLink.href = "#";
  removeLink.addEventListener("click", function (e) {
    e.preventDefault();
    that.addLink.classList.remove(that.hiddenClass);
    that.hideRow(row);
    that.updateRowOrder();
  });
  row.append(removeLink);
};

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
  }
  this.showRow(rowToShow, true);
  e.preventDefault();
};

ExtraFieldRows.prototype.updateRowOrder = function () {
  const that = this;
  const answers = [];
  for (let f = 0; f < that.extraFieldRows.length; f++) {
    const extraFieldRow = that.extraFieldRows[f];
    const textValue = extraFieldRow.querySelector("input").value;
    if (!extraFieldRow.classList.contains(this.hiddenClass)) {
      answers.push(textValue);
    }
    extraFieldRow.querySelector("input").value = "";
    that.hideRow(extraFieldRow);
  }
  answers.forEach(function (answer, answerIndex) {
    const extraFieldRow = that.extraFieldRows[answerIndex];
    const textInput = extraFieldRow.querySelector("input");
    textInput.value = answer;
    that.showRow(extraFieldRow, false);
  });
};

ExtraFieldRows.prototype.hideRow = function (row) {
  const textInput = row.querySelector("input");
  textInput.value = "";
  row.classList.add(this.hiddenClass);
  if (this.areAllRowsHidden()) {
    this.fieldset.classList.add(this.fieldsetHiddenClass);
  } else {
    this.fieldset.classList.remove(this.fieldsetHiddenClass);
  }
};

ExtraFieldRows.prototype.showRow = function (row, focus = false) {
  const errorMessage = row.querySelector(".govuk-error-message");
  const textInput = row.querySelector("input");
  this.fieldset.classList.remove(this.fieldsetHiddenClass);
  if (errorMessage) {
    errorMessage.remove();
  }
  row.classList.remove("govuk-form-group--error");
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

const extraFieldRows = document.querySelectorAll("[data-extra-field-rows]");
if (extraFieldRows) {
  extraFieldRows.forEach(function (extraFieldRows) {
    new ExtraFieldRows(extraFieldRows).init();
  });
}
