//音乐播放
const host = window.location.protocol + '//' + window.location.hostname + (window.location.port ? ':' + window.location.port : '');
const musicUrl1 = host + "/api/music/1.mp3";
const musicUrl2 = host + "/api/music/2.mp3";
const musicUrl3 = host + "/api/music/3.mp3";

const core = window._PlayerCore


core.AppendSongOnTail({
        name: "Dian Ji Xiao Zi",
        id: 1,
        src: musicUrl1,
        img: "https://p1.music.126.net/u90NnX64TqnsAn-4mb0yfQ==/2946691187826531.jpg?param=200y200"
})

core.AppendSongOnTail({
    name: "Gui Mie",
    id: 2,
    src: musicUrl2,
    img: "https://p1.music.126.net/u90NnX64TqnsAn-4mb0yfQ==/2946691187826531.jpg?param=200y200"
})

core.AppendSongOnTail(
    {
        name: "Love",
        id: 3,
        src: musicUrl3,
        img: 'https://p1.music.126.net/u90NnX64TqnsAn-4mb0yfQ==/2946691187826531.jpg?param=200y200'
    }
)