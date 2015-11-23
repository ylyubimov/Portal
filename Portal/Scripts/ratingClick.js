function OnLikeClick(url, id) {
    $.ajax({
        url: url,
        method: 'POST',
        dataType: 'json',
        success: function (data) {
            var likesCount = document.getElementById("LikesCount" + id);
            likesCount.innerText = data.likesCount;
            var dislikesCountLink = document.getElementById("DislikesCountOnClick" + id);
            var likesCountLink = document.getElementById("LikesCountOnClick" + id);
            if (likesCountLink.style.opacity != 0.4) {
                if (dislikesCountLink.style.opacity == 0.4) {
                    dislikesCountLink.style.opacity = 1;
                    $('#' + "DislikesCountOnClick" + id).on('click');
                    likesCountLink.style.opacity = 1;
                } else {
                    dislikesCountLink.style.opacity = 0.4;
                    $('#' + "DislikesCountOnClick" + id).off('click');
                }
            }
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
            var dislikesCountLink = document.getElementById("DislikesCountOnClick" + id);
            var likesCountLink = document.getElementById("LikesCountOnClick" + id);
            if (dislikesCountLink.style.opacity != 0.4) {
                if (likesCountLink.style.opacity == 0.4) {
                    dislikesCountLink.style.opacity = 1;
                    likesCountLink.style.opacity = 1;
                    $('#' + "LikesCountOnClick" + id).on('click');
                }
                else {
                    likesCountLink.style.opacity = 0.4;
                    $('#' + "LikesCountOnClick" + id).off('click');
                }
            }
        },
        error: function () {
            alert("Не удалось поставить дислайк, возможно, вы недостаточно недовольны.");
        }
    });
}