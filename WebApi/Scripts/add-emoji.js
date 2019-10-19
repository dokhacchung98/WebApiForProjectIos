function addEmoji() {
    var name = $("#txt-name").val();
    var path = $("#txt-path").val();
    var idType = $("#txt-type").val();

    var myUrl = window.location.origin + '/api/MyApi/AddEmoji?character=' + name + '&pathEmoji=' + path + '&idType=' + idType;
    console.log(myUrl);
    $.ajax({
        contentType: 'application/json',
        type: "GET",
        url: myUrl,
        success: function () {
            //  location.reload();
            alert('ok');
        },
        error: function () {
        }
    });
}

function addTypeEmoji() {
    var name = $("#txt-name-type").val();
    var path = $("#txt-path-type").val();
    var myUrl = window.location.origin + '/api/MyApi/AddTypeEmoji?name=' + name + '&path=' + path;
    console.log(myUrl);
    $.ajax({
        contentType: 'application/json',
        type: "GET",
        url: myUrl,
        success: function () {
            location.reload();
        },
        error: function () {
        }
    });
}

function removeTypeEmoji(name) {
    var myUrl = window.location.origin + "/api/MyApi/RemoveTypeEmoji?name=" + name;
    console.log('remove item id: ' + myUrl);

    $.ajax({
        contentType: 'application/json',
        type: "GET",
        url: myUrl,
        success: function () {
            location.reload();
        },
        error: function () {
        }
    });
}

function removeEmoji(id) {
    console.log('remove item id: ' + id);

    var myUrl = window.location.origin + "/api/MyApi/RemoveEmoji?id=" + id;

    $.ajax({
        contentType: 'application/json',
        type: "GET",
        url: myUrl,
        success: function () {
            location.reload();
        },
        error: function () {
        }
    });
}