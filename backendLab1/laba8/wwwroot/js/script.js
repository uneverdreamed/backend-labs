document.addEventListener('DOMContentLoaded', function () {
    const btn = document.getElementById('btn');
    const output = document.getElementById('output');

    let count = 0;

    btn.addEventListener('click', function () {
        count++;
        output.textContent = `Кнопка нажата ${count} раз`;
    });
});