// Базовый URL API для всех запросов с этой страницы
const API = '/api/cookie';

// Сохранение данных на сервер
// Сервер сам положит значения в HTTP-cookies через заголовок Set-Cookie
async function save() {
    const name = document.getElementById('name').value;
    const group = document.getElementById('group').value;

    const response = await fetch(`${API}/save`, {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        // credentials: include нужен, чтобы браузер принимал и хранил cookies от сервера
        credentials: 'include',
        body: JSON.stringify({ name, group })
    });

    const data = await response.json();
    showResult(data);
}

// Чтение cookies через сервер - JavaScript не может прочитать HttpOnly cookies напрямую
async function read() {
    const response = await fetch(`${API}/read`, {
        credentials: 'include'
    });
    const data = await response.json();
    showResult(data);
}

// Удаление cookies на сервере - отправляется Set-Cookie с истёкшей датой
async function clearData() {
    const response = await fetch(`${API}/clear`, {
        method: 'DELETE',
        credentials: 'include'
    });
    const data = await response.json();
    showResult(data);
}

// Отображение результата в блоке на странице
function showResult(data) {
    document.getElementById('result').textContent = JSON.stringify(data, null, 2);
}