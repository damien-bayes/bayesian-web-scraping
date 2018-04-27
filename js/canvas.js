function saveCanvas()
{
    var image = document.getElementById("graph").toDataURL("image/png");
    image = image.replace('data:image/png;base64,', '');

    $.ajax({
        type: 'POST',
        url: 'Awesomium.aspx/UploadImage',
        data: '{ "imageData" : "' + image + '" }',
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        success: function (msg) {
            alert('Image sent!');
        },
        error: function (xhr, ajaxOptions, thrownError)
        {
            alert(xhr.responceText);
        }
    });
}