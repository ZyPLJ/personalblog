
let app = new Vue({
    el: '#vue-header',
    data: {
        currentTheme: '',
        themes: [
            { "name": "Bootstrap" },
            { "name": "lux", "cssUrl": "https://cdn.bootcdn.net/ajax/libs/bootswatch/5.2.3/lux/bootstrap.min.css" },
            { "name": "cerulean", "cssUrl": "https://cdn.bootcdn.net/ajax/libs/bootswatch/5.2.3/cerulean/bootstrap.min.css" },
            { "name": "cosmo", "cssUrl": "https://cdn.bootcdn.net/ajax/libs/bootswatch/5.2.3/cosmo/bootstrap.min.css" },
            { "name": "cyborg", "cssUrl": "https://cdn.bootcdn.net/ajax/libs/bootswatch/5.2.3/cyborg/bootstrap.min.css" },
            { "name": "darkly", "cssUrl": "https://cdn.bootcdn.net/ajax/libs/bootswatch/5.2.3/darkly/bootstrap.min.css" },
            { "name": "flatly", "cssUrl": "https://cdn.bootcdn.net/ajax/libs/bootswatch/5.2.3/flatly/bootstrap.min.css" },
            { "name": "journal", "cssUrl": "https://cdn.bootcdn.net/ajax/libs/bootswatch/5.2.3/journal/bootstrap.min.css" },
            { "name": "litera", "cssUrl": "https://cdn.bootcdn.net/ajax/libs/bootswatch/5.2.3/litera/bootstrap.min.css" },
            { "name": "lumen", "cssUrl": "https://cdn.bootcdn.net/ajax/libs/bootswatch/5.2.3/lumen/bootstrap.min.css" },
            { "name": "materia", "cssUrl": "https://cdn.bootcdn.net/ajax/libs/bootswatch/5.2.3/materia/bootstrap.min.css" },
            { "name": "minty", "cssUrl": "https://cdn.bootcdn.net/ajax/libs/bootswatch/5.2.3/minty/bootstrap.min.css" },
            { "name": "morph", "cssUrl": "https://cdn.bootcdn.net/ajax/libs/bootswatch/5.2.3/morph/bootstrap.min.css" },
            { "name": "pulse", "cssUrl": "https://cdn.bootcdn.net/ajax/libs/bootswatch/5.2.3/pulse/bootstrap.min.css" },
            { "name": "quartz", "cssUrl": "https://cdn.bootcdn.net/ajax/libs/bootswatch/5.2.3/quartz/bootstrap.min.css" },
            { "name": "sandstone", "cssUrl": "https://cdn.bootcdn.net/ajax/libs/bootswatch/5.2.3/sandstone/bootstrap.min.css" },
            { "name": "simplex", "cssUrl": "https://cdn.bootcdn.net/ajax/libs/bootswatch/5.2.3/simplex/bootstrap.min.css" },
            { "name": "sketchy", "cssUrl": "https://cdn.bootcdn.net/ajax/libs/bootswatch/5.2.3/sketchy/bootstrap.min.css" },
            { "name": "slate", "cssUrl": "https://cdn.bootcdn.net/ajax/libs/bootswatch/5.2.3/slate/bootstrap.min.css" },
            { "name": "solar", "cssUrl": "https://cdn.bootcdn.net/ajax/libs/bootswatch/5.2.3/solar/bootstrap.min.css" },
            { "name": "spacelab", "cssUrl": "https://cdn.bootcdn.net/ajax/libs/bootswatch/5.2.3/spacelab/bootstrap.min.css" },
            { "name": "superhero", "cssUrl": "https://cdn.bootcdn.net/ajax/libs/bootswatch/5.2.3/superhero/bootstrap.min.css" },
            { "name": "united", "cssUrl": "https://cdn.bootcdn.net/ajax/libs/bootswatch/5.2.3/united/bootstrap.min.css" },
            { "name": "vapor", "cssUrl": "https://cdn.bootcdn.net/ajax/libs/bootswatch/5.2.3/vapor/bootstrap.min.css" },
            { "name": "yeti", "cssUrl": "https://cdn.bootcdn.net/ajax/libs/bootswatch/5.2.3/yeti/bootstrap.min.css" },
            { "name": "zephyr", "cssUrl": "https://cdn.bootcdn.net/ajax/libs/bootswatch/5.2.3/zephyr/bootstrap.min.css" },
        ]
    },
    created: function () {
        //fetch('/Api/Theme')
        //    .then(res => res.json())
        //    .then(res => {
        //        this.themes = res.data
        //    })

        // 读取本地主题配置
        let theme = localStorage.getItem('currentTheme')
        if (theme != null) this.currentTheme = theme
    },
    methods: {
        setTheme(themeName) {
            let theme = this.themes.find(t => t.name === themeName)
            loadStyles(theme.cssUrl)
            this.currentTheme = themeName
            localStorage.setItem('currentTheme', themeName)
            localStorage.setItem('currentThemeCssUrl', theme.cssUrl)
            // 换主题之后最好要刷新页面，不然可能样式冲突
            location.reload()
        }
    }
})


let toastTrigger = document.getElementById('liveToastBtn')
let toastLiveExample = document.getElementById('liveToast')

if (toastTrigger) {
    toastTrigger.addEventListener('click', function () {
        let toast = new bootstrap.Toast(toastLiveExample)
        toast.show()
    })
}
