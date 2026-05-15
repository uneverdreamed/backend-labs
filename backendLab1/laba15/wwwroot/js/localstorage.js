// Ключи для хранения значений в localStorage
// Вынесены в константы, чтобы избежать опечаток при чтении и записи
const KEY_NAME = 'userName';
const KEY_GROUP = 'userGroup';

// При загрузке страницы автоматически подставляем сохранённые значения в поля ввода
// Это демонстрирует, что localStorage переживает закрытие браузера
window.addEventListener('load', () => {
    const name = localStorage.getItem(KEY_NAME);
    const group = localStorage.getItem(KEY_GROUP);
    if (name) document.getElementById('name').value = name;
    if (group) document.getElementById('group').value = group;
});

// Сохранение значений в localStorage
// setItem принимает только строки - объекты нужно сериализовать через JSON.stringify
function save() {
    const name = document.getElementById('name').value;
    const group = document.getElementById('group').value;

    localStorage.setItem(KEY_NAME, name);
    localStorage.setItem(KEY_GROUP, group);

    showResult({ message: 'Сохранено в localStorage', name, group });
}

// Чтение значений из localStorage
// getItem возвращает null, если ключа нет
function read() {
    const name = localStorage.getItem(KEY_NAME);
    const group = localStorage.getItem(KEY_GROUP);

    if (!name && !group) {
        showResult({ exists: false, message: 'В localStorage нет данных' });
        return;
    }

    showResult({ exists: true, name, group });
}

// Удаление значений из localStorage
// removeItem удаляет конкретный ключ, есть ещё localStorage.clear() для полной очистки
function clearData() {
    localStorage.removeItem(KEY_NAME);
    localStorage.removeItem(KEY_GROUP);
    showResult({ message: 'Данные удалены из localStorage' });
}

// Пятая задача - отправка данных из localStorage на сервер
// Сервер обрабатывает данные и возвращает обогащённый ответ
async function syncWithServer() {
    const name = localStorage.getItem(KEY_NAME) || '';
    const group = localStorage.getItem(KEY_GROUP) || '';

    const response = await fetch('/api/profile/sync', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({ name, group })
    });

    const data = await response.json();
    document.getElementById('serverResponse').textContent =
        'Ответ сервера:\n' + JSON.stringify(data, null, 2);
}

function showResult(data) {
    document.getElementById('result').textContent = JSON.stringify(data, null, 2);
}