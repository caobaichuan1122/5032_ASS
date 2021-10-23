
const TOKEN = "pk.eyJ1IjoiY2FvYmFpY2h1YW4xMTIyIiwiYSI6ImNrdjE4OXhybjd0dTUycG1hNzZhZXZ4M3kifQ.e6hivVuXshq7eEOUgVHZIQ";
var locations = [];
// The first step is obtain all the latitude and longitude from the HTML
// The below is a simple jQuery selector
$(".coordinates").each(function () {
    var name = $(".name", this).text().trim();
    var latitude = $(".latitude", this).text().trim();
    var longitude = $(".longitude", this).text().trim();
    var description = $(".description", this).text().trim();
    // Create a point data structure to hold the values.
    var point = {

        "name": name,
        "latitude": latitude,
        "longitude": longitude,
        "description": description     
    };
    // Push them all into an array.
    locations.push(point);
});
console.log('locations',locations);
var data = [];
for (i = 0; i < locations.length; i++) {
    var feature = {
        "type": "Feature",
        "properties": {
            "description": locations[i].description,
            "icon": "circle-15"
        },
        "geometry": {
            "type": "Point",
            "coordinates": [locations[i].longitude, locations[i].latitude]
        }
    };
    data.push(feature)
}
mapboxgl.accessToken = TOKEN;
var map = new mapboxgl.Map({
    container: 'map',
    style: 'mapbox://styles/mapbox/streets-v10',
    zoom: 11,
    center: [locations[0].longitude, locations[0].latitude]
});
map.on('load', function () {
    // Add a layer showing the places.
    map.addLayer({
        "id": "places",
        "type": "symbol",
        "source": {
            "type": "geojson",
            "data": {
                "type": "FeatureCollection",
                "features": data
            }
        },
        "layout": {
            "icon-image": "{icon}",
            "icon-allow-overlap": true
        }
    });
    const geocoder = new MapboxGeocoder({
        accessToken: mapboxgl.accessToken,
        countries: 'au',
        bbox: [140.966348, -40.057893, 149.699059, -33.985844],
        mapboxgl: mapboxgl
    })
    map.addControl(geocoder, "top-left");
    let start;
    geocoder.on("result", function (ev) {
        console.log('ev', ev);
        var searchResult = ev.result.geometry;
        start = searchResult.coordinates;
        console.log('start', start);
    })

    map.addControl(new mapboxgl.NavigationControl());
    // When a click event occurs on a feature in the places layer, open a popup at the
    // location of the feature, with description HTML from its properties.
    map.on('click', 'places', function (e) {
        var coordinates = e.features[0].geometry.coordinates.slice();
        var description = e.features[0].properties.description;
        // Ensure that if the map is zoomed out such that multiple
        // copies of the feature are visible, the popup appears
        // over the copy being pointed to.
        if (start) {
            getRoute(start, coordinates);
        }
        while (Math.abs(e.lngLat.lng - coordinates[0]) > 180) {
            coordinates[0] += e.lngLat.lng > coordinates[0] ? 360 : -360;
        }
        new mapboxgl.Popup()
            .setLngLat(coordinates)
            .setHTML(description)
            .addTo(map);
    });
    // Change the cursor to a pointer when the mouse is over the places layer.
    map.on('mouseenter', 'places', function () {
        map.getCanvas().style.cursor = 'pointer';
    });
    // Change it back to a pointer when it leaves.
    map.on('mouseleave', 'places', function () {
        map.getCanvas().style.cursor = '';
    });
});

/*async function getRoute(start, end) {
    const query = await fetch(
       'https://api.mapbox.com/directions/v5/mapbox/cycling/${start[0]},${start[1]};${end[0]},${end[1]}?steps=true&geometries=geojson&access_token=${mapboxgl.accessToken}',
        { method: 'GET' }
    )*/

async function getRoute(start, end) {
    const query = await fetch(
        `https://api.mapbox.com/directions/v5/mapbox/cycling/${start[0]},${start[1]};${end[0]},${end[1]}?steps=true&geometries=geojson&access_token=${mapboxgl.accessToken}`,
        { method: 'GET' }
    );


    const json = await query.json();
    const data = json.routes[0];
    const route = data.geometry.coordinates;
    const geojson = {
        type: 'Feature',
        properties: {},
        geometry: {
            type: 'LineString',
            coordinates: route
        }
    };
    if (map.getSource('route')) {
        map.getSource('route').setData(geojson);
    } else {
        map.addLayer({
            id: 'route',
            type: 'line',
            source: {
                type: 'geojson',
                data: 'geojson'
            },
            layout: {
            'line-join': 'round',
            'line-cap': 'round'
        },
            paint: {
                'line-color': '#3887be',
                'line-width': 5,
                'line-opacity':0.75
             }
        });
    }
}