const { BrowserWindow } = require('electron');

// Application page templates
let appPages = {
    login: './admin_login/login.html',
    menu: './admin_menu/menu.html',
    ballot: './ballot_pack/ballot.html',
    authentication: './voter_authentication/authentication.html',
    worker: './service_worker/worker.html',
    reports: './report_portal/report.html'
};

// Constructs a Browser window 
function appWindow(template) {
    let window = new BrowserWindow({
        show: false, frame: false, webPreferences: {
            nodeIntegration: true,
            contextIsolation: false
        }
    });
    //window.webContents.openDevTools();
    window.loadFile(template);
    window.once('ready-to-show', e => {
        if (template !== appPages.worker) {
            window.setFullScreen(true);
            window.show();
       }
    });
    return window;
}

// Encaspulates a voter's information and thier vote in all possible elections
class VotePack {
    constructor(voter) {
        this.Vin = voter.vin;
        this.VoterAge = voter.age;
        this.VoterGender = voter.gender,
        this.VoterOccupation = voter.occupation;
        this.VoteDate = this.#getVotePackDate();
        this.VotePackStatus = 'P';
        this.VoteForPresident;
        this.VoteForSenate;
        this.VoteForReps;
        this.VoteForGovernor;
        this.VoteForAssembly;
    }
    setBallotVote(election, votedParty) {
        this[election] = votedParty;
    }
    
    #getVotePackDate() {
        var date = new Date();
        return `${date.toLocaleDateString()}T${date.toLocaleTimeString()}`;
    }
}

// Module exports
module.exports = {
    VotePack: VotePack,
    appWindow: appWindow,
    appPages: appPages
};