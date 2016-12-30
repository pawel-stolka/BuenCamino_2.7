(function getTableRow() {
    if (!document.getElementsByTagName || !document.createTextNode) return;
    //var rows = document.getElementsByClassName('table-hover').getElementsByTagName('tr');
    var rows = document.getElementsByTagName('tr');

    for (i = 0; i < rows.length; i++) {
        //inputs[i].onclick = function () {
        //    //var 
        //    alert(inputs[i].type);
        //    if (inputs[i].type === 'checkbox') {
        //        var r = this.id;
        //        alert(rowNumber);//geoData[i]);
        //        //checkBoxes.push()
        //    }
        //}
        rows[i].onclick = function () {
            var rowNumber = this.rowIndex;
            if (rowNumber !== 0) {

                var $td = $(this).closest('tr').children('td');
                var eq0 = $td.eq(0).text();
                var eq1 = $td.eq(1).text();
                var dateTime = $td.eq(2).text();
                var date = dateTime.trim();
                var day = date.substr(0, 10);

                console.log(day);

                getDataFromUrl(date);
            }
        };
    }

})();

//function convertDate(dateString) {
//    dateTimeParts = dateString.split(" ");
//    var timeParts, // = dateTimeParts[1].split(":"),
//       dateParts, // = dateTimeParts[0].split("-");
//       date;
//    if (dateTimeParts.length > 0) {

//        timeParts = dateTimeParts[1].split(":"),
//        dateParts = dateTimeParts[0].split("-");
//        date = new Date(dateParts[2], parseInt(dateParts[1], 10) - 1, dateParts[0], timeParts[0], timeParts[1]);
//    }
//    return date;
//}

function getDataFromUrl(date) {
    // ActionResult GetPointsByDateJson(int id, string dateTime)
    //return $.getJSON("/Route/GetPointsJson?id=" + id);
    var id = getIdFromUrl();
    console.log(id);
    var url = "/Route/GetPointsByDateJson?id=" + id + "&dateTime=" + date;
    console.log("date");
    y = date;// convertDate(date);
    console.log(y);
    var dataArray = [];
    $.getJSON(url)
        .done(function (data) {
            //console.log(data);
            //InitMap();
            //SetBounds(id - 1, id); //rowNumber - 1, rowNumber);
            $.each(data, function (index, value) {
                //console.log(index);
                //console.log(value);
                var pointLatLng = { lat: parseFloat(value.Latitude), lng: parseFloat(value.Longitude) };
                dataArray.push(pointLatLng);
            });
            //var pointLatLng = { lat: parseFloat(data[0].Latitude), lng: parseFloat(data[0].Longitude) };
            ShowPoints(dataArray);
            //console.log(dataArray);
        })
        .fail(function (data) {
            console.log("Nope.");
        });
}

function ZoomOnDay() {

}

function ShowPoints(latLngArray) {
    // let's assume array is 1 object
    var bounds = new google.maps.LatLngBounds();
    //var pointLatLng = new google.maps.LatLng(latLngArray.lat, latLngArray.lng);
    //bounds.extend(pointLatLng);
    //map.fitBounds(bounds);
    $.each(latLngArray, function (index, value) {
        //console.log(index + ": ");
        //console.log(value);
        //console.log(data.value.Latitude);
        //console.log(data.value.Longitude);

        var pointLatLng = new google.maps.LatLng(value.lat, value.lng);
        //console.log(pointLatLng);
        bounds.extend(pointLatLng);
    });
    map.fitBounds(bounds);
}

function SetBounds(id1, id2) {//, lastPoint) {
    //Create new bounds object
    var bounds = new google.maps.LatLngBounds();
    //markerBounds = new google.maps.LatLngBounds();
    //Loop through an array of points, add them to bounds
    //var data = bounds;

    for (var i = id1; i < id2; i++) {
        var _la = geoData[i].Latitude;
        var _lo = geoData[i].Longitude;

        var geoCode = new google.maps.LatLng(_la, _lo);

        bounds.extend(geoCode);
    }
    //Add new bounds object to map
    map.fitBounds(bounds);
}