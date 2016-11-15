function OnDocumentLoad(url, id) {
    var $input = $("#DocUpload" + id);
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