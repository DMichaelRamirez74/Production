﻿var cacheStatusValues = [];
cacheStatusValues[0] = 'uncached';
cacheStatusValues[1] = 'idle';
cacheStatusValues[2] = 'checking';
cacheStatusValues[3] = 'downloading';
cacheStatusValues[4] = 'updateready';
cacheStatusValues[5] = 'obsolete';

var cache = window.applicationCache;
cache.addEventListener('cached', logEvent, false);
cache.addEventListener('checking', logEvent, false);
cache.addEventListener('downloading', logEvent, false);
cache.addEventListener('error', logEvent, false);
cache.addEventListener('noupdate', logEvent, false);
cache.addEventListener('obsolete', logEvent, false);
cache.addEventListener('progress', logEvent, false);
cache.addEventListener('updateready', logEvent, false);

function logEvent(e) {
    var online, status, type, message;
    online = (navigator.onLine) ? 'yes' : 'no';
    status = cacheStatusValues[cache.status];
    type = e.type;
    message = 'online: ' + online;
    message += ', event: ' + type;
    message += ', status: ' + status;
    if (type == 'error' && navigator.onLine) {
        message += ' (prolly a syntax error in manifest)';
    }
}
window.applicationCache.addEventListener(
    'updateready',
    function () {

        window.location.reload();
    },
    false
);
window.applicationCache.addEventListener(
    'cached',
    function () {

        window.location.reload();
    },
    false
);
//window.applicationCache.addEventListener('downloading', function () {
//    $.blockUI({ message: "<h1>Application is upgrading with latest version Please wait...</h1>" });
//}, false);
//window.applicationCache.addEventListener('error', function () {
//    $.unblockUI({ message: "<h1>Application is upgrading with latest version Please wait...</h1>" });
//}, false);//this