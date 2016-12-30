// -------------------------------------------- distance matrix -------------------------------------------------------

var distances = [],
    couples = [];

function getCouples() {
    var start, stop;
    $.each(markers, function (item, value) {
        if (item > 0) {
            stop = {
                lat: value.position.lat(),
                lng: value.position.lng(),
                name: value.title
            };
            var couple = {
                start: start,
                stop: stop
            }
            couples.push(couple);
            start = {
                lat: value.position.lat(),
                lng: value.position.lng(),
                name: value.title
            };
        } else {
            start = {
                lat: value.position.lat(),
                lng: value.position.lng(),
                name: value.title
            };
        }
    });
    showCouples();
};

function showCouples() {
    $.each(couples, function (index, value) {
        //console.log(value);
        var latLng1 = { lat: value.start.lat, lng: value.start.lng };
        var latLng2 = { lat: value.stop.lat, lng: value.stop.lng };
        psCountDistanceBetween(latLng1, latLng2);

        var message = value.start.name + " - " + value.stop.name;// + " | " + distances[index].text;
        appendText(message);
    });
}


function appendText(message) {
    var line = "<li>" + message + "</li>";
    $("#kmList").append(line);
}

function psCountDistanceBetween(latLng0, latLng1) {
    console.log("psCountDistanceBetween");
    var service = new google.maps.DistanceMatrixService;
    service.getDistanceMatrix({
        origins: [latLng0], //LatLng],//[origin1, origin2],
        destinations: [latLng1], //LatLng],//[destinationA, destinationB],
        travelMode: selectedMode, // 'DRIVING',
        unitSystem: google.maps.UnitSystem.METRIC,
        avoidHighways: false,
        avoidTolls: false
    }, function (response, status) {
        if (status !== 'OK') {
            alert('Error was: ' + status);
        } else {
            if (status !== 'ZERO_RESULTS') {
                console.log("Dist....!");
                var distance = response.rows[0].elements[0];
                //var place1 = markers[originId].title,
                //    place2 = markers[destinationId].title;
                _status = distance.distance;

                distances.push(_status);

                showMessage(_status.text);
                console.log(_status.text);
            } else {
                console.log("sorry.. cannot do this.");
            }
        }
    });
}





function CountDistance_FromMarkers() {
    $.each(couples, function (item, value) {
        var latLng1 = { lat: value.start.lat, lng: value.start.lng };
        var latLng2 = { lat: value.stop.lat, lng: value.stop.lng };
        var between = value.start.name + " - " + value.stop.name;

        distanceBetweenMarkers(latLng1, latLng2);
    });
}



function Kms() {
    getCouples();
    $.each(couples, function (index, value) {
        //var latLng1 = { lat: value.start.lat, lng: value.start.lng };
        //var latLng2 = { lat: value.finish.lat, lng: value.finish.lng };
        var between = value.start.name + " - " + value.finish.name;
        var dist = calculateDistance(index);
        //console.log(between + ": " + dist);
        var message = between + ": " + dist;
        console.log(message);
        appendText(message);
    });
}
function showPoints(first, last) {
    $.each(couples, function (index, value) {
        if ((item >= first) && (index <= last)) {
            var latLng1 = { lat: value.start.lat, lng: value.start.lng };
            var latLng2 = { lat: value.stop.lat, lng: value.stop.lng };
            var between = value.start.name + " - " + value.stop.name;
            var dist = distanceBetweenMarkers(latLng1, latLng2);

            CountDistanceBetween(latLng1, latLng2);
            //calculateDistance(index);
            var message = between + ": " + dist;
            console.log(message);
            appendText(message);
        }
    });
}
var distance;
function calculateDistance(id) {
    getCouples();
    var service = new google.maps.DistanceMatrixService;
    //var distance;
    var o = { lat: couples[id].start.lat, lng: couples[id].start.lng, name: couples[id].start.name },
        d = { lat: couples[id].finish.lat, lng: couples[id].finish.lng, name: couples[id].finish.name };
    service.getDistanceMatrix({
        origins: [o], //LatLng],//[origin1, origin2],
        destinations: [d], //LatLng],//[destinationA, destinationB],
        travelMode: selectedMode, // 'DRIVING',
        unitSystem: google.maps.UnitSystem.METRIC,
        avoidHighways: false,
        avoidTolls: false
    }, function (response, status) {
        if (status !== 'OK') {
            alert('Error was: ' + status);
        } else {
            distance = response.rows[0].elements[0];
            showMessage(distance);
            console.log(distance);
            if (distance.status == 'ZERO_RESULTS') {
                showMessage('ZERO_RESULTS');
            } else {
                distance = response.rows[0].elements[0];
                var message = o.name + "-" + d.name + ": " + distance.distance.text;
                showMessage(message);
                var listItem = "<p>" + message + "</p>";
                $("#kmList").append(listItem);
            }
        }
    });
    return distance;
}



function distanceBetweenMarkers(latLngMarker1, latLngMarker2) {
    console.log("distanceBetweenMarkers");
    var _status;
    var service = new google.maps.DistanceMatrixService;
    service.getDistanceMatrix({
        origins: [latLngMarker1], //LatLng],//[origin1, origin2],
        destinations: [latLngMarker2], //LatLng],//[destinationA, destinationB],
        travelMode: selectedMode, // 'DRIVING',
        unitSystem: google.maps.UnitSystem.METRIC,
        avoidHighways: false,
        avoidTolls: false
    }, function (response, status) {
        if (status !== 'OK') {
            alert('Error was: ' + status);
        } else {
            if (status !== 'ZERO_RESULTS') {
                originList = response.originAddresses;
                destinationList = response.destinationAddresses;
                console.log("originList");
                console.log(originList);
                var _distance = response.rows;
                var distance = _distance[0].elements[0];
                //var place1 = markers[originId].title,
                //    place2 = markers[destinationId].title;
                _status = distance.distance;

                distances.push(_status);
                //showMessage(_status);
                console.log(_status.text);
            } else {
                showMessage("sorry...no possible to count.");
            }
        }
    });
    return _status;
}




function getDistance(originId, destinationId) {
    console.log("getDistance");
    console.log("originId: ");
    console.log(originId);
    console.log("destinationId: ");
    console.log(destinationId);
    var service = new google.maps.DistanceMatrixService;//,
    //o_lat = geoData[originId].Latitude,
    //o_lng = geoData[originId].Longitude,
    //d_lat = geoData[destinationId].Latitude,
    //d_lng = geoData[destinationId].Longitude,
    //originLatLng = { lat: parseFloat(o_lat), lng: parseFloat(o_lng) },
    //destinationLatLng = { lat: parseFloat(d_lat), lng: parseFloat(d_lng) };

    service.getDistanceMatrix({
        origins: [originId],//LatLng],//[origin1, origin2],
        destinations: [destinationId],//LatLng],//[destinationA, destinationB],
        travelMode: selectedMode,// 'DRIVING',
        unitSystem: google.maps.UnitSystem.METRIC,
        avoidHighways: false,
        avoidTolls: false
    }, function (response, status) {
        if (status !== 'OK') {
            alert('Error was: ' + status);
        } else {
            //directionsDisplay.setDirections(response);
            if (status !== 'ZERO_RESULTS') {
                originList = response.originAddresses;
                destinationList = response.destinationAddresses;
                var _distance = response.rows;
                var distance = _distance[0].elements[0];
                //var place1 = markers[originId].title,
                //    place2 = markers[destinationId].title;
                _status = distance.distance; //.text
                if (_status != undefined) {
                    //showMessage(" | distance " + _status.text + " | duration " + distance.duration.text);
                    tickMarkers = geoData;
                    var uno = tickMarkers[0].title + " - ",
                        duo = tickMarkers[1].title + " | ",
                        tre = "distance " + _status.text + " | ",
                        quatro = "duration " + distance.duration.text;

                    showMessage(uno + duo + tre + quatro);
                } else {
                    showMessage("sorry...no possible to count.");
                }
            } else {
                showMessage(status);
            }
        }
    });
}


var resp;

function calculateAndDisplay(startLocation, endLocation) {

    console.log("yupi 2");
    directionsService.route({
        origin: startLocation,
        destination: endLocation,
        travelMode: 'DRIVING'

    }, function (response, status) {
        if (status === 'OK') {
            // 1. draws a plain line between points
            //drawPolyline(startLocation, endLocation);
            resp = response;
            //console.log(resp);
            routePoints = resp.routes[0].overview_path;
            // 2. draws counted route
            //directionsDisplay.setDirections(response);
        } else {
            showMessage('Directions request failed due to ' + status);
        }
    });

}

function getLatLngFromMarker(id) {
    var lat = markers[id].position.lat();
    var lng = markers[id].position.lng();
    return { lat: lat, lng: lng };
}

function showPathBetween(id0, id1) {
    var p0 = getLatLngFromMarker(id0);
    var p1 = getLatLngFromMarker(id1);
    calculateAndDisplay(p0, p1);
}

function myCallback(id0, nextCode) {
    var id1 = id0++;
    showPathBetween(id0, id1);
    nextCode();
}

function myFunc() {
    console.log("func");
    var loc0, loc1;
    $.each(routePoints, function (index, value) {

        if (index > 0) {
            loc1 = { lat: value.lat(), lng: value.lng() };
            //console.log(loc0);
            //console.log(loc1);
            drawPolyline(loc0, loc1);
            loc0 = loc1;

        }
        //
        loc0 = { lat: value.lat(), lng: value.lng() };

    });
}

function connectArrayPoints(id0, id1) {
    showPathBetween(id0, id1);
    $.each(routePoints, function (index, value) {
        console.log(value);
        //drawPolyline
    });
}


//function GetDistancePoints() {
//    getSelectedPoints();

//    function drawLines() {
//        var lat0 = tickMarkers[0].lat(),
//                   lng0 = tickMarkers[0].lng(),
//                   lat1 = tickMarkers[1].lat(),
//                   lng1 = tickMarkers[1].lng();
//        var latLng0 = { lat: lat0, lng: lng0 },
//            latLng1 = { lat: lat1, lng: lng1 };
//        console.log("yupi 1");
//        calculateAndDisplay(latLng0, latLng1);
//    };

//    function getSelectedPoints() {
//        if (tickMarkers !== undefined) {
//            var flag = checkIfExists();
//            if (!flag) {
//                tickMarkers[1] = tickMarkers[0];
//                tickMarkers[0] = currentLatLng;
//                tickMarkers[0].title = markerTitle;

//                drawLines();

//            } else {
//                console.log("my error!");
//            }

//            function checkIfExists() {
//                if (tickMarkers.length < 2) {
//                    if (currentLatLng.lat() === tickMarkers[0].lat())
//                        return true;
//                } else {
//                    if (currentLatLng.lat() === tickMarkers[0].lat())
//                        return true;
//                    if (currentLatLng.lat() === tickMarkers[1].lat())
//                        return true;
//                }
//                return false;
//            }
//        } else {
//            tickMarkers = [];
//            tickMarkers.push(currentLatLng);
//            tickMarkers[0].title = markerTitle;
//        }

//        var amount = tickMarkers.length;
//        //console.log(amount);
//        return amount;
//    }
//}


//getCouples = function () {
//    $.each(markers, function (item, value) {
//        var startPos, startName, finishPos, finishName;
//        if (item > 0) {
//            finishPos = value.position;
//            finishName = value.name;
//            var couple = {
//                startPos: startPos,
//                startName: startName,
//                finishPos: finishPos,
//                finishName: finishName
//            }
//            couples.push(couple);
//            startPos = finishPos;
//            startName = finishName;
//        } else {
//            startPos = value.position;
//            startName = value.title;
//        }
//    });
//}
//$.each(markers, function (item, value) {
//var latLng = item.position.value;
//var name = item.title;
//console.log(item);

/*
var start, finish;
if (item > 0) {
    finish = value.position;
    var name = value.title;
    var place = { position: start, name: name };
    distances.push(place);
} else {
    start = value.position;
}*/
//var start = value.position;
//console.log(value.position.lat());
//console.log(value.position.lng());
//console.log(value.title);
//});