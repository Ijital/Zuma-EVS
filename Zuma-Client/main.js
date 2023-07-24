const { app, ipcMain } = require('electron');
const { appWindow, appPages } = require('./models');
const { database } = require('./app_data/database');

// Appplication windows
let mainWindow;
let workerWindow;


// Handles event when application has started and is ready
app.on('ready', function () {
    initialiseDatabase();
    workerWindow = appWindow(appPages.worker);
    mainWindow =  appWindow(appPages.login);
    mainWindow.on('close', e => {
        workerWindow.webContents.send('app-ready-to-quit');
        database.closeConnection();
        app.quit();
    })
})

// Initializes application database
function initialiseDatabase() {
    database.createTable('votePacksTable');
    database.createTable('usersTable');
    database.createTable('incidencesTable');
}

// Handles event when admin user is successfully logged in
ipcMain.on('login-success', e => mainWindow.loadFile(appPages.menu));


// Handles event when admin user has selected configuration portal
ipcMain.on('config-portal-selected', e => mainWindow.loadFile(appPages.config));


// Handles event when admin user has selected Voter portal
ipcMain.on('voter-portal-selected', e => mainWindow.loadFile(appPages.authentication));


// Handles event when admin user has selected election report portal
ipcMain.on('report-portal-selected', e => mainWindow.loadFile(appPages.reports));


// Handles event when voter is authenticated, loads a new ballot pack for the curent voter
ipcMain.on('voter-authorized', (e, voter) => {
    mainWindow.loadFile(appPages.ballot);
    mainWindow.webContents.once('dom-ready', () => mainWindow.webContents.send('voter-info-submitted', voter));
});

// Handles event when voter has completed thier vote pack, Reloads voter authentication page or the next voter
ipcMain.on('votepack-completed', e => setTimeout(() => mainWindow.loadFile(appPages.authentication), 10000));

// Handles event when menu option is selected, reloads main menu page
ipcMain.on('return-to-menu', e => mainWindow.loadFile(appPages.menu));