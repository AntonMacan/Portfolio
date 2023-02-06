
$(document).on("click", "#dodajSadrzaj", function () {
    var odmoristeId = document.getElementById('odmoristeId').textContent
    var url = "/Odmorista/CreateSadrzaj/"
 
    $.get(url, { id: odmoristeId }, function (data) {
        $('#sadrzajiTablica').find('tbody').append(data);
    });
});

var hideButton = function () {
    document.getElementById('dodajSadrzaj').style.visibility = 'hidden'
}

var showButton = function () {
    document.getElementById('dodajSadrzaj').style.visibility = 'visible'
}
