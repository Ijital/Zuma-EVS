const { database } = require('../app_data/database');
const { network } = require('../config');
const { createHash } = require('crypto');

// Handles window load event by scheduling vote block mine event
window.onload = () => setInterval(() => CreatePendingVoteBlock(), network.mineInterval);

// Mines a vote block on the block chain network
function CreatePendingVoteBlock() {
    database.getPendingVotePacks()
        .then(pendingVotePacks => pendingVotePacks.length > 0 ? transmitPendingVoteBlock(pendingVotePacks) : null)
        .catch(error => database.saveIncident('PendingVotes', error));
}

// Transmits vote block to all nodes in the block chain network
// What is the protocol for when some nodes have not responded/ recieved a transmission ?
function transmitPendingVoteBlock(votePacks) {
    let pendingVoteBlock = getVoteBlock(votePacks);
    let promises = network.nodes.map(node => post(node, pendingVoteBlock));
    Promise.all(promises).then(results => {
        updateVotePacks(votePacks);
    }).catch();
}

// Updates status of mine processed vote packs
function updateVotePacks(votePacks) {
    let votePackVins = votePacks.map(v => v.Vin);
    let promises = votePackVins.map(vin => database.updateVotePackStatus(vin));
    Promise.all(promises).then(results => pendingVotePacks.length = 0);
}

// Creates a new vote block for mining in the block chain network
function getVoteBlock(pendingVotePacks) {
    let votePacksJson = JSON.stringify(pendingVotePacks, (key, value) => {
        if (key === 'VotePackStatus') {
            return undefined
        } return value;
    });
    return {
        voteBlockState: 7,
        voteBlockLG: 21,
        voteBlockPU: 10,
        voteBlockGuid: crypto.randomUUID(),
        votePacks: votePacksJson,
        VoteBlockHash: createHash('sha256').update(votePacksJson).digest('hex'),
    };
}

// Excutes a post request
async function post(node, data) {
    return new Promise(async (resolve, reject) => {
        let response = await fetch(node, {
            method: 'POST',
            headers: { 'Accept': 'application/json', 'Content-Type': 'application/json' },
            body: JSON.stringify(data)
        });
        var result = await response.json();
        result == true ? resolve(true) : reject(false);
    });
};


//STRESS TEST

// let votePackCounter = 0;

// (function voteTestRun() {   
//     setInterval(() => {
//         if (votePackCounter < 100) {
//             votePackCounter++;
//             var date = new Date();
//             let votePack = {
//                 "Vin": `${Math.random() * 10000000000000000}`,
//                 "VoterAge": 44,
//                 "VoterGender": 'M',
//                 "VoterOccupation": "developer",
//                 "VoteDate": `${date.toLocaleDateString()}T${date.toLocaleTimeString()}`,
//                 "VotePackStatus": 'P',
//                 "VoteForPresident": 'PDP',
//                 "VoteForSenate": 'PDP',
//                 "VoteForReps": 'PDP',
//                 "VoteForGovernor": 'PDP',
//                 "VoteForAssembly": 'PDP'
//             };
//             database.saveVotePack(Object.values(votePack));
//         }

//     }, 100);
// })();
