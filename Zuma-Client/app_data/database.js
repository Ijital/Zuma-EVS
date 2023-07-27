const sqlite3 = require('sqlite3').verbose();
const { dbCommands } = require('./dbSettings');
const path = require('path');

// Initialises and opens connection to local Sqlite database
const db = new sqlite3.Database(path.resolve(__dirname, 'data.db'), sqlite3.OPEN_READWRITE, er => {
    er ? console.log(er.message) : console.log('Database opened');
});

// Closes connection to local Sqlite database
function closeConnection() {
    db.close(er => {
        er ? console.log(er.message) : console.log('Database closed');
    });
}

// Creates a database table by the specified table name
function createTable(tableName) {
    return new Promise((resolve, reject) => {
        db.run(dbCommands[tableName], [], er => {
            er ? reject(er.message) : resolve(`${tableName} Table created`);
        });
    });
}

// Inserts a new vote pack to the specified database table
function saveVotePack(values) {
    return new Promise((resolve, reject) => {
        db.run(dbCommands.saveVotePack, values, er => {
            er ? reject(er.message) : resolve(`votePack Saved`);
        });
    });
}

// Inserts a new incident to the specified database table
function saveIncident(values) {
    return new Promise((resolve, reject) => {
        db.run(dbCommands.saveIncident, values, er => {
            er ? reject(er.message) : resolve(`${tableName} Saved`);
        });
    });
}

// Gets all records from the specified table 
function getTransmittedVotePacks() {
    return new Promise((resolve, reject) => {
        db.all(dbCommands.getTransmittedVotePacks, [], (er, rows) => {
            er ? reject(er.message) : resolve(rows);
        });
    });
}

// Gets Pending Vote Packs
function getPendingVotePacks() {
    return new Promise((resolve, reject) => {
        db.all(dbCommands.getPendingVotePacks, [], (er, rows) => {
            er ? reject(er.message) : resolve(rows);
        });
    });
}

// looks up a record by the specified query
function findVotePack(vin) {
    return new Promise((resolve, reject) => {
        db.get(dbCommands.findVotePack, [vin], (er, row) => {
            er ? reject(er.message) : resolve(row ? true : false);
        });
    });
}

// Updates a given record given the record Id
function updateVotePackStatus(vin) {
    return new Promise((resolve, reject) => {
        db.run(dbCommands.updateVotePackStatus, [vin], (er) => {
            er ? reject(er.message) : resolve(true);
        });
    });
}

// Module exports
module.exports.database = {
    closeConnection: closeConnection,
    createTable: createTable,
    saveVotePack: saveVotePack,
    saveIncident: saveIncident,
    findVotePack: findVotePack,
    updateVotePackStatus: updateVotePackStatus,
    getPendingVotePacks: getPendingVotePacks,
    getTransmittedVotePacks: getTransmittedVotePacks
}