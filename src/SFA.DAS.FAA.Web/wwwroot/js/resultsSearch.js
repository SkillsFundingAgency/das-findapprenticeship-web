$(function () {
    $("#sort-results").change(function () {
        let selectedVal = $('#sort-results').val();
        let queryString = window.location.search;
        let params = new URLSearchParams(queryString);
        params.delete('sort');
        params.append('sort', selectedVal);
        document.location.href = "?" + params.toString();
    });
});