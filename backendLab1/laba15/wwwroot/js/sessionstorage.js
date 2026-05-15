//  лючи дл€ sessionStorage - те же названи€, что и дл€ localStorage,
// но хранилища у них разные и независимые
const KEY_NAME = 'userName';
const KEY_GROUP = 'userGroup';

// ѕри загрузке восстанавливаем пол€ из sessionStorage
// ѕри первом открытии вкладки значений не будет - sessionStorage пуст дл€ новой вкладки
window.addEventListener('load', () => {
    const name = sessionStorage.getItem(KEY_NAME);
    const group = sessionStorage.getItem(KEY_GROUP);
    if (name) document.getElementById('name').value = name;
    if (group) document.getElementById('group').value = group;
});

// API sessionStorage идентичен localStorage, отличаетс€ только врем€ жизни
function save() {
    const name = document.getElementById('name').value;
    const group = document.getElementById('group').value;

    sessionStorage.setItem(KEY_NAME, name);
    sessionStorage.setItem(KEY_GROUP, group);

    showResult({ message: '—охранено в sessionStorage', name, group });
}

function read() {
    const name = sessionStorage.getItem(KEY_NAME);
    const group = sessionStorage.getItem(KEY_GROUP);

    if (!name && !group) {
        showResult({ exists: false, message: '¬ sessionStorage нет данных' });
        return;
    }

    showResult({ exists: true, name, group });
}

function clearData() {
    sessionStorage.removeItem(KEY_NAME);
    sessionStorage.removeItem(KEY_GROUP);
    showResult({ message: 'ƒанные удалены из sessionStorage' });
}

function showResult(data) {
    document.getElementById('result').textContent = JSON.stringify(data, null, 2);
}