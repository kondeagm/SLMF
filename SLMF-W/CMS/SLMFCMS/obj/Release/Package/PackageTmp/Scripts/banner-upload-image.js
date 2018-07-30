$(function () {
    var dropbox = $('#dropbox-zone'),
        imageGallery = $('#image-uploading'),
        message = $('.message', dropbox),
        fileIndex = 0;
    var id_image_file = $("#idbanner").val();

    dropbox.filedrop({
        paramname: 'file',
        maxfiles: 1,
        maxfilesize: 1,
        url: '/Banner/UploadImage/' + id_image_file,
        uploadFinished: function (i, file, response) {
            var json = JSON.parse(response);
            if (json.estado > 0) {
                alert(json.mensaje);
                $('#loader-wait').remove();
                $('#templatepreview').remove();
                document.getElementById('main-container-drop').style.display = 'inherit';
            }
            else {
                $('#loader-wait').remove();
                $('#image-uploading').show();
                $('#buttonback').text('Regresar');
                $.data(file).addClass('done');
                alert('El Archivo se Actualizo con Exito !!!');
            }
            $('#buttonback').show();
        },
        error: function (err, file) {
            switch (err) {
                case 'BrowserNotSupported':
                    showMessage('Error: Su navegador no soporta HTML 5, no podra subir el Archivo.');
                    break;
                case 'TooManyFiles':
                    alert('Error: Solo puede subir un solo Archivo.');
                    break;
                case 'FileTooLarge':
                    alert('Error: La imagen es demasiado grande, por favor suba archivos de menos de 1 mb.');
                    break;
                default:
                    break;
            }
        },
        beforeEach: function (file) {
            if (!file.type.match(/^image\//)) {
                alert('Error: Solo puede cargar Imagenes.');
                return false;
            }
        },
        uploadStarted: function (i, file) {
            document.getElementById('main-container-drop').style.display = 'none';
            $('#buttonback').hide();
            fileIndex++;
            createImage(file);
        }
    });

    var template =
        '<div id="templatepreview" class="preview">' +
            '<span class="imageHolder">' +
                '<img id="logotipo125x425" />' +
                '<span class="uploaded"></span>' +
            '</span>' +
        '</div>' +
        '<div id="loader-wait">' +
            '<img src="../../Content/img/uploading.gif" />' +
        '</div>';

    function createImage(file) {
        var preview = $(template), image = $('#logotipo125x425', preview);
        var reader = new window.FileReader();
        reader.onload = function (e) {
            image.attr('src', e.target.result);
        };
        reader.readAsDataURL(file);
        preview.appendTo(imageGallery);
        $.data(file, preview);
    }

    function showMessage(msg) {
        message.html(msg);
    }
});