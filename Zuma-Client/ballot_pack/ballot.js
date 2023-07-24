const { ipcRenderer } = require('electron');
const { VotePack } = require('../models');
const { database } = require('../app_data/database');
const { elections, parties } = require('../config');
const Swal = require('sweetalert2');

let electionIndex = 0;
let votePack;

// Handles window load event
window.addEventListener("load", e => loadNewBallot());

// Handles event when voter info been submitted
ipcRenderer.on('voter-info-submitted', (e, voter) => votePack = new VotePack(voter));

// Handles event when voter has selected a party to vote for in current ballot
function handleBallotVote(party) {
    Swal.fire({
        title: `You have selected ${party}`,
        showCancelButton: true,
        confirmButtonText: '<img src="../app_assets/img/confirm.jpg"/>',
        cancelButtonText: '<img src="../app_assets/img/cancel3.jpg"/>',
        text: 'Please Confirm or Cancel Selection',
        imageUrl: `../app_assets/img/parties/${party}.jpg`,
        imageWidth: 180,
        imageHeight: 180,
    }).then(select => { if (select.isConfirmed) { submitBallotVote(party); } });
}

//Handles event when user has comfirmed thier vote in current ballot
function submitBallotVote(party) {
    let electionTitle = elections[electionIndex].title;
    votePack.setBallotVote(electionTitle, party);

    if (electionIndex < elections.length - 1) {
        electionIndex++;
        loadNewBallot();
    }
    else {
        database.saveVotePack(Object.values(votePack));
        ipcRenderer.send('votepack-completed');
        document.getElementById("ballot-fieldset").disabled = true;
        showVotePackSummary();
        setTimeout(() => Swal.close(), 9350);
    }
}

// Loads a new ballot
function loadNewBallot() {
    setNewBallotTitle();
    setNewBallotOptions();
}

// Sets new ballot title
function setNewBallotTitle(){
    let electionNameHolder = document.getElementById("election-title");
    electionNameHolder.innerHTML= new String();
    let newTitle = document.createElement('h1');
    newTitle.innerText= elections[electionIndex].name;
    newTitle.classList.add('animate__animated', 'animate__heartBeat');
    electionNameHolder.appendChild(newTitle);
}

// Sets new ballot options
function setNewBallotOptions(){
    let ballotTemplate = document.getElementById('ballot-template');
    ballotTemplate.innerHTML = new String();
       parties.forEach(party => {
        let partyOption = document.createElement('div');
        partyOption.classList.add('col-sm-2');
        partyOption.innerHTML =
            `<button class="btn party-logo" onclick="handleBallotVote('${party.acronym}')">
                  <img src="../app_assets/img/Parties/${party.acronym}.jpg" data-acronym="${party.acronym}">
            </button>`;
        ballotTemplate.appendChild(partyOption);
    });
}

// Displays and prints vote pack
function showVotePackSummary() {
    Swal.fire({
        showConfirmButton: false,
        allowOutsideClick: false,
        html: getVotePackSummary(),
        title: '',
        showClass: { popup: 'animate__animated animate__fadeInDown' },
        hideClass: { popup: 'animate__animated animate__fadeOutUp' }
    });
}

// Gets the summary of Vote pack
function getVotePackSummary() {
    let summary = `
        <div class="container">
            <h5 id="print-ballot-header">INDEPENDENT NATIONAL ELECTORAL COMMISSION</h5>
            <h6>Election Ballot pack</h6>
        <div class="row">`
    elections.forEach(election => {
        summary += `
         <div class="col-sm-4 ballot-vote"><h6>${election.name}</br> ${votePack[election.title]}</h6></div>
         <div class="col-sm-4 ballot-vote"><img width=50 src="../app_assets/img/Parties/${votePack[election.title]}.jpg"></div>
         <div class="col-sm-4 ballot-vote"><img width=50 src="../app_assets/img/thumbprint.jpg"></div>`
    });
    return summary + `</div></div>`;
}

// Reloads Main Menu page
function returnToMenu(){
    ipcRenderer.send('return-to-menu');
}