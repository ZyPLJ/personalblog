function randomColorArray() {
    return [
        Math.round(Math.random() * 255),
        Math.round(Math.random() * 255),
        Math.round(Math.random() * 255),
    ]
}

function convertRGBA(rgbArray, a) {
    let color = Array.from(rgbArray)
    color.push(a)
    return 'rgba(' + color.join(',') + ')'
}

var ctx = document.getElementById("myChart");
$.getJSON('https://pljzy.top/Api/Category/WordCloud', function (res) {
    var labels = [];
    var values = [];
    var backgroundColors = [];
    var borderColors = [];
    $.each(res.data, function (index, item) {
        labels.push(item.name);
        values.push(item.value);
        var color = randomColorArray();
        backgroundColors.push(convertRGBA(color, 0.2));
        borderColors.push(convertRGBA(color, 1));
    });
    console.log(labels)
    console.log(values)
    let data = {
        labels: labels,
        datasets: [{
            label: '# of Votes',
            data: values,
            backgroundColor: backgroundColors,
            borderColor: borderColors,
            borderWidth: 1
        }]
    };
    let config = {
        type: 'bar',
        data: data,
        options: {
            maintainAspectRatio: false,
        }
    };
    var currentChart = new Chart(ctx, config);
});
