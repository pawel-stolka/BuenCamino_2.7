var map,
    _zoom;
var geoData = [],
    markers = [];//,

var routeId;

var maximalZoom = 17,
    selectedMode = "DRIVING";

var polyline_details = {
    color: "#0066ff",
    opacity: .65,
    weight: 5
}

var getJsonUrl = "/Route/GetPointsJson?id=";

//--------------------------- BASIC FUNCTIONALITY ----------------------------------------------
function Initialize() {
    InitMap();
    MarkPlaces();
    AddMapListeners();
}

function setInitials() {
    // hard-coded globals - Geographical midpoint of Europe - 53.57750°N 23.10611°E - https://en.wikipedia.org/wiki/Geographical_midpoint_of_Europe
    _zoom = 10;
    startLat = 53.57750;
    startLng = 23.10611;
    //console.log("initials set");
}

function InitMap() {
    setInitials();

    var startCenter = new google.maps.LatLng(startLat, startLng);

    var mapOptions = {
        zoom: _zoom,
        center: startCenter,
        draggableCursor: "crosshair",
        mapTypeId: "terrain"
    };
    map = new google.maps.Map(document.getElementById("map_canvas"), mapOptions);
}

var getRouteId = function () {
    var id = $("#RouteIdMode").val();
    routeId = parseInt(id);
    return routeId;
}

var getPoints_fromDb = function () {
    var id = getRouteId();

    return $.getJSON(getJsonUrl + id);
};

function MarkPlaces() {
    geoData = [];

    getPoints_fromDb()
        .done(function (data) {

            fulfillGeoData(data);

            SetZoomAfterMaximalZoomReached();
            putMarkers();

            ConnectAllPoints();
            ZoomOnBounds();

            $("#message").text("imported " + data.length + " points. Click on map to add new...");
        })
        .fail(function () {
            $("#message").text("sorry, no data...");
        });

    var fulfillGeoData = function (data) {
        $.each(data, function (index, value) {
            var item = {
                Latitude: parseFloat(value.Latitude),
                Longitude: parseFloat(value.Longitude),
                PlaceName: value.PlaceName
            }
            geoData.push(item);
        });
    }

    var putMarkers = function () {
        if (geoData.length > 0) {
            removeMarkers();

            $.each(geoData, function (item, value) {
                var point = {
                    lat: value.Latitude,
                    lng: value.Longitude,
                    name: value.PlaceName
                };
                AddMarker(point, "green");
            });
        } else {
            $("#message").html("geoData empty...");
        }
    }
}

function AddMarker(latLng, color) {
    var _latLng = new google.maps.LatLng(latLng),
        link = "http://maps.google.com/mapfiles/ms/icons/" + color + "-dot.png";

    var marker = new google.maps.Marker({
        position: _latLng,
        map: map,
        animation: google.maps.Animation.DROP,
        title: latLng.name
    });
    marker.setIcon(link);
    markers.push(marker);
}

function drawPolyline(latLng1, latLng2) {
    var coordinates = [
        {
            lat: latLng1.lat,
            lng: latLng1.lng
        },
        {
            lat: latLng2.lat,
            lng: latLng2.lng
        }
    ];
    var polyline_path = new google.maps.Polyline({
        path: coordinates,
        geodesic: true,
        strokeColor: polyline_details.color,
        strokeOpacity: polyline_details.opacity,
        strokeWeight: polyline_details.weight
    });
    polyline_path.setMap(map);

}

function ConnectAllPoints() {
    var pairs = [];

    $.each(geoData, function (item, value) {
        var latLng = { lat: parseFloat(value.Latitude), lng: parseFloat(value.Longitude) };
        pairs.push(latLng);
    });
    var pairsLength = pairs.length;
    if (pairsLength > 1) {
        for (var i = 1; i < pairsLength; i++) {
            drawPolyline(pairs[i - 1], pairs[i]);
        }
    }
}


function AddMapListeners() {
    var lat, lng;//, routeId;

    map.addListener('mousemove', function (event) {
        var myLatLng = event.latLng,
            lat = myLatLng.lat(),
            lng = myLatLng.lng();

        $("#moveLat").html(parseFloat(lat).toFixed(8));
        $("#moveLng").html(parseFloat(lng).toFixed(8));
    });

    map.addListener('mouseout', function (event) {
        $("#moveLat").html("");
        $("#moveLng").html("");
    });

    map.addListener('click', function (event) {

        var my_latLng = convertToLatLng(event);

        createPoint(my_latLng);

        redirectTo("/Point/Create?routeId=" + routeId);
    });

    map.addListener('rightclick', function (event) {
        console.log("right click");
    });

    var convertToLatLng = function (event) {
        var _lat = event.latLng.lat(),
            _lng = event.latLng.lng();
        var parse_lat = parseFloat(_lat),
            parse_lng = parseFloat(_lng);
        lat = parse_lat.toFixed(8),
        lng = parse_lng.toFixed(8);
        return {
            lat: parse_lat,
            lng: parse_lng
        }
    }

    var createPoint = function (latLng) {
        getRouteId();
        setLocalCoords();
        addMarker(latLng, "yellow");
    }



    var setLocalCoords = function () {
        localStorage.setItem("_latitude", lat);
        localStorage.setItem("_longitude", lng);
        localStorage.setItem("_routeId", routeId);
    }

    

    var addMarker = function (latLng) {
        var color = "yellow";
        removeMarkers();
        latLng.name = "temporal";
        AddMarker(latLng, color);
    }

    var redirectTo = function (route) {
        window.location = route;
    }
}

function ZoomOnBounds() {
    var bounds = new google.maps.LatLngBounds();
    //var _markers_;// = [];
    if (markers.length > 0) {
        $.each(markers, function (item, value) {
            var _lat1 = value.position.lat(),
                _lng1 = value.position.lng();

            var myPlace = new google.maps.LatLng(_lat1, _lng1);
            bounds.extend(myPlace);
        });
        map.fitBounds(bounds);
    } else {
        $("#message").text("markers.length = 0");
    }
}

function SetZoomAfterMaximalZoomReached() {
    // This is needed to set the zoom after fitbounds, 
    google.maps.event.addListener(map, 'zoom_changed', function () {

        zoomChangeBoundsListener =
            google.maps.event.addListener(map, 'bounds_changed', function (event) {
                if (this.getZoom() > maximalZoom) {
                    //console.log("max zoom - " + map.getZoom());
                    // Change max/min zoom here
                    this.setZoom(maximalZoom);
                }
                google.maps.event.removeListener(zoomChangeBoundsListener);
            });
    });
    map.initialZoom = true;
}

function removeMarkers() {
    for(var i = 0; i < markers.length; i++) {
        markers[i].setMap(null);
    }
    markers = [];
}

function showMessage(message) {
    $("#message").text(message);
}

