const ipcRenderer = require('electron').ipcRenderer;

//
function selectElection(election) {
    ipcRenderer.send('election-selected', election);
}