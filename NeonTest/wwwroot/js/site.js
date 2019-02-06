window.ENV = null;
window.currencies = [];

$(function () {
    window.ENV = getENV();
    refreshList();

    $('#refresher').click(() => {
        refreshList();
    });
});

function refreshList() {
    requestFactory(
        'moedas/listagem',
        'GET',
        null,
        (response) => {
            console.info('EXECUTED CALLBACK');
            window.currencies = response.data;

            let optionsBuild = '';
            response.data.forEach(item => {
                optionsBuild += `<option value="${item.sigla}">${item.sigla} - ${item.nome}</option>`;
            });

            $('#moedaOrigem').append(optionsBuild);
            $('#moedaDestino').append(optionsBuild);

            optionsBuild = null;
        }
    );
}

function requestFactory(_url, _method, _params, _callback) {
    console.info(`${window.ENV.BASE_URL}${_url}`);

    $.ajax({
        url: `${ENV.BASE_URL}${_url}`,
        method: _method,
        data: _params
    })
    .then(response => {
        if (_callback)
            _callback(response);
    })
    .always(() => {
        console.info('ENDED');
    });
}

function getENV() {
    if (window.location.href.indexOf('localhost') > -1) {
        return {
            "BASE_URL": "http://localhost:58204/"
        }
    } else {
        return {
            "BASE_URL": ""
        }
    }
}