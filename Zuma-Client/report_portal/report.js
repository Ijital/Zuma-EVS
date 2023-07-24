const { database } = require('../app_data/database');
const { ipcRenderer } = require('electron');
const {elections, parties} = require('../config');
var htmlToImage = require('html-to-image');

let allVotePacks = [];
let results = [];

// Reloads Main Menu page
function returnToMenu(){
    ipcRenderer.send('return-to-menu');
}

//Handles event when worker window is loaded
window.addEventListener("load", e => {
    database.getAllMinedVotePacks().then(votePacks => {
        allVotePacks = votePacks;
        getElectionReport();
        showElectionReport();
    });
});

// Gets the election report for Pollin unit
function getElectionReport() {
    parties.forEach(party => getAllElectionVotesForParty(party.acronym));
}

// Gets vote count for each election for the given party
function getAllElectionVotesForParty(party) {
    let partyResults = {};
    partyResults['Party'] = party;
    elections.forEach(election => partyResults[election.title] = GetElectionVotes(election.title, party));
    results.push(partyResults);
}

// Gets the vote count for a given party in the given election
function GetElectionVotes(election, party) {
    return allVotePacks.filter(vp => vp[election] === party && vp['VotePackStatus'] === 'M').length;
}

function printReport() {
    var doc = new jsPDF();

    window.print();
    // alert('Printing report');
    // var node = document.getElementById('sese');
    // htmlToImage.toPng(node)
    //   .then(function (dataUrl) {
    //     var img = new Image();
    //     img.src = dataUrl;
    //     document.body.appendChild(img);
    //   })
    //   .catch(function (error) {
    //     console.error('oops, something went wrong!', error);
    //   });

}



// Shows the election results for all parties
function showElectionReport() {
    resultsTableBody = document.getElementById('table-body');
    results.forEach(result => {
        let tableRow = document.createElement('tr');
        tableRow.innerHTML =
        `<td><img src="../app_assets/img/parties/${result.Party}.jpg">
          <span class="party-accronym">${result.Party}</span></td>
          <td>${result.VoteForPresident}</td>
          <td>${result.VoteForSenate}</td>
          <td>${result.VoteForReps}</td>
          <td>${result.VoteForGovernor}</td>
          <td>${result.VoteForAssembly}</td>
          <td></td>`;
        resultsTableBody.appendChild(tableRow);
    });

    let officialSignRow = document.createElement('tr');
    officialSignRow.innerHTML = `<td class="officials-sign" colspan="3">Presiding Officer Signature</td>`;
    resultsTableBody.appendChild(officialSignRow);
}
