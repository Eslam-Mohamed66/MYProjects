


function sendReact(idpost, iduser, react) {
   

    var fa = new FormData();
    fa.append("idpost",idpost);
    fa.append("iduser",iduser)
    fa.append("react", react);

    $.ajax({
        url: 'https://localhost:44311/api/React',
        type: 'POST',
        data: fa,
        cache: false,
        contentType: false,
        processData: false,
        success: function (data) {
            console.log(data);
            location.reload();
        }
    });
}

