const { ipcRenderer } = require('electron');
const Swal = require('sweetalert2');

// Lunches the login in dialogue when the page is ready
document.onreadystatechange = () => {
    let loginModal = new bootstrap.Modal(document.getElementById("login-modal"), {});
    setTimeout(() => loginModal.show(), 2000);
}

// Validates admin user login
function login() {
    let username = document.getElementById('user').value;
    let password = document.getElementById('password').value;

    if (username === "admin" && password === "admin") {
        ipcRenderer.send('login-success');
    }
    else{
        Swal.fire({html:'Invalid username or password'});
    }
    
}