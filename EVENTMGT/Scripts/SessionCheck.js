
/*function checkUserSession() {
    // Check if the user session exists
    var userSession = '@Session["UserID"]' !== '';

    // If the user session does not exist, redirect to the login page
    if (!userSession) {
    window.location.href = '/Registrations/Login';
    }
    }*/

/*function checkUserSession() {
    var userSession = '@Session["UserID"]';
    console.log('User Session:', userSession);

    if (userSession === '') {
        window.location.href = '/Registrations/Login';
    }
}*/

function checkUserSession() {
    // Check if the user session exists
    var userSession = userSessionId !== '';

    // If the user session does not exist, redirect to the login page
    if (!userSession) {
        window.location.href = '/Registrations/Login';
    }
}