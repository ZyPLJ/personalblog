const HEIGHT = 400
const { onMounted } = Vue

const homeApp = Vue.createApp({
    setup() {
        const poem = ref({});
        const hitokoto = ref({});
        const poemSimple = ref('');
        const chartTypes = ['bubble', 'bar'];
        const currentChartTypeIndex = ref(0);
        const currentChart = ref(null);
        const imageSrc = ref('');
        const isLoading = ref(true);
        const imageSrcArray = ref([]);
        const isLoadingArray = ref(true);
        
        const chartElem = ref(null);

        // 获取随机图片的方法
        const getRandomImages = async () => {
            try {
                const response = await fetch('/PicLib/GetRandomImageTopQiliu');
                return await response.json();
            } catch (error) {
                console.error(error);
                return [];
            }
        };
        const loadPoem = () => {
            fetch('/Api/DataAcq/Poem')
                .then(res => res.json())
                .then(res => {poemSimple.value = res.data;});
        };

        
        const loadHitokoto = () => {
            fetch('/Api/DataAcq/Hitokoto')
                .then(res => res.json())
                .then(res => (hitokoto.value = res.data));
        };

        const randomRGB = () => {
            return 'rgb(' + randomColorArray().join(',') + ')';
        };

        const randomRGBA = (a) => {
            return convertRGBA(randomColorArray(), a);
        };

        const convertRGBA = (rgbArray, a) => {
            let color = Array.from(rgbArray);
            color.push(a);
            return 'rgba(' + color.join(',') + ')';
        };

        const randomColorArray = () => {
            return [
                Math.round(Math.random() * 255),
                Math.round(Math.random() * 255),
                Math.round(Math.random() * 255),
            ];
        };

        const switchChartType = () => {
            if (currentChartTypeIndex.value >= chartTypes.length - 1)
                currentChartTypeIndex.value = 0;
            else currentChartTypeIndex.value++;
            if (currentChart.value) currentChart.value.destroy();
            chartElem.value.setAttribute('style', '');
            loadChart();
        };

        const loadChart = () => {
            let chartType = chartTypes[currentChartTypeIndex.value];
            switch (chartType) {
                case 'bubble':
                    loadBubbleChart();
                    break;
                case 'bar':
                    loadBarChart();
                    break;
                default:
            }
        };

        const loadBubbleChart = () => {
            fetch('/Api/Category/WordCloud')
                .then(res => res.json())
                .then(res => {
                    let datasets = [];
                    res.data.forEach(item => {
                        let color = randomColorArray();
                        datasets.push({
                            label: item.name,
                            data: [
                                {
                                    x: Math.round(Math.random() * 50),
                                    y: Math.round(Math.random() * 50),
                                    r: item.value,
                                },
                            ],
                            backgroundColor: convertRGBA(color, 0.2),
                            borderColor: convertRGBA(color, 1),
                            borderWidth: 1,
                        });
                    });
                    let data = {
                        datasets: datasets,
                    };
                    let config = {
                        type: 'bubble',
                        data: data,
                        options: {
                            maintainAspectRatio: false,
                        },
                    };
                    currentChart.value = new Chart(chartElem.value, config);
                    currentChart.value.resize(null, HEIGHT);
                });
        };

        const loadBarChart = () => {
            fetch('/Api/Category/WordCloud')
                .then(res => res.json())
                .then(res => {
                    let labels = [];
                    let values = [];
                    let backgroundColors = [];
                    let borderColors = [];
                    res.data.forEach(item => {
                        labels.push(item.name);
                        values.push(item.value);
                        let color = randomColorArray();
                        backgroundColors.push(convertRGBA(color, 0.2));
                        borderColors.push(convertRGBA(color, 1));
                    });
                    let data = {
                        labels: labels,
                        datasets: [
                            {
                                label: '# of Votes',
                                data: values,
                                backgroundColor: backgroundColors,
                                borderColor: borderColors,
                                borderWidth: 1,
                            },
                        ],
                    };
                    let config = {
                        type: 'bar',
                        data: data,
                        options: {
                            maintainAspectRatio: false,
                        },
                    };

                    currentChart.value = new Chart(chartElem.value, config);
                    currentChart.value.resize(null, HEIGHT);
                });
        };

        onMounted(async () => {
            loadPoem();
            loadHitokoto();
            const images = await getRandomImages();
            imageSrcArray.value = images;
            isLoadingArray.value = false;

            if (CHART_VISIBLE === true) loadChart();
            // Enable tooltips
            console.log('Enable tooltips')
            const tooltipTriggerList = document.querySelectorAll('[data-bs-toggle="tooltip"]')
            const tooltipList = [...tooltipTriggerList].map(tooltipTriggerEl => new bootstrap.Tooltip(tooltipTriggerEl))

            let liveToast2 = document.getElementById('liveToast2')
            let toast2 = new bootstrap.Toast(liveToast2)
            toast2.show()
            let liveToast3 = document.getElementById('liveToast3')
            let toast3 = new bootstrap.Toast(liveToast3)
            toast3.show()
            var exampleModal = new bootstrap.Modal(document.getElementById('exampleModal'))
            exampleModal.show()
        });

        return {
            poem,
            hitokoto,
            poemSimple,
            chartTypes,
            currentChartTypeIndex,
            currentChart,
            imageSrc,
            isLoading,
            chartElem,
            getRandomImages,
            loadPoem,
            loadHitokoto,
            randomRGB,
            randomRGBA,
            convertRGBA,
            randomColorArray,
            switchChartType,
            loadChart,
            loadBubbleChart,
            loadBarChart,
            imageSrcArray,
            isLoadingArray
        };
    },
})
homeApp.mount("#vue-app")