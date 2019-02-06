window.ENV = null;
window.currencies = [];

$(function () {
    window.ENV = getENV();
    refreshList();

    $('#refresher').click(() => {
        refreshList();
    });


    $('#formConversao').submit(evt => {
        evt.preventDefault();
        const params = {
            'moedaOrigem': $('#moedaOrigem').val(),
            'moedaDestino': $('#moedaDestino').val(),
            'valor': $('#valor').val()
        };

        if(params.moedaOrigem == '' || params.moedaDestino == '' || params.valor == '') {
            console.log(params);
            $('#mainAlert').addClass('alert-warning');
            $('#mainAlert').text('Por favor preencha as informações');
            $('#mainAlert').alert();

            setTimeout(() => {
                $('#mainAlert').removeClass('alert-warning');
                $('#mainAlert').alert('close');
            }, 2000)
            return;
        }
        
        requestFactory(
            'moedas/ConverterMoeda',
            'POST',
            params,
            (response) => {
                console.info('EXECUTED CALLBACK');
            }
        );    
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
        contentType: "application/json",
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