﻿function OnDocumentLoad(url, id) {
    var $input;

        $input = $("#DocUpload" + id);
    var files = $input.prop('files');
    if (files.length == 1) {
        if (window.FormData !== undefined) {
            var docData = new FormData();
            docData.append("file", files[0]);
            docData.append("success", "Файл успешно загружен!");
            docData.append("id", id);
           
            //Ajax запрос
            $.ajax({
                type: "POST",
                url: url,
                contentType: false,
                processData: false,
                data: docData, //передаем файл на сервер
                success: function (data) {
                    var a = document.createElement("a");
                    a.setAttribute("href", data[0]);
                    a.textContent = data[1];
                    var aDelete = document.createElement("a");
                    aDelete.setAttribute("href", data[2]);
                    aDelete.textContent = "x";
                    aDelete.setAttribute("class", "delete-doc");
                    var newDoc = document.createElement("div");
                    newDoc.setAttribute("class", "document");
                    newDoc.setAttribute("id", "document" + data[1]);
                    newDoc.appendChild(a);
                    newDoc.appendChild(aDelete);
                    document.getElementById("uploadedDocs").appendChild(newDoc);
                },
                error: function (xhr, status, p3) {
                    alert(xhr.responseText);
                }
            });
        } else {
            alert("Ваш браузер не поддерживает Html5!");
        }
    }

}

function OnCreateDocumentLoad(url) {
    var $input;

    $input = $('input[id^="DocUploadCreate_"]:last');
    var files = $input.prop('files');
    if (files.length == 1) {
        if (window.FormData !== undefined) {
            var docData = new FormData();
            docData.append("file", files[0]);
            docData.append("success", "Файл успешно загружен!");

            //Ajax запрос
            $.ajax({
                type: "POST",
                url: url,
                contentType: false,
                processData: false,
                data: docData, //передаем файл на сервер
                success: function (data) {
                    var aDelete = document.createElement("a");
                    aDelete.textContent = "x";
                    aDelete.setAttribute("class", "delete-doc");

                    var $div = $('div[id^="docUploader_"]:last');
                    var num = parseInt($div.prop("id").match(/\d+/g), 10) + 1;
                    var newDiv = $div.clone().prop('id', 'docUploader_' + num);
                    newDiv.find("input").prop('id', 'DocUploadCreate_' + num).prop('class', 'document');
                    newDiv.insertAfter($div);
                    var a = $div.find("a");
                   
                    newDiv.find("a").bind("click", function () { $(this).parent().find('input').click(); });
                    a.text(data[1]);
                    a.prop('href', data[0]);
                    a.removeClass("upload");
                    a.unbind("click");
                    $(aDelete).bind("click", function () { $(aDelete).parent().remove(); });
                    $(aDelete).insertAfter(a);

                },
                error: function (xhr, status, p3) {
                    alert(xhr.responseText);
                }
            });
        } else {
            alert("Ваш браузер не поддерживает Html5!");
        }
    }

}

function OnDelete(elem) {   
    $(elem).parent().remove();
}

function Send(id) {
    var $input = $("input[id='" + id + "']");
    $input.click();
    var $href = $("a[id='" + id + "']");
    $href.next().remove();
    $href.remove();

}