$(function () {
    var dropbox = $('#dropbox'),
        imageGallery = $('#image-uploaded-166x166'),
        message = $('.message', dropbox),
        fileIndex = 0;
    var id_image_file = $("#idmusculo").val();

    dropbox.filedrop({
        paramname: 'file',
        maxfiles: 1,
        maxfilesize: 1,
        url: '/Musculo/UploadImage/' + id_image_file,
        uploadFinished: function (i, file, response) {
            $('#waitter-166x166').hide();
            var json = JSON.parse(response);
            if (json.estado > 0) {
                alert(json.mensaje);
                $('#templatepreview166x166').remove();
                document.getElementById('main-container-drop-166x166').style.display = 'inherit';
            }
            else {
                $('#image-uploaded-166x166').show();
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
            document.getElementById('main-container-drop-166x166').style.display = 'none';
            $('#buttonback').hide();
            $('#waitter-166x166').show();
            fileIndex++;
            createImage(file);
        }
    });

    var template =
        '<div id="templatepreview166x166" class="preview">' +
            '<span class="imageHolder">' +
                '<img id="logotipo" />' +
                '<span class="uploaded"></span>' +
            '</span>' +
            '<div id="waitter-166x166">' +
                '<img src="../../Content/img/loader.gif" />' +
            '</div>' +
        '</div>';

    function createImage(file) {
        var preview = $(template), image = $('#logotipo', preview);
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