const locationInputs = document.querySelectorAll(".faa-location-autocomplete");
const apiUrl = "/locations";

if (locationInputs.length > 0) {
  for (let i = 0; i < locationInputs.length; i++) {
    const input = locationInputs[i];
    const container = document.createElement("div");

    container.className = "das-autocomplete-wrap";
    input.parentNode.replaceChild(container, input);

    const getSuggestions = (query, updateResults) => {
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
