let parties = [
    {
        "acronym": "A",
        "Name": "Accord Party"
    },
    {
        "acronym": "AA",
        "Name": "Action Alliance"
    },
    {
        "acronym": "AAC",
        "Name": "African Action Congress"
    },
    {
        "acronym": "ADC",
        "Name": "African Democratic Congress"
    },
    {
        "acronym": "ADP",
        "Name": "Action Democratic Party"
    },
    {
        "acronym": "APC",
        "Name": "All Progressives Congress"
    },
    {
        "acronym": "APGA",
        "Name": "All Progressives Grand Alliance"
    },
    {
        "acronym": "APM",
        "Name": "Allied Peoples Movement"
    },
    {
        "acronym": "APP",
        "Name": "Action Peoples Party"
    },
    {
        "acronym": "BP",
        "Name": "Boot Party"
    },
    {
        "acronym": "LP",
        "Name": "Labour Party"
    },
    {
        "acronym": "NNPP",
        "Name": "New Nigeria Peoples Party"
    },
    {
        "acronym": "NRM",
        "Name": "National Rescue Movement"
    },
    {
        "acronym": "PDP",
        "Name": "Peoples Democratic Party"
    },
    {
        "acronym": "PRP",
        "Name": "Peoples Redemption Party"
    },
    {
        "acronym": "SDP",
        "Name": "Social Democratic Party"
    },
    {
        "acronym": "YPP",
        "Name": "Young Progressive Party"
    },
    {
        "acronym": "ZLP",
        "Name": "Zenith Labour Party"
    }
]

let elections = [
    {
        "title": "VoteForPresident",
        "name": "Presidential Election"
    },
    {
        "title": "VoteForSenate",
        "name": "Senatorial Election"
    },
    {
        "title": "VoteForReps",
        "name": "Representatives Election"
    },
    {
        "title": "VoteForGovernor",
        "name": "Gubernatorial Election"
    },
    {
        "title": "VoteForAssembly",
        "name": "State Assembly Election"
    }
]

let network = {
    "mineInterval": 5000,
    "nodes": [
        "https://localhost:7149/VoteBlocks/savependingvoteblock",
    ]
}

// Module exports
module.exports = {
    parties: parties,
    elections: elections,
    network: network
};