
let app = new Vue({
    el: '#vue-header',
    data: {
        currentTheme: '',
        themes: [
            { "name": "Bootstrap" },
            { "name": "lux", "cssUrl": "/css/lux/bootstrap.min.css" },
            { "name": "cerulean", "cssUrl": "/css/cerulean/bootstrap.min.css" },
            { "name": "cosmo", "cssUrl": "/css/cosmo/bootstrap.min.css" },
            { "name": "cyborg", "cssUrl": "/css/cyborg/bootstrap.min.css" },
            { "name": "darkly", "cssUrl": "/css/darkly/bootstrap.min.css" },
            { "name": "flatly", "cssUrl": "/css/flatly/bootstrap.min.css" },
            { "name": "journal", "cssUrl": "/css/journal/bootstrap.min.css" },
            { "name": "litera", "cssUrl": "/css/litera/bootstrap.min.css" },
            { "name": "lumen", "cssUrl": "/css/lumen/bootstrap.min.css" },
            { "name": "materia", "cssUrl": "/css/materia/bootstrap.min.css" },
            { "name": "minty", "cssUrl": "/css/minty/bootstrap.min.css" },
            { "name": "morph", "cssUrl": "/css/morph/bootstrap.min.css" },
            { "name": "pulse", "cssUrl": "/css/pulse/bootstrap.min.css" },
            { "name": "quartz", "cssUrl": "/css/quartz/bootstrap.min.css" },
            { "name": "sandstone", "cssUrl": "/css/sandstone/bootstrap.min.css" },
            { "name": "simplex", "cssUrl": "/css/simplex/bootstrap.min.css" },
            { "name": "sketchy", "cssUrl": "/css/sketchy/bootstrap.min.css" },
            { "name": "slate", "cssUrl": "/css/slate/bootstrap.min.css" },
            { "name": "solar", "cssUrl": "/css/solar/bootstrap.min.css" },
            { "name": "spacelab", "cssUrl": "/css/spacelab/bootstrap.min.css" },
            { "name": "superhero", "cssUrl": "/css/superhero/bootstrap.min.css" },
            { "name": "united", "cssUrl": "/css/united/bootstrap.min.css" },
            { "name": "vapor", "cssUrl": "/css/vapor/bootstrap.min.css" },
            { "name": "yeti", "cssUrl": "/css/yeti/bootstrap.min.css" },
            { "name": "zephyr", "cssUrl": "/css/zephyr/bootstrap.min.css" },
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