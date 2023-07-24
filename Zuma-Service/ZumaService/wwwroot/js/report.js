
// Bar chart bar colors
let barColors = ['#363636', '#997950', '#0018F9', '#5F9EA0', '#4B5320', '#000080', '#DA70D6', '#B5338A', '#FF6FFF',
    '#FF00FF', '#6495ED', '#4D516D', '#4CBB17', '#C7EA46', '#6495ED', '#492000', '#FF8C00', '#BDB7AB'
];

//Election report titles
let reportTitles = {
    president: 'INEC |  Presidential Election Report',
    senate: 'INEC |  Senatorial  Election Report',
    representative: 'INEC |  Representatives  Election Report',
    governor: 'INEC |  Gubernatorial  Election Report',
    assembly: 'INEC |  Assembly Election Report',
};


var reportElection;
var report = [];
var reportHeading;
var reportFilter = 'totalVote';

// Initialises a Chart instance 
let reportChart = new Chart(document.getElementById('report-chart'),
    {
        type: 'bar',

        data: {
            labels: [],
            color: '#7FFF00',
            datasets: [
                {
                    label: '',
                    data: [],
                    barThickness: 40,
                    categoryPercentage: 1,
                    backgroundColor: barColors
                }
            ]
        },
        options: {
            indexAxis: 'y',
            legend: { display: false },
            title: {
                display: true,
                text: ""
            }
        }
    }
);

// Loads election report unto chart and table
async function getReport(election) {
    document.getElementById('totalVote').checked = true;
    reportElection = election;
    document.title = reportTitles[election];

    showProcessCursor();
    var chart = document.getElementById('report');
    chart.style.visibility = "initial";

    var response = await fetch(`/electionreports/${election}`);
    report = await response.json();
    hideProcessCursor();

    renderElectionResultTable(reportFilter);

    document.getElementById("report-table").style.visibility = "initial";
    renderElectionResultChart(reportFilter);
};


// Renders election report in UI Chart component
function renderElectionResultChart(filter) {

    hideProcessCursor();
    document.getElementById("report-table").style.visibility = "initial";

    reportChart.data.labels = report.partyVoteCounts.map(row => row.party);
    reportChart.data.datasets[0].data = report.partyVoteCounts.map(row => row[filter]);
    reportChart.options.title.text = reportTitles[reportElection];
    reportChart.update();
}

// Renders election report in UI table component
function renderElectionResultTable(filter) {
    let voteCount = 0;

    resultsTableBody = document.getElementById('table-body');
    resultsTableBody.innerHTML = '';

    report.partyVoteCounts.forEach(vCount => {
        voteCount += vCount[filter];
        let tableRow = document.createElement('tr');
        tableRow.innerHTML =
            `<td>${vCount.party}</td>
          <td>${vCount[filter]}</td>`;
        resultsTableBody.appendChild(tableRow);
    });

    let totalVotesRow = document.createElement('tr');
    totalVotesRow.classList.add('vote-totals');
    totalVotesRow.innerHTML = `<td>Total</td><td>${voteCount}</td>`;

    resultsTableBody.appendChild(totalVotesRow);
}

(function setFilter() {
    const radios = document.querySelectorAll('input[name="filterOption"]');
    radios.forEach(radio => {
        radio.onclick = () => {
            reportChart.data.labels = report.partyVoteCounts.map(row => row.party);
            reportChart.data.datasets[0].data = report.partyVoteCounts.map(row => row[radio.value]);
            reportChart.options.title.text = reportTitles[reportElection];
            reportChart.update();
            renderElectionResultTable(radio.value);
        }
    })
})();

// Show the await cursor when processing data
function showProcessCursor() {
    document.getElementById("spin").style.visibility = "initial";
}

// Hides the await cursor when data processing is complete
function hideProcessCursor() {
    document.getElementById("spin").style.visibility = "hidden";
}
