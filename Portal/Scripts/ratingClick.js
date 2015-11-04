function OnLikeClick(url, id) {
    $.ajax({
        url: url,
        method: 'POST',
        dataType: 'json',
        success: function (data) {
            var likesCount = document.getElementById("LikesCount" + id);
            likesCount.innerText = data.likesCount;
        },
        error: function () {
            alert("Не удалось поставить лайк, возможно, вам недостаточно нравится.");
        }
    });
}

function OnDislikeClick(url, id) {
    $.ajax({
        url: url,
        method: 'POST',
        dataType: 'json',
        success: function (data) {
            var dislikesCount = document.getElementById("DislikesCount" + id);
            dislikesCount.innerText = data.dislikesCount;
        },
        error: function () {
            alert("Не удалось поставить дислайк, возможно, вы недостаточно недовольны.");
        }
    });
}