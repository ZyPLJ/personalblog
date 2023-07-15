const { ref, onMounted } = Vue
let app = Vue.createApp({
    setup() {
        const currentTheme = ref('');
        const themes = [
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
        ];

        const loadStyles = (cssUrl) => {
            // 实现加载样式的逻辑
            let link = document.createElement("link");
            link.rel = "stylesheet";
            link.type = "text/css";
            link.href = cssUrl;
            let head = document.getElementsByTagName("head")[0];
            head.appendChild(link);
        };

        const setTheme = (themeName) => {
            const theme = themes.find(t => t.name === themeName);
            if (theme) {
                loadStyles(theme.cssUrl);
                currentTheme.value = themeName;
                localStorage.setItem('currentTheme', themeName);
                localStorage.setItem('currentThemeCssUrl', theme.cssUrl);
                location.reload();
            }
        };

        let toastTrigger;
        let toastLiveExample;
        let liveToast2;
        let toast2;
        let liveToast3;
        let toast3;

        onMounted(()=>{
            let currentTheme = localStorage.getItem('currentTheme')
            if (currentTheme !== 'Bootstrap') {
                let themeCssUrl = localStorage.getItem('currentThemeCssUrl')
                if (themeCssUrl != null) loadStyles(themeCssUrl)
            }

            toastTrigger = document.getElementById('liveToastBtn');
            toastLiveExample = document.getElementById('liveToast');

            if (toastTrigger) {
                toastTrigger.addEventListener('click', function () {
                    let toast = new bootstrap.Toast(toastLiveExample);
                    toast.show();
                });
            }
        })

        // 在created钩子中执行逻辑
        const created = () => {
            let theme = localStorage.getItem('currentTheme');
            if (theme) {
                currentTheme.value = theme;
            }
        };
        
        return {
            currentTheme,
            themes,
            setTheme,
            created
        };
    }
})

app.mount('#vue-header')
