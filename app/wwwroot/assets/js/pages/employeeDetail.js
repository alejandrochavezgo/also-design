function initializeTracesTable() {
    try {
        $('#dvTableTraces').DataTable({
            "scrollX": true,
            "autoWidth": false,
            "responsive": true,
            "ajax": {
                "url": "getEmployeeTracesByEmployeeId",
                "type": "get",
                "data": function(d) {
                    d.id = $('#employeeId').val();
                },
                "dataSrc": function(data) {
                    if (!data.isSuccess) {
                        Swal.fire({
                            title: 'Error!!',
                            html: data.message,
                            icon: 'error',
                            confirmButtonClass: 'btn btn-danger w-xs mt-2',
                            buttonsStyling: false,
                            footer: '',
                            showCloseButton: true
                        });
                        return [];
                    }
                    return data.results;
                },
                "error": function(xhr, error, thrown) {
                    Swal.fire({
                        title: 'Error!!',
                        html: error,
                        icon: 'error',
                        confirmButtonClass: 'btn btn-danger w-xs mt-2',
                        buttonsStyling: false,
                        footer: '',
                        showCloseButton: true
                    });
                }
            },
            "columns": [
                { "data": "id", "title": "ID" },
                { "data": "traceTypeDescription", "title": "Trace Type" },
                { "data": "username", "title": "Username" },
                { "data": "comments", "title": "Comments" },
                { "data": "creationDateAsString", "title": "Creation Date" }
            ],
            "order": [[0, 'desc']],
            "rowCallback": function (row, data) {
                $(row).addClass('cursor-pointer');
                $(row).addClass('interactive-row');
                $(row).off('click').on('click', function () {
                    showTraceInformation(data.id);
                });
            }
        }).on('processing.dt', function(e, settings, processing) {
            $('#loader').toggle(processing);
        });
    } catch(exception) {
        Swal.fire({
            title: 'Error!!',
            html: exception,
            icon: 'error',
            confirmButtonClass: 'btn btn-danger w-xs mt-2',
            buttonsStyling: false,
            footer: '',
            showCloseButton: true
        });
        $('#loader').hide();
    }
}

async function showTraceInformation(traceId) {
    try {
        $('#loader').show();
        var response = await fetch(window.location.origin + '/employee/getEmployeeTraceById?id=' + traceId);
        if (!response ||!response.ok) {
            Swal.fire({
                title: 'Error!!',
                html: `HTTP error! Status: ${response.status}`,
                icon: 'error',
                confirmButtonClass: 'btn btn-danger w-xs mt-2',
                buttonsStyling: false,
                footer: '',
                showCloseButton: true
            });
            $('#loader').hide();
            return;
        }

        var trace = await response.json();
        if (!trace || !trace.isSuccess) {
            Swal.fire({
                title: 'Error!!',
                html: trace.message,
                icon: 'error',
                confirmButtonClass: 'btn btn-danger w-xs mt-2',
                buttonsStyling: false,
                footer: '',
                showCloseButton: true
            });
            $('#loader').hide();
            return;
        }

        if (!trace.results.beforeChange && !trace.results.afterChange || (trace.results.beforeChange == '-' && trace.results.afterChange == '-')) {
            Swal.fire({
                title: 'Error!!',
                html: 'This trace cannot be compared because there is no data to compare against.',
                icon: 'error',
                confirmButtonClass: 'btn btn-danger w-xs mt-2',
                buttonsStyling: false,
                footer: '',
                showCloseButton: true
            });
            $('#loader').hide();
            return;
        }

        if (trace.results.beforeChange == '-') {
            var afterChange = JSON.parse(trace.results.afterChange)
            renderJSONWithNoDifferences(afterChange, '#beforeChangeContainer', '#afterChangeContainer');
        } else {
            var beforeChange = JSON.parse(trace.results.beforeChange);
            var afterChange = JSON.parse(trace.results.afterChange);
            renderJSONWithDifferences(beforeChange, afterChange, '#beforeChangeContainer', '#afterChangeContainer');
        }

        $('#traceInformationModal').modal('show');
        $('#loader').hide();
    } catch(exception) {
        Swal.fire({
            title: 'Error!!',
            html: exception,
            icon: 'error',
            confirmButtonClass: 'btn btn-danger w-xs mt-2',
            buttonsStyling: false,
            footer: '',
            showCloseButton: true
        });
        $('#loader').hide();
    }
}

function renderJSONWithDifferences(beforeJSON, afterJSON, beforeContainerSelector, afterContainerSelector) {
    var beforeHTML = generateHighlightedHTML(normalizeJSON(beforeJSON), normalizeJSON(afterJSON), false);
    var afterHTML = generateHighlightedHTML(normalizeJSON(afterJSON), normalizeJSON(beforeJSON), true);
    $(beforeContainerSelector).html(beforeHTML);
    $(afterContainerSelector).html(afterHTML);
}

function renderJSONWithNoDifferences(afterJSON, beforeContainerSelector, afterContainerSelector) {
    var afterHTML = generateHTML(afterJSON);
    $(beforeContainerSelector).html('');
    $(afterContainerSelector).html(afterHTML);
}

function generateHighlightedHTML(baseJSON, compareJSON, highlightDifferences) {
    var baseKeys = Object.keys(baseJSON);
    let html = '';
    baseKeys.forEach(key => {
        var baseValue = baseJSON[key];
        var compareValue = compareJSON[key];
        var isDifferent = !deepEqual(baseValue, compareValue);
        var highlightedClass = isDifferent && highlightDifferences ? 'diff-highlight' : '';
        html += `<div>
            <span class="${highlightedClass}">${key}: ${JSON.stringify(baseValue)}</span>
        </div>`;
    });
    return html;
}

function deepEqual(obj1, obj2) {
    if (typeof obj1 !== typeof obj2) return false;
    if (obj1 === obj2) return true;

    if (typeof obj1 === 'object' && obj1 !== null && obj2 !== null) {
        let keys1 = Object.keys(obj1).sort();
        let keys2 = Object.keys(obj2).sort();
        if (keys1.length !== keys2.length) return false;

        return keys1.every(key => deepEqual(obj1[key], obj2[key]));
    }

    return false;
}

function normalizeJSON(json) {
    if (typeof json !== 'object' || json === null) return json;
    const keys = Object.keys(json).sort();
    let normalized = {};
    keys.forEach(key => {
        normalized[key] = normalizeJSON(json[key]);
    });
    return normalized;
}

function generateHTML(baseJSON) {
    var baseKeys = Object.keys(baseJSON);
    let html = '';
    baseKeys.forEach(key => {
        var baseValue = baseJSON[key];
        html += `<div>
            <span>${key}: ${JSON.stringify(baseValue)}</span>
        </div>`;
    });
    return html;
}

$(document).ready(function () {
    $('#loader').show();
    initializeTracesTable();
});