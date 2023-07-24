const { ipcRenderer } = require('electron');
const { database } = require('../app_data/database');
const { VotePack } = require('../models');

// Reload the main menu page
function returnToMenu(){
    ipcRenderer.send('return-to-menu'); 
}

// Activates the voter authentication dialogue
function activateAuthentication() {
    document.getElementById('screen-active').style.display = 'block';
    document.getElementById('screen-sleep').style.display = 'none';
    document.body.style.backgroundColor = 'white';
}

// Authenticates a voter
function authenthicate() {
    let voter = getVoterInfo();
    database.findVotePack(voter.vin).then(record => {
        record ? reportIncident(voter.vin) : ipcRenderer.send('voter-authorized', voter);
    });
}

// Reports incident when double voter record is found
function reportIncident(vin) {
    database.saveIncident(['Mutiple vote', vin]).then(e => setTimeout(() => location.reload(), 6000));
}

// Gets voter authentication information
function getVoterInfo() {
    return {
        "vin": `${Math.random()*10000000000000000}`,
        "age": 44,
        "gender": "F",
        "occupation": "developer",
    };
}