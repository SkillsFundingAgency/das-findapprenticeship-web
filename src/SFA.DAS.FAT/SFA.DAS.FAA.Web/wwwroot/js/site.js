
// AUTOCOMPLETE

var $keywordsInput = $('#search-location');
var $submitOnConfirm = $('#search-location').data('submit-on-selection');
var $defaultValue = $('#search-location').data('default-value');
if ($keywordsInput.length > 0) {
    $keywordsInput.wrap('<div id="autocomplete-container" class="das-autocomplete-wrap"></div>');
    var container = document.querySelector('#autocomplete-container');
    var apiUrl = '/locations';
    $(container).empty();
    function getSuggestions(query, updateResults) {
        var results = [];
        $.ajax({
            url: apiUrl,
            type: "get",
            dataType: 'json',
            data: { searchTerm: query }
        }).done(function (data) {
            results = data.locations.map(function (r) {
                return r.name;
            });
            updateResults(results);
        });
    }
    function onConfirm() {
        var form = this.element.parentElement.parentElement;  
        var form2 = this.element.parentElement.parentElement.parentElement;
        setTimeout(function(){
            if (form.tagName.toLocaleLowerCase() === 'form' && $submitOnConfirm) {
                form.submit()
            }
            if (form2.tagName.toLocaleLowerCase() === 'form' && $submitOnConfirm) {
                form2.submit()
            }
    },200,form);}

    accessibleAutocomplete({
        element: container,
        id: 'search-location',
        name: 'location',
        displayMenu: 'overlay',
        showNoOptionsFound: false,
        minLength: 2,
        source: getSuggestions,
        placeholder: "",
        onConfirm: onConfirm,
        defaultValue: $defaultValue,
        confirmOnBlur: false,
        autoselect: true
    });
}


// FILTER CHECKBOXES
// If National filter is checked, then the parent 
// checkbox is also checked

$('#deliveryMode-National').on('change',function (){
    if ($(this).is(':checked')) {
        $('#deliveryMode-Workplace').prop('checked', true);
    }
});
$('#deliveryMode-Workplace').on('change', function(){
    if (!$(this).is(':checked')) {
        $('#deliveryMode-National').prop('checked', false);
    }
});


// BACK LINK
// If users history-1 does not come from this site, 
// then show a link to homepage

var $backLinkOrHome = $('.das-js-back-link-or-home');
var backLinkOrHome = function () {

    var referrer = document.referrer;

    var backLink = $('<a>')
        .attr({'href': '#', 'class': 'govuk-back-link'})
        .text('Back')
        .on('click', function (e) {
            window.history.back();
            e.preventDefault();
        });

    if (referrer && referrer !== document.location.href) {
        $backLinkOrHome.replaceWith(backLink);
    }
}

if ($backLinkOrHome) {
    backLinkOrHome();
}


// BACK TO TOP 
// Shows a back-to-top link in a floating header
// as soon as the breadcrumbs scroll out of view

$(window).bind('scroll', function() {

    var isCookieBannerVisible = $('.das-cookie-banner:visible').length,
        showHeaderDistance = 150 + (isCookieBannerVisible * 240),
        $breadcrumbs = $('.govuk-breadcrumbs');

    if ($breadcrumbs.length > 0) {
        var breadcrumbDistanceFromTop = $breadcrumbs.offset().top,
            breadcrumbHeight = $breadcrumbs.outerHeight();

        showHeaderDistance = breadcrumbDistanceFromTop + breadcrumbHeight;
    }

    if ($(window).scrollTop() > showHeaderDistance) {
        $('.app-shortlist-banner').addClass('app-shortlist-banner__fixed');
    } else {
        $('.app-shortlist-banner').removeClass('app-shortlist-banner__fixed');
    }

}).trigger("scroll");


// SCROLL TO TARGET 
// On click of the link, checks to see if the target exists
// If so, scrolls the page to that point, taking into account
// the back-to-top header

$("a[data-scroll-to-target]").on('click', function () {
    var target = $(this).data('scroll-to-target'),
        $target = $(target);
        headerOffset = $('.app-shortlist-banner__fixed').outerHeight() || 50;

    setTimeout(function() {
        if ($target.length > 0) {
            var scrollTo = $target.offset().top - headerOffset;
            $('html, body').animate({scrollTop: scrollTo}, 0);
        }
    }, 10)

});

// SHORTLIST

var shortlistAddForms = $('.app-provider-shortlist-add form');
var shortlistRemoveForms = $('.app-provider-shortlist-remove form');

var providerAddedClassName = 'app-provider-shortlist-added'

shortlistAddForms.on('submit', function(e) {
    var form = $(this);
    var formData = new FormData(this);
    formData.delete('routeName');
    sendData(formData, this.action, addFormDone, form);
    e.preventDefault();
});

shortlistRemoveForms.on('submit', function(e) {
    var form = $(this);
    var formData = new FormData(this);
    formData.delete('routeName');
    sendData(formData, this.action, removeFormDone, form);
    e.preventDefault();
});

var addFormDone = function(data, form) {
    var wrapper = form.closest('.app-provider-shortlist-control');
    var removeForm = wrapper.find('.app-provider-shortlist-remove form');
    removeForm.attr("action", "/shortlist/items/" + data);
    wrapper.addClass(providerAddedClassName)
    updateShortlistCount();
}

var removeFormDone = function(data, form) {
    var wrapper = form.closest('.app-provider-shortlist-control');
    var removeForm = wrapper.find('.app-provider-shortlist-remove form');
    removeForm.attr("action", "/shortlist/items/00000000-0000-0000-0000-000000000000");
    wrapper.removeClass(providerAddedClassName);
    updateShortlistCount(true);
}

var sendData = function(formData, action, doneCallBack, form){
    $.ajax({
        type: "POST",
        url: action,
        data: formData,
        processData: false,
        contentType: false
    }).done(function(data) {
        doneCallBack(data, form)
    });
}

var updateShortlistCount = function(remove) {
    var currentCount = $('body').data('shortlistcount');
    var shortlistCountsUi = $('.app-view-shortlist-link__number');
    
    currentCount += remove ? -1 : 1;

    $('body').data('shortlistcount', currentCount)
    shortlistCountsUi.text(currentCount).addClass('app-view-shortlist-link__number-update')

    setTimeout(function() {
        shortlistCountsUi.removeClass('app-view-shortlist-link__number-update')
    }, 1000);
}



// FEEDBACK GRAPH
// Generates the html to display
// feedback graph and populates the target

function nodeListForEach(nodes, callback) {
    if (window.NodeList.prototype.forEach) {
        return nodes.forEach(callback)
    }
    for (var i = 0; i < nodes.length; i++) {
        callback.call(window, nodes[i], i, nodes);
    }
}

function FeedbackGraph(table) {
    this.table = table
    this.target = this.table.dataset.target
    this.hideLegend = this.table.dataset.hideLegend === "true"
    this.label = this.table.dataset.label || "people"
    this.rows = this.table.querySelectorAll("tbody tr")
}

FeedbackGraph.prototype.init = function() {
    if (!document.getElementById(this.target)) {
        return
    }

    var that = this
    var rowCount = 0
    var legendlistHtml
    var graphHtml = document.createElement("div")
        graphHtml.className = "app-graph"

    var graphList = document.createElement("ul")
        graphList.className = "app-graph__list"

    var caption = this.table.querySelector("caption")
    var categoryHtml

    nodeListForEach(this.rows, function (row) {
        if (rowCount === 0) {
            legendlistHtml = that.legendHtml(row)
        }
        graphList.appendChild(that.graphRow(row))
        rowCount++
    });

    if (!this.hideLegend) {
        graphHtml.appendChild(legendlistHtml)
    }
    
    if (caption) {
        categoryHtml = document.createElement("h2")
        categoryHtml.className = "govuk-caption-l app-graph__category"
        categoryHtml.textContent = caption.textContent
        graphHtml.appendChild(categoryHtml)
    }

    graphHtml.appendChild(graphList)
    document.getElementById(this.target).appendChild(graphHtml)
}

FeedbackGraph.prototype.legendHtml = function(row) {
    var legendList = document.createElement('ul')
        legendList.className = "app-graph-key"
    var dataCells = row.querySelectorAll("td")
    var cellCount = 0

    if (this.hideLegend) {
        return ''
    }

    nodeListForEach(dataCells, function (dataCell) {
        if (isNaN(dataCell.textContent)) {

            var legendListItem = document.createElement("li")
                legendListItem.className = "app-graph-key__list-item app-graph-key__list-item--colour-" + (cellCount + 1)
                legendListItem.textContent = dataCell.dataset.label

            legendList.appendChild(legendListItem)
            cellCount++
        }
    });
    return legendList
}

FeedbackGraph.prototype.graphRow = function(row) {
    var that = this
    var questionText = row.querySelector("th").textContent
    var dataCells = row.querySelectorAll("td")
    var graphRowHtml = document.createElement('li')
    var barsHtml = document.createElement("div")
    var totalAsked = 0
    var barCount = 0

    graphRowHtml.className = "app-graph__list-item"
    barsHtml.className = "app-graph__chart-wrap"

    nodeListForEach(dataCells, function (dataCell) {
        if (isNaN(dataCell.textContent)) {
            var barHtml = that.barHtml(dataCell, barCount+1)
            if (barHtml !== undefined) {
                barsHtml.appendChild(barHtml)
            }
            barCount++
        } else {
            totalAsked = dataCell.textContent
        }
    });

    var caption = document.createElement('span')
        caption.className = "app-graph__caption"
        caption.textContent = "(selected by " + totalAsked + " " + this.label + ")"
    var heading = document.createElement('h3')
        heading.className = "app-graph__label"
        heading.textContent = questionText
        if (totalAsked > 0) {
            heading.appendChild(caption)
        }

        graphRowHtml.appendChild(heading)
        graphRowHtml.appendChild(barsHtml)

    return (graphRowHtml)
}

FeedbackGraph.prototype.barHtml = function(dataCell, barCount) {
    var percentage = parseFloat(dataCell.textContent.slice(0, -1));
    var span1 = document.createElement('span')
        span1.textContent = percentage + "%"
        span1.className = "app-graph__figure"

    var span2 = document.createElement('span')
        span2.className = "app-graph__bar-value app-graph__bar-value--colour-" + barCount
        span2.style.width = percentage + "%"
        span2.title = dataCell.dataset.title
        span2.tabIndex = 0
        span2.appendChild(span1)

    var span3 = document.createElement('span')
        span3.className = "app-graph__bar"
        span3.appendChild(span2)

    var span4 = document.createElement('span')
        span4.className = "app-graph__chart"
        span4.appendChild(span3)

    return(span4)

}



// SHOW/HIDE PANELS

function ShowHidePanels(container) {
    this.container = container
    this.panels = this.container.querySelectorAll('.app-show-hide-panel')
    this.hideClass = 'app-show-hide-panel__hidden'
}

ShowHidePanels.prototype.init = function() {
    var panelData = []
    var that = this
    nodeListForEach(this.panels, function (panel) {
        var panelObj = {
            id: panel.id,
            label: panel.dataset.panelLabel
        }
        panelData.push(panelObj)
        that.hidePanel(panel)
    })
    nodeListForEach(this.panels, function (panel) {
        panel.prepend(that.panelNav(panel.id, panelData))
    })
    this.showPanel(this.panels[0])
}

ShowHidePanels.prototype.panelNav = function(panelId, panelData) {
    var that = this
    var buttonWrap = document.createElement('div')
        buttonWrap.className = "govuk-button-group app-show-hide-panel__buttons"
    var filteredData = panelData.filter(function(item) {
        return item.id !== panelId
    })
    filteredData.forEach((item) => {
        buttonWrap.appendChild(that.showHideButton(item))
    });
    return buttonWrap
}

ShowHidePanels.prototype.showHideButton = function(item) {
    var button = document.createElement('a')
        button.className = 'govuk-button govuk-button--secondary govuk-!-font-size-16'
        button.textContent = 'Change to ' + item.label + ' view'
        button.href = '#' + item.id
        button.addEventListener('click', this.handleButtonClick.bind(this))
    return button
}

ShowHidePanels.prototype.handleButtonClick = function(e) {
    var that = this
    var targetPanel = e.target.hash.substring(1)
    nodeListForEach(this.panels, function (panel) {
        if (panel.id !== targetPanel) {
            that.hidePanel(panel)
        } else {
            that.showPanel(panel)
        }
    })
    e.preventDefault()
}

ShowHidePanels.prototype.hidePanel = function(panel) {
    panel.classList.add(this.hideClass)
}

ShowHidePanels.prototype.showPanel = function(panel) {
    panel.classList.remove(this.hideClass)
}




var showHidePanels = document.querySelectorAll('[data-show-hide-panels]');
nodeListForEach(showHidePanels, function (showHidePanel) {
  new ShowHidePanels(showHidePanel).init();
});

var feedbackGraphs = document.querySelectorAll('[data-feedback-graph]');
nodeListForEach(feedbackGraphs, function (feedbackGraph) {
  new FeedbackGraph(feedbackGraph).init();
});
