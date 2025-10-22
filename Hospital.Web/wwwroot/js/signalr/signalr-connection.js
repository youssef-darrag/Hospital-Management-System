'use strict';

/**
 * SignalR Connection Manager
 * Handles creation and lifecycle of the SignalR connection
 */
const SignalRConnection = (function () {
    let connection = null;

    async function startConnection(url, onConnected) {
        if (!window.signalR) {
            console.error('❌ SignalR library not found.');
            return;
        }

        connection = new signalR.HubConnectionBuilder()
            .withUrl(url)
            .withAutomaticReconnect([0, 2000, 5000, 10000, 15000])
            .configureLogging(signalR.LogLevel.Warning)
            .build();

        // connection lifecycle events
        connection.onreconnecting(() => console.log('🔁 SignalR reconnecting...'));
        connection.onreconnected(() => {
            console.log('✅ SignalR reconnected');
            if (typeof onConnected === 'function') onConnected();
        });
        connection.onclose(() => console.log('❌ SignalR connection closed'));

        try {
            await connection.start();
            console.log('✅ SignalR connected successfully');
            if (typeof onConnected === 'function') onConnected();
        } catch (err) {
            console.error('❌ Error while starting SignalR connection:', err);
            setTimeout(() => startConnection(url, onConnected), 5000);
        }

        return connection;
    }

    function getConnection() {
        return connection;
    }

    return {
        start: startConnection,
        get: getConnection
    };
})();