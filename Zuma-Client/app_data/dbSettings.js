// Database entities schema
const schema = {
    VotePacks: `(Vin,VoterAge,VoterGender,VoterOccupation,VoteDate,
    VotePackStatus,VoteForPresident,VoteForSenate,VoteForReps,VoteForGovernor,VoteForAssembly)`,
    Users: `(Username, Password)`,
    Incidences: `(IncidentType, IncidentSummary)`,
}

// Database sql commands
const dbCommands = {
    votePacksTable: `create table if not exists VotePacks ${schema.VotePacks}`,
    saveVotePack: `insert into VotePacks ${schema.VotePacks} values(?,?,?,?,?,?,?,?,?,?,?)`,
    getPendingVotePacks: `select * from VotePacks where VotePackStatus = "P"`,
    getTransmittedVotePacks: `select * from VotePacks where VotePackStatus = "T"`,
    updateVotePackStatus: `update VotePacks set VotePackStatus = "T" where Vin = ?`,
    findVotePack: `select Vin from VotePacks where Vin = ?`,
    incidencesTable: `create table if not exists Incidences ${schema.Incidences}`,
    saveIncident: `insert into Incidences (IncidentType, IncidentSummary) values(?,?)`,
    usersTable: `create table if not exists Users ${schema.Users}`,
}

// Module exports
module.exports = {
    dbCommands: dbCommands
}