(function() {
    var _latitude = localStorage.getItem("_latitude");
    var _longitude = localStorage.getItem("_longitude");
    var _routeId = localStorage.getItem("_routeId");

    $("#Latitude").val(_latitude);
    $("#Longitude").val(_longitude);
    $("#RouteId").val(_routeId);

    $("#clickLatitude").val(_latitude);
    $("#clickLongitude").val(_longitude);
})();
