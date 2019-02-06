window.ENV = null;
window.currencies = [];

$(function () {
    window.ENV = getENV();
    listaMoedas();

    $('#refresher').click(() => {
        listaMoedas();
    });


    $('#formConversao').submit(evt => {
        evt.preventDefault();
        let params = {
            'moedaOrigem': $('#moedaOrigem').val(),
            'moedaDestino': $('#moedaDestino').val(),
            'valor': $('#valor').val()
        };

        if(validaParametros(params)) {
            console.log(params);
            mostraAlerta('Por favor preencha todos os campos' , 'alert-warning', true);
            return;
        }

        params.moedaDestino = window.currencies.filter(item => item.sigla === params.moedaDestino)[0];
        params.moedaOrigem = window.currencies.filter(item => item.sigla === params.moedaOrigem)[0];
        
        requestFactory(
            'moedas/ConverterMoeda',
            'POST',
            params,
            (response) => {
                const _data = response.data;
                console.log('EXECUTED', response);

                $('#resultadoConversao').removeClass('d-none');
                $('#resultadoConversao .title').text(`${_data.moedaOrigem.sigla} -> ${_data.moedaDestino.sigla}`);
                $('#resultadoConversao #moedaDestinoCotacao').html(`${_data.moedaOrigem.valor}`);
                $('#resultadoConversao #moedaOrigemCotacao').html(`${_data.moedaDestino.valor}`);

                $('#resultadoConversao #quantidade').html(`<strong>Valor:</strong> ${_data.moedaOrigem.sigla} ${_data.valor.toFixed(2)}`);
                $('#resultadoConversao #resultado').html(`<strong>Resultado:</strong> ${_data.moedaDestino.sigla} ${_data.resultado.toFixed(2)}`);
            });    
    });
});


function validaParametros (params) {
    return params.moedaOrigem == '' || params.moedaDestino == '' || params.valor == '';
}

function mostraAlerta(mensagem, tipo, autoEsconde) {
    $('#mainAlert').addClass(`${tipo} show`);
    $('#mainAlert').text(mensagem);
    $('#mainAlert').alert();

    if(autoEsconde){
        setTimeout(() => {
            escondeAlerta();
        }, 2000)
    }
}

function escondeAlerta() {
    $('#mainAlert').removeAttr('class');
    $('#mainAlert').addClass('alert fade hide')
    $('#mainAlert').alert();
}

function listaMoedas () {
    requestFactory(
        'moedas/listagem',
        'GET',
        null,
        (response) => {
            // console.info('EXECUTED CALLBACK');
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

function requestFactory (_url, _method, _params, _callback) {
    console.info(`${window.ENV.BASE_URL}${_url}`);
    mostraAlerta('Carregando...', 'alert-info', false);

    $.ajax({
        url: `${ENV.BASE_URL}${_url}`,
        contentType: "application/json",
        method: _method,
        data: JSON.stringify(_params)
    })
    .then(response => {
        if (_callback)
            _callback(response);
    })
    .always(() => {
        escondeAlerta();
        // console.info('ENDED');
    });
}

function getENV () {
    if (window.location.href.indexOf('localhost') > -1) {
        return {
            "BASE_URL": "http://localhost:58204/"
        }
    } else {
        return {
            "BASE_URL": "https://neontestconversaomonetaria.azurewebsites.net/"
        }
    }
}