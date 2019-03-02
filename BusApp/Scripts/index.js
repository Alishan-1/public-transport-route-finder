       
        var myfunction = function (e) {
            var input = e.target,
            list = input.getAttribute('list'),
            options = document.querySelectorAll('#' + list + ' option'),
            label = input.value;
            for (var i = 0; i < options.length; i++) {
                var option = options[i];

                if (option.value === label) {
                    input.value = option.getAttribute('value');
                    break;
                }
                else {
                    input.value = null;
                }
            }
            if (input.value == "") {
                $('#noStopMatch').css("display", "inline");
            }
            else {
                $('#noStopMatch').css("display", "none");
            }
        };
var inputs = document.querySelectorAll('input[list]');
for (var i = 0; i < inputs.length; i++) {
    inputs[i].addEventListener('blur', function (e) {
        myfunction(e);
    });
};



    $(document).ready(function () {
        $('#SearchBtn').click(function ()
        {
            if ($('#pickup').val() == '' || $('#destination').val() == '')
            {
                $('#StopsNotSelected').css("display", "inline");
            }
            else
            {
                $('#StopsNotSelected').css("display", "none");
                $('#SearchBtnTxt').html(' Loading...')
                $('#LoadingIcon').addClass('glyphicon glyphicon-refresh glyphicon-refresh-animate');
                    

                $.ajax({
                    type: 'POST',
                    data: {
                        start: $('#pickup').val(),
                        dest: $('#destination').val()
                    },
                    url: 'FindRoute/Index',
                    success: function (response) {

                        window.response = response;
                        var ressStr = '';
                        ressStr += '<b> ' + response.length + ' result(s) - Click on the result to view MAP</b><br/>';
                        for (var i = 0; i < response.length; i++) {
                            ressStr += '<div id="result' + i + '" class="results" no="'+i+'">'
                            if (response.length > 1) {
                                i++;
                                ressStr += i + ' - ';
                                i--;
                            }

                            ressStr += 'Total Length: ' + response[i].TotalLength.toFixed(2) + ' km, ';
                            ressStr += 'Total Fare: Rs.' + response[i].TotalFare.toFixed(2) + ', ';
                            ressStr += 'Total Approx Time: ' + response[i].TotalApproxTimeInMints.toFixed(1) + ' Minutes, ';
                            ressStr += 'Buses to take ' + response[i].busesToTake.length.toFixed(0) + '<br/>';
                            for (var j = 0; j < response[i].busesToTake.length; j++) {
                                if (response[i].busesToTake.length > 1 && j === 0) {
                                    ressStr += 'Firstly ';
                                }
                                else if (j > 0) {
                                    ressStr += 'Than ';
                                }
                                if (response[i].busesToTake[j].journeyDirection === 0) {
                                    ressStr += 'take Bus no <b>' + response[i].busesToTake[j].routeNo.Name + '</b> from <b>';
                                    ressStr += response[i].busesToTake[j].stops[0].Stop.Name + '</b> Stop and ride upto <b>';
                                    ressStr += response[i].busesToTake[j].stops[
                                        response[i].busesToTake[j].stops.length - 1
                                    ].Stop.Name + '</b> Stop <br/>';
                                }
                                else if (response[i].busesToTake[j].journeyDirection === 1) {
                                    ressStr += 'take Bus no <b>' + response[i].busesToTake[j].routeNo.Name + '</b> from <b>';
                                    ressStr += response[i].busesToTake[j].stops[
                                        response[i].busesToTake[j].stops.length - 1
                                    ].Stop.Name + '</b> Stop and ride upto <b>';
                                    ressStr += response[i].busesToTake[j].stops[0].Stop.Name + '</b> Stop <br/>';
                                }
                                else {
                                    ressStr += 'ERROR<br/>';
                                }
                            }
                            ressStr += '</div><br/>'
                        }
                        $('#SearchArea').html('');
                        $('#dvMap').css({
                            "background-color": "rgba(255, 255, 255, 0.7)",
                            "height": "auto",
                            "padding": "4px",
                            "font-size": "14px"
                        });
                        $('#dvMap').html(ressStr);
                        $('.results').on("click", function (e) {
                            var id = e.currentTarget.attributes["no"].value;
                            id = parseInt(id);
                            ShowMap(id, "MapDiv");
                        });
                    }
                });
            }
               
        });



    });

var ShowMap = function(id,divid)
{
    $('#'+divid).css({
        "width":"auto",
        "height": "400px",
        "padding": "4px",
        "background-color": "rgba(255, 255, 255, 0.7)",
                
    });
    var response = window.response;
    var markers = [];
    for (var j = 0; j < response[id].busesToTake.length; j++) {
                
        if (response[id].busesToTake[j].journeyDirection === 0) {
            for (var k = 0; k < response[id].busesToTake[j].stops.length; k++)
            {
                var marker = {
                    label: '',
                    title: '',
                    lat: '',
                    lng: '',
                    description: ''
                };
                marker.label = '';
                marker.title = response[id].busesToTake[j].routeNo.Name + ' ' + response[id].busesToTake[j].stops[k].Stop.Name;
                marker.lat = response[id].busesToTake[j].stops[k].Stop.Latitude;
                marker.lng = response[id].busesToTake[j].stops[k].Stop.Longitude;
                marker.description = 'Bus No. '+response[id].busesToTake[j].routeNo.Name + ' - Stop: ' + response[id].busesToTake[j].stops[k].Stop.Name;
                markers.push(marker);
            }
                    
        }
        else if (response[id].busesToTake[j].journeyDirection === 1) {
            for (var k = response[id].busesToTake[j].stops.length-1; k >=0 ; k--) {
                var marker = {
                    label: '',
                    title: '',
                    lat: '',
                    lng: '',
                    description: ''
                };
                marker.label = '';
                marker.title = response[id].busesToTake[j].routeNo.Name + ' ' + response[id].busesToTake[j].stops[k].Stop.Name;
                marker.lat = response[id].busesToTake[j].stops[k].Stop.Latitude;
                marker.lng = response[id].busesToTake[j].stops[k].Stop.Longitude;
                marker.description = 'Bus No. ' + response[id].busesToTake[j].routeNo.Name + ' - Stop: ' + response[id].busesToTake[j].stops[k].Stop.Name;
                markers.push(marker);
            }
        }
        else {
            console.log("ERROR 0x7800");
        }
    }
    //created marksrs
    initMyMap(markers, divid);
}
function initMyMap(markers, mapdivId) {
    var mapOptions = {
        center: new google.maps.LatLng(markers[0].lat, markers[0].lng),
        zoom: 10,
        mapTypeId: google.maps.MapTypeId.ROADMAP
    };
    var map = new google.maps.Map(document.getElementById(mapdivId), mapOptions);
    var infoWindow = new google.maps.InfoWindow();
    var lat_lng = new Array();
    var latlngbounds = new google.maps.LatLngBounds();
    for (i = 0; i < markers.length; i++) {
        //if(i==0 && i==markers.length -1){
        var data = markers[i]
        var myLatlng = new google.maps.LatLng(data.lat, data.lng);
        lat_lng.push(myLatlng);
        var marker = new google.maps.Marker({
            position: myLatlng,
            map: map,
            label: data.label,
            title: data.title
        });
        latlngbounds.extend(marker.position);
        (function (marker, data) {
            google.maps.event.addListener(marker, "click", function (e) {
                infoWindow.setContent(data.description);
                infoWindow.open(map, marker);
            });
        })(marker, data);
        //}
    }
    map.setCenter(latlngbounds.getCenter());
    map.fitBounds(latlngbounds);

    var JourneyPath = new google.maps.Polyline({
        path: lat_lng,
        strokeColor: "#0000FF",
        strokeOpacity: 0.8,
        strokeWeight: 2
    });
    JourneyPath.setMap(map);
}
