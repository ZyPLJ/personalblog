var globalAudio = new Audio("http://music.163.com/song/media/outer/url?id=1846812463.mp3");
window.addEventListener("DOMContentLoaded", function () {
    var playPauseButton = document.getElementById("play-pause-button");
    if (playPauseButton) {
        playPauseButton.addEventListener("click", function () {
            // 如果音乐正在播放，则暂停；如果已暂停，则继续播放
            if (globalAudio.paused) {
                globalAudio.play();
                playPauseButton.innerHTML = `
                <i class="bi bi-music-note-beamed"></i>
                暂停
                `;
            } else {
                globalAudio.pause();
                playPauseButton.innerHTML = `
                <i class="bi bi-music-note-beamed"></i>
                音乐
                `;
            }
        });
        // 添加一个 'ended' 事件监听器
        globalAudio.addEventListener("ended", function () {
            playPauseButton.innerHTML = `
            <i class="bi bi-music-note-beamed"></i>
            音乐
            `;
        });
    }
});